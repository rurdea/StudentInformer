<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Projects.aspx.cs" Inherits="StudentInformerWebApp.Projects" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Panel runat="server" ID="ProjectsPanel">
        <div class="form-horizontal">
            <section id="projectForm">
                    <div class="form-horizontal">
                        <h4>Proiecte Curente</h4>
                        <hr />
                        <asp:Label ID="ProjectsMessageLabel" runat="server" EnableViewState="false"></asp:Label>
                        <br/>
                        <div class="form-group">
                            <asp:GridView ID="ProjectsGrid" runat="server"  AutoGenerateColumns="False" OnRowDataBound="ProjectsGrid_RowDataBound" OnRowCommand="ProjectsGrid_RowCommand">
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="SelectButton" runat="server" Text="Selecteaza" CommandName="Select"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Name" HeaderText="Nume" />
                                    <asp:TemplateField HeaderText="Autor">
                                        <ItemTemplate>
                                            <asp:Label ID="Author" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Profesor">
                                        <ItemTemplate>
                                            <asp:Label ID="Reviewer" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="DateUploaded" HeaderText="Data" />
                                    <asp:TemplateField HeaderText="Status">
                                        <ItemTemplate>
                                            <asp:Label ID="Status" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Grade" HeaderText="Nota" />
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="DownloadButton" runat="server" Text="Descarca" CommandName="Download" />

                                            <asp:LinkButton ID="DeleteButton" runat="server" Text="Sterge" CommandName="CustomDelete" OnClientClick="return confirm('Sunteti sigur?')" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </section>
            </div>
    </asp:Panel>
    <asp:HiddenField ID="CurrentProjectId" runat="server" />
    <asp:Panel runat="server" ID="CommentsPanel">
        <asp:Repeater ID="CommentsRepeater" runat="server">
            <HeaderTemplate>
                <table>
                    <thead>

                    </thead>
                    <tbody>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td>
                        <asp:Label ID="CommentLabel" runat="server"></asp:Label>
                    </td>
                </tr>
            </ItemTemplate>
            <SeparatorTemplate>

            </SeparatorTemplate>
            <FooterTemplate>
                    </tbody>
                </table>
            </FooterTemplate>
        </asp:Repeater>
    </asp:Panel>
    <asp:Panel runat="server" ID="UpdatePanel">
        <asp:LoginView runat="server" ID="StudentLogin"  ViewStateMode="Enabled">
            <RoleGroups>
                <asp:RoleGroup Roles="Admin,Student">
                    <ContentTemplate>
                        <div class="form-horizontal">
                            <section id="projectForm">
                                    <div class="form-horizontal">
                                        <h4>Adauga/Actualizeaza Proiect</h4>
                                        <hr />
                                        <asp:Label ID="ProjectMessageLabel" runat="server" EnableViewState="false"></asp:Label>
                                        <br/>
                                        <div class="form-group">
                                            <asp:Label runat="server" ID="NameLabel" AssociatedControlID="Name" CssClass="col-md-2 control-label">Nume Proiect</asp:Label>
                                            <div class="col-md-10">
                                                <asp:TextBox runat="server" ID="Name" TextMode="SingleLine" CssClass="form-control" />
                                                <asp:RequiredFieldValidator runat="server" ControlToValidate="Name"
                                                    CssClass="text-danger" ErrorMessage="Campul este obligatoriu!"
                                                    ValidationGroup="Projects" />
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <asp:Label runat="server" ID="ReviewerLabel" AssociatedControlID="Reviewer" CssClass="col-md-2 control-label">Indrumator</asp:Label>
                                            <div class="col-md-10">
                                                <asp:DropDownList ID="Reviewer" runat="server" EnableViewState="true"></asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <asp:Label runat="server" ID="FileLabel" AssociatedControlID="File" CssClass="col-md-2 control-label">Fisier</asp:Label>
                                            <div class="col-md-10">
                                                <asp:FileUpload ID="File" runat="server" AllowMultiple="false"/>
                                                <asp:RequiredFieldValidator runat="server" ControlToValidate="File"
                                                    CssClass="text-danger" ErrorMessage="Campul este obligatoriu!"
                                                    ValidationGroup="Projects" />
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <div class="col-md-offset-2 col-md-10">
                                                <asp:Button runat="server" ID="Submit" Text="Adauga" ValidationGroup="Projects" OnClick="UpdateProject_Click" CssClass="btn btn-default" />
                                            </div>
                                        </div>
                                    </div>
                            </section>
                        </div>   
                     </ContentTemplate>
                </asp:RoleGroup>
         </RoleGroups>
        </asp:LoginView>
    </asp:Panel>
    <asp:Panel runat="server" ID="ReviewPanel">
        <asp:LoginView runat="server" ID="ProfessorLogin"  ViewStateMode="Enabled">
            <RoleGroups>
                <asp:RoleGroup Roles="Admin,Professor">
                    <ContentTemplate>
                        <div class="form-horizontal">
                            <section id="reviewerForm">
                                    <div class="form-horizontal">
                                        <h4>Actualizeaza Status Proiect</h4>
                                        <hr />
                                        <asp:Label ID="ReviewMessageLabel" runat="server" EnableViewState="false"></asp:Label>
                                        <br/>
                                        <div class="form-group">
                                            <asp:Label runat="server" ID="CurrentStatusLabel" AssociatedControlID="CurrentStatus" CssClass="col-md-2 control-label">Status Curent</asp:Label>
                                            <div class="col-md-10">
                                                <asp:Label runat="server" ID="CurrentStatus"></asp:Label>
                                            </div>
                                        </div>
                                         <div class="form-group">
                                            <asp:Label runat="server" ID="NewStatusLabel" AssociatedControlID="NewStatus" CssClass="col-md-2 control-label">Status Nou</asp:Label>
                                            <div class="col-md-10">
                                                <asp:DropDownList ID="NewStatus" runat="server"></asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <asp:Label runat="server" ID="GradeLabel" AssociatedControlID="Grade" CssClass="col-md-2 control-label">Nota</asp:Label>
                                            <div class="col-md-10">
                                                <asp:TextBox runat="server" ID="Grade" TextMode="SingleLine" CssClass="form-control" />
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <asp:Label runat="server" ID="CommentLabel" AssociatedControlID="Comment" CssClass="col-md-2 control-label">Comentariu</asp:Label>
                                            <div class="col-md-10">
                                                <asp:TextBox runat="server" ID="Comment" TextMode="MultiLine" CssClass="form-control" />
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <div class="col-md-offset-2 col-md-10">
                                                <asp:Button runat="server" ID="Submit" Text="Adauga" ValidationGroup="ProjectHistory" OnClick="UpdateProjectHistory_Click" CssClass="btn btn-default" />
                                            </div>
                                        </div>
                                    </div>
                            </section>
                        </div>   
                    </ContentTemplate>
                </asp:RoleGroup>
         </RoleGroups>
        </asp:LoginView>
    </asp:Panel>
</asp:Content>
