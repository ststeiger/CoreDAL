
using System;



using Microsoft.Extensions.Configuration;
// using Microsoft.Extensions.Configuration.Json;
// using Microsoft.Extensions.Configuration.UserSecrets;


public class Entry
{
    public string Key;
    public string Value;


    public Entry() : this(System.Guid.NewGuid().ToString(), "")
    { }


    public Entry(string key, string value)
    {
        this.Key = key;
        this.Value = value;
    }

}


public class DalTests
{

    CoreDb.WriteDAL DAL = null;


    public void InsertTest()
    {
        System.Collections.Generic.List<Entry> list = new System.Collections.Generic.List<Entry>();
        for (int i = 0; i < 10; ++i)
        {
            list.Add(new Entry(i.ToString(), i.ToString()));
        }


        System.Data.Common.DbCommand cmd = DAL.CreateCommand();
        cmd.CommandType = System.Data.CommandType.Text;
        cmd.CommandText = "INSERT INTO Setting(Key, Value) VALUES(@key, @value);";

        DAL.InsertList<Entry>(cmd, list,
            delegate (System.Data.IDbCommand cmd1, Entry thisItem)
            {
                DAL.SetParameter(cmd1.Parameters[0], thisItem.Key);
                DAL.SetParameter(cmd1.Parameters[1], thisItem.Value);
            } // End delegate

        );


        DAL.InsertList<Entry>(cmd, list,
            delegate (Entry thisItem)
            {
                DAL.SetParameter(cmd.Parameters[0], thisItem.Key);
                DAL.SetParameter(cmd.Parameters[1], thisItem.Value);
            } // End delegate
        );

    } // End Sub 


}


class Program
{




    public class ConsoleEnvironment
    {

        protected string m_MachineName;


        public ConsoleEnvironment()
        {
            this.m_MachineName = System.Environment.GetEnvironmentVariable("COMPUTERNAME");
            string loc = System.Reflection.IntrospectionExtensions.GetTypeInfo(typeof(ConsoleEnvironment)).Assembly.Location;
            this.ContentRootPath = System.IO.Path.GetDirectoryName(loc);
        }


        public string ContentRootPath { get; set; }

        //
        // Summary:
        //     Gets or sets the name of the environment. This property is automatically set
        //     by the host to the value of the "ASPNETCORE_ENVIRONMENT" environment variable.
        public string EnvironmentName { get; set; }

        //
        // Summary:
        //     Gets or sets the name of the application. This property is automatically set
        //     by the host to the assembly containing the application entry point.
        public string ApplicationName { get; set; }


        public string MachineName
        {
            get
            {
                return this.m_MachineName;
            }
            set
            {
                this.m_MachineName = value;
            }
        }


    }

    public static Microsoft.Extensions.Configuration.IConfigurationRoot Config { get; set; }


    public static string GetMsCon()
    {

        System.Data.SqlClient.SqlConnectionStringBuilder csb = new System.Data.SqlClient.SqlConnectionStringBuilder();
        csb.IntegratedSecurity = false;

        if (!csb.IntegratedSecurity)
        {
            csb.UserID = "sa";
            csb.Password = "Password123";
        }

        csb.DataSource = "127.0.0.1";
        csb.InitialCatalog = "master";

        // csb.CurrentLanguage = "de";
        csb.ConnectTimeout = 15;

        csb.PersistSecurityInfo = false;
        csb.PacketSize = 4096;

        csb.Pooling = true;
        csb.MinPoolSize = 1;
        csb.MaxPoolSize = 5;

        csb.MultipleActiveResultSets = true;
        csb.WorkstationID = System.Environment.GetEnvironmentVariable("COMPUTERNAME");


        csb.ApplicationName = "CoreDb Test";

        return csb.ConnectionString;
    }


    public static string GetPgCon()
    {
        Npgsql.NpgsqlConnectionStringBuilder csb = new Npgsql.NpgsqlConnectionStringBuilder();
        csb.Host = "127.0.0.1";
        csb.Port = 5432;
        csb.Database = "postgres";

        csb.IntegratedSecurity = false;

        if (!csb.IntegratedSecurity)
        {
            csb.Username = "postgres";
            csb.Password = "foobar";
        }

        csb.PersistSecurityInfo = false;
        csb.MinPoolSize = 1;
        csb.MaxPoolSize = 5;
        csb.Pooling = true;

        csb.ApplicationName = "foobar Application";
        csb.Encoding = System.Text.Encoding.UTF8.WebName;

        return csb.ConnectionString;
    }



    // https://github.com/mono/mono/blob/master/mcs/class/System.Json/System.Json/JsonValue.cs
    private static bool NeedEscape(string src, int i)
    {
        char c = src[i];
        return c < 32 || c == '"' || c == '\\'
            // Broken lead surrogate
            || (c >= '\uD800' && c <= '\uDBFF' &&
                (i == src.Length - 1 || src[i + 1] < '\uDC00' || src[i + 1] > '\uDFFF'))
            // Broken tail surrogate
            || (c >= '\uDC00' && c <= '\uDFFF' &&
                (i == 0 || src[i - 1] < '\uD800' || src[i - 1] > '\uDBFF'))
            // To produce valid JavaScript
            || c == '\u2028' || c == '\u2029'
            // Escape "</" for <script> tags
            || (c == '/' && i > 0 && src[i - 1] == '<');
    } // End Function NeedEscape 


    public static string EscapeString(string src)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();

        int start = 0;
        for (int i = 0; i < src.Length; i++)
            if (NeedEscape(src, i))
            {
                sb.Append(src, start, i - start);
                switch (src[i])
                {
                    case '\b': sb.Append("\\b"); break;
                    case '\f': sb.Append("\\f"); break;
                    case '\n': sb.Append("\\n"); break;
                    case '\r': sb.Append("\\r"); break;
                    case '\t': sb.Append("\\t"); break;
                    case '\"': sb.Append("\\\""); break;
                    case '\\': sb.Append("\\\\"); break;
                    case '/': sb.Append("\\/"); break;
                    default:
                        sb.Append("\\u");
                        sb.Append(((int)src[i]).ToString("x04"));
                        break;
                } // End switch (src[i]) 

                start = i + 1;
            } // End if (NeedEscape(src, i)) 

        sb.Append(src, start, src.Length - start);
        return sb.ToString();
    } // End Function EscapeString 


    static void Main(string[] args)
    {
        string ms = GetMsCon();
        string pg = GetPgCon();
        
        ms = EscapeString(ms);
        pg = EscapeString(pg);

        // DESKTOP-4P9UFE8
        System.Console.WriteLine($"MS: {ms}\r\nPG: {pg}");

        ConsoleEnvironment env = new ConsoleEnvironment();

        // https://stackoverflow.com/questions/40169673/read-appsettings-in-asp-net-core-console-application


        // Microsoft.Extensions.Configuration
        // Microsoft.Extensions.Configuration.Json
        // Set to copy to output
        Microsoft.Extensions.Configuration.IConfigurationBuilder builder = 
            new Microsoft.Extensions.Configuration.ConfigurationBuilder()
            .SetBasePath(env.ContentRootPath)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
            .AddJsonFile($"appsettings.{env.MachineName}.json", optional: true)
            .AddEnvironmentVariables()
            .AddUserSecrets()
        ;




        // myc - connection 
        // time added - dbtype
        // System.Collections.Generic.Dictionary<System.Guid, System.Data.Common.DbConnection> didi;
        // System.Collections.Generic.Dictionary<System.Guid, CoreDb.ReadDAL> didi;
        // new { DAL = CoreDb.ReadDAL, Con=null, dateTimeAdded}




        // '...Visual Studio 2017\Projects\Loggy\CoreDbTest\bin\Debug\netcoreapp1.0\appsettings.json'.'

        Config = builder.Build();

        Console.WriteLine($"option1 = {Config.GetSection("ConnectionStrings")[env.MachineName]}");

        System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();


        foreach (IConfigurationSection kvp in Config.GetSection("ConnectionStrings").GetChildren())
        {
            System.Console.WriteLine($"{kvp.Key}:\t{kvp.Value}");

            if (System.StringComparer.OrdinalIgnoreCase.Equals(kvp.Key, env.MachineName))
            {
                System.Console.WriteLine(kvp.Value);
            }

        } // Next kvp 



        foreach (IConfigurationSection kvp in Config.GetSection("ConnectionStrings").GetChildren())
        {
            System.Console.WriteLine($"{kvp.Key}:\t{kvp.Value}");


            if (System.StringComparer.OrdinalIgnoreCase.Equals(kvp.Key, env.MachineName))
            {
                System.Console.WriteLine(kvp.Value);
            }

        } // Next kvp 



        // CoreDb.DalConfig conme = new CoreDb.DalConfig("constring");

        CoreDb.DalConfig con = new CoreDb.DalConfig(
            delegate (CoreDb.DalConfig config)
            {
                string sectionName = Config.GetSection("DbProviderName").Value;
                if (sectionName == null)
                    sectionName = "ConnectionStrings";

                string cs = Config.GetSection(sectionName).GetSection(env.MachineName).Value;

                if (cs == null)
                    cs = Config.GetSection(sectionName).GetSection("DefaultConnection").Value;

                if (cs == null)
                    throw new System.IO.InvalidDataException("No connection string found");

                return cs;
            }
        );


        System.Console.WriteLine(con.ConnectionString);



        string strJSO = Newtonsoft.Json.JsonConvert.SerializeObject(Config, Newtonsoft.Json.Formatting.Indented);
        System.Console.WriteLine(strJSO);
        
        // Overwrites configuration, but one value at a time, 
        // does not replace entire section.
        string foo = Config.GetConnectionString("PG3");
        System.Console.WriteLine(foo);
        foo = Config.GetConnectionString("DefaultConnection");
        System.Console.WriteLine(foo);
        foo = Config.GetConnectionString("DefaultConnection2");
        System.Console.WriteLine(foo);


        Console.WriteLine("Hello World!");
    }


}
