using LivroCaixa2023.Classes;
using LivroCaixa2023.Tabelas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LivroCaixa2023.Paginas
{
    public partial class Index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            inicializa();

            //Usuario.lista.Add(new Usuario("helio.rangel", "12345", "rangel", 'A', "32369280778"));

            //Serializa.saveUsuario(Usuario.lista);
            //return;

            if (Usuario.lista.Count == 0)
            {
                Usuario.lista = Serializa.loadUsuario();
                if (Usuario.lista != null)
                {
                    foreach (Usuario u in Usuario.lista)
                    {
                        Usuario.idRaiz = u.id > Usuario.idRaiz ? u.id : Usuario.idRaiz;
                    }
                }
            }
            if (Session["usuario"] != null)
            {
                Usuario usuario = (Usuario)Session["usuario"];
                if (usuario.perfil == 'A')
                {
                    pnl_Menu.Visible = true;
                    return;
                }
            }
        }

        private void inicializa()
        {
            lbl_Titulo.Text = "Login";
            lbl_Login.Text = "Login";
            lbl_Senha.Text = "Senha";
            btn_Login.Text = "Login";
        }

        protected void btn_Login_Click(object sender, EventArgs e)
        {
            Usuario busca = new Usuario(txt_Login.Text, txt_Senha.Text);

            foreach (Usuario usuario in Usuario.lista)
            {
                if (usuario.usuarioOk(busca))
                {
                    Session["usuario"] = usuario;

                    HttpCookie cookie = Response.Cookies["login"];
                    if (cookie != null)
                    {
                        cookie = new HttpCookie(usuario.id.ToString());
                        cookie.Values.Add("ID", usuario.id.ToString());
                        cookie.Expires = DateTime.Now.AddDays(1);
                        cookie.HttpOnly = true;
                        Response.AppendCookie(cookie);
                    }

                    if (usuario.perfil == 'A')
                    {
                        pnl_Menu.Visible = true;
                        return;
                    }
                    else
                    {
                        Response.Redirect("FluxodeCaixa.aspx", false);
                        return;
                    }
                }
            }
            lbl_Resposta.Text = "Usuário não cadastrado!";
        }

        protected void btn_RedirectCadastroUsuario_Click(object sender, EventArgs e)
        {
            Response.Redirect("CadastroUsuario.aspx", false);
            return;
        }

        protected void btn_RedirectFluxoDeCaixa_Click(object sender, EventArgs e)
        {
            Response.Redirect("FluxoDeCaixa.aspx", false);
            return;
        }

        protected void btn_Sair_Click(object sender, EventArgs e)
        {
            Session.Abandon();
            Response.Redirect("Index.aspx", false);
        }
    }
}