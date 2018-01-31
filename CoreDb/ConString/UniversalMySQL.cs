
namespace CoreDb
{
    

    public class MySqlUniversalConnectionStringBuilder : UniversalConnectionStringBuilder
    {
        protected new MySql.Data.MySqlClient.MySqlConnectionStringBuilder m_ConnectionStringBuilder;


        public MySqlUniversalConnectionStringBuilder()
        {
            this.m_ConnectionStringBuilder = new MySql.Data.MySqlClient.MySqlConnectionStringBuilder();
        }

        public MySqlUniversalConnectionStringBuilder(string connectionString)
        {
            this.m_ConnectionStringBuilder = new MySql.Data.MySqlClient.MySqlConnectionStringBuilder(connectionString);
        }



        public override System.Data.Common.DbConnectionStringBuilder csb
        {
            get
            {
                return this.m_ConnectionStringBuilder;
            }
        }


        public override string ApplicationName
        {
            get
            {
                // object val;
                // bool success = m_ConnectionStringBuilder.TryGetValue("ApplicationName", out val);
                // return (string)val;

                return (string)this.m_ConnectionStringBuilder["Application Name"];
            }
            set
            {
                this.m_ConnectionStringBuilder["Application Name"] = value;
            }
        }


        public override string Server
        {
            get
            {
                return this.m_ConnectionStringBuilder.Server;
            }
            set
            {
                this.m_ConnectionStringBuilder.Server = value;
            }
        }

        public override int Port
        {
            get
            {
                return (int) this.m_ConnectionStringBuilder.Port;
            }
            set
            {
                this.m_ConnectionStringBuilder.Port = (uint) value;
            }
        }



        public override string DataBase
        {
            get
            {
                return this.m_ConnectionStringBuilder.Database;
            }
            set
            {
                this.m_ConnectionStringBuilder.Database = value;
            }
        }


        public override bool IntegratedSecurity
        {
            get
            {
                // return this.m_ConnectionStringBuilder.IntegratedSecurity;
                object val;
                bool success = m_ConnectionStringBuilder.TryGetValue("Integrated Security", out val);
                return (bool)val;
            }
            set
            {
                this.m_ConnectionStringBuilder["Integrated Security"] = value;
            }
        }


        public override string UserName
        {
            get
            {
                return this.m_ConnectionStringBuilder.UserID;
            }
            set
            {
                this.m_ConnectionStringBuilder.UserID = value;
            }
        }

        public override string Password
        {
            get
            {
                // return this.m_ConnectionStringBuilder.Password;
                return "********";
            }
            set
            {
                this.m_ConnectionStringBuilder.Password = value;
            }

        }


        public override bool IsReadOnly
        {
            get
            {
                object val;
                var x = m_ConnectionStringBuilder.TryGetValue("IsReadOnly", out val);
                return (bool)val;
            }
        }


        public override bool SSL
        {
            get
            {
                if (this.m_ConnectionStringBuilder.SslMode == MySql.Data.MySqlClient.MySqlSslMode.Required)
                    return true;

                return false;
            }
            set
            {
                if (value)
                    this.m_ConnectionStringBuilder.SslMode = MySql.Data.MySqlClient.MySqlSslMode.Required;
                else
                    this.m_ConnectionStringBuilder.SslMode = MySql.Data.MySqlClient.MySqlSslMode.Preferred;
            }
        }


        //public override bool IsFixedSize
        //{
        //    get
        //    {
        //        return this.m_ConnectionStringBuilder.IsFixedSize;
        //    }
        //}


        public override bool Pooling
        {
            get
            {
                return this.m_ConnectionStringBuilder.Pooling;
            }
            set
            {
                this.m_ConnectionStringBuilder.Pooling = value;
            }
        }


        public override int MinPoolSize
        {
            get
            {
                return (int)this.m_ConnectionStringBuilder.MinimumPoolSize;
            }
            set
            {
                this.m_ConnectionStringBuilder.MinimumPoolSize = (uint)value;
            }
        }


        public override int MaxPoolSize
        {
            get
            {
                return (int) this.m_ConnectionStringBuilder.MaximumPoolSize;
            }
            set
            {
                this.m_ConnectionStringBuilder.MaximumPoolSize = (uint)value;
            }
        }


        public override bool Enlist
        {
            get
            {
                return (bool)this.m_ConnectionStringBuilder["Enlist"];
            }
            set
            {
                this.m_ConnectionStringBuilder["Enlist"] = value;
            }
        }


        public override bool PreloadReader
        {
            get
            {
                return (bool)this.m_ConnectionStringBuilder["PreloadReader"];
            }
            set
            {
                this.m_ConnectionStringBuilder["PreloadReader"] = value;
            }
        }


        // Gets flag indicating if we are using Synchronous notification or not. The default value is false.
        //public override bool SyncNotification
        //{
        //    get
        //    {
        //        return this.m_ConnectionStringBuilder.SyncNotification;
        //    }
        //    set
        //    {
        //        this.m_ConnectionStringBuilder.SyncNotification = value;
        //    }
        //}


        // ConnectionTimeout=0 is bad, make it something reasonable like 30 seconds. 
        public override int ConnectTimeout
        {
            get
            {
                return (int) this.m_ConnectionStringBuilder.ConnectionTimeout;
            }
            set
            {
                this.m_ConnectionStringBuilder.ConnectionTimeout = (uint) value;
            }
        }



        // how long a connection lives before it is killed and recreated. 
        // A lifetime of 0 means never kill and recreate. 
        // Through various bugs your connections may get stuck in an unstable state 
        // (like when dealing with weird 3 way transactions).. 
        // but 99% of the time it is good to keep connection lifetime as infinite.
        public override int ConnectionLifeTime
        {
            get
            {
                return (int)this.m_ConnectionStringBuilder["ConnectionLifeTime"];
            }
            set
            {
                this.m_ConnectionStringBuilder["ConnectionLifeTime"] = value;
            }
        }


        public override int CommandTimeout
        {
            get
            {
                return (int)this.m_ConnectionStringBuilder["CommandTimeout"];
            }
            set
            {
                this.m_ConnectionStringBuilder["CommandTimeout"] = value;
            }
        }


        public override string ConnectionString
        {
            get
            {
                return this.m_ConnectionStringBuilder.ConnectionString;
            }
            set
            {
                this.m_ConnectionStringBuilder.ConnectionString = value;
            }

        }


        public override string SearchPath
        {
            get
            {
                return (string)this.m_ConnectionStringBuilder["SearchPath"];
            }
            set
            {
                this.m_ConnectionStringBuilder["SearchPath"] = value;
            }

        }


    } // End Class MySqlUniversalConnectionStringBuilder 


} // End Namespace 
