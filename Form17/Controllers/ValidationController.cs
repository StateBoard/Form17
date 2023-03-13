using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Form17.Models;

namespace Form17.Controllers
{
    public class ValidationController : Controller
    {
        // GET: HSCForm17Validation
        Form17Entities db = new Form17Entities();
        //DateTime DefaultSchoolLeavingDate = Convert.ToDateTime(System.Configuration.ConfigurationManager.AppSettings["SchoolLeavingDate"].ToString());
        string DefaultSchoolLeavingDate = "31/08/2021";
        public JsonResult HSCForm17Validation(Tbl_HSC_Form17 tbl_HSC_Form17)
        {
            try
            {
                if (tbl_HSC_Form17.Email_ID == null || tbl_HSC_Form17.Email_ID.Trim() == "" || !IsValidEmailAddress(tbl_HSC_Form17.Email_ID.Trim()))
                { return Json(new { Result = false, Message = "Please enter valid Email ID" }, JsonRequestBehavior.AllowGet); }

                var IsAlreadyRegistered = db.Tbl_HSC_Form17_Final.Where(x => x.Email_ID.Trim().ToUpper() == tbl_HSC_Form17.Email_ID.Trim().ToUpper()).FirstOrDefault();
                if(IsAlreadyRegistered != null)
                {
                    return Json(new { Result = false, Message = IsAlreadyRegistered.Email_ID.Trim()+" - is already registered !" }, JsonRequestBehavior.AllowGet);
                }
                if (IsSchoolLeavingDateGreater(tbl_HSC_Form17.Date_of_Leaving_Secondary_School.ToString().Substring(0, 10), DefaultSchoolLeavingDate))
                {
                    return Json(new { Result = false, Message = "Your date of school leaving must be on or before 30 Aug 2021" }, JsonRequestBehavior.AllowGet);
                }

                //if (tbl_HSC_Form17.Last_Name == null || tbl_HSC_Form17.Last_Name.Trim() == "" || containsNonAlphabets(tbl_HSC_Form17.Last_Name.Trim()))
                //{ return Json(new { Result = false, Message = "Please enter valid Last Name" }, JsonRequestBehavior.AllowGet); }
                if (tbl_HSC_Form17.First_Name == null || tbl_HSC_Form17.First_Name.Trim() == "" || containsNonAlphabets(tbl_HSC_Form17.First_Name.Trim()))
                { return Json(new { Result = false, Message = "Please enter valid First Name" }, JsonRequestBehavior.AllowGet); }
                if (tbl_HSC_Form17.Father_Husband_Name == null || tbl_HSC_Form17.Father_Husband_Name.Trim() == "" || containsNonAlphabets(tbl_HSC_Form17.Father_Husband_Name.Trim()))
                { return Json(new { Result = false, Message = "Please enter valid Father/Husband Name" }, JsonRequestBehavior.AllowGet); }
                if (tbl_HSC_Form17.Mother_Name == null || tbl_HSC_Form17.Mother_Name.Trim() == "" || containsNonAlphabets(tbl_HSC_Form17.Mother_Name.Trim()))
                { return Json(new { Result = false, Message = "Please enter valid Mother Name" }, JsonRequestBehavior.AllowGet); }
                if (tbl_HSC_Form17.Gender == null || tbl_HSC_Form17.Gender.Trim() == "0")
                { return Json(new { Result = false, Message = "Please Select Gender" }, JsonRequestBehavior.AllowGet); }
                if (tbl_HSC_Form17.Residential_Address == null || tbl_HSC_Form17.Residential_Address.Trim() == "")
                { return Json(new { Result = false, Message = "Please enter Residential Address" }, JsonRequestBehavior.AllowGet); }
                if (tbl_HSC_Form17.Pincode == null || tbl_HSC_Form17.Pincode.Trim() == "" || containsNonDigits(tbl_HSC_Form17.Pincode.Trim()) || tbl_HSC_Form17.Pincode.Trim().Length != 6)
                { return Json(new { Result = false, Message = "Please enter valid Pincode" }, JsonRequestBehavior.AllowGet); }
                if (tbl_HSC_Form17.District == null || tbl_HSC_Form17.District.Trim() == "0")
                { return Json(new { Result = false, Message = "Please Select District" }, JsonRequestBehavior.AllowGet); }
                if (tbl_HSC_Form17.Taluka == null || tbl_HSC_Form17.Taluka.Trim() == "0")
                { return Json(new { Result = false, Message = "Please Select Taluka" }, JsonRequestBehavior.AllowGet); }
                if (tbl_HSC_Form17.State == null || tbl_HSC_Form17.State.Trim() == "0")
                { return Json(new { Result = false, Message = "Please Select State" }, JsonRequestBehavior.AllowGet); }
                if (tbl_HSC_Form17.Aadhar_No == null || tbl_HSC_Form17.Aadhar_No.Trim() == "" || containsNonDigits(tbl_HSC_Form17.Aadhar_No.Trim()) || tbl_HSC_Form17.Aadhar_No.Trim().Length != 12)
                { return Json(new { Result = false, Message = "Please enter valid Aadhar No" }, JsonRequestBehavior.AllowGet); }
                if (tbl_HSC_Form17.Mobile_No == null || tbl_HSC_Form17.Mobile_No.Trim() == "" || containsNonDigits(tbl_HSC_Form17.Mobile_No.Trim()) || tbl_HSC_Form17.Mobile_No.Trim().Length != 10)
                { return Json(new { Result = false, Message = "Please enter valid Mobile No" }, JsonRequestBehavior.AllowGet); }               
                if (tbl_HSC_Form17.DOB == null)
                { return Json(new { Result = false, Message = "Please Enter Date of Birth" }, JsonRequestBehavior.AllowGet); }
                if (tbl_HSC_Form17.Village_of_Birth == null || tbl_HSC_Form17.Village_of_Birth.Trim() == "")
                { return Json(new { Result = false, Message = "Please enter  Village of Birth" }, JsonRequestBehavior.AllowGet); }
                if (tbl_HSC_Form17.Taluka_of_Birth == null || tbl_HSC_Form17.Taluka_of_Birth.Trim() == "")
                { return Json(new { Result = false, Message = "Please enter Taluka of Birth" }, JsonRequestBehavior.AllowGet); }
                if (tbl_HSC_Form17.District_of_Birth == null || tbl_HSC_Form17.District_of_Birth.Trim() == "")
                { return Json(new { Result = false, Message = "Please enter District of Birth" }, JsonRequestBehavior.AllowGet); }
                if (tbl_HSC_Form17.SSC_Passing_Month == null || tbl_HSC_Form17.SSC_Passing_Month.Trim() == "0")
                { return Json(new { Result = false, Message = "Please Select SSC Passing Month" }, JsonRequestBehavior.AllowGet); }
                if (tbl_HSC_Form17.SSC_Passing_Year == null || tbl_HSC_Form17.SSC_Passing_Year.Trim() == "0")
                { return Json(new { Result = false, Message = "Please Select SSC Passing Year" }, JsonRequestBehavior.AllowGet); }
                if (tbl_HSC_Form17.SSC_Division_Board == null || tbl_HSC_Form17.SSC_Division_Board.Trim() == "0")
                { return Json(new { Result = false, Message = "Please Select SSC Division Board" }, JsonRequestBehavior.AllowGet); }
                if (tbl_HSC_Form17.SSC_Division_Board == "OTHER" && (tbl_HSC_Form17.Other_SSC_Board == null || tbl_HSC_Form17.Other_SSC_Board.Trim() == ""))
                { return Json(new { Result = false, Message = "Please Enter Name of other Board" }, JsonRequestBehavior.AllowGet); }
                if (tbl_HSC_Form17.Date_of_Leaving_Secondary_School == null)
                { return Json(new { Result = false, Message = "Please Select Date of Leaving Secondary School" }, JsonRequestBehavior.AllowGet); }
                if (tbl_HSC_Form17.Attended_Junior_College_Yes_No.Trim() == "Yes" && (tbl_HSC_Form17.Date_of_Leaving_Junior_College == null))
                { return Json(new { Result = false, Message = "Please Select Date of Leaving Junior College" }, JsonRequestBehavior.AllowGet); }
                if (tbl_HSC_Form17.Name_of_Secondary_School == null || tbl_HSC_Form17.Name_of_Secondary_School.Trim() == "")
                { return Json(new { Result = false, Message = "Please enter Name of Secondary School" }, JsonRequestBehavior.AllowGet); }
                if (tbl_HSC_Form17.Secondary_School_Village == null || tbl_HSC_Form17.Secondary_School_Village.Trim() == "")
                { return Json(new { Result = false, Message = "Please enter Village/Town of Secondary School" }, JsonRequestBehavior.AllowGet); }
                if (tbl_HSC_Form17.Secondary_School_Taluka == null || tbl_HSC_Form17.Secondary_School_Taluka.Trim() == "")
                { return Json(new { Result = false, Message = "Please enter Taluka of Secondary School" }, JsonRequestBehavior.AllowGet); }
                if (tbl_HSC_Form17.Secondary_School_District == null || tbl_HSC_Form17.Secondary_School_District.Trim() == "")
                { return Json(new { Result = false, Message = "Please enter District of Secondary School" }, JsonRequestBehavior.AllowGet); }
                if (tbl_HSC_Form17.Secondary_School_State == null || tbl_HSC_Form17.Secondary_School_State.Trim() == "")
                { return Json(new { Result = false, Message = "Please enter State of Secondary School" }, JsonRequestBehavior.AllowGet); }
                if (tbl_HSC_Form17.Secondary_School_Index_No != null && tbl_HSC_Form17.Secondary_School_Index_No.Trim() != "" && (tbl_HSC_Form17.Secondary_School_Index_No.Trim().Length != 7 || containsNonDigits(tbl_HSC_Form17.Secondary_School_Index_No.Trim())))
                { return Json(new { Result = false, Message = "Please enter valid Index No of Secondary School" }, JsonRequestBehavior.AllowGet); }
                if (tbl_HSC_Form17.Secondary_School_Udise_No != null && tbl_HSC_Form17.Secondary_School_Udise_No.Trim() != "" && (tbl_HSC_Form17.Secondary_School_Udise_No.Trim().Length != 11 || containsNonDigits(tbl_HSC_Form17.Secondary_School_Udise_No.Trim())))
                { return Json(new { Result = false, Message = "Please enter valid Udise No Secondary School" }, JsonRequestBehavior.AllowGet); }
                if (tbl_HSC_Form17.Attended_Junior_College_Yes_No == null || tbl_HSC_Form17.Attended_Junior_College_Yes_No.Trim() == "0")
                { return Json(new { Result = false, Message = "Please select - Have you attended junior college ?" }, JsonRequestBehavior.AllowGet); }
                if (tbl_HSC_Form17.Attended_Junior_College_Yes_No.Trim() == "Yes" && (tbl_HSC_Form17.Name_of_Junior_College == null || tbl_HSC_Form17.Name_of_Junior_College.Trim() == ""))
                { return Json(new { Result = false, Message = "Please enter Name of Junior College" }, JsonRequestBehavior.AllowGet); }
                if (tbl_HSC_Form17.Attended_Junior_College_Yes_No.Trim() == "Yes" && (tbl_HSC_Form17.Junior_College_Village == null || tbl_HSC_Form17.Junior_College_Village.Trim() == ""))
                { return Json(new { Result = false, Message = "Please enter Village/Town of Junior College Village" }, JsonRequestBehavior.AllowGet); }
                if (tbl_HSC_Form17.Attended_Junior_College_Yes_No.Trim() == "Yes" && (tbl_HSC_Form17.Junior_College_District == null || tbl_HSC_Form17.Junior_College_District.Trim() == ""))
                { return Json(new { Result = false, Message = "Please enter District of Junior College" }, JsonRequestBehavior.AllowGet); }
                if (tbl_HSC_Form17.Attended_Junior_College_Yes_No.Trim() == "Yes" && (tbl_HSC_Form17.Junior_College_State == null || tbl_HSC_Form17.Junior_College_State.Trim() == ""))
                { return Json(new { Result = false, Message = "Please enter State of Junior College" }, JsonRequestBehavior.AllowGet); }
                if (tbl_HSC_Form17.Attended_Junior_College_Yes_No.Trim() == "Yes" && (tbl_HSC_Form17.Junior_College_Index_No != null && tbl_HSC_Form17.Junior_College_Index_No.Trim() != "" && (tbl_HSC_Form17.Junior_College_Index_No.Trim().Length != 7 || containsNonDigits(tbl_HSC_Form17.Junior_College_Index_No.Trim()))))
                { return Json(new { Result = false, Message = "Please enter valid Index No of Junior College" }, JsonRequestBehavior.AllowGet); }
                if (tbl_HSC_Form17.Attended_Junior_College_Yes_No.Trim() == "Yes" && (tbl_HSC_Form17.Junior_College_Udise_No != null && tbl_HSC_Form17.Junior_College_Udise_No.Trim() != "" && (tbl_HSC_Form17.Junior_College_Udise_No.Trim().Length != 11 || containsNonDigits(tbl_HSC_Form17.Junior_College_Udise_No.Trim()))))
                { return Json(new { Result = false, Message = "Please enter valid Udise No of Junior College" }, JsonRequestBehavior.AllowGet); }
                if (tbl_HSC_Form17.Attended_Junior_College_Yes_No.Trim() == "Yes" && (tbl_HSC_Form17.Junior_College_Stream == null || tbl_HSC_Form17.Junior_College_Stream.Trim() == "0"))
                { return Json(new { Result = false, Message = "Please enter Junior College Stream" }, JsonRequestBehavior.AllowGet); }
                if (tbl_HSC_Form17.Attended_Junior_College_Yes_No.Trim() == "Yes" && (tbl_HSC_Form17.FYJC_Passing_Month == null || tbl_HSC_Form17.FYJC_Passing_Month.Trim() == "0"))
                { return Json(new { Result = false, Message = "Please select Passing Month of Junior College" }, JsonRequestBehavior.AllowGet); }
                if (tbl_HSC_Form17.Attended_Junior_College_Yes_No.Trim() == "Yes" && (tbl_HSC_Form17.FYJC_Passing_Year == null || tbl_HSC_Form17.FYJC_Passing_Year.Trim() == "0"))
                { return Json(new { Result = false, Message = "Please enter FYJC Passing Year" }, JsonRequestBehavior.AllowGet); }
                if (tbl_HSC_Form17.Attended_Junior_College_Yes_No.Trim() == "Yes" && (tbl_HSC_Form17.FYJC_Passing_Status == null || tbl_HSC_Form17.FYJC_Passing_Status.Trim() == "0"))
                { return Json(new { Result = false, Message = "Please enter FYJC Passing Status" }, JsonRequestBehavior.AllowGet); }
                if (tbl_HSC_Form17.District_of_Form_Submission == null || tbl_HSC_Form17.District_of_Form_Submission.Trim() == "0")
                { return Json(new { Result = false, Message = "Please Select District of Form Submission" }, JsonRequestBehavior.AllowGet); }
                if (tbl_HSC_Form17.Taluka_of_Form_Submission == null || tbl_HSC_Form17.Taluka_of_Form_Submission.Trim() == "0")
                { return Json(new { Result = false, Message = "Please Select Taluka of Form Submission" }, JsonRequestBehavior.AllowGet); }
                if (tbl_HSC_Form17.Stream_of_Form == null || tbl_HSC_Form17.Stream_of_Form.Trim() == "0")
                { return Json(new { Result = false, Message = "Please Select Stream of Form" }, JsonRequestBehavior.AllowGet); }
                if (tbl_HSC_Form17.Medium_of_Form == null || tbl_HSC_Form17.Medium_of_Form.Trim() == "0")
                { return Json(new { Result = false, Message = "Please Select Medium of Form" }, JsonRequestBehavior.AllowGet); }
                if (tbl_HSC_Form17.College_of_Form_Submission == null || tbl_HSC_Form17.College_of_Form_Submission.Trim() == "0")
                { return Json(new { Result = false, Message = "Please Select College of Form Submission" }, JsonRequestBehavior.AllowGet); }
                if (tbl_HSC_Form17.College_Index_No == null || tbl_HSC_Form17.College_Index_No.Trim() == "")
                { return Json(new { Result = false, Message = "Please enter valid College_Index_No" }, JsonRequestBehavior.AllowGet); }
                if (tbl_HSC_Form17.Declaration_Yes_No == null || tbl_HSC_Form17.Declaration_Yes_No.Trim() == "0")
                { return Json(new { Result = false, Message = "Please Select option in point no 11" }, JsonRequestBehavior.AllowGet); }
                if (tbl_HSC_Form17.Declaration_Yes_No.Trim() == "Yes" && (tbl_HSC_Form17.Name_of_Debarred_Board == null || tbl_HSC_Form17.Name_of_Debarred_Board.Trim() == ""))
                { return Json(new { Result = false, Message = "Please enter Name of Debarred Board" }, JsonRequestBehavior.AllowGet); }
                if (tbl_HSC_Form17.Declaration_Yes_No.Trim() == "Yes" && (tbl_HSC_Form17.Exam_of_Debar == null || tbl_HSC_Form17.Exam_of_Debar.Trim() == ""))
                { return Json(new { Result = false, Message = "Please enter Name of Exam of Debar" }, JsonRequestBehavior.AllowGet); }
                if (tbl_HSC_Form17.Declaration_Yes_No.Trim() == "Yes" && (tbl_HSC_Form17.Debarred_From_Year == null || tbl_HSC_Form17.Debarred_From_Year.Trim() == "" || containsNonDigits(tbl_HSC_Form17.Debarred_From_Year.Trim()) || tbl_HSC_Form17.Debarred_From_Year.Trim().Length != 4))
                { return Json(new { Result = false, Message = "Please enter valid 'Debarred From' Year" }, JsonRequestBehavior.AllowGet); }
                if (tbl_HSC_Form17.Declaration_Yes_No.Trim() == "Yes" && (tbl_HSC_Form17.Debarred_To_Year == null || tbl_HSC_Form17.Debarred_To_Year.Trim() == "" || containsNonDigits(tbl_HSC_Form17.Debarred_To_Year.Trim()) || tbl_HSC_Form17.Debarred_To_Year.Trim().Length != 4))
                { return Json(new { Result = false, Message = "Please enter valid 'Debarred To' Year" }, JsonRequestBehavior.AllowGet); }
                if (tbl_HSC_Form17.Secondary_School_Certificate_Yes_No == null || tbl_HSC_Form17.Secondary_School_Certificate_Yes_No.Trim() != "Yes")
                { return Json(new { Result = false, Message = "Please enclose secondary school marksheet and certificate" }, JsonRequestBehavior.AllowGet); }
                if (tbl_HSC_Form17.FYJC_Leaving_Certificate_Yes_No == null || tbl_HSC_Form17.FYJC_Leaving_Certificate_Yes_No.Trim() == "0")
                { return Json(new { Result = false, Message = "Please Select Yes/No Option of 'Point 12 - (B)'" }, JsonRequestBehavior.AllowGet); }
                if (tbl_HSC_Form17.School_Leaving_Certificate_Yes_No == null || tbl_HSC_Form17.School_Leaving_Certificate_Yes_No.Trim() == "0")
                { return Json(new { Result = false, Message = "Please enclose school leaving certificate" }, JsonRequestBehavior.AllowGet); }
                if (tbl_HSC_Form17.School_Leaving_Certificate_Yes_No.Trim() == "No" && (tbl_HSC_Form17.School_Leaving_Duplicate_Certificate_Yes_No == null || tbl_HSC_Form17.School_Leaving_Duplicate_Certificate_Yes_No.Trim() == "0"))
                { return Json(new { Result = false, Message = "Please Select Yes/No Option of 'Point 12 - (D)'" }, JsonRequestBehavior.AllowGet); }
                if (tbl_HSC_Form17.School_Leaving_Certificate_Yes_No.Trim() == "No" && tbl_HSC_Form17.School_Leaving_Duplicate_Certificate_Yes_No.Trim() == "No")
                { return Json(new { Result = false, Message = "Please enclose either original school leaving certificate or duplicate school leaving certificate" }, JsonRequestBehavior.AllowGet); }
                if (tbl_HSC_Form17.Selected_Language == null || tbl_HSC_Form17.Selected_Language.Trim() == "0")
                { return Json(new { Result = false, Message = "Please Select Medium (Language) Option after 'Point 12'" }, JsonRequestBehavior.AllowGet); }

                if (tbl_HSC_Form17.HSC_Photo == null)
                { return Json(new { Result = false, Message = "Please Upload photo" }, JsonRequestBehavior.AllowGet); }
                if (tbl_HSC_Form17.HSC_Photo != null)
                {
                    string photoextension = Path.GetExtension(tbl_HSC_Form17.HSC_Photo.FileName);
                    if (photoextension != ".jpeg" && photoextension != ".jpg" && photoextension != ".png" && photoextension != ".JPEG" && photoextension != ".JPG" && photoextension != ".PNG")
                    { 
                        return Json(new { Result = false, Message = "photo file type should be 'jpeg/jpg/png' only." }, JsonRequestBehavior.AllowGet); 
                    }
                }
                if (tbl_HSC_Form17.HSC_Photo.ContentLength == 0)
                { return Json(new { Result = false, Message = "Your Passport Photo's file is empty" }, JsonRequestBehavior.AllowGet); }
                if (tbl_HSC_Form17.HSC_Photo.ContentLength > 204800)
                { return Json(new { Result = false, Message = "Your Passport Photo's file size exceeding 200 KB" }, JsonRequestBehavior.AllowGet); }

                if (tbl_HSC_Form17.HSC_School_LC == null)
                { return Json(new { Result = false, Message = "Please upload LC/TC" }, JsonRequestBehavior.AllowGet); }

                if (tbl_HSC_Form17.HSC_School_LC != null)
                {
                    string LCextension = Path.GetExtension(tbl_HSC_Form17.HSC_School_LC.FileName);
                    if (LCextension != ".jpeg" && LCextension != ".jpg" && LCextension != ".png" && LCextension != ".pdf" && LCextension != ".JPEG" && LCextension != ".JPG" && LCextension != ".PNG" && LCextension != ".PDF")
                    {
                        return Json(new { Result = false, Message = "LC/TC file type should be 'jpeg/jpg/png/pdf' only." }, JsonRequestBehavior.AllowGet);
                    }
                }
                if (tbl_HSC_Form17.HSC_School_LC.ContentLength == 0)
                { return Json(new { Result = false, Message = "Your LC/TC's file is empty" }, JsonRequestBehavior.AllowGet); }
                if (tbl_HSC_Form17.HSC_School_LC.ContentLength > 204800)
                { return Json(new { Result = false, Message = "Your LC/TC's file size exceeding 200 KB" }, JsonRequestBehavior.AllowGet); }

                if (tbl_HSC_Form17.HSC_School_Marksheet == null)
                { return Json(new { Result = false, Message = "Please Upload Secondary School Markseet & Certificate " }, JsonRequestBehavior.AllowGet); }
                if (tbl_HSC_Form17.HSC_School_LC != null)
                {
                    string MarksheetExtension = Path.GetExtension(tbl_HSC_Form17.HSC_School_Marksheet.FileName);
                    if (MarksheetExtension != ".jpeg" && MarksheetExtension != ".jpg" && MarksheetExtension != ".png" && MarksheetExtension != ".pdf" && MarksheetExtension != ".JPEG" && MarksheetExtension != ".JPG" && MarksheetExtension != ".PNG" && MarksheetExtension != ".PDF")
                    { 
                        return Json(new { Result = false, Message = "School Marksheet/Certificate file type should be 'jpeg/jpg/png/pdf' only." }, JsonRequestBehavior.AllowGet); 
                    }
                }
                if (tbl_HSC_Form17.HSC_School_Marksheet.ContentLength == 0)
                { return Json(new { Result = false, Message = "Your Secondary School Marksheet & Certificate file is empty" }, JsonRequestBehavior.AllowGet); }
                if (tbl_HSC_Form17.HSC_School_Marksheet.ContentLength > 204800)
                { return Json(new { Result = false, Message = "Your Secondary School Marksheet & Certificate's file size exceeding 200 KB" }, JsonRequestBehavior.AllowGet); }

                if (tbl_HSC_Form17.FYJC_Leaving_Certificate_Yes_No.Trim() == "Yes" && tbl_HSC_Form17.JCLC == null)
                { return Json(new { Result = false, Message = "Please Upload Junior College Leaving Certificate." }, JsonRequestBehavior.AllowGet); }
                if (tbl_HSC_Form17.FYJC_Leaving_Certificate_Yes_No.Trim() == "Yes" && tbl_HSC_Form17.JCLC != null)
                {
                    string JCLCExtension = Path.GetExtension(tbl_HSC_Form17.JCLC.FileName);
                    if(JCLCExtension != ".jpeg" && JCLCExtension != ".jpg" && JCLCExtension != ".png" && JCLCExtension != ".pdf" && JCLCExtension != ".JPEG" && JCLCExtension != ".JPG" && JCLCExtension != ".PNG" && JCLCExtension != ".PDF")
                    {
                        return Json(new { Result = false, Message = "Junior College LC/TC file type should be 'jpeg/jpg/png/pdf' only." }, JsonRequestBehavior.AllowGet);
                    }                    
                }
                if (tbl_HSC_Form17.FYJC_Leaving_Certificate_Yes_No.Trim() == "Yes" && tbl_HSC_Form17.JCLC.ContentLength == 0)
                { return Json(new { Result = false, Message = "Your Junior College Leaving Certificate file is empty" }, JsonRequestBehavior.AllowGet); }
                if (tbl_HSC_Form17.FYJC_Leaving_Certificate_Yes_No.Trim() == "Yes" && tbl_HSC_Form17.JCLC.ContentLength > 204800)
                { return Json(new { Result = false, Message = "Your Junior College Leaving Ceritificate's file size exceeding 200 KB" }, JsonRequestBehavior.AllowGet); }              
                
                return Json(new { Result = true, Dialog = "Are you sure want to submit ?", DialogDescription = "Once form is submitted cannot be edited. By clicking 'Yes' you will be redirected to payment gateway." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Result = false, Message = "Sorry, something went wrong while processing your request !" }, JsonRequestBehavior.AllowGet);           
            }
            
        }

        public JsonResult SSCForm17Validation(Tbl_SSC_Form17 tbl_SSC_Form17)
        {
            try
            {
                if (tbl_SSC_Form17.Email_ID == null || tbl_SSC_Form17.Email_ID.Trim() == "" || !IsValidEmailAddress(tbl_SSC_Form17.Email_ID.Trim()))
                { return Json(new { Result = false, Message = "Please Enter valid Email ID" }, JsonRequestBehavior.AllowGet); }

                var IsAlreadyRegistered = db.Tbl_SSC_Form17_Final.Where(x => x.Email_ID.Trim().ToUpper() == tbl_SSC_Form17.Email_ID.Trim().ToUpper()).FirstOrDefault();
                if (IsAlreadyRegistered != null)
                {
                    return Json(new { Result = false, Message = IsAlreadyRegistered.Email_ID.Trim() + " - is already registered !" }, JsonRequestBehavior.AllowGet);
                }

                if (IsSchoolLeavingDateGreater(tbl_SSC_Form17.Date_of_Leaving_Secondary_School.ToString().Substring(0, 10), DefaultSchoolLeavingDate))
                {
                    return Json(new { Result = false, Message = "Your date of school leaving must be on or before 30 Aug 2021" }, JsonRequestBehavior.AllowGet);
                }

                //if (tbl_SSC_Form17.Last_Name == null || tbl_SSC_Form17.Last_Name.Trim() == "" || containsNonAlphabets(tbl_SSC_Form17.Last_Name.Trim()))
                //{ return Json(new { Result = false, Message = "Please Enter valid Last Name" }, JsonRequestBehavior.AllowGet); }
                if (tbl_SSC_Form17.First_Name == null || tbl_SSC_Form17.First_Name.Trim() == "" || containsNonAlphabets(tbl_SSC_Form17.First_Name.Trim()))
                { return Json(new { Result = false, Message = "Please Enter valid First Name" }, JsonRequestBehavior.AllowGet); }
                if (tbl_SSC_Form17.Father_Husband_Name == null || tbl_SSC_Form17.Father_Husband_Name.Trim() == "" || containsNonAlphabets(tbl_SSC_Form17.Father_Husband_Name.Trim()))
                { return Json(new { Result = false, Message = "Please Enter valid Fatehr/Husband Name" }, JsonRequestBehavior.AllowGet); }
                if (tbl_SSC_Form17.Mother_Name == null || tbl_SSC_Form17.Mother_Name.Trim() == "" || containsNonAlphabets(tbl_SSC_Form17.Mother_Name.Trim()))
                { return Json(new { Result = false, Message = "Please Enter valid Mother_Name" }, JsonRequestBehavior.AllowGet); }
                if (tbl_SSC_Form17.Gender == null || tbl_SSC_Form17.Gender.Trim() == "0")
                { return Json(new { Result = false, Message = "Please Select Gender" }, JsonRequestBehavior.AllowGet); }
                if (tbl_SSC_Form17.Residential_Address == null || tbl_SSC_Form17.Residential_Address.Trim() == "")
                { return Json(new { Result = false, Message = "Please Enter Residential Address" }, JsonRequestBehavior.AllowGet); }
                if (tbl_SSC_Form17.Pincode == null || tbl_SSC_Form17.Pincode.Trim() == "" || tbl_SSC_Form17.Pincode.Trim().Length != 6 || containsNonDigits(tbl_SSC_Form17.Pincode.Trim()))
                { return Json(new { Result = false, Message = "Please Enter valid Pincode" }, JsonRequestBehavior.AllowGet); }
                if (tbl_SSC_Form17.District == null || tbl_SSC_Form17.District.Trim() == "0")
                { return Json(new { Result = false, Message = "Please Select District" }, JsonRequestBehavior.AllowGet); }
                if (tbl_SSC_Form17.Taluka == null || tbl_SSC_Form17.Taluka.Trim() == "0")
                { return Json(new { Result = false, Message = "Please Select Taluka" }, JsonRequestBehavior.AllowGet); }
                if (tbl_SSC_Form17.State == null || tbl_SSC_Form17.State.Trim() == "0")
                { return Json(new { Result = false, Message = "Please Select State" }, JsonRequestBehavior.AllowGet); }
                if (tbl_SSC_Form17.Aadhar_No == null || tbl_SSC_Form17.Aadhar_No.Trim() == "" || containsNonDigits(tbl_SSC_Form17.Aadhar_No.Trim()) || tbl_SSC_Form17.Aadhar_No.Trim().Length != 12)
                { return Json(new { Result = false, Message = "Please Enter valid Aadhar No" }, JsonRequestBehavior.AllowGet); }
                if (tbl_SSC_Form17.Mobile_No == null || tbl_SSC_Form17.Mobile_No.Trim() == "" || containsNonDigits(tbl_SSC_Form17.Mobile_No.Trim()) || tbl_SSC_Form17.Mobile_No.Trim().Length != 10)
                { return Json(new { Result = false, Message = "Please Enter valid Mobile No" }, JsonRequestBehavior.AllowGet); }                
                if (tbl_SSC_Form17.DOB == null)
                { return Json(new { Result = false, Message = "Please Select Date of Birth" }, JsonRequestBehavior.AllowGet); }
                if (tbl_SSC_Form17.Village_of_Birth == null || tbl_SSC_Form17.Village_of_Birth.Trim() == "")
                { return Json(new { Result = false, Message = "Please Enter Village of Birth" }, JsonRequestBehavior.AllowGet); }
                if (tbl_SSC_Form17.Taluka_of_Birth == null || tbl_SSC_Form17.Taluka_of_Birth.Trim() == "")
                { return Json(new { Result = false, Message = "Please Enter Taluka of Birth" }, JsonRequestBehavior.AllowGet); }
                if (tbl_SSC_Form17.District_of_Birth == null || tbl_SSC_Form17.District_of_Birth.Trim() == "")
                { return Json(new { Result = false, Message = "Please Enter District of Birth" }, JsonRequestBehavior.AllowGet); }
                if (tbl_SSC_Form17.Name_of_Secondary_School == null || tbl_SSC_Form17.Name_of_Secondary_School.Trim() == "")
                { return Json(new { Result = false, Message = "Please Enter Name of Secondary School" }, JsonRequestBehavior.AllowGet); }
                if (tbl_SSC_Form17.Secondary_School_Village == null || tbl_SSC_Form17.Secondary_School_Village.Trim() == "")
                { return Json(new { Result = false, Message = "Please Enter Name of Secondary School" }, JsonRequestBehavior.AllowGet); }
                if (tbl_SSC_Form17.Secondary_School_Pincode == null || tbl_SSC_Form17.Secondary_School_Pincode.Trim() == "" || tbl_SSC_Form17.Secondary_School_Pincode.Trim().Length != 6 || containsNonDigits(tbl_SSC_Form17.Secondary_School_Pincode.Trim()))
                { return Json(new { Result = false, Message = "Please Enter valid Pincode of Secondary School" }, JsonRequestBehavior.AllowGet); }
                if (tbl_SSC_Form17.Secondary_School_Taluka == null || tbl_SSC_Form17.Secondary_School_Taluka.Trim() == "")
                { return Json(new { Result = false, Message = "Please Enter Name of Secondary School Taluka" }, JsonRequestBehavior.AllowGet); }
                if (tbl_SSC_Form17.Secondary_School_District == null || tbl_SSC_Form17.Secondary_School_District.Trim() == "")
                { return Json(new { Result = false, Message = "Please Enter Name of Secondary School District" }, JsonRequestBehavior.AllowGet); }
                if (tbl_SSC_Form17.Secondary_School_State == null || tbl_SSC_Form17.Secondary_School_State.Trim() == "")
                { return Json(new { Result = false, Message = "Please Enter Name of Secondary School State" }, JsonRequestBehavior.AllowGet); }
                if (tbl_SSC_Form17.Secondary_School_Udise_No != null && tbl_SSC_Form17.Secondary_School_Udise_No.Trim() != "" && (tbl_SSC_Form17.Secondary_School_Udise_No.Trim().Length != 11 || containsNonDigits(tbl_SSC_Form17.Secondary_School_Udise_No.Trim())))
                { return Json(new { Result = false, Message = "Please Enter valid Udise No of Secondary School OR Leave it blank if You dont Know the Udise No" }, JsonRequestBehavior.AllowGet); }
                if (tbl_SSC_Form17.Whether_Handicap == null || tbl_SSC_Form17.Whether_Handicap.Trim() == "0")
                { return Json(new { Result = false, Message = "Please Select Whether Student is Handicap?" }, JsonRequestBehavior.AllowGet); }
                if (tbl_SSC_Form17.Whether_Handicap == "Yes" && (tbl_SSC_Form17.Handicap_Category == null || tbl_SSC_Form17.Handicap_Category.Trim() == "0"))
                { return Json(new { Result = false, Message = "Please Select Handicapped Category" }, JsonRequestBehavior.AllowGet); }
                if (tbl_SSC_Form17.Date_of_Leaving_Secondary_School == null)
                { return Json(new { Result = false, Message = "Please Select Date of Leaving Secondary School" }, JsonRequestBehavior.AllowGet); }               
                if (tbl_SSC_Form17.Last_Studied_Standard == null || tbl_SSC_Form17.Last_Studied_Standard.Trim() == "0")
                { return Json(new { Result = false, Message = "Please Select Last Studied Standard" }, JsonRequestBehavior.AllowGet); }
                if (tbl_SSC_Form17.Status_of_Last_Studied_Standard == null || tbl_SSC_Form17.Status_of_Last_Studied_Standard.Trim() == "0")
                { return Json(new { Result = false, Message = "Please Select Status of Last Studied Standard" }, JsonRequestBehavior.AllowGet); }
                if (tbl_SSC_Form17.District_of_Form_Submission == null || tbl_SSC_Form17.District_of_Form_Submission.Trim() == "0")
                { return Json(new { Result = false, Message = "Please Select District of Form Submission" }, JsonRequestBehavior.AllowGet); }
                if (tbl_SSC_Form17.Taluka_of_Form_Submission == null || tbl_SSC_Form17.Taluka_of_Form_Submission.Trim() == "0")
                { return Json(new { Result = false, Message = "Please Select Taluka of Form Submission" }, JsonRequestBehavior.AllowGet); }
                if (tbl_SSC_Form17.Medium_of_Form == null || tbl_SSC_Form17.Medium_of_Form.Trim() == "0")
                { return Json(new { Result = false, Message = "Please Select Medium" }, JsonRequestBehavior.AllowGet); }
                if (tbl_SSC_Form17.School_of_Form_Submission == null || tbl_SSC_Form17.School_of_Form_Submission.Trim() == "0")
                { return Json(new { Result = false, Message = "Please Select School of Form Submission" }, JsonRequestBehavior.AllowGet); }
                if (tbl_SSC_Form17.Index_No_of_School == null || tbl_SSC_Form17.Index_No_of_School.Trim() == "")
                { return Json(new { Result = false, Message = "Please Enter Index No of School" }, JsonRequestBehavior.AllowGet); }
                if (tbl_SSC_Form17.Declaration_Yes_No == null || tbl_SSC_Form17.Declaration_Yes_No.Trim() == "0")
                { return Json(new { Result = false, Message = "Please Select option of Point 10" }, JsonRequestBehavior.AllowGet); }
                if (tbl_SSC_Form17.Declaration_Yes_No == "Yes" && (tbl_SSC_Form17.Name_of_Debarred_Board == null || tbl_SSC_Form17.Name_of_Debarred_Board.Trim() == ""))
                { return Json(new { Result = false, Message = "Please Select Name of Debarred Board" }, JsonRequestBehavior.AllowGet); }
                if (tbl_SSC_Form17.Declaration_Yes_No == "Yes" && (tbl_SSC_Form17.Exam_of_Debar == null || tbl_SSC_Form17.Exam_of_Debar.Trim() == ""))
                { return Json(new { Result = false, Message = "Please Enter Name of Exam of Debar" }, JsonRequestBehavior.AllowGet); }
                if (tbl_SSC_Form17.Declaration_Yes_No == "Yes" && (tbl_SSC_Form17.Debarred_From_Year == null || tbl_SSC_Form17.Debarred_From_Year.Trim() == "" || tbl_SSC_Form17.Debarred_From_Year.Trim().Length != 4 || containsNonDigits(tbl_SSC_Form17.Debarred_From_Year.Trim())))
                { return Json(new { Result = false, Message = "Please Enter valid 'Debarred From Year'" }, JsonRequestBehavior.AllowGet); }
                if (tbl_SSC_Form17.Declaration_Yes_No == "Yes" && (tbl_SSC_Form17.Debarred_To_Year == null || tbl_SSC_Form17.Debarred_To_Year.Trim() == "" || tbl_SSC_Form17.Debarred_To_Year.Trim().Length != 4 || containsNonDigits(tbl_SSC_Form17.Debarred_To_Year.Trim())))
                { return Json(new { Result = false, Message = "Please Enter valid 'Debarred To Year'" }, JsonRequestBehavior.AllowGet); }
                if (tbl_SSC_Form17.Leaving_Certificate_Yes_No == null || tbl_SSC_Form17.Leaving_Certificate_Yes_No.Trim() == "0")
                { return Json(new { Result = false, Message = "Please Enclose Secondary School Leaving Certificate" }, JsonRequestBehavior.AllowGet); }
                if (tbl_SSC_Form17.Leaving_Certificate_Yes_No == "No" && (tbl_SSC_Form17.Duplicate_Leaving_Certificate_Yes_No == null || tbl_SSC_Form17.Duplicate_Leaving_Certificate_Yes_No.Trim() == "0"))
                { return Json(new { Result = false, Message = "Please Enclose Duplicate Leaving Certificate" }, JsonRequestBehavior.AllowGet); }
                if (tbl_SSC_Form17.Selected_Language == null || tbl_SSC_Form17.Selected_Language.Trim() == "0")
                { return Json(new { Result = false, Message = "Please Select Medium (Language) Option after 'Point 11'" }, JsonRequestBehavior.AllowGet); }

                if (tbl_SSC_Form17.SSC_Photo == null)
                { return Json(new { Result = false, Message = "Please Upload photo" }, JsonRequestBehavior.AllowGet); }
                if (tbl_SSC_Form17.SSC_Photo != null)
                {
                    string PhotoExtension = Path.GetExtension(tbl_SSC_Form17.SSC_Photo.FileName);
                    if (PhotoExtension != ".jpeg" && PhotoExtension != ".jpg" && PhotoExtension != ".png" && PhotoExtension != ".JPEG" && PhotoExtension != ".JPG" && PhotoExtension != ".PNG")
                    { return Json(new { Result = false, Message = "Photo file type should be 'jpeg/jpg/png' only" }, JsonRequestBehavior.AllowGet); }
                }
                if (tbl_SSC_Form17.SSC_Photo.ContentLength == 0)
                { return Json(new { Result = false, Message = "Your Passport Photo's file is empty" }, JsonRequestBehavior.AllowGet); }
                if (tbl_SSC_Form17.SSC_Photo.ContentLength > 204800)
                { return Json(new { Result = false, Message = "Your Passport Photo's file size exceeding 200 KB" }, JsonRequestBehavior.AllowGet); }
                if (tbl_SSC_Form17.Handicap_Category.Trim() != "0" && tbl_SSC_Form17.SSC_Handicap_Certificate == null)
                { return Json(new { Result = false, Message = "Please Upload Handicap Certificate" }, JsonRequestBehavior.AllowGet); }
                if (tbl_SSC_Form17.Handicap_Category.Trim() != "0" && tbl_SSC_Form17.SSC_Handicap_Certificate != null)
                {
                    string HandiCertiExetension = Path.GetExtension(tbl_SSC_Form17.SSC_Handicap_Certificate.FileName);
                    if (HandiCertiExetension != ".jpeg" && HandiCertiExetension != ".jpg" && HandiCertiExetension != ".png" && HandiCertiExetension != ".pdf" && HandiCertiExetension != ".JPEG" && HandiCertiExetension != ".JPG" && HandiCertiExetension != ".PNG" && HandiCertiExetension != ".PDF")
                    { return Json(new { Result = false, Message = "Handicap Certificate file type should be 'jpeg/jpg/png/pdf' only." }, JsonRequestBehavior.AllowGet); }
                }
                if (tbl_SSC_Form17.Handicap_Category.Trim() != "0" && tbl_SSC_Form17.SSC_Handicap_Certificate.ContentLength == 0)
                { return Json(new { Result = false, Message = "Your Handicap Certificate file is empty" }, JsonRequestBehavior.AllowGet); }
                if (tbl_SSC_Form17.Handicap_Category.Trim() != "0" && tbl_SSC_Form17.SSC_Handicap_Certificate.ContentLength > 204800)
                { return Json(new { Result = false, Message = "Your Handicap Certificate file's size is exceeding 200 KB" }, JsonRequestBehavior.AllowGet); }

                if (tbl_SSC_Form17.State.Trim() != "Maharashtra" && tbl_SSC_Form17.SSC_Domicile == null)
                { return Json(new { Result = false, Message = "Please Upload Domicile Certificate" }, JsonRequestBehavior.AllowGet); }
                if (tbl_SSC_Form17.State.Trim() != "Maharashtra" && tbl_SSC_Form17.SSC_Domicile != null)
                {
                    string DomicileExtension = Path.GetExtension(tbl_SSC_Form17.SSC_Domicile.FileName);
                    if (tbl_SSC_Form17.State != "Maharashtra" && (DomicileExtension != ".jpeg" && DomicileExtension != ".jpg" && DomicileExtension != ".png" && DomicileExtension != ".pdf"  && DomicileExtension != ".JPEG" && DomicileExtension != ".JPG" && DomicileExtension != ".PNG" && DomicileExtension != ".PDF"))
                    { return Json(new { Result = false, Message = "Domicile Certificate file type should be 'jpeg/jpg/png/pdf' only." }, JsonRequestBehavior.AllowGet); }
                }
                if (tbl_SSC_Form17.State.Trim() != "Maharashtra" && tbl_SSC_Form17.SSC_Domicile.ContentLength == 0)
                { return Json(new { Result = false, Message = "Your Domicile Certificate file is empty" }, JsonRequestBehavior.AllowGet); }
                if (tbl_SSC_Form17.State.Trim() != "Maharashtra" && tbl_SSC_Form17.SSC_Domicile.ContentLength > 204800)
                { return Json(new { Result = false, Message = "Your Domicile Certificate's file size is exceeding 200 KB" }, JsonRequestBehavior.AllowGet); }

                if ((tbl_SSC_Form17.Leaving_Certificate_Yes_No.Trim() == "Yes" || tbl_SSC_Form17.Duplicate_Leaving_Certificate_Yes_No.Trim() == "Yes") && (tbl_SSC_Form17.SSC_LC == null))
                { return Json(new { Result = false, Message = "Please Upload School Leaving Certificate" }, JsonRequestBehavior.AllowGet); }
                if (tbl_SSC_Form17.Leaving_Certificate_Yes_No.Trim() == "Yes" || tbl_SSC_Form17.Duplicate_Leaving_Certificate_Yes_No.Trim() == "Yes" && tbl_SSC_Form17.SSC_LC != null)
                {
                    string LCExtenion = Path.GetExtension(tbl_SSC_Form17.SSC_LC.FileName);
                    if ((tbl_SSC_Form17.Leaving_Certificate_Yes_No.Trim() == "Yes" || tbl_SSC_Form17.Duplicate_Leaving_Certificate_Yes_No.Trim() == "Yes") && (LCExtenion != ".jpeg" && LCExtenion != ".jpg" && LCExtenion != ".png" && LCExtenion != ".pdf" && LCExtenion != ".PDF" && LCExtenion != ".JPEG" && LCExtenion != ".JPG" && LCExtenion != ".PNG" && LCExtenion != ".PDF"))
                    { return Json(new { Result = false, Message = "Leaving Certificate file type should be 'jpeg/jpg/png/pdf' only." }, JsonRequestBehavior.AllowGet); }
                }
                if ((tbl_SSC_Form17.Leaving_Certificate_Yes_No.Trim() == "Yes" || tbl_SSC_Form17.Duplicate_Leaving_Certificate_Yes_No.Trim() == "Yes") && (tbl_SSC_Form17.SSC_LC.ContentLength == 0))
                { return Json(new { Result = false, Message = "Your School Leaving Certificate file is empty" }, JsonRequestBehavior.AllowGet); }
                if ((tbl_SSC_Form17.Leaving_Certificate_Yes_No.Trim() == "Yes" || tbl_SSC_Form17.Duplicate_Leaving_Certificate_Yes_No.Trim() == "Yes") && ( tbl_SSC_Form17.SSC_LC.ContentLength > 204800))
                { return Json(new { Result = false, Message = "Your School Leaving Certificate file's size is exceeding 200 KB" }, JsonRequestBehavior.AllowGet); } 
                
                return Json(new { Result = true, Dialog = "Are you sure want to submit ?", DialogDescription = "Once form is submitted cannot be edited. By clicking 'Yes' you will be redirected to payment gateway." }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                return Json(new { Result = false, Message = "Sorry, something went wrong while processing your request !" }, JsonRequestBehavior.AllowGet);
            }            
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
        public bool IsSchoolLeavingDateGreater (string actual_date, string default_date)
        {
            string[] adate = actual_date.Split('/');
            string[] ddate = default_date.Split('/');
            if(Convert.ToInt32(adate[2]) >= Convert.ToInt32(ddate[2]))
            {
                if (Convert.ToInt32(adate[1]) >= Convert.ToInt32(ddate[1]))
                {
                    if (Convert.ToInt32(adate[0]) >= Convert.ToInt32(ddate[0]))
                    {
                        return true;
                    }
                }
            }   
            return false;
        }

        bool IsValidEmailAddress(string emailID)
        {
            return new System.ComponentModel.DataAnnotations.EmailAddressAttribute().IsValid(emailID);
        }
    }
}