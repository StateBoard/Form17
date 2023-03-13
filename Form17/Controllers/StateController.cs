using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Form17.Models;
using OfficeOpenXml;
using CSVLibraryAK;

namespace Form17.Controllers
{
    public class StateController : Controller
    {
        // GET: State
        Form17Entities db = new Form17Entities();

        public OleDbConnection OledbConn { get; private set; }
        public OleDbCommand OledbCmd { get; private set; }
        DataAccessLayer dataAccessLayer = new DataAccessLayer();

        [HandleError]
        public ActionResult Login()
        {
            ViewBag.IsActive = db.Tbl_Site_Status.Select(x => x.Active_Status).FirstOrDefault();
            return View();
        }

        [HandleError]
        [HttpPost]
        public ActionResult Login(Tbl_Login_Credentials tbl_Login_Credentials)
        {
            try
            {
                ViewBag.IsActive = db.Tbl_Site_Status.Select(x => x.Active_Status).FirstOrDefault();
                var IsMatched = db.Tbl_Login_Credentials.Where(x => x.Username == tbl_Login_Credentials.Username && x.Password == tbl_Login_Credentials.Password).FirstOrDefault();
                if (IsMatched != null)
                {
                    Session["State"] = IsMatched.Username;
                    Session["Username"] = IsMatched.Username;
                    FormsAuthentication.SetAuthCookie(tbl_Login_Credentials.Username, false);
                    return RedirectToAction("ExportToCSVStateLevel", "State");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid Credentials.");
                    return View();
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Shared");
            }
        }
        [HandleError]
        [HttpGet]
        public ActionResult UploadECData()
        {
            return View();
        }
        [HttpPost]
        public JsonResult UploadECData(UploadECDataModel uploadECDataModel)
        {
            try
            {
                if (uploadECDataModel.STD == "SSC")
                {
                    if (uploadECDataModel.EC_Data != null && (uploadECDataModel.EC_Data.ContentLength > 0))
                    {
                        var extension = Path.GetExtension(uploadECDataModel.EC_Data.FileName);
                        uploadECDataModel.EC_Data.SaveAs(Server.MapPath("../AppFiles/EC_Files/") + "SSC_EC_Data" + extension);
                    }
                    InitializeOledbConnection(Server.MapPath("../AppFiles/EC_Files/SSC_EC_Data.xls"), ".xls");
                    DataTable tempTable = ReadFile();
                    if (tempTable.Rows.Count > 0)
                    {
                        foreach (DataRow row in tempTable.Rows)
                        {
                            if (row["EC_Status"] != null && row["EC_Status"].ToString() != "" && row["S_ID"] != null && row["S_ID"].ToString() != "" && row["S_ID"].ToString().Length == 7)
                            {
                                Tbl_SSC_Form17_Final tbl_SSC_Form17_Final = new Tbl_SSC_Form17_Final();
                                var temp = row["S_ID"];
                                tbl_SSC_Form17_Final = db.Tbl_SSC_Form17_Final.Where(x => x.S_ID == temp.ToString()).FirstOrDefault();
                                if (tbl_SSC_Form17_Final.EC_Status == null && row["EC_Status"].ToString() == "1")
                                {
                                    tbl_SSC_Form17_Final.EC_Status = "1";
                                    tbl_SSC_Form17_Final.EC_Code = "220" + tbl_SSC_Form17_Final.Division + "" + (100000 + tbl_SSC_Form17_Final.ID);
                                    tbl_SSC_Form17_Final.EC_Date = DateTime.Now.ToString("dd/MM/yyyy");
                                    db.Tbl_SSC_Form17_Final.Attach(tbl_SSC_Form17_Final);
                                    db.Entry(tbl_SSC_Form17_Final).Property(x => x.EC_Code).IsModified = true;
                                    db.Entry(tbl_SSC_Form17_Final).Property(x => x.EC_Status).IsModified = true;
                                    db.Entry(tbl_SSC_Form17_Final).Property(x => x.EC_Date).IsModified = true;
                                    db.SaveChanges();
                                }
                            }
                        }
                    }

                    return Json(new { Result = true, Message = "File uploaded successfully !" }, JsonRequestBehavior.AllowGet);
                }
                else if (uploadECDataModel.STD == "HSC")
                {
                    if (uploadECDataModel.EC_Data != null && (uploadECDataModel.EC_Data.ContentLength > 0))
                    {
                        var extension = Path.GetExtension(uploadECDataModel.EC_Data.FileName);
                        uploadECDataModel.EC_Data.SaveAs(Server.MapPath("../AppFiles/EC_Files/") + "HSC_EC_Data" + extension);
                    }
                    InitializeOledbConnection(Server.MapPath("../AppFiles/EC_Files/HSC_EC_Data.xls"), ".xls");
                    DataTable tempTable = ReadFile();
                    if (tempTable.Rows.Count > 0)
                    {
                        foreach (DataRow row in tempTable.Rows)
                        {
                            if (row["EC_Status"] != null && row["EC_Status"].ToString() != "" && row["S_ID"] != null && row["S_ID"].ToString() != "" && row["S_ID"].ToString().Length == 7)
                            {
                                Tbl_HSC_Form17_Final tbl_HSC_Form17_Final = new Tbl_HSC_Form17_Final();
                                var temp = row["S_ID"];
                                tbl_HSC_Form17_Final = db.Tbl_HSC_Form17_Final.Where(x => x.S_ID == temp.ToString()).FirstOrDefault();
                                if(tbl_HSC_Form17_Final != null)
                                {
                                    if (tbl_HSC_Form17_Final.EC_Status == null && row["EC_Status"].ToString() == "1")
                                    {
                                        tbl_HSC_Form17_Final.EC_Status = "1";
                                        tbl_HSC_Form17_Final.EC_Code = "220" + tbl_HSC_Form17_Final.Division + "" + (200000 + tbl_HSC_Form17_Final.ID);
                                        tbl_HSC_Form17_Final.EC_Date = DateTime.Now.ToString("dd/MM/yyyy");
                                        db.Tbl_HSC_Form17_Final.Attach(tbl_HSC_Form17_Final);
                                        db.Entry(tbl_HSC_Form17_Final).Property(x => x.EC_Code).IsModified = true;
                                        db.Entry(tbl_HSC_Form17_Final).Property(x => x.EC_Status).IsModified = true;
                                        db.Entry(tbl_HSC_Form17_Final).Property(x => x.EC_Date).IsModified = true;
                                        db.SaveChanges();
                                    }
                                }                                
                            }
                        }
                    }
                    return Json(new { Result = true, Message = "File uploaded successfully !" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = false, Message = "Please Select Standard" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = false, Message = ex.ToString() }, JsonRequestBehavior.AllowGet);
            }

        }
        [HttpGet]
        public ActionResult RemoveECData()
        {
            return View();
        }
        public JsonResult RemoveECData(UploadECDataModel uploadECDataModel)
        {
            try
            {
                if (uploadECDataModel.STD == "SSC")
                {
                    if (uploadECDataModel.EC_Data != null && (uploadECDataModel.EC_Data.ContentLength > 0))
                    {
                        var extension = Path.GetExtension(uploadECDataModel.EC_Data.FileName);
                        uploadECDataModel.EC_Data.SaveAs(Server.MapPath("../AppFiles/EC_Files/") + "SSC_EC_Data_Remove" + extension);
                    }
                    InitializeOledbConnection(Server.MapPath("../AppFiles/EC_Files/SSC_EC_Data_Remove.xls"), ".xls");
                    DataTable tempTable = ReadFile();
                    if (tempTable.Rows.Count > 0)
                    {
                        foreach (DataRow row in tempTable.Rows)
                        {
                            if (row["EC_Status"] != null && row["EC_Status"].ToString() != "" && row["S_ID"] != null && row["S_ID"].ToString() != "" && row["S_ID"].ToString().Length == 7)
                            {
                                Tbl_SSC_Form17_Final tbl_SSC_Form17_Final = new Tbl_SSC_Form17_Final();
                                var temp = row["S_ID"];
                                tbl_SSC_Form17_Final = db.Tbl_SSC_Form17_Final.Where(x => x.S_ID == temp.ToString()).FirstOrDefault();
                                if (tbl_SSC_Form17_Final.EC_Status == "1" && row["EC_Status"].ToString() == "2")
                                {
                                    tbl_SSC_Form17_Final.EC_Status = null;
                                    tbl_SSC_Form17_Final.EC_Code = null;
                                    tbl_SSC_Form17_Final.EC_Date = null;
                                    db.Tbl_SSC_Form17_Final.Attach(tbl_SSC_Form17_Final);
                                    db.Entry(tbl_SSC_Form17_Final).Property(x => x.EC_Code).IsModified = true;
                                    db.Entry(tbl_SSC_Form17_Final).Property(x => x.EC_Status).IsModified = true;
                                    db.Entry(tbl_SSC_Form17_Final).Property(x => x.EC_Date).IsModified = true;
                                    db.SaveChanges();
                                }
                            }
                        }
                    }

                    return Json(new { Result = true, Message = "File uploaded successfully !" }, JsonRequestBehavior.AllowGet);
                }
                else if (uploadECDataModel.STD == "HSC")
                {
                    if (uploadECDataModel.EC_Data != null && (uploadECDataModel.EC_Data.ContentLength > 0))
                    {
                        var extension = Path.GetExtension(uploadECDataModel.EC_Data.FileName);
                        uploadECDataModel.EC_Data.SaveAs(Server.MapPath("../AppFiles/EC_Files/") + "HSC_EC_Data_Remove" + extension);
                    }
                    InitializeOledbConnection(Server.MapPath("../AppFiles/EC_Files/HSC_EC_Data_Remove.xls"), ".xls");
                    DataTable tempTable = ReadFile();
                    if (tempTable.Rows.Count > 0)
                    {
                        foreach (DataRow row in tempTable.Rows)
                        {
                            if (row["EC_Status"] != null && row["EC_Status"].ToString() != "" && row["S_ID"] != null && row["S_ID"].ToString() != "" && row["S_ID"].ToString().Length == 7)
                            {
                                Tbl_HSC_Form17_Final tbl_HSC_Form17_Final = new Tbl_HSC_Form17_Final();
                                var temp = row["S_ID"];
                                tbl_HSC_Form17_Final = db.Tbl_HSC_Form17_Final.Where(x => x.S_ID == temp.ToString()).FirstOrDefault();
                                if (tbl_HSC_Form17_Final != null)
                                {
                                    if (tbl_HSC_Form17_Final.EC_Status == "1" && row["EC_Status"].ToString() == "2")
                                    {
                                        tbl_HSC_Form17_Final.EC_Status = null;
                                        tbl_HSC_Form17_Final.EC_Code = null;
                                        tbl_HSC_Form17_Final.EC_Date = null;
                                        db.Tbl_HSC_Form17_Final.Attach(tbl_HSC_Form17_Final);
                                        db.Entry(tbl_HSC_Form17_Final).Property(x => x.EC_Code).IsModified = true;
                                        db.Entry(tbl_HSC_Form17_Final).Property(x => x.EC_Status).IsModified = true;
                                        db.Entry(tbl_HSC_Form17_Final).Property(x => x.EC_Date).IsModified = true;
                                        db.SaveChanges();
                                    }
                                }
                            }
                        }
                    }
                    return Json(new { Result = true, Message = "File uploaded successfully !" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Result = false, Message = "Please Select Standard" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = false, Message = ex.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
        [HandleError]
        public ActionResult StateView()
        {
            return View();
        }

        public ActionResult DownloadData()
        {
            return View();
        }

        void InitializeOledbConnection(string filename, string extrn)
        {
            string connString = "";

            if (extrn == ".xls")
                //Connectionstring for excel v8.0    
                connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filename + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=1\"";
            else
                //Connectionstring fo excel v12.0    
                connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filename + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=1\"";
            OledbConn = new OleDbConnection(connString);
        }
        public DataTable ReadFile()
        {
            try
            {
                DataTable schemaTable = new DataTable();
                OledbCmd = new OleDbCommand();
                OledbCmd.Connection = OledbConn;
                OledbConn.Open();
                OledbCmd.CommandText = "Select * from [Sheet1$]";
                OleDbDataReader dr = OledbCmd.ExecuteReader();
                DataTable ContentTable = null;
                if (dr.HasRows)
                {
                    ContentTable = new DataTable();
                    ContentTable.Columns.Add("S_ID", typeof(string));
                    ContentTable.Columns.Add("EC_Status", typeof(string));
                    while (dr.Read())
                    {
                        if (dr[0].ToString().Trim() != string.Empty && dr[1].ToString().Trim() != string.Empty && dr[0].ToString().Trim() != " " && dr[1].ToString().Trim() != " ")
                            ContentTable.Rows.Add(dr[0].ToString().Trim(), dr[1].ToString().Trim());
                    }
                }
                dr.Close();
                OledbConn.Close();
                return ContentTable;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        [HandleError]
        [HttpPost]
        public JsonResult StateView(Tbl_Site_Status tbl_Site_Status)
        {
            Tbl_Site_Status oldTable = new Tbl_Site_Status();
            oldTable = (from row in db.Tbl_Site_Status select row).FirstOrDefault();
            oldTable.Active_Status = tbl_Site_Status.Active_Status;
            oldTable.Late_Fee_Date = tbl_Site_Status.Late_Fee_Date;
            oldTable.Super_Late_Fee_Date = tbl_Site_Status.Super_Late_Fee_Date;
            string folder1 = Server.MapPath(string.Format("~/AppFiles/CenterList/HSC/"));
            var extension1 = Path.GetExtension(tbl_Site_Status.HSC_CenterList.FileName);
            if (!Directory.Exists(folder1))
            {
                Directory.CreateDirectory(folder1);
            }
            tbl_Site_Status.HSC_CenterList.SaveAs(Server.MapPath("../AppFiles/CenterList/HSC/") + "HSC_CenterList" + extension1);
            string folder2 = Server.MapPath(string.Format("~/AppFiles/CenterList/SSC/"));
            var extension2 = Path.GetExtension(tbl_Site_Status.SSC_CenterList.FileName);
            if (!Directory.Exists(folder2))
            {
                Directory.CreateDirectory(folder2);
            }
            tbl_Site_Status.SSC_CenterList.SaveAs(Server.MapPath("../AppFiles/CenterList/SSC/") + "SSC_CenterList" + extension2);
            db.SaveChanges();
            return Json(new { oldTable, Result = true, Message = "Data saved successfully !" }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult getSummary()
        {
            var recentTable = (from row in db.Tbl_Site_Status select row).FirstOrDefault();
            return Json(new { recentTable }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult ExportToCSVStateLevel()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ExportToCSVStateLevel(DownloadStateDataModel downloadStateDataModel)
        {
            if (downloadStateDataModel.STD == "HSC")
            {
                DataTable dt = dataAccessLayer.ReturnDataTable(@"select ID,	S_ID,	Ref_ID,	Last_Name,	First_Name,	Father_Husband_Name,	Mother_Name,	Gender,	Pincode,	District,	Taluka,	State,	Aadhar_No,	Mobile_No,	Email_ID,	DOB,	Village_of_Birth,	Taluka_of_Birth,	District_of_Birth,	SSC_Passing_Month,	SSC_Passing_Year,	SSC_Division_Board,	Other_SSC_Board,	Date_of_Leaving_Secondary_School,	Date_of_Leaving_Junior_College,	Secondary_School_Village,	Secondary_School_Taluka,	Secondary_School_District,	Secondary_School_State,	Secondary_School_Index_No,	Secondary_School_Udise_No,	Attended_Junior_College_Yes_No,	Junior_College_Village,	Junior_College_District,	Junior_College_State,	Junior_College_Index_No,	Junior_College_Udise_No,	Junior_College_Stream,	FYJC_Passing_Month,	FYJC_Passing_Year,	FYJC_Passing_Status,	Division,	District_of_Form_Submission,	Taluka_of_Form_Submission,	Stream_of_Form,	Medium_of_Form,	College_of_Form_Submission,	College_Index_No,	Declaration_Yes_No,	Name_of_Debarred_Board,	Exam_of_Debar,	Debarred_From_Year,	Debarred_To_Year,	Secondary_School_Certificate_Yes_No,	FYJC_Leaving_Certificate_Yes_No,	School_Leaving_Certificate_Yes_No,	School_Leaving_Duplicate_Certificate_Yes_No,	Unique_ID_Payment,	Selected_Language,	EC_Status,	EC_Code,	EC_Date,	Date_Time from Tbl_HSC_Form17_Final where Convert(date ,Date_Time) >= convert(varchar, convert(date, '" + downloadStateDataModel.Start_Date + "', 103), 120)  and  Convert(date ,Date_Time) <= convert(varchar, convert(date, '" + downloadStateDataModel.End_Date + "', 103), 120) and EC_Status = '1'");          
                string fileName = DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
                string fullpath = Path.Combine(Server.MapPath("~/State/CSV"), fileName + ".csv");        
                ToCSV(dt, fullpath);
                byte[] fileByteArray = System.IO.File.ReadAllBytes(fullpath);
                System.IO.File.Delete(fullpath);
                return File(fileByteArray, "text/csv", fileName + ".csv");
            }
            if (downloadStateDataModel.STD == "SSC")
            {
                //var sscstudentlist = db.Database.SqlQuery<Tbl_SSC_Form17_Final>("select ID,	S_ID,	Ref_ID,	Last_Name,	First_Name,	Father_Husband_Name,	Mother_Name, Gender,	Pincode, District, Taluka,	State,	Aadhar_No,	Mobile_No,	Email_ID,	DOB,	Village_of_Birth,	Taluka_of_Birth,	District_of_Birth,	Name_of_Secondary_School,	Secondary_School_Village,	Secondary_School_Taluka,	Secondary_School_District,	Secondary_School_State,	Secondary_School_Udise_No,	Secondary_School_Pincode,	Whether_Handicap,	Handicap_Category,	Date_of_Leaving_Secondary_School,	Last_Studied_Standard,	Status_of_Last_Studied_Standard,	Division,	District_of_Form_Submission,	Taluka_of_Form_Submission,	Medium_of_Form,	School_of_Form_Submission,	Index_No_of_School,	Declaration_Yes_No,	Name_of_Debarred_Board,	Exam_of_Debar,	Debarred_From_Year,	Debarred_To_Year,	Leaving_Certificate_Yes_No,	Duplicate_Leaving_Certificate_Yes_No,	Unique_ID_Payment,	Selected_Language,	EC_Status,	EC_Code,	EC_Date,	Date_Time from Tbl_SSC_Form17_Final where Convert(date ,Date_Time) >= convert(varchar, convert(date, '" + downloadStateDataModel.Start_Date + "', 103), 120)  and  Convert(date ,Date_Time) <= convert(varchar, convert(date, '" + downloadStateDataModel.End_Date + "', 103), 120) and EC_Status = '1'").ToList();

                //DataTable dt = ToDataTable(sscstudentlist);
                DataTable dt = dataAccessLayer.ReturnDataTable(@"select ID,	S_ID,	Ref_ID,	Last_Name,	First_Name,	Father_Husband_Name,	Mother_Name, Gender,	Pincode, District, Taluka,	State,	Aadhar_No,	Mobile_No,	Email_ID,	DOB,	Village_of_Birth,	Taluka_of_Birth,	District_of_Birth,	Secondary_School_Village,	Secondary_School_Taluka,	Secondary_School_District,	Secondary_School_State,	Secondary_School_Udise_No,	Secondary_School_Pincode,	Whether_Handicap,	Handicap_Category,	Date_of_Leaving_Secondary_School,	Last_Studied_Standard,	Status_of_Last_Studied_Standard,	Division,	District_of_Form_Submission,	Taluka_of_Form_Submission,	Medium_of_Form,	School_of_Form_Submission,	Index_No_of_School,	Declaration_Yes_No,	Name_of_Debarred_Board,	Exam_of_Debar,	Debarred_From_Year,	Debarred_To_Year,	Leaving_Certificate_Yes_No,	Duplicate_Leaving_Certificate_Yes_No,	Unique_ID_Payment,	Selected_Language,	EC_Status,	EC_Code,	EC_Date,	Date_Time from Tbl_SSC_Form17_Final where Convert(date ,Date_Time) >= convert(varchar, convert(date, '" + downloadStateDataModel.Start_Date + "', 103), 120)  and  Convert(date ,Date_Time) <= convert(varchar, convert(date, '" + downloadStateDataModel.End_Date + "', 103), 120) and EC_Status = '1'");
                string fileName = DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
                string fullpath = Path.Combine(Server.MapPath("~/State/CSV"), fileName + ".csv");
                ToCSV(dt, fullpath);
                byte[] fileByteArray = System.IO.File.ReadAllBytes(fullpath);
                System.IO.File.Delete(fullpath);
                return File(fileByteArray, "text/csv", fileName + ".csv");               
            }
            
            return View();
        }
        public ActionResult ExportToExcelStateLevel(DownloadStateDataModel downloadStateDataModel)
        {
            if (downloadStateDataModel.STD == "HSC")
            {
                List<Tbl_HSC_Form17_Final> sscstudentlist = db.Database.SqlQuery<Tbl_HSC_Form17_Final>("select * from Tbl_HSC_Form17_Final where Convert(date ,Date_Time) >= convert(varchar, convert(date, '" + downloadStateDataModel.Start_Date + "', 103), 120)  and  Convert(date ,Date_Time) <= convert(varchar, convert(date, '" + downloadStateDataModel.End_Date + "', 103), 120)").ToList();
                DataTable dt = ToDataTable(sscstudentlist);
                DataSet ds = new DataSet();
                ds.Tables.Add(dt);
                ds.Tables[0].TableName = "HSC";
                string fileName = DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
                CreateExcelFile(ds, fileName);
                string fullPath = Path.Combine(Server.MapPath("~/State"), fileName + ".xls");
                byte[] fileByteArray = System.IO.File.ReadAllBytes(fullPath);
                System.IO.File.Delete(fullPath);
                return File(fileByteArray, "application/vnd.ms-excel", fileName + ".xls");
            }
            if (downloadStateDataModel.STD == "SSC")
            {
                List<Tbl_SSC_Form17_Final> sscstudentlist = db.Database.SqlQuery<Tbl_SSC_Form17_Final>("select * from Tbl_SSC_Form17_Final where Convert(date ,Date_Time) >= convert(varchar, convert(date, '" + downloadStateDataModel.Start_Date + "', 103), 120)  and  Convert(date ,Date_Time) <= convert(varchar, convert(date, '" + downloadStateDataModel.End_Date + "', 103), 120)").ToList();
                DataTable dt = ToDataTable(sscstudentlist);
                DataSet ds = new DataSet();
                ds.Tables.Add(dt);
                ds.Tables[0].TableName = "SSC";
                string fileName = DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString();
                CreateExcelFile(ds, fileName);
                string fullPath = Path.Combine(Server.MapPath("~/State"), fileName + ".xls");
                byte[] fileByteArray = System.IO.File.ReadAllBytes(fullPath);
                System.IO.File.Delete(fullPath);
                return File(fileByteArray, "application/vnd.ms-excel", fileName + ".xls");
            }
            return View();
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

        void CreateExcelFile(DataSet ds1, string filename)
        {
            try
            {
                using (DataSet ds = ds1)
                {
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        string rootFolder = Server.MapPath("").ToString();
                        string fileName = @"" + filename + ".xls";

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
        public void ToCSV(DataTable dtDataTable, string strFilePath)
        {
            try
            {
                StreamWriter sw = new StreamWriter(strFilePath, false);
                //headers    
                for (int i = 0; i < dtDataTable.Columns.Count; i++)
                {
                    sw.Write(dtDataTable.Columns[i]);
                    if (i < dtDataTable.Columns.Count - 1)
                    {
                        sw.Write(",");
                    }
                }
                sw.Write(sw.NewLine);
                foreach (DataRow dr in dtDataTable.Rows)
                {
                    for (int i = 0; i < dtDataTable.Columns.Count; i++)
                    {
                        if (!Convert.IsDBNull(dr[i]))
                        {
                            string value = dr[i].ToString();
                            if (value.Contains(','))
                            {
                                //  value = String.Format("\"{0}\"", value);
                                value = value.Replace(",", ".");
                                sw.Write(value);
                            }
                            else
                            {
                                sw.Write(dr[i].ToString());
                            }
                        }
                        if (i < dtDataTable.Columns.Count - 1)
                        {
                            sw.Write(",");
                        }
                    }
                    sw.Write(sw.NewLine);
                }
                sw.Close();
            }
            catch (Exception exe)
            {

            }
        }

    }
}