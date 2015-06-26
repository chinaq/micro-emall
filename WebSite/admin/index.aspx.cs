using System;
using Jumpcity.Web;
using Jumpcity.Utility.Extend;
using MicroEmall.Models;

public partial class AdminIndex : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Helper.Module = null;
            Helper.Action = null;
            AdminName.Focus();
        }
    }

    protected void LoginButton_Click(object sender, EventArgs e)
    {
        string name = AdminName.Text;
        string pwd = AdminPassword.Text;
        WMAdministrators admin = WMAdministrators.Login(name, pwd);

        if (admin != null)
        {
            Helper.Admin = new Account
            {
                Id = admin.Id.ToString(),
                RoleId = admin.RoleId,
                RoleName = admin.RoleName,
                LoginName = admin.UserName,
                NickName = admin.UserName,
                LoginDate = DateTime.Now,
                AddDate = admin.AddDate
            };

            string url = "~" + Helper.Link("user", "list");
            Response.Redirect(url);
        }
        else
        {
            Helper.ExecScript("alert('用户名或密码不正确');");
        }
    }
}