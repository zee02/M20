using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjetoASP.Models
{
    public class Utilizador
    {
        public int Mutilizador { get; }
        [Display(Name="E-mail")]
        [DataType(DataType.EmailAddress)]

        public string email { get; set; }
        [DataType(DataType.Password)]

        public string password { get; set; }
    }
}