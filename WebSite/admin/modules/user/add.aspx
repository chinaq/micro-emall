<%@ Page Title="" Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" CodeFile="add.aspx.cs" Inherits="AdminUserAddon" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="container-fluid padding-left-0">
        <div class="row">
            <div class="col-lg-6 col-md-8 col-sm-12 input-warp">
                <div class="form-group">
                    <label>用户头像</label>
                    <asp:Image ID="UserImage" runat="server" CssClass="img-thumbnail width-100 block margin-bottom-10"/>
                    <asp:FileUpload ID="UserImageUpload" runat="server" CssClass="pointer"/>   
                </div>
                <div class="input-group">
                    <span class="input-group-addon font-size-15">用户名<i class="text-danger">*</i></span>
                    <asp:TextBox ID="UserName" runat="server" CssClass="form-control" placeholder="输入用户名"></asp:TextBox>
                </div>
                <p class="valid-group">                   
                    <asp:RequiredFieldValidator ID="Validator1" runat="server" ControlToValidate="UserName" ValidationGroup="UserValid" 
                        Display="Dynamic" CssClass="alert alert-warning" role="alert">
                        <i class="fa fa-ban text-danger"></i>&nbsp;&nbsp;必须填写用户名
                    </asp:RequiredFieldValidator>
                </p>
                <div class="input-group">
                    <span class="input-group-addon font-size-15">密&nbsp;&nbsp;&nbsp;&nbsp;码<i class="text-danger">*</i></span>
                    <asp:TextBox ID="UserPassword" runat="server" CssClass="form-control" placeholder="输入密码" TextMode="Password"></asp:TextBox>
                </div>
                <p class="valid-group">                   
                    <asp:RequiredFieldValidator ID="Validator2" runat="server" ControlToValidate="UserPassword" ValidationGroup="UserValid" 
                        Display="Dynamic" CssClass="alert alert-warning" role="alert">
                        <i class="fa fa-ban text-danger"></i>&nbsp;&nbsp;必须填写密码
                    </asp:RequiredFieldValidator>
                </p>
                <div class="input-group">
                    <span class="input-group-addon font-size-15">确认密码<i class="text-danger">*</i></span>
                    <asp:TextBox ID="UserTruePassword" runat="server" CssClass="form-control" placeholder="再次输入密码" TextMode="Password"></asp:TextBox>
                </div>
                <p class="valid-group">                   
                    <asp:RequiredFieldValidator ID="Validator3" runat="server" ControlToValidate="UserTruePassword" ValidationGroup="UserValid"
                        Display="Dynamic" CssClass="alert alert-warning" role="alert">
                        <i class="fa fa-ban text-danger"></i>&nbsp;&nbsp;请再次输入密码
                    </asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="Validator4" runat="server" ControlToValidate="UserTruePassword" ControlToCompare="UserPassword" ValidationGroup="UserValid"
                        Display="Dynamic" CssClass="alert alert-warning" role="alert" Operator="Equal">
                        <i class="fa fa-ban text-danger"></i>&nbsp;&nbsp;两次输入密码必须相同
                    </asp:CompareValidator>
                </p>             
                <div class="input-group">
                    <span class="input-group-addon">昵&nbsp;&nbsp;&nbsp;&nbsp;称<i class="text-danger">*</i></span>
                    <asp:TextBox ID="NickName" runat="server" CssClass="form-control" placeholder="输入昵称"></asp:TextBox>
                </div>
                <p class="valid-group">                   
                    <asp:RequiredFieldValidator ID="Validator5" runat="server" ControlToValidate="NickName" ValidationGroup="UserValid" 
                        Display="Dynamic" CssClass="alert alert-warning" role="alert">
                        <i class="fa fa-ban text-danger"></i>&nbsp;&nbsp;必须填写昵称
                    </asp:RequiredFieldValidator>
                </p>           
                <div class="input-group">
                    <span class="input-group-addon">类&nbsp;&nbsp;&nbsp;&nbsp;型<i class="text-danger">*</i></span>
                    <asp:DropDownList ID="UserRole" runat="server" CssClass="form-control max-width-120">
                    </asp:DropDownList>
                </div>
                <p class="valid-group"></p>
                <div class="input-group">
                    <span class="input-group-addon">当前积分<i class="text-danger">*</i></span>
                    <asp:TextBox ID="UserInte" runat="server" CssClass="form-control max-width-100" Text="0" placeholder="当前积分"></asp:TextBox>
                </div>
                <p class="valid-group">
                    <asp:RequiredFieldValidator ID="Validator6" runat="server" ControlToValidate="UserInte" ValidationGroup="UserValid" 
                        Display="Dynamic" CssClass="alert alert-warning" role="alert">
                        <i class="fa fa-ban text-danger"></i>&nbsp;&nbsp;必须填写用户积分
                    </asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="Validator7" runat="server" ControlToValidate="UserInte" ValidationGroup="UserValid" 
                        Display="Dynamic" CssClass="alert alert-warning" role="alert" ValidationExpression="^\d+$">
                        <i class="fa fa-ban text-danger"></i>&nbsp;&nbsp;请输入数字
                    </asp:RegularExpressionValidator>
                </p> 
                <div class="input-group">
                    <span class="input-group-addon">手&nbsp;&nbsp;&nbsp;&nbsp;机</span>
                    <asp:TextBox ID="UserMobile" runat="server" CssClass="form-control" placeholder="输入手机号码"></asp:TextBox>
                </div>
                <p class="valid-group">                   
                    <asp:RegularExpressionValidator ID="Validator8" runat="server" ControlToValidate="UserMobile" ValidationGroup="UserValid" 
                        Display="Dynamic" CssClass="alert alert-warning" role="alert" ValidationExpression="^(13|14|15|18)\d{9}$">
                        <i class="fa fa-ban text-danger"></i>&nbsp;&nbsp;手机号码格式不正确
                    </asp:RegularExpressionValidator>
                </p>
                <div class="input-group">
                    <span class="input-group-addon">银行卡号</span>
                    <asp:TextBox ID="UserBankCard" runat="server" CssClass="form-control" placeholder="输入银行卡号"></asp:TextBox>
                </div>
                <p class="valid-group"></p>
                <div class="input-group">
                    <span class="input-group-addon">注册日期</span>
                    <asp:TextBox ID="UserAddDate" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                </div>
                <p class="valid-group"></p>
                <div class="input-group">
                    <span class="input-group-addon">状&nbsp;&nbsp;&nbsp;&nbsp;态<i class="text-danger">*</i></span>
                    <asp:DropDownList ID="UserState" runat="server" CssClass="form-control max-width-100">
                    </asp:DropDownList>
                </div>
                <p class="valid-group"></p>
                <div class="input-group">
                    <asp:Button ID="SubmitButton" runat="server" Text="保存" CssClass="btn btn-primary margin-left-90" ValidationGroup="UserValid" OnClick="SubmitButton_Click"/>
                    <a class="btn btn-default margin-left-15" data-module="user" data-action="list">返回列表</a>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

