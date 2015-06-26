using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Jumpcity.Web;
using MicroEmall.Models;
using Jumpcity.Utility.Extend;

public partial class AdminGoodList : System.Web.UI.Page
{
    protected string CategoryId
    {
        get { return ViewState["cid"] as string; }
        set { ViewState["cid"] = value; }
    }

    protected int StateId
    {
        get
        {
            object stateId = ViewState["sid"];
            return stateId != null ? (int)stateId : 0;
        }
        set { ViewState["sid"] = value; }
    }

    protected string BrandId
    {
        get { return ViewState["bid"] as string; }
        set { ViewState["bid"] = value; }
    }

    protected string OrderBy
    {
        get { return ViewState["orderBy"] as string; }
        set { ViewState["orderBy"] = value; }
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
        CategoryBind();
        BrandBind();
        StateBind();
    }

    protected void ListBind()
    {
        int pageCount = 0;
        List<WMGoods> list = WMGoods.GetList(out pageCount, CategoryId, BrandId, null, StateId, OrderBy, Pager.CurrentPageIndex - 1, Pager.PageSize);

        ListConatner.DataSource = list;
        ListConatner.DataBind();
        Pager.RecordCount = pageCount;
    }

    protected void CategoryBind()
    {
        List<WMGoodCategories> roots = WMGoodCategories.GetList();
        if (!General.IsNullable(roots))
        {
            List<WMGoodCategories> list = new List<WMGoodCategories>();

            foreach (WMGoodCategories rc in roots)
            {
                list.Add(rc);
                List<WMGoodCategories> subs = WMGoodCategories.GetList(rc.Id);
                if (!General.IsNullable(subs))
                {
                    foreach (WMGoodCategories sub in subs)
                        sub.Name = "&nbsp;&nbsp;|--&nbsp;" + sub.Name;
                    list.AddRange(subs);
                }
            }

            CategoryList.DataSource = list;
            CategoryList.DataBind();
        }   
    }

    protected void BrandBind()
    {
        List<WMGoodBrands> list = WMGoodBrands.GetList();
        BrandList.DataSource = list;
        BrandList.DataBind();
    }

    protected void StateBind()
    {
        List<WMGoodState> list = WMGoodState.GetList();
        StateList.DataSource = list;
        StateList.DataBind();
    }

    protected void ListConatner_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "Delete")
        {
            string goodId = e.CommandArgument.ToString();
            bool flag = WMGoods.Delete(goodId);

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

    protected void StateList_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        StateId = Convert.ToInt32(e.CommandArgument);

        if (StateId > 0)
        {
            string name = ((LinkButton)e.CommandSource).Text;
            StateName.Text = "状态：" + name;
        }
        else
            StateName.Text = "状态";

        ListBind();
    }

    protected void CategoryList_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        CategoryId = e.CommandArgument.ToString();

        if (!General.IsNullable(CategoryId))
        {
            string name = ((LinkButton)e.CommandSource).Text.TrimStart("&nbsp;&nbsp;|--&nbsp;".ToCharArray());
            CategoryName.Text = "分类：" + name;
        }
        else
            CategoryName.Text = "分类";

        ListBind();
    }

    protected void BrandList_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        BrandId = e.CommandArgument.ToString();

        if (!General.IsNullable(BrandId))
        {
            string name = ((LinkButton)e.CommandSource).Text;
            BrandName.Text = "品牌：" + name;
        }
        else
            BrandName.Text = "品牌";

        ListBind();
    }

    protected void OrderBy_Command(object sender, CommandEventArgs e)
    {
        string name = e.CommandName;
        string direction = e.CommandArgument.ToString();
        LinkButton link = sender as LinkButton;

        if (link != null)
        {
            string dir = direction.Equals("desc") ? "asc" : "desc";
            link.CommandArgument = dir;
            link.CssClass = "fa fa-sort-" + dir;
        }
        
        OrderBy = name + " " + direction;
        ListBind();
    }
}