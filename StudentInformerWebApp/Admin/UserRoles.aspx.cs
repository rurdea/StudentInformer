using StudentInformerWebApp.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Owin;

namespace StudentInformerWebApp.Admin
{
    public partial class UserRoles : BaseDataPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                // Bind the users and roles
                BindUsersToUserList();
                BindRolesToList();

                // Check the selected user's roles
                CheckRolesForSelectedUser();

                // Display those users belonging to the currently selected role
                DisplayUsersBelongingToRole();
            }
        }

        private void BindRolesToList()
        {
            // Get all of the roles
            string[] roles = RoleManager.Roles.Select(r => r.Name).ToArray();
            UsersRoleList.DataSource = roles;
            UsersRoleList.DataBind();

            RoleList.DataSource = roles;
            RoleList.DataBind();
        }

        #region 'By User' Interface-Specific Methods
        private void BindUsersToUserList()
        {
            UserList.DataSource = UserManager.Users.ToList();
            UserList.DataBind();
        }

        protected void UserList_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckRolesForSelectedUser();
        }

        private void CheckRolesForSelectedUser()
        {
            // Determine what roles the selected user belongs to
            string selectedUserName = UserList.SelectedValue;


            string[] selectedUsersRoles = UserManager.FindByName(selectedUserName).Roles.Select(r => RoleManager.FindById(r.RoleId).Name).ToArray();

            // Loop through the Repeater's Items and check or uncheck the checkbox as needed
            foreach (RepeaterItem ri in UsersRoleList.Items)
            {
                // Programmatically reference the CheckBox
                CheckBox RoleCheckBox = ri.FindControl("RoleCheckBox") as CheckBox;

                // See if RoleCheckBox.Text is in selectedUsersRoles
                if (selectedUsersRoles.Contains<string>(RoleCheckBox.Text))
                    RoleCheckBox.Checked = true;
                else
                    RoleCheckBox.Checked = false;
            }
        }

        protected void RoleCheckBox_CheckChanged(object sender, EventArgs e)
        {
            // Reference the CheckBox that raised this event
            CheckBox RoleCheckBox = sender as CheckBox;

            // Get the currently selected user and role
            string selectedUserName = UserList.SelectedValue;
            string roleName = RoleCheckBox.Text;

            // Determine if we need to add or remove the user from this role
            if (RoleCheckBox.Checked)
            {
                // Add the user to the role
                UserManager.AddToRole(UserManager.FindByName(selectedUserName).Id, roleName);
                
                // Display a status message
                ActionStatus.Text = string.Format("Userul {0} a fost adaugat la Rolul {1}.", selectedUserName, roleName);
            }
            else
            {
                // Remove the user from the role
                UserManager.RemoveFromRole(UserManager.FindByName(selectedUserName).Id, roleName);

                // Display a status message
                ActionStatus.Text = string.Format("Userul {0} a fost sters de la Rolul {1}.", selectedUserName, roleName);
            }

            // Refresh the "by role" interface
            DisplayUsersBelongingToRole();
        }
        #endregion

        #region 'By Role' Interface-Specific Methods
        protected void RoleList_SelectedIndexChanged(object sender, EventArgs e)
        {
            DisplayUsersBelongingToRole();
        }

        private void DisplayUsersBelongingToRole()
        {
            // Get the selected role
            string selectedRoleName = RoleList.SelectedValue;

            // Get the list of usernames that belong to the role
            string[] usersBelongingToRole = RoleManager.FindByName(selectedRoleName).Users.Select(u => UserManager.FindById(u.UserId).UserName).ToArray();

            // Bind the list of users to the GridView
            RolesUserList.DataSource = usersBelongingToRole;
            RolesUserList.DataBind();
        }

        protected void RolesUserList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            // Get the selected role
            string selectedRoleName = RoleList.SelectedValue;

            // Reference the UserNameLabel
            Label UserNameLabel = RolesUserList.Rows[e.RowIndex].FindControl("UserNameLabel") as Label;


            // Remove the user from the role
            UserManager.RemoveFromRole(UserManager.FindByName(UserNameLabel.Text).Id, selectedRoleName);

            // Refresh the GridView
            DisplayUsersBelongingToRole();

            // Display a status message
            ActionStatus.Text = string.Format("User {0} was removed from role {1}.", UserNameLabel.Text, selectedRoleName);

            // Refresh the "by user" interface
            CheckRolesForSelectedUser();
        }

        protected void AddUserToRoleButton_Click(object sender, EventArgs e)
        {
            // Get the selected role and username
            string selectedRoleName = RoleList.SelectedValue;
            string userNameToAddToRole = UserNameToAddToRole.Text;

            // Make sure that a value was entered
            if (userNameToAddToRole.Trim().Length == 0)
            {
                ActionStatus.Text = "Introduceti numele de utilizator.";
                return;
            }

            // Make sure that the user exists in the system
            if (UserManager.FindByName(userNameToAddToRole)==null)
            {
                ActionStatus.Text = "Userul specificat nu exista!";
                return;
            }

            // Make sure that the user doesn't already belong to this role
            if (RoleManager.FindByName(selectedRoleName).Users.FirstOrDefault(u=>u.UserId.Equals(UserManager.FindByName(userNameToAddToRole).Id))!=null)
            {
                ActionStatus.Text = string.Format("Userul {0} apartine deja Rolului {1}.", userNameToAddToRole, selectedRoleName);
                return;
            }

            // If we reach here, we need to add the user to the role
            UserManager.AddToRole(UserManager.FindByName(userNameToAddToRole).Id, selectedRoleName);

            // Clear out the TextBox
            UserNameToAddToRole.Text = string.Empty;

            // Refresh the GridView
            DisplayUsersBelongingToRole();

            // Display a status message
            ActionStatus.Text = string.Format("User {0} was added to role {1}.", userNameToAddToRole, selectedRoleName);

            // Refresh the "by user" interface
            CheckRolesForSelectedUser();
        }
        #endregion

    }
}