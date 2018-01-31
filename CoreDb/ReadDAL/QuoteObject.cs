
namespace CoreDb
{


    partial class ReadDAL
    {


        public virtual string QuoteObject(string objectName)
        {
            if (string.IsNullOrEmpty(objectName))
                throw new System.ArgumentNullException("objectName");

            return "\"" + objectName.Replace("\"", "\"\"") + "\"";
        }


        protected System.Collections.Generic.HashSet<string> m_reservedWords;


        protected virtual System.Collections.Generic.HashSet<string> PopulateReservedWords()
        {
            System.Collections.Generic.HashSet<string> reservedWords = 
                new System.Collections.Generic.HashSet<string>(System.StringComparer.InvariantCultureIgnoreCase);

            string[] kw = new string[] { 
                  "SELECT", "INSERT", "CREATE", "ALTER", "UPDATE", "DELETE", "TRUNCATE", "DROP", "DUMP"
                , "EXECUTE", "EXEC", "CONNECT", "DISCONNECT", "BACKUP",  "RESTORE", "INTO", "MERGE"
                , "PLAN", "SHOWPLAN"
                , "NULLIF", "COALESCE", "SUBSTRING", "REPLACE", "CAST", "FLOOR", "CEILING"
                , "CURRENT_TIMESTAMP", "CURRENT_DATE", "LOCALTIME", "LOCALTIMESTAMP"
                , "NULL", "TRUE", "FALSE"
                , "MIN", "MAX", "STDDEV_POP","STDDEV_SAMP"
                , "COUNT", "DISTINCT", "TOP", "PERCENT", "FIRST", "ADD", "UNION", "ALL", "EXCEPT"
                , "DECLARE", "WITH", "WITHOUT", "RECURSIVE"
                , "REFRENCES", "REFERENCING", "DEFAULT", "CONSTRAINT", "UNIQUE","CHECK", "CASCADE", "CASCADED"
                , "PRIMARY", "FOREIGN", "KEY", "CLUSTERED"
                , "TRANSACTION", "TRAN", "COMMIT", "ROLLBACK", "REVERT"
                , "*", "IS", "ON", "AND", "OR", "NOT", "IN", "ANY", "LIKE", "ESCAPE", "EXISTS", "BETWEEN"
                , "CURSOR", "OPEN", "FETCH", "FETCH_STATUS", "NEXT", "FROM", "TO", "AS", "ALIAS", "CLOSE", "ALLOCATE", "DEALLOCATE"
                , "LEFT", "RIGHT", "INNER", "OUTER", "NATURAL", "FULL", "CROSS", "JOIN", "APPLY", "LATERAL"
                , "DATASPACE", "DATABASE","SCHEMA" ,"TABLE", "VIEW", "PROCEDURE", "PROC", "FUNCTION", "FUNC", "METHOD"
                , "TRIGGER", "INSTEAD", "AGGREGATE", "SYNONYM", "DOMAIN", "COLLATION"
                , "ASSEMBLY", "OBJECT"
                , "WHER", "GROUP", "HAVING"
                , "ROW_NUMBER", "RANK", "DENSE_RANK", "PERCENT_RANK", "PERCENTILE_CONT", "PERCENTILE_DISC"
                , "COVAR_POP", "COVAR_SAMP"
                , "OVER", "ORDER", "PARTITION", "BY", "ASC", "DESC", "OFFSET", "OFFSETS", "ROWS", "ONLY", "COLLATE"

                , "IF", "THEN", "ELSE", "CASE", "WHEN", "WHILE", "FOR", "EACH", "BEGIN", "END"
                , "RETURN", "RETURNS", "GOTO", "EXIT", "BREAK", "CONTINUE"
                , "USER", "ROLE", "LOGIN", "PRINCIPAL", "STATE", "DEFINITION"
                , "YEAR", "MONTH", "DAY", "HOUR", "MINUTE", "SECOND"
                , "EQUALS", "EVERY", "EXCEPTION", "FILTER", "EXTERNAL", "CARDINALITY", "CHECKPOINT", "CLASS"
                , "GRANT", "REVOKE","DENY", "READ", "WRITE", "READ_ONLY", "FORWARD_ONLY"
                , "LOCAL", "GLOBAL", "STATIC", "DYNAMIC"
                , "CONTROL", "SET", "NOCOUNT", "ROWCOUNT", "ON", "OFF",  "GET", "USE", "OPTION", "AUTHORIZATION"

                , "SESSION_USER", "SYSTEM_USER", "CURRENT_USER", "LANGUAGE"
                , "TRAILING", "TEMPORARY", "SYMMETRIC", "SYMMETRIC"
                , "ACTION", "ABSOLUTE", "ADMIN", "AFTER", "ARE", "ARRAY", "ATOMIC", "BEFORE"
                , "MEMBER", "IMMEDIATE", "INOUT", "OUT", "INPUT", "OUTPUT", "ZONE"
                , "SPACE", "SPECIFIC", "SPECIFICTYPE", "SOME", "SIZE", "SIMILAR", "CYCLE",  "CURRENT_PATH" 
                , "CURRENT_ROLE", "DESTROY", "DESTRUCTOR", "DIAGNOSTICS", "DICTIONARY", "DETERMINISTIC", "ELEMENT"
                , "WHENEVER", "THAN", "RANGE", "INTERSECT", "INTERSECTION", "INTERVAL", "SEARCH", "FOUND", "SCOPE", "ROLLUP"
                , "VALUE", "VALUES", "VARCHAR", "NATIONAL", "VARYING", "CHARACTER"
                , "MOD", "MULTISET", "SUBMULTISET", "STATISTICS", "STRUCTURE", "SYSTEM", "CORR"
                , "CHARACTER_LENGTH","CHAR_LENGTH", "BIT_LENGTH", "OCTET_LENGTH"
                , "NUMERIC", "FLOAT", "REAL", "DEC", "DECIMAL", "INT", "INTEGER", "SECTION", "RULE", "FREE", "NEW"
                , "GENERAL", "NORMALIZE", "PREORDER", "PRIVILEGES", "RELEASE"
                , "GROUPING", "HOST", "HOLD", "FORTRAN", "CALLED", "BREADTH", "DATA", "MATCH"
            };


            reservedWords.UnionWith(kw);

            return reservedWords;
        }


        public virtual bool IsReservedKeyword(string objectName)
        {
            return this.m_reservedWords.Contains(objectName);
        }


        public virtual bool ObjectNameNeedsEscaping(string objectName)
        {
            if (string.IsNullOrEmpty(objectName))
                return false;

            if (this.IsReservedKeyword(objectName))
                return true;

            if (objectName.StartsWith("@"))
                return true;

            // The underscore (_), at sign (@), or number sign (#).
            // %^&({}+-/ ]['''
            char[] lsIllegalCharacters = "+-*/%<>=&|^(){}[]\"'´`\t\n\r \\,.;?!~¨¦§°¢£€".ToCharArray();

            for (int i = 0; i < lsIllegalCharacters.Length; ++i)
            {
                if (objectName.IndexOf(lsIllegalCharacters[i]) != -1)
                    return true;
            }

            return false;
        } // End Function MustEscape 


        public virtual string QuoteObjectIfNecessary(string objectName)
        {
            if (string.IsNullOrEmpty(objectName))
                throw new System.ArgumentNullException("ObjectName");

            if (ObjectNameNeedsEscaping(objectName))
                return QuoteObject(objectName);

            return objectName;
        } // End Function QuoteObjectIfNecessary


    } // End Class ReadDAL 


} // End Namespace 
