<%@ Page Title="Administreaza Cont" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Manage.aspx.cs" Inherits="StudentInformerWebApp.Account.Manage" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %></h2>
    <div>
        <asp:PlaceHolder runat="server" ID="successMessage" Visible="false" ViewStateMode="Disabled">
            <p class="text-success"><%: SuccessMessage %></p>
        </asp:PlaceHolder>
    </div>

    <asp:ValidationSummary runat="server" ShowModelStateErrors="true" CssClass="text-danger" />

    <div class="form-horizontal">
        <section id="accountForm">
            <asp:PlaceHolder runat="server" ID="account">
                <div class="form-horizontal">
                    <h4>Detalii Cont</h4>
                    <hr />
                    <div class="form-group">
                        <asp:Label runat="server" ID="UserNameLabel" AssociatedControlID="UserName" CssClass="col-md-2 control-label">Nume Utilizator</asp:Label>
                        <div class="col-md-10">
                            <asp:TextBox runat="server" ID="UserName" TextMode="SingleLine" CssClass="form-control" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="UserName"
                                CssClass="text-danger" ErrorMessage="Campul nume utilizator este obligatoriu!"
                                ValidationGroup="Account" />
                        </div>
                    </div>
                    <div class="form-group">
                        <asp:Label runat="server" ID="EmailLabel" AssociatedControlID="Email" CssClass="col-md-2 control-label">Adresa Email</asp:Label>
                        <div class="col-md-10">
                            <asp:TextBox runat="server" ID="Email" TextMode="Email" CssClass="form-control" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="Email"
                                CssClass="text-danger" ErrorMessage="Campul adresa email este obligatoriu!"
                                ValidationGroup="Account" />
                        </div>
                    </div>
                    <div class="form-group">
                        <asp:Label runat="server" ID="LastNameLabel" AssociatedControlID="LastName" CssClass="col-md-2 control-label">Nume</asp:Label>
                        <div class="col-md-10">
                            <asp:TextBox runat="server" ID="LastName" TextMode="SingleLine" CssClass="form-control" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="LastName"
                                CssClass="text-danger" ErrorMessage="Campul nume este obligatoriu!"
                                ValidationGroup="Account" />
                        </div>
                    </div>
                    <div class="form-group">
                        <asp:Label runat="server" ID="FirstNameLabel" AssociatedControlID="FirstName" CssClass="col-md-2 control-label">Prenume</asp:Label>
                        <div class="col-md-10">
                            <asp:TextBox runat="server" ID="FirstName" TextMode="SingleLine" CssClass="form-control" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="FirstName"
                                CssClass="text-danger" ErrorMessage="Campul prenume este obligatoriu!"
                                ValidationGroup="Account" />
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-offset-2 col-md-10">
                            <asp:Button runat="server" Text="Actualizeaza" ValidationGroup="Account" OnClick="UpdateAccount_Click" CssClass="btn btn-default" />
                        </div>
                    </div>
                </div>
            </asp:PlaceHolder>
        </section>
        <section id="passwordForm">
            <asp:PlaceHolder runat="server" ID="changePasswordHolder" Visible="false">
                <div class="form-horizontal">
                    <h4>Schimba Parola</h4>
                    <hr />
                    <%--<asp:ValidationSummary runat="server" ShowModelStateErrors="true" CssClass="text-danger" />--%>
                    <div class="form-group">
                        <asp:Label runat="server" ID="CurrentPasswordLabel" AssociatedControlID="CurrentPassword" CssClass="col-md-2 control-label">Parola Curenta</asp:Label>
                        <div class="col-md-10">
                            <asp:TextBox runat="server" ID="CurrentPassword" TextMode="Password" CssClass="form-control" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="CurrentPassword"
                                CssClass="text-danger" ErrorMessage="Va rugam introduceti parola curenta."
                                ValidationGroup="ChangePassword" />
                        </div>
                    </div>
                    <div class="form-group">
                        <asp:Label runat="server" ID="NewPasswordLabel" AssociatedControlID="NewPassword" CssClass="col-md-2 control-label">Parola Noua</asp:Label>
                        <div class="col-md-10">
                            <asp:TextBox runat="server" ID="NewPassword" TextMode="Password" CssClass="form-control" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="NewPassword"
                                CssClass="text-danger" ErrorMessage="Va rugam introduceti parola noua."
                                ValidationGroup="ChangePassword" />
                        </div>
                    </div>
                    <div class="form-group">
                        <asp:Label runat="server" ID="ConfirmNewPasswordLabel" AssociatedControlID="ConfirmNewPassword" CssClass="col-md-2 control-label">Confirmare Parola Noua</asp:Label>
                        <div class="col-md-10">
                            <asp:TextBox runat="server" ID="ConfirmNewPassword" TextMode="Password" CssClass="form-control" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="ConfirmNewPassword"
                                CssClass="text-danger" Display="Dynamic" ErrorMessage="Va rugam confirmati parola."
                                ValidationGroup="ChangePassword" />
                            <asp:CompareValidator runat="server" ControlToCompare="NewPassword" ControlToValidate="ConfirmNewPassword"
                                CssClass="text-danger" Display="Dynamic" ErrorMessage="Cele doua parola nu coincid!"
                                ValidationGroup="ChangePassword" />
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-offset-2 col-md-10">
                            <asp:Button runat="server" Text="Schimba Parola" ValidationGroup="ChangePassword" OnClick="ChangePassword_Click" CssClass="btn btn-default" />
                        </div>
                    </div>
                </div>
            </asp:PlaceHolder>
        </section>
    </div>
</asp:Content>
