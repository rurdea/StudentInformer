<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Roles.aspx.cs" Inherits="StudentInformerWebApp.Admin.Roles" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Administrare Roluri</h2>
    <p>
        <b>Rol Nou: </b>
        <asp:TextBox ID="RoleName" runat="server"></asp:TextBox>
        <asp:RequiredFieldValidator ID="RoleNameReqField" runat="server" 
            ControlToValidate="RoleName" Display="Dynamic" 
            ErrorMessage="Va rugam introduceti un nume."></asp:RequiredFieldValidator>
        
        <br />
        <asp:Button ID="CreateRoleButton" runat="server" Text="Creeaza Rol" 
            onclick="CreateRoleButton_Click" />
    </p>
    <p>
        <asp:GridView ID="RoleList" runat="server" AutoGenerateColumns="False" 
            onrowdeleting="RoleList_RowDeleting">
            <Columns>
                <asp:CommandField DeleteText="Sterge Rol" ShowDeleteButton="True" />
                <asp:TemplateField HeaderText="Nume Rol">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="RoleNameLabel" Text='<%#  Eval("Name") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </p>
</asp:Content>
