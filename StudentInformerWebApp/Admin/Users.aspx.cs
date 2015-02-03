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
    public partial class Users : System.Web.UI.Page
    {
        private ApplicationUserManager _userManager = null;
        public ApplicationUserManager UserManager
        {
            get
            {
                if (_userManager == null)
                {
                    _userManager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
                }
                return _userManager;

            }
        }

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