
namespace CoreDb
{


    partial class zzzReadDAL
    {

        private System.Data.Common.DbProviderFactory m_factory;

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


        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public virtual T GetParameterValue<T>(System.Data.IDbCommand idbc, string strParameterName)
        {
            if (!strParameterName.StartsWith("@"))
            {
                strParameterName = "@" + strParameterName;
            }

            return (T)(((System.Data.IDbDataParameter)idbc.Parameters[strParameterName]).Value);
        } // End Function GetParameterValue<T>


        public virtual string ConnectionString
        {
            get
            {
                return "";
            }
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

        public virtual T ExecuteScalar<T>(System.Data.IDbCommand cmd)
        {
            return ExecuteScalar<T>(cmd, true);
        }


        public virtual T ExecuteScalar<T>(System.Data.IDbCommand cmd, bool throwOnAssignNullToNonNullableType)
        {
            object objReturnValue = null;
            string strReturnValue = null;
            System.Type tReturnType = typeof(T);

            System.Reflection.TypeInfo ti = System.Reflection.IntrospectionExtensions.GetTypeInfo(tReturnType);
            bool typeIsNullable = (ti.IsGenericType && object.ReferenceEquals(tReturnType.GetGenericTypeDefinition(), typeof(System.Nullable<>)));
            bool typeCanBeAssignedNull = !ti.IsValueType || typeIsNullable;

            if (typeIsNullable)
            {
                tReturnType = System.Nullable.GetUnderlyingType(tReturnType);
            } // End if (typeIsNullable) 


            using (System.Data.IDbConnection idbc = this.Connection)
            {
                cmd.Connection = idbc;



                try
                {
                    if (cmd.Connection.State != System.Data.ConnectionState.Open)
                        cmd.Connection.Open();

                    objReturnValue = cmd.ExecuteScalar();
                } // End Try
                catch (System.Data.Common.DbException ex)
                {
                    if (Log("cDAL.ExecuteScalar<T>(string strSQL)", ex, cmd))
                        throw;
                } // End Catch
                finally
                {
                    if (cmd.Connection.State != System.Data.ConnectionState.Closed)
                        cmd.Connection.Close();
                } // End Finally


            } // End using idbc


            // Catch & Normalize NULL 
            if (objReturnValue == null || objReturnValue == System.DBNull.Value)
            {
                if (typeCanBeAssignedNull)
                    return (T)(object)null;

                if (throwOnAssignNullToNonNullableType)
                    throw new System.IO.InvalidDataException("ExecuteScalar cannot assign NULL to non-nullable type.");

                return default(T);
            } // End if (objReturnValue == null) 


            System.Type tDataTypeOfScalarResult = objReturnValue.GetType();
            if (object.ReferenceEquals(tReturnType, tDataTypeOfScalarResult))
                return (T)objReturnValue;


            strReturnValue = System.Convert.ToString(objReturnValue, System.Globalization.CultureInfo.InvariantCulture);
            objReturnValue = null;

            try
            {
                if (object.ReferenceEquals(tReturnType, typeof(object)))
                {
                    return (T)objReturnValue;
                }
                else if (object.ReferenceEquals(tReturnType, typeof(string)))
                {
                    return (T)(object)strReturnValue;
                } // End if string
                else if (object.ReferenceEquals(tReturnType, typeof(bool)))
                {
                    bool bReturnValue = false;

                    if (bool.TryParse(strReturnValue, out bReturnValue))
                        return (T)(object)bReturnValue;

                    if (strReturnValue == "0")
                        return (T)(object)false;

                    if (strReturnValue == "0.0")
                        return (T)(object)false;

                    return (T)(object)true;
                } // End if bool
                else if (object.ReferenceEquals(tReturnType, typeof(int)))
                {
                    int iReturnValue;
                    if (int.TryParse(strReturnValue, out iReturnValue))
                        return (T)(object)iReturnValue;

                    throw new System.IO.InvalidDataException("ExecuteScalar: Returned data \"" + strReturnValue + "\" is not a valid integer.");
                } // End if int
                else if (object.ReferenceEquals(tReturnType, typeof(uint)))
                {
                    uint uiReturnValue;
                    if (uint.TryParse(strReturnValue, out uiReturnValue))
                        return (T)(object)uiReturnValue;

                    throw new System.IO.InvalidDataException("ExecuteScalar: Returned data \"" + strReturnValue + "\" is not a valid unsigned integer.");
                } // End if uint
                else if (object.ReferenceEquals(tReturnType, typeof(long)))
                {
                    long lngReturnValue;
                    if (long.TryParse(strReturnValue, out lngReturnValue))
                        return (T)(object)lngReturnValue;

                    throw new System.IO.InvalidDataException("ExecuteScalar: Returned data \"" + strReturnValue + "\" is not a valid unsigned integer.");
                } // End if long
                else if (object.ReferenceEquals(tReturnType, typeof(ulong)))
                {
                    ulong ulngReturnValue;
                    if (ulong.TryParse(strReturnValue, out ulngReturnValue))
                        return (T)(object)ulngReturnValue;

                    throw new System.IO.InvalidDataException("ExecuteScalar: Returned data \"" + strReturnValue + "\" is not a valid unsigned long.");
                } // End if ulong
                else if (object.ReferenceEquals(tReturnType, typeof(float)))
                {
                    float fltReturnValue;
                    if (float.TryParse(strReturnValue, out fltReturnValue))
                        return (T)(object)fltReturnValue;

                    throw new System.IO.InvalidDataException("ExecuteScalar: Returned data \"" + strReturnValue + "\" is not a valid float.");
                }
                else if (object.ReferenceEquals(tReturnType, typeof(double)))
                {
                    double dblReturnValue;
                    if (double.TryParse(strReturnValue, out dblReturnValue))
                        return (T)(object)dblReturnValue;

                    throw new System.IO.InvalidDataException("ExecuteScalar: Returned data \"" + strReturnValue + "\" is not a valid double.");
                }
                else if (object.ReferenceEquals(tReturnType, typeof(System.Net.IPAddress)))
                {
                    System.Net.IPAddress ipAddress = null;

                    if (System.Net.IPAddress.TryParse(strReturnValue, out ipAddress))
                        return (T)(object)ipAddress;

                    throw new System.IO.InvalidDataException("ExecuteScalar: Returned data \"" + strReturnValue + "\" is not a valid IP address.");
                } // End if IPAddress
                else if (object.ReferenceEquals(tReturnType, typeof(System.Guid)))
                {
                    System.Guid retUID;
                    if (System.Guid.TryParse(strReturnValue, out retUID))
                        return (T)(object)retUID;

                    throw new System.IO.InvalidDataException("ExecuteScalar: Returned data \"" + strReturnValue + "\" is not a valid GUID.");
                } // End if System.Guid
                else // No datatype matches
                {
                    throw new System.NotImplementedException("ExecuteScalar<T>: No implicit conversion operation defined for type \"" + tReturnType.FullName + "\".");
                } // End else of if tReturnType = datatype

            } // End Try
            catch (System.Exception ex)
            {
                if (Log("cDAL.cs ==> ExecuteScalar<T>(string strSQL)", ex, cmd))
                    throw;
            } // End Catch

            return (T)(object)null;
        } // End Function ExecuteScalar(cmd)


        public virtual T ExecuteScalar<T>(string strSQL)
        {
            T tReturnValue = default(T);

            // pfff, mono C# compiler problem...
            //sqlCMD = new System.Data.SqlClient.SqlCommand(strSQL, m_SqlConnection);
            using (System.Data.IDbCommand cmd = CreateCommand(strSQL))
            {
                tReturnValue = ExecuteScalar<T>(cmd);
            } // End Using cmd

            return tReturnValue;
        } // End Function ExecuteScalar(strSQL)







        private bool m_Rethrow = true;


        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public virtual bool Log(System.Exception ex
            , [System.Runtime.CompilerServices.CallerMemberName] string memberName = ""
            , [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = ""
            , [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0
            )
        {
            return Log(ex, null, memberName, sourceFilePath, sourceLineNumber);
        }


        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public virtual bool Log(System.Exception ex
            , System.Data.IDbCommand cmd
            , [System.Runtime.CompilerServices.CallerMemberName] string memberName = ""
            , [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = ""
            , [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0
            )
        {

            // https://stackoverflow.com/questions/41112381/get-the-name-of-the-currently-executing-method-in-dotnet-core
            // https://stackoverflow.com/questions/1678816/dumping-the-call-stack-programatically

            // string stTrace = System.Environment.StackTrace;
            // https://stackoverflow.com/questions/171970/how-can-i-find-the-method-that-called-the-current-method
            // https://stackoverflow.com/questions/615940/retrieving-the-calling-method-name-from-within-a-method
            // https://stackoverflow.com/questions/41112381/get-the-name-of-the-currently-executing-method-in-dotnet-core

            // new System.Diagnostics.StackTrace
            return Log("somewhere", ex, cmd, memberName, sourceFilePath, sourceLineNumber);
        }


        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public virtual bool Log(string where
            , System.Exception ex
            , System.Data.IDbCommand cmd
            , [System.Runtime.CompilerServices.CallerMemberName] string memberName = ""
            , [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = ""
            , [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0
            )
        {
            return this.m_Rethrow;
        }


        public System.Data.Common.DbCommand Command
        {
            get
            {
                return m_factory.CreateCommand();
            }
        }


        public virtual System.Data.Common.DbCommand CreateCommand()
        {
            return this.Command;
        }


        public virtual System.Data.Common.DbCommand CreateCommand(string commandText)
        {
            System.Data.Common.DbCommand cmd = this.Command;
            cmd.CommandText = commandText;

            return cmd;
        }


        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static string JoinArray<T>(string separator, T[] inputTypeArray)
        {
            return JoinArray<T>(separator, inputTypeArray, object.ReferenceEquals(typeof(T), typeof(string)));
        }



        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static string JoinArray<T>(string separator, T[] inputTypeArray, bool sqlEscapeString)
        {
            string strRetValue = null;
            System.Collections.Generic.List<string> ls = new System.Collections.Generic.List<string>();

            for (int i = 0; i < inputTypeArray.Length; ++i)
            {
                string str = System.Convert.ToString(inputTypeArray[i], System.Globalization.CultureInfo.InvariantCulture);

                if (!string.IsNullOrEmpty(str))
                {
                    // SQL-Escape
                    if (sqlEscapeString)
                        str = str.Replace("'", "''");

                    ls.Add(str);
                } // End if (!string.IsNullOrEmpty(str))

            } // Next i 

            strRetValue = string.Join(separator, ls.ToArray());
            ls.Clear();
            ls = null;

            return strRetValue;
        } // End Function JoinArray


        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public virtual void AddArrayParameter<T>(System.Data.IDbCommand command, string strParameterName, params T[] values)
        {
            if (values == null)
                return;

            if (!strParameterName.StartsWith("@"))
                strParameterName = "@" + strParameterName;

            string strSqlInStatement = JoinArray<T>(",", values);

            command.CommandText = command.CommandText.Replace(strParameterName, strSqlInStatement);
        } // End Function AddArrayParameter


        // From Type to DBType
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        protected virtual System.Data.DbType GetDbType(System.Type type)
        {
            // http://social.msdn.microsoft.com/Forums/en/winforms/thread/c6f3ab91-2198-402a-9a18-66ce442333a6
            string strTypeName = type.Name;
            System.Data.DbType DBtype = System.Data.DbType.String; // default value

            try
            {
                if (object.ReferenceEquals(type, typeof(System.DBNull)))
                {
                    return DBtype;
                }

                if (object.ReferenceEquals(type, typeof(System.Byte[])))
                {
                    return System.Data.DbType.Binary;
                }

                DBtype = (System.Data.DbType)System.Enum.Parse(typeof(System.Data.DbType), strTypeName, true);

                // Es ist keine Zuordnung von DbType UInt64 zu einem bekannten SqlDbType vorhanden.
                // http://msdn.microsoft.com/en-us/library/bbw6zyha(v=vs.71).aspx
                if (DBtype == System.Data.DbType.UInt64)
                    DBtype = System.Data.DbType.Int64;
            }
            catch (System.Exception)
            {
                // add error handling to suit your taste
            }

            return DBtype;
        } // End Function GetDbType
        

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public virtual System.Data.IDbDataParameter AddParameter(System.Data.IDbCommand command, string strParameterName, object objValue)
        {
            return AddParameter(command, strParameterName, objValue, System.Data.ParameterDirection.Input);
        } // End Function AddParameter


        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public virtual System.Data.IDbDataParameter AddParameter(System.Data.IDbCommand command, string strParameterName, object objValue, System.Data.ParameterDirection pad)
        {
            if (objValue == null)
            {
                //throw new ArgumentNullException("objValue");
                objValue = System.DBNull.Value;
            } // End if (objValue == null)

            System.Type tDataType = objValue.GetType();
            System.Data.DbType dbType = GetDbType(tDataType);

            return AddParameter(command, strParameterName, objValue, pad, dbType);
        } // End Function AddParameter


        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public virtual System.Data.IDbDataParameter AddParameter(System.Data.IDbCommand command, string strParameterName, object objValue, System.Data.ParameterDirection pad, System.Data.DbType dbType)
        {
            System.Data.IDbDataParameter parameter = command.CreateParameter();

            if (!strParameterName.StartsWith("@"))
            {
                strParameterName = "@" + strParameterName;
            } // End if (!strParameterName.StartsWith("@"))

            parameter.ParameterName = strParameterName;
            parameter.DbType = dbType;
            parameter.Direction = pad;

            // Es ist keine Zuordnung von DbType UInt64 zu einem bekannten SqlDbType vorhanden.
            // No association  DbType UInt64 to a known SqlDbType
            SetParameter(parameter, objValue);

            command.Parameters.Add(parameter);
            return parameter;
        } // End Function AddParameter

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void SetParameter(object param, object objValue)
        {
            SetParameter((System.Data.IDbDataParameter)param, objValue);
        }


        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void SetParameter(System.Data.IDbDataParameter param, object objValue)
        {
            if (objValue == null)
                param.Value = System.DBNull.Value;
            else
                param.Value = objValue;
        }


        public virtual System.Data.Common.DbDataReader ExecuteReader(System.Data.IDbCommand cmd, System.Data.CommandBehavior cmdBehaviour)
        {
            System.Data.Common.DbDataReader idr = null;

            try
            {
                System.Data.IDbConnection idbc = this.Connection;
                cmd.Connection = idbc;

                if (cmd.Connection.State != System.Data.ConnectionState.Open)
                    cmd.Connection.Open();

                // this line is Evil ;)
                idr = ((System.Data.Common.DbCommand)cmd).ExecuteReader(cmdBehaviour);
            }
            catch (System.Exception ex)
            {
                if (Log(ex, cmd))
                    throw;
            }

            return idr;
        } // End Function ExecuteReader


        public virtual System.Data.Common.DbDataReader ExecuteReader(System.Data.IDbCommand cmd)
        {
            return ExecuteReader(cmd, System.Data.CommandBehavior.CloseConnection);
        } // End Function ExecuteReader


        public virtual System.Data.Common.DbDataReader ExecuteReader(string strSQL)
        {
            return ExecuteReader(strSQL, System.Data.CommandBehavior.CloseConnection);
        } // End Function ExecuteReader


        public virtual System.Data.Common.DbDataReader ExecuteReader(string strSQL, System.Data.CommandBehavior cmdBehaviour)
        {
            System.Data.Common.DbDataReader idr = null;

            using (System.Data.Common.DbCommand cmd = this.CreateCommand(strSQL))
            {
                idr = ExecuteReader(cmd, cmdBehaviour);
            } // End Using cmd

            return idr;
        } // End Function ExecuteReader


        public virtual object ExecuteStoredProcedure(System.Data.IDbCommand cmd)
        {
            object objReturnValue = null;
            try
            {

                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                //// System.Data.SqlClient.SqlParameter returnValue = new System.Data.SqlClient.SqlParameter();
                System.Data.IDbDataParameter returnValue = cmd.CreateParameter();
                returnValue.Direction = System.Data.ParameterDirection.ReturnValue;

                //this.AddParameter(cmd, "name", null, System.Data.ParameterDirection.ReturnValue);
                cmd.Parameters.Add(returnValue);

                using (System.Data.IDbConnection con = this.Connection)
                {

                    cmd.Connection = con;

                    try
                    {
                        if (cmd.Connection.State != System.Data.ConnectionState.Open)
                            cmd.Connection.Open();

                        cmd.ExecuteNonQuery();

                        //Assert.AreEqual(123, (int)returnValue.Value);
                        //System.Diagnostics.Debug.Assert(123 == (int)((System.Data.IDataParameter)cmd.Parameters["name"]).Value);
                        //System.Diagnostics.Debug.Assert(123 == (int)returnValue.Value);
                        objReturnValue = returnValue.Value;
                    }
                    catch (System.Exception ex)
                    {
                        if (Log("cDAL.ExecuteStoredProcedure - inner", ex, cmd))
                            throw;
                    }
                    finally
                    {
                        if (cmd.Connection != null && cmd.Connection.State != System.Data.ConnectionState.Closed)
                            cmd.Connection.Close();
                    } // End Finally

                } // End using con

            }
            catch (System.Exception ex)
            {
                if (Log("cDAL.ExecuteStoredProcedure - outer", ex, cmd))
                    throw;
            } // End catch

            return objReturnValue;
        } // End Function ExecuteStoredProcedure


        public virtual object ExecuteStoredProcedure(string strProcedureName)
        {
            object objReturnValue = null;

            //// using (SqlCommand cmd = new SqlCommand("TestProc", cn))
            using (System.Data.Common.DbCommand cmd = this.CreateCommand(strProcedureName))
            {
                objReturnValue = ExecuteStoredProcedure(cmd);
            } // End Using cmd

            return objReturnValue;
        } // End Function ExecuteStoredProcedure


        public virtual System.Data.Common.DbDataReader ExecuteReaderFromStoredProcedure(string strStoredProcedureName)
        {
            using (System.Data.Common.DbCommand cmd = this.CreateCommand(strStoredProcedureName))
            {
                return this.ExecuteReaderFromStoredProcedure(cmd);
            } // End Using cmd

        } // End Function ExecuteReaderFromStoredProcedure


        public virtual System.Data.Common.DbDataReader ExecuteReaderFromStoredProcedure(System.Data.IDbCommand cmd)
        {
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            return this.ExecuteReader(cmd);
        } // End Function ExecuteReaderFromStoredProcedure


        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        protected virtual string SqlTypeFromDbType(System.Data.DbType type)
        {
            string strRetVal = null;

            // http://msdn.microsoft.com/en-us/library/cc716729.aspx
            switch (type)
            {
                case System.Data.DbType.Guid:
                    strRetVal = "uniqueidentifier";
                    break;
                case System.Data.DbType.Date:
                    strRetVal = "date";
                    break;
                case System.Data.DbType.Time:
                    strRetVal = "time(7)";
                    break;
                case System.Data.DbType.DateTime:
                    strRetVal = "datetime";
                    break;
                case System.Data.DbType.DateTime2:
                    strRetVal = "datetime2";
                    break;
                case System.Data.DbType.DateTimeOffset:
                    strRetVal = "datetimeoffset(7)";
                    break;

                case System.Data.DbType.StringFixedLength:
                    strRetVal = "nchar(MAX)";
                    break;
                case System.Data.DbType.String:
                    strRetVal = "nvarchar(MAX)";
                    break;

                case System.Data.DbType.AnsiStringFixedLength:
                    strRetVal = "char(MAX)";
                    break;
                case System.Data.DbType.AnsiString:
                    strRetVal = "varchar(MAX)";
                    break;

                case System.Data.DbType.Single:
                    strRetVal = "real";
                    break;
                case System.Data.DbType.Double:
                    strRetVal = "float";
                    break;
                case System.Data.DbType.Decimal:
                    strRetVal = "decimal(19, 5)";
                    break;
                case System.Data.DbType.VarNumeric:
                    strRetVal = "numeric(19, 5)";
                    break;

                case System.Data.DbType.Boolean:
                    strRetVal = "bit";
                    break;
                case System.Data.DbType.SByte:
                case System.Data.DbType.Byte:
                    strRetVal = "tinyint";
                    break;
                case System.Data.DbType.Int16:
                    strRetVal = "smallint";
                    break;
                case System.Data.DbType.Int32:
                    strRetVal = "int";
                    break;
                case System.Data.DbType.Int64:
                    strRetVal = "bigint";
                    break;
                case System.Data.DbType.Xml:
                    strRetVal = "xml";
                    break;
                case System.Data.DbType.Binary:
                    strRetVal = "varbinary(MAX)"; // or image
                    break;
                case System.Data.DbType.Currency:
                    strRetVal = "money";
                    break;
                case System.Data.DbType.Object:
                    strRetVal = "sql_variant";
                    break;

                case System.Data.DbType.UInt16:
                case System.Data.DbType.UInt32:
                case System.Data.DbType.UInt64:
                    throw new System.NotImplementedException("Uints not mapped - MySQL only");
            }

            return strRetVal;
        } // End Function SqlTypeFromDbType


        // http://www.sqlusa.com/bestpractices/datetimeconversion/
        const string DATEFORMAT = "{0:yyyyMMdd}"; // YYYYMMDD ISO date format works at any language setting - international standard
        const string DATETIMEFORMAT = "{0:yyyy'-'MM'-'dd'T'HH:mm:ss.fff}"; // ISO 8601 format: international standard - works with any language setting


        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public virtual string GetParametrizedQueryText(System.Data.IDbCommand cmd)
        {
            string strReturnValue = "";

            try
            {
                System.Text.StringBuilder msg = new System.Text.StringBuilder();
                System.DateTime dtLogTime = System.DateTime.UtcNow;


                if (cmd == null || string.IsNullOrEmpty(cmd.CommandText))
                {
                    return strReturnValue;
                } // End if (cmd == null || string.IsNullOrEmpty(cmd.CommandText))


                if (cmd.Parameters != null && cmd.Parameters.Count > 0)
                {
                    msg.AppendLine("-- ***** Listing Parameters *****");

                    foreach (System.Data.IDataParameter idpThisParameter in cmd.Parameters)
                    {
                        // http://msdn.microsoft.com/en-us/library/cc716729.aspx
                        msg.AppendLine(string.Format("DECLARE {0} AS {1} -- DbType: {2}", idpThisParameter.ParameterName, SqlTypeFromDbType(idpThisParameter.DbType), idpThisParameter.DbType.ToString()));
                    } // Next idpThisParameter

                    msg.AppendLine(System.Environment.NewLine);
                    msg.AppendLine(System.Environment.NewLine);

                    foreach (System.Data.IDataParameter idpThisParameter in cmd.Parameters)
                    {
                        string strParameterValue = null;
                        if (object.ReferenceEquals(idpThisParameter.Value, System.DBNull.Value))
                        {
                            strParameterValue = "NULL";
                        }
                        else
                        {
                            if (idpThisParameter.DbType == System.Data.DbType.Date)
                                strParameterValue = System.String.Format(DATEFORMAT, idpThisParameter.Value);
                            else if (idpThisParameter.DbType == System.Data.DbType.DateTime || idpThisParameter.DbType == System.Data.DbType.DateTime2)
                                strParameterValue = System.String.Format(DATETIMEFORMAT, idpThisParameter.Value);
                            else
                                strParameterValue = idpThisParameter.Value.ToString();

                            strParameterValue = "'" + strParameterValue.Replace("'", "''") + "'";
                        }

                        msg.AppendLine(string.Format("SET {0} = {1}", idpThisParameter.ParameterName, strParameterValue));
                    } // Next idpThisParameter

                    msg.AppendLine("-- ***** End Parameter Listing *****");
                    msg.AppendLine(System.Environment.NewLine);
                } // End if cmd.Parameters != null || cmd.Parameters.Count > 0 



                msg.AppendLine(string.Format("-- ***** Start Query from {0} *****", dtLogTime.ToString(DATEFORMAT)));
                msg.AppendLine(cmd.CommandText);
                msg.AppendLine(string.Format("-- ***** End Query from {0} *****", dtLogTime.ToString(DATEFORMAT)));
                msg.AppendLine(System.Environment.NewLine);

                strReturnValue = msg.ToString();
                msg = null;
            } // End Try
            catch (System.Exception ex)
            {
                strReturnValue = "Error in Function cDAL.GetParametrizedQueryText";
                strReturnValue += System.Environment.NewLine;
                strReturnValue += ex.Message;
            } // End Catch

            return strReturnValue;
        } // End Function GetParametrizedQueryText


    }


}
