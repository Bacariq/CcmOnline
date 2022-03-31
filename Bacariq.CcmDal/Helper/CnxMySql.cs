using Bacariq.CcmDal.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Text;

namespace Bacariq.CcmDal
{
    public class CnxMysSql
    {
        public String mConnString;
        public String connString
        {
            get
            {
                return mConnString;
            }
            set
            {
                mConnString = value;
            }
        }
        private MySqlConnection m_Conn;

        private readonly string m_Host = "127.0.0.1";
        private readonly int m_Port = 3306;
        private readonly string m_Database = "bacariqccmomnipro";
        private readonly string m_Username = "root";
        private readonly string m_Password = "";
        public CnxMysSql()
        {
            connString = "Server=" + m_Host +
                         ";Database=" + m_Database +
                         ";Port=" + m_Port +
                         ";Uid=" + m_Username +
                         ";Pwd=" + m_Password;
        }

        public string Requete { get; set; }
        public string Table { get; set; }

        private DbDataReader m_Reader = null;

        public void OpenDBConnection()
        {
            try
            {
                m_Conn = new MySqlConnection(connString);
                m_Conn.Open();
            }
            catch (Exception ex)
            {
                ErrorLog.SaveLog("CnxMysSql.OpenDBConnection", ex.ToString());
            }

        }

        public void CloseDBConnection()
        {
            m_Conn.Close();
            m_Conn.Dispose();
        }

        private string GetGuid(string pId)
        {
            string sRet = "";
            OpenDBConnection();
            try
            {
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = m_Conn,
                    CommandText = "select PkGuid from " + Table.ToLower() + " where Id = " + pId
                };

                m_Reader = cmd.ExecuteReader();
                if (m_Reader.HasRows)
                {
                    while (m_Reader.Read())
                    {
                        sRet = m_Reader.GetString(m_Reader.GetOrdinal("PkGuid"));
                    }
                    m_Reader.Close();
                }
            }
            catch (MySqlException exSql)
            {
                ErrorLog.SaveLog("CnxMysSql.GetGuid", exSql.Message);
            }
            catch (Exception ex)
            {
                ErrorLog.SaveLog("CnxMysSql.GetGuid", ex.Message);
            }
            finally
            {
                CloseDBConnection();
            }
            return sRet;
        }


        public string Execute()
        {
            string sRet = "";
            string sIdReturned = "";
            OpenDBConnection();


            try
            {
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = m_Conn,
                    CommandText = Requete
                };
                int numRowsUpdated = cmd.ExecuteNonQuery();

                sIdReturned = cmd.LastInsertedId.ToString();
                if (sIdReturned != "0")
                {
                    sRet = GetGuid(sIdReturned);

                    if (string.IsNullOrEmpty(sRet) == true)
                    {
                        sRet = sIdReturned;
                    }
                }
            }
            catch (MySqlException exSql)
            {
                ErrorLog.SaveLog("CnxMysSql.Execute", exSql.Message);
                sRet = "Error";
            }
            catch (Exception ex)
            {
                ErrorLog.SaveLog("CnxMysSql.Execute", ex.Message);
                sRet = "Error";
            }
            finally
            {
                CloseDBConnection();
            }
            return sRet;
        }

        public DbDataReader Query()
        {
            DbDataReader pDbDataReader = null;
            try
            {
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = m_Conn,
                    CommandText = Requete
                };
                pDbDataReader = cmd.ExecuteReader();

            }
            catch (Exception e)
            {
                ErrorLog.SaveLog("CnxMysSql.Query", e.Message);
            }

            return pDbDataReader;
        }

        public int isRecordExist(string pRequete)
        {
            int nbRecords = 0;
            string OldQuery = Requete;
            OpenDBConnection();
            Requete = pRequete;
            DbDataReader dr_NBRecords = Query();

            if (dr_NBRecords.HasRows)
            {
                while (dr_NBRecords.Read())
                {
                    nbRecords = dr_NBRecords.GetInt16(dr_NBRecords.GetOrdinal("nb"));
                }
                dr_NBRecords.Close();
            }
            CloseDBConnection();

            Requete = OldQuery;
            return nbRecords;
        }


    }
}
