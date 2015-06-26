using System;
using System.Linq;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Jumpcity.Web;
using MicroEmall.Models;
using Jumpcity.Utility.Extend;

public partial class AdminGoodEdit : System.Web.UI.Page
{
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
            Id = Request.GetString("id");
            Bind();
        }
    }

    protected void Bind()
    {
        ImageBind();
        WMGoods good = WMGoods.Get(Id);

        if (good != null)
        {
            CategoryMasterBind(good.Categories);
            BrandBind(good.BrandId);
            StateBind(good.StatusId);

            GoodName.Text = good.Name;
            OriginalPrice.Text = good.OriginalPrice.ToString();
            PresentPrice.Text = good.PresentPrice.ToString();
            GoodUnit.Text = good.Unit;
            GoodInte.Text = good.Integral.ToString();
            GoodBonus.Text = good.Bonus.ToString();
            GoodGoldPool.Text = good.GoldPool.ToString();
            AddDate.Text = good.AddDate.ToString("yyyy-MM-dd HH:mm");
            GoodDsec.Text = good.Desc;
            GoodSpec.Text = good.Spec;
            GoodService.Text = good.Service;
            GoodClick.Text = good.Clicks.ToString();
            GoodSave.Text = good.Saves.ToString();
        }
    }

    protected void ImageBind()
    {
        List<WMGoodImages> list = WMGoodImages.GetList(Id);
        ImageRepeater.DataSource = list;
        ImageRepeater.DataBind();
    }

    protected void CategoryMasterBind(List<WMGoodCategories> cList)
    {
        List<WMGoodCategories> list = WMGoodCategories.GetList();
        CategoryMasterList.DataSource = list;
        CategoryMasterList.DataBind();

        if (!General.IsNullable(cList))
        {
            Helper.ExecScript(string.Format("var __mcid=\"{0}\";", cList[0].Id));

            if (cList.Count > 1)
                Helper.ExecScript(string.Format("var __scid=\"{0}\";", cList.Last().Id));
        }
    }

    protected void BrandBind(string bid)
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
                item.Selected = b.Id.Equals(bid);
                BrandList.Items.Add(item);
            }
        }
    }

    protected void StateBind(int stateId)
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
                item.Selected = state.StateId == stateId;
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

            WMGoods good = WMGoods.Get(Id);

            if (good != null)
            {
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

                flag = good.Update();
            }
        }

        if (flag)
            Bind();

        Helper.MessageBox(flag);
    }

    protected void UploadButton_Click(object sender, EventArgs e)
    {
        bool flag = false;

        if (!ImageUpload.HasFile)
            PicValid.CssClass = "alert alert-warning";
        else
        {
            WMGoodImages gi = new WMGoodImages();
            gi.GoodId = Id;
            string fileName = string.Format("/share/upload/good/{0}/{1}.jpg", gi.GoodId, DateTime.Now.ToFileTime());
            Jumpcity.IO.FileUpload upload = new Jumpcity.IO.FileUpload("~" + fileName, 0, 0);
            if (upload.Upload(ImageUpload.PostedFile.InputStream))
            {
                gi.URL = fileName;
                flag = gi.Add();
            }
        }

        Bind();
        TabPlay("image");
        Helper.MessageBox(flag);
    }

    protected void ImageRepeater_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
    {
        string id = e.CommandArgument.ToString();
        string cmd = e.CommandName;
        bool flag = false;
        if (cmd == "Delete")
            flag = WMGoodImages.Delete(id);
        else if (cmd == "Cover")
            flag = WMGoodImages.UpdateCover(id);

        Bind();
        TabPlay("image");
        Helper.MessageBox(flag);
    }

    protected void TabPlay(string current)
    {
        string script = "var __tabs = {\"goodEditTabs\":{\"current\":\"" + current + "\"}};";
        Helper.ExecScript(script);
    }
}