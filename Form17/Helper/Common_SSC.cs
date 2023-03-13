using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.IO;
using System.Net;
using System.Data.OleDb;
using System.Configuration;
using System.Web.Script.Serialization;
using System.Net.Mail;

using System.Security.Cryptography;
using System.Text;

/// <summary>
/// Summary description for Common
/// </summary>
public class Common_SSC
{
    public Common_SSC()
    {
        //
        // TODO: Add constructor logic here
        //

    }
    public static string GetSession()
    {
        return "";
    }
    public static string Payment_Getway_URL()
    {
        //return "https://securepay.sabpaisa.in/SabPaisa/sabPaisaInit";
        return "https://uatsp.sabpaisa.in/SabPaisa/sabPaisaInit";
    }
    public static string Payment_Client_Code()
    {
        //return "MSB10";
         return "SIPL1";
    }
    public static string Payment_Username()
    {
        //return "nishant.jha_3168";
        return "nishant.jha_2885";
    }
    public static string Payment_Password()
    {
        // return  "MSB10_SP3168";
        return "SIPL1_SP2885";
    }
    public static string Payment_Authentication_KEY()
    {
        //return "P3gcnDR3XNapNmFC";
        return "rMnggTKFvmGx8y1z";
    }
    public static string Payment_Authentication_IV()
    {
         //return "0g0cAheasfsaFSz2";
        return "0QvWIQBSz4AX0VoH";

    }
    public static string GetExamYear()
    {
        return "MAR 2023";
    }
    public static string Server_Url()
    {
        return "http://streetlight.pixeltechservices.in/";
    }
    public static string READRESPONCE(String URL)
    {
        WebRequest wrGETURL;
        StreamReader objReader, sURL;
        Stream objStream;
        wrGETURL = WebRequest.Create(URL);
        objStream = wrGETURL.GetResponse().GetResponseStream();
        objReader = new StreamReader(objStream);
        objReader.Read();
        string dataString = objReader.ReadToEnd();
        objReader.Close();
        objStream.Close();
        return dataString;

    }



    public DataTable Import_To_Grid(string FilePath, string Extension, string isHDR)
    {
        string conStr = "";
        switch (Extension)
        {
            case ".xls": //Excel 97-03
                conStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties = 'Excel 8.0;HDR={1}'";

                break;
            case ".xlsx": //Excel 07
                conStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties = 'Excel 8.0;HDR={1}'";
                break;
        }
        conStr = String.Format(conStr, FilePath, isHDR);
        OleDbConnection connExcel = new OleDbConnection(conStr);
        OleDbCommand cmdExcel = new OleDbCommand();
        OleDbDataAdapter oda = new OleDbDataAdapter();
        DataTable dt = new DataTable();
        cmdExcel.Connection = connExcel;

        //Get the name of First Sheet
        connExcel.Open();
        DataTable dtExcelSchema;
        dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
        string SheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
        connExcel.Close();

        // Read Data from First Sheet
        connExcel.Open();
        cmdExcel.CommandText = "SELECT * From [" + SheetName + "]";
        oda.SelectCommand = cmdExcel;
        oda.Fill(dt);
        connExcel.Close();

        return dt;


        ////Bind Data to GridView
        //GridView1.Caption = Path.GetFileName(FilePath);
        //GridView1.DataSource = dt;
        //GridView1.DataBind();
    }


    public void Custom_DataTableToJSONWithJavaScriptSerializer(DataSet dataset)
    {
        JavaScriptSerializer jsSerializer = new JavaScriptSerializer();

        Dictionary<string, object> ssvalue = new Dictionary<string, object>();

        foreach (DataTable table in dataset.Tables)
        {
            List<Dictionary<string, object>> parentRow = new List<Dictionary<string, object>>();
            Dictionary<string, object> childRow;

            string tablename = table.TableName;
            foreach (DataRow row in table.Rows)
            {
                childRow = new Dictionary<string, object>();
                foreach (DataColumn col in table.Columns)
                {
                    childRow.Add(col.ColumnName, row[col]);
                }
                parentRow.Add(childRow);
            }

            ssvalue.Add(tablename, parentRow);
        }

        // return jsSerializer.Serialize(ssvalue);

        string jsonstr = string.Empty;
        jsonstr = "," + jsSerializer.Serialize(ssvalue).Remove(0, 1);
        JavaScriptSerializer js = new JavaScriptSerializer();
        js.MaxJsonLength = Int32.MaxValue;
        HttpContext.Current.Response.BufferOutput = true;
        HttpContext.Current.Response.ContentType = "text/plain";
        HttpContext.Current.Response.Write(jsonstr);
        HttpContext.Current.Response.Flush();
        HttpContext.Current.Response.End();
    }
    public void DataTableToJSONWithJavaScriptSerializer(DataTable table)
    {
        JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
        List<Dictionary<string, object>> parentRow = new List<Dictionary<string, object>>();
        Dictionary<string, object> childRow;
        foreach (DataRow row in table.Rows)
        {
            childRow = new Dictionary<string, object>();
            foreach (DataColumn col in table.Columns)
            {
                childRow.Add(col.ColumnName, row[col]);
            }
            parentRow.Add(childRow);
        }
        HttpContext.Current.Response.BufferOutput = true;
        string jsonstr = string.Empty;
        jsonstr = jsSerializer.Serialize(parentRow);
        JavaScriptSerializer js = new JavaScriptSerializer();
        js.MaxJsonLength = Int32.MaxValue;
        HttpContext.Current.Response.BufferOutput = true;
        HttpContext.Current.Response.ContentType = "text/plain";
        HttpContext.Current.Response.Write(jsonstr);
        HttpContext.Current.Response.Flush();
        HttpContext.Current.Response.End();


    }

    public void DataTableToJSONWithJavaScriptSerializer(DataSet dataset)
    {

        JavaScriptSerializer jsSerializer = new JavaScriptSerializer();

        Dictionary<string, object> ssvalue = new Dictionary<string, object>();

        foreach (DataTable table in dataset.Tables)
        {
            List<Dictionary<string, object>> parentRow = new List<Dictionary<string, object>>();
            Dictionary<string, object> childRow;

            string tablename = table.TableName;
            foreach (DataRow row in table.Rows)
            {
                childRow = new Dictionary<string, object>();
                foreach (DataColumn col in table.Columns)
                {
                    childRow.Add(col.ColumnName, row[col]);
                }
                parentRow.Add(childRow);
            }

            ssvalue.Add(tablename, parentRow);
        }

        // return jsSerializer.Serialize(ssvalue);

        string jsonstr = string.Empty;
        jsonstr = jsSerializer.Serialize(ssvalue);
        JavaScriptSerializer js = new JavaScriptSerializer();
        js.MaxJsonLength = Int32.MaxValue;
        HttpContext.Current.Response.BufferOutput = true;
        HttpContext.Current.Response.ContentType = "text/plain";
        HttpContext.Current.Response.Write(jsonstr);
        HttpContext.Current.Response.Flush();
        HttpContext.Current.Response.End();
    }

    public void DataTableToJSONWithJavaScriptSerializer(DataSet dataset, int Total)
    {

        JavaScriptSerializer jsSerializer = new JavaScriptSerializer();

        Dictionary<string, object> ssvalue = new Dictionary<string, object>();

        foreach (DataTable table in dataset.Tables)
        {
            List<Dictionary<string, object>> parentRow = new List<Dictionary<string, object>>();
            Dictionary<string, object> childRow;

            string tablename = table.TableName;
            foreach (DataRow row in table.Rows)
            {
                childRow = new Dictionary<string, object>();
                foreach (DataColumn col in table.Columns)
                {
                    childRow.Add(col.ColumnName, row[col]);
                }
                parentRow.Add(childRow);
            }

            ssvalue.Add(tablename, parentRow);
        }
        ssvalue.Add("total", Total);
        // return jsSerializer.Serialize(ssvalue);

        string jsonstr = string.Empty;
        jsonstr = jsSerializer.Serialize(ssvalue);
        JavaScriptSerializer js = new JavaScriptSerializer();
        js.MaxJsonLength = Int32.MaxValue;
        HttpContext.Current.Response.BufferOutput = true;
        HttpContext.Current.Response.ContentType = "text/plain";
        HttpContext.Current.Response.Write(jsonstr);
        HttpContext.Current.Response.Flush();
        HttpContext.Current.Response.End();
    }

    public DataTable Get_MSG(string MSG, int MSG_ID)
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Message_Type", typeof(string));
        dt.Rows.Add(0);
        dt.Columns.Add("Message", typeof(string));

        if (MSG_ID == 1)
        {
            dt.Rows[0]["Message_Type"] = "Success";
            dt.Rows[0]["Message"] = MSG;

        }
        else
        {
            dt.Rows[0]["Message_Type"] = "Failure";
            dt.Rows[0]["Message"] = MSG;

        }
        dt.TableName = "Message";
        return dt;
    }
    public DataTable Set_Excel_File(string File_Name)
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Message_Type", typeof(string));
        dt.Rows.Add(0);
        dt.Columns.Add("File_Name", typeof(string));

       
            dt.Rows[0]["Message_Type"] = "Success";
            dt.Rows[0]["File_Name"] = File_Name;

       
        
        dt.TableName = "File_Name_Table";
        return dt;
    }

    public string Get_Division_Address(int Division_Code)
    {
        string Address = "";
        switch (Division_Code)
        {
            case 1:
                Address = "FROM:\n THE DIVISIONAL SECRETARY,\n MAHARASHTRA STATE BOARD OF\n SECONDARY AND HIGHER\n SECONDARY EDUCATION,\n PUNE DIVISIONAL BOARD.\n PUNE - 411005";
                break;
            case 2:
                Address = "FROM:\n THE DIVISIONAL SECRETARY,\n MAHARASHTRA STATE BOARD OF\n SECONDARY AND HIGHER\n SECONDARY EDUCATION,\n NAGPUR DIVISIONAL BOARD.\n NAGPUR ";
                break;
            case 3:
                Address = "FROM:\n THE DIVISIONAL SECRETARY,\n MAHARASHTRA STATE BOARD OF\n SECONDARY AND HIGHER\n SECONDARY EDUCATION,\n AURANGABAD DIVISIONAL BOARD.\n AURANGABAD ";
                break;
            case 4:
                Address = "FROM:\n THE DIVISIONAL SECRETARY,\n MAHARASHTRA STATE BOARD OF\n SECONDARY AND HIGHER\n SECONDARY EDUCATION,\n MUMBAI DIVISIONAL BOARD.\n MUMBAI ";
                break;
            case 5:
                Address = "FROM:\n THE DIVISIONAL SECRETARY,\n MAHARASHTRA STATE BOARD OF\n SECONDARY AND HIGHER\n SECONDARY EDUCATION,\n KOLHAPUR DIVISIONAL BOARD.\n KOLHAPUR - 416004";
                break;
            case 6:
                Address = "FROM:\n THE DIVISIONAL SECRETARY,\n MAHARASHTRA STATE BOARD OF\n SECONDARY AND HIGHER\n SECONDARY EDUCATION,\n AMRAVATI DIVISIONAL BOARD.\n AMRAVATI";
                break;
            case 7:
                Address = "FROM:\n THE DIVISIONAL SECRETARY,\n MAHARASHTRA STATE BOARD OF\n SECONDARY AND HIGHER\n SECONDARY EDUCATION,\n NASHIK DIVISIONAL BOARD.\n NASHIK";
                break;
            case 8:
                Address = "FROM:\n THE DIVISIONAL SECRETARY,\n MAHARASHTRA STATE BOARD OF\n SECONDARY AND HIGHER\n SECONDARY EDUCATION,\n LATUR DIVISIONAL BOARD.\n LATUR";
                break;
            case 9:
                Address = "FROM:\n THE DIVISIONAL SECRETARY,\n MAHARASHTRA STATE BOARD OF\n SECONDARY AND HIGHER\n SECONDARY EDUCATION,\n KONKAN DIVISIONAL BOARD.\n KONKAN";
                break;

        }

        return Address;
    }
    public string Get_Division_Name(int Division_Code)
    {
        string Address = "";
        switch (Division_Code)
        {
            case 1:
                Address = " PUNE";
                break;
            case 2:
                Address = " NAGPUR";
                break;
            case 3:
                Address = " AURANGABAD";
                break;
            case 4:
                Address = " MUMBAI";
                break;
            case 5:
                Address = " KOLHAPUR";
                break;
            case 6:
                Address = " AMRAVATI";
                break;
            case 7:
                Address = " NASHIK";
                break;
            case 8:
                Address = " LATUR";
                break;
            case 9:
                Address = " KONKAN";
                break;

        }

        return Address;
    }


    public static string Get_Head_Name(int Head_ID)
    {
        string Payment_Head = "";
        switch (Head_ID)
        {
            case 301:
                Payment_Head = "उत्तरपत्रिका गुणपडताळणी";
                break;
            case 302:
                Payment_Head = "उत्तरपत्रिका छायाप्रत ";
                break;
            case 303:
                Payment_Head = "उत्तरपत्रिका पुनर्मूल्यांकन";
                break;
            case 304:
                Payment_Head = "स्थलांतर प्रमाणपत्र";
                break;
        }
        return Payment_Head;
    }

  
    public void MailSend(String Email, String Title,String Message)
    {
        try
        {
            MailMessage msg = new MailMessage();
            msg.From = new MailAddress("support@msbshse.ac.in");
            msg.To.Add(Email);

            msg.Subject = Title;
            msg.Body = Message;
            msg.IsBodyHtml = true;
            MailAddress copy = new MailAddress("receipt@msbshse.ac.in");
            msg.CC.Add(copy);

            msg.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "mail.msbshse.ac.in";
            smtp.Port = 25;
            smtp.Credentials = new System.Net.NetworkCredential("support", "Pixel@123");
            smtp.EnableSsl = false;
            smtp.Send(msg);

        }
        catch (Exception ex)
        {
            //throw ex;
        }
    }

    public string Get_Payment_Slip_Body(DataTable dt)
    {

        return @"<html>
   <body>
      <table style='max-width:600px;margin:auto;border-spacing:0;background:#249a96b7;padding:4px;border-radius:16px;overflow:hidden' align='center' border='0' cellpadding='0' cellspacing='0' width='100%'>
         <tbody>
            <tr>
               <td style='border-collapse:collapse'>
                  <table style='margin:auto;border-spacing:0;background:white;border-radius:12px;overflow:hidden' align='center' border='0' cellpadding='0' cellspacing='0' width='100%'>
                     <tbody>
                        <tr>
                           <td style='border-collapse:collapse'>
                              <table style='border-spacing:0;border-collapse:collapse' bgcolor='#ffffff' border='0' cellpadding='0' cellspacing='0' width='100%'>
                                 <tbody>
                                    <tr>
                                       <td style='border-collapse:collapse;padding:16px 32px' align='left' valign='middle'>
                                          <table style='border-spacing:0;border-collapse:collapse' bgcolor='#ffffff' border='0' cellpadding='0' cellspacing='0' width='100%'>
                                             <tbody>
                                                <tr>
                                                   <td style='padding:0;text-align:left;border-collapse:collapse' width='40' align='left' valign='middle'>                     <a href='http://verification.mh-hsc.ac.in/'>                     <img src='http://verification.mh-hsc.ac.in/design/images/MSBSHSE-logo.png' title='MSBSHSE'  style='margin:auto;text-align:center;border:0;outline:none;text-decoration:none;min-height:40px' align='middle' border='0' width='40' class='CToWUd'>                     </a>                     </td>
                                                   <td  align='left' valign='middle' style='border-collapse:collapse'>MAHARASHTRA STATE BOARD OF SECONDARY & HIGHER SECONDARY EDUCATION,PUNE</td>
                                                   <td align='right' valign='middle'>                           </td>
                                                </tr>
                                             </tbody>
                                          </table>
                                       </td>
                                    </tr>
                                 </tbody>
                              </table>
                           </td>
                        </tr>
                        <tr>
                           <td style='border-collapse:collapse;padding:0 16px'>
                              <table align='center' border='0' cellpadding='0' cellspacing='0' width='100%' style='background:#f7f9fa;padding:16px;border-radius:8px;overflow:hidden'>
                                 <tbody>
                                    <tr>
                                       <td align='left' valign='middle' style='border-collapse:collapse;padding-bottom:16px;border-bottom:1px solid #eaeaed'>
                                          <table align='center' border='0' cellpadding='0' cellspacing='0' width='100%'>
                                             <tbody>
                                                <tr>
                                                   <td align='left' valign='top' style='border-collapse:collapse;text-transform:capitalize'>                                         <span style='border-collapse:collapse;width:100%;display:block'>Name</span>                                         <span style='border-collapse:collapse;font-size:16px;font-weight:500;width:100%;display:block'>"+dt.Rows[0]["Name"].ToString()+@" </span>                                     </td>
                                                   <td width='32' align='left' valign='middle' style='border-collapse:collapse'></td>
                                                   <td align='right' valign='middle' style='border-collapse:collapse;font-size:20px;font-weight:500'>                                         ₹ "+ dt.Rows[0]["orgTxnAmount"].ToString() + @"                                     </td>
                                                </tr>
                                             </tbody>
                                          </table>
                                       </td>
                                    </tr>
                                    <tr>
                                       <td align='left' valign='middle' style='border-collapse:collapse;padding:8px 0;border-bottom:1px solid #eaeaed;font-size:12px'>
                                          <table align='center' border='0' cellpadding='0' cellspacing='0' width='100%'>
                                             <tbody>
                                                <tr>
                                                   <td width='28%' align='left' valign='top' style='border-collapse:collapse;text-transform:capitalize'>                                     Txn. ID                                     </td>
                                                   <td width='16' align='left' valign='top' style='border-collapse:collapse;font-weight:normal'>:</td>
                                                   <td align='left' valign='top' style='border-collapse:collapse;font-weight:normal'>                                     " + dt.Rows[0]["SabPaisaTxId"].ToString() + @"                                      </td>
                                                </tr>
                                             </tbody>
                                          </table>
                                       </td>
                                    </tr>
                                    <tr>
                                       <td align='left' valign='middle' style='border-collapse:collapse;padding:8px 0;border-bottom:1px solid #eaeaed;font-size:12px'>
                                          <table align='center' border='0' cellpadding='0' cellspacing='0' width='100%'>
                                             <tbody>
                                                <tr>
                                                   <td width='28%' align='left' valign='top' style='border-collapse:collapse;text-transform:capitalize'>                                     Txn. status                                     </td>
                                                   <td width='16' align='left' valign='top' style='border-collapse:collapse;font-weight:normal'>:</td>
                                                   <td align='left' valign='top' style='border-collapse:collapse;font-weight:normal;font-size:16px;color:#5eaa46'>                                     " + dt.Rows[0]["spRespStatus"].ToString() + @"                                      </td>
                                                </tr>
                                             </tbody>
                                          </table>
                                       </td>
                                    </tr>
                                    <tr>
                                       <td align='left' valign='middle' style='border-collapse:collapse;padding:8px 0;border-bottom:1px solid #eaeaed;font-size:12px'>
                                          <table align='center' border='0' cellpadding='0' cellspacing='0' width='100%'>
                                             <tbody>
                                                <tr>
                                                   <td width='28%' align='left' valign='top' style='border-collapse:collapse;text-transform:capitalize'>                                     Debited from                                     </td>
                                                   <td width='16' align='left' valign='top' style='border-collapse:collapse;font-weight:normal'>:</td>
                                                   <td align='left' valign='top' style='border-collapse:collapse;font-weight:normal;font-size:12px'>                                     <span style='border-collapse:collapse;font-size:16px;width:100%;display:block'>                                 " + dt.Rows[0]["payMode"].ToString() + @"                                         </span>                                                                         </td>
                                                </tr>
                                             </tbody>
                                          </table>
                                       </td>
                                    </tr>
                                    <tr>
                                       <td align='left' valign='middle' style='border-collapse:collapse;padding:8px 0;border-bottom:1px solid #eaeaed;font-size:12px'>
                                          <table align='center' border='0' cellpadding='0' cellspacing='0' width='100%'>
                                             <tbody>
                                                <tr>
                                                   <td width='28%' align='left' valign='top' style='border-collapse:collapse;text-transform:capitalize'>                                     Appication Type                                     </td>
                                                   <td width='16' align='left' valign='top' style='border-collapse:collapse;font-weight:normal'>:</td>
                                                   <td align='left' valign='top' style='border-collapse:collapse;font-weight:normal'>                                     "+ Common_SSC.Get_Head_Name(Convert.ToInt32(dt.Rows[0]["Payment_Head"].ToString())) +@"                                     </td>
                                                </tr>
                                             </tbody>
                                          </table>
                                       </td>
                                    </tr>
                                    <tr>
                                       <td align='left' valign='middle' style='border-collapse:collapse;padding:8px 0 0;font-size:12px'>
                                          <table align='center' border='0' cellpadding='0' cellspacing='0' width='100%'>
                                             <tbody>
                                                <tr>
                                                   <td width='28%' align='left' valign='top' style='border-collapse:collapse'>                                     Case No                                     </td>
                                                   <td width='16' align='left' valign='top' style='border-collapse:collapse;font-weight:normal'>:</td>
                                                   <td align='left' valign='top' style='border-collapse:collapse;font-weight:normal'>                       " + dt.Rows[0]["Case_No"].ToString() + @"                                                     </td>
                                                </tr>
                                             </tbody>
                                          </table>
                                       </td>
                                    </tr>
                                    <tr>
                                       <td align='left' valign='middle' style='border-collapse:collapse;padding:8px 0 0;font-size:12px'>
                                          <table align='center' border='0' cellpadding='0' cellspacing='0' width='100%'>
                                             <tbody>
                                                <tr>
                                                   <td width='28%' align='left' valign='top' style='border-collapse:collapse'>                                     Seat No                                     </td>
                                                   <td width='16' align='left' valign='top' style='border-collapse:collapse;font-weight:normal'>:</td>
                                                   <td align='left' valign='top' style='border-collapse:collapse;font-weight:normal'>                       " + dt.Rows[0]["Seat_No"].ToString() + @"                                                     </td>
                                                </tr>
                                             </tbody>
                                          </table>
                                       </td>
                                    </tr>
                                    <tr>
                                       <td align='left' valign='middle' style='border-collapse:collapse;padding:8px 0 0;font-size:12px'>
                                         आपल्या अर्जावर वेळोवेळी होणाऱ्या कार्यवाहीचा तपशील पाहण्यासाठी तपशील पाहण्यासाठी http://verification.mh-hsc.ac.in या संकेतस्थळाला वेळोवेळी भेट द्यावी 
                                       </td>
                                    </tr>
                                 </tbody>
                              </table>
                           </td>
                        </tr>
                       
                                  
                                 </tbody>
                              </table>
                           </td>
                        </tr>
                     </tbody>
                  </table>
               </td>
            </tr>
         </tbody>
      </table>
   </body>
</html>";
    }

   
   
    public string Encrypt(string clearText)
    {
        string EncryptionKey = "SAMA2SPBNI99212";
        byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
        using (Aes encryptor = Aes.Create())
        {
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
            encryptor.Key = pdb.GetBytes(32);
            encryptor.IV = pdb.GetBytes(16);
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(clearBytes, 0, clearBytes.Length);
                    cs.Close();
                }
                clearText = Convert.ToBase64String(ms.ToArray());
            }
        }
        return clearText;
    }
    public string Decrypt(string cipherText)
    {
        string EncryptionKey = "SAMA2SPBNI99212";
        byte[] cipherBytes = Convert.FromBase64String(cipherText);
        using (Aes encryptor = Aes.Create())
        {
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
            encryptor.Key = pdb.GetBytes(32);
            encryptor.IV = pdb.GetBytes(16);
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(cipherBytes, 0, cipherBytes.Length);
                    cs.Close();
                }
                cipherText = Encoding.Unicode.GetString(ms.ToArray());
            }
        }
        return cipherText;
    }

}