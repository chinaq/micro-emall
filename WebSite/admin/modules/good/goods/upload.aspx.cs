using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Jumpcity.Web;
using MicroEmall.Models;
using Jumpcity.Utility.Extend;

public partial class AdminGoodUpload : System.Web.UI.Page
{
    protected string GoodId
    {
        get { return ViewState["gid"] as string; }
        set { ViewState["gid"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            GoodId = Request.GetString("gid");
            Bind();
        }
    }

    protected void Bind()
    {
        List<WMGoodImages> list = WMGoodImages.GetList(GoodId);
        ImageRepeater.DataSource = list;
        ImageRepeater.DataBind();
    }

    protected void SubmitButton_Click(object sender, EventArgs e)
    {
        bool flag = false;

        if (!ImageUpload.HasFile)
        {
            PicValid.CssClass = "alert alert-warning";
            Bind();
            return;
        }
        else
        {
            WMGoodImages gi = new WMGoodImages();
            gi.GoodId = GoodId;
            string fileName = string.Format("/share/upload/good/{0}/{1}.jpg", gi.GoodId, DateTime.Now.ToFileTime());
            Jumpcity.IO.FileUpload upload = new Jumpcity.IO.FileUpload("~" + fileName, 0, 0);
            if (upload.Upload(ImageUpload.PostedFile.InputStream))
            {
                gi.URL = fileName;
                flag = gi.Add();
            }
        }

        if (flag)
            Bind();

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
        
        if (flag)
            Bind();

        Helper.MessageBox(flag);
    }
}