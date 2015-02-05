using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity;
using StudentInformerWebApp.Models;

namespace StudentInformerWebApp
{
    public partial class Projects : BaseDataPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LoadProjects();
                if (UserManager.IsInRole(LoggedInUserId, "Admin") ||
                    UserManager.IsInRole(LoggedInUserId, "Professor"))
                {
                    LoadStatuses();
                }

                if (UserManager.IsInRole(LoggedInUserId, "Admin") ||
                    UserManager.IsInRole(LoggedInUserId, "Student"))
                {
                    LoadReviewers();
                }
            }
        }

        #region Load
        private void LoadStatuses()
        {
            var statusDropdown = (DropDownList)ProfessorLogin.FindControl("NewStatus");
            statusDropdown.Items.Clear();
            var statuses = Enum.GetValues(typeof(ProjectStatus));
            foreach (var status in statuses)
            {
                statusDropdown.Items.Add(new ListItem(GetStatusDescription((ProjectStatus)status), ((int)(ProjectStatus)status).ToString()));
            }
        }

        private void LoadReviewers()
        {
            var reviewerDropdown = (DropDownList)StudentLogin.FindControl("Reviewer");
            reviewerDropdown.Items.Clear();
            var users = UserManager.Users.ToArray();

            foreach (var user in users)
            {
                if (UserManager.IsInRole(user.Id, "Professor"))
                {
                    reviewerDropdown.Items.Add(new ListItem(string.Format("{0} {1}", user.LastName, user.FirstName), user.Id));
                }
            }
        }

        private void LoadProjects()
        {
            IQueryable<Project> projects = null;
            if (UserManager.IsInRole(LoggedInUserId, "Admin"))
            {
                projects = DatabaseContext.Projects.AsQueryable();
            }
            else if (UserManager.IsInRole(LoggedInUserId, "Professor"))
            {
                projects = DatabaseContext.Projects.Where(p=>p.Reviewer==LoggedInUserId);
            }
            else if (UserManager.IsInRole(LoggedInUserId, "Student"))
            {
                projects = DatabaseContext.Projects.Where(p => p.Author == LoggedInUserId);
            }
            if (projects != null)
            {
                projects = projects.OrderBy(p => p.DateUploaded);
                ProjectsGrid.DataSource = projects.ToArray();
                ProjectsGrid.DataBind();
            }
            LoadProject(null);
        }

        private void LoadProject(Project project)
        {
            // load comments
            CommentsRepeater.DataSource = project != null ? DatabaseContext.ProjectHistories.Where(p => p.ProjectId == project.Id).ToArray() : null;
            CommentsRepeater.DataBind();

            // load project update form
            if (UserManager.IsInRole(LoggedInUserId, "Admin")||
                UserManager.IsInRole(LoggedInUserId, "Student"))
            {
                if (project == null ||
                    project.Status == ProjectStatus.Incomplete)
                {
                    LoadProjectForm(project);
                }
                else
                {
                    ProjectsMessageLabel.Text = "Proiectul selectat nu necesita schimbari.";
                }
                
            }

            // load reviewer form
            if (UserManager.IsInRole(LoggedInUserId, "Admin") ||
                UserManager.IsInRole(LoggedInUserId, "Professor"))
            {
                LoadReviewerForm(project);
            }
        }

        private void LoadProjectForm(Project project)
        {
            var nameTexbox = (TextBox)StudentLogin.FindControl("Name");
            var submitButton = (Button)StudentLogin.FindControl("Submit");
            if (project == null)
            {
                nameTexbox.Text = string.Empty;
                submitButton.Text = "Adauga";
                submitButton.CommandArgument = string.Empty;
            }
            else
            {
                nameTexbox.Text = project.Name;
                submitButton.Text = "Actualizeaza";
                submitButton.CommandArgument = project.Id.ToString();
            }
        }

        private void LoadReviewerForm(Project project)
        {
            if (project == null)
            {
                return;
            }

            var currentStatusLabel = (Label)ProfessorLogin.FindControl("CurrentStatus");
            var newStatusDropDown = (DropDownList)ProfessorLogin.FindControl("NewStatus");
            var gradeTextbox = (TextBox)ProfessorLogin.FindControl("Grade");
            var commentTextbox = (TextBox)ProfessorLogin.FindControl("Comment");
            var submitButton = (Button)ProfessorLogin.FindControl("Submit");

            currentStatusLabel.Text = GetStatusDescription(project.Status);
            gradeTextbox.Text = project.Grade;
            foreach (ListItem item in newStatusDropDown.Items)
            {
                if (item.Value == ((int)project.Status).ToString())
                {
                    item.Enabled = false;
                }
                else
                {
                    item.Enabled = true;
                }
            }
            commentTextbox.Text = string.Empty;

            submitButton.CommandArgument = project.Id.ToString();

        }

        private string GetStatusDescription(ProjectStatus projectStatus)
        {
            switch (projectStatus)
            {
                case ProjectStatus.Complete:
                    return "Complet";
                case ProjectStatus.Created:
                    return "Nou";
                case ProjectStatus.Incomplete:
                    return "Incomplet";
            }
            return "Necunoscut";
        }

        protected void ProjectsGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var project = (Project)e.Row.DataItem;

                var author = UserManager.FindById(project.Author);
                ((Label)e.Row.FindControl("Author")).Text = string.Format("{0} {1}", author.LastName, author.FirstName);

                var reviewer = UserManager.FindById(project.Reviewer);
                ((Label)e.Row.FindControl("Reviewer")).Text = string.Format("{0} {1}", reviewer.LastName, reviewer.FirstName);

                ((Label)e.Row.FindControl("Status")).Text = GetStatusDescription(project.Status);

                var selectButton = (LinkButton)e.Row.FindControl("SelectButton");
                selectButton.CommandArgument = project.Id.ToString();

                var downloadButton = ((LinkButton)e.Row.FindControl("DownloadButton"));
                downloadButton.CommandArgument = project.Id.ToString();

                var deleteButton = ((LinkButton)e.Row.FindControl("DeleteButton"));
                deleteButton.CommandArgument = project.Id.ToString();
                deleteButton.Visible = false; // false for now
                
            }

        }

        protected void ProjectsGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            var projectId = new Guid(e.CommandArgument.ToString());
            var project = DatabaseContext.Projects.First(p => p.Id.Equals(projectId));
            switch (e.CommandName)
            {
                case "Select":
                    LoadProject(project);
                    break;
                case "Download":
                    Response.Redirect(project.Url, false);
                    break;
                case "CustomDelete":
                    DatabaseContext.Projects.Remove(project);
                    DatabaseContext.SaveChanges();
                    LoadProjects();
                    break;
            }
        }
        #endregion


        #region Save
        protected void UpdateProject_Click(object sender, EventArgs e)
        {
            var button = sender as Button;
            var nameTextbox = (TextBox)StudentLogin.FindControl("Name");
            var reviewerDropdown = (DropDownList)StudentLogin.FindControl("Reviewer");
            var fileUpload = (FileUpload)StudentLogin.FindControl("File");
            var statusLabel = (Label)StudentLogin.FindControl("ProjectMessageLabel");

            try
            {
                var fileName = fileUpload.FileName;
                var path = Server.MapPath(".") + "\\ProjectFiles\\" + fileName;
                fileUpload.SaveAs(path);
                var url = GetBaseUrl() + "ProjectFiles/" + fileName;

                var project = default(Project);
                var isNew = true;
                if (!string.IsNullOrWhiteSpace(button.CommandArgument))
                {// existing project
                    var projectId = new Guid(button.CommandArgument);
                    project = DatabaseContext.Projects.First(p => p.Id == projectId);
                    isNew = false;
                }
                else
                {// new project
                    project = new Project();
                    project.Id = Guid.NewGuid();
                }
                project.Name = nameTextbox.Text;
                project.Reviewer = reviewerDropdown.SelectedValue;
                project.DateUploaded = DateTime.Now;
                project.Author = LoggedInUserId;
                project.Url = url;
                project.Status = ProjectStatus.Created;
                project.PhisicalPath = path;
                
                if (isNew){
                    DatabaseContext.Projects.Add(project);
                }
                else
                {
                    DatabaseContext.Entry(project).State = System.Data.Entity.EntityState.Modified;
                }
                DatabaseContext.SaveChanges();

                LoadProjects();
            }
            catch (Exception ex)
            {
                statusLabel.Text = "Proiectul nu a putut fi adaugat. " + ex.ToString();
            }
        }

        protected void UpdateProjectHistory_Click(object sender, EventArgs e)
        {
            var button = sender as Button;
            if (string.IsNullOrWhiteSpace(button.CommandArgument))
            {
                return;
            }

            var statusLabel = (Label)ProfessorLogin.FindControl("ReviewMessageLabel");
            var newStatusDropdown = (DropDownList)ProfessorLogin.FindControl("NewStatus");
            var newStatus =  (ProjectStatus)Convert.ToInt16(newStatusDropdown.SelectedValue);
            var gradeTextbox = (TextBox)ProfessorLogin.FindControl("Grade");
            var commentTextbox = (TextBox)ProfessorLogin.FindControl("Comment");
            var projectId = new Guid(button.CommandArgument);
            var project = DatabaseContext.Projects.First(p => p.Id == projectId);

            

            // add history
            var projectHistory = new ProjectHistory();
            projectHistory.Id = Guid.NewGuid();
            projectHistory.ChangeDate = DateTime.Now;
            projectHistory.Comments = commentTextbox.Text;
            projectHistory.OldStatus = project.Status;
            projectHistory.NewStatus = newStatus;
            projectHistory.UserId = LoggedInUserId;

            DatabaseContext.ProjectHistories.Add(projectHistory);

            // update project status and grade
            project.Grade = gradeTextbox.Text;
            project.Status = newStatus;

            DatabaseContext.Entry(project).State = System.Data.Entity.EntityState.Modified;

            DatabaseContext.SaveChanges();

            LoadProjects();
        }
        #endregion
    }
}