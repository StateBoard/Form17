using Form17.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Form17.Controllers
{
    public class PaymentController : Controller
    {
        Form17Entities db = new Form17Entities();
        PDFMaker pdfMaker = new PDFMaker();
        // GET: Payment
        [HandleError]
        public ActionResult SSC_Success()
        {
            try
            {
                SabPaisaIntegration sabPaisaIntegration = new SabPaisaIntegration();
                Dictionary<string, string> sabPaisaRespdict = new Dictionary<string, string>();

                string query = Request.QueryString["query"].ToString();
                string authIV = Common_SSC.Payment_Authentication_IV();//"qz7zPW07upqREhuo";
                string authKey = Common_SSC.Payment_Authentication_KEY();//"vuQy2eFx4q095E03";
                sabPaisaRespdict = sabPaisaIntegration.subPaisaResponse(query, authIV, authKey);
                string Status = "", UniqueID = "", Seat_No = "", Transaction_ID = "", Div_Code = "", Payment_Head = "", Payment_Head_Description = "";
                int Pre_Ref_ID = 0;
                int result = 0;
                SqlConnection _Con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString1"].ConnectionString);
                SqlCommand insert_command = new SqlCommand(@"insert into Tbl_SSC_Payment (pgRespCode,	PGTxnNo,	SabPaisaTxId,	issuerRefNo,	authIdCode,	amount,	clientTxnId,	firstName,	lastName,	payMode,	email,	mobileNo,	spRespCode,	cid,	bid,	clientCode,	payeeProfile,	transDate,	spRespStatus,	m3,	challanNo,	reMsg,	orgTxnAmount,	programId,	midName,	[Add],	param1,	param2,	param3,	param4,	udf5,	udf6,	udf7,	udf8,	udf9,	udf10,	udf11,	udf12,	udf13,	udf14,	udf15,	udf16,	udf17,	udf18,	udf19,	udf20) 
                                                                                   values(@pgRespCode,	@PGTxnNo,	@SabPaisaTxId,	@issuerRefNo,	@authIdCode,	@amount,	@clientTxnId,	@firstName,	@lastName,	@payMode,	@email,	@mobileNo,	@spRespCode,	@cid,	@bid,	@clientCode,	@payeeProfile,	@transDate,	@spRespStatus,	@m3,	@challanNo,	@reMsg,	@orgTxnAmount,	@programId,	@midName,	@Add,	@param1,	@param2,	@param3,	@param4,	@udf5,	@udf6,	@udf7,	@udf8,	@udf9,	@udf10,	@udf11,	@udf12,	@udf13,	@udf14,	@udf15,	@udf16,	@udf17,	@udf18,	@udf19,	@udf20)", _Con);

                foreach (KeyValuePair<string, string> pair in sabPaisaRespdict)
                {

                    // Temp +=  " <br /> " + pair.Key.ToString() + "  -  " + pair.Value.ToString();
                    insert_command.Parameters.AddWithValue("@" + pair.Key.ToString(), pair.Value.ToString());
                    switch (pair.Key.ToString())
                    {
                        case "spRespStatus":
                            Status = pair.Value.ToString();
                            break;
                        case "clientTxnId":
                            Transaction_ID = pair.Value.ToString();
                            break;
                        case "udf17":
                            Div_Code = pair.Value.ToString();
                            break;
                        case "udf18":
                            Pre_Ref_ID = Convert.ToInt32(pair.Value.ToString());
                            break;
                        case "udf19":
                            Seat_No = pair.Value.ToString();
                            break;
                    }
                }

                try
                {
                    var tbl = (from A in db.Tbl_SSC_Form17
                               where A.ID == Pre_Ref_ID
                               select new
                               {
                                   A.ID,
                                   A.Division,
                               }).FirstOrDefault();

                    int anyValue = 0;
                    ObjectParameter reportId = new ObjectParameter("RESULT", anyValue);
                    db.INSERT_DATA_SSC(Pre_Ref_ID.ToString(), tbl.Division, reportId);
                    if (_Con.State != ConnectionState.Open)
                    {
                        _Con.Close();
                    }
                    _Con.Open();
                    if (insert_command.ExecuteNonQuery() > 0)
                    { _Con.Close(); }
                    else
                    {
                        insert_command = new SqlCommand(@"insert into Tbl_SSC_Payment_Error (pgRespCode,	PGTxnNo,	SabPaisaTxId,	issuerRefNo,	authIdCode,	amount,	clientTxnId,	firstName,	lastName,	payMode,	email,	mobileNo,	spRespCode,	cid,	bid,	clientCode,	payeeProfile,	transDate,	spRespStatus,	m3,	challanNo,	reMsg,	orgTxnAmount,	programId,	midName,	[Add],	param1,	param2,	param3,	param4,	udf5,	udf6,	udf7,	udf8,	udf9,	udf10,	udf11,	udf12,	udf13,	udf14,	udf15,	udf16,	udf17,	udf18,	udf19,	udf20) 
                                                                                   values(@pgRespCode,	@PGTxnNo,	@SabPaisaTxId,	@issuerRefNo,	@authIdCode,	@amount,	@clientTxnId,	@firstName,	@lastName,	@payMode,	@email,	@mobileNo,	@spRespCode,	@cid,	@bid,	@clientCode,	@payeeProfile,	@transDate,	@spRespStatus,	@m3,	@challanNo,	@reMsg,	@orgTxnAmount,	@programId,	@midName,	@Add,	@param1,	@param2,	@param3,	@param4,	@udf5,	@udf6,	@udf7,	@udf8,	@udf9,	@udf10,	@udf11,	@udf12,	@udf13,	@udf14,	@udf15,	@udf16,	@udf17,	@udf18,	@udf19,	@udf20)", _Con);
                        insert_command.ExecuteNonQuery();

                        _Con.Close();
                    }
                }
                catch (Exception ex)
                {

                }
                SSC_Success_View succes_view =new  SSC_Success_View();
                var model2 =  (from A in db.Tbl_SSC_Payment
                              join B in db.Tbl_SSC_Form17_Final on A.udf18 equals B.Ref_ID.ToString()
                              where A.udf18 == Pre_Ref_ID.ToString()
                              select new 
                               {
                                 A,B
                               }).FirstOrDefault();

                succes_view.sid = model2.B.S_ID;
                succes_view.SabPaisaTxId = model2.A.SabPaisaTxId;
                succes_view.issuerRefNo = model2.A.issuerRefNo;
                succes_view.transDate = model2.A.transDate;
                succes_view.amount = model2.A.amount;

                var model = db.Tbl_SSC_Payment.Where(x => x.udf18 == Pre_Ref_ID.ToString()).FirstOrDefault();
                pdfMaker.Create_SSC_PDF(Pre_Ref_ID, model);
                return View(succes_view);
            }
            catch (Exception)
            {

            }
            return View();
        }
        [HandleError]
        public ActionResult SSC_Failure()
        {
            SabPaisaIntegration sabPaisaIntegration = new SabPaisaIntegration();
            Dictionary<string, string> sabPaisaRespdict = new Dictionary<string, string>();

            string query = Request.QueryString["query"].ToString();
            string authIV = Common_SSC.Payment_Authentication_IV();//"qz7zPW07upqREhuo";
            string authKey = Common_SSC.Payment_Authentication_KEY();//"vuQy2eFx4q095E03";
            sabPaisaRespdict = sabPaisaIntegration.subPaisaResponse(query, authIV, authKey);
            string Status = "", UniqueID = "", Seat_No = "", Transaction_ID = "", Div_Code = "", Pre_Ref_ID = "", Payment_Head = "", Payment_Head_Description = "";
            int result = 0;
            SqlConnection _Con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString1"].ConnectionString);
            SqlCommand insert_command = new SqlCommand(@"insert into Tbl_SSC_Payment_Error (pgRespCode,	PGTxnNo,	SabPaisaTxId,	issuerRefNo,	authIdCode,	amount,	clientTxnId,	firstName,	lastName,	payMode,	email,	mobileNo,	spRespCode,	cid,	bid,	clientCode,	payeeProfile,	transDate,	spRespStatus,	m3,	challanNo,	reMsg,	orgTxnAmount,	programId,	midName,	[Add],	param1,	param2,	param3,	param4,	udf5,	udf6,	udf7,	udf8,	udf9,	udf10,	udf11,	udf12,	udf13,	udf14,	udf15,	udf16,	udf17,	udf18,	udf19,	udf20) 
                                                                                   values(@pgRespCode,	@PGTxnNo,	@SabPaisaTxId,	@issuerRefNo,	@authIdCode,	@amount,	@clientTxnId,	@firstName,	@lastName,	@payMode,	@email,	@mobileNo,	@spRespCode,	@cid,	@bid,	@clientCode,	@payeeProfile,	@transDate,	@spRespStatus,	@m3,	@challanNo,	@reMsg,	@orgTxnAmount,	@programId,	@midName,	@Add,	@param1,	@param2,	@param3,	@param4,	@udf5,	@udf6,	@udf7,	@udf8,	@udf9,	@udf10,	@udf11,	@udf12,	@udf13,	@udf14,	@udf15,	@udf16,	@udf17,	@udf18,	@udf19,	@udf20)", _Con);
            foreach (KeyValuePair<string, string> pair in sabPaisaRespdict)
            {

                // Temp +=  " <br /> " + pair.Key.ToString() + "  -  " + pair.Value.ToString();
                insert_command.Parameters.AddWithValue("@" + pair.Key.ToString(), pair.Value.ToString());
                if (pair.Key.ToString() == "spRespStatus")
                {
                    Status = pair.Value.ToString();
                }
                if (pair.Key.ToString() == "clientTxnId")
                {
                    Transaction_ID = pair.Value.ToString();
                }
                if (pair.Key.ToString() == "udf17")
                {
                    Div_Code = pair.Value.ToString();
                }
                if (pair.Key.ToString() == "udf18")
                {
                    UniqueID = pair.Value.ToString();
                }
            }
            try
            {
                _Con.Open();
                if (insert_command.ExecuteNonQuery() > 0)
                { _Con.Close(); }
                else
                { _Con.Close(); }
            }
            catch (Exception)
            {

            }
            var model = db.Tbl_SSC_Payment_Error.Where(x => x.udf18 == UniqueID).FirstOrDefault();
            return View(model);
        }

        public ActionResult HSC_Success()
        {
            try
            {
                SabPaisaIntegration sabPaisaIntegration = new SabPaisaIntegration();
                Dictionary<string, string> sabPaisaRespdict = new Dictionary<string, string>();

                string query = Request.QueryString["query"].ToString();
                string authIV = Common_HSC.Payment_Authentication_IV();//"qz7zPW07upqREhuo";
                string authKey = Common_HSC.Payment_Authentication_KEY();//"vuQy2eFx4q095E03";
                sabPaisaRespdict = sabPaisaIntegration.subPaisaResponse(query, authIV, authKey);
                string Status = "", UniqueID = "", Seat_No = "", Transaction_ID = "", Div_Code = "", Payment_Head = "", Payment_Head_Description = "";
                int result = 0, Pre_Ref_ID = 0;
                SqlConnection _Con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString1"].ConnectionString);
                SqlCommand insert_command = new SqlCommand(@"insert into Tbl_HSC_Payment (pgRespCode,	PGTxnNo,	SabPaisaTxId,	issuerRefNo,	authIdCode,	amount,	clientTxnId,	firstName,	lastName,	payMode,	email,	mobileNo,	spRespCode,	cid,	bid,	clientCode,	payeeProfile,	transDate,	spRespStatus,	m3,	challanNo,	reMsg,	orgTxnAmount,	programId,	midName,	[Add],	param1,	param2,	param3,	param4,	udf5,	udf6,	udf7,	udf8,	udf9,	udf10,	udf11,	udf12,	udf13,	udf14,	udf15,	udf16,	udf17,	udf18,	udf19,	udf20) 
                                                                                   values(@pgRespCode,	@PGTxnNo,	@SabPaisaTxId,	@issuerRefNo,	@authIdCode,	@amount,	@clientTxnId,	@firstName,	@lastName,	@payMode,	@email,	@mobileNo,	@spRespCode,	@cid,	@bid,	@clientCode,	@payeeProfile,	@transDate,	@spRespStatus,	@m3,	@challanNo,	@reMsg,	@orgTxnAmount,	@programId,	@midName,	@Add,	@param1,	@param2,	@param3,	@param4,	@udf5,	@udf6,	@udf7,	@udf8,	@udf9,	@udf10,	@udf11,	@udf12,	@udf13,	@udf14,	@udf15,	@udf16,	@udf17,	@udf18,	@udf19,	@udf20)", _Con);

                foreach (KeyValuePair<string, string> pair in sabPaisaRespdict)
                {
                    // Temp +=  " <br /> " + pair.Key.ToString() + "  -  " + pair.Value.ToString();
                    insert_command.Parameters.AddWithValue("@" + pair.Key.ToString(), pair.Value.ToString());
                    switch (pair.Key.ToString())
                    {
                        case "spRespStatus":
                            Status = pair.Value.ToString();
                            break;
                        case "clientTxnId":
                            Transaction_ID = pair.Value.ToString();
                            break;
                        case "udf17":
                            Div_Code = pair.Value.ToString();
                            break;
                        case "udf18":
                            Pre_Ref_ID = Convert.ToInt32(pair.Value.ToString());
                            break;
                        case "udf19":
                            Seat_No = pair.Value.ToString();
                            break;
                    }
                }

                try
                {
                    var tbl = (from A in db.Tbl_HSC_Form17
                               where A.ID == Pre_Ref_ID
                               select new
                               {
                                   A.ID,
                                   A.Division,
                               }).FirstOrDefault();

                    int anyValue = 0;
                    ObjectParameter reportId = new ObjectParameter("RESULT", anyValue);
                    db.INSERT_DATA_HSC(Pre_Ref_ID.ToString(), tbl.Division, reportId);
                    if (_Con.State != ConnectionState.Open)
                    {
                        _Con.Close();
                    }
                    _Con.Open();
                    if (insert_command.ExecuteNonQuery() > 0)
                    {
                        _Con.Close();

                    }
                    else
                    {
                        insert_command = new SqlCommand(@"insert into Tbl_HSC_Payment_Error (pgRespCode,	PGTxnNo,	SabPaisaTxId,	issuerRefNo,	authIdCode,	amount,	clientTxnId,	firstName,	lastName,	payMode,	email,	mobileNo,	spRespCode,	cid,	bid,	clientCode,	payeeProfile,	transDate,	spRespStatus,	m3,	challanNo,	reMsg,	orgTxnAmount,	programId,	midName,	[Add],	param1,	param2,	param3,	param4,	udf5,	udf6,	udf7,	udf8,	udf9,	udf10,	udf11,	udf12,	udf13,	udf14,	udf15,	udf16,	udf17,	udf18,	udf19,	udf20) 
                                                                                   values(@pgRespCode,	@PGTxnNo,	@SabPaisaTxId,	@issuerRefNo,	@authIdCode,	@amount,	@clientTxnId,	@firstName,	@lastName,	@payMode,	@email,	@mobileNo,	@spRespCode,	@cid,	@bid,	@clientCode,	@payeeProfile,	@transDate,	@spRespStatus,	@m3,	@challanNo,	@reMsg,	@orgTxnAmount,	@programId,	@midName,	@Add,	@param1,	@param2,	@param3,	@param4,	@udf5,	@udf6,	@udf7,	@udf8,	@udf9,	@udf10,	@udf11,	@udf12,	@udf13,	@udf14,	@udf15,	@udf16,	@udf17,	@udf18,	@udf19,	@udf20)", _Con);
                        insert_command.ExecuteNonQuery();
                        _Con.Close();
                    }
                }
                catch (Exception ex)
                {

                }
                HSC_Success_View succes_view = new HSC_Success_View();
                var model2 = (from A in db.Tbl_HSC_Payment
                              join B in db.Tbl_HSC_Form17_Final on A.udf18 equals B.Ref_ID.ToString()
                              where A.udf18 == Pre_Ref_ID.ToString()
                              select new
                              {
                                  A,
                                  B
                              }).FirstOrDefault();

                succes_view.sid = model2.B.S_ID;
                succes_view.SabPaisaTxId = model2.A.SabPaisaTxId;
                succes_view.issuerRefNo = model2.A.issuerRefNo;
                succes_view.transDate = model2.A.transDate;
                succes_view.amount = model2.A.amount;

                var model = db.Tbl_HSC_Payment.Where(x => x.udf18 == Pre_Ref_ID.ToString()).FirstOrDefault();
                pdfMaker.Create_HSC_PDF(Int32.Parse(Pre_Ref_ID.ToString()), model);
                return View(succes_view);
            }
            catch (Exception ex)
            {

            }
            return View();
        }
        public ActionResult HSC_Failure()
        {
            SabPaisaIntegration sabPaisaIntegration = new SabPaisaIntegration();
            Dictionary<string, string> sabPaisaRespdict = new Dictionary<string, string>();

            string query = Request.QueryString["query"].ToString();
            string authIV = Common_HSC.Payment_Authentication_IV();//"qz7zPW07upqREhuo";
            string authKey = Common_HSC.Payment_Authentication_KEY();//"vuQy2eFx4q095E03";
            sabPaisaRespdict = sabPaisaIntegration.subPaisaResponse(query, authIV, authKey);
            string Status = "", UniqueID = "", Seat_No = "", Transaction_ID = "", Div_Code = "", Pre_Ref_ID = "", Payment_Head = "", Payment_Head_Description = "";
            int result = 0;
            SqlConnection _Con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString1"].ConnectionString);
            SqlCommand insert_command = new SqlCommand(@"insert into Tbl_HSC_Payment_Error (pgRespCode,	PGTxnNo,	SabPaisaTxId,	issuerRefNo,	authIdCode,	amount,	clientTxnId,	firstName,	lastName,	payMode,	email,	mobileNo,	spRespCode,	cid,	bid,	clientCode,	payeeProfile,	transDate,	spRespStatus,	m3,	challanNo,	reMsg,	orgTxnAmount,	programId,	midName,	[Add],	param1,	param2,	param3,	param4,	udf5,	udf6,	udf7,	udf8,	udf9,	udf10,	udf11,	udf12,	udf13,	udf14,	udf15,	udf16,	udf17,	udf18,	udf19,	udf20) 
                                                                                   values(@pgRespCode,	@PGTxnNo,	@SabPaisaTxId,	@issuerRefNo,	@authIdCode,	@amount,	@clientTxnId,	@firstName,	@lastName,	@payMode,	@email,	@mobileNo,	@spRespCode,	@cid,	@bid,	@clientCode,	@payeeProfile,	@transDate,	@spRespStatus,	@m3,	@challanNo,	@reMsg,	@orgTxnAmount,	@programId,	@midName,	@Add,	@param1,	@param2,	@param3,	@param4,	@udf5,	@udf6,	@udf7,	@udf8,	@udf9,	@udf10,	@udf11,	@udf12,	@udf13,	@udf14,	@udf15,	@udf16,	@udf17,	@udf18,	@udf19,	@udf20)", _Con);
            foreach (KeyValuePair<string, string> pair in sabPaisaRespdict)
            {

                // Temp +=  " <br /> " + pair.Key.ToString() + "  -  " + pair.Value.ToString();
                insert_command.Parameters.AddWithValue("@" + pair.Key.ToString(), pair.Value.ToString());
                if (pair.Key.ToString() == "spRespStatus")
                {
                    Status = pair.Value.ToString();
                }
                if (pair.Key.ToString() == "clientTxnId")
                {
                    Transaction_ID = pair.Value.ToString();
                }
                if (pair.Key.ToString() == "udf17")
                {
                    Div_Code = pair.Value.ToString();
                }
                if (pair.Key.ToString() == "udf18")
                {
                    UniqueID = pair.Value.ToString();
                }
            }
            try
            {
                _Con.Open();
                if (insert_command.ExecuteNonQuery() > 0)
                { _Con.Close(); }
                else
                { _Con.Close(); }
            }
            catch (Exception exe)
            {

            }
            var model = db.Tbl_HSC_Payment_Error.Where(x => x.udf18 == UniqueID).FirstOrDefault();
            return View(model);
        }
    }
}