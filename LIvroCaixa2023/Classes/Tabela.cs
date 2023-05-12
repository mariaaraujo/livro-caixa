using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LivroCaixa2023.Tabelas
{

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web;


    public class Tabela
    {
        // private int linhas;
        private int colunas;

        private string titulo;

        public string[,] celula;

        public Tabela(int linhas, int colunas, String titulo)
        {
            celula = new string[linhas, colunas];
            // this.linhas = linhas;
            this.colunas = colunas;
            this.titulo = titulo;
        }

        public string tabela(string[] titulos, int linhas)
        {
            StringBuilder str = new StringBuilder("<meta charset='UTF-8'/>");
            str.Append("<table class='blueTable' style='margin: auto;'>");

            if (titulo != null && titulo != String.Empty)
            {
                //str.Append("<TR><td colspan='" + colunas + "'></td></TR>");
                str.Append("<tr><td style='text-align:center;font-size:1.5rem;font-weight:bold;' colspan='" + colunas + "'>" + titulo + "</td></tr>");
            }

            str.Append("<tr><td colspan='" + colunas + "'></td></tr>");
            str.Append("<tr>");
            for (int col = 0; col < colunas; col++)
            {
                str.Append("<td>&nbsp;<B>" + titulos[col] + "</B>&nbsp;</td>");
            }
            str.Append("</tr>");
            str.Append("<tr><td colspan='" + colunas + "'></td></tr>");
            for (int lin = 0; lin < linhas; lin++)
            {
                str.Append("<tr>");
                for (int col = 0; col < colunas; col++)
                {
                    double valor;
                    if (Double.TryParse(celula[lin, col].Replace(",", "."), out valor))
                    {
                        str.Append("<td style='text-align:right'>&nbsp;" + celula[lin, col] + "&nbsp;</td>");
                    }
                    else
                    {
                        str.Append("<td>&nbsp;" + celula[lin, col] + "&nbsp;</td>");
                    }
                }
                str.Append("</tr>");
            }
            str.Append("<tr><td colspan='" + colunas + "'></td></tr>");
            str.Append("</table>");
            return str.ToString();
        }
    }

}