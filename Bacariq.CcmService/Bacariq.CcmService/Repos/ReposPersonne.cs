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
    public class ReposPersonneOmniPro
    {
        private ICnxDataBase m_Cnx;

        public ReposPersonneOmniPro()
        {
            m_Cnx = new CnxAccess();
            m_Cnx.Table = "personnes";
        }

        ~ReposPersonneOmniPro()
        {
            m_Cnx = null;
        }

        private List<Personnes> GetBySql(string pSql)
        {
            List<Personnes> LstItem = new List<Personnes>();
            try
            {
                m_Cnx.OpenDBConnection();
                m_Cnx.Requete = pSql;
                OleDbDataReader dr_Item = m_Cnx.Query<OleDbDataReader>();
                if (dr_Item.HasRows)
                {
                    while (dr_Item.Read())
                    {
                        LstItem.Add(new Personnes()
                        {
                            //PKGUID = "",
                            NUMPERS = int.Parse(dr_Item["numpers"].ToString()),
                            NOM = dr_Item["nom"].ToString(),
                            PRENOM = dr_Item["prenom"].ToString(),
                            CLENOM = dr_Item["Cle"].ToString(),
                            CLEPRENOM = dr_Item["Cleprenom"].ToString()
                        });
                    }
                    dr_Item.Close();
                }
                m_Cnx.CloseDBConnection();
            }
            catch (Exception ex)
            {
                ErrorLog.SaveLog("Bacariq.Lawyer.Dal.ImporOmniPro.ReposPersonneOmniPro.GetAll", ex.ToString());
            }
            finally
            {
                m_Cnx.CloseDBConnection();
            }
            return LstItem;
        }

        public Personnes GetByGuid(int pNumPers)
        {
            string sQuery = "select * from personnes where numpers = " + pNumPers;
            return GetBySql(sQuery).FirstOrDefault();
        }
    }
}
