using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjetoASP.Models
{
    public class Aluno
    {
        [Required]
        [Display (Name ="Número do Aluno")]
        public int Naluno { get; set; }

        [Required]
        [Display(Name = "Primeiro Nome")]
        public string PriNome { get; set; }

        [Required]
        [Display(Name = "Ultimo Nome")]
        public string UltNome { get; set; }

        [Required]
        public string Morada { get; set; }

        [Required]
        public Genero Genero { get; set; }

        [Required]
        [Display(Name = "Data de Nascimento")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString ="{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DataNasc { get; set; }

        [Required]
        [Display(Name = "Ano de Escolaridade")]
        [Range(1,12)]
        public int AnoEscolaridade { get; set; }

        [Display(Name = "Imagem")]
        public string ImgPath { get; set; }

        public HttpPostedFileBase Imagem { get; set; }
    }
}