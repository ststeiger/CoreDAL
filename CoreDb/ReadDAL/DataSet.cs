
namespace CoreDb
{


    partial class ReadDAL
    {


        public virtual System.Data.DataSet GetDataSet(System.Data.IDbCommand cmd)
        {
            string datasetName = "NewDataSet";
            System.Data.DataSet ds = new System.Data.DataSet(datasetName);

            using (System.Data.IDbConnection idbc = this.Connection)
            {

                try
                {
                    cmd.Connection = idbc;

                    using (System.Data.Common.DbDataAdapter daQueryTable = this.m_factory.CreateDataAdapter())
                    {
                        daQueryTable.SelectCommand = (System.Data.Common.DbCommand)cmd;
                        daQueryTable.Fill(ds);
                    } // End Using daQueryTable

                } // End Try
                catch (System.Data.Common.DbException ex)
                {
                    //if (Log("cDAL.GetDataTable(System.Data.IDbCommand cmd)", ex, cmd.CommandText))
                    throw;
                }// End Catch
                finally
                {
                    if (idbc.State != System.Data.ConnectionState.Closed)
                        idbc.Close();
                } // End Finally

            } // End Using idbc

            return ds;
        } // End Function GetDataSet


        public virtual System.Data.DataSet GetDataSet(string strSQL)
        {
            System.Data.DataSet ds = null;

            using (System.Data.IDbCommand cmd = CreateCommand(strSQL))
            {
                ds = GetDataSet(cmd);
            } // End Using cmd 

            return ds;
        } // End Function GetDataSet


    }


}
