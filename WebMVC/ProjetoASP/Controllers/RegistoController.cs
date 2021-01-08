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
    public class RegistoController : Controller
    {
        // GET: Registo
        public ActionResult Registo()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Registo(Utilizador utilizador)
        {
            if (ModelState.IsValid)
            {
                ConexaoDB conn = new ConexaoDB("localhost", 3307, "root", "root", "formacao");
                using (MySqlConnection conexao = conn.ObterConexao())
                {
                    if (conexao != null)
                    {
                        string stm = "insert into utilizadores values (0,@Email,MD5(@Password))";
                        using (MySqlCommand cmd = new MySqlCommand(stm,conexao))
                        {
                            cmd.Parameters.AddWithValue("@Email", utilizador.email);
                            cmd.Parameters.AddWithValue("@Password", utilizador.password);

                            int nRegistos = cmd.ExecuteNonQuery();

                            if (nRegistos == 1)

                                return RedirectToAction("Login");
                            
                        }
                    }
                }
            }
            return RedirectToAction("Registo");
        }
            public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Utilizador utilizador)
        {
            if (ModelState.IsValid)
            {
                ConexaoDB conn = new ConexaoDB("localhost", 3307, "root", "root", "formacao");
                using (MySqlConnection conexao = conn.ObterConexao())
                {
                    if (conexao != null)
                    {
                        string stm = "select * from utilizadores where email=@email and password=MD5(@password)";
                        using (MySqlCommand cmd = new MySqlCommand(stm, conexao))
                        {
                            cmd.Parameters.AddWithValue("@Email", utilizador.email);
                            cmd.Parameters.AddWithValue("@Password", utilizador.password);

                            using (MySqlDataReader reader = cmd.ExecuteReader())
                            {
                                if(reader.Read())
                                {
                                    Session["login"] = 1;
                                    Session["email"] = utilizador.email;


                                    return RedirectToAction("ListarAluno", "Aluno");
                                }
                            }


                        }
                    }
                }
            }
            return RedirectToAction("Login");
        }

        public ActionResult Logout()
        {
            if(Session["login"] != null)
            {
                Session.Abandon();
            }
            return RedirectToAction("Login");
        }
    }
}