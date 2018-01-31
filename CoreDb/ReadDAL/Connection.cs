
namespace CoreDb
{


    partial class ReadDAL
    {

        protected string m_ConnectionString;


        public virtual string ConnectionString
        {
            get
            {
                return m_ConnectionString;
            }

            set
            {
                 m_ConnectionString = value;
            }
        }


        public virtual string GetConnectionString(string dbName)
        {
            string conStr = this.ConnectionString;

            System.Data.SqlClient.SqlConnectionStringBuilder csb = new System.Data.SqlClient.SqlConnectionStringBuilder(conStr);
            csb.InitialCatalog = dbName;
            conStr = csb.ConnectionString;
            csb.Clear();
            csb = null;

            return conStr;
        }


        public System.Data.Common.DbConnection Connection
        {
            get
            {
                System.Data.Common.DbConnection conn = m_factory.CreateConnection();
                conn.ConnectionString = this.ConnectionString;

                return conn;
            }
        }


        public virtual System.Data.IDbConnection GetConnection(string dbName)
        {
            if (dbName == null)
                return this.Connection;

            System.Data.Common.DbConnection con = this.m_factory.CreateConnection();
            con.ConnectionString = GetConnectionString(dbName);

            return con;
        }


    }


}
