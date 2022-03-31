using Bacariq.CcmDal.Models;
using Bacariq.CcmService.Helper;
using Bacariq.CcmService.Models;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bacariq.CcmService.Repos
{
    public class ReposPatientOmniPro
    {
        private ICnxDataBase m_Cnx;

        public ReposPatientOmniPro()
        {
            m_Cnx = new CnxAccess();
            m_Cnx.Table = "patients";
        }

        ~ReposPatientOmniPro()
        {
            m_Cnx = null;
        }

        private List<Patients> GetBySql(string pSql)
        {
            List<Patients> LstItem = new List<Patients>();
            try
            {
                m_Cnx.OpenDBConnection();
                m_Cnx.Requete = pSql;
                OleDbDataReader dr_Item = m_Cnx.Query<OleDbDataReader>();
                if (dr_Item.HasRows)
                {
                    while (dr_Item.Read())
                    {
                        LstItem.Add(new Patients()
                        {
                            PKGUID = dr_Item["PkGuid"].ToString(),
                            NUMPAT = int.Parse(dr_Item["NUMPAT"].ToString()),
                            CLENOM = dr_Item["CLENOM"].ToString(),
                            CLEPRENOM = dr_Item["CLEPRENOM"].ToString(),
                            NOM = dr_Item["NOM"].ToString(),
                            PRENOM = dr_Item["PRENOM"].ToString()
                        });
                    }
                    dr_Item.Close();
                }
                m_Cnx.CloseDBConnection();
            }
            catch (Exception ex)
            {
                ErrorLog.SaveLog("Bacariq.Lawyer.Dal.ImporOmniPro.ReposPatientOmniPro.GetAll", ex.ToString());
            }
            finally
            {
                m_Cnx.CloseDBConnection();
            }
            return LstItem;
        }

        public void SetGuid()
        {
            string sQuery = "select numpat from patients where Len(PkGuid) is null or Len(PkGuid) = 0 ";
            List<int> LstItem = new List<int>();
            try
            {
                m_Cnx.OpenDBConnection();
                m_Cnx.Requete = sQuery;
                OleDbDataReader dr_Item = m_Cnx.Query<OleDbDataReader>();
                if (dr_Item != null && dr_Item.HasRows)
                {
                    while (dr_Item.Read())
                    {
                        LstItem.Add(int.Parse(dr_Item["numpat"].ToString()));
                    }
                    dr_Item.Close();
                }
                m_Cnx.CloseDBConnection();
            }
            catch (Exception ex)
            {
                ErrorLog.SaveLog("testApi.Repos.ReposPatientOmniPro.SetGuid", ex.ToString());
            }
            finally
            {
                m_Cnx.CloseDBConnection();
            }

            m_Cnx.OpenDBConnection();
            foreach (int item in LstItem)
            {
                string mGuid = Guid.NewGuid().ToString();
                sQuery = $"update patients set PkGuid = '{mGuid}' where numpat = {item}";
                m_Cnx.Requete = sQuery;

                sQuery = $"insert into Correspondance (NumPat, PkGuidPat) value ({item}, '{mGuid}')";
                m_Cnx.Requete = sQuery;
                m_Cnx.Query<OleDbDataReader>();
            }
            m_Cnx.CloseDBConnection();
        }

        public void SetGuid(int pNumpat)
        {
            string sQuery = $"update patients set PkGuid = '{Guid.NewGuid().ToString()}' where numpat = {pNumpat}";
            m_Cnx.OpenDBConnection();
            m_Cnx.Requete = sQuery;
            OleDbDataReader dr_Item = m_Cnx.Query<OleDbDataReader>();
            m_Cnx.CloseDBConnection();
        }

        public Patients GetByGuid(int pNumpat)
        {
            string sQuery = "select * from patients where numpat = " + pNumpat;
            return GetBySql(sQuery).FirstOrDefault();
        }
    }
}
