using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LivroCaixa2023.Tabelas
{

    [Serializable]
    public class Usuario : IComparable<Usuario>
    {
        public int CompareTo(Usuario u) { return this.id - u.id; }
        public Usuario(int id) { this.id = id; }

        public static List<Usuario> lista = new List<Usuario>();
        public static int idRaiz = 1;
        public int id { get; set; }
        public string login { get; set; }
        public string password { get; set; }
        public string nome { get; set; }
        public char perfil { get; set; }
        public string cpf { get; set; }

        public Usuario(String login, String password, String nome,
            char perfil, string cpf)
        {
            this.id = idRaiz++;
            this.login = login;
            this.password = password;
            this.nome = nome;
            this.perfil = perfil;
            this.cpf = cpf;
        }
        public Usuario(String login, String password)
        {
            this.id = idRaiz++;
            this.login = login;
            this.password = password;
            this.nome = login.ToUpper().Replace(".", " ");
            this.perfil = 'A';
        }

        public bool usuarioOk(Usuario u)
        {
            return this.login == u.login && this.password == u.password;
        }
        public static string montaTabela(String titulo)
        {

            Tabela tabela = new Tabela(lista.Count, 6, titulo);

            String[] titulos = { "ID", "Nome", "Login", "Senha", "Perfil", "CPF" };

            int i = 0;


            foreach (Usuario l in lista)
            {
                tabela.celula[i, 0] = l.id.ToString("D4");
                tabela.celula[i, 1] = l.nome;
                tabela.celula[i, 2] = l.login;
                tabela.celula[i, 3] = l.password == l.cpf ? l.cpf : "Já alterada";
                tabela.celula[i, 4] = l.perfil + "";
                tabela.celula[i, 5] = l.cpf;
                i++;
            }

            return tabela.tabela(titulos, i);
        }
    }
}