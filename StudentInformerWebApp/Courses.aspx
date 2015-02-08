<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Courses.aspx.cs" Inherits="StudentInformerWebApp.Courses" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Panel runat="server">
        <h4>Cursuri Curente</h4>
        <hr />
        <asp:GridView ID="CoursesGrid" runat="server"  AutoGenerateColumns="False" OnRowDataBound="CoursesGrid_RowDataBound" OnRowCommand="CoursesGrid_RowCommand"
            GridLines="None"  
            AllowPaging="false"  
            CssClass="grid"  
            PagerStyle-CssClass="pgr"  
            AlternatingRowStyle-CssClass="alt">
            <Columns>
                <asp:BoundField DataField="Name" HeaderText="Nume" />
                <asp:BoundField DataField="Year" HeaderText="An" />
                <asp:BoundField DataField="Semester" HeaderText="Semestru" />
                <asp:BoundField DataField="Subject" HeaderText="Materie" />
                <asp:BoundField DataField="DateUploaded" HeaderText="Data" />
                <asp:TemplateField HeaderText="Adaugat De">
                    <ItemTemplate>
                        <asp:Label ID="UploadedBy" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="DownloadCount" HeaderText="Numar Descarcari" />
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:LinkButton ID="DownloadButton" runat="server" Text="Descarca" CommandName="Download" />

                        <asp:LinkButton ID="DeleteButton" runat="server" Text="Sterge" CommandName="CustomDelete" OnClientClick="return confirm('Sunteti sigur?')" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </asp:Panel>
    <br />
    <asp:LoginView runat="server" ID="ProfessorLogin"  ViewStateMode="Disabled">
        <RoleGroups>
            <asp:RoleGroup Roles="Professor, Admin">
                <ContentTemplate>
                    <div class="form-horizontal">
                        <section id="accountForm">
                            <asp:PlaceHolder runat="server" ID="account">
                                <div class="form-horizontal">
                                    <h4>Adauga Curs</h4>
                                    <hr />
                                    <asp:Label ID="StatusMessageLabel" runat="server" EnableViewState="false"></asp:Label>
                                    <br/>
                                    <div class="form-group">
                                        <asp:Label runat="server" ID="NameLabel" AssociatedControlID="Name" CssClass="col-md-2 control-label">Nume Curs</asp:Label>
                                        <div class="col-md-10">
                                            <asp:TextBox runat="server" ID="Name" TextMode="SingleLine" CssClass="form-control" />
                                            <asp:RequiredFieldValidator runat="server" ControlToValidate="Name"
                                                CssClass="text-danger" ErrorMessage="Campul este obligatoriu!"
                                                ValidationGroup="Courses" />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <asp:Label runat="server" ID="YearLabel" AssociatedControlID="Year" CssClass="col-md-2 control-label">An Studiu</asp:Label>
                                        <div class="col-md-10">
                                            <asp:TextBox runat="server" ID="Year" TextMode="SingleLine" CssClass="form-control" />
                                            <asp:RequiredFieldValidator runat="server" ControlToValidate="Year"
                                                CssClass="text-danger" ErrorMessage="Campul este obligatoriu!"
                                                ValidationGroup="Courses" />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <asp:Label runat="server" ID="SemesterLabel" AssociatedControlID="Semester" CssClass="col-md-2 control-label">Semestru</asp:Label>
                                        <div class="col-md-10">
                                            <asp:TextBox runat="server" ID="Semester" TextMode="SingleLine" CssClass="form-control" />
                                            <asp:RequiredFieldValidator runat="server" ControlToValidate="Semester"
                                                CssClass="text-danger" ErrorMessage="Campul este obligatoriu!"
                                                ValidationGroup="Courses" />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <asp:Label runat="server" ID="SubjectLabel" AssociatedControlID="Subject" CssClass="col-md-2 control-label">Materie</asp:Label>
                                        <div class="col-md-10">
                                            <asp:TextBox runat="server" ID="Subject" TextMode="SingleLine" CssClass="form-control" />
                                            <asp:RequiredFieldValidator runat="server" ControlToValidate="Subject"
                                                CssClass="text-danger" ErrorMessage="Campul este obligatoriu!"
                                                ValidationGroup="Courses" />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <asp:Label runat="server" ID="FileLabel" AssociatedControlID="File" CssClass="col-md-2 control-label">Fisier</asp:Label>
                                        <div class="col-md-10">
                                            <asp:FileUpload ID="File" runat="server" AllowMultiple="false"/>
                                            <asp:RequiredFieldValidator runat="server" ControlToValidate="File"
                                                CssClass="text-danger" ErrorMessage="Campul este obligatoriu!"
                                                ValidationGroup="Courses" />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-offset-2 col-md-10">
                                            <asp:Button runat="server" Text="Adauga" ValidationGroup="Courses" OnClick="AddCourse_Click" CssClass="btn btn-default" />
                                        </div>
                                    </div>
                                </div>
                            </asp:PlaceHolder>
                        </section>
                    </div>   
                </ContentTemplate>
            </asp:RoleGroup>
        </RoleGroups>
    </asp:LoginView>
</asp:Content>
