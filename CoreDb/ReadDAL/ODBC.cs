
namespace CoreDb
{


    partial class ReadDAL
    {


        // MsgBox(String.Join(Environment.NewLine, GetArguments("bla")));
        protected virtual string[] GetArguments(string strAllArguments)
        {
            string EscapeCharacter = System.Convert.ToChar(8).ToString();

            strAllArguments = strAllArguments.Replace("''", EscapeCharacter);

            bool bInString = false;
            int iLastSplitAt = 0;

            System.Collections.Generic.List<string> lsArguments = new System.Collections.Generic.List<string>();


            int iInFunction = 0;

            for (int i = 0; i < strAllArguments.Length; i++)
            {
                char strCurrentChar = strAllArguments[i];

                if (strCurrentChar == '\'')
                    bInString = !bInString;


                if (bInString)
                    continue;


                if (strCurrentChar == '(')
                    iInFunction += 1;


                if (strCurrentChar == ')')
                    iInFunction -= 1;



                if (strCurrentChar == ',')
                {

                    if (iInFunction == 0)
                    {
                        string strExtract = "";
                        if (iLastSplitAt != 0)
                        {
                            strExtract = strAllArguments.Substring(iLastSplitAt + 1, i - iLastSplitAt - 1);
                        }
                        else
                        {
                            strExtract = strAllArguments.Substring(iLastSplitAt, i - iLastSplitAt);
                        }

                        strExtract = strExtract.Replace(EscapeCharacter, "''");
                        lsArguments.Add(strExtract);
                        iLastSplitAt = i;
                    } // End if (iInFunction == 0)

                } // End if (strCurrentChar == ',')

            } // Next i


            string strExtractLast = "";
            if (lsArguments.Count > 0)
            {
                strExtractLast = strAllArguments.Substring(iLastSplitAt + 1);
            }
            else
            {
                strExtractLast = strAllArguments.Substring(iLastSplitAt);
            }

            strExtractLast = strExtractLast.Replace(EscapeCharacter, "''");
            lsArguments.Add(strExtractLast);

            string[] astrResult = lsArguments.ToArray();
            lsArguments.Clear();
            lsArguments = null;

            return astrResult;
        } // End Function GetArguments


        protected virtual string OdbcFunctionReplacementCallback(System.Text.RegularExpressions.Match mThisMatch)
        {
            // Get the matched string.
            string strExpression = mThisMatch.Groups[1].Value;
            string strFunctionName = mThisMatch.Groups[2].Value;
            string strArguments = mThisMatch.Groups[3].Value;

            if (System.Text.RegularExpressions.Regex.IsMatch(strArguments, strOdbcMatchingPattern))
            {
                strArguments = System.Text.RegularExpressions.Regex.Replace(strArguments, strOdbcMatchingPattern, OdbcFunctionReplacementCallback);
            }


            // Simple one or 0 arguments
            // if (StringComparer.OrdinalIgnoreCase.Equals("lcase", strFunctionName))
            // { return "LOWER(" + strArguments + ") "; }


            string[] astrArguments = GetArguments(strArguments);

            if (System.StringComparer.OrdinalIgnoreCase.Equals("ilike", strFunctionName))
            {
                string strTerm = "( " + astrArguments[0] + " LIKE " + astrArguments[1] + @" ESCAPE '\' ) ";
                return strTerm;
            }

            if (System.StringComparer.OrdinalIgnoreCase.Equals("like", strFunctionName))
            {
                string strTerm = "( " + astrArguments[0] + " COLLATE Latin1_General_BIN LIKE " + astrArguments[1] + @" ESCAPE '\' ) ";
                return strTerm;
            }



            // if (StringComparer.OrdinalIgnoreCase.Equals("left", strFunctionName))
            // { string strTerm = "LPAD(" + astrArguments[0] + ", " + astrArguments[1] + ", '') "; return strTerm;}

            return "ODBC FUNCTION \"" + strFunctionName + "\" not defined in abstraction layer...";
        } // End Function OdbcFunctionReplacementCallback


        // Credits: http://web.archive.org/web/20100123183531/http://blogs.msdn.com/bclteam/archive/2005/03/15/396452.aspx
        protected const string strOdbcMatchingPattern = "({fn\\s*(.+?)\\s*\\(([^{}]*(((?<Open>{)[^{}]*)+((?<Close-Open>})[^{}]*)+)*(?(Open)(?!)))\\s*\\)\\s*})";

        protected virtual string ReplaceOdbcFunctions(string strSQL)
        {
            if (string.IsNullOrEmpty(strSQL))
            {
                return strSQL;
            }

            //Dim strOdbcMatchingPattern As String = "{fn LCASE(.*)}"
            //strOdbcMatchingPattern = "<customtag>(.+?)</customtag>"
            //strOdbcMatchingPattern = "{fn\s*(LCASE)\s*\((.+?)\s*\)\s*}"
            //'strOdbcMatchingPattern = "{fn\s*(.+?)\s*\((.+?)\s*\)\s*}"
            //'strOdbcMatchingPattern = "(<[^<>]*(((?<Open><)[^<>]*)+((?<Close-Open>>)[^<>]*)+)*(?(Open)(?!))>)"
            //strOdbcMatchingPattern = "({[^{}]*(((?<Open>{)[^{}]*)+((?<Close-Open>})[^{}]*)+)*(?(Open)(?!))})" ' match parantheses

            //strOdbcMatchingPattern = "({fn\s*[^{}]*(((?<Open>{)[^{}]*)+((?<Close-Open>})[^{}]*)+)*(?(Open)(?!))})" ' match parantheses
            //string strOdbcMatchingPattern = "({fn\\s*(.+?)\\s*\\(([^{}]*(((?<Open>{)[^{}]*)+((?<Close-Open>})[^{}]*)+)*(?(Open)(?!)))\\s*\\)\\s*})";
            // match parantheses

            //str = System.Text.RegularExpressions.Regex.Replace(str, strPattern, "equivalent")


            string strReturnValue = "";
            strReturnValue = System.Text.RegularExpressions.Regex.Replace(strSQL, strOdbcMatchingPattern, OdbcFunctionReplacementCallback);
            return strReturnValue;
        } // End Function ReplaceOdbcFunctions


    }


}
