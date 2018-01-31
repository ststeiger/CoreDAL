
namespace CoreDb
{


    partial class ReadDAL
    {


        protected System.Data.Common.DbProviderFactory m_factory;



        public virtual System.Data.Common.DbProviderFactory GetFactory(System.Type type)
        {
            if (type != null && System.Reflection.IntrospectionExtensions.GetTypeInfo(type).IsSubclassOf(typeof(System.Data.Common.DbProviderFactory)))
            {
                // Provider factories are singletons with Instance field having
                // the sole instance


                System.Reflection.FieldInfo field = System.Reflection.IntrospectionExtensions.GetTypeInfo(type).GetField("Instance"
                    , System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static
                );

                if (field != null)
                {
                    return (System.Data.Common.DbProviderFactory)field.GetValue(null);
                    //return field.GetValue(null) as DbProviderFactory;
                } // End if (field != null)

            } // End if (type != null && type.IsSubclassOf(typeof(System.Data.Common.DbProviderFactory)))

            throw new System.Exception("DataProvider is missing!");
            //throw new System.Configuration.ConfigurationException("DataProvider is missing!");
        } // End Function GetFactory


        public System.Data.Common.DbProviderFactory GetFactory<T>()
        {
            System.Type t = typeof(T);
            return GetFactory(t);
        } // End Function GetFactory


        public virtual System.Data.Common.DbProviderFactory GetFactory(string assemblyType)
        {
#if TARGET_JVM // case insensitive GetType is not supported
			Type type = Type.GetType (assemblyType, false);
#else
            System.Type type = System.Type.GetType(assemblyType, false, true);
#endif

            return GetFactory(type);
        } // End Function GetFactory


    }


}
