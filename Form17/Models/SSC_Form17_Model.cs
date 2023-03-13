using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Form17.Models
{
    public class SSC_Form17_Model
    {
        public int ID { get; set; }

        [RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "Use Alphabates only")]
        public string Last_Name { get; set; }

        [Required(ErrorMessage = "First Name is Required")]
        [RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "Use Alphabates only")]
        public string First_Name { get; set; }

        [Required(ErrorMessage = "Father/Husband Name is Required")]
        [RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "Use Alphabates only")]
        public string Father_Husband_Name { get; set; }

        [Required(ErrorMessage = "Mother Name is Required")]
        [RegularExpression(@"^[a-zA-Z ]+$", ErrorMessage = "Use Alphabates only")]
        public string Mother_Name { get; set; }
        public string Gender { get; set; }

        [Required(ErrorMessage = "Please Enter Residential Address")]       
        public string Residential_Address { get; set; }

        [Required(ErrorMessage = "Please Enter Pin Code")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Use Digits only")]
        [StringLength(6, ErrorMessage = "Pin Code must be 6 digits", MinimumLength = 6)]
        public string Pincode { get; set; }
        public string District { get; set; }
        public string Taluka { get; set; }
        public string State { get; set; }

        [Required(ErrorMessage = "Please Enter Aadhar No")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Use Digits only")]
        [StringLength(12, ErrorMessage = "Invalid Aadhar No", MinimumLength = 12)]
        public string Aadhar_No { get; set; }

        [Required(ErrorMessage = "Please Enter Mobile No")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Invalid Mobile No")]
        [StringLength(10, ErrorMessage = "Invalid Mobile No", MinimumLength = 10)]
        public string Mobile_No { get; set; }

        [Required(ErrorMessage = "Email ID is Required.")]
        [RegularExpression(@"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$", ErrorMessage = "Invalid Email ID")]
        public string Email_ID { get; set; }
        public string DOB { get; set; }

        [Required(ErrorMessage = "Please Enter Village of Birth")]
        public string Village_of_Birth { get; set; }

        [Required(ErrorMessage = "Please Enter Taluka of Birth")]
        public string Taluka_of_Birth { get; set; }

        [Required(ErrorMessage = "Please Enter District of Birth")]
        public string District_of_Birth { get; set; }

        [Required(ErrorMessage = "Please Enter Name of Secondary School")]
        public string Name_of_Secondary_School { get; set; }

        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Use Digits only")]
        [StringLength(11, ErrorMessage = "Invalid Udise No", MinimumLength = 11)]
        public string Secondary_School_Udise_No { get; set; }

        [Required(ErrorMessage = "Please Enter Pincode")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Use Digits only")]
        [StringLength(6, ErrorMessage = "Invalid Pincode", MinimumLength = 6)]
        public string Secondary_School_Pincode { get; set; }

        [Required(ErrorMessage = "Please Enter Name of Secondary School Village/Town")]
        public string Secondary_School_Village { get; set; }

        [Required(ErrorMessage = "Please Enter Name of Secondary School Taluka")]
        public string Secondary_School_Taluka { get; set; }

        [Required(ErrorMessage = "Please Enter Name of Secondary School District")]
        public string Secondary_School_District { get; set; }

        [Required(ErrorMessage = "Please Enter Name of Secondary School State")]
        public string Secondary_School_State { get; set; }
      
        public string Whether_Handicap { get; set; }
        public string Handicap_Category { get; set; }
        public string Date_of_Leaving_Secondary_School { get; set; }
        public string Last_Studied_Standard { get; set; }
        public string Status_of_Last_Studied_Standard { get; set; }
        public string District_of_Form_Submission { get; set; }
        public string Taluka_of_Form_Submission { get; set; }
        public string Medium_of_Form { get; set; }
        public string School_of_Form_Submission { get; set; }
        public string Index_No_of_School { get; set; }
        public string Declaration_Yes_No { get; set; }
        public string Name_of_Debarred_Board { get; set; }
        public string Exam_of_Debar { get; set; }

        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Use Digits only")]
        [StringLength(4, ErrorMessage = "Invalid year", MinimumLength = 4)]
        public string Debarred_From_Year { get; set; }

        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Use Digits only")]
        [StringLength(4, ErrorMessage = "Invalid year", MinimumLength = 4)]
        public string Debarred_To_Year { get; set; }
        public string Leaving_Certificate_Yes_No { get; set; }
        public string Duplicate_Leaving_Certificate_Yes_No { get; set; }
        public string Selected_Langauage { get; set; }
        public HttpPostedFileBase SSC_Photo { get; set; }
        public HttpPostedFileBase SSC_LC { get; set; }       
        public HttpPostedFileBase SSC_Domicile { get; set; }
        public HttpPostedFileBase SSC_Handicap_Certificate { get; set; }        
    }
}