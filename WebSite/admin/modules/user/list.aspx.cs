using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Jumpcity.Web;
using MicroEmall.Models;

public partial class AdminUserList : System.Web.UI.Page
{
    protected int UserRoleId
    {
        get
        {
            object roleId = ViewState["roleId"];
            return roleId != null ? (int)roleId : 0;
        }
        set { ViewState["roleId"] = value; }
    }

    protected int UserStateId
    {
        get
        {
            object stateId = ViewState["stateId"];
            return stateId != null ? (int)stateId : 0;
        }
        set { ViewState["stateId"] = value; }
    }

    protected string UserKey
    {
        get
        {
            string userKey = ViewState["suerKey"] as string;
            return userKey != null ? userKey : string.Empty;
        }
        set { ViewState["userKey"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Bind();
        }
    }

    protected void Bind()
    {
        ListBind();
        RoleBind();
        StateBind();
    }

    protected void ListBind()
    {
        int pageCount = 0;
        List<WMUsers> list = WMUsers.GetList(out pageCount, UserRoleId, UserStateId, UserKey, Pager.CurrentPageIndex - 1, Pager.PageSize);

        ListConatner.DataSource = list;
        ListConatner.DataBind();
        Pager.RecordCount = pageCount;
    }

    protected void RoleBind()
    {
        List<WMUserRole> list = WMUserRole.GetList();
        RoleList.DataSource = list;
        RoleList.DataBind();
    }

    protected void StateBind()
    {
        List<WMAccountState> list = WMAccountState.GetList();
        StateList.DataSource = list;
        StateList.DataBind();
    }

    protected void ListConatner_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "Delete")
        {
            string userId = e.CommandArgument.ToString();
            bool flag = WMUsers.Delete(userId);

            if (flag)
                Bind();

            Helper.MessageBox(flag);
        }
    }

    protected void Pager_PageChanging(object src, Wuqi.Webdiyer.PageChangingEventArgs e)
    {
        Pager.CurrentPageIndex = e.NewPageIndex;
        ListBind();
    }

    protected void RoleList_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        UserRoleId = Convert.ToInt32(e.CommandArgument);
        if (UserRoleId > 0)
        {
            string name = ((LinkButton)e.CommandSource).Text;
            UserRoleName.Text = "类型：" + name;
        }
        else
            UserRoleName.Text = "类型";
        ListBind();
    }

    protected void StateList_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        UserStateId = Convert.ToInt32(e.CommandArgument);

        if (UserStateId > 0)
        {
            string name = ((LinkButton)e.CommandSource).Text;
            UserStateName.Text = "状态：" + name;
        }
        else
            UserStateName.Text = "状态";

        ListBind();
    }
}