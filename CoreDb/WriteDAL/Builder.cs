
namespace CoreDb
{


    partial class WriteDAL
    {

        public virtual void InsertUpdateDataTable(string tableName, System.Data.DataTable dt)
        {
            InsertUpdateDataTable(null, tableName, dt);
        }

        public virtual void InsertUpdateDataTable(string tableSchema, string tableName, System.Data.DataTable dt)
        {
            string strSQL = "SELECT * FROM ";

            if (tableSchema != null)
            {
                strSQL += this.QuoteObjectIfNecessary(tableSchema) + ".";
            }

            strSQL += this.QuoteObjectIfNecessary(tableName) + " WHERE (1 = 2) ";

            using (System.Data.Common.DbConnection connection = this.Connection)
            {

                using (System.Data.Common.DbDataAdapter daInsertUpdate = this.m_factory.CreateDataAdapter())
                {

                    using (System.Data.Common.DbCommand cmdSelect = connection.CreateCommand())
                    {
                        cmdSelect.CommandText = strSQL;

                        System.Data.Common.DbCommandBuilder cb = this.m_factory.CreateCommandBuilder();
                        cb.DataAdapter = daInsertUpdate;

                        daInsertUpdate.SelectCommand = cmdSelect;
                        daInsertUpdate.InsertCommand = cb.GetInsertCommand();
                        daInsertUpdate.UpdateCommand = cb.GetUpdateCommand();
                        daInsertUpdate.DeleteCommand = cb.GetDeleteCommand();

                        daInsertUpdate.Update(dt);
                    } // End Using cmdSelect

                } // End Using daInsertUpdate
                
            } // End Using connection 

        } // End Sub InsertUpdateDataTable 


    }


}
