using Bacariq.CcmDal.Helper;
using Bacariq.CcmDal.Singleton;
using System;
using System.IO;

namespace Bacariq.CcmDal.Models
{
    public static class ErrorLog
    {

        public static void LoadLog(ref JsonData sDate) {
            string sRet = "";
            string FileName = sDate.VALUE + ".log";
            string FileUrl = @Path.Combine(SingletonDataService.Instance.TempDirectory, FileName);

            if (File.Exists(FileUrl) == true)
            {
                using (var sr = new StreamReader(FileUrl))
                {
                    sRet = sr.ReadToEnd();
                }
            }

            sDate.FILENAME = FileName;
            sDate.VALUE = sRet.Base64Encode();
        }

        public static void SaveLog(String pTitre, string pMessage)
        {
            if (Directory.Exists(SingletonDataService.Instance.TempDirectory) == false)
            {
                Directory.CreateDirectory(SingletonDataService.Instance.TempDirectory);
            }

            string FileName = DateTime.Now.ToString("yyyyMMdd") + ".log";
            string FileUrl = @Path.Combine(SingletonDataService.Instance.TempDirectory, FileName);

            if (File.Exists(FileUrl) == false)
            {
                File.Create(FileUrl);
            }

            try
            {
                using (StreamWriter sw = File.AppendText(FileUrl))
                {
                    sw.WriteLine($"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}] : {pTitre} {pMessage}");
                }
            }
            catch { }

        }
    }
}

