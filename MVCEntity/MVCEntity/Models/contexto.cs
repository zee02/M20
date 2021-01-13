using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MVCEntity.Models
{
    public class contexto:DbContext
    {
        public DbSet<pessoa> pessoas { get; set; }
    }
}