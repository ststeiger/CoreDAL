
namespace CoreDb
{

 
    partial class ReadDAL
    {


        public virtual System.Data.Common.DbDataReader ExecuteStoredProcedureReader(System.Data.IDbCommand cmd)
        {
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            return this.ExecuteReader(cmd);
        } // End Function ExecuteReaderFromStoredProcedure


        public virtual System.Data.Common.DbDataReader ExecuteStoredProcedureReader(string strStoredProcedureName)
        {
            using (System.Data.Common.DbCommand cmd = this.CreateCommand(strStoredProcedureName))
            {
                return this.ExecuteStoredProcedureReader(cmd);
            } // End Using cmd

        } // End Function ExecuteReaderFromStoredProcedure


    }


}
