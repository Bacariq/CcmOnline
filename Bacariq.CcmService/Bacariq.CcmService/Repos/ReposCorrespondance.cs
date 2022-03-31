using Bacariq.CcmService.Helper;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bacariq.CcmService.Repos
{
    public class ReposCorrespondance
    {
        private ICnxDataBase mCnx;
        public ReposCorrespondance()
        {
            mCnx = new CnxAccess();
            mCnx.Table = "Correspondance";

        }
        ~ReposCorrespondance()
        {
            mCnx = null;
        }

        public string GetGuid(int pNumRdv = -1, int pNumPat = -1)
        {
            string sRet = "";
            string sQuery = "";
            if (pNumRdv > 0)
            {
                sQuery = $"select PkGuidRdv from Correspondance where NumRdv = {pNumRdv} ";
                mCnx.OpenDBConnection();
                mCnx.Requete = sQuery;
                OleDbDataReader dr_Item = mCnx.Query<OleDbDataReader>();
                if (dr_Item != null && dr_Item.HasRows)
                {
                    while (dr_Item.Read())
                    {
                        sRet = dr_Item["PkGuidRdv"].ToString();
                    }
                    dr_Item.Close();
                }
                mCnx.CloseDBConnection();
            }
            if (pNumPat > 0)
            {
                sQuery = $"select PkGuidPat from Correspondance where NumPat = {pNumPat} ";
                mCnx.OpenDBConnection();
                mCnx.Requete = sQuery;
                OleDbDataReader dr_Item = mCnx.Query<OleDbDataReader>();
                if (dr_Item != null && dr_Item.HasRows)
                {
                    while (dr_Item.Read())
                    {
                        sRet = dr_Item["PkGuidPat"].ToString();
                    }
                    dr_Item.Close();
                }
                mCnx.CloseDBConnection();
            }

            return sRet;

        }

        public void CreateTableAccess()
        {

            string Sql = "CREATE TABLE Correspondance " +
                          " (ID INTEGER PRIMARY KEY, " +
                          "  NumRdv INTEGER NOT NULL UNIQUE DEFAULT 0, " +
                          "  NumPat INTEGER NOT NULL UNIQUE DEFAULT 0, " +
                          "  PkGuidRdv TEXT(50), " +
                          "  PkGuidPat TEXT(50))";

            mCnx.Requete = Sql;
            mCnx.OpenDBConnection();
            mCnx.Query<OleDbDataReader>();
            mCnx.CloseDBConnection();
        }

        public bool AddItem(int pNumRdv = -1, int pNumPat = -1)
        {
            string Sql = "";
            bool bRet = false;
            try
            {
                if (pNumRdv > 0)
                {
                    Sql = $"insert into Correspondance (NumRdv, PkGuidRdv) value ({pNumRdv}, '{Guid.NewGuid().ToString()}')";
                    mCnx.Requete = Sql;
                    mCnx.OpenDBConnection();
                    mCnx.Query<OleDbDataReader>();
                    mCnx.CloseDBConnection();
                    bRet = true;
                }
                if (pNumPat > 0)
                {
                    Sql = $"insert into Correspondance (NumPat, PkGuidPat) value ({pNumPat}, '{Guid.NewGuid().ToString()}')";
                    mCnx.Requete = Sql;
                    mCnx.OpenDBConnection();
                    mCnx.Query<OleDbDataReader>();
                    mCnx.CloseDBConnection();
                    bRet = true;
                }
            }
            catch (Exception ex)
            {
                bRet = false;
            }
            return bRet;
        }

    }
}
