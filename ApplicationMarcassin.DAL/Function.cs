using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationMarcassin.DAL
{
    partial class BBD_projetEntities
    {
        public int LangueParDefaut()
        {
            var query = "SELECT [dbo].[LangueParDefaut]()";
            int result = this.Database.SqlQuery<int>(query).First();
            
            return result;
        }
    }
}
