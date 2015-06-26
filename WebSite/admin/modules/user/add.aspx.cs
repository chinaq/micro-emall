using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Jumpcity.Web;
using MicroEmall.Models;
using Jumpcity.Utility.Extend;

public partial class AdminUserAddon : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            UserBind();
        }
    }

    protected void UserBind()
    {
        UserAddDate.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
        RoleBind();
        StateBind();
    }

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
            if (WMUsers.IsExitis(null, UserName.Text))
            {
                msg = "用户名已存在";
            }
            else
            {
                WMUsers user = new WMUsers();
                user.Id = General.UniqueString(user.Id);
                user.UserName = UserName.Text;
                user.Password = UserPassword.Text;
                user.RoleId = UserRole.SelectedValue.ToInt32();
                user.NickName = NickName.Text;
                user.Mobile = UserMobile.Text;
                user.BankCard = UserBankCard.Text;
                user.StatusId = UserState.SelectedValue.ToInt32();

                if (UserImageUpload.HasFile)
                {
                    string fileName = string.Format("/share/upload/user/{0}/face.jpg", user.Id);
                    Jumpcity.IO.FileUpload upload = new Jumpcity.IO.FileUpload("~" + fileName, 0, 0);
                    if (upload.Upload(UserImageUpload.PostedFile.InputStream))
                    {
                        user.Image = fileName;
                        UserImage.ImageUrl = upload.FileName;
                    }
                    else
                    {
                        msg = upload.ErrorMessage;
                        Helper.MessageBox(flag, msg);
                        return;
                    }
                }

                flag = user.Add();
              
                if (flag)
                {
                    //后台手动添加用户积分
                    int integarl = UserInte.Text.ToInt32();
                    if (integarl > 0)
                    {
                        WMUserIntegrals inte = new WMUserIntegrals();
                        inte.UserId = user.Id;
                        inte.Integral = integarl;
                        inte.SourceId = 452;
                        inte.Add();
                    }
                }
            }
        }

        Helper.MessageBox(flag, msg);
    }
}