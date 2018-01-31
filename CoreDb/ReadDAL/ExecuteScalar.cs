
namespace CoreDb
{


    partial class ReadDAL
    {


        public virtual T ExecuteScalar<T>(System.Data.IDbCommand cmd, bool throwOnAssignNullToNonNullableType)
        {
            object objReturnValue = null;

            using (System.Data.IDbConnection idbc = this.Connection)
            {
                cmd.Connection = idbc;

                try
                {
                    if (cmd.Connection.State != System.Data.ConnectionState.Open)
                        cmd.Connection.Open();

                    objReturnValue = cmd.ExecuteScalar();
                } // End Try
                catch (System.Data.Common.DbException ex)
                {
                    if (Log(ex, cmd))
                        throw;
                } // End Catch
                finally
                {
                    if (cmd.Connection.State != System.Data.ConnectionState.Closed)
                        cmd.Connection.Close();
                } // End Finally

            } // End using idbc

            return ConvertResult<T>(objReturnValue, throwOnAssignNullToNonNullableType);
        } // End Function ExecuteScalar(cmd)


        public virtual T ExecuteScalar<T>(System.Data.IDbCommand cmd)
        {
            return ExecuteScalar<T>(cmd, true);
        }


        public virtual T ExecuteScalar<T>(string strSQL, bool throwOnAssignNullToNonNullableType)
        {
            T tReturnValue = default(T);

            // pfff, mono C# compiler problem...
            //sqlCMD = new System.Data.SqlClient.SqlCommand(strSQL, m_SqlConnection);
            using (System.Data.IDbCommand cmd = CreateCommand(strSQL))
            {
                tReturnValue = ExecuteScalar<T>(cmd, throwOnAssignNullToNonNullableType);
            } // End Using cmd

            return tReturnValue;
        } // End Function ExecuteScalar(strSQL)


        public virtual T ExecuteScalar<T>(string strSQL)
        {
            return ExecuteScalar<T>(strSQL, true);
        } // End Function ExecuteScalar(strSQL)


    }


}
