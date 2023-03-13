using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Form17.Models
{
    public class UpdateSchoolByDivisionModel
    {
        public int ID { get; set; }
        public string Application_No { get; set; }
        public string S_ID { get; set; }
        public Nullable<int> Ref_ID { get; set; }
        public string Last_Name { get; set; }
        public string First_Name { get; set; }
        public string Father_Husband_Name { get; set; }
        public string District_of_Form_Submission { get; set; }
        public string Taluka_of_Form_Submission { get; set; }
        public string Medium_of_Form { get; set; }
        public string School_of_Form_Submission { get; set; }
        public string Index_No_of_School { get; set; }
    }
}