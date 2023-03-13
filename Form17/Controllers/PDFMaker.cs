using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Web.Mvc;
using System.IO;
using Form17.Models;
using System.Configuration;

namespace Form17.Controllers
{
    public class PDFMaker : Controller
    {
        Form17Entities db = new Form17Entities();
        DateConverter dt_covert = new DateConverter();
        public bool IsSuperLateForm(DateTime actual_date, DateTime default_date)
        {
            if (actual_date >= default_date)
            {
                return true;
            }
            return false;
        }
        public bool IsLateForm(DateTime actual_date, DateTime default_date)
        {
            if (actual_date >= default_date)
            {
                return true;
            }
            return false;
        }
        public void Create_SSC_PDF(int IDs, Tbl_SSC_Payment payment_model)
        {
            try
            {

                Document pdfDoc = new Document(PageSize.A4, 25, 25, 25, 15);
                //try
                //{
                Tbl_SSC_Form17_Final tbl = db.Tbl_SSC_Form17_Final.Where(a => a.Ref_ID == IDs).FirstOrDefault();

                Tbl_Center centerName = new Tbl_Center();
                //db.(tbl.Email_ID.Trim().ToUpper() == )
                centerName = db.Tbl_Center.Where(s => s.Index_No == tbl.Index_No_of_School && s.STD == 10).FirstOrDefault();
                //if ()
                if (centerName == null)
                {

                    centerName.Name = ""; centerName.Index_No = "";
                    if (tbl.Index_No_of_School == "1615066")
                    {
                        centerName.Name = "MOTHER TERESA SECONDARY CONVENT SCHOOL, AIROLI, NAVI MUMBAI - 400 708"; centerName.Index_No = "1615066";
                    }
                    else if (tbl.Index_No_of_School == "1615086")
                    {
                        centerName.Name = "NEELKANTH PATIL VIDYALAY, AIROLI, NAVI MUMBAI"; centerName.Index_No = "1615086";
                    }
                    else if (tbl.Index_No_of_School == "1617053")
                    {
                        centerName.Name = "SAI SANSTHECHE ENGLISH SECONDARY CONVENT SCHOOL "; centerName.Index_No = "1617053";
                    }
                    else if (tbl.Index_No_of_School == "1618017")
                    {
                        centerName.Name = "SITALDAS KHEMANI HIGH SCHOOL, ULHASNAGAR - 421 002"; centerName.Index_No = "1618017";
                    }
                    else if (tbl.Index_No_of_School == "1701040")
                    {
                        centerName.Name = "JANATA VIDYAMANDIR, AJIWALI, TAL - PANVEL 410 206"; centerName.Index_No = "1701040";
                    }
                    else if (tbl.Index_No_of_School == "1805007")
                    {
                        centerName.Name = "MADHYAMIK VIDYALAY MU POST UDHWA TAL - TALASARI"; centerName.Index_No = "1805007";
                    }
                    else if (tbl.Index_No_of_School == "1808108")
                    {
                        centerName.Name = "JIVDANI VIDYAVARDHINI HIGH SCHOOL VIRAR, PALGHAR EAST - 401 303"; centerName.Index_No = "1808108";
                    }
                    else if (tbl.Index_No_of_School == "3104066")
                    {
                        centerName.Name = "BHARATRATNA DR. BABASAHEB AMBEDKAR VIDYALAY, DHARAVI, MUMBAI - 400 017"; centerName.Index_No = "3104066";
                    }
                    else if (tbl.Index_No_of_School == "1615119")
                    {
                        centerName.Name = "NAVI MUMBAI MAHANAGARPALIKA SANCHALIT MADHYAMIK VIDYALAY TURBHEGAAV - NAVI MUMBAI"; centerName.Index_No = "1615119";
                    }
                    else
                    {
                        centerName.Name = ""; centerName.Index_No = "";
                    }
                }
                //var newcenterName = db.Upda.Where(s => s.Index_No == tbl.Index_No_of_School && s.STD == 10).FirstOrDefault();

                var DistrictName = db.Tbl_Code_Master.Where(m => m.district_code == tbl.District).FirstOrDefault().district_name;
                var divcode = db.Tbl_Code_Master.Where(m => m.district_code == tbl.District_of_Form_Submission).FirstOrDefault().division_code;
                iTextSharp.text.Image check1 = iTextSharp.text.Image.GetInstance(Path.Combine(HttpRuntime.AppDomainAppPath, "Images/check.png"));

                PdfWriter.GetInstance(pdfDoc, new System.IO.FileStream(Path.Combine(HttpRuntime.AppDomainAppPath, "PDF/SSC/" + tbl.S_ID + ".pdf"), FileMode.Create));
                BaseFont bf = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                iTextSharp.text.Font Head_Font = new iTextSharp.text.Font(bf, 20, iTextSharp.text.Font.BOLD);
                iTextSharp.text.Font sub_font = new iTextSharp.text.Font(bf, 14, iTextSharp.text.Font.BOLD);
                iTextSharp.text.Font table_font = new iTextSharp.text.Font(bf, 12, iTextSharp.text.Font.BOLD);
                iTextSharp.text.Font middle_font = new iTextSharp.text.Font(bf, 16, iTextSharp.text.Font.BOLD);
                pdfDoc.Open();
                PdfPTable table = new PdfPTable(2);
                float[] width = new float[] { 50f, 100f };
                table.SetWidths(width);
                table.DefaultCell.FixedHeight = 20f;
                table.WidthPercentage = 90;
                table.DefaultCell.Border = 0;
                table.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right

                PdfPCell cell = new PdfPCell();
                cell.Border = 0;
                iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(Path.Combine(HttpRuntime.AppDomainAppPath, "Images/MSBSHSE-LOGO.png"));
                logo.ScaleAbsolute(100, 50);
                cell.AddElement(logo);
                table.AddCell(cell);

                Paragraph para = new Paragraph("\n Maharashtra State Board of Secondary \n   And Higher Secondary Education,  \n               Pune-411004 ", Head_Font);
                cell = new PdfPCell();
                para.Alignment = 2;
                table.AddCell(para);
                pdfDoc.Add(table);

                List<Tbl_Site_Status> tbl_Site_Status = db.Tbl_Site_Status.ToList();

                if (IsSuperLateForm(Convert.ToDateTime(tbl.Date_Time), Convert.ToDateTime(tbl_Site_Status[0].Super_Late_Fee_Date)))
                {
                    Paragraph suoerlateform = new Paragraph(" Form No:" + tbl.S_ID.ToString() + " [Super Late Form] \n ", sub_font);
                    suoerlateform.Alignment = 2;
                    suoerlateform.IndentationRight = 20;
                    pdfDoc.Add(suoerlateform);
                }
                else if (IsLateForm(Convert.ToDateTime(tbl.Date_Time), Convert.ToDateTime(tbl_Site_Status[0].Late_Fee_Date.ToString())))
                {
                    Paragraph lateform = new Paragraph(" Form No:" + tbl.S_ID.ToString() + " [Late Form] \n ", sub_font);
                    lateform.Alignment = 2;
                    lateform.IndentationRight = 20;
                    pdfDoc.Add(lateform);
                }
                else
                {
                    Paragraph para1 = new Paragraph(" Form No:" + tbl.S_ID.ToString() + " \n ", sub_font);
                    para1.Alignment = 2;
                    para1.IndentationRight = 20;
                    pdfDoc.Add(para1);
                }
                Paragraph para2 = new Paragraph(" Form No:" + tbl.S_ID.ToString() + " \n ", sub_font);
                //para2.Alignment = 2;
                //para2.IndentationRight = 20;
                //pdfDoc.Add(para2);

                table = new PdfPTable(2);
                float[] width1 = new float[] { 60f, 90f };
                table.SetWidths(width1);

                table.WidthPercentage = 100;
                table.DefaultCell.Border = 0;
                table.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.SpacingAfter = 10;


                string divname = "";
                switch (tbl.Division)
                {
                    case "1":
                        divname = "PUNE";
                        break;
                    case "2":
                        divname = "NAGPUR";
                        break;
                    case "3":
                        divname = "AURANGABAD";
                        break;
                    case "4":
                        divname = "MUMBAI";
                        break;
                    case "5":
                        divname = "KOLHAPUR";
                        break;
                    case "6":
                        divname = "AMRAVATI";
                        break;
                    case "7":
                        divname = "NASHIK";
                        break;
                    case "8":
                        divname = "LATUR";
                        break;
                    case "9":
                        divname = "KOKAN";
                        break;
                }
                cell = new PdfPCell();
                para2 = new Paragraph("To,\nThe Divisional Secretary,\nMaharashtra state board of Secondary\nAnd Higher Secondary Education,\n" + divname + " Divisional Board ", sub_font);
                para2.Alignment = 0;
                para2.Leading = 15;
                cell.Border = 0;
                cell.AddElement(para2);
                table.AddCell(cell);

                cell = new PdfPCell();
                iTextSharp.text.Image passport = iTextSharp.text.Image.GetInstance(Path.Combine(HttpRuntime.AppDomainAppPath, "AppFiles/Images/SSC/" + tbl.Ref_ID + "/Photo.jpeg"));
                passport.ScaleAbsolute(60, 40);
                cell.Border = 0;
                passport.Alignment = 2;
                cell.AddElement(passport);
                table.AddCell(cell);
                pdfDoc.Add(table);

                Paragraph para3 = new Paragraph(" \n  Sir,\n    I request you to issue enrollment Certificate as a Private Candidate at the S.S.C Examination \n (Std.X) to be held in " + ConfigurationManager.AppSettings["MonthYearOFExam"].ToString() + " and submit the following particulars for your information. \n ", FontFactory.GetFont("Times New Roman", 12));
                para2.Alignment = 0;
                pdfDoc.Add(para3);

                table = new PdfPTable(1);
                float[] width3 = new float[] { 90f };
                table.SetWidths(width3);
                table.SpacingAfter = 20;
                table.WidthPercentage = 80;
                table.DefaultCell.Border = 0;
                table.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell = new PdfPCell();
                para2 = new Paragraph(centerName.Index_No + " " + centerName.Name.ToUpper(), table_font);
                para2.Alignment = 1;
                para2.Leading = 14;
                cell.AddElement(para2);
                table.AddCell(cell);
                pdfDoc.Add(table);

                table = new PdfPTable(4);
                float[] width10 = new float[] { 60f, 60f, 60f, 60f };
                table.SetWidths(width10);
                table.DefaultCell.FixedHeight = 30f;
                table.WidthPercentage = 95;
                table.HorizontalAlignment = Element.ALIGN_CENTER;
                table.HorizontalAlignment = 0;
                table.SpacingAfter = 50f;

                cell = new PdfPCell();
                cell = new PdfPCell(new Phrase("REMARKS", table_font));
                cell.Colspan = 2;
                cell.Padding = 5;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("DOCUMENT RECIEVED ", table_font));
                cell.Colspan = 2;
                cell.Padding = 5;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                table.AddCell("Eligible");
                table.AddCell(check1);

                table.AddCell("LC (Std V/VI/VII/VIII)");
                table.AddCell(check1);

                table.AddCell("Incomplete");
                table.AddCell(check1);

                table.AddCell("L.C./T.C.(Std.X/XI)");
                table.AddCell(check1);

                table.AddCell("Ineligible ");
                table.AddCell(check1);

                table.AddCell("Aadhar Card");
                table.AddCell(check1);

                PdfPCell cell1 = new PdfPCell(new Phrase("Clerk"));
                cell1.Colspan = 2;

                PdfPTable nestedTable = new PdfPTable(2);
                float[] nestedwidth = new float[] { 60f, 60f };
                nestedTable.SetWidths(nestedwidth);
                nestedTable.DefaultCell.FixedHeight = 20f;
                nestedTable.WidthPercentage = 95;
                nestedTable.HorizontalAlignment = Element.ALIGN_CENTER;

                nestedTable.DefaultCell.Border = 0;
                nestedTable.AddCell(new Paragraph("   "));
                nestedTable.AddCell(new Paragraph("  "));

                nestedTable.AddCell(new Paragraph("  "));
                nestedTable.AddCell(new Paragraph("  "));

                nestedTable.AddCell(new Paragraph("Clerk"));
                nestedTable.AddCell(new Paragraph("  Head of Br"));
                cell1.AddElement(nestedTable);
                table.AddCell(cell1);

                table.AddCell("Affidavit attested Photocopy / True Copy");
                nestedTable = new PdfPTable(1);
                float[] nestedwidth1 = new float[] { 60f };
                nestedTable.SetWidths(nestedwidth1);
                nestedTable.DefaultCell.FixedHeight = 30f;
                nestedTable.WidthPercentage = 95;
                nestedTable.HorizontalAlignment = Element.ALIGN_CENTER;

                nestedTable.HorizontalAlignment = 0;
                nestedTable.DefaultCell.Border = 0;
                PdfPCell cell3 = new PdfPCell(new Phrase("Clerk"));
                check1 = iTextSharp.text.Image.GetInstance(Path.Combine(HttpRuntime.AppDomainAppPath, "Images/check.png"));
                nestedTable.AddCell(check1);
                cell3.AddElement(nestedTable);
                table.AddCell(cell3);

                PdfPCell cell2 = new PdfPCell(new Phrase(""));
                cell2.Colspan = 2;

                nestedTable = new PdfPTable(2);
                float[] nestedwidth14 = new float[] { 60f, 60f };
                nestedTable.SetWidths(nestedwidth14);
                nestedTable.DefaultCell.FixedHeight = 20f;
                nestedTable.WidthPercentage = 95;
                nestedTable.HorizontalAlignment = Element.ALIGN_CENTER;
                nestedTable.DefaultCell.Border = 0;

                nestedTable.AddCell(new Paragraph(""));
                nestedTable.AddCell(new Paragraph(""));

                nestedTable.AddCell(new Paragraph(""));
                nestedTable.AddCell(new Paragraph(""));

                nestedTable.AddCell(new Paragraph("Sr.Supdt."));

                nestedTable.AddCell(new Paragraph("   Jt.Secy"));
                cell2.AddElement(nestedTable);
                table.AddCell(cell2);

                table.AddCell("Bonafide Certificate");
                nestedTable = new PdfPTable(1);
                float[] nestedwidth15 = new float[] { 60f };
                nestedTable.SetWidths(nestedwidth15);
                nestedTable.DefaultCell.FixedHeight = 30f;
                nestedTable.WidthPercentage = 95;
                nestedTable.HorizontalAlignment = Element.ALIGN_CENTER;
                nestedTable.HorizontalAlignment = 0;
                nestedTable.DefaultCell.Border = 0;
                PdfPCell cell4 = new PdfPCell(new Phrase("Clerk"));
                check1 = Image.GetInstance(Path.Combine(HttpRuntime.AppDomainAppPath, "Images/check.png"));
                nestedTable.AddCell(check1);
                cell4.AddElement(nestedTable);
                table.AddCell(cell4);

                cell = new PdfPCell(new Phrase("Migration Certificate"));
                cell.Colspan = 3;
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                cell.Padding = 10;
                table.AddCell(cell);
                table.AddCell(check1);
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);
                pdfDoc.Add(table);

                pdfDoc.NewPage();
                table = new PdfPTable(2);
                float[] width11 = new float[] { 100f, 100f };
                table.SetWidths(width11);
                table.DefaultCell.FixedHeight = 0f;
                table.WidthPercentage = 95;
                table.HorizontalAlignment = Element.ALIGN_CENTER;
                table.HorizontalAlignment = 0;

                table.SpacingAfter = 30f;

                cell = new PdfPCell();
                cell = new PdfPCell(new Phrase("Full Name", sub_font));
                table.AddCell(cell);
                table.AddCell(tbl.First_Name.ToUpper().Trim() + " " + tbl.Father_Husband_Name.ToUpper() + " " + tbl.Last_Name.ToUpper().Trim());

                cell = new PdfPCell(new Phrase("Mother's Name", sub_font));
                table.AddCell(cell);

                table.AddCell(tbl.Mother_Name.ToUpper());

                cell = new PdfPCell(new Phrase("Residential Address", sub_font));
                table.AddCell(cell);
                table.AddCell(tbl.Residential_Address.ToUpper());

                cell = new PdfPCell(new Phrase("Residential PinCode", sub_font));
                table.AddCell(cell);
                table.AddCell(tbl.Pincode);

                cell = new PdfPCell(new Phrase("District", sub_font));
                table.AddCell(cell);
                table.AddCell(DistrictName.ToUpper());

                cell = new PdfPCell(new Phrase("State", sub_font));
                table.AddCell(cell);
                table.AddCell(tbl.State.ToUpper());

                cell = new PdfPCell(new Phrase("Student Email ID", sub_font));
                table.AddCell(cell);
                table.AddCell(tbl.Email_ID.ToUpper().Trim());

                cell = new PdfPCell(new Phrase("Mobile Number", sub_font));
                table.AddCell(cell);
                table.AddCell(tbl.Mobile_No);

                cell = new PdfPCell(new Phrase("Aadhar Number ", sub_font));
                table.AddCell(cell);
                table.AddCell(tbl.Aadhar_No);

                cell = new PdfPCell(new Phrase("Gender ", sub_font));
                table.AddCell(cell);
                table.AddCell(tbl.Gender.ToUpper());


                string dob = tbl.DOB.ToString();

                cell = new PdfPCell(new Phrase("Birth Date ", sub_font));
                table.AddCell(cell);
                table.AddCell(dob);
                string dateinwords = "";
                try
                {

                     dateinwords = ConvertDate(DateTime.Parse(dt_covert.ConversionDate_DDMMYYYY_To_MMDDYYYY(tbl.DOB.ToString())));
                }
                catch
                { 
                
                }
               
                //string dateinwords = ConvertDate(DateTime.Parse(tbl.DOB.ToString()));
                cell = new PdfPCell(new Phrase("Birth Date In Words ", sub_font));
                table.AddCell(cell);
                table.AddCell(dateinwords);

                cell = new PdfPCell(new Phrase("Birth Village/town ", sub_font));
                table.AddCell(cell);
                table.AddCell(tbl.Village_of_Birth.ToUpper());

                cell = new PdfPCell(new Phrase("Birth Taluka  ", sub_font));
                table.AddCell(cell);
                table.AddCell(tbl.Taluka_of_Birth.ToUpper());

                cell = new PdfPCell(new Phrase("Birth District ", sub_font));
                table.AddCell(cell);
                table.AddCell(tbl.District_of_Birth.ToUpper());

                cell = new PdfPCell(new Phrase("Name of Last Attended School ", sub_font));
                table.AddCell(cell);
                table.AddCell(tbl.Name_of_Secondary_School.ToUpper().Trim());

                cell = new PdfPCell(new Phrase("Village/Town of Last Attended School ", sub_font));
                table.AddCell(cell);
                table.AddCell(tbl.Secondary_School_Village.ToUpper().Trim());

                cell = new PdfPCell(new Phrase("Taluka of Last Attended School ", sub_font));
                table.AddCell(cell);
                table.AddCell(tbl.Secondary_School_Taluka.ToUpper().Trim());

                cell = new PdfPCell(new Phrase("District of Last Attended School ", sub_font));
                table.AddCell(cell);
                table.AddCell(tbl.Secondary_School_District.ToUpper().Trim());

                cell = new PdfPCell(new Phrase("State of Last Attended School  ", sub_font));
                table.AddCell(cell);
                table.AddCell(tbl.Secondary_School_State.ToUpper().Trim());

                cell = new PdfPCell(new Phrase("Pincode Of Last Attended School ", sub_font));
                table.AddCell(cell);
                table.AddCell(tbl.Secondary_School_Pincode.ToString().Trim());

                cell = new PdfPCell(new Phrase("Udise Code", sub_font));
                table.AddCell(cell);
                if (tbl.Secondary_School_Udise_No != null)
                {
                    table.AddCell(tbl.Secondary_School_Udise_No.ToUpper().Trim());
                }
                else
                {
                    cell = new PdfPCell(new Phrase(" ", sub_font));
                    table.AddCell(cell);
                }

                cell = new PdfPCell(new Phrase("Is handicap", sub_font));
                table.AddCell(cell);
                table.AddCell(tbl.Whether_Handicap.ToString().Trim());

                if (tbl.Handicap_Category.Trim() != "0")
                {
                    cell = new PdfPCell(new Phrase("Handicap category", sub_font));
                    table.AddCell(cell);
                    table.AddCell(tbl.Handicap_Category);
                }

                dob = tbl.Date_of_Leaving_Secondary_School.ToString().Trim();


                cell = new PdfPCell(new Phrase("Medium : ", sub_font));
                table.AddCell(cell);
                table.AddCell(tbl.Medium_of_Form);

                cell = new PdfPCell(new Phrase("Date of leaving the last attended School ", sub_font));
                table.AddCell(cell);
                table.AddCell(dob);


                cell = new PdfPCell(new Phrase("Standard in which studying at the time of leaving school ", sub_font));
                table.AddCell(cell);
                table.AddCell(tbl.Last_Studied_Standard.ToString());

                cell = new PdfPCell(new Phrase("Pass/Fail/Studying ", sub_font));
                table.AddCell(cell);
                table.AddCell(tbl.Status_of_Last_Studied_Standard.ToString());

                cell = new PdfPCell(new Phrase("Debarred from appearing at any of the examination ", sub_font));
                table.AddCell(cell);
                table.AddCell(tbl.Declaration_Yes_No.ToUpper().Trim());
                pdfDoc.Add(table);


                Paragraph paragra1 = new Paragraph("Note: Kindly submit application form to college.  \n", FontFactory.GetFont("Times New Roman", 12));
                paragra1.Alignment = 0;
                pdfDoc.Add(paragra1);

                table = new PdfPTable(1);

                float[] width12 = new float[] { 90f };
                table.SetWidths(width12);
                table.SpacingAfter = 7f;
                table.SpacingBefore = 7f;
                table.WidthPercentage = 80;
                table.DefaultCell.Border = 0;
                table.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                cell = new PdfPCell();
                para2 = new Paragraph(centerName.Index_No + " " + centerName.Name.ToUpper().Trim(), table_font);
                para2.Alignment = 1;
                para2.Leading = 14;
                cell.AddElement(para2);
                table.AddCell(cell);
                pdfDoc.Add(table);


                Paragraph paragra2 = new Paragraph("Along with original documents on or before 06/12/2021 \n", FontFactory.GetFont("Times New Roman", 12));
                paragra2.Alignment = 0;
                pdfDoc.Add(paragra2);


                Paragraph paragra3 = new Paragraph("You have to submit following Documents: \n", sub_font);
                paragra3.Alignment = 0;
                pdfDoc.Add(paragra3);

                Paragraph paragra4 = new Paragraph(" * Original / Duplicate School Leaving Certificate.\n\n", FontFactory.GetFont("Times New Roman", 12));
                paragra4.Alignment = 0;
                pdfDoc.Add(paragra4);

                Paragraph paragra5 = new Paragraph(" * Secondary School Certificate and Marksheet.\n\n", FontFactory.GetFont("Times New Roman", 12));
                paragra5.Alignment = 0;
                pdfDoc.Add(paragra5);

                if (tbl.State.Trim() != "Maharashtra")
                {
                    Paragraph paragra6 = new Paragraph(" * Domicile Certificate.\n\n", FontFactory.GetFont("Times New Roman", 12));
                    paragra6.Alignment = 0;
                    pdfDoc.Add(paragra6);
                }
                if (tbl.Handicap_Category.Trim() != "0")
                {
                    Paragraph paragra7 = new Paragraph(" * Handicapped Certificate.\n\n", FontFactory.GetFont("Times New Roman", 12));
                    paragra7.Alignment = 0;
                    pdfDoc.Add(paragra7);
                }
                para2 = new Paragraph("Signature of the Applicant ", FontFactory.GetFont("Times New Roman", 12));
                para2.Alignment = 2;
                para2.IndentationRight = 60;
                pdfDoc.Add(para2);

                para2 = new Paragraph("(" + tbl.First_Name.Trim().ToUpper() + " " + tbl.Father_Husband_Name.Trim().ToUpper() + " " + tbl.Last_Name.Trim().ToUpper() + ")", FontFactory.GetFont("Times New Roman", 12));
                para2.Alignment = 2;
                para2.IndentationRight = 60;
                pdfDoc.Add(para2);

                pdfDoc.NewPage();
                para = new Paragraph("MAHARASHTRA STATE BOARD OF SECONDARY AND HIGHER", middle_font);

                para.Alignment = 0;
                pdfDoc.Add(para);

                para = new Paragraph("SECONDARY EDUCATION, PUNE", middle_font);
                para.Alignment = 1;
                para.IndentationRight = 70;
                pdfDoc.Add(para);

                para = new Paragraph(" Private Candidate Application (Form No:17)", sub_font);
                para.Alignment = 1;
                para.IndentationRight = 70;
                pdfDoc.Add(para);

                para = new Paragraph("Payment Receipt", sub_font);
                para.Alignment = 1;
                para.IndentationRight = 70;
                pdfDoc.Add(para);

                var division = db.Tbl_Code_Master.Where(x => x.district_code == tbl.District_of_Form_Submission).FirstOrDefault().division_name;
                paragra1 = new Paragraph("Division: " + division + " \n", FontFactory.GetFont("Times New Roman", 12));
                paragra1.Alignment = 0;
                pdfDoc.Add(paragra1);

                table = new PdfPTable(4);
                float[] width14 = new float[] { 60f, 60f, 60f, 60f };
                table.SetWidths(width14);
                table.DefaultCell.FixedHeight = 90f;
                table.WidthPercentage = 95;
                table.HorizontalAlignment = Element.ALIGN_CENTER;
                table.HorizontalAlignment = 0;

                table.SpacingAfter = 7f;
                table.SpacingBefore = 7f;

                cell = new PdfPCell();

                table.AddCell(Cell_Align_Center("Application No "));
                table.AddCell(Cell_Align_Center(tbl.S_ID));
                table.AddCell(Cell_Align_Center("Exam"));
                table.AddCell(Cell_Align_Center("SSC"));
                table.AddCell(Cell_Align_Center("Name "));
                cell = new PdfPCell(new Phrase(tbl.First_Name.ToUpper().Trim() + " " + tbl.Father_Husband_Name.ToUpper() + " " + tbl.Last_Name.ToUpper().Trim()));
                cell.Colspan = 3;
                cell.Padding = 5;
                table.AddCell(cell);

                table.AddCell(Cell_Align_Center("Address "));
                cell = new PdfPCell(new Phrase(tbl.Residential_Address.ToString().Trim()));
                cell.Colspan = 3;
                cell.Padding = 5;
                table.AddCell(cell);

                table.AddCell(Cell_Align_Center("Mobile No "));
                table.AddCell(Cell_Align_Center(tbl.Mobile_No.ToString()));
                table.AddCell(Cell_Align_Center("Email ID:"));
                table.AddCell(Cell_Align_Center(tbl.Email_ID.ToString().Trim()));
                cell.Padding = 5;

                cell = new PdfPCell(new Phrase("Amount Rs. "));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(payment_model.orgTxnAmount));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("Tranaction Status"));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("Success"));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.Padding = 5;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Transaction ID "));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(payment_model.SabPaisaTxId));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("Transaction Time"));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(payment_model.transDate));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.Padding = 5;
                table.AddCell(cell);
                pdfDoc.Add(table);
                pdfDoc.Close();
            }
            catch (Exception ex)
            {
                Random r = new Random();
                int a = r.Next();
                //string path = Server.MapPath("../AppFiles/Error/" + a.ToString() + ".txt");
                string path = Path.Combine(HttpRuntime.AppDomainAppPath, "PDF/SSC/" + a.ToString() + ".txt");
                using (StreamWriter sw = System.IO.File.CreateText(path))
                {
                    sw.WriteLine(ex.ToString());
                }
            }


        }
        public void Create_HSC_PDF(int IDs, Tbl_HSC_Payment payment_model)
        {
            try
            {
                Tbl_HSC_Form17_Final tbl = db.Tbl_HSC_Form17_Final.Where(b => b.Ref_ID == IDs).FirstOrDefault();
                var centerName = db.Tbl_Center.Where(s => s.Index_No == tbl.College_Index_No && s.STD == 12).FirstOrDefault();
                var DistrictName = db.Tbl_Code_Master.Where(m => m.district_code == tbl.District).FirstOrDefault().district_name;
                iTextSharp.text.Image check1 = iTextSharp.text.Image.GetInstance(Path.Combine(HttpRuntime.AppDomainAppPath, "Images/check.png"));
                Document pdfDoc = new Document(PageSize.A4, 25, 25, 25, 15);
                PdfWriter.GetInstance(pdfDoc, new System.IO.FileStream(Path.Combine(HttpRuntime.AppDomainAppPath, "PDF/HSC/" + tbl.S_ID + ".pdf"), FileMode.Create));
                BaseFont bf = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                iTextSharp.text.Font Head_Font = new iTextSharp.text.Font(bf, 20, iTextSharp.text.Font.BOLD);
                iTextSharp.text.Font sub_font = new iTextSharp.text.Font(bf, 14, iTextSharp.text.Font.BOLD);
                iTextSharp.text.Font table_font = new iTextSharp.text.Font(bf, 12, iTextSharp.text.Font.BOLD);
                iTextSharp.text.Font middle_font = new iTextSharp.text.Font(bf, 16, iTextSharp.text.Font.BOLD);

                pdfDoc.Open();

                PdfPTable table = new PdfPTable(2);
                float[] width = new float[] { 50f, 100f };
                table.SetWidths(width);
                table.DefaultCell.FixedHeight = 20f;
                table.WidthPercentage = 90;
                table.DefaultCell.Border = 0;
                table.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right

                PdfPCell cell = new PdfPCell();
                cell.Border = 0;
                iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(Path.Combine(HttpRuntime.AppDomainAppPath, "Images/MSBSHSE-LOGO.png"));
                logo.ScaleAbsolute(100, 50);
                cell.AddElement(logo);
                table.AddCell(cell);

                Paragraph para = new Paragraph("\n Maharashtra State Board of Secondary \n   And Higher Secondary Education,  \n               Pune-411004 ", Head_Font);
                cell = new PdfPCell();
                para.Alignment = 2;
                table.AddCell(para);
                pdfDoc.Add(table);

                List<Tbl_Site_Status> tbl_Site_Status = db.Tbl_Site_Status.ToList();
                if (IsSuperLateForm(Convert.ToDateTime(tbl.Date_Time), Convert.ToDateTime(tbl_Site_Status[0].Super_Late_Fee_Date)))
                {
                    Paragraph suoerlateform = new Paragraph(" Form No:" + tbl.S_ID.ToString() + " [Super Late Form] \n ", sub_font);
                    suoerlateform.Alignment = 2;
                    suoerlateform.IndentationRight = 20;
                    pdfDoc.Add(suoerlateform);
                }
                else if (IsLateForm(Convert.ToDateTime(tbl.Date_Time), Convert.ToDateTime(tbl_Site_Status[0].Late_Fee_Date)))
                {
                    Paragraph lateform = new Paragraph(" Form No:" + tbl.S_ID.ToString() + " [Late Form] \n ", sub_font);
                    lateform.Alignment = 2;
                    lateform.IndentationRight = 20;
                    pdfDoc.Add(lateform);
                }
                else
                {
                    Paragraph para1 = new Paragraph(" Form No:" + tbl.S_ID.ToString() + " \n ", sub_font);
                    para1.Alignment = 2;
                    para1.IndentationRight = 20;
                    pdfDoc.Add(para1);
                }
                Paragraph para2 = new Paragraph(" Form No:" + tbl.S_ID.ToString() + " \n ", sub_font);
                //para2.Alignment = 2;
                //para2.IndentationRight = 20;
                //pdfDoc.Add(para2);

                table = new PdfPTable(2);
                float[] width1 = new float[] { 60f, 90f };
                table.SetWidths(width1);

                table.WidthPercentage = 100;
                table.DefaultCell.Border = 0;
                table.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                table.SpacingAfter = 10;


                string divname = "";
                switch (tbl.Division)
                {
                    case "1":
                        divname = "PUNE";
                        break;
                    case "2":
                        divname = "NAGPUR";
                        break;
                    case "3":
                        divname = "AURANGABAD";
                        break;
                    case "4":
                        divname = "MUMBAI";
                        break;
                    case "5":
                        divname = "KOLHAPUR";
                        break;
                    case "6":
                        divname = "AMRAVATI";
                        break;
                    case "7":
                        divname = "NASHIK";
                        break;
                    case "8":
                        divname = "LATUR";
                        break;
                    case "9":
                        divname = "KOKAN";
                        break;
                }
                cell = new PdfPCell();
                para2 = new Paragraph("To,\nThe Divisional Secretary,\nMaharashtra state board of Secondary\nAnd Higher Secondary Education,\n" + divname + " Divisional Board ", sub_font);
                para2.Alignment = 0;
                para2.Leading = 15;
                cell.Border = 0;
                cell.AddElement(para2);
                table.AddCell(cell);

                cell = new PdfPCell();
                iTextSharp.text.Image passport = iTextSharp.text.Image.GetInstance(Path.Combine(HttpRuntime.AppDomainAppPath, "AppFiles/Images/HSC/" + tbl.Ref_ID + "/Photo.jpeg"));
                passport.ScaleAbsolute(60, 40);
                cell.Border = 0;
                passport.Alignment = 2;
                cell.AddElement(passport);
                table.AddCell(cell);
                pdfDoc.Add(table);

                Paragraph para3 = new Paragraph(" \n  Sir,\n    I request you to issue enrollment Certificate as a Private Candidate at the H.S.C Examination \n (Std.XII) to be held in FEB 2022 and submit the following particulars for your information. \n ", FontFactory.GetFont("Times New Roman", 12));
                para2.Alignment = 0;
                pdfDoc.Add(para3);

                table = new PdfPTable(1);
                float[] width3 = new float[] { 90f };
                table.SetWidths(width3);
                table.SpacingAfter = 20;
                table.WidthPercentage = 80;
                table.DefaultCell.Border = 0;
                table.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                cell = new PdfPCell();
                para2 = new Paragraph(centerName.Index_No + " " + centerName.Name.ToUpper(), table_font);
                para2.Alignment = 1;
                para2.Leading = 14;
                cell.AddElement(para2);
                table.AddCell(cell);
                pdfDoc.Add(table);

                table = new PdfPTable(4);
                float[] width10 = new float[] { 60f, 60f, 60f, 60f };
                table.SetWidths(width10);
                table.DefaultCell.FixedHeight = 30f;
                table.WidthPercentage = 95;
                table.HorizontalAlignment = Element.ALIGN_CENTER;
                table.HorizontalAlignment = 0;
                table.SpacingAfter = 50f;

                cell = new PdfPCell();
                cell = new PdfPCell(new Phrase("REMARKS", table_font));
                cell.Colspan = 2;
                cell.Padding = 5;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("DOCUMENT RECIEVED ", table_font));
                cell.Colspan = 2;
                cell.Padding = 5;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);

                table.AddCell("Eligible");
                table.AddCell(check1);

                table.AddCell("SSC Certificate");
                table.AddCell(check1);

                table.AddCell("Incomplete");
                table.AddCell(check1);

                table.AddCell("L.C./T.C.(Std.X/XI)");
                table.AddCell(check1);

                table.AddCell("Ineligible ");
                table.AddCell(check1);

                table.AddCell("SSC Marksheet");
                table.AddCell(check1);

                PdfPCell cell1 = new PdfPCell(new Phrase("Clerk"));
                cell1.Colspan = 2;

                PdfPTable nestedTable = new PdfPTable(2);
                float[] nestedwidth = new float[] { 60f, 60f };
                nestedTable.SetWidths(nestedwidth);
                nestedTable.DefaultCell.FixedHeight = 20f;
                nestedTable.WidthPercentage = 95;
                nestedTable.HorizontalAlignment = Element.ALIGN_CENTER;

                nestedTable.DefaultCell.Border = 0;
                nestedTable.AddCell(new Paragraph("   "));
                nestedTable.AddCell(new Paragraph("  "));

                nestedTable.AddCell(new Paragraph("  "));
                nestedTable.AddCell(new Paragraph("  "));

                nestedTable.AddCell(new Paragraph("Clerk"));
                nestedTable.AddCell(new Paragraph("  Head of Br"));
                cell1.AddElement(nestedTable);
                table.AddCell(cell1);

                table.AddCell("Affidavit attested Photocopy / True Copy");
                nestedTable = new PdfPTable(1);
                float[] nestedwidth1 = new float[] { 60f };
                nestedTable.SetWidths(nestedwidth1);
                nestedTable.DefaultCell.FixedHeight = 30f;
                nestedTable.WidthPercentage = 95;
                nestedTable.HorizontalAlignment = Element.ALIGN_CENTER;

                nestedTable.HorizontalAlignment = 0;
                nestedTable.DefaultCell.Border = 0;
                PdfPCell cell3 = new PdfPCell(new Phrase("Clerk"));
                check1 = iTextSharp.text.Image.GetInstance(Path.Combine(HttpRuntime.AppDomainAppPath, "Images/check.png"));
                nestedTable.AddCell(check1);
                cell3.AddElement(nestedTable);
                table.AddCell(cell3);

                PdfPCell cell2 = new PdfPCell(new Phrase(""));
                cell2.Colspan = 2;

                nestedTable = new PdfPTable(2);
                float[] nestedwidth14 = new float[] { 60f, 60f };
                nestedTable.SetWidths(nestedwidth14);
                nestedTable.DefaultCell.FixedHeight = 20f;
                nestedTable.WidthPercentage = 95;
                nestedTable.HorizontalAlignment = Element.ALIGN_CENTER;
                nestedTable.DefaultCell.Border = 0;

                nestedTable.AddCell(new Paragraph(""));
                nestedTable.AddCell(new Paragraph(""));

                nestedTable.AddCell(new Paragraph(""));
                nestedTable.AddCell(new Paragraph(""));

                nestedTable.AddCell(new Paragraph("Sr.Supdt."));

                nestedTable.AddCell(new Paragraph("   Jt.Secy"));
                cell2.AddElement(nestedTable);
                table.AddCell(cell2);

                table.AddCell("Bonafide Certificate");
                nestedTable = new PdfPTable(1);
                float[] nestedwidth15 = new float[] { 60f };
                nestedTable.SetWidths(nestedwidth15);
                nestedTable.DefaultCell.FixedHeight = 30f;
                nestedTable.WidthPercentage = 95;
                nestedTable.HorizontalAlignment = Element.ALIGN_CENTER;
                nestedTable.HorizontalAlignment = 0;
                nestedTable.DefaultCell.Border = 0;
                PdfPCell cell4 = new PdfPCell(new Phrase("Clerk"));
                check1 = Image.GetInstance(Path.Combine(HttpRuntime.AppDomainAppPath, "Images/check.png"));
                nestedTable.AddCell(check1);
                cell4.AddElement(nestedTable);
                table.AddCell(cell4);

                cell = new PdfPCell(new Phrase("Migration Certificate"));
                cell.Colspan = 3;
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                cell.Padding = 10;
                table.AddCell(cell);
                table.AddCell(check1);
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);
                pdfDoc.Add(table);

                pdfDoc.NewPage();
                table = new PdfPTable(2);
                float[] width11 = new float[] { 100f, 100f };
                table.SetWidths(width11);
                table.DefaultCell.FixedHeight = 0f;
                table.WidthPercentage = 95;
                table.HorizontalAlignment = Element.ALIGN_CENTER;
                table.HorizontalAlignment = 0;

                table.SpacingAfter = 30f;

                cell = new PdfPCell();
                cell = new PdfPCell(new Phrase("Full Name", sub_font));
                table.AddCell(cell);
                table.AddCell(tbl.First_Name.ToUpper().Trim() + " " + tbl.Father_Husband_Name.ToUpper() + " " + tbl.Last_Name.ToUpper().Trim());

                cell = new PdfPCell(new Phrase("Mother's Name", sub_font));
                table.AddCell(cell);
                table.AddCell(tbl.Mother_Name.ToUpper());

                cell = new PdfPCell(new Phrase("Residential Address", sub_font));
                table.AddCell(cell);
                table.AddCell(tbl.Residential_Address.ToUpper());

                cell = new PdfPCell(new Phrase("Residential PinCode", sub_font));
                table.AddCell(cell);
                table.AddCell(tbl.Pincode);

                cell = new PdfPCell(new Phrase("District", sub_font));
                table.AddCell(cell);
                table.AddCell(DistrictName.ToUpper());

                cell = new PdfPCell(new Phrase("State", sub_font));
                table.AddCell(cell);
                table.AddCell(tbl.State.ToUpper());

                cell = new PdfPCell(new Phrase("Student Email ID", sub_font));
                table.AddCell(cell);
                table.AddCell(tbl.Email_ID.ToUpper());

                cell = new PdfPCell(new Phrase("Mobile Number", sub_font));
                table.AddCell(cell);
                table.AddCell(tbl.Mobile_No);

                cell = new PdfPCell(new Phrase("Aadhar Number ", sub_font));
                table.AddCell(cell);
                table.AddCell(tbl.Aadhar_No);

                cell = new PdfPCell(new Phrase("Gender ", sub_font));
                table.AddCell(cell);
                table.AddCell(tbl.Gender.ToUpper());

                // DateTime DOB_Date = DateTime.Parse(tbl.DOB.ToString());

                string dob = tbl.DOB.ToString();

                cell = new PdfPCell(new Phrase("Birth Date ", sub_font));
                table.AddCell(cell);
                table.AddCell(dob);

                //string dateinwords = "Twenty Second of Feb One Thousand Nine Hundred Ninety Seven";
                string dateinwords = "";
                try
                {
                    dateinwords = ConvertDate(DateTime.Parse(dt_covert.ConversionDate_DDMMYYYY_To_MMDDYYYY(tbl.DOB.ToString())));
                }
                catch (Exception exe)
                { 
                
                }
                cell = new PdfPCell(new Phrase("Birth Date In Words ", sub_font));
                table.AddCell(cell);
                table.AddCell(dateinwords);

                cell = new PdfPCell(new Phrase("Birth Village/town ", sub_font));
                table.AddCell(cell);
                table.AddCell(tbl.Village_of_Birth.ToUpper());

                cell = new PdfPCell(new Phrase("Birth Taluka  ", sub_font));
                table.AddCell(cell);
                table.AddCell(tbl.Taluka_of_Birth.ToUpper());

                cell = new PdfPCell(new Phrase("Birth District ", sub_font));
                table.AddCell(cell);
                table.AddCell(tbl.District_of_Birth.ToUpper());

                cell = new PdfPCell(new Phrase("Month of Passing SSC Exam", sub_font));
                table.AddCell(cell);
                table.AddCell(tbl.SSC_Passing_Month.ToUpper());

                cell = new PdfPCell(new Phrase("Year of Passing SSC Exam", sub_font));
                table.AddCell(cell);
                table.AddCell(tbl.SSC_Passing_Year.ToUpper());

                cell = new PdfPCell(new Phrase("Name of the examination Board or University", sub_font));
                table.AddCell(cell);
                if (tbl.SSC_Division_Board == "OTHER")
                {
                    table.AddCell(tbl.Other_SSC_Board.ToUpper());
                }
                else
                {
                    table.AddCell(tbl.SSC_Division_Board.ToUpper());
                }



                cell = new PdfPCell(new Phrase("Name of Last Attended School ", sub_font));
                table.AddCell(cell);
                table.AddCell(tbl.Name_of_Secondary_School.ToUpper().Trim());

                cell = new PdfPCell(new Phrase("village/town of Last Attended School ", sub_font));
                table.AddCell(cell);
                table.AddCell(tbl.Secondary_School_Village.ToUpper().Trim());

                cell = new PdfPCell(new Phrase("Taluka of Last Attended School ", sub_font));
                table.AddCell(cell);
                table.AddCell(tbl.Secondary_School_Taluka.ToUpper().Trim());

                cell = new PdfPCell(new Phrase("District of Last Attended School ", sub_font));
                table.AddCell(cell);
                table.AddCell(tbl.Secondary_School_District.ToUpper().Trim());

                cell = new PdfPCell(new Phrase("State of Last Attended School  ", sub_font));
                table.AddCell(cell);
                table.AddCell(tbl.Secondary_School_State.ToUpper().Trim());

                cell = new PdfPCell(new Phrase("Udise Code of Last Attended School  ", sub_font));
                table.AddCell(cell);
                if (tbl.Secondary_School_Udise_No != null)
                {
                    table.AddCell(tbl.Secondary_School_Udise_No.ToUpper().Trim());
                }
                else
                {
                    cell = new PdfPCell(new Phrase(" ", sub_font));
                    table.AddCell(cell);
                }

                dob = tbl.Date_of_Leaving_Secondary_School.ToString().Trim();


                cell = new PdfPCell(new Phrase("Date of leaving the last attended School ", sub_font));
                table.AddCell(cell);
                table.AddCell(dob);

                cell = new PdfPCell(new Phrase("I would like to appear in Stream ", sub_font));
                table.AddCell(cell);
                table.AddCell(tbl.Stream_of_Form.Trim().ToUpper());

                cell = new PdfPCell(new Phrase("Medium : ", sub_font));
                table.AddCell(cell);
                table.AddCell(tbl.Medium_of_Form);

                bool AttendedJC = tbl.Attended_Junior_College_Yes_No.Trim() == "Yes" ? true : false;
                cell = new PdfPCell(new Phrase("Name of Last Attended junior College ", sub_font));
                table.AddCell(cell);
                if (AttendedJC)
                {
                    table.AddCell(tbl.Name_of_Junior_College.Trim().ToUpper());
                }
                else
                {
                    cell = new PdfPCell(new Phrase(" ", sub_font));
                    table.AddCell(cell);
                }

                cell = new PdfPCell(new Phrase("Village/Town of Last Attended Junior College ", sub_font));
                table.AddCell(cell);
                if (AttendedJC)
                {
                    table.AddCell(tbl.Junior_College_Village.Trim().ToUpper());
                }
                else
                {
                    cell = new PdfPCell(new Phrase(" ", sub_font));
                    table.AddCell(cell);
                }

                cell = new PdfPCell(new Phrase("District of Last Attended Junior College  ", sub_font));
                table.AddCell(cell);
                if (AttendedJC)
                {
                    table.AddCell(tbl.Junior_College_District.Trim().ToUpper());
                }
                else
                {
                    cell = new PdfPCell(new Phrase(" ", sub_font));
                    table.AddCell(cell);
                }

                cell = new PdfPCell(new Phrase("State of Last Attended Junior College  ", sub_font));
                table.AddCell(cell);
                if (AttendedJC)
                {
                    table.AddCell(tbl.Junior_College_State.Trim().ToUpper());
                }
                else
                {
                    cell = new PdfPCell(new Phrase(" ", sub_font));
                    table.AddCell(cell);
                }

                cell = new PdfPCell(new Phrase("Udise Code of Last Attended Junior College  ", sub_font));
                table.AddCell(cell);
                if (AttendedJC)
                {
                    table.AddCell(tbl.Junior_College_Udise_No);
                }
                else
                {
                    cell = new PdfPCell(new Phrase(" ", sub_font));
                    table.AddCell(cell);
                }

                cell = new PdfPCell(new Phrase("Index Number Of Last Attended Junior College  ", sub_font));
                table.AddCell(cell);
                if (AttendedJC)
                {
                    table.AddCell(tbl.Junior_College_Index_No);
                }
                else
                {
                    cell = new PdfPCell(new Phrase(" ", sub_font));
                    table.AddCell(cell);
                }

                cell = new PdfPCell(new Phrase("The month & Year of passing 1st year of junior college exam  ", sub_font));
                table.AddCell(cell);
                if (AttendedJC)
                {
                    table.AddCell(tbl.FYJC_Passing_Month.Trim().ToUpper() + " " + tbl.FYJC_Passing_Year);
                }
                else
                {
                    cell = new PdfPCell(new Phrase(" ", sub_font));
                    table.AddCell(cell);
                }

                cell = new PdfPCell(new Phrase("Stream of passing 1st year of junior college exam  ", sub_font));
                table.AddCell(cell);
                if (AttendedJC)
                {
                    table.AddCell(tbl.Junior_College_Stream.Trim().ToUpper());
                }
                else
                {
                    cell = new PdfPCell(new Phrase(" ", sub_font));
                    table.AddCell(cell);
                }

                cell = new PdfPCell(new Phrase("Status 1st year of junior college exam  ", sub_font));
                table.AddCell(cell);
                if (AttendedJC)
                {
                    table.AddCell(tbl.FYJC_Passing_Status.Trim().ToUpper());
                }
                else
                {
                    cell = new PdfPCell(new Phrase(" ", sub_font));
                    table.AddCell(cell);
                }

                cell = new PdfPCell(new Phrase("Date of Leaving the last Attended Junior College ", sub_font));
                table.AddCell(cell);
                if (tbl.Date_of_Leaving_Junior_College != null)
                {

                    dob = tbl.Date_of_Leaving_Junior_College.ToString().Trim();
                    table.AddCell(dob);
                }
                else
                {
                    cell = new PdfPCell(new Phrase(" ", sub_font));
                    table.AddCell(cell);
                }

                cell = new PdfPCell(new Phrase("Debarred from appearing at any of the examination ", sub_font));
                table.AddCell(cell);
                table.AddCell(tbl.Declaration_Yes_No.ToUpper().Trim());
                pdfDoc.Add(table);


                Paragraph paragra1 = new Paragraph("Note: Kindly submit application form to college.  \n", FontFactory.GetFont("Times New Roman", 12));
                paragra1.Alignment = 0;
                pdfDoc.Add(paragra1);

                table = new PdfPTable(1);

                float[] width12 = new float[] { 90f };
                table.SetWidths(width12);
                table.SpacingAfter = 7f;
                table.SpacingBefore = 7f;
                table.WidthPercentage = 80;
                table.DefaultCell.Border = 0;
                table.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                cell = new PdfPCell();
                para2 = new Paragraph(centerName.Index_No + " " + centerName.Name.ToUpper().Trim(), table_font);
                para2.Alignment = 1;
                para2.Leading = 14;
                cell.AddElement(para2);
                table.AddCell(cell);
                pdfDoc.Add(table);


                Paragraph paragra2 = new Paragraph("Along with original documents on or before 06/12/2021 \n", FontFactory.GetFont("Times New Roman", 12));
                paragra2.Alignment = 0;
                pdfDoc.Add(paragra2);


                Paragraph paragra3 = new Paragraph("You have to submit following Documents: \n", sub_font);
                paragra3.Alignment = 0;
                pdfDoc.Add(paragra3);


                Paragraph paragra4 = new Paragraph(" * Original/Duplicate Secondary School Certificate and Marksheet(s) and one Xerox copy or true copy of each of it attested by the principal.\n\n", FontFactory.GetFont("Times New Roman", 12));
                paragra4.Alignment = 0;
                pdfDoc.Add(paragra4);

                Paragraph paragra5 = new Paragraph(" * Original LC/TC OR Duplicate School leaving Certificate and its attested true copy with self attested Affidavit (Original).\n\n", FontFactory.GetFont("Times New Roman", 12));
                paragra5.Alignment = 0;
                pdfDoc.Add(paragra5);

                if (tbl.FYJC_Leaving_Certificate_Yes_No == "Yes")
                {
                    Paragraph paragra6 = new Paragraph(" * Junior college Leaving Certificate and its one Xerox copy or true copy attested by the principal.\n\n", FontFactory.GetFont("Times New Roman", 12));
                    paragra6.Alignment = 0;
                    pdfDoc.Add(paragra6);
                }

                para2 = new Paragraph("Signature of the Applicant ", FontFactory.GetFont("Times New Roman", 12));
                para2.Alignment = 2;
                para2.IndentationRight = 60;
                pdfDoc.Add(para2);

                para2 = new Paragraph("(" + tbl.First_Name.Trim().ToUpper() + " " + tbl.Father_Husband_Name.Trim().ToUpper() + " " + tbl.Last_Name.Trim().ToUpper() + ")", FontFactory.GetFont("Times New Roman", 12));
                para2.Alignment = 2;
                para2.IndentationRight = 60;
                pdfDoc.Add(para2);

                pdfDoc.NewPage();
                para = new Paragraph("MAHARASHTRA STATE BOARD OF SECONDARY AND HIGHER", middle_font);

                para.Alignment = 0;
                pdfDoc.Add(para);

                para = new Paragraph("SECONDARY EDUCATION, PUNE", middle_font);
                para.Alignment = 1;
                para.IndentationRight = 70;
                pdfDoc.Add(para);

                para = new Paragraph(" Private Candidate Application (Form No:17)", sub_font);
                para.Alignment = 1;
                para.IndentationRight = 70;
                pdfDoc.Add(para);

                para = new Paragraph("Payment Receipt", sub_font);
                para.Alignment = 1;
                para.IndentationRight = 70;
                pdfDoc.Add(para);


                var division = db.Tbl_Code_Master.Where(x => x.district_code == tbl.District_of_Form_Submission).FirstOrDefault().division_name;
                paragra1 = new Paragraph("Division: " + division + " \n", FontFactory.GetFont("Times New Roman", 12));
                paragra1.Alignment = 0;
                pdfDoc.Add(paragra1);

                table = new PdfPTable(4);
                float[] width14 = new float[] { 60f, 60f, 60f, 60f };
                table.SetWidths(width14);
                table.DefaultCell.FixedHeight = 90f;
                table.WidthPercentage = 95;
                table.HorizontalAlignment = Element.ALIGN_CENTER;
                table.HorizontalAlignment = 0;

                table.SpacingAfter = 7f;
                table.SpacingBefore = 7f;

                cell = new PdfPCell();

                table.AddCell(Cell_Align_Center("Application No "));
                table.AddCell(Cell_Align_Center(tbl.S_ID));
                table.AddCell(Cell_Align_Center("Exam"));
                table.AddCell(Cell_Align_Center("HSC"));
                table.AddCell(Cell_Align_Center("Name "));
                cell = new PdfPCell(new Phrase(tbl.First_Name.ToUpper().Trim() + " " + tbl.Father_Husband_Name.ToUpper() + " " + tbl.Last_Name.ToUpper().Trim()));
                cell.Colspan = 3;
                cell.Padding = 5;
                table.AddCell(cell);

                table.AddCell(Cell_Align_Center("Address "));
                cell = new PdfPCell(new Phrase(tbl.Residential_Address.Trim()));
                cell.Colspan = 3;
                cell.Padding = 5;
                table.AddCell(cell);

                table.AddCell(Cell_Align_Center("Mobile No "));
                table.AddCell(Cell_Align_Center(tbl.Mobile_No));
                table.AddCell(Cell_Align_Center("Email ID:"));
                table.AddCell(Cell_Align_Center(tbl.Email_ID));
                cell.Padding = 5;

                cell = new PdfPCell(new Phrase("Amount Rs."));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(payment_model.orgTxnAmount));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("Tranaction Status"));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("Success"));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.Padding = 5;
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase("Transaction ID "));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(payment_model.SabPaisaTxId));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("Transaction Time"));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(payment_model.transDate));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.Padding = 5;
                table.AddCell(cell);
                pdfDoc.Add(table);
                pdfDoc.Close();                
            }
            catch (Exception ex)
            {
                Random r = new Random();
                int a = r.Next();
                //string path = Server.MapPath("../AppFiles/Error/" + a.ToString() + ".txt");
                string path = Path.Combine(HttpRuntime.AppDomainAppPath, "PDF/HSC/" + a.ToString() + ".txt");
                using (StreamWriter sw = System.IO.File.CreateText(path))
                {
                    sw.WriteLine(ex.ToString());
                }
            }


        }

        public string ConvertDate(DateTime date)
        {
            string[] ordinals =
            {
                "First",
                "Second",
                "Third",
                "Fourth",
                "Fifth",
                "Sixth",
                "Seventh",
                "Eighth",
                "Ninth",
                "Tenth",
                "Eleventh",
                "Twelfth",
                "Thirteenth",
                "Fourteenth",
                "Fifteenth",
                "Sixteenth",
                "Seventeenth",
                "Eighteenth",
                "Nineteenth",
                "Twentieth",
                "Twenty First",
                "Twenty Second",
                "Twenty Third",
                "Twenty Fourth",
                "Twenty Fifth",
                "Twenty Sixth",
                "Twenty Seventh",
                "Twenty Eighth",
                "Twenty Ninth",
                "Thirtieth",
                "Thirty First"
            };
            int day = Int32.Parse(date.Day.ToString());
            int month = Int32.Parse(date.Month.ToString());
            int year = Int32.Parse(date.Year.ToString());
            DateTime dtm = new DateTime(1, month, 1);
            string DateInWords = ordinals[day - 1] + " of " + dtm.ToString("MMMM") + " " + NumberToText(year, true);
            return DateInWords;
        }
        public static string NumberToText(int number, bool isUK)
        {
            if (number == 0) return "Zero";
            string and = isUK ? "and " : ""; // deals with UK or US numbering           
            int[] num = new int[4];
            int first = 0;
            int u, h, t;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            if (number < 0)
            {
                sb.Append("Minus ");
                number = -number;
            }
            string[] words0 = { "", "One ", "Two ", "Three ", "Four ", "Five ", "Six ", "Seven ", "Eight ", "Nine " };
            string[] words1 = { "Ten ", "Eleven ", "Twelve ", "Thirteen ", "Fourteen ", "Fifteen ", "Sixteen ", "Seventeen ", "Eighteen ", "Nineteen " };
            string[] words2 = { "Twenty ", "Thirty ", "Forty ", "Fifty ", "Sixty ", "Seventy ", "Eighty ", "Ninety " };
            string[] words3 = { "Thousand ", "Million ", "Billion " };
            num[0] = number % 1000;           // units
            num[1] = number / 1000;
            num[2] = number / 1000000;
            num[1] = num[1] - 1000 * num[2];  // thousands
            num[3] = number / 1000000000;     // billions
            num[2] = num[2] - 1000 * num[3];  // millions
            for (int i = 3; i > 0; i--)
            {
                if (num[i] != 0)
                {
                    first = i;
                    break;
                }
            }
            for (int i = first; i >= 0; i--)
            {
                if (num[i] == 0) continue;
                u = num[i] % 10;              // ones
                t = num[i] / 10;
                h = num[i] / 100;             // hundreds
                t = t - 10 * h;               // tens
                if (h > 0) sb.Append(words0[h] + "Hundred ");
                if (u > 0 || t > 0)
                {
                    if (h > 0 || i < first) sb.Append(and);
                    if (t == 0)
                        sb.Append(words0[u]);
                    else if (t == 1)
                        sb.Append(words1[u]);
                    else
                        sb.Append(words2[t - 2] + words0[u]);
                }
                if (i != 0) sb.Append(words3[i - 1]);
            }
            return sb.ToString().TrimEnd();
        }
        private static PdfPCell Cell_Align_Center(string Value)
        {
            PdfPCell cell = new PdfPCell(new Phrase(Value));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            return cell;
        }
    }
}