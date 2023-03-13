using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Form17.Models;
using System.Linq.Dynamic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using ClosedXML.Excel;
using System.IO;
using PagedList;
using System.Reflection;
using OfficeOpenXml;
using Form17.Webservice_SMS;

namespace Form17.Controllers
{
    public class DivisionController : Controller
    {
        Form17Entities db = new Form17Entities();
        DataAccessLayer dataAccessLayer = new DataAccessLayer();
        WebService_sms sms = new WebService_sms();

        // GET: Division
        public ActionResult Login()
        {
            ViewBag.IsActive = db.Tbl_Site_Status.Select(x => x.Active_Status).FirstOrDefault();
            return View();
        }
        [HttpPost]
        public JsonResult Login(Tbl_Login_Credentials tbl_Login_Credentials)
        {
            try
            {
                if (tbl_Login_Credentials.ID == 0)
                {
                    return Json(new { Result = false, Message = "Please Select Division." }, JsonRequestBehavior.AllowGet);
                }
                if (tbl_Login_Credentials.Password == null)
                {
                    return Json(new { Result = false, Message = "Please Enter Credentials." }, JsonRequestBehavior.AllowGet);
                }
                var IsMatched = db.Tbl_Login_Credentials.Where(x => x.ID == tbl_Login_Credentials.ID && x.Password == tbl_Login_Credentials.Password).FirstOrDefault();
                if (IsMatched != null)
                {
                    Session["Division"] = IsMatched.Division_Name;
                    Session["Username"] = IsMatched.Division_Name;
                    FormsAuthentication.SetAuthCookie(tbl_Login_Credentials.ID.ToString(), false);
                    return Json(new { Result = true, Message = "../Division/DivisionInfo" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = false, Message = "Invalid Login Credentials." }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = false, Message = "Something went wrong!" }, JsonRequestBehavior.AllowGet);
                throw ex;
            }
        }
        public JsonResult getECDetails(string std, string formno)
        {
            try
            {
                if (std == null || std == "0" || formno == null || formno.Trim().Length != 7)
                {
                    return Json(new { Result = false, Message = "Please provide valid details" }, JsonRequestBehavior.AllowGet);
                }
                else if (std == "HSC")
                {
                    var record = db.Tbl_HSC_Form17_Final.Where(x => x.S_ID == formno.Trim().ToUpper() && x.Division == this.User.Identity.Name).Select(a => new
                    {
                        a.First_Name,
                        a.Father_Husband_Name,
                        a.Last_Name,
                        a.EC_Code,
                        a.EC_Status
                    }).FirstOrDefault();
                    if (record == null)
                    {
                        return Json(new { Result = false, Message = "No record found" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { Result = true, data = record });
                    }
                }
                else if (std == "SSC")
                {
                    var record = db.Tbl_SSC_Form17_Final.Where(x => x.S_ID == formno.Trim().ToUpper() && x.Division == this.User.Identity.Name).Select(a => new
                    {
                        a.First_Name,
                        a.Father_Husband_Name,
                        a.Last_Name,
                        a.EC_Code,
                        a.EC_Status
                    }).FirstOrDefault();
                    if (record == null)
                    {
                        return Json(new { Result = false, Message = "No record found" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { Result = true, data = record }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new { Result = false, Message = "No record found" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = false, Message = "Something went wrong !!" }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult ResendOTP()
        {
            try
            {
                OTPsend();
                return Json(new { Result = true, Message = "OTP is sent to divisional board's secretary's mobile !" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = false, Message = "Something went wrong !" }, JsonRequestBehavior.AllowGet);
            }
        }


        public void OTPsend()
        {
            List<Tbl_OTP> OTPList = db.Database.SqlQuery<Tbl_OTP>("select * from Tbl_OTP where Convert(date, Date) =  cast(getDate() as Date) and Division = '" + this.User.Identity.Name + "'").ToList();
            if (OTPList.Count != 0)
            {
                string mobilenumber = "";
                List<Tbl_Division_Details> tbl_Division_Details = db.Tbl_Division_Details.ToList();
                mobilenumber = tbl_Division_Details[tbl_Division_Details.FindIndex(a => a.Division_Code.Contains(this.User.Identity.Name))].Mobile_No;
                sms.unicodesms("" + mobilenumber + "", "Please note one Time Password for Enrollment_Certificate is " + OTPList[0].OTP + " STATE BOARD PUNE", "1007338141306915280");
            }
            else
            {
                Random r = new Random();
                string mobilenumber = "";
                int genRand = r.Next(1000, 9999);
                List<Tbl_Division_Details> tbl_Division_Details = db.Tbl_Division_Details.ToList();
                mobilenumber = tbl_Division_Details[tbl_Division_Details.FindIndex(a => a.Division_Code.Contains(this.User.Identity.Name))].Mobile_No;
                sms.unicodesms("" + mobilenumber + "", "Please note one Time Password for Enrollment_Certificate is " + genRand + " STATE BOARD PUNE", "1007338141306915280");
                Tbl_OTP tbl_OTP = new Tbl_OTP();
                tbl_OTP.OTP = genRand.ToString();
                tbl_OTP.Division = Int32.Parse(this.User.Identity.Name);
                tbl_OTP.Date = DateTime.Now;
                db.Tbl_OTP.Add(tbl_OTP);
                db.SaveChanges();
            }
            
        }
        public ActionResult DivisionInfo()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        public ActionResult AllocateEC()
        {
            List<Tbl_OTP> OTPList = db.Database.SqlQuery<Tbl_OTP>("select * from Tbl_OTP where Convert(date, Date) =  cast(getDate() as Date) and Division = '" + this.User.Identity.Name + "'").ToList();
            if(OTPList.Count != 0)
            {
                return View();
            }
            OTPsend();
            return View();
        }

        public JsonResult VerifyOTPAndAllocateEC(string otp, string std, string formno)
        {
            try
            {
                if (otp == null || std == null || formno == null || otp.Trim() == "" || std.Trim() == "0" || formno.Trim().Length != 7)
                {
                    return Json(new { Result = false, Message = "Please provide Form details"}, JsonRequestBehavior.AllowGet);
                }                
                List<Tbl_OTP> OTPList = db.Database.SqlQuery<Tbl_OTP>("select * from Tbl_OTP where Convert(date, Date) =  cast(getDate() as Date) and Division = '"+this.User.Identity.Name+"'").ToList();
                
                if (OTPList != null && OTPList[0].OTP == otp)
                {
                    if (std == "SSC")
                    {
                        Tbl_SSC_Form17_Final tbl_SSC_Form17_Final = new Tbl_SSC_Form17_Final();
                        tbl_SSC_Form17_Final = db.Tbl_SSC_Form17_Final.Where(x => x.S_ID == formno.Trim().ToUpper()).FirstOrDefault();
                        tbl_SSC_Form17_Final.EC_Code = "220" + tbl_SSC_Form17_Final.Division + "" + (100000 + tbl_SSC_Form17_Final.ID);
                        tbl_SSC_Form17_Final.EC_Status = "1";
                        tbl_SSC_Form17_Final.EC_Date = DateTime.Now.ToString("dd/MM/yyyy");
                        db.Tbl_SSC_Form17_Final.Attach(tbl_SSC_Form17_Final);
                        db.Entry(tbl_SSC_Form17_Final).Property(x => x.EC_Code).IsModified = true;
                        db.Entry(tbl_SSC_Form17_Final).Property(x => x.EC_Date).IsModified = true;
                        db.Entry(tbl_SSC_Form17_Final).Property(x => x.EC_Status).IsModified = true;
                        db.SaveChanges();
                        return Json(new { Result = true, Message = "EC generated successfully for " + formno, RedirectURL = "../Division/AllocateEC" }, JsonRequestBehavior.AllowGet);
                    }
                    if (std == "HSC")
                    {
                        Tbl_HSC_Form17_Final tbl_HSC_Form17_Final = new Tbl_HSC_Form17_Final();
                        tbl_HSC_Form17_Final = db.Tbl_HSC_Form17_Final.Where(x => x.S_ID == formno.Trim().ToUpper()).FirstOrDefault();
                        tbl_HSC_Form17_Final.EC_Code = "220" + tbl_HSC_Form17_Final.Division + "" + (200000 + tbl_HSC_Form17_Final.ID);
                        tbl_HSC_Form17_Final.EC_Status = "1";
                        tbl_HSC_Form17_Final.EC_Date = DateTime.Now.ToString("dd/MM/yyyy");
                        db.Tbl_HSC_Form17_Final.Attach(tbl_HSC_Form17_Final);
                        db.Entry(tbl_HSC_Form17_Final).Property(x => x.EC_Code).IsModified = true;
                        db.Entry(tbl_HSC_Form17_Final).Property(x => x.EC_Date).IsModified = true;
                        db.Entry(tbl_HSC_Form17_Final).Property(x => x.EC_Status).IsModified = true;
                        db.SaveChanges();
                        return Json(new { Result = true, Message = "EC generated successfully for " + formno, RedirectURL = "../Division/AllocateEC" }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new { Result = false, Message = "OTP not matched !"}, JsonRequestBehavior.AllowGet);
                }
                return Json(new { Result = false, Message = "OTP not matched !!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = false, Message = "Something went wrong !" }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult IsAlreadySchoolUpdatedByDivision(string application)
        {
            UpdateSchoolByDivisionModel model = new UpdateSchoolByDivisionModel();
            if (application == null || application.Trim() == "" || application.Trim().Length != 7 || !db.Tbl_SSC_Form17_Final.Any(x => x.S_ID == application.Trim().ToUpper()))
            {
                return Json(new { Result = false, Message = "Invalid Application No" }, JsonRequestBehavior.AllowGet);
            }
            if (db.Tbl_Update_Old_College.Any(a => a.Form_No == application.Trim().ToUpper()))
            {
                return Json(new { Result = false, Message = "Student details are already updated." }, JsonRequestBehavior.AllowGet);
            }
            var studdetails = db.Tbl_SSC_Form17_Final.Where(x => x.S_ID == application.Trim().ToUpper()).FirstOrDefault();
            model.S_ID = studdetails.S_ID; model.Ref_ID = studdetails.Ref_ID; model.Last_Name = studdetails.Last_Name; model.First_Name = studdetails.First_Name; model.Father_Husband_Name = studdetails.Father_Husband_Name; model.District_of_Form_Submission = studdetails.District_of_Form_Submission; model.Taluka_of_Form_Submission = studdetails.Taluka_of_Form_Submission; model.Medium_of_Form = studdetails.Medium_of_Form; model.School_of_Form_Submission = studdetails.School_of_Form_Submission; model.Index_No_of_School = studdetails.Index_No_of_School;

            return Json(new { Result = true, data = model }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SchoolUpdateByDivision(UpdateSchoolByDivisionModel updateSchoolByDivisionModel)
        {
            try
            {
                if (db.Tbl_Update_Old_College.Any(x => x.Form_No == updateSchoolByDivisionModel.Application_No.Trim().ToUpper()))
                {
                    return Json(new { Result = false, Message = "Student details are already updated." }, JsonRequestBehavior.AllowGet);
                }
                var divcode = db.Tbl_Code_Master.Where(x => x.district_code == updateSchoolByDivisionModel.Index_No_of_School.Substring(0, 2)).FirstOrDefault().division_code;
                if (this.User.Identity.Name != divcode)
                {
                    return Json(new { Result = false, Message = "Student's new center should be within the divisional board's district" });
                }

                var oldrecord = db.Tbl_SSC_Form17_Final.Where(x => x.S_ID == updateSchoolByDivisionModel.Application_No.Trim().ToUpper()).FirstOrDefault();
                Tbl_Update_Old_College updatecollege = new Tbl_Update_Old_College();
                updatecollege.Form_No = updateSchoolByDivisionModel.Application_No.Trim().ToUpper();
                updatecollege.Old_College = oldrecord.School_of_Form_Submission;
                updatecollege.Updated_College = updateSchoolByDivisionModel.School_of_Form_Submission;
                updatecollege.Email_ID = oldrecord.Email_ID.Trim();
                db.Tbl_Update_Old_College.Add(updatecollege);
                db.SaveChanges();

                Tbl_SSC_Form17_Final tbl_SSC_Form17_Final = new Tbl_SSC_Form17_Final();
                tbl_SSC_Form17_Final = db.Tbl_SSC_Form17_Final.Where(x => x.S_ID == updateSchoolByDivisionModel.Application_No.Trim().ToUpper()).FirstOrDefault();
                tbl_SSC_Form17_Final.First_Name = updateSchoolByDivisionModel.First_Name.Trim();
                tbl_SSC_Form17_Final.Father_Husband_Name = updateSchoolByDivisionModel.Father_Husband_Name.Trim();
                tbl_SSC_Form17_Final.Last_Name = updateSchoolByDivisionModel.Last_Name.Trim();
                tbl_SSC_Form17_Final.District_of_Form_Submission = updateSchoolByDivisionModel.District_of_Form_Submission.Trim();
                tbl_SSC_Form17_Final.Taluka_of_Form_Submission = updateSchoolByDivisionModel.Taluka_of_Form_Submission.Trim();
                tbl_SSC_Form17_Final.Medium_of_Form = updateSchoolByDivisionModel.Medium_of_Form.Trim();
                tbl_SSC_Form17_Final.School_of_Form_Submission = updateSchoolByDivisionModel.School_of_Form_Submission.Trim();
                tbl_SSC_Form17_Final.Index_No_of_School = updateSchoolByDivisionModel.Index_No_of_School.Trim();

                db.Tbl_SSC_Form17_Final.Attach(tbl_SSC_Form17_Final);
                db.Entry(tbl_SSC_Form17_Final).Property(x => x.First_Name).IsModified = true;
                db.Entry(tbl_SSC_Form17_Final).Property(x => x.Father_Husband_Name).IsModified = true;
                db.Entry(tbl_SSC_Form17_Final).Property(x => x.Last_Name).IsModified = true;
                db.Entry(tbl_SSC_Form17_Final).Property(x => x.District_of_Form_Submission).IsModified = true;
                db.Entry(tbl_SSC_Form17_Final).Property(x => x.Taluka_of_Form_Submission).IsModified = true;
                db.Entry(tbl_SSC_Form17_Final).Property(x => x.Medium_of_Form).IsModified = true;
                db.Entry(tbl_SSC_Form17_Final).Property(x => x.School_of_Form_Submission).IsModified = true;
                db.Entry(tbl_SSC_Form17_Final).Property(x => x.Index_No_of_School).IsModified = true;
                db.SaveChanges();
                return Json(new { Result = true, Message = "Center and other details are updated successfully !" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = false, Message = "Something went wrong." }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult IsAlreadyCollegeUpdatedByDivision(string application)
        {
            UpdateCollegeByDivisionModel model = new UpdateCollegeByDivisionModel();
            if (application == null || application.Trim() == "" || application.Trim().Length != 7 || !db.Tbl_HSC_Form17_Final.Any(x => x.S_ID == application.Trim().ToUpper()))
            {
                return Json(new { Result = false, Message = "Invalid Application No" }, JsonRequestBehavior.AllowGet);
            }
            if (db.Tbl_Update_Old_College.Any(a => a.Form_No == application.Trim().ToUpper()))
            {
                return Json(new { Result = false, Message = "Student details are already updated." }, JsonRequestBehavior.AllowGet);
            }
            var studdetails = db.Tbl_HSC_Form17_Final.Where(x => x.S_ID == application.Trim().ToUpper()).FirstOrDefault();
            model.S_ID = studdetails.S_ID; model.Ref_ID = studdetails.Ref_ID; model.Last_Name = studdetails.Last_Name; model.First_Name = studdetails.First_Name; model.Father_Husband_Name = studdetails.Father_Husband_Name; model.District_of_Form_Submission = studdetails.District_of_Form_Submission; model.Taluka_of_Form_Submission = studdetails.Taluka_of_Form_Submission; model.Stream_of_Form = studdetails.Stream_of_Form; model.Medium_of_Form = studdetails.Medium_of_Form; model.College_of_Form_Submission = studdetails.College_of_Form_Submission; model.College_Index_No = studdetails.College_Index_No;

            return Json(new { Result = true, data = model }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CollegeUpdateByDivision(UpdateCollegeByDivisionModel updateCollegeByDivisionModel)
        {
            try
            {
                if (db.Tbl_Update_Old_College.Any(x => x.Form_No == updateCollegeByDivisionModel.Application_No.Trim().ToUpper()))
                {
                    return Json(new { Result = false, Message = "Student details are already updated." }, JsonRequestBehavior.AllowGet);
                }
                var divcode = db.Tbl_Code_Master.Where(x => x.district_code == updateCollegeByDivisionModel.College_Index_No.Substring(0, 2)).FirstOrDefault().division_code;
                if (this.User.Identity.Name != divcode)
                {
                    return Json(new { Result = false, Message = "Student's new center should be within the divisonal board's district" });
                }
                var oldrecord = db.Tbl_HSC_Form17_Final.Where(x => x.S_ID == updateCollegeByDivisionModel.Application_No.Trim().ToUpper()).FirstOrDefault();
                Tbl_Update_Old_College updatecollege = new Tbl_Update_Old_College();
                updatecollege.Form_No = updateCollegeByDivisionModel.Application_No.Trim().ToUpper();
                updatecollege.Old_College = oldrecord.College_of_Form_Submission;
                updatecollege.Updated_College = updateCollegeByDivisionModel.College_of_Form_Submission;
                updatecollege.Email_ID = oldrecord.Email_ID.Trim();
                db.Tbl_Update_Old_College.Add(updatecollege);
                db.SaveChanges();

                Tbl_HSC_Form17_Final tbl_HSC_Form17_Final = new Tbl_HSC_Form17_Final();
                tbl_HSC_Form17_Final = db.Tbl_HSC_Form17_Final.Where(x => x.S_ID == updateCollegeByDivisionModel.Application_No.Trim().ToUpper()).FirstOrDefault();
                tbl_HSC_Form17_Final.First_Name = updateCollegeByDivisionModel.First_Name.Trim();
                tbl_HSC_Form17_Final.Father_Husband_Name = updateCollegeByDivisionModel.Father_Husband_Name.Trim();
                tbl_HSC_Form17_Final.Last_Name = updateCollegeByDivisionModel.Last_Name.Trim();
                tbl_HSC_Form17_Final.District_of_Form_Submission = updateCollegeByDivisionModel.District_of_Form_Submission.Trim();
                tbl_HSC_Form17_Final.Taluka_of_Form_Submission = updateCollegeByDivisionModel.Taluka_of_Form_Submission.Trim();
                tbl_HSC_Form17_Final.Stream_of_Form = updateCollegeByDivisionModel.Stream_of_Form.Trim();
                tbl_HSC_Form17_Final.Medium_of_Form = updateCollegeByDivisionModel.Medium_of_Form.Trim();
                tbl_HSC_Form17_Final.College_of_Form_Submission = updateCollegeByDivisionModel.College_of_Form_Submission.Trim();
                tbl_HSC_Form17_Final.College_Index_No = updateCollegeByDivisionModel.College_Index_No.Trim();

                db.Tbl_HSC_Form17_Final.Attach(tbl_HSC_Form17_Final);
                db.Entry(tbl_HSC_Form17_Final).Property(x => x.First_Name).IsModified = true;
                db.Entry(tbl_HSC_Form17_Final).Property(x => x.Father_Husband_Name).IsModified = true;
                db.Entry(tbl_HSC_Form17_Final).Property(x => x.Last_Name).IsModified = true;
                db.Entry(tbl_HSC_Form17_Final).Property(x => x.District_of_Form_Submission).IsModified = true;
                db.Entry(tbl_HSC_Form17_Final).Property(x => x.Taluka_of_Form_Submission).IsModified = true;
                db.Entry(tbl_HSC_Form17_Final).Property(x => x.Stream_of_Form).IsModified = true;
                db.Entry(tbl_HSC_Form17_Final).Property(x => x.Medium_of_Form).IsModified = true;
                db.Entry(tbl_HSC_Form17_Final).Property(x => x.College_of_Form_Submission).IsModified = true;
                db.Entry(tbl_HSC_Form17_Final).Property(x => x.College_Index_No).IsModified = true;


                db.SaveChanges();
                return Json(new { Result = true, Message = "College and other details are updated successfully !" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = false, Message = "Something went wrong." }, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult EditSSCDetails()
        {
            GetDistrictList();
            return View();
        }

        public ActionResult EditHSCDetails()
        {
            GetDistrictList();
            return View();
        }
        public ActionResult ExportToHSCExcel()
        {
            var divcode = this.User.Identity.Name;
            List<Tbl_HSC_Form17_Final> hscstudentlist = new List<Tbl_HSC_Form17_Final>();
            hscstudentlist = db.Tbl_HSC_Form17_Final.Where(x => x.Division == divcode).ToList();
            DataTable dt = ToDataTable(hscstudentlist);
            DataSet ds = new DataSet();
            ds.Tables.Add(dt);

            ds.Tables[0].TableName = "HSC";
            string fileName = DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
            CreateExcelFile(ds, fileName);
            string fullPath = Path.Combine(Server.MapPath("~/division"), fileName + ".xlsx");
            byte[] fileByteArray = System.IO.File.ReadAllBytes(fullPath);
            System.IO.File.Delete(fullPath);
            return File(fileByteArray, "application/vnd.ms-excel", fileName + ".xlsx");
            // return View();
        }
        void CreateExcelFile(DataSet ds1, string filename)
        {
            try
            {
                using (DataSet ds = ds1)
                {
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        string rootFolder = Server.MapPath("").ToString();
                        string fileName = @"" + filename + ".xlsx";

                        System.IO.FileInfo file = new System.IO.FileInfo(Path.Combine(rootFolder, fileName));
                        using (ExcelPackage xp = new ExcelPackage(file))
                        {
                            foreach (DataTable dt in ds.Tables)
                            {
                                ExcelWorksheet ws = xp.Workbook.Worksheets.Add(dt.TableName);
                                int rowstart = 1;
                                int colstart = 1;
                                int rowend = rowstart;
                                int colend = colstart + dt.Columns.Count;
                                rowend = rowstart + dt.Rows.Count;
                                ws.Cells[rowstart, colstart].LoadFromDataTable(dt, true);
                                int i = 1;
                                foreach (DataColumn dc in dt.Columns)
                                {
                                    i++;
                                    if (dc.DataType == typeof(decimal))
                                        ws.Column(i).Style.Numberformat.Format = "#0.00";
                                }
                                ws.Cells[ws.Dimension.Address].AutoFitColumns();
                                ws.Cells[rowstart, colstart, rowend, colend].Style.Border.Top.Style =
                                   ws.Cells[rowstart, colstart, rowend, colend].Style.Border.Bottom.Style =
                                   ws.Cells[rowstart, colstart, rowend, colend].Style.Border.Left.Style =
                                   ws.Cells[rowstart, colstart, rowend, colend].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                            }
                            xp.Save();
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        public ActionResult ExportToSSCExcel()
        {
            var divcode = this.User.Identity.Name;
            List<Tbl_SSC_Form17_Final> sscstudentlist = new List<Tbl_SSC_Form17_Final>();
            sscstudentlist = db.Tbl_SSC_Form17_Final.Where(x => x.Division == divcode).ToList();
            DataTable dt = ToDataTable(sscstudentlist);
            DataSet ds = new DataSet();
            ds.Tables.Add(dt);

            ds.Tables[0].TableName = "SSC";
            string fileName = DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
            CreateExcelFile(ds, fileName);
            string fullPath = Path.Combine(Server.MapPath("~/division"), fileName + ".xlsx");
            byte[] fileByteArray = System.IO.File.ReadAllBytes(fullPath);
            System.IO.File.Delete(fullPath);
            return File(fileByteArray, "application/vnd.ms-excel", fileName + ".xlsx");
        }
        public ActionResult SSCDivisionInfo(string Sorting_Order, string Search_By, string Search_Data, string Filter_Value, int? Page_No)
        {
            if (Search_Data != null)
            {
                Page_No = 1;
            }
            else
            {
                Search_Data = Filter_Value;
            }

            ViewBag.FilterValue = Search_Data;
            var divcode = this.User.Identity.Name;
            List<Tbl_SSC_Form17_Final> sscstudentlist = new List<Tbl_SSC_Form17_Final>();
            List<Tbl_SSC_Form17_Final> ssclist = new List<Tbl_SSC_Form17_Final>();
            switch (Search_By)
            {
                case "application":
                    sscstudentlist = db.Tbl_SSC_Form17_Final.Where(x => x.Division == divcode && (x.S_ID.ToUpper().Contains(Search_Data.Trim().ToUpper()))).ToList();
                    ssclist.AddRange(sscstudentlist);
                    break;
                case "name":
                    sscstudentlist = db.Tbl_SSC_Form17_Final.Where(x => x.Division == divcode && (x.First_Name.ToUpper().Contains(Search_Data.Trim().ToUpper()) || x.Last_Name.ToUpper().Contains(Search_Data.Trim().ToUpper()) || x.Father_Husband_Name.Trim().Contains(Search_Data.Trim().ToUpper()))).ToList();
                    ssclist.AddRange(sscstudentlist);
                    break;
                case "email":
                    sscstudentlist = db.Tbl_SSC_Form17_Final.Where(x => x.Division == divcode && (x.Email_ID.ToUpper().Contains(Search_Data.Trim().ToUpper()))).ToList();
                    ssclist.AddRange(sscstudentlist);
                    break;
                case "mobile":
                    sscstudentlist = db.Tbl_SSC_Form17_Final.Where(x => x.Division == divcode && (x.Mobile_No.ToUpper().Contains(Search_Data.Trim().ToUpper()))).ToList();
                    ssclist.AddRange(sscstudentlist);
                    break;
                case "index":
                    sscstudentlist = db.Tbl_SSC_Form17_Final.Where(x => x.Division == divcode && (x.Email_ID.ToUpper().Contains(Search_Data.Trim().ToUpper()))).ToList();
                    ssclist.AddRange(sscstudentlist);
                    break;
                default:
                    sscstudentlist = db.Tbl_SSC_Form17_Final.Where(x => x.Division == divcode).ToList();
                    ssclist.AddRange(sscstudentlist);
                    break;
            }
            int Size_Of_Page = 10;
            int No_Of_Page = (Page_No ?? 1);
            return View(ssclist.ToPagedList(No_Of_Page, Size_Of_Page));
        }
        [HandleError]
        public ActionResult ExportToSSCNotepad()
        {
            List<Tbl_SSC_Form17_Final> divisionwisedatamodel = new List<Tbl_SSC_Form17_Final>();
            divisionwisedatamodel = (from p in db.Tbl_SSC_Form17_Final
                                     where p.Division == this.User.Identity.Name.ToString() && p.EC_Status != null
                                     orderby p.School_of_Form_Submission
                                     select p).ToList();

            string path = Server.MapPath("../AppFiles/Divisionwise_Notepad/Division" + this.User.Identity.Name + "SSC.txt");
            using (StreamWriter sw = System.IO.File.CreateText(path))
            {
                int i = 1;
                int pageno = 1;
                int flag = 0;
                int init = 0;
                string oldindex = divisionwisedatamodel[0].School_of_Form_Submission;
                foreach (var item in divisionwisedatamodel)
                {
                    if (flag % 11 == 0 || oldindex != item.School_of_Form_Submission)
                    {
                        if (oldindex != item.School_of_Form_Submission)
                        {
                            i = 1;
                            flag = 0;
                        }
                        if(init == 0)
                        {
                            sw.WriteLine(SSCCommonHeading(item, pageno++));
                        }
                        else
                        {
                            sw.WriteLine(CommonFooter());
                            sw.WriteLine(SSCCommonHeading(item, pageno++));
                        }                                         
                    }                   
                    sw.Write(Add_Space((i++).ToString(), 3));
                    sw.Write(Add_Space(item.S_ID, 8));
                    sw.Write(Add_Space(item.First_Name + " " + item.Father_Husband_Name + " " + item.Last_Name, 68));
                    sw.Write(Add_Space(item.Gender, 8));
                    sw.Write(Add_Space(item.DOB, 13));
                    sw.Write(Add_Space(item.EC_Status, 5));
                    sw.Write(Add_Space(item.EC_Code, 11));
                    sw.Write(Add_Space(item.Mobile_No, 9));
                    sw.WriteLine();
                    sw.Write(Add_Space("", 12));
                    sw.Write(Add_Space(item.Residential_Address, 68));
                    sw.Write(Add_Space(item.Medium_of_Form, 8));
                    sw.Write(Add_Space(item.Date_of_Leaving_Secondary_School, 13));
                    sw.WriteLine();
                    sw.Write(Add_Space("", 12));
                    sw.Write(Add_Space("Dist : " + GetDistrictName(item.District), 68));
                    sw.WriteLine();
                    sw.Write(Add_Space("", 12));
                    sw.Write(Add_Space("State : " + item.State, 68));
                    sw.WriteLine("\n");
                    oldindex = item.School_of_Form_Submission;
                    flag++;
                    init++;
                }
            }
            return File(path, "text/plain", "Division"+this.User.Identity.Name+"SSC.txt");
        }

        [HandleError]
        public ActionResult ExportToHSCNotepad()
        {
            List<Tbl_HSC_Form17_Final> divisionwisedatamodel = new List<Tbl_HSC_Form17_Final>();
            divisionwisedatamodel = (from p in db.Tbl_HSC_Form17_Final
                                     where p.Division == this.User.Identity.Name.ToString() && p.EC_Status != null
                                     orderby p.College_of_Form_Submission
                                     select p).ToList();

            string path = Server.MapPath("../AppFiles/Divisionwise_Notepad/Division" + this.User.Identity.Name + "HSC.txt");
            using (StreamWriter sw = System.IO.File.CreateText(path))
            {
                int i = 1;
                int pageno = 1;
                int flag = 0;
                int init = 0;
                string oldindex = divisionwisedatamodel[0].College_of_Form_Submission;
                foreach (var item in divisionwisedatamodel)
                {
                    if (flag % 11 == 0 || oldindex != item.College_of_Form_Submission)
                    {
                        if (oldindex != item.College_of_Form_Submission)
                        {
                            i = 1;
                            flag = 0;
                        }
                        if (init == 0)
                        {
                            sw.WriteLine(HSCCommonHeading(item, pageno++));
                        }
                        else
                        {
                            sw.WriteLine(CommonFooter());
                            sw.WriteLine(HSCCommonHeading(item, pageno++));
                        }
                    }
                    sw.Write(Add_Space((i++).ToString(), 3));
                    sw.Write(Add_Space(item.S_ID, 8));
                    sw.Write(Add_Space(item.First_Name + " " + item.Father_Husband_Name + " " + item.Last_Name, 68));
                    sw.Write(Add_Space(item.Gender, 8));
                    sw.Write(Add_Space(item.DOB, 13));
                    sw.Write(Add_Space(item.EC_Status, 5));
                    sw.Write(Add_Space(item.EC_Code, 11));
                    sw.Write(Add_Space(item.Mobile_No, 9));
                    sw.WriteLine();
                    sw.Write(Add_Space("", 12));
                    sw.Write(Add_Space(item.Residential_Address, 68));
                    sw.Write(Add_Space(item.Medium_of_Form, 8));
                    sw.Write(Add_Space(item.Date_of_Leaving_Secondary_School, 13));
                    sw.WriteLine();
                    sw.Write(Add_Space("", 12));
                    sw.Write(Add_Space("Dist : " + GetDistrictName(item.District), 68));
                    sw.WriteLine();
                    sw.Write(Add_Space("", 12));
                    sw.Write(Add_Space("State : " + item.State, 68));
                    sw.WriteLine("\n");
                    oldindex = item.College_of_Form_Submission;
                    flag++;
                    init++;
                }
            }
            return File(path, "text/plain", "Division" + this.User.Identity.Name + "HSC.txt");
        }
        public string GetDistrictName(string distcode)
        {
            string str = "";
            switch (distcode)
            {
                case "01": str = "AKOLA"; break;
                case "02": str = "AMRAVATI"; break;
                case "03": str = "BHANDARA"; break;
                case "04": str = "BULDHANA"; break;
                case "05": str = "CHANDRAPUR"; break;
                case "06": str = "NAGPUR"; break;
                case "07": str = "WARDHA"; break;
                case "08": str = "YEOTMAL"; break;
                case "09": str = "GADCHIROLI"; break;
                case "10": str = "WASHIM"; break;
                case "11": str = "PUNE"; break;
                case "12": str = "AHAMADNAGAR"; break;
                case "13": str = "NASIK"; break;
                case "14": str = "DHULE"; break;
                case "15": str = "JALGAON"; break;
                case "16": str = "THANE"; break;
                case "17": str = "RAIGAD"; break;
                case "18": str = "PALGHAR"; break;
                case "19": str = "NANDURBAR"; break;
                case "21": str = "SATARA"; break;
                case "22": str = "SANGALI"; break;
                case "23": str = "KOLHAPUR"; break;
                case "24": str = "SOLAPUR"; break;
                case "25": str = "RATANAGIRI"; break;
                case "26": str = "SINDHUDURG"; break;
                case "29": str = "GONDIA"; break;
                case "31": str = "GR. MUMBAI"; break;
                case "32": str = "MUMBAI(SUB1)"; break;
                case "33": str = "MUMBAI(SUB2)"; break;
                case "56": str = "AURANGABAD"; break;
                case "57": str = "BEED"; break;
                case "58": str = "NANDED"; break;
                case "59": str = "OSMANABAD"; break;
                case "60": str = "PARBHANI"; break;
                case "61": str = "JALNA"; break;
                case "62": str = "LATUR"; break;
                case "66": str = "HINGOLI"; break;
                default: str = ""; break;
            }
            return str;
        }
        public string GetDivName()
        {
            string division = "";
            switch (this.User.Identity.Name)
            {
                case "1": division = "PUNE"; break;
                case "2": division = "NAGPUR"; break;
                case "3": division = "AURANGABAD"; break;
                case "4": division = "MUMBAI"; break;
                case "5": division = "KOLHAPUR"; break;
                case "6": division = "AMRAVATI"; break;
                case "7": division = "NASHIK"; break;
                case "8": division = "LATUR"; break;
                case "9": division = "KOKAN"; break;          
            }
            return division;
        }
        public string Add_Space(string value, int value_length)
        {
            int count = value.Length;
            string temp_space = "";
            for (int i = count; i <= value_length; i++)
            {
                if (value.Contains("\r\n"))
                {
                    //string[] address = value.Split("\r\n");
                }
                temp_space = temp_space + " ";
            }
            return value + temp_space;
        }
        public string SSCCommonHeading(Tbl_SSC_Form17_Final divisionwisedatamodel, int page)
        {
            string commantext = "\n             Maharashtra State Board of Secondary and Higher Secondary Education, PUNE Divisional BOARD, Pune -5\n                                       PRIVATE CANDIDATE REGISTER FOR ENROLLMENT CERTIFICATE\n                     PAGE :           "+page+"\n                        DIVISION :    "+GetDivName()+"            EXAM : SSC          SESSION :FEB/MARCH             YEAR : 2022\n School INDEX No S"+divisionwisedatamodel.School_of_Form_Submission.Substring(0,2)+"."+divisionwisedatamodel.School_of_Form_Submission.Substring(2,2)+"."+divisionwisedatamodel.School_of_Form_Submission.Substring(4,3)+"\n--------------------------------------------------------------------------------------------------------------------------------------\nSRNO  CASE   NAME & ADDRESS                                                       SEX     BIRTH DATE     EC      EC        Mobile No\n      NO                                                                          MEDIUM  SCH LIV DATE   Status   NO                 \n--------------------------------------------------------------------------------------------------------------------------------------";
            return commantext;
        }
        public string HSCCommonHeading(Tbl_HSC_Form17_Final divisionwisedatamodel, int page)
        {
            string commantext = "\n             Maharashtra State Board of Secondary and Higher Secondary Education, PUNE Divisional BOARD, Pune -5\n                                       PRIVATE CANDIDATE REGISTER FOR ENROLLMENT CERTIFICATE\n                     PAGE :           " + page + "\n                        DIVISION :    " + GetDivName() + "            EXAM : HSC          SESSION :FEB/MARCH             YEAR : 2022\n College INDEX No J" + divisionwisedatamodel.College_of_Form_Submission.Substring(0, 2) + "." + divisionwisedatamodel.College_of_Form_Submission.Substring(2, 2) + "." + divisionwisedatamodel.College_of_Form_Submission.Substring(4, 3) + "\n--------------------------------------------------------------------------------------------------------------------------------------\nSRNO  CASE   NAME & ADDRESS                                                       SEX     BIRTH DATE     EC      EC        Mobile No\n      NO                                                                          MEDIUM  SCH LIV DATE   Status   NO                 \n--------------------------------------------------------------------------------------------------------------------------------------";
            return commantext;
        }
        public string CommonFooter()
        {
            string commantext = "--------------------------------------------------------------------------------------------------------------------------------------\n";
            return commantext;
        }
        public ActionResult HSCDivisionInfo(string Sorting_Order, string Search_By, string Search_Data, string Filter_Value, int? Page_No, string Standard)
        {
            if (Search_Data != null)
            {
                Page_No = 1;
            }
            else
            {
                Search_Data = Filter_Value;
            }

            ViewBag.FilterValue = Search_Data;
            var divcode = this.User.Identity.Name;
            List<Tbl_HSC_Form17_Final> hscstudentlist = new List<Tbl_HSC_Form17_Final>();
            List<Tbl_HSC_Form17_Final> hsclist = new List<Tbl_HSC_Form17_Final>();
            switch (Search_By)
            {
                case "application":
                    hscstudentlist = db.Tbl_HSC_Form17_Final.Where(x => x.Division == divcode && (x.S_ID.ToUpper().Contains(Search_Data.Trim().ToUpper()))).ToList();
                    hsclist.AddRange(hscstudentlist);
                    break;
                case "name":
                    hscstudentlist = db.Tbl_HSC_Form17_Final.Where(x => x.Division == divcode && (x.First_Name.ToUpper().Contains(Search_Data.Trim().ToUpper()) || x.Last_Name.ToUpper().Contains(Search_Data.Trim().ToUpper()) || x.Father_Husband_Name.Trim().Contains(Search_Data.Trim().ToUpper()))).ToList();
                    hsclist.AddRange(hscstudentlist);
                    break;
                case "email":
                    hscstudentlist = db.Tbl_HSC_Form17_Final.Where(x => x.Division == divcode && (x.Email_ID.ToUpper().Contains(Search_Data.Trim().ToUpper()))).ToList();
                    hsclist.AddRange(hscstudentlist);
                    break;
                case "mobile":
                    hscstudentlist = db.Tbl_HSC_Form17_Final.Where(x => x.Division == divcode && (x.Mobile_No.ToUpper().Contains(Search_Data.Trim().ToUpper()))).ToList();
                    hsclist.AddRange(hscstudentlist);
                    break;
                case "index":
                    hscstudentlist = db.Tbl_HSC_Form17_Final.Where(x => x.Division == divcode && (x.Email_ID.ToUpper().Contains(Search_Data.Trim().ToUpper()))).ToList();
                    hsclist.AddRange(hscstudentlist);
                    break;
                default:
                    hscstudentlist = db.Tbl_HSC_Form17_Final.Where(x => x.Division == divcode).ToList();
                    hsclist.AddRange(hscstudentlist);
                    break;
            }
            int Size_Of_Page = 10;
            int No_Of_Page = (Page_No ?? 1);
            return View(hsclist.ToPagedList(No_Of_Page, Size_Of_Page));
        }

        public JsonResult GetDivReport()
        {
            var records = (from row in db.Tbl_HSC_Form17 select row).Take(100);
            if (records == null)
            {
                return Json(new { Result = false, message = "No record found" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Result = true, Responce = records }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetStudentList(string standard/*, int nthslot, int length*/)
        {
            try
            {
                //ExportToExcel(standard);
                var divcode = this.User.Identity.Name;
                List<Tbl_HSC_Form17_Final> hscstudentlist = new List<Tbl_HSC_Form17_Final>();
                List<Tbl_SSC_Form17_Final> sscstudentlist = new List<Tbl_SSC_Form17_Final>();
                List<Tbl_SSC_Form17_Final> ssclist = new List<Tbl_SSC_Form17_Final>();
                List<Tbl_HSC_Form17_Final> hsclist = new List<Tbl_HSC_Form17_Final>();
                if (standard == "0")
                {
                    return Json(new { Result = false }, JsonRequestBehavior.AllowGet);
                }
                else if (standard == "SSC")
                {
                    sscstudentlist = db.Tbl_SSC_Form17_Final.Where(x => x.Division == divcode).ToList();
                    ssclist.AddRange(sscstudentlist);
                    var count = ssclist.Count();
                    ssclist = ssclist.Skip(0).Take(10).ToList();
                    return Json(new { Result = true, data = ssclist, totcont = count }, JsonRequestBehavior.AllowGet);
                }

                else if (standard == "HSC")
                {
                    hscstudentlist = db.Tbl_HSC_Form17_Final.Where(x => x.Division == divcode).ToList();
                    hsclist.AddRange(hscstudentlist);
                    var count = hsclist.Count();
                    hsclist = hsclist.Skip(0).Take(10).ToList();
                    return Json(new { Result = true, data = hsclist, totcont = count }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { Result = false }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = false, Message = "Something went wrong !" }, JsonRequestBehavior.AllowGet);
                throw ex;
            }
        }


        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Defining type of data column gives proper data table 
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name, type);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }
        public void GetDistrictList()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem { Text = "-Select District-", Value = "0" });
            var model = db.Tbl_Code_Master.Select(n => new
            {
                n.district_name,
                n.district_code
            }).Distinct().OrderBy(n => n.district_name).ToList();
            foreach (var item in model)
            {
                list.Add(new SelectListItem { Text = item.district_name, Value = item.district_code.ToString() });
            }
            ViewBag.DistrictList = new SelectList(list, "Value", "Text");
        }
    }
}