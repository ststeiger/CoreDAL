
namespace CoreDb
{


    partial class ReadDAL
    {

        public virtual System.Collections.Generic.HashSet<T> GetHashSet<T>(System.Data.IDbCommand cmd, System.Collections.Generic.IEqualityComparer<T> comp)
        {
            System.Collections.Generic.List<T> ls = GetList<T>(cmd);

            System.Collections.Generic.HashSet<T> hs =
                new System.Collections.Generic.HashSet<T>(ls, comp)
            ;

            ls.Clear();
            ls = null;

            return hs;
        }


        public virtual System.Collections.Generic.HashSet<string> GetHashSet(System.Data.IDbCommand cmd)
        {
            return GetHashSet<string>(cmd, System.StringComparer.OrdinalIgnoreCase);
        }


        public virtual System.Collections.Generic.HashSet<T> GetHashSet<T>(string sql, System.Collections.Generic.IEqualityComparer<T> comp)
        {
            System.Collections.Generic.HashSet<T> hs = null;

            using (System.Data.Common.DbCommand cmd = this.CreateCommand(sql))
            {
                hs = GetHashSet<T>(cmd, comp);
            } // End Using cmd

            return hs;
        }


        public virtual System.Collections.Generic.HashSet<string> GetHashSet(string sql)
        {
            return GetHashSet<string>(sql, System.StringComparer.OrdinalIgnoreCase);
        }


    }


}
