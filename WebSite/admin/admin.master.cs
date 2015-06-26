using System;
using Jumpcity.Web;
using Jumpcity.Utility.Extend;

public partial class AdminMaster : System.Web.UI.MasterPage
{   
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Helper.Admin != null)
            LoginName.Text = Helper.Admin.NickName;
        Helper.Active();
    }
}
