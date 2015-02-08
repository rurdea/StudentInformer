<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Schedule.aspx.cs" Inherits="StudentInformerWebApp.Schedule" %>
<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %> 
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h4>Orar</h4>
    <hr />
    <asp:Button ID="UpdateButton" runat="server" CssClass="btn btn-default" OnClick="UpdateButton_Click" Text="Actualizeaza" />
    <asp:Panel ID="ViewPanel" runat="server">
        <div id="ViewDiv" runat="server" enableviewstate="false"></div>
    </asp:Panel>
    <asp:Panel ID="EditPanel" runat="server">
        <div>
           <CKEditor:CKEditorControl ID="CKEditor1" BasePath="~/ckeditor/" runat="server">
           </CKEditor:CKEditorControl>
        </div>
    </asp:Panel>
</asp:Content>
