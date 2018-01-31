
namespace CoreDb
{

    public delegate void callbackAddData_t<T>(System.Data.IDbCommand cmd, T thisItem);
    public delegate void callbackAddDataClosure_t<T>(T thisItem);
    

    partial class WriteDAL : ReadDAL
    {


        public virtual void InsertList<T>(string cmdInsert
            , System.Collections.Generic.IEnumerable<T> listToInsert
            , callbackAddDataClosure_t<T> addDataCallback)
        {
            using (System.Data.Common.DbCommand cmd = this.CreateCommand(cmdInsert))
            {
                InsertList<T>(cmd, listToInsert, addDataCallback);
            }

        } // End Sub InsertList 



        public virtual void InsertList<T>(System.Data.IDbCommand cmdInsert
            , System.Collections.Generic.IEnumerable<T> listToInsert
            , callbackAddDataClosure_t<T> addDataCallback)
        {
            using (System.Data.Common.DbConnection conn = this.Connection)
            {
                if (conn.State != System.Data.ConnectionState.Open)
                    conn.Open();

                cmdInsert.Connection = conn;

                using (System.Data.Common.DbTransaction transact = conn.BeginTransaction())
                {
                    cmdInsert.Transaction = transact;

                    try
                    {
                        foreach (T thisItem in listToInsert)
                        {
                            addDataCallback(thisItem);

                            if (cmdInsert.ExecuteNonQuery() != 1)
                            {
                                //'handled as needed, 
                                //' but this snippet will throw an exception to force a rollback
                                throw new System.InvalidProgramException();
                            } // End if (cmdInsert.ExecuteNonQuery() != 1) 

                        } // Next thisObject
                        transact.Commit();
                    } // End Try 
                    catch (System.Exception)
                    {
                        transact.Rollback();
                        throw;
                    } // End Catch 
                    finally
                    {
                        if (conn.State != System.Data.ConnectionState.Closed)
                            conn.Close();
                    } // End Finally

                } // End Using transact 

            } // End Using conn 

        } // End Sub InsertList 



        public virtual void InsertList<T>(string cmdInsert
            , System.Collections.Generic.IEnumerable<T> listToInsert
            , callbackAddData_t<T> addDataCallback)
        {

            using (System.Data.Common.DbCommand cmd = this.CreateCommand(cmdInsert))
            {
                InsertList<T>(cmd, listToInsert, addDataCallback);
            } // End Using cmd 

        } // End Sub InsertList 


        public virtual void InsertList<T>(System.Data.IDbCommand cmdInsert
            , System.Collections.Generic.IEnumerable<T> listToInsert
            , callbackAddData_t<T> addDataCallback)
        {
            using (System.Data.Common.DbConnection conn = this.Connection)
            {
                if (conn.State != System.Data.ConnectionState.Open)
                    conn.Open();

                cmdInsert.Connection = conn;

                using (System.Data.Common.DbTransaction transact = conn.BeginTransaction())
                {
                    cmdInsert.Transaction = transact;

                    try
                    {
                        foreach (T thisItem in listToInsert)
                        {
                            addDataCallback(cmdInsert, thisItem);

                            if (cmdInsert.ExecuteNonQuery() != 1)
                            {
                                // handled as needed, 
                                // but this snippet will throw an exception to force a rollback
                                throw new System.InvalidProgramException();
                            } // End if (cmdInsert.ExecuteNonQuery() != 1) 

                        } // Next thisObject
                        transact.Commit();
                    } // End Try 
                    catch (System.Exception)
                    {
                        transact.Rollback();
                        throw;
                    } // End Catch 
                    finally
                    {
                        if (conn.State != System.Data.ConnectionState.Closed)
                            conn.Close();
                    } // End Finally

                } // End Using transact 

            } // End Using conn 

        } // End Sub InsertList 
        


        public virtual int Execute(System.Data.IDbCommand cmd)
        {
            int iAffected = -1;

            try
            {

                using (System.Data.IDbConnection idbConn = this.Connection)
                {
                    cmd.Connection = idbConn;

                    if (cmd.Connection.State != System.Data.ConnectionState.Open)
                        cmd.Connection.Open();

                    using (System.Data.IDbTransaction idbtTrans = idbConn.BeginTransaction())
                    {

                        try
                        {
                            cmd.Transaction = idbtTrans;

                            iAffected = cmd.ExecuteNonQuery();
                            idbtTrans.Commit();
                        } // End Try
                        catch (System.Data.Common.DbException ex)
                        {
                            if (idbtTrans != null)
                                idbtTrans.Rollback();

                            iAffected = -2;

                            if (this.Log(ex))
                                throw;
                        } // End catch
                        finally
                        {
                            if (cmd.Connection.State != System.Data.ConnectionState.Closed)
                                cmd.Connection.Close();
                        } // End Finally

                    } // End Using idbtTrans


                } // End Using idbConn

            } // End Try
            catch (System.Exception ex)
            {
                iAffected = -3;
                if (Log(ex, cmd))
                    throw;
            }
            finally
            {

            }

            return iAffected;
        } // End Function Execute


        public virtual int Execute(string strSQL)
        {
            int iReturnValue = -1;

            using (System.Data.IDbCommand cmd = this.CreateCommand(strSQL))
            {
                iReturnValue = Execute(cmd);
            } // End Using cmd 

            return iReturnValue;
        } // End Sub Execute
        

        public virtual System.Exception Execute(System.Data.IDbCommand cmd, bool bReturnError)
        {
            System.Exception exReturnValue = null;

            using (System.Data.IDbConnection idbc = this.Connection)
            {

                try
                {
                    cmd.Connection = idbc;

                    if (idbc.State != System.Data.ConnectionState.Open)
                        idbc.Open();

                    cmd.ExecuteNonQuery();
                } // End Try
                catch (System.Data.Common.DbException ex)
                {
                    Log("cDAL.Execute(System.Data.IDbCommand cmd, bool bReturnError)", ex, cmd);
                    exReturnValue = ex;
                } // End Catch
                finally
                {
                    if (idbc.State != System.Data.ConnectionState.Closed)
                        idbc.Close();

                } // End Finally

            } // End Using idbc

            return exReturnValue;
        } // End Function Execute


        public virtual System.Exception Execute(string strSQL, bool bReturnError)
        {
            System.Exception exReturnValue = null;

            using (System.Data.IDbCommand cmd = this.CreateCommand(strSQL))
            {
                exReturnValue = Execute(cmd, bReturnError);
            } // End Using cmd

            return exReturnValue;
        } // End Function Execute



        public virtual int ExecuteWithoutTransaction(string strSQL)
        {
            return ExecuteWithoutTransaction(strSQL, 30);
        }


        public virtual int ExecuteWithoutTransaction(string strSQL, int iTimeout)
        {
            int iReturnValue = -1;

            using (System.Data.IDbCommand cmd = this.CreateCommand(strSQL))
            {
                iReturnValue = ExecuteWithoutTransaction(cmd, iTimeout);
            }

            return iReturnValue;
        } // End Sub Execute


        public virtual int ExecuteWithoutTransaction(System.Data.IDbCommand cmd)
        {
            return ExecuteWithoutTransaction(cmd, 30);
        }


        public virtual int ExecuteWithoutTransaction(System.Data.IDbCommand cmd, int iTimeout)
        {
            int iAffected = -1;

            try
            {

                using (System.Data.IDbConnection idbConn = this.Connection)
                {

                    cmd.Connection = idbConn;
                    cmd.CommandTimeout = iTimeout;

                    try
                    {
                        if (cmd.Connection.State != System.Data.ConnectionState.Open)
                            cmd.Connection.Open();

                        iAffected = cmd.ExecuteNonQuery();
                    }
                    catch (System.Exception ex)
                    {
                        if (Log(ex, cmd))
                            throw;
                    }
                    finally
                    {
                        if (cmd.Connection.State != System.Data.ConnectionState.Closed)
                            cmd.Connection.Close();
                    } // End Finally

                } // End Using idbConn

            } // End Try
            catch (System.Exception ex)
            {
                iAffected = -3;
                if (Log(ex, cmd))
                    throw;
            }
            finally
            {

            }

            return iAffected;
        } // End Function Execute
        

    }


}
