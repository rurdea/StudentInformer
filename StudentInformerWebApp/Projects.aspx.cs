using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity;
using StudentInformerWebApp.Models;
using System.IO;

namespace StudentInformerWebApp
{
    public partial class Projects : BaseDataPage
    {

        private bool? _isStudent;
        public bool IsStudent
        {
            get
            {
                if (!_isStudent.HasValue)
                {
                    _isStudent = UserManager.IsInRole(LoggedInUserId, "Admin") || UserManager.IsInRole(LoggedInUserId, "Student");
                }
                return _isStudent.Value;
            }
        }

        private bool? _isProfessor;
        public bool IsProfessor
        {
            get
            {
                if (!_isProfessor.HasValue)
                {
                    _isProfessor = UserManager.IsInRole(LoggedInUserId, "Admin") || UserManager.IsInRole(LoggedInUserId, "Professor");
                }
                return _isProfessor.Value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                InitializeControls();
                LoadProjects();
                if (IsProfessor)
                {
                    LoadStatuses();
                }

                if (IsStudent)
                {
                    LoadReviewers();
                }
            }
        }

        private void InitializeControls()
        {
            AddNewProject.Visible = IsStudent;
            ProjectUpdatePanel.Visible = false;
            CommentsPanel.Visible = false;
            ReviewerCommentPanel.Visible = IsProfessor;
            StudentCommentPanel.Visible = IsStudent;
        }

        #region Load
        private void LoadStatuses()
        {
            CommentNewStatus.Items.Clear();
            var statuses = Enum.GetValues(typeof(ProjectStatus));
            foreach (var status in statuses)
            {
                CommentNewStatus.Items.Add(new ListItem(GetStatusDescription((ProjectStatus)status), ((int)(ProjectStatus)status).ToString()));
            }
        }

        private void LoadReviewers()
        {
            ProjectReviewer.Items.Clear();
            var users = UserManager.Users.ToArray();

            foreach (var user in users)
            {
                if (UserManager.IsInRole(user.Id, "Professor"))
                {
                    ProjectReviewer.Items.Add(new ListItem(string.Format("{0} {1}", user.LastName, user.FirstName), user.Id));
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
                projects = DatabaseContext.Projects.Where(p => p.Reviewer == LoggedInUserId);
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
            CommentsRepeater.DataSource = project != null ? DatabaseContext.ProjectHistories.Where(p => p.ProjectId == project.Id).OrderByDescending(p=>p.ChangeDate).ToArray() : null;
            CommentsRepeater.DataBind();

            // load project update form
            LoadProjectForm(project);

            // load reviewer form
            if (IsProfessor)
            {
                LoadReviewerForm(project);
            }
            if (IsStudent)
            {
                LoadStudentForm(project);
            }
        }

        private void LoadProjectForm(Project project)
        {
            var nameTexbox = ProjectName;
            var submitButton = ProjectSubmit;
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

            var currentStatusLabel = CurrentStatus;
            var newStatusDropDown = CommentNewStatus;
            var gradeTextbox = CommentGrade;
            var commentTextbox = Comment;
            var submitButton = CommentSubmit;

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

        private void LoadStudentForm(Project project)
        {
            if (project == null)
            {
                return;
            }

            StudentCurrentStatus.Text = GetStatusDescription(project.Status);
            StudentComment.Text = string.Empty;
            StudentSubmit.CommandArgument = project.Id.ToString();
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
                deleteButton.Visible = UserManager.IsInRole(LoggedInUserId, "Admin");

                var cssClass = "projects-new";
                switch(project.Status){
                    case ProjectStatus.Complete:
                        cssClass = "projects-complete";
                        break;
                    case ProjectStatus.Created:
                        cssClass = "projects-new";
                        break;
                    case ProjectStatus.Incomplete:
                        cssClass = "projects-incomplete";
                        break;
                }
                e.Row.CssClass = cssClass;
            }

        }

        protected void ProjectsGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            var projectId = new Guid(e.CommandArgument.ToString());
            var project = DatabaseContext.Projects.First(p => p.Id.Equals(projectId));
            switch (e.CommandName)
            {
                case "Select":
                    ProjectActionTitle.Text = project.Name;
                    LoadProject(project);
                    CommentsPanel.Visible = true;
                    StudentCommentPanel.Visible = IsStudent && project.Status!= ProjectStatus.Complete;
                    ProjectUpdatePanel.Visible = false;
                    break;
                case "Download":
                    var fileName = Path.GetFileName(project.PhisicalPath);
                    Response.Clear();
                    Response.ContentType = "application/pdf";
                    Response.AppendHeader("Content-Disposition", string.Format("attachment; filename={0}", fileName));
                    Response.TransmitFile(project.PhisicalPath);
                    Response.End();

                    break;
                case "CustomDelete":
                    DatabaseContext.Projects.Remove(project);
                    var history = DatabaseContext.ProjectHistories.Where(h => h.ProjectId == project.Id).ToArray();
                    if (history!=null && history.Length>0)
                    {
                        DatabaseContext.ProjectHistories.RemoveRange(history);
                    }
                    DatabaseContext.SaveChanges();
                    LoadProjects();
                    CommentsPanel.Visible = false;
                    break;
            }
        }
        #endregion


        #region Save
        protected void UpdateProject_Click(object sender, EventArgs e)
        {
            var button = sender as Button;
            var nameTextbox = ProjectName;
            var reviewerDropdown = ProjectReviewer;
            var fileUpload = ProjectFile;
            var statusLabel = ProjectMessageLabel;

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

                if (isNew)
                {
                    DatabaseContext.Projects.Add(project);
                }
                else
                {
                    DatabaseContext.Entry(project).State = System.Data.Entity.EntityState.Modified;
                }
                DatabaseContext.SaveChanges();

                LoadProjects();

                statusLabel.Text = "Proiectul a fost adaugat cu succes.";
                ProjectUpdatePanel.Visible = false;
                CommentsPanel.Visible = false;
                ProjectActionTitle.Text = string.Empty;
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

            var newStatusDropdown = CommentNewStatus;
            var newStatus = (ProjectStatus)Convert.ToInt16(newStatusDropdown.SelectedValue);
            var gradeTextbox = CommentGrade;
            var commentTextbox = Comment;
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
            projectHistory.ProjectId = projectId;

            DatabaseContext.ProjectHistories.Add(projectHistory);

            // update project status and grade
            project.Grade = gradeTextbox.Text;
            project.Status = newStatus;

            DatabaseContext.Entry(project).State = System.Data.Entity.EntityState.Modified;
            DatabaseContext.SaveChanges();

            LoadProjects();
            LoadProject(project);
        }

        protected void StudentUpdateProject_Click(object sender, EventArgs e)
        {
            var button = sender as Button;
            if (string.IsNullOrWhiteSpace(button.CommandArgument))
            {
                return;
            }

            var projectId = new Guid(button.CommandArgument);
            var project = DatabaseContext.Projects.First(p => p.Id == projectId);

            var fileName = StudentProjectFile.FileName;
            var path = Server.MapPath(".") + "\\ProjectFiles\\" + fileName;
            StudentProjectFile.SaveAs(path);
            var url = GetBaseUrl() + "ProjectFiles/" + fileName;

            project.Url = url;
            var oldStatus = project.Status;
            project.Status = ProjectStatus.Created;
            project.PhisicalPath = path;

            DatabaseContext.Entry(project).State = System.Data.Entity.EntityState.Modified;

            // add history
            var projectHistory = new ProjectHistory();
            projectHistory.Id = Guid.NewGuid();
            projectHistory.ChangeDate = DateTime.Now;
            projectHistory.Comments = StudentComment.Text;
            projectHistory.OldStatus = project.Status;
            projectHistory.NewStatus = oldStatus;
            projectHistory.UserId = LoggedInUserId;
            projectHistory.ProjectId = projectId;

            DatabaseContext.ProjectHistories.Add(projectHistory);

            DatabaseContext.SaveChanges();

            LoadProjects();
            LoadProject(project);
        }
        #endregion

        protected void CommentsRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item ||
                e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var projectHistory = e.Item.DataItem as ProjectHistory;
                var reviewerLabel = (Label)e.Item.FindControl("Reviewer");
                var changeLabel = (Label)e.Item.FindControl("Change");
                var commentLabel = (Label)e.Item.FindControl("Comment");
                var dateLabel = (Label)e.Item.FindControl("Date");

                var reviewer = UserManager.FindById(projectHistory.UserId);

                reviewerLabel.Text = FormatName(reviewer.FirstName, reviewer.LastName);
                dateLabel.Text = projectHistory.ChangeDate.ToString();
                changeLabel.Text = string.Format("Status nou: {0}", GetStatusDescription(projectHistory.NewStatus));
                commentLabel.Text = projectHistory.Comments;
            }
            else if (e.Item.ItemType == ListItemType.Footer)
            {
                ((Label)e.Item.FindControl("EmptyDataLabel")).Visible = CommentsRepeater.Items.Count == 0;
            }
        }

        protected void AddNewProject_Click(object sender, EventArgs e)
        {
            ProjectActionTitle.Text = "Adauga Proiect Nou";
            LoadProjectForm(null);
            ProjectUpdatePanel.Visible = true;
            CommentsPanel.Visible = false;
        }

        protected void ProjectCancel_Click(object sender, EventArgs e)
        {
            ProjectActionTitle.Text = string.Empty;
            ProjectUpdatePanel.Visible = false;
        }
    }
}