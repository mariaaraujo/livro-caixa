<%@ Page Title="Cadastro Usuario" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CadastroUsuario.aspx.cs" Inherits="LIvroCaixa2023.Paginas.CadastroUsuario" %>

<asp:Content ID="Content" ContentPlaceHolderID="MainContent" runat="server">
    <div>
        <asp:Button ID="btn_Voltar" runat="server" Text="&larr; Voltar" OnClick="btn_Voltar_Click" />
    </div>
    <div id="mainContentCadastro">
        <div style="margin: auto; text-align: center;">
            <div id="titulo">
                <asp:Label ID="lbl_Titulo" runat="server" />
            </div>
            <div>
                <asp:Label ID="lbl_Nome" runat="server" />
                <asp:TextBox ID="txt_Nome" runat="server" />
            </div>
            <div>
                <asp:Label ID="lbl_Login" runat="server" />
                <asp:TextBox ID="txt_Login" runat="server" />
            </div>
            <div>
                <asp:Label ID="lbl_CPF" runat="server" />
                <asp:TextBox ID="txt_CPF" runat="server" />
            </div>
            <div>
                <asp:Label ID="lbl_Perfil" runat="server" />
                <asp:CheckBox ID="ccb_Adm" runat="server" />
                <asp:Label ID="lbl_Adm" runat="server" Text="Adm" />
                <asp:CheckBox ID="ccb_User" runat="server" />
                <asp:Label ID="lbl_User" runat="server" Text="User" />
            </div>
            <div>
                <asp:Button ID="btn_Salvar" runat="server" OnClick="btn_Salvar_Click" />
                <asp:Button ID="btn_Limpar" runat="server" OnClick="btn_Limpar_Click" />
            </div>

            <div id="message">
                <asp:Label ID="lbl_Mensagem" runat="server" Style="font-size: medium; color: red;" />
                <asp:Label ID="lbl_MensagemSucesso" runat="server" Style="font-size: medium; color: green;" />
            </div>
            <div>
                <asp:Label ID="lbl_BuscaPorId" runat="server" Style="font-size: large" />
                <asp:TextBox ID="txt_IdBusca" runat="server" />
                <asp:Button ID="btn_BuscaId" runat="server" OnClick="btn_BuscaId_Click" />
                <asp:Button ID="btn_Excluir" runat="server" OnClick="btn_Excluir_Click" />
                <asp:Button ID="btn_GerarSenha" runat="server" OnClick="btn_GerarSenha_Click" />
            </div>
        </div>
        <asp:Literal ID="ltl_Tabela" runat="server" />
    </div>
</asp:Content>
