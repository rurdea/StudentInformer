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
using Owin;

namespace StudentInformerWebApp.Admin
{
    public partial class Users : BaseDataPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
                DisplayUsersInGrid();
        }

        private void DisplayUsersInGrid()
        {
            UserList.DataSource = UserManager.Users.ToArray();
            UserList.DataBind();
        }
    }
}