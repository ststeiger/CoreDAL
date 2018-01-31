using System;
using System.Collections.Generic;
using System.Text;

namespace CoreDb
{
    partial class ReadDAL
    {



        public virtual object ExecuteStoredProcedure(System.Data.IDbCommand cmd)
        {
            object objReturnValue = null;
            try
            {


                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                //// System.Data.SqlClient.SqlParameter returnValue = new System.Data.SqlClient.SqlParameter();
                System.Data.IDbDataParameter returnValue = cmd.CreateParameter();
                returnValue.Direction = System.Data.ParameterDirection.ReturnValue;

                //this.AddParameter(cmd, "name", null, System.Data.ParameterDirection.ReturnValue);
                cmd.Parameters.Add(returnValue);

                using (System.Data.IDbConnection con = this.Connection)
                {
                    cmd.Connection = con;

                    try
                    {
                        if (cmd.Connection.State != System.Data.ConnectionState.Open)
                            cmd.Connection.Open();

                        cmd.ExecuteNonQuery();

                        //Assert.AreEqual(123, (int)returnValue.Value);
                        //System.Diagnostics.Debug.Assert(123 == (int)((System.Data.IDataParameter)cmd.Parameters["name"]).Value);
                        //System.Diagnostics.Debug.Assert(123 == (int)returnValue.Value);
                        objReturnValue = returnValue.Value;
                    }
                    catch (System.Exception ex)
                    {
                        if (Log("cDAL.ExecuteStoredProcedure - inner", ex, cmd))
                            throw;
                    }
                    finally
                    {
                        if (cmd.Connection != null && cmd.Connection.State != System.Data.ConnectionState.Closed)
                            cmd.Connection.Close();
                    } // End Finally

                } // End using con

            }
            catch (System.Exception ex)
            {
                if (Log("cDAL.ExecuteStoredProcedure - outer", ex, cmd))
                    throw;
            } // End catch

            return objReturnValue;
        } // End Function ExecuteStoredProcedure


        public virtual object ExecuteStoredProcedure(string strProcedureName)
        {
            object objReturnValue = null;

            //// using (SqlCommand cmd = new SqlCommand("TestProc", cn))
            using (System.Data.Common.DbCommand cmd = this.CreateCommand(strProcedureName))
            {
                objReturnValue = ExecuteStoredProcedure(cmd);
            } // End Using cmd

            return objReturnValue;
        } // End Function ExecuteStoredProcedure


    }


}
