<%@ Page Title="" Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" CodeFile="add.aspx.cs" Inherits="AdminGoodAddon" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script type="text/javascript" src="/admin/share/js/good-categories.js"></script>
    <script type="text/javascript">
        hidden = $("#" + '<%=CategoryId.ClientID%>');
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="container-fluid padding-left-0">
        <div class="row">
            <div class="col-lg-6 col-md-8 col-sm-12 input-warp">
                <div class="input-group">
                    <span class="input-group-addon font-size-15">名&nbsp;&nbsp;&nbsp;&nbsp;称<i class="text-danger">*</i></span>
                    <asp:TextBox ID="GoodName" runat="server" CssClass="form-control max-width-b100" placeholder="输入商品名称"></asp:TextBox>
                </div>
                <p class="valid-group">                   
                    <asp:RequiredFieldValidator ID="Validator1" runat="server" ControlToValidate="GoodName" ValidationGroup="GoodValid" 
                        Display="Dynamic" CssClass="alert alert-warning" role="alert">
                        <i class="fa fa-ban text-danger"></i>&nbsp;&nbsp;必须填写商品名称
                    </asp:RequiredFieldValidator>
                </p>
                <div class="input-group">
                    <span class="input-group-addon">分&nbsp;&nbsp;&nbsp;&nbsp;类<i class="text-danger">*</i></span>
                    <div class="btn-group btn-group-line">
                        <button type="button" class="btn btn-default dropdown-toggle width-120" data-toggle="dropdown" aria-expanded="false">
                            <span data-for="menu-first">主分类</span>
                            <span class="caret"></span>
                        </button>
                        <asp:Repeater ID="CategoryMasterList" runat="server">
                            <HeaderTemplate>
                                <ul class="dropdown-menu dropdown-menu-control" data-role="menu-first" role="menu">
                            </HeaderTemplate>
                            <ItemTemplate>
                                    <li>
                                        <a href="javascript:;" data-value='<%#Eval("Id")%>' data-toggle="categorySubLoad">
                                            <%#Eval("Name")%>
                                        </a>
                                    </li>
                            </ItemTemplate>
                            <FooterTemplate>
                                </ul>
                            </FooterTemplate>
                        </asp:Repeater>
                    </div>
                    <div class="btn-group btn-group-line">
                        <button type="button" class="btn btn-default dropdown-toggle width-120" data-toggle="dropdown" aria-expanded="false">
                            <span data-for="menu-second">子分类</span>
                            <span class="caret"></span>
                        </button>
                        <ul class="dropdown-menu dropdown-menu-control" data-role="menu-second" role="menu">
                        </ul>
                    </div>
                </div>
                <p class="valid-group">
                    <asp:Label ID="CategoryValid" runat="server" CssClass="alert alert-warning hide" role="alert">
                        <i class="fa fa-ban text-danger"></i>&nbsp;&nbsp;必须选择商品分类
                    </asp:Label>
                </p>
                <div class="input-group">
                    <span class="input-group-addon">品&nbsp;&nbsp;&nbsp;&nbsp;牌<i class="text-danger">*</i></span>
                    <asp:DropDownList ID="BrandList" runat="server" CssClass="form-control max-width-120">
                    </asp:DropDownList>
                </div>
                <p class="valid-group"></p>
                <div class="input-group max-width-300">
                    <span class="input-group-addon font-size-15">原&nbsp;&nbsp;&nbsp;&nbsp;价<i class="text-danger">*</i></span>
                    <asp:TextBox ID="OriginalPrice" runat="server" CssClass="form-control" placeholder="商品原价"></asp:TextBox>
                    <span class="input-group-addon addon-right">元</span>
                </div>
                <p class="valid-group">                   
                    <asp:RequiredFieldValidator ID="Validator2" runat="server" ControlToValidate="OriginalPrice" ValidationGroup="GoodValid" 
                        Display="Dynamic" CssClass="alert alert-warning" role="alert">
                        <i class="fa fa-ban text-danger"></i>&nbsp;&nbsp;必须填写商品原价
                    </asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="Validator3" runat="server" ControlToValidate="OriginalPrice" ValidationGroup="GoodValid" 
                        Display="Dynamic" CssClass="alert alert-warning" role="alert" ValidationExpression="^\d+(\.\d+)?$">
                        <i class="fa fa-ban text-danger"></i>&nbsp;&nbsp;请输入数字
                    </asp:RegularExpressionValidator>
                </p>
                <div class="input-group max-width-300">
                    <span class="input-group-addon font-size-15">现&nbsp;&nbsp;&nbsp;&nbsp;价</span>
                    <asp:TextBox ID="PresentPrice" runat="server" CssClass="form-control" placeholder="商品的现价"></asp:TextBox>
                    <span class="input-group-addon addon-right">元</span>
                </div>
                <p class="valid-group">                   
                    <asp:RegularExpressionValidator ID="Validator4" runat="server" ControlToValidate="PresentPrice" ValidationGroup="GoodValid" 
                        Display="Dynamic" CssClass="alert alert-warning" role="alert" ValidationExpression="^\d+(\.\d+)?$">
                        <i class="fa fa-ban text-danger"></i>&nbsp;&nbsp;请输入数字
                    </asp:RegularExpressionValidator>
                </p>             
                <div class="input-group">
                    <span class="input-group-addon">计件单位<i class="text-danger">*</i></span>
                    <asp:TextBox ID="GoodUnit" runat="server" CssClass="form-control max-width-100" placeholder="商品计件单位" Text="件"></asp:TextBox>
                </div>
                <p class="valid-group">                   
                    <asp:RequiredFieldValidator ID="Validator5" runat="server" ControlToValidate="GoodUnit" ValidationGroup="GoodValid" 
                        Display="Dynamic" CssClass="alert alert-warning" role="alert">
                        <i class="fa fa-ban text-danger"></i>&nbsp;&nbsp;必须填写计件单位
                    </asp:RequiredFieldValidator>
                </p>            
                <div class="input-group">
                    <span class="input-group-addon">积&nbsp;&nbsp;&nbsp;&nbsp;分<i class="text-danger">*</i></span>
                    <asp:TextBox ID="GoodInte" runat="server" CssClass="form-control max-width-100" Text="0" placeholder="当前积分"></asp:TextBox>
                </div>
                <p class="valid-group">
                    <asp:RequiredFieldValidator ID="Validator6" runat="server" ControlToValidate="GoodInte" ValidationGroup="GoodValid" 
                        Display="Dynamic" CssClass="alert alert-warning" role="alert">
                        <i class="fa fa-ban text-danger"></i>&nbsp;&nbsp;必须填写商品积分
                    </asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="Validator7" runat="server" ControlToValidate="GoodInte" ValidationGroup="GoodValid" 
                        Display="Dynamic" CssClass="alert alert-warning" role="alert" ValidationExpression="^\d+$">
                        <i class="fa fa-ban text-danger"></i>&nbsp;&nbsp;请输入数字
                    </asp:RegularExpressionValidator>
                </p> 
                <div class="input-group max-width-300">
                    <span class="input-group-addon">分红比例<i class="text-danger">*</i></span>
                    <asp:TextBox ID="GoodBonus" runat="server" CssClass="form-control" placeholder="商品分红比例"></asp:TextBox>
                    <span class="input-group-addon addon-right">%</span>
                </div>
                <p class="valid-group">
                    <asp:RequiredFieldValidator ID="Validator8" runat="server" ControlToValidate="GoodBonus" ValidationGroup="GoodValid" 
                        Display="Dynamic" CssClass="alert alert-warning" role="alert">
                        <i class="fa fa-ban text-danger"></i>&nbsp;&nbsp;必须填写商品分红比例
                    </asp:RequiredFieldValidator>                   
                    <asp:RegularExpressionValidator ID="Validator9" runat="server" ControlToValidate="GoodBonus" ValidationGroup="GoodValid" 
                        Display="Dynamic" CssClass="alert alert-warning" role="alert" ValidationExpression="^\d+$">
                        <i class="fa fa-ban text-danger"></i>&nbsp;&nbsp;请输入数字
                    </asp:RegularExpressionValidator>
                </p>
                <div class="input-group max-width-300">
                    <span class="input-group-addon">奖金池<i class="text-danger">*</i></span>
                    <asp:TextBox ID="GoodGoldPool" runat="server" CssClass="form-control" placeholder="奖金池比例"></asp:TextBox>
                    <span class="input-group-addon addon-right">%</span>
                </div>
                <p class="valid-group">
                    <asp:RequiredFieldValidator ID="Validator10" runat="server" ControlToValidate="GoodGoldPool" ValidationGroup="GoodValid" 
                        Display="Dynamic" CssClass="alert alert-warning" role="alert">
                        <i class="fa fa-ban text-danger"></i>&nbsp;&nbsp;必须填写商品奖金池比例
                    </asp:RequiredFieldValidator> 
                    <asp:RegularExpressionValidator ID="Validator11" runat="server" ControlToValidate="GoodGoldPool" ValidationGroup="GoodValid" 
                        Display="Dynamic" CssClass="alert alert-warning" role="alert" ValidationExpression="^\d+$">
                        <i class="fa fa-ban text-danger"></i>&nbsp;&nbsp;请输入数字
                    </asp:RegularExpressionValidator>
                </p>
                <div class="input-group">
                    <span class="input-group-addon">注册日期</span>
                    <asp:TextBox ID="AddDate" runat="server" CssClass="form-control" Enabled="false"></asp:TextBox>
                </div>
                <p class="valid-group"></p>
                <div class="input-group">
                    <span class="input-group-addon">状&nbsp;&nbsp;&nbsp;&nbsp;态<i class="text-danger">*</i></span>
                    <asp:DropDownList ID="StateList" runat="server" CssClass="form-control max-width-100">
                    </asp:DropDownList>
                </div>
                <p class="valid-group"></p>
                <div class="form-group">
                    <label>商品介绍</label>
                    <asp:TextBox ID="GoodDsec" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="5" placeholder="填写商品介绍"></asp:TextBox>
                </div>
                <div class="form-group">
                    <label>规格参数</label>
                    <asp:TextBox ID="GoodSpec" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" placeholder="填写规格参数"></asp:TextBox>
                </div>
                <div class="form-group">
                    <label>售后服务</label>
                    <asp:TextBox ID="GoodService" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" placeholder="填写售后服务"></asp:TextBox>
                </div>
                <div class="input-group">
                    <asp:Button ID="SubmitButton" runat="server" Text="保存并下一步" CssClass="btn btn-primary margin-left-90" ValidationGroup="GoodValid" OnClick="SubmitButton_Click"/>
                    <a class="btn btn-default margin-left-15" data-module="good/goods" data-action="list">返回列表</a>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="CategoryId" runat="server"/>
</asp:Content>

