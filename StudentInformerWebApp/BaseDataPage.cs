using StudentInformerWebApp.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using StudentInformerWebApp.Models;

namespace StudentInformerWebApp
{
    public class BaseDataPage : System.Web.UI.Page
    {
        private string _loggedInUserId = null;
        public string LoggedInUserId
        {
            get
            {
                if (_loggedInUserId == null)
                {
                    _loggedInUserId = User.Identity.GetUserId();
                }
                return _loggedInUserId;
            }
        }

        private ApplicationUser _loggedInUser = null;
        public ApplicationUser LoggedInUser
        {
            get
            {
                if (_loggedInUser == null)
                {
                    _loggedInUser = UserManager.FindById(LoggedInUserId);
                }
                return _loggedInUser;
            }
        }

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

        private RoleManager<IdentityRole> _roleManager = null;
        public RoleManager<IdentityRole> RoleManager
        {
            get
            {
                if (_roleManager == null)
                {
                    var context = new ApplicationDbContext();
                    var roleStore = new RoleStore<IdentityRole>(context);
                    _roleManager = new RoleManager<IdentityRole>(roleStore);
                }
                return _roleManager;

            }
        }

        private StudentInformerDbContext _databaseContext;
        public StudentInformerDbContext DatabaseContext
        {
            get
            {
                if (_databaseContext == null)
                {
                    _databaseContext = new StudentInformerDbContext();
                }
                return _databaseContext;
            }
        }

        public string GetBaseUrl()
        {
            return Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + "/";
        }

        public string FormatName(string firstName, string lastName)
        {
            return string.Format("{0} {1}", lastName, firstName);
        }
    }
}