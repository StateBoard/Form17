using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel;
using Form17.Models;
using System.Data;
using System.IO;
using System.Web.Security;
using System.Configuration;

namespace Form17.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        Form17Entities db = new Form17Entities();
        DataAccessLayer dc = new DataAccessLayer();
        PDFMaker pdfMaker = new PDFMaker();
        EC_HSC_PDFController hscec = new EC_HSC_PDFController();
        EC_SSC_PDFController sscec = new EC_SSC_PDFController();
        EC_HSC_ModelController hec = new EC_HSC_ModelController();

        [HandleError]
        [HttpGet]

        public string SetDivisionName(string divcode)
        {
            string str = "";
            switch (divcode)
            {
                case "1": str = "Pune"; break;
                case "2": str = "Nagpur"; break;
                case "3": str = "Aurangabad"; break;
                case "4": str = "Mumbai"; break;
                case "5": str = "Kolhapur"; break;
                case "6": str = "Amravati"; break;
                case "7": str = "Nashik"; break;
                case "8": str = "Latur"; break;
                case "9": str = "Kokan"; break;
            }
            return str;
        }
        public ActionResult Halt()
        {
            return View();
        }
        public ActionResult Init()
        {
            return View();
        }
        public ActionResult Index()
       {
            try
            {
                ViewBag.IsActive = db.Tbl_Site_Status.Select(x => x.Active_Status).FirstOrDefault();
                return View();
            }
            catch (Exception ex)
            {

                return RedirectToAction("Error", "Shared");
                throw ex;
            }
        }

        [HandleError]
        [HttpGet]
        public ActionResult SSCForm17()
        {
            try
            {
                ViewBag.MonthYearofExam = ConfigurationManager.AppSettings["MonthYearOFExam"].ToString();
                //ViewBag.IsActive = db.Tbl_Site_Status.Select(x => x.Active_Status).FirstOrDefault();
                //if (ViewBag.IsActive == 0)
                //{
                //    return RedirectToAction("DeActive", "Shared");
                //}
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
                return View();
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Shared");
                throw ex;
            }
        }

        [HandleError]
        [HttpPost]
        public JsonResult SSCForm17(Tbl_SSC_Form17 tbl_SSC_Form17)
        {
            try
            {
                SabPaisaIntegration objsb = new SabPaisaIntegration();
                tbl_SSC_Form17.Residential_Address = tbl_SSC_Form17.Residential_Address.Trim();
                tbl_SSC_Form17.Name_of_Secondary_School = tbl_SSC_Form17.Name_of_Secondary_School.Trim();
                tbl_SSC_Form17.Division = db.Tbl_Code_Master.Where(x => x.district_code == tbl_SSC_Form17.District_of_Form_Submission).FirstOrDefault().division_code;
                tbl_SSC_Form17.Unique_ID_Payment = objsb.randomTxnId().ToString() + "501";
                db.Tbl_SSC_Form17.Add(tbl_SSC_Form17);
                db.SaveChanges();

                string folder = Server.MapPath(string.Format("~/AppFiles/Images/SSC/{0}/", tbl_SSC_Form17.ID));
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                if (tbl_SSC_Form17.SSC_Photo != null && (tbl_SSC_Form17.SSC_Photo.ContentLength > 0))
                {
                    var extension = Path.GetExtension(tbl_SSC_Form17.SSC_Photo.FileName);
                    tbl_SSC_Form17.SSC_Photo.SaveAs(Server.MapPath("../AppFiles/Images/SSC/" + tbl_SSC_Form17.ID + "/") + "Photo.jpeg");
                }
                if (tbl_SSC_Form17.SSC_Domicile != null && (tbl_SSC_Form17.SSC_Domicile.ContentLength > 0))
                {
                    var extension = Path.GetExtension(tbl_SSC_Form17.SSC_Domicile.FileName);
                    tbl_SSC_Form17.SSC_Domicile.SaveAs(Server.MapPath("../AppFiles/Images/SSC/" + tbl_SSC_Form17.ID + "/") + "Domicile_Certificate" + extension);
                }
                if (tbl_SSC_Form17.SSC_LC != null && (tbl_SSC_Form17.SSC_LC.ContentLength > 0))
                {
                    var extension = Path.GetExtension(tbl_SSC_Form17.SSC_LC.FileName);
                    tbl_SSC_Form17.SSC_LC.SaveAs(Server.MapPath("../AppFiles/Images/SSC/" + tbl_SSC_Form17.ID + "/") + "Leaving_Certificate" + extension);
                }
                if (tbl_SSC_Form17.SSC_Handicap_Certificate != null && (tbl_SSC_Form17.SSC_Handicap_Certificate.ContentLength > 0))
                {
                    var extension = Path.GetExtension(tbl_SSC_Form17.SSC_LC.FileName);
                    tbl_SSC_Form17.SSC_Handicap_Certificate.SaveAs(Server.MapPath("../AppFiles/Images/SSC/" + tbl_SSC_Form17.ID + "/") + "Handicap_Certificate" + extension);
                }

                tbl_SSC_Form17 = db.Tbl_SSC_Form17.Where(model => model.ID == tbl_SSC_Form17.ID).FirstOrDefault();
                //DataTable dt = dc.ReturnDataTable(@"SELECT DATEDIFF(DAY, Convert(date ,Super_Late_Fee_Date), '"+DateTime.Now.ToString("yyyy/mm/dd")+"') AS DateDiff from Tbl_Site_Status");
                var date = DateTime.Now.ToString("dd/MM/yyyy");
                //DataTable dt = dc.ReturnDataTable(@"SELECT DATEDIFF(DAY, Convert(date ,Super_Late_Fee_Date), '" + ConversionDate_DDMMYYYY_To_YYYYMMDD(date) + "') AS DateDiff from Tbl_Site_Status");
               // var superlatefee = 1245 + ((Int32.Parse(dt.Rows[0]["DateDiff"].ToString()) + 1) * 20);
                SabPaisaRequest requestToGateway = new SabPaisaRequest();
                requestToGateway.spUrl = Common_SSC.Payment_Getway_URL();
                requestToGateway.authIV = Common_SSC.Payment_Authentication_IV();//"qz7zPW07upqREhuo";
                requestToGateway.authKey = Common_SSC.Payment_Authentication_KEY(); ;//"vuQy2eFx4q095E03";
                requestToGateway.clientCode = Common_SSC.Payment_Client_Code();//"NITE5";
                requestToGateway.userName = Common_SSC.Payment_Username();//"Ish988@sp";
                requestToGateway.pass = Common_SSC.Payment_Password();//"wF2F0io7gdNj";
                requestToGateway.clientTxnId = tbl_SSC_Form17.Unique_ID_Payment;//will be unique for every transaction
                double amount = 1100; //superlatefee;
                requestToGateway.amt = amount.ToString();        //will change according to user payble amount    
                                                                 //requestToGateway.successUrl = "http://form17.mh-ssc.ac.in/Payment/SSC_Success";// For example 
                                                                 // requestToGateway.failureUrl = "http://form17.mh-ssc.ac.in/Payment/SSC_Failure";//For example
                requestToGateway.successUrl = "https://localhost:44374//Payment/SSC_Success";// For example 
                requestToGateway.failureUrl = "https://localhost:44374//Payment/SSC_Failure";//For example
                requestToGateway.firstName = tbl_SSC_Form17.First_Name.ToString(); //will change according to userName
                requestToGateway.lastName = tbl_SSC_Form17.Last_Name.ToString(); ;//will change according to userLastN
                requestToGateway.email = tbl_SSC_Form17.Email_ID.ToString().Trim();//will change according to user Email
                requestToGateway.contactNo = tbl_SSC_Form17.Mobile_No.ToString(); // will change according to user Mobile Number
                requestToGateway.add = "StateBoard PUNE"; // will change according to user Address
                requestToGateway.prodCode = "P005";
                requestToGateway.programId = "NA";
                requestToGateway.udf1 = tbl_SSC_Form17.Index_No_of_School.ToString();
                requestToGateway.udf15 = "SSC_" + tbl_SSC_Form17.Division;
                int Payment_Head = 501;
                requestToGateway.udf16 = "2023_" + Payment_Head.ToString();
                requestToGateway.udf17 = tbl_SSC_Form17.Index_No_of_School.ToString();
                requestToGateway.udf18 = tbl_SSC_Form17.ID.ToString();
                requestToGateway.udf19 = tbl_SSC_Form17.Email_ID.ToString() + Payment_Head;
                requestToGateway.udf20 = tbl_SSC_Form17.Email_ID.ToString();
                return Json(new { Result = true, RedirectURL = objsb.forwardToSabPaisa(requestToGateway).ToString() }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //return Json(new { Result = false, Message = "Sorry, something went wrong while processing your request !" });
                return Json(new { Result = false, Message = ex.ToString() });
                throw ex;
            }
        }

        [HandleError]
        [HttpGet]
        public ActionResult HSCForm17()
        {
            try
            {
                ViewBag.MonthYearofExam = ConfigurationManager.AppSettings["MonthYearOFExam"].ToString();
                //ViewBag.IsActive = db.Tbl_Site_Status.Select(x => x.Active_Status).FirstOrDefault();
                //if (ViewBag.IsActive == 0)
                //{
                //    return RedirectToAction("DeActive", "Shared");
                //
                //}
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
                return View();
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Shared");
                throw ex;
            }
        }

        [HandleError]
        [HttpPost]
        public JsonResult HSCForm17(Tbl_HSC_Form17 tbl_HSC_Form17)
        {
            try
            {
                SabPaisaIntegration objsb = new SabPaisaIntegration();
                tbl_HSC_Form17.Residential_Address = tbl_HSC_Form17.Residential_Address.Trim();
                tbl_HSC_Form17.Name_of_Secondary_School = tbl_HSC_Form17.Name_of_Secondary_School.Trim();
                if (tbl_HSC_Form17.Attended_Junior_College_Yes_No.Trim() == "Yes")
                {
                    tbl_HSC_Form17.Name_of_Junior_College = tbl_HSC_Form17.Name_of_Junior_College.Trim();
                }
                tbl_HSC_Form17.Unique_ID_Payment = objsb.randomTxnId().ToString() + "501";
                tbl_HSC_Form17.Division = db.Tbl_Code_Master.Where(x => x.district_code == tbl_HSC_Form17.District_of_Form_Submission).FirstOrDefault().division_code;
                db.Tbl_HSC_Form17.Add(tbl_HSC_Form17);
                db.SaveChanges();

                string folder = Server.MapPath(string.Format("~/AppFiles/Images/HSC/{0}/", tbl_HSC_Form17.ID));
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }

                if (tbl_HSC_Form17.HSC_Photo != null && (tbl_HSC_Form17.HSC_Photo.ContentLength > 0))
                {
                    var extension = Path.GetExtension(tbl_HSC_Form17.HSC_Photo.FileName);
                    tbl_HSC_Form17.HSC_Photo.SaveAs(Server.MapPath("../AppFiles/Images/HSC/" + tbl_HSC_Form17.ID + "/") + "Photo.jpeg");
                }
                if (tbl_HSC_Form17.HSC_School_LC != null && (tbl_HSC_Form17.HSC_Photo.ContentLength > 0))
                {
                    var extension = Path.GetExtension(tbl_HSC_Form17.HSC_School_LC.FileName);
                    tbl_HSC_Form17.HSC_School_LC.SaveAs(Server.MapPath("../AppFiles/Images/HSC/" + tbl_HSC_Form17.ID + "/") + "Original_LC" + extension);
                }
                if (tbl_HSC_Form17.HSC_School_LC != null && (tbl_HSC_Form17.HSC_School_LC.ContentLength > 0))
                {
                    var extension = Path.GetExtension(tbl_HSC_Form17.HSC_School_LC.FileName);
                    tbl_HSC_Form17.HSC_School_LC.SaveAs(Server.MapPath("../AppFiles/Images/HSC/" + tbl_HSC_Form17.ID + "/") + "Duplicate_LC" + extension);
                }
                if (tbl_HSC_Form17.HSC_Domicile != null && (tbl_HSC_Form17.HSC_Domicile.ContentLength > 0))
                {
                    var extension = Path.GetExtension(tbl_HSC_Form17.HSC_Domicile.FileName);
                    tbl_HSC_Form17.HSC_School_LC.SaveAs(Server.MapPath("../AppFiles/Images/HSC/" + tbl_HSC_Form17.ID + "/") + "Domicile" + extension);
                }


                tbl_HSC_Form17 = db.Tbl_HSC_Form17.Where(model => model.ID == tbl_HSC_Form17.ID).FirstOrDefault();
                var date = DateTime.Now.ToString("dd/MM/yyyy");
               // DataTable dt = dc.ReturnDataTable(@"SELECT DATEDIFF(DAY, Convert(date ,Super_Late_Fee_Date), '" + ConversionDate_DDMMYYYY_To_YYYYMMDD(date.Replace('-', '/')) + "') AS DateDiff from Tbl_Site_Status");
               // var superlatefee = 745 + ((Int32.Parse(dt.Rows[0]["DateDiff"].ToString()) + 1) * 20);
                SabPaisaRequest requestToGateway = new SabPaisaRequest();
                requestToGateway.spUrl = Common_HSC.Payment_Getway_URL();
                requestToGateway.authIV = Common_HSC.Payment_Authentication_IV();//"qz7zPW07upqREhuo";
                requestToGateway.authKey = Common_HSC.Payment_Authentication_KEY(); ;//"vuQy2eFx4q095E03";
                requestToGateway.clientCode = Common_HSC.Payment_Client_Code();//"NITE5";
                requestToGateway.userName = Common_HSC.Payment_Username();//"Ish988@sp";
                requestToGateway.pass = Common_HSC.Payment_Password();//"wF2F0io7gdNj";
                requestToGateway.clientTxnId = tbl_HSC_Form17.Unique_ID_Payment;//will be unique for every transaction
                double amount = 600; // superlatefee;
                requestToGateway.amt = amount.ToString();        //will change according to user payble amount    
                requestToGateway.successUrl = "http://form17.mh-hsc.ac.in/Payment/HSC_Success";// For example 
                requestToGateway.failureUrl = "http://form17.mh-hsc.ac.in/Payment/HSC_Failure";//For example
                requestToGateway.firstName = tbl_HSC_Form17.First_Name.ToString(); //will change according to userName
                requestToGateway.lastName = tbl_HSC_Form17.Last_Name.ToString(); ;//will change according to userLastN
                requestToGateway.email = tbl_HSC_Form17.Email_ID.ToString().Trim();//will change according to user Email
                requestToGateway.contactNo = tbl_HSC_Form17.Mobile_No.ToString(); // will change according to user Mobile Number
                requestToGateway.add = "StateBoard PUNE"; // will change according to user Address
                requestToGateway.prodCode = "P005";
                requestToGateway.programId = "NA";
                requestToGateway.udf1 = tbl_HSC_Form17.College_Index_No.ToString();
                requestToGateway.udf15 = "HSC_" + tbl_HSC_Form17.Division;
                int Payment_Head = 501;
                requestToGateway.udf16 = "Form17_2022_" + Payment_Head.ToString();
                requestToGateway.udf17 = tbl_HSC_Form17.College_Index_No.ToString();
                requestToGateway.udf18 = tbl_HSC_Form17.ID.ToString();
                requestToGateway.udf19 = tbl_HSC_Form17.Email_ID.ToString() + Payment_Head;
                requestToGateway.udf20 = tbl_HSC_Form17.Email_ID.ToString();
                // pdfMaker.ConvertDate();
                return Json(new { Result = true, RedirectURL = objsb.forwardToSabPaisa(requestToGateway).ToString() }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = false, Message = "Sorry, something went wrong while processing your request !" }, JsonRequestBehavior.AllowGet);
                throw ex;
            }
        }
        [HttpGet]
        public ActionResult RePrintApplication()
        {
            try
            {
                ViewBag.IsActive = db.Tbl_Site_Status.Select(x => x.Active_Status).FirstOrDefault();
                return View();
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Shared");
                //throw ex;
            }
        }
        [HttpPost]
        public JsonResult RePrintApplication(string std, string email, string mobile)
        {
            try
            {
                if (std == "0" || email == null || mobile == null || email.Trim() == "" || mobile.Trim() == "")
                {
                    return Json(new { Result = false, Message = "Please provide complete details." });
                }
                if (std == "SSC")
                {
                    var IsRegistered = db.Tbl_SSC_Form17_Final.Where(x => x.Email_ID.Trim().ToUpper() == email.Trim().ToUpper() && x.Mobile_No == mobile).FirstOrDefault();
                    if (IsRegistered == null)
                    {
                        return Json(new { Result = false, Message = "No record found." });
                    }
                    if (IsRegistered.Unique_ID_Payment != null)
                    {
                        pdfMaker.Create_SSC_PDF(Int32.Parse(IsRegistered.Ref_ID.ToString()), db.Tbl_SSC_Payment.Where(x => x.udf18 == IsRegistered.Ref_ID.ToString()).FirstOrDefault());
                        var FileVirtualPath = "/PDF/SSC/" + IsRegistered.S_ID + ".pdf";
                        return Json(new { Result = true, file = FileVirtualPath });
                    }
                }
                else if (std == "HSC")
                {
                    var IsRegistered = db.Tbl_HSC_Form17_Final.Where(x => x.Email_ID.Trim().ToUpper() == email.Trim().ToUpper() && x.Mobile_No == mobile).FirstOrDefault();
                    if (IsRegistered == null)
                    {
                        return Json(new { Result = false, Message = "No record found." });
                    }
                    if (IsRegistered.Unique_ID_Payment != null)
                    {
                        pdfMaker.Create_HSC_PDF(Int32.Parse(IsRegistered.Ref_ID.ToString()), db.Tbl_HSC_Payment.Where(x => x.udf18 == IsRegistered.Ref_ID.ToString()).FirstOrDefault());
                        var FileVirtualPath = "/PDF/HSC/" + IsRegistered.S_ID + ".pdf";
                        return Json(new { Result = true, file = FileVirtualPath });
                    }
                }
                else
                {
                    return Json(new { Result = false, Message = "No record found." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = false, Message = ex.ToString() });
            }
            return Json(new { Result = false, Message = "Something went wrong." });
        }

       
        public JsonResult IsAlreadyCollegeUpdated(string FormNo, string mobileno, string email)
        {
            try
            {
                if (FormNo == null || mobileno == null || email == null || FormNo.Trim() == "" || mobileno.Trim() == "" || email.Trim() == "")
                {
                    return Json(new { Result = true, Message = "Please provide complete details" });
                }
                if (db.Tbl_Update_Old_College.Any(x => x.Form_No.Trim().ToUpper() == FormNo.Trim().ToUpper() && x.Email_ID.Trim().ToUpper() == email.Trim().ToUpper()))
                {
                    return Json(new { Result = true, Message = "You have already changed your center of form submission" });
                }
                else
                {
                    var oldcollegerecord = db.Tbl_HSC_Form17_Final.Where(x => x.S_ID.Trim().ToUpper() == FormNo.Trim().ToUpper() && x.Email_ID.Trim().ToUpper() == email.Trim().ToUpper()).FirstOrDefault();
                    var oldcentermodel = db.Tbl_Center.Where(x => x.Index_No == oldcollegerecord.College_Index_No && x.STD == 12).FirstOrDefault();
                    if (oldcentermodel != null)
                    {
                        return Json(new { Result = false, index = oldcollegerecord.College_Index_No, centername = oldcentermodel.Name });
                    }
                    else
                    {
                        return Json(new { Result = false, index = oldcollegerecord.College_Index_No, centername = "NA(College is cancelled)" });
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = true, Message = "Something went wrong" });
            }
        }
        public JsonResult IsAlreadySchoolUpdated(string FormNo, string mobileno, string email)
        {
            try
            {
                if (FormNo == null || mobileno == null || email == null || FormNo.Trim() == "" || mobileno.Trim() == "" || email.Trim() == "")
                {
                    return Json(new { Result = true, Message = "Please provide complete details" });
                }
                if (db.Tbl_Update_Old_College.Any(x => x.Form_No.Trim().ToUpper() == FormNo.Trim().ToUpper() && x.Email_ID.Trim().ToUpper() == email.Trim().ToUpper()))
                {
                    return Json(new { Result = true, Message = "You have already changed your center of form submission" });
                }
                else
                {
                    var oldschoolecord = db.Tbl_SSC_Form17_Final.Where(x => x.S_ID.Trim().ToUpper() == FormNo.Trim().ToUpper() && x.Email_ID.Trim().ToUpper() == email.Trim().ToUpper()).FirstOrDefault();
                    var oldcentermodel = db.Tbl_Center.Where(x => x.Index_No == oldschoolecord.School_of_Form_Submission && x.STD == 10).FirstOrDefault();
                    if (oldcentermodel != null)
                    {
                        return Json(new { Result = false, index = oldschoolecord.School_of_Form_Submission, centername = oldcentermodel.Name });
                    }
                    else
                    {
                        return Json(new { Result = false, index = oldschoolecord.School_of_Form_Submission, centername = "NA(Center is cancelled)" });
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = true, Message = "Something went wrong" });
            }
        }
        public bool IsIndexOfAnotherDivision(string index, string oldindex)
        {
            var olddivcode = db.Tbl_Code_Master.Where(a => a.district_code == oldindex.Substring(0, 2)).FirstOrDefault().division_code;
            var newdivcode = db.Tbl_Code_Master.Where(a => a.district_code == index.Substring(0, 2)).FirstOrDefault().division_code;
            if (olddivcode == newdivcode)
            {
                return false;
            }
            return true;
        }
        [HttpGet]
        public ActionResult UpdateOldSchool()
        {
            ViewBag.IsActive = db.Tbl_Site_Status.Select(x => x.Active_Status).FirstOrDefault();
            GetDistrictList();
            return View();
        }
        public JsonResult UpdateOldSchool(string FormNo, string District, string Taluka, string Medium, string Old_School, string Updated_School, string Email_ID)
        {
            try
            {
                ViewBag.IsActive = db.Tbl_Site_Status.Select(x => x.Active_Status).FirstOrDefault();
                if (FormNo == null || Old_School == null || Updated_School == null || Email_ID == null || FormNo.Trim() == "" || Old_School.Trim() == "" || Updated_School.Trim() == "" || Email_ID.Trim() == "")
                {
                    return Json(new { Result = false, Message = "Please provide complete details !" });
                }
                var isexist = db.Tbl_Update_Old_College.Where(x => x.Form_No.Trim().ToUpper() == FormNo.Trim().ToUpper()).FirstOrDefault();
                if (isexist != null)
                {
                    return Json(new { Result = false, Message = "You have already changed your center of form submission !" });
                }
                if (IsIndexOfAnotherDivision(Updated_School, Old_School))
                {
                    return Json(new { Result = false, Message = "Your new center should be within the previous divisonal board's district" });
                }

                Tbl_SSC_Form17_Final tbl_SSC_Form17_Final = db.Tbl_SSC_Form17_Final.Where(x => x.Email_ID.Trim().ToUpper() == Email_ID.Trim().ToUpper()).FirstOrDefault();
                tbl_SSC_Form17_Final.Division = db.Tbl_Code_Master.Where(x => x.district_code == District).FirstOrDefault().division_code;               

                tbl_SSC_Form17_Final.District_of_Form_Submission = District;
                tbl_SSC_Form17_Final.Taluka_of_Form_Submission = Taluka;
                tbl_SSC_Form17_Final.Medium_of_Form = Medium;
                tbl_SSC_Form17_Final.Index_No_of_School = Updated_School;
                tbl_SSC_Form17_Final.School_of_Form_Submission = Updated_School;

                db.Tbl_SSC_Form17_Final.Attach(tbl_SSC_Form17_Final);
                db.Entry(tbl_SSC_Form17_Final).Property(x => x.District_of_Form_Submission).IsModified = true;
                db.Entry(tbl_SSC_Form17_Final).Property(x => x.Taluka_of_Form_Submission).IsModified = true;
                db.Entry(tbl_SSC_Form17_Final).Property(x => x.Medium_of_Form).IsModified = true;
                db.Entry(tbl_SSC_Form17_Final).Property(x => x.Index_No_of_School).IsModified = true;
                db.Entry(tbl_SSC_Form17_Final).Property(x => x.School_of_Form_Submission).IsModified = true;
                db.SaveChanges();

                Tbl_Update_Old_College updatecollege = new Tbl_Update_Old_College();
                updatecollege.Form_No = FormNo.Trim().ToUpper();
                updatecollege.Old_College = Old_School;
                updatecollege.Updated_College = Updated_School;
                updatecollege.Email_ID = Email_ID.Trim();
                db.Tbl_Update_Old_College.Add(updatecollege);
                db.SaveChanges();
                return Json(new { Result = true, Message = "Your center of form submission is changed, you can reprint the application!" });

            }
            catch (Exception ex)
            {
                return Json(new { Result = false, Message = "Something went wrong !" });
            }
        }
        [HttpGet]
        public ActionResult UpdateOldCollege()
        {
            ViewBag.IsActive = db.Tbl_Site_Status.Select(x => x.Active_Status).FirstOrDefault();
            GetDistrictList();
            return View();
        }
        public JsonResult UpdateOldCollege(string FormNo, string District, string Taluka, string Stream, string Medium, string Old_College, string Updated_College, string Email_ID)
        {
            try
            {
                ViewBag.IsActive = db.Tbl_Site_Status.Select(x => x.Active_Status).FirstOrDefault();
                if (FormNo == null || Old_College == null || Updated_College == null || Email_ID == null || FormNo.Trim() == "" || Old_College.Trim() == "" || Updated_College.Trim() == "" || Email_ID.Trim() == "")
                {
                    return Json(new { Result = false, Message = "Please provide complete details !" });
                }
                var isexist = db.Tbl_Update_Old_College.Where(x => x.Form_No.Trim().ToUpper() == FormNo.Trim().ToUpper()).FirstOrDefault();
                if (isexist != null)
                {
                    return Json(new { Result = false, Message = "You have already changed your center of form submission !" });
                }
                if (IsIndexOfAnotherDivision(Updated_College, Old_College))
                {
                    return Json(new { Result = false, Message = "Your new center should be within the previous divisonal board's district" });
                }

                Tbl_HSC_Form17_Final tbl_HSC_Form17_Final = db.Tbl_HSC_Form17_Final.Where(x => x.Email_ID.Trim().ToUpper() == Email_ID.Trim().ToUpper()).FirstOrDefault();
                tbl_HSC_Form17_Final.Division = db.Tbl_Code_Master.Where(x => x.district_code == District).FirstOrDefault().division_code;

                tbl_HSC_Form17_Final.District_of_Form_Submission = District;
                tbl_HSC_Form17_Final.Taluka_of_Form_Submission = Taluka;
                tbl_HSC_Form17_Final.Stream_of_Form = Stream;
                tbl_HSC_Form17_Final.Medium_of_Form = Medium;
                tbl_HSC_Form17_Final.College_Index_No = Updated_College;
                tbl_HSC_Form17_Final.College_of_Form_Submission = Updated_College;
                db.Tbl_HSC_Form17_Final.Attach(tbl_HSC_Form17_Final);
                db.Entry(tbl_HSC_Form17_Final).Property(x => x.District_of_Form_Submission).IsModified = true;
                db.Entry(tbl_HSC_Form17_Final).Property(x => x.Taluka_of_Form_Submission).IsModified = true;
                db.Entry(tbl_HSC_Form17_Final).Property(x => x.Stream_of_Form).IsModified = true;
                db.Entry(tbl_HSC_Form17_Final).Property(x => x.Medium_of_Form).IsModified = true;
                db.Entry(tbl_HSC_Form17_Final).Property(x => x.College_Index_No).IsModified = true;
                db.Entry(tbl_HSC_Form17_Final).Property(x => x.College_of_Form_Submission).IsModified = true;
                db.SaveChanges();

                Tbl_Update_Old_College updatecollege = new Tbl_Update_Old_College();
                updatecollege.Form_No = FormNo.Trim().ToUpper();
                updatecollege.Old_College = Old_College;
                updatecollege.Updated_College = Updated_College;
                updatecollege.Email_ID = Email_ID.Trim();
                db.Tbl_Update_Old_College.Add(updatecollege);
                db.SaveChanges();
                return Json(new { Result = true, Message = "Your center of form submission is changed, you can reprint the application!" });

            }
            catch (Exception ex)
            {
                return Json(new { Result = false, Message = "Something went wrong !" });
            }
        }
        public ActionResult HSCEnrollmentCertificate()
        {
            try
            {
                ViewBag.IsActive = db.Tbl_Site_Status.Select(x => x.Active_Status).FirstOrDefault();
                return View();
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Shared");
                throw ex;
            }
        }
        [HttpPost]
        public JsonResult HSCEnrollmentCertificate(string formno, string mobile)
        {
            try
            {
                if (formno == null || mobile == null || formno.Trim() == "" || formno.Trim() == "" || formno.Trim().Length != 7)
                {
                    return Json(new { Result = false, Message = "Please provide valid details." });
                }
                else
                {
                    var IsExist = db.Tbl_HSC_Form17_Final.Where(x => x.Mobile_No.Trim().ToUpper() == mobile.Trim().ToUpper() && x.S_ID == formno && x.EC_Status == "1").FirstOrDefault();
                    if (IsExist == null)
                    {
                        return Json(new { Result = false, Message = "Your EC is not generated. Please contact to your divisional board" });
                    }
                    if (IsExist.EC_Status == "1")
                    {
                        Tbl_HSC_Form17_Final tbl_HSC_Form17_Final = new Tbl_HSC_Form17_Final();
                        tbl_HSC_Form17_Final = db.Tbl_HSC_Form17_Final.Where(x => x.S_ID == formno.Trim().ToUpper() && x.EC_Status == "1").FirstOrDefault();
                        var centerdetails = db.Tbl_Center.Where(x => x.Index_No == tbl_HSC_Form17_Final.College_of_Form_Submission && x.STD == 12).FirstOrDefault();
                        hec.HSC_Year = 2023;
                        hec.Candidate_Name = tbl_HSC_Form17_Final.First_Name + " " + tbl_HSC_Form17_Final.Father_Husband_Name + " " + tbl_HSC_Form17_Final.Last_Name;
                        hec.Mother_Name = tbl_HSC_Form17_Final.Mother_Name;
                        hec.Divisional_Board = SetDivisionName(tbl_HSC_Form17_Final.Division);
                        hec.Birth_date = tbl_HSC_Form17_Final.DOB;
                        hec.Gender = tbl_HSC_Form17_Final.Gender;
                        hec.College_No = centerdetails.Index_No;
                        hec.Stream = tbl_HSC_Form17_Final.Stream_of_Form;
                        hec.EC_No = tbl_HSC_Form17_Final.EC_Code;
                        hec.College_Addr = centerdetails.Name + ", " + centerdetails.Address;
                        hec.Case_No = "X" + tbl_HSC_Form17_Final.S_ID;
                        hec.Ref_ID = tbl_HSC_Form17_Final.Ref_ID.ToString();
                        hec.S_ID = tbl_HSC_Form17_Final.S_ID;
                        hec.Division = tbl_HSC_Form17_Final.Division;
                        hec.Date_Issue =tbl_HSC_Form17_Final.EC_Date;
                        hscec.Create_HSC_EC_PDF(hec);
                        var FileVirtualPath = "/PDF/HSC_EC/" + tbl_HSC_Form17_Final.S_ID + ".pdf";
                        return Json(new { Result = true, file = FileVirtualPath });
                    }
                }
            }
            catch(Exception ex)
            {
                
            }
            return Json(new { Result = false, Message = "Something went wrong!" });
        }
        public ActionResult SSCEnrollmentCertificate()
        {
            try
            {
                ViewBag.IsActive = db.Tbl_Site_Status.Select(x => x.Active_Status).FirstOrDefault();
                return View();
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Shared");
                throw ex;
            }
        }
        [HttpPost]
        public JsonResult SSCEnrollmentCertificate(string formno, string mobile)
        {
            try
            {
                if (formno == null || mobile == null || formno.Trim() == "" || formno.Trim() == "" || formno.Trim().Length != 7)
                {
                    return Json(new { Result = false, Message = "Please provide valid details." });
                }
                else
                {
                    var IsExist = db.Tbl_SSC_Form17_Final.Where(x => x.Mobile_No.Trim().ToUpper() == mobile.Trim().ToUpper() && x.S_ID == formno && x.EC_Status == "1").FirstOrDefault();
                    if (IsExist == null)
                    {
                        return Json(new { Result = false, Message = "Your EC is not generated. Please contact to your divisional board" });
                    }
                    if (IsExist.EC_Status == "1")
                    {
                        Tbl_SSC_Form17_Final tbl_SSC_Form17_Final = new Tbl_SSC_Form17_Final();
                        tbl_SSC_Form17_Final = db.Tbl_SSC_Form17_Final.Where(x => x.S_ID == formno.Trim().ToUpper() && x.EC_Status == "1").FirstOrDefault();
                        var centerdetails = db.Tbl_Center.Where(x => x.Index_No == tbl_SSC_Form17_Final.School_of_Form_Submission && x.STD == 10).FirstOrDefault();
                        hec.HSC_Year = 2023;
                        hec.Candidate_Name = tbl_SSC_Form17_Final.First_Name + " " + tbl_SSC_Form17_Final.Father_Husband_Name + " " + tbl_SSC_Form17_Final.Last_Name;
                        hec.Mother_Name = tbl_SSC_Form17_Final.Mother_Name;
                        hec.Divisional_Board = SetDivisionName(tbl_SSC_Form17_Final.Division);
                        hec.Birth_date = tbl_SSC_Form17_Final.DOB;
                        hec.Gender = tbl_SSC_Form17_Final.Gender;
                        if(centerdetails == null)
                        {
                            if (tbl_SSC_Form17_Final.School_of_Form_Submission == "1615066")
                            {
                                hec.College_No = "1615066";
                                hec.College_Addr = "MOTHER TERESA SECONDARY CONVENT SCHOOL, AIROLI, NAVI MUMBAI - 400 708";
                            }
                            else if (tbl_SSC_Form17_Final.School_of_Form_Submission == "1615086")
                            {
                                hec.College_No = "1615086";
                                hec.College_Addr = "NEELKANTH PATIL VIDYALAY, AIROLI, NAVI MUMBAI";
                            }
                            else if (tbl_SSC_Form17_Final.School_of_Form_Submission == "1617053")
                            {
                                hec.College_No = "1617053";
                                hec.College_Addr = "SAI SANSTHECHE ENGLISH SECONDARY CONVENT SCHOOL ";
                            }
                            else if (tbl_SSC_Form17_Final.School_of_Form_Submission == "1618017")
                            {
                                hec.College_No = "1618017";
                                hec.College_Addr = "SITALDAS KHEMANI HIGH SCHOOL, ULHASNAGAR - 421 002";
                            }
                            else if (tbl_SSC_Form17_Final.School_of_Form_Submission == "1701040")
                            {
                                hec.College_No = "1701040";
                                hec.College_Addr = "JANATA VIDYAMANDIR, AJIWALI, TAL - PANVEL 410 206";
                            }
                            else if (tbl_SSC_Form17_Final.School_of_Form_Submission == "1805007")
                            {
                                hec.College_No = "1805007";
                                hec.College_Addr = "MADHYAMIK VIDYALAY MU POST UDHWA TAL - TALASARI";
                            }
                            else if (tbl_SSC_Form17_Final.School_of_Form_Submission == "1808108")
                            {
                                hec.College_No = "1808108";
                                hec.College_Addr = "JIVDANI VIDYAVARDHINI HIGH SCHOOL VIRAR, PALGHAR EAST - 401 303";
                            }
                            else if (tbl_SSC_Form17_Final.School_of_Form_Submission == "3104066")
                            {
                                hec.College_No = "3104066";
                                hec.College_Addr = "BHARATRATNA DR. BABASAHEB AMBEDKAR VIDYALAY, DHARAVI, MUMBAI - 400 017";
                            }
                            else if (tbl_SSC_Form17_Final.School_of_Form_Submission == "1615119")
                            {
                                hec.College_No = "1615119";
                                hec.College_Addr = "NAVI MUMBAI MAHANAGARPALIKA SANCHALIT MADHYAMIK VIDYALAY TURBHEGAAV - NAVI MUMBAI";
                            }
                            else
                            {
                                hec.College_No = "";
                                hec.College_Addr = "";
                            }
                        }
                        else
                        {
                            hec.College_No = centerdetails.Index_No;
                            hec.College_Addr = centerdetails.Name + ", " + centerdetails.Address;
                        }
                        
                        hec.EC_No = tbl_SSC_Form17_Final.EC_Code;
                        
                        hec.Case_No = "X" + tbl_SSC_Form17_Final.S_ID;
                        hec.Ref_ID = tbl_SSC_Form17_Final.Ref_ID.ToString();
                        hec.S_ID = tbl_SSC_Form17_Final.S_ID;
                        hec.Division = tbl_SSC_Form17_Final.Division;
                        hec.Date_Issue = tbl_SSC_Form17_Final.EC_Date;
                        sscec.Create_SSC_EC_PDF(hec);
                        var FileVirtualPath = "/PDF/SSC_EC/" + tbl_SSC_Form17_Final.S_ID + ".pdf";
                        return Json(new { Result = true, file = FileVirtualPath });
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return Json(new { Result = false, Message = "Something went wrong!" });
        }
        public JsonResult getTalukaList(string distCode)
        {
            var model = db.Tbl_Code_Master.Where(x => x.district_code == distCode).Select(n => new
            {
                n.taluka_name,
                n.taluka_code
            }).Distinct().OrderBy(n => n.taluka_name).ToList();
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem { Text = "-Select Taluka-", Value = "0" });
            if (model != null)
            {
                foreach (var item in model)
                {
                    list.Add(new SelectListItem { Text = item.taluka_name.ToString(), Value = item.taluka_code.ToString() });
                }
            }
            return Json(new SelectList(list, "Value", "Text", JsonRequestBehavior.AllowGet));
        }

        public JsonResult getStreamList(string distCode, string talCode)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            var model = (from p in db.Tbl_Center_Details
                         where p.Index_No.Substring(0, 2).ToString() == distCode && p.Index_No.Substring(2, 2).ToString() == talCode && p.STD == 12
                         orderby p.Stream
                         select p.Stream).Distinct().ToList();
            list.Add(new SelectListItem { Text = "-Select Stream-", Value = "0" });
            if (model != null)
            {
                foreach (var item in model)
                {
                    list.Add(new SelectListItem { Text = item.ToString(), Value = item.ToString() });
                }
            }
            return Json(new SelectList(list, "Value", "Text", JsonRequestBehavior.AllowGet));
        }
        public JsonResult getMediumList(string distCode, string talCode, string stream)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            var model = (from p in db.Tbl_Center_Details
                         where p.Index_No.Substring(0, 2).ToString() == distCode && p.Index_No.Substring(2, 2).ToString() == talCode && p.Stream == stream && p.STD == 12
                         orderby p.Medium
                         select p.Medium).Distinct().ToList();
            list.Add(new SelectListItem { Text = "-Select Medium-", Value = "0" });
            if (model != null)
            {
                foreach (var item in model)
                {
                    list.Add(new SelectListItem { Text = item.ToString(), Value = item.ToString() });
                }
            }
            return Json(new SelectList(list, "Value", "Text", JsonRequestBehavior.AllowGet));
        }
        public JsonResult getCollegeList(string distCode, string talCode, string stream, string medium)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem { Text = "-Select College-", Value = "0" });
            var model = (from a in db.Tbl_Center
                         join b in db.Tbl_Center_Details on a.Index_No equals b.Index_No
                         where b.Index_No.Substring(0, 2).ToString() == distCode && b.Index_No.Substring(2, 2).ToString() == talCode && b.Stream == stream && b.STD == 12 && a.STD == 12 && b.Medium == medium && (db.Tbl_HSC_Form17_Final.Where(g => g.College_Index_No == a.Index_No).Count() < a.Count)
                         orderby a.Name
                         select new { a.Name, a.Index_No }).Distinct().ToList();
            if (model != null)
            {
                foreach (var item in model)
                {
                    list.Add(new SelectListItem { Text = item.Name.ToString(), Value = item.Index_No.ToString() });
                }
            }
            return Json(new SelectList(list, "Value", "Text", JsonRequestBehavior.AllowGet));
        }

        public JsonResult getCollegeListForDivision(string distCode, string talCode, string stream, string medium)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem { Text = "-Select College-", Value = "0" });
            var model = (from a in db.Tbl_Center
                         join b in db.Tbl_Center_Details on a.Index_No equals b.Index_No
                         where b.Index_No.Substring(0, 2).ToString() == distCode && b.Index_No.Substring(2, 2).ToString() == talCode && b.Stream == stream  && b.Medium == medium && a.STD == 12 && b.STD == 12
                         orderby a.Name
                         select new { a.Name, a.Index_No }).Distinct().ToList();
            if (model != null)
            {
                foreach (var item in model)
                {
                    list.Add(new SelectListItem { Text = item.Name.ToString(), Value = item.Index_No.ToString() });
                }
            }
            return Json(new SelectList(list, "Value", "Text", JsonRequestBehavior.AllowGet));
        }
        public JsonResult getSSCMediumList(string distCode, string talCode)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            var model = (from p in db.Tbl_Center_Details
                         where p.Index_No.Substring(0, 2).ToString() == distCode && p.Index_No.Substring(2, 2).ToString() == talCode && p.STD == 10
                         orderby p.Medium
                         select p.Medium).Distinct().ToList();
            list.Add(new SelectListItem { Text = "-Select Medium-", Value = "0" });
            if (model != null)
            {
                foreach (var item in model)
                {
                    list.Add(new SelectListItem { Text = item.ToString(), Value = item.ToString() });
                }
            }
            return Json(new SelectList(list, "Value", "Text", JsonRequestBehavior.AllowGet));
        }
        public JsonResult getSchoolList(string distCode, string talCode, string medium)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem { Text = "-Select School-", Value = "0" });
            var model = (from a in db.Tbl_Center
                         join b in db.Tbl_Center_Details on a.Index_No equals b.Index_No
                         where b.Index_No.Substring(0, 2).ToString() == distCode && b.Index_No.Substring(2, 2).ToString() == talCode && b.Medium == medium && b.STD == 10 && a.STD == 10 && (db.Tbl_SSC_Form17_Final.Where(g => g.Index_No_of_School == a.Index_No).Count() < a.Count)
                         orderby a.Name
                         select new { a.Name, a.Index_No }).Distinct().ToList();
            if (model != null)
            {
                foreach (var item in model)
                {
                    list.Add(new SelectListItem { Text = item.Name.ToString(), Value = item.Index_No.ToString() });
                }
            }
            return Json(new SelectList(list, "Value", "Text", JsonRequestBehavior.AllowGet));
        }

        public JsonResult getSchoolListForDivision(string distCode, string talCode, string medium)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem { Text = "-Select School-", Value = "0" });
            var model = (from a in db.Tbl_Center
                         join b in db.Tbl_Center_Details on a.Index_No equals b.Index_No
                         where b.Index_No.Substring(0, 2).ToString() == distCode && b.Index_No.Substring(2, 2).ToString() == talCode && b.Medium == medium && b.STD == 10 && a.STD == 10
                         orderby a.Name
                         select new { a.Name, a.Index_No }).Distinct().ToList();
            if (model != null)
            {
                foreach (var item in model)
                {
                    list.Add(new SelectListItem { Text = item.Name.ToString(), Value = item.Index_No.ToString() });
                }
            }
            return Json(new SelectList(list, "Value", "Text", JsonRequestBehavior.AllowGet));
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

        bool IsValidEmailAddress(string emailID)
        {
            return new System.ComponentModel.DataAnnotations.EmailAddressAttribute().IsValid(emailID);
        }

        bool containsNonDigits(string str)
        {
            foreach (char c in str)
            {
                if (!Char.IsDigit(c))
                {
                    return true;
                }
            }
            return false;
        }
        bool containsNonAlphabets(string str)
        {
            foreach (char c in str)
            {
                if (!Char.IsLetter(c))
                {
                    if (c == ' ')
                        continue;
                    return true;
                }
            }
            return false;
        }
        public string ConversionDate_DDMMYYYY_To_MMDDYYYY(string date)
        {
            if (date.Length != 10)
            {
                return date;
            }
            string[] str = date.Split('/');
            var getdate = str[1] + '/' + str[0] + '/' + str[2];
            return getdate;
        }
        public string ConversionDate_MMDDYYYY_To_DDMMYYYY(string date)
        {
            if (date.Length != 10)
            {
                return date;
            }
            string[] str = date.Split('/');
            var getdate = str[1] + '/' + str[0] + '/' + str[2];
            return getdate;
        }
        public string ConversionDate_DDMMYYYY_To_YYYYMMDD(string date)
        {
            if (date.Length != 10)
            {
                return date;
            }
            string[] str = date.Split('/');
            var getdate = str[2] + '/' + str[1] + '/' + str[0];
            return getdate;
        }
        public ActionResult Logout()
        {
            try
            {
                Session.Abandon();
                Session.Clear();
                FormsAuthentication.SignOut();
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}