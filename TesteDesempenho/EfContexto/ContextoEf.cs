using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EfContexto
{
    public class ContextoEf:DbContext
    {
        public ContextoEf():base("strConnection")
        {
            
        }

        public DbSet<Teste> Testes { get; set; }
    }
}
