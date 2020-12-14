using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace WebMVC.Models
{
    public class aluno
    {
        [Required]
        [Display(Name = "Número do Aluno" )]
        public int Naluno { get; set; }

        [Required]
        [Display(Name = "Primeiro Nome")]
        public string PriNome { get; set; }

        [Required]
        [Display(Name = "Último Nome")]
        public string UltNome { get; set; }

        [Required]
        public string Morada { get; set; }

        [Required]
        [Display(Name = "Género")]
        public Genero Genero { get; set; }

        [Required]
        [Display(Name = "Data de Nascimento")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString ="{0:yyyy-mm-dd}", ApplyFormatInEditMode =true)]
        public DateTime DataNasc { get; set; }

        [Required]
        [Display(Name = "Ano de Escolaridade")]
        [Range(1,12)]
        public int AnoEscolaridade { get; set; }

        [Display(Name = "Imagem")]
        public string ImgPath { get; set; }

        [Required]
        public HttpPostedFileBase imagem { get; set; }
    }

  
}