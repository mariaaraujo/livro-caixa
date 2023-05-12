using LivroCaixa2023.Classes;
using LivroCaixa2023.Tabelas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LIvroCaixa2023.Paginas
{
    public partial class FluxoDeCaixa : System.Web.UI.Page
    {
        private LivroCaixa livroCaixa = new LivroCaixa();
        private static List<LivroCaixa> listaLivro;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (listaLivro == null)
            {
                listaLivro = Serializa.loadLivroCaixa();
                if (listaLivro == null) // Isso só roda se a lista de Livros estiver zerada
                {
                    int mesCorrente = DateTime.Now.Month;
                    int anoCorrente = DateTime.Now.Year;

                    listaLivro = new List<LivroCaixa>();
                    livroCaixa = new LivroCaixa(0);
                    livroCaixa.ano = anoCorrente.ToString();
                    livroCaixa.mes = mesCorrente.ToString();
                    livroCaixa.stLivroFechado = false;
                    listaLivro.Add(livroCaixa);
                    Serializa.saveLivroCaixa(listaLivro);
                }
                listaLivro.Sort();

                //listaLivro[0].stLivroFechado = true;
                //listaLivro[1].stLivroFechado = false;


                //listaLivro[0].mes = "4";
                //listaLivro[1].mes = "5";

                Serializa.saveLivroCaixa(listaLivro);

                livroCaixa = listaLivro[listaLivro.Count - 1];
                LivroCaixa.idRaiz = livroCaixa.idLivroCaixa + 1;
            }

            if (Session["usuario"] == null)
            {
                Response.Redirect("index.aspx", false);
                return;
            }

            if (LivroCaixa.lancamentos != null &&
                LivroCaixa.lancamentos.Count == 0)
            {
                LivroCaixa.lancamentos = Serializa.loadLancamento();

                if (LivroCaixa.lancamentos != null && LivroCaixa.lancamentos.Count > 0)
                {
                    LivroCaixa.lancamentos.Sort();
                    Lancamento.idRaiz =
                        LivroCaixa.lancamentos[LivroCaixa.lancamentos.Count - 1].idLancamento + 1;
                }
                else
                {
                    LivroCaixa.lancamentos = new List<Lancamento>();
                }
            }

            ltl_Lancamento.Text = LivroCaixa.montaTabela("Lançamentos do Mês ", livroCaixa.valorTransportado, livroCaixa);

            inicializa();

            if (IsPostBack)
            {
                String evento = (this.Request["__EVENTTARGET"] == null) ? String.Empty : this.Request["__EVENTTARGET"];
                string resposta = (this.Request["__EVENTARGUMENT"] == null) ? string.Empty : this.Request["__EVENTARGUMENT"];
                if (resposta == "true" && evento == "fecharPeriodo")
                {
                    fecharPeriodo();
                }
            }
            populaComboAno(ddl_Ano);
            populaComboMes(ddl_Mes);
        }

        private void inicializa()
        {
            Usuario usuario = (Usuario)Session["usuario"];

            btn_Voltar.Visible = usuario.perfil == 'A';
            btn_Sair.Visible = !btn_Voltar.Visible;
            lbl_ValorTransportado.Text = "Valor transportado " + livroCaixa.valorTransportado;
        }

        private void fecharPeriodo()
        {
            return;
            livroCaixa.stLivroFechado = true;
            LivroCaixa novo = new LivroCaixa(livroCaixa.pegaSaldo(livroCaixa));
            int mes, ano;
            if(!int.TryParse(livroCaixa.ano, out ano) || ano == 0)
            {
                ano = DateTime.Now.Year;
            }
            if(!int.TryParse(livroCaixa.mes, out mes) || mes == 0)
            {
                mes = DateTime.Now.Month;
            }
            DateTime periodoAtual = new DateTime(ano, mes, 1);
            DateTime proximo = periodoAtual.AddMonths(1);
            novo.mes = proximo.Month + "";
            novo.ano = proximo.Year + "";
            listaLivro.Add(novo);
            listaLivro.Sort();
            livroCaixa = novo;
            Serializa.saveLivroCaixa(listaLivro);
            lbl_MensagemSucesso.Text = "Livro caixa fechado com sucesso";
        }

        protected void btn_Ok_Click(object sender, EventArgs e)
        {
            lbl_Mensagem.Text = "";
            Double valor;
            DateTime data;
            if (txt_Valor.Text == "" || txt_Descricao.Text == "" || txt_Data.Text == "")
            {
                lbl_Mensagem.Text = "Preencha todos os campos!";
                return;
            }
            if (ccb_Credito.Checked && ccb_Debito.Checked)
            {
                lbl_Mensagem.Text = "Selecione apenas um tipo de transação!";
                return;
            }
            if (!ccb_Debito.Checked && !ccb_Credito.Checked)
            {
                lbl_Mensagem.Text = "Selecione um tipo de transação!";
                return;
            }
            if (!Double.TryParse(txt_Valor.Text, out valor))
            {
                lbl_Mensagem.Text = "Valor inválido!";
                return;
            }
            if (!DateTime.TryParse(txt_Data.Text, out data))
            {
                lbl_Mensagem.Text = "Data inválida!";
                return;
            }
            if (data.CompareTo(DateTime.Now) > 0)
            {
                lbl_Mensagem.Text = "Data Digitada deve ser igual ou anterior a data atual!";
                return;
            }
            if (valor < 0)
            {
                lbl_Mensagem.Text = "Lançamento não pode ser negativo";
                return;
            }
            if (ccb_Debito.Checked)
            {
                double saldo = livroCaixa.pegaSaldo(livroCaixa);
                if (saldo < valor)
                {
                    lbl_Mensagem.Text = "Saldo Insuficiente!";
                    return;
                }
            }
            Usuario user = (Usuario)Session["usuarioBusca"];
            Lancamento l = new Lancamento(txt_Descricao.Text, Lancamento.idRaiz, valor, ccb_Credito.Checked ? 'C' : 'D', user, data);
            LivroCaixa.lancamentos.Add(l);
            ltl_Lancamento.Text = LivroCaixa.montaTabela("Lançamentos", 0, livroCaixa);
            Serializa.saveLancamento(LivroCaixa.lancamentos);
            limpar();
        }

        private void limpar()
        {
            txt_Data.Text = "";
            txt_Descricao.Text = "";
            txt_Valor.Text = "";
            ccb_Credito.Checked = ccb_Debito.Checked = false;
        }

        protected void btn_Sair_Click(object sender, EventArgs e)
        {
            Session.Abandon();
            Response.Redirect("Index.aspx", true);
            return;
        }
        protected void btn_Voltar_Click(object sender, EventArgs e)
        {
            Response.Redirect("Index.aspx", true);
            return;
        }
        protected void btn_Fechar_Click(object sender, EventArgs e)
        {
            messageBox();
        }

        private void messageBox()
        {
            StringBuilder myScript = new StringBuilder(String.Empty);
            myScript.Append("<script type='text/javascript' language='javascript'> ");
            myScript.Append("var result = window.confirm('Confirma fechamento do período atual? Uma vez fechado não aceitará mais lançamentos! :/'); ");
            myScript.Append("__doPostBack('fecharPeriodo', result); ");
            myScript.Append("</script> ");
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "msg", myScript.ToString(), false);
        }

        protected void btn_Buscar_Click(object sender, EventArgs e)
        {

            string mes = ddl_Mes.SelectedValue;
            string ano = ddl_Ano.SelectedValue;

            ltl_Lancamento.Text = LivroCaixa.montaTabela("Lançamentos do Mês ", 0, livroCaixa);
        }

        private void populaComboAno(DropDownList comboAno)
        {
            if (comboAno.Items.Count > 0)
            {
                return;
            }

            for (int i = DateTime.Now.Year - 6; i <= DateTime.Now.Year; i++)
            {
                ListItem li = new ListItem();
                li.Text = i.ToString();
                li.Value = i.ToString();
                comboAno.Items.Add(li);
            }
            ddl_Ano.SelectedValue = DateTime.Now.Year.ToString();
        }

        private void populaComboMes(DropDownList comboMes)
        {
            if (comboMes.Items.Count > 0)
            {
                return;
            }

            for (int i =1; i <= 12; i++)
            {
                ListItem li = new ListItem();
                li.Text = i.ToString("D2");
                li.Value = i.ToString();
                comboMes.Items.Add(li);
            }
            ddl_Mes.SelectedValue = DateTime.Now.Month.ToString();
        }
    }
}