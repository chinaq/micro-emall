using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Jumpcity.Web;
using MicroEmall.Models;
using Jumpcity.Utility.Extend;

public partial class AdminGoodAddon : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Bind();
        }
    }

    protected void Bind()
    {
        AddDate.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
        CategoryMasterBind();
        BrandBind();
        StateBind();
    }

    protected void CategoryMasterBind()
    {
        List<WMGoodCategories> list = WMGoodCategories.GetList();
        CategoryMasterList.DataSource = list;
        CategoryMasterList.DataBind();
    }

    protected void BrandBind()
    {
        List<WMGoodBrands> list = WMGoodBrands.GetList();

        if (!General.IsNullable(list))
        {
            BrandList.Items.Clear();
            foreach (var b in list)
            {
                ListItem item = new ListItem();
                item.Text = b.Name;
                item.Value = b.Id;
                BrandList.Items.Add(item);
            }
        }
    }

    protected void StateBind()
    {
        List<WMGoodState> list = WMGoodState.GetList();

        if (!General.IsNullable(list))
        {
            StateList.Items.Clear();
            foreach (var state in list)
            {
                ListItem item = new ListItem();
                item.Text = state.StateName;
                item.Value = state.StateId.ToString();
                StateList.Items.Add(item);
            }
        }
    }

    protected void SubmitButton_Click(object sender, EventArgs e)
    {
        bool flag = false;

        if (this.IsValid)
        {
            string cid = CategoryId.Value;
            if (General.IsNullable(cid))
            {
                CategoryValid.CssClass = CategoryValid.CssClass.Replace(" hide", "");
                return;
            }

            WMGoods good = new WMGoods();

            good.Name = GoodName.Text;
            good.CategoryId = cid;
            good.BrandId = BrandList.SelectedValue;
            good.OriginalPrice = OriginalPrice.Text.ToDouble().Value;
            good.PresentPrice = PresentPrice.Text.ToDouble();
            good.Unit = GoodUnit.Text;
            good.Integral = GoodInte.Text.ToInt32();
            good.Bonus = GoodBonus.Text.ToInt32();
            good.GoldPool = GoodGoldPool.Text.ToInt32();
            good.Desc = GoodDsec.Text;
            good.Spec = GoodSpec.Text;
            good.Service = GoodService.Text;
            good.StatusId = StateList.SelectedValue.ToInt32();

            flag = good.Add();

            if(flag)
            {
                string url = string.Format("~/admin/modules/good/goods/upload.aspx?gid={0}", good.Id);
                Response.Redirect(url);
                return;
            }
        }

        Helper.MessageBox(flag);
    }
}