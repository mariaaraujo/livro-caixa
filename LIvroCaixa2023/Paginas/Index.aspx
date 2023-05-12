<%@ Page Title="Login" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="LivroCaixa2023.Paginas.Index" %>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <div id="mainContentIndex">
        <div style="margin: auto; text-align: center;">

            <div id="titulo">
                <asp:Label ID="lbl_Titulo" runat="server" />
            </div>
            <div>
                <asp:Label ID="lbl_Login" runat="server" />
                <asp:TextBox ID="txt_Login" runat="server" />
            </div>
            <div>
                <asp:Label ID="lbl_Senha" runat="server" />
                <asp:TextBox ID="txt_Senha" runat="server" TextMode="Password" />
            </div>
            <div>
                <asp:Button ID="btn_Login" runat="server" OnClick="btn_Login_Click" />
            </div>
            <hr style="width: 30%;" />
            <div>
                <asp:Label ID="lbl_Resposta" runat="server" Style="font-size: medium; margin-top: 40px;" />
            </div>
        </div>

        <asp:Panel runat="server" ID="pnl_Menu" Visible="false">
            <div style="text-align: center;">
                <div>
                    <h2>Menu</h2>
                </div>
                <div>
                    <asp:Button ID="btn_RedirectCadastroUsuario" runat="server" Text="Cadastro de Usuário" ToolTip="Clique aqui para ir ao Cadastro de Usuários" OnClick="btn_RedirectCadastroUsuario_Click" />
                </div>
                <div>
                    <asp:Button ID="btn_RedirectFluxoDeCaixa" runat="server" Text="Fluxo de Caixa" ToolTip="Clique aqui para ir ao Fluxo de Caixa" OnClick="btn_RedirectFluxoDeCaixa_Click" />
                </div>
                <div>
                    <asp:Button ID="btn_Sair" runat="server" Text="Sair" ToolTip="Clique aqui para ir sair" OnClick="btn_Sair_Click" />
                </div>
            </div>
        </asp:Panel>
    </div>
</asp:Content>

