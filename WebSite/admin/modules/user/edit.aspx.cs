using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Jumpcity.Web;
using MicroEmall.Models;
using Jumpcity.Utility.Extend;

public partial class AdminUserEdit : System.Web.UI.Page
{
    #region 编辑界面数据初始化...

    protected string UserId
    {
        get { return ViewState["userId"] as string; }
        set { ViewState["userId"] = value; }
    }

    protected int UserRoleId
    {
        get
        {
            object rid = ViewState["rid"];
            return rid != null ? (int)rid : 0;
        }
        set { ViewState["rid"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Helper.Action = "list";
            UserId = Request.GetString("id");
            UserBind();
        }
    }

    protected void UserBind()
    {
        //用户编辑 -- 用户类型项绑定
        RoleBind();

        //用户编辑 -- 用户状态项绑定
        StateBind();

        //积分明细 -- 积分来源项绑定
        SourceBind();
        
        WMUsers user = WMUsers.Get(UserId);
        if (user != null)
        {
            UserRoleId = user.RoleId;

            //用户基本信息编辑
            UserImage.ImageUrl = user.Image;
            NickName.Text = user.NickName;
            UserMobile.Text = user.Mobile;
            UserBankCard.Text = user.BankCard;
            UserRole.SelectedValue = user.RoleId.ToString();
            UserState.SelectedValue = user.StatusId.ToString();
            UserAddDate.Text = user.AddDate.ToString("yyyy-MM-dd HH:mm");

            //积分明细
            Integarl.Text = string.Format("当前积分：{0}", user.Integral);
            IntegralBind();

            //配送信息
            ReceiptBind();

            //用户收藏
            SaveBind();

            if (UserRoleId == 102)
            {
                //用户分享
                ShareBind();

                //用户分红
                BonusBind();

                //浏览点击
                ClickBind();
            }
        }
    }

    #endregion 编辑界面数据初始化...

    #region 用户基本信息编辑...

    protected void RoleBind()
    {
        List<WMUserRole> list = WMUserRole.GetList();

        if (!General.IsNullable(list))
        {
            UserRole.Items.Clear();
            foreach (var role in list)
            {
                ListItem item = new ListItem();
                item.Text = role.RoleName;
                item.Value = role.RoleId.ToString();
                UserRole.Items.Add(item);
            }
        }
    }

    protected void StateBind()
    {
        List<WMAccountState> list = WMAccountState.GetList();

        if (!General.IsNullable(list))
        {
            UserState.Items.Clear();
            foreach (var state in list)
            {
                ListItem item = new ListItem();
                item.Text = state.StateName;
                item.Value = state.StateId.ToString();
                UserState.Items.Add(item);
            }
        }
    }

    protected void SubmitButton_Click(object sender, EventArgs e)
    {
        bool flag = false;
        string msg = string.Empty;

        if (this.IsValid)
        {
            WMUsers user = WMUsers.Get(UserId);
            user.RoleId = UserRole.SelectedValue.ToInt32();
            user.NickName = NickName.Text;
            user.Mobile = UserMobile.Text;
            user.BankCard = UserBankCard.Text;
            user.StatusId = UserState.SelectedValue.ToInt32();

            if (UserImageUpload.HasFile)
            {
                string fileName = string.Format("~/share/upload/user/{0}/face.jpg", user.Id);
                Jumpcity.IO.FileUpload upload = new Jumpcity.IO.FileUpload(fileName, 0, 0);
                if (upload.Upload(UserImageUpload.PostedFile.InputStream))
                {
                    user.Image = fileName;
                }
                else
                {
                    msg = upload.ErrorMessage;
                    Helper.MessageBox(flag, msg);
                    return;
                }
            }

            flag = user.Update();
        }

        Helper.MessageBox(flag);
    }

    #endregion 用户基本信息编辑...

    #region 用户积分...

    protected int SourceId
    {
        get
        {
            object sid = ViewState["sid"];
            return sid != null ? (int)sid : 0;
        }
        set { ViewState["sid"] = value; }
    }

    protected void SourceBind()
    {
        List<WMIntegralSource> list = WMIntegralSource.GetList();
        SourceList.DataSource = list;
        SourceList.DataBind();
    }

    protected void IntegralBind(bool current = false)
    {
        if (current)
            TabPlay("inte");

        int pageCount = 0;
        List<WMUserIntegrals> list = WMUserIntegrals.GetList(out pageCount, SourceId, UserId, IntegralPager.CurrentPageIndex - 1, IntegralPager.PageSize);
        IntegralListConatner.DataSource = list;
        IntegralListConatner.DataBind();
        IntegralPager.RecordCount = pageCount;
    }

    protected void SourceList_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        SourceId = Convert.ToInt32(e.CommandArgument);

        if (SourceId > 0)
            SourceName.Text = "来源：" + ((LinkButton)e.CommandSource).Text;
        else
            SourceName.Text = "来源";

        IntegralBind(true);
    }

    protected void IntegralPager_PageChanging(object src, Wuqi.Webdiyer.PageChangingEventArgs e)
    {
        IntegralPager.CurrentPageIndex = e.NewPageIndex;
        IntegralBind(true);
    }

    #endregion 用户积分...

    #region 用户的配送信息...

    protected void ReceiptBind(bool current = false)
    {
        if (current)
            TabPlay("receipt");

        int pageCount = 0;
        List<WMUserReceipts> list = WMUserReceipts.GetList(out pageCount, UserId, ReceiptPager.CurrentPageIndex - 1, ReceiptPager.PageSize);
        ReceiptList.DataSource = list;
        ReceiptList.DataBind();
        ReceiptPager.RecordCount = pageCount;
    }

    protected void ReceiptPager_PageChanging(object src, Wuqi.Webdiyer.PageChangingEventArgs e)
    {
        ReceiptPager.CurrentPageIndex = e.NewPageIndex;
        ReceiptBind(true);
    }

    #endregion 用户的配送信息...

    #region 用户分享...

    protected void ShareBind(bool current = false)
    {
        if (current)
            TabPlay("share");

        int pageCount = 0;
        List<WMUserSets> list = WMUserSets.GetList(out pageCount, 402, UserId, SharePager.CurrentPageIndex - 1, SharePager.PageSize);
        ShareList.DataSource = list;
        ShareList.DataBind();
        SharePager.RecordCount = pageCount;
    }

    protected void SharePager_PageChanging(object src, Wuqi.Webdiyer.PageChangingEventArgs e)
    {
        SharePager.CurrentPageIndex = e.NewPageIndex;
        ShareBind(true);
    }

    #endregion 用户分享...

    #region 用户收藏...

    protected void SaveBind(bool current = false)
    {
        if (current)
            TabPlay("save");

        int pageCount = 0;
        List<WMUserSets> list = WMUserSets.GetList(out pageCount, 401, UserId, SavePager.CurrentPageIndex - 1, SavePager.PageSize);
        SaveList.DataSource = list;
        SaveList.DataBind();
        SavePager.RecordCount = pageCount;
    }

    protected void SavePager_PageChanging(object src, Wuqi.Webdiyer.PageChangingEventArgs e)
    {
        SavePager.CurrentPageIndex = e.NewPageIndex;
        SaveBind(true);
    }

    #endregion 用户收藏...

    #region 用户分红...

    protected DateTime? MinDate
    {
        get
        {
            object min = ViewState["minDate"];
            return min != null ? (DateTime?)min : null;
        }
        set { ViewState["minDate"] = value; }
    }

    protected DateTime? MaxDate
    {
        get
        {
            object max = ViewState["maxDate"];
            return max != null ? (DateTime?)max : null;
        }
        set { ViewState["maxDate"] = value; }
    }

    protected void BonusBind(bool current = false)
    {
        if (current)
            TabPlay("bonus");

        int pageCount = 0;
        List<WMUserBonus> list = WMUserBonus.GetList(out pageCount, MinDate, MaxDate, UserId, BonusPager.CurrentPageIndex - 1, BonusPager.PageSize);
        BonusList.DataSource = list;
        BonusList.DataBind();
        BonusPager.RecordCount = pageCount;
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

        BonusBind(true);
    }

    protected void BonusPager_PageChanging(object src, Wuqi.Webdiyer.PageChangingEventArgs e)
    {
        BonusPager.CurrentPageIndex = e.NewPageIndex;
        BonusBind(true);
    }

    #endregion 用户分红...

    #region 浏览点击...

    protected void ClickBind(bool current = false)
    {
        if (current)
            TabPlay("click");

        int pageCount = 0;
        List<WMUserClicks> list = WMUserClicks.GetList(out pageCount, UserId, ClickPager.CurrentPageIndex - 1, ClickPager.PageSize);
        ClickList.DataSource = list;
        ClickList.DataBind();
        ClickPager.RecordCount = pageCount;
    }

    protected void ClickPager_PageChanging(object src, Wuqi.Webdiyer.PageChangingEventArgs e)
    {
        ClickPager.CurrentPageIndex = e.NewPageIndex;
        ClickBind(true);
    }

    #endregion 浏览点击...

    #region 通用工具方法...

    protected void TabPlay(string current)
    {
        string script = "var __tabs = {\"userEditTabs\":{\"current\":\"" + current + "\"}};";
        Helper.ExecScript(script);
    }

    #endregion 通用工具方法...
}