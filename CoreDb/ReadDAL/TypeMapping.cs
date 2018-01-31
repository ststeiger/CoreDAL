
namespace CoreDb
{


    partial class ReadDAL
    {

        // From Type to DBType
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        protected virtual System.Data.DbType GetDbType(System.Type type)
        {
            // http://social.msdn.microsoft.com/Forums/en/winforms/thread/c6f3ab91-2198-402a-9a18-66ce442333a6
            string strTypeName = type.Name;
            System.Data.DbType dbType = System.Data.DbType.String; // default value

            try
            {
                if (object.ReferenceEquals(type, typeof(System.DBNull)))
                {
                    return dbType;
                }

                if (object.ReferenceEquals(type, typeof(System.Byte[])))
                {
                    return System.Data.DbType.Binary;
                }

                dbType = (System.Data.DbType)System.Enum.Parse(typeof(System.Data.DbType), strTypeName, true);

                // Es ist keine Zuordnung von DbType UInt64 zu einem bekannten SqlDbType vorhanden.
                // http://msdn.microsoft.com/en-us/library/bbw6zyha(v=vs.71).aspx
                if (dbType == System.Data.DbType.UInt64)
                    dbType = System.Data.DbType.Int64;
            }
            catch (System.Exception)
            {
                // add error handling to suit your taste
            }

            return dbType;
        } // End Function GetDbType



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


    }
}
