<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Users.aspx.cs" Inherits="StudentInformerWebApp.Admin.Users" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Administrare Useri</h2>
    <p>
        <asp:GridView ID="UserList" runat="server" AutoGenerateColumns="False"
             GridLines="None"  
            AllowPaging="false"  
            CssClass="grid"  
            PagerStyle-CssClass="pgr"  
            AlternatingRowStyle-CssClass="alt">
            <Columns>
                <asp:HyperLinkField Text="Editeaza" DataNavigateUrlFormatString="~/Account/Manage.aspx?u={0}" DataNavigateUrlFields="Id" />
                <asp:BoundField DataField="UserName" HeaderText="Username" />
                <asp:BoundField DataField="Email" HeaderText="Adresa Email" />
                <asp:BoundField DataField="LastName" HeaderText="Nume" />
                <asp:BoundField DataField="FirstName" HeaderText="Prenume" />
            </Columns>
        </asp:GridView>
    </p>
</asp:Content>
