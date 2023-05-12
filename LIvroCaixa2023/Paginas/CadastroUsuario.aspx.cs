using LivroCaixa2023.Classes;
using LivroCaixa2023.Paginas;
using LivroCaixa2023.Tabelas;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LIvroCaixa2023.Paginas
{
    public partial class CadastroUsuario : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuario"] == null)
            {
                Response.Redirect("Index.aspx", false);
            }
            inicializa();

            ltl_Tabela.Text = Usuario.montaTabela("Tabela de Usuários");
        }

        private void inicializa()
        {
            lbl_Titulo.Text = "Cadastrar Usuário";
            lbl_Login.Text = "Login";
            lbl_Nome.Text = "Nome";
            lbl_CPF.Text = "CPF";
            lbl_Perfil.Text = "Perfil";
            btn_Salvar.Text = "Ok";
            btn_Limpar.Text = "Limpar";
            txt_CPF.ReadOnly = Session["usuarioBusca"] != null;
            lbl_BuscaPorId.Text = "Busca por ID";
            btn_BuscaId.Text = "Buscar";
            btn_Excluir.Text = "Excluir";
            btn_GerarSenha.Text = "Reinicializar senha";
            btn_Excluir.Enabled = btn_GerarSenha.Enabled = false;
            btn_Excluir.BackColor = btn_GerarSenha.BackColor = Color.FromName("gray");
            btn_Excluir.ToolTip = "Limpa os campos e inicializa o usuário da busca!";
        }

        protected void btn_Salvar_Click(object sender, EventArgs e)
        {

            lbl_Mensagem.Text = "";
            lbl_MensagemSucesso.Text = "";
            if (txt_Login.Text == "" || txt_Nome.Text == "" || txt_CPF.Text == "")
            {
                lbl_Mensagem.Text = "Preencha todos os campos! :(";
            }
            if (ccb_Adm.Checked && ccb_User.Checked)
            {
                lbl_Mensagem.Text = "Selecione apenas uma opção de perfil! :(";
                return;
            }
            if (!ccb_Adm.Checked && !ccb_User.Checked)
            {
                lbl_Mensagem.Text = "Selecione uma opção de perfil! :(";
                return;
            }
            if (!validaCPF(txt_CPF.Text))
            {
                lbl_Mensagem.Text = "O CPF digitado é inválido! :(";
                return;
            }
            if (Session["usuarioBusca"] != null)
            {
                Usuario uSelecionado = (Usuario)Session["usuarioBusca"];
                alteraUsuario(uSelecionado);
                return;
            }

            if (Usuario.lista != null)
            {
                foreach (Usuario usu in Usuario.lista)
                {
                    if (txt_Login.Text == usu.login)
                    {
                        lbl_Mensagem.Text = "Login já Cadastrado";
                        return;
                    }

                    if (txt_CPF.Text == usu.cpf)
                    {
                        lbl_Mensagem.Text = "CPF já Cadastrado";
                        return;
                    }
                }
            }

            Usuario u = new Usuario(txt_Login.Text, txt_CPF.Text, txt_Nome.Text, ccb_Adm.Checked ? 'A' : 'U', txt_CPF.Text);
            Usuario.lista.Add(u);
            Serializa.saveUsuario(Usuario.lista);

            lbl_MensagemSucesso.Text = "Usuário cadastrado com sucesso! :)";
            txt_Nome.Text = "";
            txt_Login.Text = "";
            txt_CPF.Text = "";
            lbl_Mensagem.Text = "";
            ccb_Adm.Checked = false;
            ccb_User.Checked = false;
            ltl_Tabela.Text = Usuario.montaTabela("Tabela de Usuários");
        }

        private void alteraUsuario(Usuario u)
        {
            u.nome = txt_Nome.Text;
            u.login = txt_Login.Text;
            u.perfil = ccb_Adm.Checked ? 'A' : 'U';
            Serializa.saveUsuario(Usuario.lista);
            ltl_Tabela.Text = Usuario.montaTabela("Tabela de Usuários");
            lbl_MensagemSucesso.Text = "Usuário alterado com sucesso! :)";
        }

        public bool validaCPF(string cpf)
        {
            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            string tempCpf;
            string digito;
            int soma;
            int resto;

            cpf = cpf.Trim();
            cpf = cpf.Replace(".", "").Replace("-", "").Replace("/", "").Replace("-", " ");

            if (cpf.Length != 11)
                return false;

            tempCpf = cpf.Substring(0, 9);
            soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

            resto = soma % 11;

            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito = resto.ToString();

            tempCpf = tempCpf + digito;

            soma = 0;

            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];

            resto = soma % 11;

            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito = digito + resto.ToString();

            return cpf.EndsWith(digito);
        }

        protected void btn_BuscaId_Click(object sender, EventArgs e)
        {
            lbl_Mensagem.Text = "";
            lbl_MensagemSucesso.Text = "";
            if (txt_IdBusca.Text.Trim() == String.Empty)
            {
                lbl_Mensagem.Text = "Digite um ID de usuário para busca";
                return;
            }

            int id = 0;

            if (!Int32.TryParse(txt_IdBusca.Text, out id))
            {
                lbl_Mensagem.Text = "ID inválido! :(";
                return;
            }

            Usuario uBusca = new Usuario(id);
            Usuario.lista.Sort();
            int a = Usuario.lista.BinarySearch(uBusca);

            if (a >= 0)
            {
                Usuario u = Usuario.lista[a];
                txt_CPF.ReadOnly = true;
                Session["usuarioBusca"] = u;
                lbl_MensagemSucesso.Text = "Usuário encontrado! :)";
                btn_Excluir.Enabled = btn_GerarSenha.Enabled = true;
                btn_Excluir.BackColor = btn_GerarSenha.BackColor = Color.FromName("none");
                mostra(u);
                return;
            }

            lbl_Mensagem.Text = String.Concat("Usuário ID ", id, " não encontrado! :(");
        }
        private void mostra(Usuario u)
        {
            txt_Nome.Text = u.nome;
            txt_Login.Text = u.login;
            txt_CPF.Text = u.cpf;
            ccb_Adm.Checked = u.perfil == 'A';
            ccb_User.Checked = u.perfil == 'U';
        }
        private void limpar()
        {
            lbl_Mensagem.Text = txt_IdBusca.Text = txt_Nome.Text = txt_Login.Text = txt_CPF.Text = "";
            txt_CPF.ReadOnly = false;
            lbl_MensagemSucesso.Text = lbl_Mensagem.Text = "";

            ccb_Adm.Checked = ccb_User.Checked = false;

            Session["usuarioBusca"] = null;
        }
        protected void btn_Limpar_Click(object sender, EventArgs e)
        {
            limpar();
        }

        protected void btn_Excluir_Click(object sender, EventArgs e)
        {
            if (Session["usuarioBusca"] == null)
            {
                return;
            }

            Usuario uBusca = (Usuario)Session["usuarioBusca"];
            Usuario.lista.Remove(uBusca);

            Serializa.saveUsuario(Usuario.lista);

            limpar();
            lbl_MensagemSucesso.Text = "Usuario excluído com sucesso! :)";
            ltl_Tabela.Text = Usuario.montaTabela("Usuarios Cadastrados");
        }

        protected void btn_GerarSenha_Click(object sender, EventArgs e)
        {
            if (Session["usuarioBusca"] == null)
            {
                return;
            }

            Usuario u = (Usuario)Session["usuarioBusca"];
            u.password = u.cpf;
            Serializa.saveUsuario(Usuario.lista);

            ltl_Tabela.Text = Usuario.montaTabela("Usuários Cadastrados");
            lbl_MensagemSucesso.Text = "Senha inicializada!";
        }

        protected void btn_Voltar_Click(object sender, EventArgs e)
        {
            Response.Redirect("Index.aspx", true);
            return;
        }
    }
}
