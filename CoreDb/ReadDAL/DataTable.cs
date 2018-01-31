
namespace CoreDb 
{
    
    
    partial class ReadDAL
    {


        public virtual System.Data.DataTable GetDataTable(System.Data.IDbCommand cmd, string dbName)
        {
            System.Data.DataTable dt = new System.Data.DataTable();

            using (System.Data.IDbConnection idbc = this.GetConnection(dbName))
            {

                try
                {
                    cmd.Connection = idbc;

                    using (System.Data.Common.DbDataAdapter daQueryTable = this.m_factory.CreateDataAdapter())
                    {
                        daQueryTable.SelectCommand = (System.Data.Common.DbCommand)cmd;
                        daQueryTable.Fill(dt);
                    } // End Using daQueryTable

                } // End Try
                catch (System.Data.Common.DbException ex)
                {
                    // if (Log("cDAK.GetDataTable(System.Data.IDbCommand cmd)", ex, cmd.CommandText))
                    throw;
                }// End Catch
                finally
                {
                    if (idbc.State != System.Data.ConnectionState.Closed)
                        idbc.Close();
                } // End Finally

            } // End Using idbc

            return dt;
        } // End Function GetDataTable


        public virtual System.Data.DataTable GetDataTable(System.Data.IDbCommand cmd)
        {
            return GetDataTable(cmd, null);
        }


        public virtual System.Data.DataTable GetDataTable(string strSQL, string strInitialCatalog)
        {
            System.Data.DataTable dt = null;

            using (System.Data.IDbCommand cmd = this.CreateCommand(strSQL))
            {
                dt = GetDataTable(cmd, strInitialCatalog);
            } // End Using cmd

            return dt;
        } // End Function GetDataTable


        public virtual System.Data.DataTable GetDataTable(string strSQL)
        {
            return GetDataTable(strSQL, null);
        } // End Function GetDataTable


    }


}
