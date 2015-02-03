using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Owin;
using StudentInformerWebApp.Models;

namespace StudentInformerWebApp.Account
{
    public partial class Manage : System.Web.UI.Page
    {
        protected string SuccessMessage
        {
            get;
            private set;
        }

        protected string CurrentUserId
        {
            get
            {
                return ViewState["CurrentUserId"] as string;
            }
            private set
            {
                ViewState["CurrentUserId"] = value;
            }
        }

        private bool HasPassword(ApplicationUserManager manager)
        {
            return manager.HasPassword(CurrentUserId);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();

            if (!IsPostBack)
            {
                var userId = Request.QueryString["u"];
                // set the updating user id
                CurrentUserId = userId ?? User.Identity.GetUserId();

                // Determine the sections to render
                if (HasPassword(manager))
                {
                    changePasswordHolder.Visible = true;
                }

                // Render success message
                var message = Request.QueryString["m"];
                if (message != null)
                {
                    // Strip the query string from action
                    Form.Action = ResolveUrl("~/Account/Manage");

                    SuccessMessage =
                        message == "ChangePwdSuccess" ? "Parola a fost schimbata cu succes."
                        : message == "UpdateSuccess" ? "Informatiile contului au fost salvate cu succes."
                        : String.Empty;
                    successMessage.Visible = !String.IsNullOrEmpty(SuccessMessage);
                }

                LoadAccountDetails(manager);
            }
        }

        private void LoadAccountDetails(ApplicationUserManager manager)
        {
            var user = manager.FindById(CurrentUserId);
            FirstName.Text = user.FirstName;
            LastName.Text = user.LastName;
            UserName.Text = user.UserName;
            Email.Text = user.Email;
        }

        protected void UpdateAccount_Click(object sender, EventArgs e)
        {
            if (IsValid)
            {
                var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var user = manager.FindById(CurrentUserId);
                user.UserName = UserName.Text;
                user.Email = Email.Text;
                user.FirstName = FirstName.Text;
                user.LastName = LastName.Text;
                IdentityResult result = manager.Update(user);
                if (result.Succeeded)
                {
                    Response.Redirect("~/Account/Manage?m=UpdateSuccess&u=" + CurrentUserId);
                }
                else
                {
                    AddErrors(result);
                }
            }
        }

        protected void ChangePassword_Click(object sender, EventArgs e)
        {
            if (IsValid)
            {
                var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
                IdentityResult result = manager.ChangePassword(CurrentUserId, CurrentPassword.Text, NewPassword.Text);
                if (result.Succeeded)
                {
                    if (CurrentUserId.Equals(User.Identity.GetUserId()))
                    {
                        var signInManager = Context.GetOwinContext().Get<ApplicationSignInManager>();
                        var user = manager.FindById(CurrentUserId);
                        signInManager.SignIn(user, isPersistent: false, rememberBrowser: false);
                    }
                    Response.Redirect("~/Account/Manage?m=ChangePwdSuccess&u=" + CurrentUserId);
                }
                else
                {
                    AddErrors(result);
                }
            }
        }


        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
    }
}