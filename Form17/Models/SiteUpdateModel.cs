using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Form17.Models
{
    public class SiteUpdateModel
    {
        public int ID { get; set; }
        public Nullable<int> Active_Status { get; set; }
        public Nullable<System.DateTime> Late_Fee_Date { get; set; }
        public Nullable<System.DateTime> Super_Late_Fee_Date { get; set; }
        public HttpPostedFileBase HSC_CenterList { get; set; }
        public HttpPostedFileBase SSC_CenterList { get; set; }
    }
}