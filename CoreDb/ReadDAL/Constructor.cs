using System;
using System.Collections.Generic;
using System.Text;

namespace CoreDb
{
    public partial class ReadDAL
    {

        public ReadDAL()
        { }

        public ReadDAL(DalConfig config)
        {
            this.m_factory = config.ProviderFactory;
            this.m_ConnectionString = config.ConnectionString;
            this.m_reservedWords = this.PopulateReservedWords();
        }

    }
}
