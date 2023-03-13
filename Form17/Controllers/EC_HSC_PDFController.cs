using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Web.Mvc;
using System.IO;
using System.Configuration;
using DocumentFormat.OpenXml.Vml;
//using iText.IO.Image;
using Form17.Models;

namespace Form17.Controllers
{
    public class EC_HSC_PDFController : Controller
    {
       

        // GET: EC_PDF
        public void Create_HSC_EC_PDF(EC_HSC_ModelController ec)
        {
          
            var date = DateTime.Now.ToString("d/M/yyyy");

            Document document = new Document(PageSize.A4, 25, 25, 30, 30);

            Random r = new Random();

            PdfWriter.GetInstance(document, new System.IO.FileStream(System.IO.Path.Combine(HttpRuntime.AppDomainAppPath, "PDF/HSC_EC/" + ec.S_ID + ".pdf"), FileMode.Create));

            int totalfonts = FontFactory.RegisterDirectory("C:\\WINDOWS\\Fonts");
            Font fdefault12 = FontFactory.GetFont("Arial", 12, Font.NORMAL, BaseColor.BLACK);
            Font fdefault1 = FontFactory.GetFont("TIMES NEW ROMAN", 14, Font.BOLD, BaseColor.BLACK);
            Font fontb = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12, BaseColor.BLACK);
            Font fdefault2 = FontFactory.GetFont("Arial", 12, Font.BOLD, BaseColor.BLACK);
            Font Notes = FontFactory.GetFont("Arial", 11, Font.NORMAL, BaseColor.BLACK);
            Font Enroll = FontFactory.GetFont("TIMES NEW ROMAN", 14, Font.BOLD, BaseColor.RED);


            document.Open();

            PdfPTable table = new PdfPTable(6);

            table.TotalWidth = 508;

            table.LockedWidth = true;

            //////////////////////////////////////////////////////////////////////////////////
            iTextSharp.text.Image img2 = iTextSharp.text.Image.GetInstance(System.IO.Path.Combine(HttpRuntime.AppDomainAppPath, "Design/images/MSBSHSE-LOGO.png"));
            img2.ScaleToFit(60.0f, 50.0f);
            var ph2 = new Phrase();
          
            ph2.Add(new Chunk(img2, -2, -33));
            ph2.Add(new Chunk("       Maharashtra State Board of Secondary & Higher\nSecondary Education, Pune 411005\n", fdefault1));
            ph2.Add(new Chunk("       Enrollment Certificate\r", Enroll));
            iTextSharp.text.pdf.PdfPCell imgCell22 = new iTextSharp.text.pdf.PdfPCell(ph2);
            imgCell22.Colspan = 6;
            imgCell22.PaddingBottom = 5;
            imgCell22.HorizontalAlignment = Element.ALIGN_CENTER;
            imgCell22.Border = iTextSharp.text.Rectangle.BOX;
            imgCell22.BorderColor = new BaseColor(System.Drawing.Color.Black);
            table.AddCell(imgCell22);
           
            /////////////////////////////////////////////////////////////////////////////////           

            var phars1 = new Phrase();
            phars1.Add(new Chunk("Certified that the candidate whose particulars are mentioned below is permitted to appear as a PRIVATE CANDIDATE for the regular H.S.C Examination of", fdefault12));
            phars1.Add(new Chunk(" Year "+ec.HSC_Year+" ", fontb));
            phars1.Add(new Chunk("or any subsequent H.S.C Examination, Subject to rules governing admission of Private Candidate's to the Examination,through the recognised High School", fdefault12));

            PdfPCell cell12 = new PdfPCell(phars1);
            cell12.Colspan = 5;
            cell12.PaddingBottom = 10;
            cell12.HorizontalAlignment = Element.ALIGN_JUSTIFIED;
            cell12.PaddingLeft = 20f;
            cell12.PaddingRight = 10f;
            cell12.Border = iTextSharp.text.Rectangle.LEFT_BORDER;
            cell12.BorderColor = new BaseColor(System.Drawing.Color.Black);
            table.AddCell(cell12);

            /////////////////////////////////////////////////////////////////////////

            iTextSharp.text.Image img1 = iTextSharp.text.Image.GetInstance(System.IO.Path.Combine(HttpRuntime.AppDomainAppPath, "AppFiles/Images/HSC/"+ec.Ref_ID+"/Photo.jpeg"));
            img1.Alignment = iTextSharp.text.Image.ALIGN_CENTER;
            img1.ScaleAbsolute(80.0f, 70.0f);
            iTextSharp.text.pdf.PdfPCell imgCell1 = new iTextSharp.text.pdf.PdfPCell(img1);
            imgCell1.HorizontalAlignment = Element.ALIGN_CENTER;
            imgCell1.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
            imgCell1.BorderColor = new BaseColor(System.Drawing.Color.Black);
            table.AddCell(imgCell1);
            //////////////////////////////////////////////////////////////////////////////////

            var phars12 = new Phrase();
            phars12.Add(new Chunk("  CANDIDATE'S NAME : ", fontb));
            phars12.Add(new Chunk(ec.Candidate_Name, fontb));
            phars12.Add(new Chunk("\r\r  MOTHER'S NAME : ", fontb));
            phars12.Add(new Chunk(ec.Mother_Name, fontb));

            PdfPCell cell = new PdfPCell(new Phrase(phars12));
            cell.PaddingBottom = 5;
            cell.Colspan = 6;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.BorderColor = new BaseColor(System.Drawing.Color.Black);
            table.AddCell(cell);

            ///////////////////////////////////////////////////////////////////////
            PdfPCell cellop1 = new PdfPCell(new Phrase("DIVISIONAL BOARD", fdefault12));
            cellop1.PaddingBottom = 5;
            cellop1.BorderColor = new BaseColor(System.Drawing.Color.Black);
            cellop1.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(cellop1);

            PdfPCell cellop2 = new PdfPCell(new Phrase("DATE OF BIRTH\r(dd/mm/yyyy)", fdefault12));
            cellop2.PaddingBottom = 5;
            cellop2.BorderColor = new BaseColor(System.Drawing.Color.Black);
            cellop2.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(cellop2);

            PdfPCell cellop3 = new PdfPCell(new Phrase("M/F", fdefault12));
            cellop3.PaddingBottom = 5;
            cellop3.BorderColor = new BaseColor(System.Drawing.Color.Black);
            cellop3.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(cellop3);

            PdfPCell cellop4 = new PdfPCell(new Phrase("Jr.COLLEGE NO.", fdefault12));
            cellop3.PaddingBottom = 5;
            cellop4.BorderColor = new BaseColor(System.Drawing.Color.Black);
            cellop4.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(cellop4);


            PdfPCell cellop51 = new PdfPCell(new Phrase("STREAM", fdefault12));
            cellop51.PaddingBottom = 5;
            cellop51.BorderColor = new BaseColor(System.Drawing.Color.Black);
            cellop51.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(cellop51);

            PdfPCell cellop5 = new PdfPCell(new Phrase("EC NUMBER", fdefault12));
            cellop5.PaddingBottom = 5;
            cellop5.BorderColor = new BaseColor(System.Drawing.Color.Black);
            cellop5.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(cellop5);

            ////////////////////////////////////////////////////////////

            PdfPCell cello14 = new PdfPCell(new Phrase(ec.Divisional_Board, fontb));
            cello14.PaddingBottom = 5;
            cello14.BorderColor = new BaseColor(System.Drawing.Color.Black);
            cello14.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(cello14);
            ////////////////////////////////////////////////////////////////////

            PdfPCell cello = new PdfPCell(new Phrase(ec.Birth_date, fontb));
            cello.PaddingBottom = 5;
            cello.BorderColor = new BaseColor(System.Drawing.Color.Black);
            cello.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(cello);
            /////////////////////////////////////////////////////////////////////

            PdfPCell cello1 = new PdfPCell(new Phrase(ec.Gender, fontb));
            cello1.PaddingBottom = 5;
            cello1.BorderColor = new BaseColor(System.Drawing.Color.Black);
            cello1.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(cello1);

            ///////////////////////////////////////////////////////////////////////////

            PdfPCell cello12 = new PdfPCell(new Phrase(ec.College_No, fontb));
            cello12.PaddingBottom = 5;
            cello12.BorderColor = new BaseColor(System.Drawing.Color.Black);
            cello12.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(cello12);

            ///////////////////////////////////////////////////////////////////////

            PdfPCell cello13 = new PdfPCell(new Phrase(ec.Stream, fontb));
            cello13.PaddingBottom = 5;
            cello13.BorderColor = new BaseColor(System.Drawing.Color.Black);
            cello13.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(cello13);

            ////////////////////////////////////////////////////////////////////////

            PdfPCell cello131 = new PdfPCell(new Phrase(ec.EC_No, fontb));
            cello131.PaddingBottom = 5;
            cello131.BorderColor = new BaseColor(System.Drawing.Color.Black);
            cello131.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(cello131);
            /////////////////////////////////////////////////////////////////////////////////////////////////

            var cell6 = new Phrase();
            cell6.Add(new Chunk("Name & Address of the School : "+ec.College_Addr, fdefault2));
            PdfPCell Sch_Name = new PdfPCell(new Phrase(cell6));
            Sch_Name.Colspan = 6;

            Sch_Name.HorizontalAlignment = 3;

            table.AddCell(Sch_Name);

            /////////////////////////////////////////////////////////////////////////////

            var cell7 = new Phrase();
            cell7.Add(new Chunk(" DATE OF ISSUE:"+ec.Date_Issue+"                                               DATE OF DOWNLOAD:"+date, fdefault12));
            PdfPCell DOI = new PdfPCell(new Phrase(cell7));
            DOI.Colspan = 6;

            DOI.HorizontalAlignment = 3;

            table.AddCell(DOI);
            ////////////////////////////////////////////////////////////////////////////////////////////

            PdfPCell Notes2 = new PdfPCell(new Phrase("Notes: \n\n1) Student must submit examination form for H.S.C examination from above mentioned school only. \n\n2)This Certificate Must be preserved and EC Number must be entered on candidate's examination application form for admission to the examination.\n\n3) This Certificate shall be valid for admission to the H.S.C Examination held in March or July of any year unless otherwise notified.", Notes));

            Notes2.Colspan = 5;

            table.AddCell(Notes2);

            //////////////////////////////////////////////////////////////////////////////

            iTextSharp.text.Image Sign = iTextSharp.text.Image.GetInstance(System.IO.Path.Combine(HttpRuntime.AppDomainAppPath, GetSignaturePath(ec.Division)));
            Sign.ScaleAbsolute(70.0f, 70.0f);
            var ph = new Phrase();
            ph.Add(new Chunk("\r"));
            ph.Add(new Chunk(Sign, 0, 0));
            ph.Add(new Chunk("\r\r"));
            ph.Add(new Chunk("Divisional\rSecretary\r", fdefault2));
            iTextSharp.text.pdf.PdfPCell imgCell12 = new iTextSharp.text.pdf.PdfPCell(ph);
            imgCell12.PaddingBottom = 5;
            imgCell12.HorizontalAlignment = Element.ALIGN_CENTER;
            imgCell12.Border = iTextSharp.text.Rectangle.RIGHT_BORDER;
            imgCell12.BorderColor = new BaseColor(System.Drawing.Color.Black);

            table.AddCell(imgCell12);
           
            //////////////////////////////////////////////////////////////////

            var cell8 = new Phrase();
            cell8.Add(new Chunk("CASE NO :", fontb));
            cell8.Add(new Chunk("  "+ec.Case_No+"\n\n"));

            PdfPCell Case_No = new PdfPCell(new Phrase(cell8));

            Case_No.Colspan = 6;

            Case_No.HorizontalAlignment = 3;

            table.AddCell(Case_No);

            document.Add(table);

            document.Close();
        }

        public string GetSignaturePath(string Div)
        {
            string str = "";
            switch (Div)
            {
                case "1": str = "Images/DivChairmanSigns/Pune.png"; break;
                case "2": str = "Images/DivChairmanSigns/Nagpur.png"; break;
                case "3": str = "Images/DivChairmanSigns/Aurangabad.png"; break;
                case "4": str = "Images/DivChairmanSigns/Mumbai.png"; break;
                case "5": str = "Images/DivChairmanSigns/Kolhapur.png"; break;
                case "6": str = "Images/DivChairmanSigns/Amravati.png"; break;
                case "7": str = "Images/DivChairmanSigns/Nashik.png"; break;
                case "8": str = "Images/DivChairmanSigns/Latur.png"; break;
                case "9": str = "Images/DivChairmanSigns/Kokan.png"; break;
            }
            return str;
        }
    }
   
}