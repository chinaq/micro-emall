using System;
using Jumpcity.Web;
using MicroEmall.Models;
using Jumpcity.Utility.Extend;

public partial class AdminGoodCategoryEdit : System.Web.UI.Page
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
        WMGoodBrands model = WMGoodBrands.Get(Id);
        if (model != null)
        {
            BrandName.Text = model.Name;
            BrandUrl.Text = model.URL;
            BrandLogo.ImageUrl = model.Logo;
        }
    }

    protected void SubmitButton_Click(object sender, EventArgs e)
    {
        bool flag = false;
        string msg = string.Empty;

        if (this.IsValid)
        {
            WMGoodBrands brand = !General.IsNullable(Id) ? WMGoodBrands.Get(Id) : new WMGoodBrands();
            brand.Name = BrandName.Text;
            brand.URL = BrandUrl.Text;

            if (ImageUpload.HasFile)
            {
                string fileName = string.Format("/share/upload/good/brand/{0}.jpg", DateTime.Now.ToFileTime());
                Jumpcity.IO.FileUpload upload = new Jumpcity.IO.FileUpload("~" + fileName, 0, 0);
                if (upload.Upload(ImageUpload.PostedFile.InputStream))
                {
                    brand.Logo = fileName;
                    BrandLogo.ImageUrl = upload.FileName;
                }
            }

            if (!General.IsNullable(brand.Id))
                flag = brand.Update();
            else
                flag = brand.Add();
        }

        Helper.MessageBox(flag);
    }
}