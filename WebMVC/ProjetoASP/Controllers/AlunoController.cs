using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProjetoASP.Models;
using System.IO;
using MySql.Data.MySqlClient;

namespace ProjetoASP.Controllers
{
    public class AlunoController : Controller
    {
        // GET: Aluno
        public ActionResult CriaAluno()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CriaAluno(Aluno aluno)
        {
            if(ModelState.IsValid)
            {
                string ImagemNome = Path.GetFileNameWithoutExtension(aluno.Imagem.FileName);
                string ImagemExt = Path.GetExtension(aluno.Imagem.FileName);
                ImagemNome = DateTime.Now.ToString("yyyyMMddHHmmss") + "-" + ImagemNome.Trim() + ImagemExt;
                aluno.ImgPath = @"\Content\Imagens\" + ImagemNome;
                aluno.Imagem.SaveAs(ControllerContext.HttpContext.Server.MapPath(aluno.ImgPath));

                ConexaoDB conn = new ConexaoDB("localhost", 3307, "root", "root", "formacao");

                using(MySqlConnection conexao = conn.ObterConexao())
                {
                    if(conexao!=null)
                    {
                        string stm = "insert into alunos values(0, @primeiroNome, @ultimoNome, @morada, @genero, @dataNasc, @ano, @foto)";
                        using(MySqlCommand cmd = new MySqlCommand(stm, conexao))
                        {
                            cmd.Parameters.AddWithValue("@primeiroNome", aluno.PriNome);
                            cmd.Parameters.AddWithValue("@ultimoNome", aluno.UltNome);
                            cmd.Parameters.AddWithValue("@morada", aluno.Morada);
                            cmd.Parameters.AddWithValue("@genero", aluno.Genero);
                            cmd.Parameters.AddWithValue("@dataNasc", aluno.DataNasc);
                            cmd.Parameters.AddWithValue("@ano", aluno.AnoEscolaridade);
                            cmd.Parameters.AddWithValue("@foto", aluno.ImgPath);

                            int nRegistos = cmd.ExecuteNonQuery();
                        }
                    }
                }
            }

            return RedirectToAction("CriaAluno");
        }
    }
}