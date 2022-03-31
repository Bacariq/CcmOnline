using Bacariq.CcmDal.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Bacariq.CcmDal.Helper
{
    public static class CnxWebApi
    {
        public static List<Messages> GetMessage(string pUrl, string pOrigine, string pGuid)
        {
            List<Messages> TmpMessagesListe = new List<Messages>();
            try
            {
                string sText = "";
                WebClient wc = new WebClient();
                wc.QueryString.Add("ACTION", "GetMessage");
                wc.QueryString.Add("FKGUID", pGuid);
                wc.QueryString.Add("ORIGINE", pOrigine);
                var data = wc.UploadValues(pUrl, "POST", wc.QueryString);
                sText = Encoding.UTF8.GetString(data);

                MessagesApi TmpMessagesApi = JsonConvert.DeserializeObject<MessagesApi>(sText);
                TmpMessagesListe = JsonConvert.DeserializeObject<List<Messages>>(TmpMessagesApi.Data.Base64Decode());
                TmpMessagesApi = null;
            }
            catch (Exception ex)
            {
                ErrorLog.SaveLog("Bacariq.CcmDal.Helper.GetMessage", ex.ToString());
            }
            return TmpMessagesListe;
        }

        public static string SendMessage(string pAction, string pUrl, string pMessage, string pOrigine, string pGuid)
        {
            string sRet = "";
            try
            {
                WebClient wc = new WebClient();
                wc.QueryString.Add("ACTION", pAction); 
                wc.QueryString.Add("FKGUID", pGuid);
                wc.QueryString.Add("ORIGINE", pOrigine);
                wc.QueryString.Add("DATA", pMessage);
                var data = wc.UploadValues(pUrl, "POST", wc.QueryString);
                sRet = Encoding.UTF8.GetString(data);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return sRet;
        }


        public static string UpdateMessage(string pUrl, string pMessage)
        {
            string sRet = "";
            try
            {
                WebClient wc = new WebClient();
                wc.QueryString.Add("ACTION", "OkMessage");
                wc.QueryString.Add("DATA", pMessage);
                var data = wc.UploadValues(pUrl, "POST", wc.QueryString);
                sRet = Encoding.UTF8.GetString(data);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return sRet;
        }

    }
}
