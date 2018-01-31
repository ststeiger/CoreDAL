
namespace CoreDb
{


    partial class ReadDAL
    {


        private static void WriteAssociativeArray(Newtonsoft.Json.JsonTextWriter jsonWriter, System.Data.Common.DbDataReader dr)
        {
            WriteAssociativeArray(jsonWriter, dr, false);
        } // End Sub WriteAssociativeArray 


        private static void WriteAssociativeArray(Newtonsoft.Json.JsonTextWriter jsonWriter, System.Data.Common.DbDataReader dr, bool dataType)
        {
            // JSON: 
            //{
            //     "column_1":{ "index":0,"fieldType":"int"}
            //    ,"column_2":{ "index":1,"fieldType":"int"}
            //}

            jsonWriter.WriteStartObject();

            for (int i = 0; i < dr.FieldCount; ++i)
            {
                jsonWriter.WritePropertyName(dr.GetName(i));
                jsonWriter.WriteStartObject();

                jsonWriter.WritePropertyName("index");
                jsonWriter.WriteValue(i);

#if false
                jsonWriter.WritePropertyName("columnName");
                jsonWriter.WriteValue(dr.GetName(i));
#endif

                if (dataType)
                {
                    jsonWriter.WritePropertyName("fieldType");
                    jsonWriter.WriteValue(GetAssemblyQualifiedNoVersionName(dr.GetFieldType(i)));
                } // End if (dataType) 

                jsonWriter.WriteEndObject();
            } // Next i 

            jsonWriter.WriteEndObject();
        } // End Sub WriteAssociativeArray 


        private static void WriteArray(Newtonsoft.Json.JsonTextWriter jsonWriter, System.Data.Common.DbDataReader dr)
        {
            jsonWriter.WriteStartArray();

            for (int i = 0; i < dr.FieldCount; ++i)
            {
                jsonWriter.WriteStartObject();

                jsonWriter.WritePropertyName("index");
                jsonWriter.WriteValue(i);

                jsonWriter.WritePropertyName("columnName");
                jsonWriter.WriteValue(dr.GetName(i));

                jsonWriter.WritePropertyName("fieldType");
                jsonWriter.WriteValue(GetAssemblyQualifiedNoVersionName(dr.GetFieldType(i)));

                jsonWriter.WriteEndObject();
            } // Next i 

            jsonWriter.WriteEndArray();
        } // End Sub WriteArray 

        public virtual void SerializeDataTableAsAssociativeJsonArray(System.Data.Common.DbCommand cmd, System.IO.Stream output, bool pretty, System.Text.Encoding enc)
        {
            Newtonsoft.Json.JsonSerializer ser = new Newtonsoft.Json.JsonSerializer();

            using (System.IO.TextWriter sw = new System.IO.StreamWriter(output, enc))
            {

                using (Newtonsoft.Json.JsonTextWriter jsonWriter = new Newtonsoft.Json.JsonTextWriter(sw))
                {
                    if (pretty)
                        jsonWriter.Formatting = Newtonsoft.Json.Formatting.Indented;

                    // jsonWriter.WriteStartObject();

                    // jsonWriter.WritePropertyName("tables");
                    // jsonWriter.WriteStartArray();

                    using (System.Data.Common.DbConnection con = this.Connection)
                    {
                        cmd.Connection = con;

                        if (con.State != System.Data.ConnectionState.Open)
                            con.Open();

                        try
                        {

                            using (System.Data.Common.DbDataReader dr = cmd.ExecuteReader(System.Data.CommandBehavior.SequentialAccess
                                 | System.Data.CommandBehavior.CloseConnection
                                ))
                            {

                                do
                                {
                                    // jsonWriter.WriteStartObject(); // tbl = new Table();

                                    //jsonWriter.WritePropertyName("columns");

                                    //// WriteArray(jsonWriter, dr);
                                    //WriteAssociativeArray(jsonWriter, dr);
                                    

                                    //jsonWriter.WritePropertyName("rows");
                                    jsonWriter.WriteStartArray();

                                    if (dr.HasRows)
                                    {

                                        string[] columns = new string[dr.FieldCount];

                                        for (int i = 0; i < dr.FieldCount; i++)
                                        {
                                            columns[i] = dr.GetName(i);
                                        } // Next i 

                                        while (dr.Read())
                                        {
                                            object[] thisRow = new object[dr.FieldCount];

                                            // jsonWriter.WriteStartArray(); // object[] thisRow = new object[dr.FieldCount];
                                            jsonWriter.WriteStartObject(); // tbl = new Table();

                                            for (int i = 0; i < dr.FieldCount; ++i)
                                            {

                                                jsonWriter.WritePropertyName(columns[i]);

                                                object obj = dr.GetValue(i);
                                                if (obj == System.DBNull.Value) obj = null;

                                                jsonWriter.WriteValue(obj);
                                            } // Next i

                                            // jsonWriter.WriteEndArray(); // tbl.Rows.Add(thisRow);
                                            jsonWriter.WriteEndObject();
                                        } // Whend 

                                    } // End if (dr.HasRows) 

                                    jsonWriter.WriteEndArray();

                                    // jsonWriter.WriteEndObject(); // ser.Tables.Add(tbl);
                                } while (dr.NextResult());

                            } // End using dr 
                        }
                        catch (System.Exception ex)
                        {
                            System.Console.WriteLine(ex.Message);
                            throw;
                        }

                        if (con.State != System.Data.ConnectionState.Closed)
                            con.Close();
                    } // End using con 

                    // jsonWriter.WriteEndArray();

                    // jsonWriter.WriteEndObject();
                    jsonWriter.Flush();
                } // End Using jsonWriter 

            } // End Using sw 

        } // End Sub SerializeDataTableAsAssociativeJsonArray 


        public virtual void SerializeDataTableAsAssociativeJsonArray(System.Data.Common.DbCommand cmd, System.IO.Stream output, bool pretty)
        {
            SerializeDataTableAsAssociativeJsonArray(cmd, output, pretty, System.Text.Encoding.UTF8);
        } // End Sub SerializeDataTableAsAssociativeJsonArray 


        public virtual void SerializeDataTableAsAssociativeJsonArray(System.Data.Common.DbCommand cmd, System.IO.Stream output, System.Text.Encoding enc)
        {
            SerializeDataTableAsAssociativeJsonArray(cmd, output, false, enc);
        } // End Sub SerializeDataTableAsAssociativeJsonArray 


        public virtual void SerializeDataTableAsAssociativeJsonArray(System.Data.Common.DbCommand cmd, System.IO.Stream output)
        {
            SerializeDataTableAsAssociativeJsonArray(cmd, output, false, System.Text.Encoding.UTF8);
        } // End Sub SerializeDataTableAsAssociativeJsonArray 


    } // End Class ReadDAL 


} // End Namespace CoreDb 
