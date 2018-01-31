
namespace CoreDb
{


    partial class ReadDAL
    {


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


        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public virtual T GetParameterValue<T>(System.Data.IDbCommand idbc, string strParameterName)
        {
            if (!strParameterName.StartsWith("@"))
            {
                strParameterName = "@" + strParameterName;
            }

            return (T)(((System.Data.IDbDataParameter)idbc.Parameters[strParameterName]).Value);
        } // End Function GetParameterValue<T>




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


            if (command.Parameters.Contains(strParameterName))
            {
                command.Parameters.RemoveAt(strParameterName);
            }

            SetParameter(parameter, objValue);

            command.Parameters.Add(parameter);
            return parameter;
        } // End Function AddParameter


    }


}
