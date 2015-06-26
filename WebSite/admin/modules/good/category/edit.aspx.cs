using System;
using System.Linq;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Jumpcity.Web;
using MicroEmall.Models;
using Jumpcity.Utility.Extend;

public partial class AdminGoodCategoryEdit : System.Web.UI.Page
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

    protected string Id
    {
        get { return ViewState["id"] as string; }
        set { ViewState["id"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Helper.Action = "list";
            ParentId = Request.GetString("pid", "root");
            Id = Request.GetString("id");
            Bind();
        }
    }

    protected void Bind()
    {
        string pid = ParentId;

        if (!General.IsNullable(pid, "root"))
        {
            List<WMGoodCategories> list = WMGoodCategories.GetTree(pid);
            ParentName.Text = string.Join(" > ", list.Select(gc => gc.Name).ToList());
        }

        if (!General.IsNullable(Id))
        {
            WMGoodCategories model = WMGoodCategories.Get(Id);
            if (model != null)
            {
                CategoryName.Text = model.Name;
                CategoryLevel.Text = model.Level.ToString();
                CategorySort.Text = model.Sort.ToString();
            }
            
        }
        else
        {
            categoryE.Visible = false;
        }
    }

    protected void SubmitButton_Click(object sender, EventArgs e)
    {
        bool flag = false;
        string msg = string.Empty;

        if (this.IsValid)
        {
            WMGoodCategories gc = !General.IsNullable(Id) ? WMGoodCategories.Get(Id) : new WMGoodCategories();
            gc.Name = CategoryName.Text;
            gc.ParentId = ParentId;

            if (!General.IsNullable(gc.Id))
                flag = gc.Update();
            else
                flag = gc.Add();
        }

        Helper.MessageBox(flag);
    }
}