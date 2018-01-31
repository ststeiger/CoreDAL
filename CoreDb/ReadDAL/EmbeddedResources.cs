
namespace CoreDb
{


    partial class ReadDAL
    {


        public System.Data.Common.DbCommand CreateCommandFromFile(string resourceName)
        {
            string sql = this.GetEmbeddedResource(resourceName);
            return this.CreateCommand(sql);
        }

        public System.Data.Common.DbCommand CreateCommandFromFile(System.Type type, string resourceName)
        {
            string sql = this.GetEmbeddedResource(type, resourceName);
            return this.CreateCommand(sql);
        }

        public System.Data.Common.DbCommand CreateCommandFromFile(System.Reflection.Assembly asm, string resourceName)
        {
            string sql = this.GetEmbeddedResource(asm, resourceName);
            return this.CreateCommand(sql);
        }
        

        public string GetEmbeddedResource(string resourceName)
        {
            return GetEmbeddedResource(typeof(ReadDAL), resourceName);
        }


        public string GetEmbeddedResource(System.Type type, string resourceName)
        {
            System.Reflection.Assembly asm = System.Reflection.IntrospectionExtensions
                .GetTypeInfo(type).Assembly;

            return GetEmbeddedResource(asm, resourceName);
        }

        public string GetEmbeddedResource(System.Reflection.Assembly asm, string resourceName)
        {
            string resource = null;

            string foundResourceName = null;

            foreach (string thisResourceName in asm.GetManifestResourceNames())
            {
                if (thisResourceName.EndsWith(resourceName, System.StringComparison.OrdinalIgnoreCase))
                {
                    foundResourceName = thisResourceName;
                    break;
                }
            }

            if (foundResourceName == null)
                throw new System.IO.InvalidDataException("The provided resourceName is not present.");

            using (System.IO.Stream strm = asm.GetManifestResourceStream(foundResourceName))
            {
                using (System.IO.StreamReader sr = new System.IO.StreamReader(strm))
                {
                    resource = sr.ReadToEnd();
                }
            }

            return resource;
        }


    }


}
