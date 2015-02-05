using StudentInformerWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using StudentInformerWebApp.DAL;

namespace StudentInformerWebApp
{
    public partial class Courses : BaseDataPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCourses();
            }
        }

        protected void LoadCourses()
        {
            CoursesGrid.DataSource = new StudentInformerWebApp.DAL.StudentInformerDbContext().Courses.ToArray();
            CoursesGrid.DataBind();
        }

        protected void AddCourse_Click(object sender, EventArgs e)
        {
            try
            {
                var course = new Course();
                course.Id = Guid.NewGuid();
                course.Name = ((TextBox)ProfessorLogin.FindControl("Name")).Text;
                course.Year = ((TextBox)ProfessorLogin.FindControl("Year")).Text;
                course.Semester = ((TextBox)ProfessorLogin.FindControl("Semester")).Text;
                course.Subject = ((TextBox)ProfessorLogin.FindControl("Subject")).Text;

                var fileUpload = (FileUpload)ProfessorLogin.FindControl("File");
                var fileName = fileUpload.FileName;
                string path = Server.MapPath(".") + "\\CourseFiles\\" + fileName;
                fileUpload.SaveAs(path);
                course.PhisicalPath = path;
                course.Url = GetBaseUrl() + "CourseFiles/" + fileName;
                course.UploadedBy = User.Identity.GetUserId();
                course.DateUploaded = DateTime.Now;

                DatabaseContext.Courses.Add(course);
                DatabaseContext.SaveChanges();

                OnAddSucceeded();
            }
            catch (Exception ex)
            {
                var err = new CustomValidator();
                err.ValidationGroup = "Courses";
                err.IsValid = false;
                err.ErrorMessage = ex.ToString();
                Page.Validators.Add(err);
            }
        }

        private void OnAddSucceeded()
        {
            ((TextBox)ProfessorLogin.FindControl("Name")).Text = string.Empty;
            ((TextBox)ProfessorLogin.FindControl("Year")).Text = string.Empty;
            ((TextBox)ProfessorLogin.FindControl("Semester")).Text = string.Empty;
            ((TextBox)ProfessorLogin.FindControl("Subject")).Text = string.Empty;
            ((Label)ProfessorLogin.FindControl("StatusMessageLabel")).Text = "Curs adaugat cu succes.";

            LoadCourses();

        }

        protected void CoursesGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var course = (Course)e.Row.DataItem;

                var uploadedByUser = UserManager.FindById(course.UploadedBy);
                ((Label)e.Row.FindControl("UploadedBy")).Text = string.Format("{0} {1}", uploadedByUser.FirstName, uploadedByUser.LastName);

                var downloadButton = ((LinkButton)e.Row.FindControl("DownloadButton"));
                downloadButton.CommandArgument = course.Id.ToString();

                var deleteButton = ((LinkButton)e.Row.FindControl("DeleteButton"));
                deleteButton.CommandArgument = course.Id.ToString();
                if (UserManager.IsInRole(User.Identity.GetUserId(), "Admin") ||
                    User.Identity.GetUserId() == course.UploadedBy)
                {
                    deleteButton.Visible = true;
                }
                else
                {
                    deleteButton.Visible = false;
                }
            }
            
        }

        protected void CoursesGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            var courseId = new Guid(e.CommandArgument.ToString());
            var course = DatabaseContext.Courses.First(c => c.Id.Equals(courseId));
            switch (e.CommandName)
            {
                case "Download":
                    course.DownloadCount++;
                    DatabaseContext.SaveChanges();
                    Response.Redirect(course.Url, false);
                    break;
                case "CustomDelete":
                    DatabaseContext.Courses.Remove(course);
                    DatabaseContext.SaveChanges();
                    LoadCourses();
                    break;
            }
        }
    }
}