<%@ Page Title="" Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" CodeFile="edit.aspx.cs" Inherits="AdminGoodCategoryEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="container-fluid padding-left-0">
        <div class="row">
            <div class="col-lg-6 col-md-8 col-sm-12 input-warp">
                <div class="form-group">
                    <label>品牌Logo</label>
                    <asp:Image ID="BrandLogo" runat="server" CssClass="img-thumbnail width-100 block margin-bottom-10"/>
                    <asp:FileUpload ID="ImageUpload" runat="server" CssClass="pointer"/>   
                </div>
                <p class="valid-group"></p>                              
                <div class="input-group">
                    <span class="input-group-addon">品牌名称<i class="text-danger">*</i></span>
                    <asp:TextBox ID="BrandName" runat="server" CssClass="form-control" placeholder="输入品牌名称"></asp:TextBox>
                </div>
                <p class="valid-group">                   
                    <asp:RequiredFieldValidator ID="Validator1" runat="server" ControlToValidate="BrandName" ValidationGroup="BrandValid" 
                        Display="Dynamic" CssClass="alert alert-warning" role="alert">
                        <i class="fa fa-ban text-danger"></i>&nbsp;&nbsp;必须填写品牌名称
                    </asp:RequiredFieldValidator>
                </p> 
                <div class="input-group">
                    <span class="input-group-addon">链&nbsp;&nbsp;&nbsp;&nbsp;接</span>
                    <asp:TextBox ID="BrandUrl" runat="server" CssClass="form-control max-width-b100"></asp:TextBox>
                </div>
                <p class="valid-group">
                    <asp:RegularExpressionValidator ID="Validator2" runat="server" ControlToValidate="BrandUrl" ValidationGroup="BrandValid"
                        ValidationExpression="^[a-zA-z]+://(\w+(-\w+)*)(\.(\w+(-\w+)*))*(\?\S*)?$" Display="Dynamic" CssClass="alert alert-warning" role="alert">
                        <i class="fa fa-ban text-danger"></i>&nbsp;&nbsp;请输入正确的URL地址格式
                    </asp:RegularExpressionValidator>
                </p>
                <div class="input-group">
                    <asp:Button ID="SubmitButton" runat="server" Text="保存" CssClass="btn btn-primary margin-left-90" ValidationGroup="BrandValid" OnClick="SubmitButton_Click"/>
                    <a class="btn btn-default margin-left-15" data-module="good/brand" data-action="list">返回列表</a>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

