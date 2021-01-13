using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCEntity.Models
{
    public class pessoa
    {
        [Key]
        public int Codigo { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
    }
}