using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;
using System.Collections;
using System.Net;
using System.IO;
using System.Text;


    public class SabPaisaRequest
    {
        private string m_prodCode;
        private string m_clientCode;
        private string m_userName;
        private string m_pass;
        private string m_clientTxnId;
        private string m_amt;
        private string m_authKey;
        private string m_authIV;
        private string m_successUrl;
        private string m_failureUrl;
        private string m_firstName;
        private string m_lastName;
        private string m_email;
        private string m_add;
        private string m_contactNo;
        private string m_programId;
        private string m_spUrl;
        private string m_udf1;
    private string m_udf15;
    private string m_udf16;
    private string m_udf17;
    private string m_udf18;
    private string m_udf19;
    private string m_udf20;


    public string udf1
    {
        set
        {
            m_udf1 = value;
        }
        get
        {
            return m_udf1;
        }
    }
    public string udf15
    {
        set
        {
            m_udf15 = value;
        }
        get
        {
            return m_udf15;
        }
    }
    public string udf16
    {
        set
        {
            m_udf16 = value;
        }
        get
        {
            return m_udf16;
        }
    }
    public string udf17
    {
        set
        {
            m_udf17 = value;
        }
        get
        {
            return m_udf17;
        }
    }
    public string udf18
    {
        set
        {
            m_udf18 = value;
        }
        get
        {
            return m_udf18;
        }
    }
    public string udf19
    {
        set
        {
            m_udf19 = value;
        }
        get
        {
            return m_udf19;
        }
    }
   
    public string udf20
    {
        set
        {
            m_udf20 = value;
        }
        get
        {
            return m_udf20;
        }
    }
    public string prodCode
        {
            set
            {
                m_prodCode=value ;
            }
            get
            {
                return m_prodCode;
            }
        }
        public string clientCode
        {
            set
            {
                m_clientCode = value;
            }
            get
            {
                return m_clientCode;
            }
        }
        public string userName
        {
            set
            {
                m_userName = value;
            }
            get
            {
                return m_userName;
            }
        }
        //
        public string pass
        {
            set
            {
                m_pass = value;
            }
            get
            {
                return m_pass;
            }
        }
        public string clientTxnId
        {
            set
            {
                m_clientTxnId = value;
            }
            get
            {
                return m_clientTxnId;
            }
        }
        public string amt
        {
            set
            {
                m_amt = value;
            }
            get
            {
                return m_amt;
            }
        }
        public string authKey
        {
            set
            {
                m_authKey = value;
            }
            get
            {
                return m_authKey;
            }
        }
        public string authIV
        {
            set
            {
                m_authIV = value;
            }
            get
            {
                return m_authIV;
            }
        }
        public string successUrl
        {
            set
            {
                m_successUrl = value;
            }
            get
            {
                return m_successUrl;
            }
        }
        public string failureUrl
        {
            set
            {
                m_failureUrl = value;
            }
            get
            {
                return m_failureUrl;
            }
        }
        public string firstName
        {
            set
            {
                m_firstName = value;
            }
            get
            {
                return m_firstName;
            }
        }
        public string lastName
        {
            set
            {
                m_lastName = value;
            }
            get
            {
                return m_lastName;
            }
        }
        public string email
        {
            set
            {
                m_email = value;
            }
            get
            {
                return m_email;
            }
        }
        public string add
        {
            set
            {
                m_add = value;
            }
            get
            {
                return m_add;
            }
        }
        public string contactNo
        {
            set
            {
                m_contactNo = value;
            }
            get
            {
                return m_contactNo;
            }
        }
        public string spUrl
        {
            set
            {
                m_spUrl = value;
            }
            get
            {
                return m_spUrl;
            }
        }
        public string programId
        {
            set
            {
                m_programId = value;
            }
            get
            {
                return m_programId;
            }
        }


        public Hashtable QueryValue()
        {
            Hashtable hst = new Hashtable();

            hst.Add(pass,"arvimd");
            return hst;
        }

    }

    public class SabPaisaIntegration
    {

        public string forwardToSabPaisa(SabPaisaRequest sabPaisaMember)
        {
            string query = "";
            Page page = new Page();
            string spurl = "https://securepay.sabpaisa.in/SabPaisa/sabPaisaInit";
            if (sabPaisaMember.spUrl.Trim()!= "")
            {
                spurl = sabPaisaMember.spUrl.Trim();
            }
            query = query + spurl + "?";
          
                query = query + "clientName=" + sabPaisaMember.clientCode.Trim() + "";
                query = query + "&usern=" + sabPaisaMember.userName.Trim() + "";
                query = query + "&pass=" + sabPaisaMember.pass.Trim() + "";
                query = query + "&txnId=" + sabPaisaMember.clientTxnId.Trim() + "";
                query = query + "&amt=" + sabPaisaMember.amt.Trim() + "";
                query = query + "&authKey=" + sabPaisaMember.authKey.Trim() + "";
                query = query + "&authIV=" + sabPaisaMember.authIV.Trim() + "";
                //query = query + "&grNumber=" + sabPaisaMember.grNumber.Trim() + "";
                query = query + "&ru=" + sabPaisaMember.successUrl.Trim() + "";
                query = query + "&failureURL=" + sabPaisaMember.failureUrl.Trim() + "";
                query = query + "&firstName=" + sabPaisaMember.firstName.Trim() + "";
                query = query + "&lstName=" + sabPaisaMember.lastName.Trim() + "";
                query = query + "&Email=" + sabPaisaMember.email.Trim() + "";
                query = query + "&Add=" + sabPaisaMember.add.Trim() + "";
                query = query + "&contactNo=" + sabPaisaMember.contactNo.Trim() + "";
                query = query + "&programId=" + sabPaisaMember.programId.Trim() + "";
                query = query + "&udf1=" + sabPaisaMember.udf1.Trim() + "";
                query = query + "&udf15=" + sabPaisaMember.udf15.Trim() + "";
                query = query + "&udf16=" + sabPaisaMember.udf16.Trim() + "";
                query = query + "&udf17=" + sabPaisaMember.udf17.Trim() + "";
                query = query + "&udf18=" + sabPaisaMember.udf18.Trim() + "";
                query = query + "&udf19=" + sabPaisaMember.udf19.Trim() + "";
                query = query + "&udf20=" + sabPaisaMember.udf20.Trim() + "";
                query = EncryptString(query, sabPaisaMember.authIV, sabPaisaMember.authKey);



                query = query.Replace("+", "%2B");
                //query = query.Replace(" ", "%20");
               
                query = spurl + "?query=" + query + "&clientName=" + sabPaisaMember.clientCode.Trim() + "";
                //string SMSScript = "<script>window.open('" + query + "'</script>)";
                //ScriptManager.RegisterStartupScript(page, this.GetType(), "MyScript", SMSScript, true);

                //http://192.168.43.115:8081/SabPaisaOld/sabPaisaInit?query=LUtoyFGxO3SsXx3dyqyaxB9CRvsWlWKh8FuCV6Q9zYGsf0hQJYqX1EuPMOA9n1YNQs08mmyQvOS9F15jOtMCysxR69cjQHUS6smf9aFQuUug9k+X+TfZQsLNEeXwRmApCxrAz2vmZLmSCBVY+dAHlTX6nNgVx7yXbCOQuWu518VLJNPCRZK4Hm+B7bOJLDACXt0xS0ZrPxKTAm25SUPwHyECpNdhPzzX4y77datWLwpDI/uq5LZWgvt4l/9z74G+pAFge94sJWYsCCxSIaDd/JcnEHoRomKVAJhfB1Tt7uU3D37Y6ZtzgCYb5jKmeV6/E7AuatqN7l8zK8h7j32iNMTulCS8fw1kaR7dFrm2OkTYH5oQ/hOeBu+z3LI0z5tXLiBBD9m96HK6UBLNoVVgHhcwuNIhBFQPURzvy+yBhI2IBd+6VQKyDN4AuDxx1hhp8ous7SpBjOndHY2wUaZC85Sts5Xd4JtQd+JFkstQ8xyHhymmM8YtproFx6iIfHA6InAm549jPDKAKMtVyTs16W6bZKjtPPwOnLJV7dYXwjqCyW19+TXYh+YbwLI9t71X&clientName=SSNC2&prodCode=LINKP

                return query;
        }// sabPaisaIntregration end

        public Dictionary<string, string> subPaisaResponse(string query, string authIV, string authKey)
        {
            string decQuery = "";
            query.Replace("%2B", "+");
            //query.Replace(" ", "+");

            decQuery = DecryptString(query, authIV, authKey);
            Dictionary<string, string> dictParams = new Dictionary<string, string>();

            dictParams = quearyParser(decQuery);

            /*foreach (KeyValuePair<string, string> pair in dictParams)
		    {
			    Console.WriteLine(pair.Key.ToString ()+ "  -  "  + pair.Value.ToString () );
		    }*/
            return dictParams;
        }
      
        public static string EncryptString(string spURL, string AuthIV, string AuthKey)
        {
            using (var csp = new AesCryptoServiceProvider())
            {
                ICryptoTransform e = GetCryptoTransform(csp, true, AuthIV, AuthKey);
                byte[] inputBuffer = Encoding.UTF8.GetBytes(spURL);
                byte[] output = e.TransformFinalBlock(inputBuffer, 0, inputBuffer.Length);

                string encrypted = Convert.ToBase64String(output);

                return encrypted;
            }
        }

        private static ICryptoTransform GetCryptoTransform(AesCryptoServiceProvider csp, bool encrypting, String AuthIV, String AuthKey)
        {
            csp.Mode = CipherMode.CBC;
            csp.Padding = PaddingMode.PKCS7;
            String iv = AuthIV;
            String AESKey1 = AuthKey;
            csp.IV = Encoding.UTF8.GetBytes(iv);
            byte[] inputBuffer = Encoding.UTF8.GetBytes(AESKey1);
            csp.Key = Encoding.UTF8.GetBytes(AESKey1);
            if (encrypting)
            {
                return csp.CreateEncryptor();
            }
            return csp.CreateDecryptor();
        }

        public static string DecryptString(string encrypted, string AuthIV, string AuthKey)
        {
            using (var csp = new AesCryptoServiceProvider())
            {
                // encrypted = encrypted.Substring(0, encrypted.IndexOf("&"));
                var d = GetCryptoTransform(csp, false, AuthIV, AuthKey);
                byte[] output = Convert.FromBase64String(encrypted);

                byte[] decryptedOutput = d.TransformFinalBlock(output, 0, output.Length);
                string decypted = Encoding.UTF8.GetString(decryptedOutput);
                return decypted;
            }
        }        

	private static Dictionary<string, string> quearyParser(String values){	
		Dictionary<string, string> dict = new Dictionary<string, string>();
		//String values = "pgRespCode=F&PGTxnNo=37811436&SabPaisaTxId=7399702091711511122664015&issuerRefNo=NA&authIdCode=0&amount=57.0&clientTxnId=TESTING020917115040588&firstName=TPK&lastName=Test&payMode=CreditCards&email=test@gmail.com&mobileNo=9908944111&spRespCode=0000&cid=null&bid=null&clientCode=CXY10&payeeProfile=Student&transDate=Sat Sep 02 11:55:00 IST 2017&spRespStatus=success¶m3=BE&challanNo=&reMsg=null&orgTxnAmount=55.0&programId=mtech";

        string[] sites = values.Split('&');
		String[] token;
		
		foreach (string s in sites) {
			token = s.Split('=');
			dict.Add(token.GetValue(0).ToString(),token.GetValue(1).ToString());
		}		
        return dict;
	}

	public int randomTxnId()
{
Random random = new Random();
int num = random.Next(100000000, 999999999); 
Console.WriteLine("No >> "+num);	
return num;
}
  }
//namesapaceEnd
