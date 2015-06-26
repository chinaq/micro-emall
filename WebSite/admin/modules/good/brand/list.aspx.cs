using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using MicroEmall.Models;
using Jumpcity.Web;
using Jumpcity.Utility.Extend;

public partial class AdminGoodBrandList : System.Web.UI.Page
{
    protected string Name
    {
        get { return ViewState["name"] as string; }
        set { ViewState["name"] = value; }
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
        int pageCount = 0;
        List<WMGoodBrands> list = WMGoodBrands.GetList(out pageCount, Name, Pager.CurrentPageIndex - 1, Pager.PageSize);
        ListConatner.DataSource = list;
        ListConatner.DataBind();
        Pager.RecordCount = pageCount;
    }

    protected void ListConatner_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "Delete")
        {
            string id = e.CommandArgument.ToString();
            bool flag = WMGoodBrands.Delete(id);

            if (flag)
                Bind();

            Helper.MessageBox(flag);
        }
    }

    protected void Pager_PageChanging(object src, Wuqi.Webdiyer.PageChangingEventArgs e)
    {
        Pager.CurrentPageIndex = e.NewPageIndex;
        Bind();
    }

    protected void SearchLink_Click(object sender, EventArgs e)
    {
        Name = SearchText.Text;
        Bind();
    }
}