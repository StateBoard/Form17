using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Form17.Models
{
    public class EC_HSC_ModelController
    {
        // GET: EC_Model
        public string S_ID { get; set; }
        public string Ref_ID { get; set; }
        public string Division { get; set; }
        public int HSC_Year { get; set; }
        public string Candidate_Name { get; set; }
        public string Mother_Name { get; set; }
        public string Divisional_Board { get; set; }
        public string Birth_date { get; set; }
        public string Gender { get; set; }
        public string College_No { get; set; }
        public string Stream { get; set; }
        public string EC_No { get; set; }
        public string College_Addr { get; set; }
        public string Date_Issue { get; set; }
        public string Case_No { get; set; }
    }
}