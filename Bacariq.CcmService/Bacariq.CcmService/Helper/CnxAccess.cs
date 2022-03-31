using Bacariq.CcmDal.Models;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bacariq.CcmService.Helper
{
    public interface ICnxDataBase
    {
        string Requete { get; set; }
        string Table { get; set; }
        String connString { get; set; }

        void OpenDBConnection();
        void CloseDBConnection();
        T Query<T>();
        int isRecordExist();
        string Execute();

    }

    public class CnxAccess : ICnxDataBase
    {
        protected OleDbConnection m_Conn;
        public string Requete { get; set; }
        public string Table { get; set; }
        public String connString { get; set; }

        public CnxAccess()
        {
            connString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=«BDAccessFile»;User Id=admin;Password=;";
            //connString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=«BDAccessFile»;User Id=admin;Password=;";
            connString = connString.Replace("«BDAccessFile»", @"C:\Temp\patients.mdb");
        }

        public void OpenDBConnection()
        {
            try
            {
                m_Conn = new OleDbConnection(connString);
                m_Conn.Open();
            }
            catch (Exception ex)
            {
                ErrorLog.SaveLog("OpenDBConnection => ", ex.Message);
            }

        }

        public void CloseDBConnection()
        {
            if (m_Conn.State == System.Data.ConnectionState.Open)
            {
                m_Conn.Close();
                m_Conn.Dispose();
            }
        }

        public int isRecordExist()
        {
            throw new NotImplementedException();
        }

        public string Execute()
        {
            throw new NotImplementedException();
        }

        public T Query<T>()
        {
            OleDbDataReader pDbDataReader = null;
            try
            {
                OleDbCommand cmd = new OleDbCommand
                {
                    Connection = m_Conn,
                    CommandText = Requete
                };
                pDbDataReader = cmd.ExecuteReader();

            }
            catch (Exception ex)
            {
                ErrorLog.SaveLog("Query<T> => ", ex.Message);
            }

            return (T)Convert.ChangeType(pDbDataReader, typeof(T));
        }
    }
}
