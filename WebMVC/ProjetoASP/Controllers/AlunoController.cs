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
        public ActionResult ListarAluno()
        {
            ConexaoDB Conn = new ConexaoDB("localhost", 3307, "root", "root", "formacao");
            List<Aluno> lista = new List<Aluno>();
            using (MySqlConnection conexao = Conn.ObterConexao())
            {
                if (conexao != null)
                    using (MySqlCommand cmd = new MySqlCommand("select * from alunos", conexao))
                    {
                        using(MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while(reader.Read())
                            {
                                lista.Add(new Aluno()
                                {
                                    Naluno = reader.GetInt32("idAlunos"),
                                    PriNome = reader.GetString("nome"),
                                    UltNome = reader.GetString("ultimo_nome"),
                                    Morada = reader.GetString("morada"),
                                    Genero = reader.GetString("genero") == "Masculino" ? Genero.Masculino : Genero.Feminino,
                                    DataNasc = reader.GetDateTime("datanasc"),
                                    AnoEscolaridade = reader.GetInt16("ano_escolaridade"),
                                });
                            }
                        }
                    }
               
            }
            return View(lista);
        }
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
                            cmd.Parameters.AddWithValue("@genero", aluno.Genero.ToString());
                            cmd.Parameters.AddWithValue("@dataNasc", aluno.DataNasc);
                            cmd.Parameters.AddWithValue("@ano", aluno.AnoEscolaridade);
                            cmd.Parameters.AddWithValue("@foto", aluno.ImgPath);

                            int nRegistos = cmd.ExecuteNonQuery();
                        }
                    }
                }
            }

            return RedirectToAction("ListarAluno");
        }

        public ActionResult DetalheAluno(int? id)
        {
            ConexaoDB Conn = new ConexaoDB("localhost", 3307, "root", "root", "formacao");
            Aluno aluno = null;
            using (MySqlConnection conexao = Conn.ObterConexao())
            {
                if (conexao != null)
                    using (MySqlCommand cmd = new MySqlCommand("select * from alunos where idalunos=@idaluno", conexao))
                    {
                        cmd.Parameters.AddWithValue("@idaluno", id);
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if(reader.Read())
                            {

                                aluno = new Aluno()
                                {
                                    Naluno = reader.GetInt32("idalunos"),
                                    PriNome = reader.GetString("nome"),
                                    UltNome = reader.GetString("ultimo_nome"),
                                    Morada = reader.GetString("morada"),
                                    Genero = reader.GetString("genero") == "Masculino" ? Genero.Masculino : Genero.Feminino,
                                    DataNasc = reader.GetDateTime("datanasc"),
                                    AnoEscolaridade = reader.GetInt16("ano_escolaridade"),
                                    ImgPath = reader.GetString("foto")
                                };
                                return View(aluno);
                            }
                        }
                    }

            }
            return RedirectToAction("ListarAluno");
        }
       
        [HttpPost]
        public ActionResult EditaAluno(Aluno aluno)
        {
            if(ModelState.IsValid)
            {
                bool img = false;
                if(aluno.Imagem != null)
                {
                    string ImagemNome = Path.GetFileNameWithoutExtension(aluno.Imagem.FileName);
                    string ImagemExt = Path.GetExtension(aluno.Imagem.FileName);

                    ImagemNome = DateTime.Now.ToString("yyyyMMddHHmmss") + "-" + ImagemNome.Trim() + ImagemExt;
                    aluno.ImgPath = @"\Content\Imagens\" + ImagemNome;

                    aluno.Imagem.SaveAs(ControllerContext.HttpContext.Server.MapPath(aluno.ImgPath));
                    img = true;
                }
                ConexaoDB conn = new ConexaoDB("localhost", 3307, "root", "root", "formacao");
                using(MySqlConnection conexao = conn.ObterConexao())
                {
                    if(conexao != null)
                    {
                        string strFoto = (img) ? ",foto=@foto" : "";
                        string stm = "update alunos set nome=@primeiroNome," + "ultimo_nome=@ultimoNome," + 
                        "morada=@morada," + "genero=@genero," + "datanasc=@dataNasc," + "ano_escolaridade=@ano" + strFoto + " where idalunos=@idAluno";

                        using (MySqlCommand cmd = new MySqlCommand(stm, conexao))
                        {
                            cmd.Parameters.AddWithValue("@primeiroNome", aluno.PriNome);
                            cmd.Parameters.AddWithValue("@ultimoNome", aluno.UltNome);
                            cmd.Parameters.AddWithValue("@morada", aluno.Morada);
                            cmd.Parameters.AddWithValue("@genero", aluno.Genero.ToString());
                            cmd.Parameters.AddWithValue("@dataNasc", aluno.DataNasc);
                            cmd.Parameters.AddWithValue("@ano", aluno.AnoEscolaridade);
                            cmd.Parameters.AddWithValue("@idAluno", aluno.Naluno);

                            if (img)
                                cmd.Parameters.AddWithValue("@foto", aluno.ImgPath);

                                int nRgistos = cmd.ExecuteNonQuery();
                               
                            
                        }   
                    }
                }
            }
            return RedirectToAction("ListarAluno");
        }
        public ActionResult EditaAluno(int? id)
        {
            ConexaoDB Conn = new ConexaoDB("localhost", 3307, "root", "root", "formacao");
            Aluno aluno = null;
            using (MySqlConnection conexao = Conn.ObterConexao())
            {
                if (conexao != null)
                    using (MySqlCommand cmd = new MySqlCommand("select * from alunos where idalunos=@idaluno", conexao))
                    {
                        cmd.Parameters.AddWithValue("@idaluno", id);
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {

                                aluno = new Aluno()
                                {
                                    Naluno = reader.GetInt32("idalunos"),
                                    PriNome = reader.GetString("nome"),
                                    UltNome = reader.GetString("ultimo_nome"),
                                    Morada = reader.GetString("morada"),
                                    Genero = reader.GetString("genero") == "Masculino" ? Genero.Masculino : Genero.Feminino,
                                    DataNasc = reader.GetDateTime("datanasc"),
                                    AnoEscolaridade = reader.GetInt16("ano_escolaridade"),
                                    ImgPath = reader.GetString("foto")
                                };
                                return View(aluno);
                            }
                        }
                    }

            }
            return RedirectToAction("ListarAluno");
        }

        public ActionResult EliminaAluno(int? id)
        {
            ConexaoDB Conn = new ConexaoDB("localhost", 3307, "root", "root", "formacao");
            Aluno aluno = null;

            using (MySqlConnection conexao = Conn.ObterConexao())
            {
                if (conexao != null)
                    using (MySqlCommand cmd = new MySqlCommand("select * from alunos where idalunos=@idaluno", conexao))
                    {
                        cmd.Parameters.AddWithValue("@idaluno", id);
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {

                                aluno = new Aluno()
                                {
                                    Naluno = reader.GetInt32("idalunos"),
                                    PriNome = reader.GetString("nome"),
                                    UltNome = reader.GetString("ultimo_nome"),
                                    Morada = reader.GetString("morada"),
                                    Genero = reader.GetString("genero") == "Masculino" ? Genero.Masculino : Genero.Feminino,
                                    DataNasc = reader.GetDateTime("datanasc"),
                                    AnoEscolaridade = reader.GetInt16("ano_escolaridade"),
                                    ImgPath = reader.GetString("foto")
                                };
                                TempData["ImagemPath"] = aluno.ImgPath;
                                return View(aluno);
                           
                            }
                        }
                    }

            }
            return RedirectToAction("ListarAluno");
        }

        [HttpPost, ActionName("EliminaAluno")]
        public ActionResult EliminaAlunoConfirmacao(int? id)
        {
            ConexaoDB conn = new ConexaoDB("localhost", 3307, "root", "root", "formacao");

            using (MySqlConnection conexao = conn.ObterConexao())
            {
                if(conexao != null)
                {
                    string stm = "delete from alunos where idalunos = @idAluno";

                    using(MySqlCommand cmd = new MySqlCommand(stm,conexao))
                    {
                        cmd.Parameters.AddWithValue("@idAluno", id);
                        int nRgistos = cmd.ExecuteNonQuery();

                        if(nRgistos == 1)
                        {
                            new FileInfo(ControllerContext.HttpContext.Server.MapPath(TempData["ImagemPath"].ToString())).Delete();
                        }
                    }
                }
            }
            return RedirectToAction("ListarAluno");
        }


    }
}