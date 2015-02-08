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
using System.IO;

namespace StudentInformerWebApp
{
    public partial class Schedule : BaseDataPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitializeControls();
                LoadSchedule();
            }
        }

        private void LoadSchedule()
        {
            ViewDiv.InnerHtml = File.ReadAllText(Server.MapPath("Schedule.html"));
        }

        private void InitializeControls()
        {
            ViewPanel.Visible = true;
            EditPanel.Visible = false;
            UpdateButton.Visible = UserManager.IsInRole(LoggedInUserId, "Admin") || UserManager.IsInRole(LoggedInUserId, "Professor");
            UpdateButton.CommandArgument = "update";
        }

        protected void UpdateButton_Click(object sender, EventArgs e)
        {
            switch(UpdateButton.CommandArgument)
            {
                case "update":
                    ViewPanel.Visible = false;
                    EditPanel.Visible = true;
                    UpdateButton.CommandArgument = "save";
                    CKEditor1.Text = File.ReadAllText(Server.MapPath("Schedule.html"));
                    break;
                case "save":
                    ViewPanel.Visible = true;
                    EditPanel.Visible = false;
                    UpdateButton.CommandArgument = "update";
                    File.WriteAllText(Server.MapPath("Schedule.html"), CKEditor1.Text);
                    ViewDiv.InnerHtml = CKEditor1.Text;
                    break;
            }
        } 
    }
}