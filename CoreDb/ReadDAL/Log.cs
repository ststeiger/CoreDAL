
namespace CoreDb
{


    partial class ReadDAL
    {


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



    }
}
