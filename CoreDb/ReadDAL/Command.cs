
namespace CoreDb
{


    partial class ReadDAL
    {


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


        public virtual System.Data.IDbCommand CreateLimitedCommand(string strSQL, int iLimit)
        {
            // strSQL = string.Format(strSQL, "", "LIMIT " + iLimit.ToString());
            strSQL += $" {System.Environment.NewLine}OFFSET 0 ROWS FETCH NEXT {iLimit} ROWS ONLY";

            return CreateCommand(strSQL);
        }


        public virtual System.Data.IDbCommand CreateLimitedOdbcCommand(string strSQL, int iLimit)
        {
            // strSQL = ReplaceOdbcFunctions(strSQL);
            return CreateLimitedCommand(strSQL, iLimit);
        }


        public virtual System.Data.IDbCommand CreatePagedCommand(string strSQL, ulong ulngPageNumber, ulong ulngPageSize)
        {
            ulong ulngStartIndex = ((ulngPageSize * ulngPageNumber) - ulngPageSize) + 1;
            // ulong ulngEndIndex = ulngStartIndex + ulngPageSize - 1;

            // strSQL += $" {System.Environment.NewLine}OFFSET {ulngStartIndex} LIMIT {ulngPageSize} ";
            strSQL += $" {System.Environment.NewLine}OFFSET {ulngStartIndex} ROWS FETCH NEXT {ulngPageSize} ROWS ONLY";


            //OFFSET @ulngStartIndex 
            //LIMIT (@ulngEndIndex - @ulngStartIndex) 

            System.Data.IDbCommand cmd = this.CreateCommand(strSQL);

            // fsck... {fn NOW()}
            // cmd.CommandText = string.Format(strSQL, " /* TOP 1 */ ", "OFFSET 0 FETCH NEXT 1 ROWS ONLY");

            // this.AddParameter(cmd, "ulngStartIndex", ulngStartIndex);
            // this.AddParameter(cmd, "ulngEndIndex", ulngEndIndex);
            return cmd;
        }


        public System.Data.IDbCommand CreatePagedCommand(string strSQL, int ulngStartIndex, int ulngEndIndex)
        {
            return CreatePagedCommand(strSQL, (ulong)ulngStartIndex, (ulong)ulngEndIndex);
        }


    }


}
