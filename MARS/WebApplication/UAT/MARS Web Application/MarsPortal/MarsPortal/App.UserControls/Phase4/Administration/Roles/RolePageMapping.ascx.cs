using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mars.App.Classes.Phase4Bll.Administration;
using Mars.App.Classes.Phase4Dal.Administration.UserEntities;
using Mars.App.Classes.Phase4Dal.Administration.Users;

namespace Mars.App.UserControls.Phase4.Administration.Roles
{
    public partial class RolePageMapping : UserControl
    {
        public const string AssignCommand = "AssignCommand";
        public const string UnassignCommand = "UnassignCommand";


        private int RoleIdSelected
        {
            get { return int.Parse(lbRoles.SelectedValue); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                
                using (var dataAccess = new UsersAndRolesDataAccess())
                {
                    var companyTypes = dataAccess.GetCompanyTypes();
                    rblCompanyTypes.Items.AddRange(companyTypes.ToArray());
                    rblCompanyTypes.SelectedIndex = 0;
                }
                PopulateRolesListBox();
            }
            
        }

        private void PopulateRolesListBox()
        {
            using (var dataAccess = new RolesAndPagesDataAccess())
            {
                var selectedCompanyType = int.Parse(rblCompanyTypes.SelectedValue);
                var roleData = dataAccess.GetAllRoles(selectedCompanyType);
                BindRoles(roleData);
            }
            rptPages.DataSource = null;
            rptPages.DataBind();
        }

        private void BindRoles(IEnumerable<RoleEntity> roleData )
        {
            var roles = from rd in roleData
                     select new ListItem(rd.RoleName, rd.UserRoleId.ToString());

            lbRoles.Items.Clear();
            lbRoles.Items.AddRange(roles.ToArray());
        }

        protected void lbRoles_Selected(object sender, EventArgs e)
        {
            PopulateAccessTree();
        }

        private void PopulateAccessTree(RolesAndPagesDataAccess dataAccess = null )
        {
            List<PageEntity> pages;
            var companyTypeSelected = (CompanyTypeEnum)Enum.Parse(typeof(CompanyTypeEnum), rblCompanyTypes.SelectedValue);
            if(dataAccess == null)
            {
                using (var newDataAccess = new RolesAndPagesDataAccess())
                {
                    pages = newDataAccess.GetPagesForRole(RoleIdSelected, companyTypeSelected);
                }    
            }
            else
            {
                pages = dataAccess.GetPagesForRole(RoleIdSelected, companyTypeSelected);
            }
            rptPages.DataSource = pages;
            rptPages.DataBind();
        }

        protected void rblCompanyTypes_SelectionChanged(object sender, EventArgs e)
        {
            PopulateRolesListBox();
        }

        public void RepeaterCommand(object sender, EventArgs e)
        {
            if (e is CommandEventArgs)
            {
                var commandEventArgs = e as CommandEventArgs;
                if (commandEventArgs.CommandName == AssignCommand || commandEventArgs.CommandName == UnassignCommand)
                {
                    var urlId = int.Parse(commandEventArgs.CommandArgument.ToString());
                    var assignCommand = commandEventArgs.CommandName == AssignCommand;

                    using (var dataAccess = new RolesAndPagesDataAccess())
                    {
                        if (assignCommand)
                        {
                            dataAccess.AssignUrlToRole(RoleIdSelected, urlId);
                        }
                        else
                        {
                            dataAccess.UnassignUrlFromRole(RoleIdSelected, urlId);
                        }
                        PopulateAccessTree(dataAccess);
                    }
                }
            }
        }



    }
}