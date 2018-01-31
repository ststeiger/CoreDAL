
namespace CoreDb
{


    class testme
    {

        /*
         Auch in Russland gibt es Unabhängigkeitsbewegungen. 
Herr Putin, was tun Sie um das Land zusammen zu halten ? 
Ich besuche regelmässig alle grossen Städte unseres Landes
z.B. nächstes Jahr St. Petersburg, Omsk oder Warschau. 
Aber - Warschau ist doch keine russische Stadt ? 
Nächstes Jahr schon. 

Есть также независимость движения в России.
Г-н Путин, что вы делаете, чтобы держать страну вместе?
Я регулярно посещать все большие города нашей страны.
Например, в следующем году в Санкт-Петербурге, Омске или Варшаве.
Но - Варшава это не русский город!
В следующем году это будет.


Wir Russen haben vielen Ländern 1990 die Unabhängigkeit gegeben, das war ein Fehler.  




--
TableExists
ColumnExists
PrimaryKeyExists
ForeinKeyExists
UniqueConstraintExists
UniqueIndexExists
ColumnHasType
ColumnHasLength
ColumnHasPrecision

         */

        public class WriteSchema
        {
            public bool CreateTable() { return false; }
            public bool CreatePrimaryKey() { return false; }
            public bool CreateUniqueConstraint() { return false; }
            public bool CreateUniqueIndex() { return false; }
            public bool CreateIndex() { return false; }
            public bool CreateConstraint() { return false; }

            public bool EnsureColumnType() { return false; }


            public bool DropColumn() { return false; }
            public bool DropTable() { return false; }
            public bool DropPrimaryKey() { return false; }
            public bool DropForeignKey() { return false; }
            public bool DropIndex() { return false; }
            public bool DropConstraint() { return false; }

            public bool AddColumn() { return false; }
        }


        public class ColumnInformation
        {
            public string tableName;
            public string tableSchema;
            public string columnName;
            public int ordinalPosition;

            public bool nullable;
            public string data_type;

            public int? length;
            public int? precision;
            public int? scale;

            public bool signed;
            public string collation;

        }

        public class ReadSchema
        {
            public bool TableExists(string tableName) { return false; }
            public bool ColumnExists(string schema, string table, string columnName) { return false; }
public bool PrimaryKeyExists(string schema, string table) { return false; }
            public bool ForeinKeyExists(string table, string fk) { return false; }
            public bool UniqueConstraintExists(string table, string constraint) { return false; }
            public bool UniqueIndexExists(string table, string indexName) { return false; }

            public bool ColumnIsNullable(string schema, string table, string columnName, bool nullable) { return false; }
            public bool ColumnHasType(string schema, string table, string columnName, object type) { return false; }
            public bool ColumnHasLength() { return false; }
            public bool ColumnHasPrecision() { return false; }



        }


        public static void TestPG()
        {
            Npgsql.NpgsqlConnectionStringBuilder csb = new Npgsql.NpgsqlConnectionStringBuilder();
            csb.Host = "127.0.0.1";
            csb.Database = "somedb";
            csb.Port = 5432;

            csb.IntegratedSecurity = false;

            if (!csb.IntegratedSecurity)
            {
                csb.Username = "postgres";
                csb.Password = "TopSecret";
            }

            csb.SslMode = Npgsql.SslMode.Prefer;
            csb.Pooling = true;
            csb.MinPoolSize = 1;
            csb.MaxPoolSize = 5;

            csb.PersistSecurityInfo = false;
            csb.ApplicationName = "MyApplication";
            csb.Timeout = 30;
            csb.CommandTimeout = 30;
            csb.Encoding = System.Text.Encoding.UTF8.WebName;
            
            using (Npgsql.NpgsqlConnection conn = new Npgsql.NpgsqlConnection(csb.ConnectionString))
            {
                string foo = conn.ServerVersion;
            }


        }


        public static void TestMySQL()
        {
            MySql.Data.MySqlClient.MySqlConnectionStringBuilder csb = new MySql.Data.MySqlClient.MySqlConnectionStringBuilder();

            csb.Server = "";
            csb.Port = 123;
            csb.Database = "";

            csb.UserID = "";
            csb.Password = "";

            csb.PersistSecurityInfo = false;
            csb.Pooling = true;
            csb.MinimumPoolSize = 1;
            csb.MaximumPoolSize = 5;
            csb.CharacterSet = System.Text.Encoding.UTF8.WebName;
            csb.ConnectionTimeout = 30;
            csb.ConnectionReset = true;
            csb.ConvertZeroDateTime = false;
            csb.SslMode = MySql.Data.MySqlClient.MySqlSslMode.Preferred;
            csb.TreatTinyAsBoolean = false;
            


            using(MySql.Data.MySqlClient.MySqlConnection con = new MySql.Data.MySqlClient.MySqlConnection(csb.ConnectionString))
            {
                string ser = con.ServerVersion;
            }
            

            System.Console.WriteLine(csb.ConnectionString);

        }


    }


}
