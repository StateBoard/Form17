using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Form17.Models
{
    public class UploadECDataModel
    {
        public string STD { get; set; }
        public HttpPostedFileBase EC_Data { get; set; }
    }
}