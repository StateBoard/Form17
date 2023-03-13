using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Form17.Models
{
    public class HSC_Form17_Model
    {
        public int ID { get; set; }
             
        [RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "Use Alphabates only")]
        public string Last_Name { get; set; }

        [Required(ErrorMessage = "First Name is Required")]
        [RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "Use Alphabates only.")]
        public string First_Name { get; set; }

        [Required(ErrorMessage = "Father/Husband Name is Required")]
        [RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "Use Alphabates only.")]
        public string Father_Husband_Name { get; set; }

        [Required(ErrorMessage = "Mother Name is Required")]
        [RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "Use Alphabates only.")]
        public string Mother_Name { get; set; }
        public string Gender { get; set; }

        [Required(ErrorMessage = "Residential Address is Required")]
        public string Residential_Address { get; set; }

        [Required(ErrorMessage = "Pin Code is Required")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Use Digits only.")]
        [StringLength(6, ErrorMessage = "Pin Code must be 6 digits", MinimumLength = 6)]
        public string Pincode { get; set; }
        public string District { get; set; }
        public string Taluka { get; set; }
        public string State { get; set; }

        [Required(ErrorMessage = "Aadhar no is Required")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Use Digits only please")]
        [StringLength(12, ErrorMessage = "Aadhar No must be 12 digits", MinimumLength = 12)]
        public string Aadhar_No { get; set; }

        [Required(ErrorMessage = "Mobile no is Required.")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Please insert valid mobile no.")]
        [StringLength(10, ErrorMessage = "Mobile no must be 10 digits", MinimumLength = 10)]
        public string Mobile_No { get; set; }

        [Required(ErrorMessage ="Email ID is Required.")]
        [RegularExpression(@"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$", ErrorMessage ="Invalid Email ID")]
        public string Email_ID { get; set; }
        public string DOB { get; set; }

        [Required(ErrorMessage = "Village/Town is required.")]
        public string Village_of_Birth { get; set; }

        [Required(ErrorMessage = "Taluka is required.")]
        public string Taluka_of_Birth { get; set; }

        [Required(ErrorMessage = "District is required.")]
        public string District_of_Birth { get; set; }
        public string SSC_Passing_Month { get; set; }
        public string SSC_Passing_Year { get; set; }
        public string SSC_Division_Board { get; set; }
        public string Other_SSC_Board { get; set; }
        public string Date_of_Leaving_Secondary_School { get; set; }
        public string Date_of_Leaving_Junior_College { get; set; }

        //[Required(ErrorMessage = "Please Enter Name of secondary school.")]
        public string Name_of_Secondary_School { get; set; }

        [Required(ErrorMessage = "Please Enter Village/Town.")]
        public string Secondary_School_Village { get; set; }

        [Required(ErrorMessage = "Please Enter Taluka.")]
        public string Secondary_School_Taluka { get; set; }

        [Required(ErrorMessage = "Please Enter District.")]
        public string Secondary_School_District { get; set; }

        [Required(ErrorMessage = "Please Enter State.")]
        public string Secondary_School_State { get; set; }

        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Use Digits only.")]
        [StringLength(7, ErrorMessage = "Invalid Index No", MinimumLength = 7)]
        public string Secondary_School_Index_No { get; set; }

        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Use Digits only.")]
        [StringLength(11, ErrorMessage = "Invalid Udise No", MinimumLength = 11)]
        public string Secondary_School_Udise_No { get; set; }
        public string Attended_Junior_College_Yes_No { get; set; }
        public string Name_of_Junior_College { get; set; }
        public string Junior_College_Village { get; set; }
        public string Junior_College_District { get; set; }
        public string Junior_College_State { get; set; }

        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Use Digits only.")]
        [StringLength(7, ErrorMessage = "Invalid Index No", MinimumLength = 7)]
        public string Junior_College_Index_No { get; set; }
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Use Digits only.")]
        [StringLength(11, ErrorMessage = "Invalid Index No", MinimumLength = 11)]
        public string Junior_College_Udise_No { get; set; }
        public string Junior_College_Stream { get; set; }
        public string FYJC_Passing_Month { get; set; }
        public string FYJC_Passing_Year { get; set; }
        public string FYJC_Passing_Status { get; set; }
        public string District_of_Form_Submission { get; set; }
        public string Taluka_of_Form_Submission { get; set; }
        public string Stream_of_Form { get; set; }
        public string Medium_of_Form { get; set; }
        public string College_of_Form_Submission { get; set; }
        public string College_Index_No { get; set; }
        public string Declaration_Yes_No { get; set; }
        public string Name_of_Debarred_Board { get; set; }
        public string Exam_of_Debar { get; set; }

        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Use Digits only.")]
        [StringLength(4, ErrorMessage = "Invalid year", MinimumLength = 4)]
        public string Debarred_From_Year { get; set; }

        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Use Digits only.")]
        [StringLength(4, ErrorMessage = "Invalid year", MinimumLength = 4)]
        public string Debarred_To_Year { get; set; }
        public string Secondary_School_Certificate_Yes_No { get; set; }
        public string FYJC_Leaving_Certificate_Yes_No { get; set; }
        public string School_Leaving_Certificate_Yes_No { get; set; }
        public string School_Leaving_Duplicate_Certificate_Yes_No { get; set; }
        public HttpPostedFileBase HSC_Photo { get; set; }
        public HttpPostedFileBase HSC_School_LC { get; set; }        
        public HttpPostedFileBase HSC_School_Marksheet{ get; set; }        
        public HttpPostedFileBase HSC_Domicile { get; set; }
        public HttpPostedFileBase JCLC { get; set; }
    }
}