
namespace CoreDb
{


    public class DalConfig //<T> where T : System.Data.Common.DbProviderFactory
    {

        const string defaultFactory = "System.Data.SqlClient";

        public GetConnectionString_t GetConnectionString;

        public string ProviderName;
        public System.Type ProviderFactoryType;
        public System.Data.Common.DbProviderFactory ProviderFactory;

        public string ConnectionString;
        public delegate string GetConnectionString_t(DalConfig conf);


        protected static System.Type TypeFromProvider(string providerName)
        {
            if (System.StringComparer.OrdinalIgnoreCase.Equals(providerName, "Npgsql"))
                return typeof(Npgsql.NpgsqlFactory);

            if (System.StringComparer.OrdinalIgnoreCase.Equals(providerName, "SqlClient")
                || System.StringComparer.OrdinalIgnoreCase.Equals(providerName, "System.Data.SqlClient")
                )
                return typeof(System.Data.SqlClient.SqlClientFactory);

            if (System.StringComparer.OrdinalIgnoreCase.Equals(providerName, "MySqlClient")
                || System.StringComparer.OrdinalIgnoreCase.Equals(providerName, "MySql.Data.MySqlClient")
                || System.StringComparer.OrdinalIgnoreCase.Equals(providerName, "MySql")
                )
                return typeof(System.Data.SqlClient.SqlClientFactory);


            //if (System.StringComparer.OrdinalIgnoreCase.Equals(providerName, "FirebirdClient")
            //    || System.StringComparer.OrdinalIgnoreCase.Equals(providerName, "FirebirdSql.Data.FirebirdClient")
            //    || System.StringComparer.OrdinalIgnoreCase.Equals(providerName, "FirebirdSql")
            //    )
            //    return typeof(FirebirdSql.Data.FirebirdClient.FirebirdClientFactory);


            int lastindex = providerName.LastIndexOf(".") + 1;
            providerName = providerName + "." + providerName.Substring(lastindex) + "Factory";

            // System.Data.SqlClient.SqlClientFactory fac;
            // Npgsql.NpgsqlFactory;
            // MySql.Data.MySqlClient.MySqlClientFactory;
            // FirebirdSql.Data.FirebirdClient.FirebirdClientFactory

            System.Type t = System.Type.GetType(providerName, false, true);
            return t;
        }


        protected static System.Data.Common.DbProviderFactory GetFactory(System.Type type)
        {
            if (type == null || !System.Reflection.IntrospectionExtensions.GetTypeInfo(type)
                .IsSubclassOf(typeof(System.Data.Common.DbProviderFactory)))
            {
                throw new System.InvalidOperationException("Type is NULL or not a subclass of System.Data.Common.DbProviderFactory");
            }

            System.Reflection.FieldInfo field = System.Reflection.IntrospectionExtensions
                .GetTypeInfo(type).GetField("Instance"
                , System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static
            );

            if (field == null)
                throw new System.InvalidOperationException("foo");

            return (System.Data.Common.DbProviderFactory)field.GetValue(null);
        }


        public DalConfig(System.Type type, string connectionString) : this(GetFactory(type), connectionString)
        { }

        public DalConfig(System.Data.Common.DbProviderFactory factory, string connectionString)
        {
            this.ProviderFactory = factory;
            this.ProviderFactoryType = this.ProviderFactory.GetType();
            this.ProviderName = this.ProviderFactoryType.Namespace;
            this.ConnectionString = connectionString;
        }

        public DalConfig(string providerName, string connectionString) :
            this(TypeFromProvider(providerName), connectionString)
        { }

        public DalConfig(string connectionString) :
            this(defaultFactory, connectionString)
        { }



        public DalConfig(System.Data.Common.DbProviderFactory factory, GetConnectionString_t getConnectionString)
        {
            this.GetConnectionString = getConnectionString;
            this.ProviderFactory = factory;
            this.ProviderFactoryType = this.ProviderFactory.GetType();
            this.ProviderName = this.ProviderFactoryType.Namespace;
            this.ConnectionString = getConnectionString(this);
        }

        public DalConfig(System.Type t, GetConnectionString_t getConnectionString)
            : this(GetFactory(t), getConnectionString)
        { }

        public DalConfig(string providerName, GetConnectionString_t getConnectionString) :
            this(TypeFromProvider(providerName), getConnectionString)
        { }

        public DalConfig(GetConnectionString_t getConnectionString) :
            this(defaultFactory, getConnectionString)
        { }


    }


}
