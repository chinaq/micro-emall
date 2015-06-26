<%@ Page Title="" Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" CodeFile="edit.aspx.cs" Inherits="AdminUserEdit" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="container-fluid padding-left-0">
        <div class="row">
            <div id="userEditTabs" class="pull-left width-b100" role="tabpanel" data-role="tabContainer">
                <div class="padding-left-15 pull-left padding-right-15">
                    <h5>用户编辑</h5>
                </div>
                <ul class="nav nav-tabs padding-left-15" role="tablist">
                    <li role="presentation" class="active">
                        <a href="#edit" aria-controls="edit" role="tab" data-toggle="tab">基本信息</a>
                    </li>
                    <%if(UserRoleId == 102) {%>
                    <li role="presentation">
                        <a href="#bonus" aria-controls="bonus" role="tab" data-toggle="tab">分红信息</a>
                    </li>
                    <%} %>
                    <%if(UserRoleId == 102) {%>
                    <li role="presentation">
                        <a href="#click" aria-controls="click" role="tab" data-toggle="tab">浏览点击</a>
                    </li>
                    <%} %>
                    <li role="presentation">
                        <a href="#receipt" aria-controls="receipt" role="tab" data-toggle="tab">配送信息</a>
                    </li>
                    <li role="presentation">
                        <a href="#inte" aria-controls="inte" role="tab" data-toggle="tab">积分明细</a>
                    </li>
                    <%if(UserRoleId == 102) {%>
                    <li role="presentation">
                        <a href="#share" aria-controls="share" role="tab" data-toggle="tab">用户分享</a>
                    </li>
                    <%} %>
                    <li role="presentation">
                        <a href="#save" aria-controls="save" role="tab" data-toggle="tab">用户收藏</a>
                    </li>              
                </ul>
                <div class="tab-content padding-left-15">
                    <div role="tabpanel" class="tab-pane active" id="edit">
                        <div class="col-lg-6 col-md-8 col-sm-12 input-warp margin-top-20">
                            <div class="form-group">
                                <label>用户头像</label>
                                <asp:Image ID="UserImage" runat="server" CssClass="img-thumbnail width-100 block margin-bottom-10"/>
                                <asp:FileUpload ID="UserImageUpload" runat="server" CssClass="pointer"/>   
                            </div>       
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
                    <%if(UserRoleId == 102){ %>
                    <div role="tabpanel" class="tab-pane" id="bonus">
                        <div class="col-lg-8 col-md-10 col-sm-12 margin-top-20">
                            <div class="row">
                                <nav class="navbar navbar-default nav-select">
                                    <div class="container-fluid">
                                        <div class="navbar-header">
                                            <span class="navbar-brand">分红信息</span>
                                        </div>          
                                        <div class="collapse navbar-collapse">
                                            <ul class="nav navbar-nav navbar-left margin-left-15">
                                                <li>
                                                    <webdiyer:AspNetPager ID="BonusPager" runat="server" HorizontalAlign="Center" Wrap="False" FirstPageText="&lt;&lt;" PageSize="15" 
                                                        LastPageText="&gt;&gt;" PagingButtonType="Text" ShowMoreButtons="False" ShowPrevNext="True" CurrentPageButtonClass="active" 
                                                        CustomInfoHTML="%CurrentPageIndex% / %PageCount%" CustomInfoSectionWidth="20%" CssClass="pagination" NextPageText="&gt;" 
                                                        PagingButtonSpacing="0px" PrevPageText="&lt;" ShowFirstLast="True" OnPageChanging="BonusPager_PageChanging" NumericButtonCount="8">
                                                    </webdiyer:AspNetPager>
                                                </li>
                                            </ul>
                                            <ul class="nav navbar-nav navbar-right">
                                                <li class="dropdown">
                                                    <a href="javascript:;" class="dropdown-toggle" data-toggle="dropdown" role="button" aria-expanded="false">
                                                        <asp:Label ID="DateName" runat="server" Text="时间"></asp:Label>
                                                        <span class="caret"></span>
                                                    </a>
                                                    <ul class="dropdown-menu" role="menu">
                                                        <li>
                                                            <asp:LinkButton runat="server" Text="查看全部" OnCommand="Date_Command" CommandName="All"></asp:LinkButton>
                                                        </li>
                                                        <li class="divider"></li>
                                                        <li>
                                                            <asp:LinkButton runat="server" Text="今天" OnCommand="Date_Command" CommandName="Today"></asp:LinkButton>
                                                        </li>
                                                        <li>
                                                            <asp:LinkButton runat="server" Text="一周之内" OnCommand="Date_Command" CommandName="Week"></asp:LinkButton>
                                                        </li>
                                                        <li>
                                                            <asp:LinkButton runat="server" Text="一个月内" OnCommand="Date_Command" CommandName="Month" CommandArgument="1"></asp:LinkButton>
                                                        </li>
                                                        <li>
                                                            <asp:LinkButton runat="server" Text="三个月内" OnCommand="Date_Command" CommandName="Month" CommandArgument="3"></asp:LinkButton>
                                                        </li>
                                                        <li>
                                                            <asp:LinkButton runat="server" Text="六个月内" OnCommand="Date_Command" CommandName="Month" CommandArgument="6"></asp:LinkButton>
                                                        </li>
                                                        <li>
                                                            <asp:LinkButton runat="server" Text="一年之内" OnCommand="Date_Command" CommandName="Month" CommandArgument="12"></asp:LinkButton>
                                                        </li>
                                                    </ul>
                                                </li>
                                            </ul>
                                        </div>
                                    </div>
                                </nav>
                                <div class="table-responsive overflow-y-auto">
                                    <table class="table table-hover">
                                        <thead>
                                            <tr>
                                                <th>#</th>
                                                <th>订单号</th>
                                                <th>分红金额</th>
                                                <th>分红时间</th>
                                            </tr>
                                        </thead>
                                        <asp:Repeater ID="BonusList" runat="server">
                                            <HeaderTemplate>
                                                <tbody>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <td>
                                                        <%#Container.ItemIndex + 1%>
                                                    </td>
                                                    <td>
                                                        <%#Eval("OrderId")%>
                                                    </td>
                                                    <td>
                                                        <%#Eval("BonusSum")%>
                                                    </td>
                                                    <td>
                                                        <%#Eval("AddDate","{0:yyyy-MM-dd HH:mm}")%>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                            <AlternatingItemTemplate>
                                                <tr class="active">
                                                    <td>
                                                        <%#Container.ItemIndex + 1%>
                                                    </td>
                                                    <td>
                                                        <%#Eval("OrderId")%>
                                                    </td>
                                                    <td>
                                                        <%#Eval("BonusSum")%>
                                                    </td>
                                                    <td>
                                                        <%#Eval("AddDate","{0:yyyy-MM-dd HH:mm}")%>
                                                    </td>
                                                </tr>
                                            </AlternatingItemTemplate>
                                            <FooterTemplate>
                                                </tbody>    
                                            </FooterTemplate>
                                        </asp:Repeater>
                                    </table>
                                </div>
                            </div>
                        </div>
                    </div>
                    <%} %>
                    <%if(UserRoleId == 102){ %>
                    <div role="tabpanel" class="tab-pane" id="click">
                        <div class="col-lg-8 col-md-10 col-sm-12 margin-top-20">
                            <div class="row">
                                <nav class="navbar navbar-default nav-select">
                                    <div class="container-fluid">
                                        <div class="navbar-header">
                                            <span class="navbar-brand">浏览点击</span>
                                        </div>          
                                        <div class="collapse navbar-collapse">
                                            <ul class="nav navbar-nav navbar-left margin-left-15">
                                                <li>
                                                    <webdiyer:AspNetPager ID="ClickPager" runat="server" HorizontalAlign="Center" Wrap="False" FirstPageText="&lt;&lt;" PageSize="15" 
                                                        LastPageText="&gt;&gt;" PagingButtonType="Text" ShowMoreButtons="False" ShowPrevNext="True" CurrentPageButtonClass="active" 
                                                        CustomInfoHTML="%CurrentPageIndex% / %PageCount%" CustomInfoSectionWidth="20%" CssClass="pagination" NextPageText="&gt;" 
                                                        PagingButtonSpacing="0px" PrevPageText="&lt;" ShowFirstLast="True" OnPageChanging="ClickPager_PageChanging" NumericButtonCount="8">
                                                    </webdiyer:AspNetPager>
                                                </li>
                                            </ul>
                                        </div>
                                    </div>
                                </nav>
                                <div class="table-responsive overflow-y-auto">
                                    <table class="table table-hover">
                                        <thead>
                                            <tr>
                                                <th>图片</th>
                                                <th>名称</th>
                                                <th>点击者</th>
                                                <th>时间</th>
                                            </tr>
                                        </thead>
                                        <asp:Repeater ID="ClickList" runat="server">
                                            <HeaderTemplate>
                                                <tbody class="middle">
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <td>
                                                        <asp:Image runat="server" ImageUrl='<%#Eval("GoodFigure", "~{0}")%>' CssClass="img-thumbnail max-width-60"/>
                                                    </td>
                                                    <td>
                                                        <asp:HyperLink runat="server" Text='<%#Eval("GoodName")%>' data-module="good/goods" data-action="edit" data-parameters='<%#Eval("GoodId","id={0}")%>' Target="_blank"></asp:HyperLink>
                                                    </td>
                                                    <td>
                                                        <asp:HyperLink runat="server" Text='<%#Eval("CustomerName")%>' data-module="user" data-action="edit" data-parameters='<%#Eval("CustomerId","id={0}")%>' Target="_blank"></asp:HyperLink>
                                                    </td>
                                                    <td>
                                                        <%#Eval("AddDate","{0:yyyy-MM-dd HH:mm}")%>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                            <AlternatingItemTemplate>
                                                <tr class="active">
                                                    <td>
                                                        <asp:Image runat="server" ImageUrl='<%#Eval("GoodFigure", "~{0}")%>' CssClass="img-thumbnail max-width-60"/>
                                                    </td>
                                                    <td>
                                                        <asp:HyperLink runat="server" Text='<%#Eval("GoodName")%>' data-module="good/goods" data-action="edit" data-parameters='<%#Eval("GoodId","id={0}") %>' Target="_blank"></asp:HyperLink>
                                                    </td>
                                                    <td>
                                                        <asp:HyperLink runat="server" Text='<%#Eval("CustomerName")%>' data-module="user" data-action="edit" data-parameters='<%#Eval("CustomerId","id={0}")%>' Target="_blank"></asp:HyperLink>
                                                    </td>
                                                    <td>
                                                        <%#Eval("AddDate","{0:yyyy-MM-dd HH:mm}")%>
                                                    </td>
                                                </tr>
                                            </AlternatingItemTemplate>
                                            <FooterTemplate>
                                                </tbody>    
                                            </FooterTemplate>
                                        </asp:Repeater>
                                    </table>
                                </div>
                            </div>
                        </div>
                    </div>
                    <%} %>
                    <div role="tabpanel" class="tab-pane" id="receipt">
                        <div class="col-lg-6 col-md-8 col-sm-12 input-warp margin-top-20">
                            <ul class="list-group max-width-500">
                                <li class="list-group-item active font-size-14">
                                    <i class="fa fa-truck fa-fw"></i>&nbsp;用户的配送信息
                                </li>
                                <asp:Repeater ID="ReceiptList" runat="server">
                                    <ItemTemplate>
                                        <li class="list-group-item">
                                            <h4 class="list-group-item-heading">
                                                <asp:Literal runat="server" Text='<%#Eval("ProvinceName")%>'></asp:Literal>&nbsp;&nbsp;
                                                <asp:Literal runat="server" Text='<%#Eval("CityName")%>'></asp:Literal>&nbsp;&nbsp;
                                                <asp:Literal runat="server" Text='<%#Eval("AreaName")%>'></asp:Literal>
                                            </h4>
                                            <p class="list-group-item-text">
                                                <asp:Literal runat="server" Text='<%#Eval("Address","{0}<br/>") %>'></asp:Literal>
                                                <asp:Literal runat="server" Text='<%#Eval("Contact","联系人：{0}") %>'></asp:Literal>
                                                <asp:Literal runat="server" Text='<%#Eval("Phone","电话：{0}") %>'></asp:Literal>
                                                <asp:Literal runat="server" Text='<%#Eval("ZipCode","邮编：{0}") %>'></asp:Literal>
                                            </p>
                                        </li>
                                    </ItemTemplate>
                                </asp:Repeater>
                                <li class="list-group-item">
                                    <webdiyer:AspNetPager ID="ReceiptPager" runat="server" HorizontalAlign="Center" Wrap="False" FirstPageText="&lt;&lt;" PageSize="10" 
                                        LastPageText="&gt;&gt;" PagingButtonType="Text" ShowMoreButtons="False" ShowPrevNext="True" CurrentPageButtonClass="active" 
                                        CustomInfoHTML="%CurrentPageIndex% / %PageCount%" CustomInfoSectionWidth="20%" CssClass="pagination" NextPageText="&gt;" 
                                        PagingButtonSpacing="0px" PrevPageText="&lt;" ShowFirstLast="True" OnPageChanging="ReceiptPager_PageChanging" NumericButtonCount="8">
                                    </webdiyer:AspNetPager>
                                </li>
                            </ul>
                        </div> 
                    </div>
                    <div role="tabpanel" class="tab-pane" id="inte">
                        <div class="col-lg-8 col-md-10 col-sm-12 margin-top-20">
                            <div class="row">
                                <nav class="navbar navbar-default nav-select">
                                    <div class="container-fluid">
                                        <div class="navbar-header">
                                            <asp:Label ID="Integarl" runat="server" CssClass="navbar-brand"></asp:Label>
                                        </div>          
                                        <div class="collapse navbar-collapse">
                                            <ul class="nav navbar-nav navbar-left margin-left-15">
                                                <li>
                                                    <webdiyer:AspNetPager ID="IntegralPager" runat="server" HorizontalAlign="Center" Wrap="False" FirstPageText="&lt;&lt;" PageSize="15" 
                                                        LastPageText="&gt;&gt;" PagingButtonType="Text" ShowMoreButtons="False" ShowPrevNext="True" CurrentPageButtonClass="active" 
                                                        CustomInfoHTML="%CurrentPageIndex% / %PageCount%" CustomInfoSectionWidth="20%" CssClass="pagination" NextPageText="&gt;" 
                                                        PagingButtonSpacing="0px" PrevPageText="&lt;" ShowFirstLast="True" OnPageChanging="IntegralPager_PageChanging" NumericButtonCount="8">
                                                    </webdiyer:AspNetPager>
                                                </li>
                                            </ul>
                                            <ul class="nav navbar-nav navbar-right">
                                                <li class="dropdown">
                                                    <a href="javascript:;" class="dropdown-toggle" data-toggle="dropdown" role="button" aria-expanded="false">
                                                        <asp:Label ID="SourceName" runat="server" Text="来源"></asp:Label>
                                                        <span class="caret"></span>
                                                    </a>
                                                    <asp:Repeater ID="SourceList" runat="server" OnItemCommand="SourceList_ItemCommand">
                                                        <HeaderTemplate>
                                                            <ul class="dropdown-menu" role="menu">
                                                                <li>
                                                                    <asp:LinkButton runat="server" Text="查看全部" CommandName="Select" CommandArgument="0"></asp:LinkButton>
                                                                </li>
                                                                <li class="divider"></li>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                                <li>
                                                                    <asp:LinkButton runat="server" Text='<%#Eval("SourceName")%>' CommandName="Select" CommandArgument='<%#Eval("SourceId")%>'></asp:LinkButton>
                                                                </li>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            </ul>
                                                        </FooterTemplate>
                                                    </asp:Repeater>
                                                </li>
                                            </ul>
                                        </div>
                                    </div>
                                </nav>
                                <div class="table-responsive overflow-y-auto">
                                    <table class="table table-hover">
                                        <thead>
                                            <tr>
                                                <th>#</th>
                                                <th>来源</th>
                                                <th>积分</th>
                                                <th>获得时间</th>
                                            </tr>
                                        </thead>
                                        <asp:Repeater ID="IntegralListConatner" runat="server">
                                            <HeaderTemplate>
                                                <tbody>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <td>
                                                        <%#Container.ItemIndex + 1%>
                                                    </td>
                                                    <td>
                                                        <%#Eval("SourceName")%>
                                                    </td>
                                                    <td>
                                                        <%#Eval("Integral")%>
                                                    </td>
                                                    <td>
                                                        <%#Eval("AddDate","{0:yyyy-MM-dd HH:mm}")%>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                            <AlternatingItemTemplate>
                                                <tr class="active">
                                                    <td>
                                                        <%#Container.ItemIndex + 1%>
                                                    </td>
                                                    <td>
                                                        <%#Eval("SourceName")%>
                                                    </td>
                                                    <td>
                                                        <%#Eval("Integral")%>
                                                    </td>
                                                    <td>
                                                        <%#Eval("AddDate","{0:yyyy-MM-dd HH:mm}")%>
                                                    </td>
                                                </tr>
                                            </AlternatingItemTemplate>
                                            <FooterTemplate>
                                                </tbody>    
                                            </FooterTemplate>
                                        </asp:Repeater>
                                    </table>
                                </div>
                            </div>
                        </div>
                    </div>
                    <%if(UserRoleId == 102) {%>
                    <div role="tabpanel" class="tab-pane" id="share">
                        <div class="col-lg-8 col-md-10 col-sm-12 margin-top-20">
                            <div class="row">
                                <nav class="navbar navbar-default nav-select">
                                    <div class="container-fluid">
                                        <div class="navbar-header">
                                            <span class="navbar-brand">用户分享</span>
                                        </div>          
                                        <div class="collapse navbar-collapse">
                                            <ul class="nav navbar-nav navbar-left margin-left-15">
                                                <li>
                                                    <webdiyer:AspNetPager ID="SharePager" runat="server" HorizontalAlign="Center" Wrap="False" FirstPageText="&lt;&lt;" PageSize="15" 
                                                        LastPageText="&gt;&gt;" PagingButtonType="Text" ShowMoreButtons="False" ShowPrevNext="True" CurrentPageButtonClass="active" 
                                                        CustomInfoHTML="%CurrentPageIndex% / %PageCount%" CustomInfoSectionWidth="20%" CssClass="pagination" NextPageText="&gt;" 
                                                        PagingButtonSpacing="0px" PrevPageText="&lt;" ShowFirstLast="True" OnPageChanging="SharePager_PageChanging" NumericButtonCount="8">
                                                    </webdiyer:AspNetPager>
                                                </li>
                                            </ul>
                                        </div>
                                    </div>
                                </nav>
                                <div class="table-responsive overflow-y-auto">
                                    <table class="table table-hover">
                                        <thead>
                                            <tr>
                                                <th>图片</th>
                                                <th>名称</th>
                                                <th>分享时间</th>
                                                <th>状态</th>
                                            </tr>
                                        </thead>
                                        <asp:Repeater ID="ShareList" runat="server">
                                            <HeaderTemplate>
                                                <tbody class="middle">
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <td>
                                                        <asp:Image runat="server" ImageUrl='<%#Eval("GoodFigure", "~{0}")%>' CssClass="img-thumbnail max-width-60"/> 
                                                    </td>
                                                    <td>
                                                        <asp:HyperLink runat="server" Text='<%#Eval("GoodName")%>' data-module="good/goods" data-action="edit" data-parameters='<%#Eval("GoodId","id={0}") %>' Target="_blank"></asp:HyperLink>
                                                    </td>
                                                    <td>
                                                        <%#Eval("AddDate","{0:yyyy-MM-dd HH:mm}")%>
                                                    </td>
                                                    <td>
                                                        <%#Eval("Expired").ToString() == "True" ? "已过期" : "未过期"%>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                            <AlternatingItemTemplate>
                                                <tr class="active">
                                                    <td>
                                                        <asp:Image runat="server" ImageUrl='<%#Eval("GoodFigure", "~{0}")%>' CssClass="img-thumbnail max-width-60"/> 
                                                    </td>
                                                    <td>
                                                        <asp:HyperLink runat="server" Text='<%#Eval("GoodName")%>' data-module="good/goods" data-action="edit" data-parameters='<%#Eval("GoodId","id={0}") %>' Target="_blank"></asp:HyperLink>
                                                    </td>
                                                    <td>
                                                        <%#Eval("AddDate","{0:yyyy-MM-dd HH:mm}")%>
                                                    </td>
                                                    <td>
                                                        <%#Eval("Expired").ToString() == "True" ? "已过期" : "未过期"%>
                                                    </td>
                                                </tr>
                                            </AlternatingItemTemplate>
                                            <FooterTemplate>
                                                </tbody>    
                                            </FooterTemplate>
                                        </asp:Repeater>
                                    </table>
                                </div>
                            </div>
                        </div>
                    </div>
                    <%} %>
                    <div role="tabpanel" class="tab-pane" id="save">
                        <div class="col-lg-8 col-md-10 col-sm-12 margin-top-20">
                            <div class="row">
                                <nav class="navbar navbar-default nav-select">
                                    <div class="container-fluid">
                                        <div class="navbar-header">
                                            <span class="navbar-brand">用户收藏</span>
                                        </div>          
                                        <div class="collapse navbar-collapse">
                                            <ul class="nav navbar-nav navbar-left margin-left-15">
                                                <li>
                                                    <webdiyer:AspNetPager ID="SavePager" runat="server" HorizontalAlign="Center" Wrap="False" FirstPageText="&lt;&lt;" PageSize="15" 
                                                        LastPageText="&gt;&gt;" PagingButtonType="Text" ShowMoreButtons="False" ShowPrevNext="True" CurrentPageButtonClass="active" 
                                                        CustomInfoHTML="%CurrentPageIndex% / %PageCount%" CustomInfoSectionWidth="20%" CssClass="pagination" NextPageText="&gt;" 
                                                        PagingButtonSpacing="0px" PrevPageText="&lt;" ShowFirstLast="True" OnPageChanging="SavePager_PageChanging" NumericButtonCount="8">
                                                    </webdiyer:AspNetPager>
                                                </li>
                                            </ul>
                                        </div>
                                    </div>
                                </nav>
                                <div class="table-responsive overflow-y-auto">
                                    <table class="table table-hover">
                                        <thead>
                                            <tr>
                                                <th>图片</th>
                                                <th>名称</th>
                                                <th>收藏时间</th>
                                            </tr>
                                        </thead>
                                        <asp:Repeater ID="SaveList" runat="server">
                                            <HeaderTemplate>
                                                <tbody class="middle">
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <td>
                                                        <asp:Image runat="server" ImageUrl='<%#Eval("GoodFigure", "~{0}")%>' CssClass="img-thumbnail max-width-60"/> 
                                                    </td>
                                                    <td>
                                                        <asp:HyperLink runat="server" Text='<%#Eval("GoodName")%>' data-module="good/goods" data-action="edit" data-parameters='<%#Eval("GoodId","id={0}") %>' Target="_blank"></asp:HyperLink>
                                                    </td>
                                                    <td>
                                                        <%#Eval("AddDate","{0:yyyy-MM-dd HH:mm}")%>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                            <AlternatingItemTemplate>
                                                <tr class="active">
                                                    <td>
                                                        <asp:Image runat="server" ImageUrl='<%#Eval("GoodFigure", "~{0}")%>' CssClass="img-thumbnail max-width-60"/> 
                                                    </td>
                                                    <td>
                                                        <asp:HyperLink runat="server" Text='<%#Eval("GoodName")%>' data-module="good/goods" data-action="edit" data-parameters='<%#Eval("GoodId","id={0}") %>' Target="_blank"></asp:HyperLink>
                                                    </td>
                                                    <td>
                                                        <%#Eval("AddDate","{0:yyyy-MM-dd HH:mm}")%>
                                                    </td>
                                                </tr>
                                            </AlternatingItemTemplate>
                                            <FooterTemplate>
                                                </tbody>    
                                            </FooterTemplate>
                                        </asp:Repeater>
                                    </table>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

