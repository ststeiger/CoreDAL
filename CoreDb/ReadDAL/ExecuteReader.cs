
namespace CoreDb
{

    public delegate void DataReaderCallback_t(System.Data.Common.DbDataReader reader);


    partial class ReadDAL
    {


        public virtual void ExecuteReader(System.Data.IDbCommand cmd, DataReaderCallback_t callback)
        {
            using (System.Data.Common.DbConnection con = this.Connection)
            {
                cmd.Connection = con;

                if (con.State != System.Data.ConnectionState.Open)
                    con.Open();

                using (System.Data.Common.DbDataReader idr = (System.Data.Common.DbDataReader)cmd.ExecuteReader(
                        System.Data.CommandBehavior.SequentialAccess | System.Data.CommandBehavior.CloseConnection
                ))
                {
                    callback(idr);
                }

                if (con.State != System.Data.ConnectionState.Closed)
                    con.Close();
            } // End Using con 
        }


        public virtual void ExecuteReader(string sql, DataReaderCallback_t callback)
        {
            using (System.Data.Common.DbCommand cmd = CreateCommand(sql))
            {
                ExecuteReader(cmd, callback);
            } // End Using cmd 
        }



        public virtual System.Data.Common.DbDataReader ExecuteReader(
              System.Data.IDbConnection conn
            , System.Data.IDbCommand cmd
            , System.Data.CommandBehavior cmdBehaviour)
        {
            System.Data.Common.DbDataReader dataReader = null;

            try
            {
                cmd.Connection = conn;

                if (cmd.Connection.State != System.Data.ConnectionState.Open)
                    cmd.Connection.Open();

                // this line is Evil ;)
                dataReader = ((System.Data.Common.DbCommand)cmd).ExecuteReader(cmdBehaviour);
            }
            catch (System.Exception ex)
            {
                if (Log(ex, cmd))
                    throw;
            }

            return dataReader;
        }


        public virtual System.Data.Common.DbDataReader ExecuteReader(System.Data.IDbCommand cmd, System.Data.CommandBehavior cmdBehaviour)
        {
            System.Data.Common.DbDataReader idr = null;
            System.Data.IDbConnection idbc = null;

            try
            {
                idbc = this.Connection;
            }
            catch (System.Exception ex)
            {
                if (Log(ex, cmd))
                    throw;
            }

            idr = ExecuteReader(idbc, cmd, cmdBehaviour);

            return idr;
        } // End Function ExecuteReader


        public virtual System.Data.Common.DbDataReader ExecuteReader(System.Data.IDbCommand cmd)
        {
            return ExecuteReader(cmd, System.Data.CommandBehavior.CloseConnection);
        } // End Function ExecuteReader




        public virtual System.Data.Common.DbDataReader ExecuteReader(System.Data.IDbConnection conn, string strSQL, System.Data.CommandBehavior cmdBehaviour)
        {
            System.Data.Common.DbDataReader idr = null;

            using (System.Data.Common.DbCommand cmd = this.CreateCommand(strSQL))
            {
                idr = ExecuteReader(conn, cmd, cmdBehaviour);
            } // End Using cmd

            return idr;
        } // End Function ExecuteReader


        public virtual System.Data.Common.DbDataReader ExecuteReader(string strSQL, System.Data.CommandBehavior cmdBehaviour)
        {
            System.Data.Common.DbDataReader idr = null;

            using (System.Data.Common.DbCommand cmd = this.CreateCommand(strSQL))
            {
                idr = ExecuteReader(cmd, cmdBehaviour);
            } // End Using cmd

            return idr;
        } // End Function ExecuteReader


        public virtual System.Data.Common.DbDataReader ExecuteReader(string strSQL)
        {
            return ExecuteReader(strSQL, System.Data.CommandBehavior.CloseConnection);
        } // End Function ExecuteReader


        public virtual System.Data.Common.DbDataReader ExecuteReader(System.Data.IDbConnection conn, string strSQL)
        {
            System.Data.Common.DbDataReader idr = null;

            using (System.Data.Common.DbCommand cmd = this.CreateCommand(strSQL))
            {
                idr = ExecuteReader(conn, cmd, System.Data.CommandBehavior.CloseConnection);
            } // End Using cmd

            return idr;
        } // End Function ExecuteReader


    }


}
