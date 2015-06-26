using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Jumpcity.Web;
using MicroEmall.Models;
using Jumpcity.Utility.Extend;

public partial class AdminOrderList : System.Web.UI.Page
{
    protected string OrderId
    {
        get { return ViewState["oid"] as string; }
        set { ViewState["oid"] = value; }
    }

    protected DateTime? MinDate
    {
        get
        {
            object min = ViewState["min"];
            return min != null ? (DateTime?)min : null;
        }
        set { ViewState["min"] = value; }
    }

    protected DateTime? MaxDate
    {
        get
        {
            object max = ViewState["max"];
            return max != null ? (DateTime?)max : null;
        }
        set { ViewState["max"] = value; }
    }

    protected int Paid
    {
        get
        {
            object paid = ViewState["paid"];
            return paid != null ? (int)paid : -1;
        }
        set { ViewState["paid"] = value; }
    }

    protected int PayId
    {
        get
        {
            object payId = ViewState["payId"];
            return payId != null ? (int)payId : 0;
        }
        set { ViewState["payId"] = value; }
    }

    protected int StateId
    {
        get
        {
            object sid = ViewState["sid"];
            return sid != null ? (int)sid : 0;
        }
        set { ViewState["sid"] = value; }
    }

    protected string UserId
    {
        get { return ViewState["uid"] as string; }
        set { ViewState["uid"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            UserId = Request.GetString("uid");
            Bind();
        }
    }

    protected void Bind()
    {
        PayBind();
        StateBind();
        ListBind();
    }

    protected void ListBind()
    {
        int pageCount = 0;
        List<WMOrders> list = WMOrders.GetList(out pageCount, UserId, OrderId, MinDate, MaxDate, PayId, Paid, StateId, Pager.CurrentPageIndex - 1, Pager.PageSize);
        ListConatner.DataSource = list;
        ListConatner.DataBind();
        Pager.RecordCount = pageCount;
    }

    protected void PayBind()
    {
        List<WMOrderPay> list = WMOrderPay.GetList();
        PayList.DataSource = list;
        PayList.DataBind();
    }

    protected void StateBind()
    {
        List<WMOrderState> list = WMOrderState.GetList();
        StateList.DataSource = list;
        StateList.DataBind();
    }

    protected void ListConatner_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName == "Deliver")
        {
            string id = e.CommandArgument.ToString();
            bool flag = WMOrders.UpdateStatus(id, 602);

            if (flag)
                ListBind();

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
            string name = WMOrderState.Get(StateId).StateName;
            StateName.Text = "状态：" + name;
        }
        else
            StateName.Text = "状态";

        ListBind();
    }

    protected void PayList_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        PayId = Convert.ToInt32(e.CommandArgument);

        if (PayId > 0)
        {
            string name = ((LinkButton)e.CommandSource).Text;
            PayName.Text = "支付方式：" + name;
        }
        else
            PayName.Text = "支付方式";

        ListBind(); 
    }

    protected void Paid_Command(object sender, CommandEventArgs e)
    {
        Paid = Convert.ToInt32(e.CommandArgument);

        if (Paid >= 0)
        {
            string name = ((LinkButton)sender).Text;
            PaidState.Text = "付款：" + name;
        }
        else
            PaidState.Text = "付款";

        ListBind();    
    }

    protected void Date_Command(object sender, CommandEventArgs e)
    {
        string cmd = e.CommandName;
        
        if (cmd == "All")
        {
            MinDate = null;
            MaxDate = null;
            DateName.Text = "时间";
        }
        else
        {
            DateTime now = DateTime.Now;
            DateName.Text = "时间：" + ((LinkButton)sender).Text;
            
            if (cmd == "Today")
            {              
                MinDate = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);
                MaxDate = new DateTime(now.Year, now.Month, now.Day, 23, 59, 59);
            }
            else if (cmd == "Week")
            {
                MaxDate = now;
                MinDate = now.AddDays(-7);
            }
            else if (cmd == "Month")
            {
                int m = Convert.ToInt32(e.CommandArgument) * -1;
                MaxDate = now;
                MinDate = now.AddMonths(m);
            }
        }

        ListBind();
    }

    protected void SearchLink_Click(object sender, EventArgs e)
    {
        OrderId = IdSearch.Text;
        ListBind();
    }   
}