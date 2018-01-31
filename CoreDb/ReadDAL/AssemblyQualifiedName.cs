
namespace CoreDb
{


    partial class ReadDAL
    {


        public static string GetAssemblyQualifiedNoVersionName(System.Type type)
        {
            if (type == null)
                return null;

            return GetAssemblyQualifiedNoVersionName(type.AssemblyQualifiedName);
        } // End Function GetAssemblyQualifiedNoVersionName


        public static string GetAssemblyQualifiedNoVersionName(string input)
        {
            int i = 0;
            bool isNotFirst = false;
            for (; i < input.Length; ++i)
            {
                if (input[i] == ',')
                {
                    if (isNotFirst)
                        break;

                    isNotFirst = true;
                }
            }

            return input.Substring(0, i);
        } // End Function GetAssemblyQualifiedNoVersionName


    }


}
