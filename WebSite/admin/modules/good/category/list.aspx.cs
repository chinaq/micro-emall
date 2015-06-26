using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using MicroEmall.Models;
using Jumpcity.Web;
using Jumpcity.Utility.Extend;

public partial class AdminGoodCategoryList : System.Web.UI.Page
{
    protected string ParentId
    {
        get
        {
            string pid = ViewState["pid"] as string;
            return pid != null ? pid : "root";
        }
        set { ViewState["pid"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ParentId = Request.GetString("pid", "root");
            AddLink.NavigateUrl = Helper.Link(Helper.Module, "edit", new string[] { string.Format("pid={0}", ParentId) });
            Bind();
        }
    }

    protected void Bind()
    {
        string parentId = ParentId;
        List<WMGoodCategories> parentList = WMGoodCategories.GetTree(parentId);
        if (!General.IsNullable(parentList))
        {
            NavbarBrandLabel.Visible = false;
            parentList.Insert(0, new WMGoodCategories { Id = "root", ParentId = "", Name = "商品分类" });
            JoinConatner.DataSource = parentList;
            JoinConatner.DataBind();
        }

        List<WMGoodCategories> list = WMGoodCategories.GetList(parentId);
        ListConatner.DataSource = list;
        ListConatner.DataBind();
    }

    protected void ListConatner_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        string cmdName = e.CommandName;

        if(cmdName == "Edit")
        {
            string[] parameters = { 
                string.Format("pid={0}", ParentId),
                string.Format("id={0}", e.CommandArgument) 
            };
            Response.Redirect(Helper.Link(Helper.Module, "edit", parameters));
        }
    }
}