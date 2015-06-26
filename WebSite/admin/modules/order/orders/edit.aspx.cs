using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Jumpcity.Web;
using MicroEmall.Models;
using Jumpcity.Utility.Extend;

public partial class AdminOrderEdit : System.Web.UI.Page
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
        WMOrders order = WMOrders.Get(Id);
        if (order != null)
        {
            OrderId.Text = order.Id;
            AddDate.Text = order.AddDate.ToString("yyyy-MM-dd HH:mm");
            NickName.Text = order.NickName;
            PayName.Text = order.PayName;
            Freight.Text = order.Freight.ToString("N2");
            GoodSum.Text = order.GoodSum.ToString("N2");
            OrderSum.Text = order.OrderSum.ToString("N2");
            PaidState.Text = (order.Paid ? "已" : "未") + "付款";
            StatusName.Text = order.StatusName;
            OrderMessage.Text = order.Message;
            OrderContact.Text = order.Contact;
            OrderProvince.Text = order.ProvinceName;
            OrderAddress.Text = order.CityName + order.AreaName + order.Address;
            OrderPhone.Text = order.Phone;
            OrderZipCode.Text = order.ZipCode;

            GoodBind();
        }     
    }

    protected void GoodBind()
    {
        int pageCount = 0;
        List<WMOrderGoods> list = WMOrderGoods.GetList(out pageCount, Id);
        ListConatner.DataSource = list;
        ListConatner.DataBind();
    }
}