<%@ Page Title="" Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" CodeFile="edit.aspx.cs" Inherits="AdminGoodCategoryEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="container-fluid padding-left-0">
        <div class="row">
            <div class="col-lg-6 col-md-8 col-sm-12 input-warp">                              
                <div class="form-group">
                    <label>上级分类<i class="text-danger">*</i></label>
                    <h5 class="text-primary">
                        <asp:Literal ID="ParentName" runat="server" Text="root"></asp:Literal>
                    </h5>
                </div>
                <div class="input-group">
                    <span class="input-group-addon">名&nbsp;&nbsp;&nbsp;&nbsp;称<i class="text-danger">*</i></span>
                    <asp:TextBox ID="CategoryName" runat="server" CssClass="form-control" placeholder="输入分类名称"></asp:TextBox>
                </div>
                <p class="valid-group">                   
                    <asp:RequiredFieldValidator ID="Validator1" runat="server" ControlToValidate="CategoryName" ValidationGroup="CategoryValid" 
                        Display="Dynamic" CssClass="alert alert-warning" role="alert">
                        <i class="fa fa-ban text-danger"></i>&nbsp;&nbsp;必须填写分类名称
                    </asp:RequiredFieldValidator>
                </p>
                <div id="categoryE" runat="server">  
                    <div class="input-group">
                        <span class="input-group-addon">层&nbsp;&nbsp;&nbsp;&nbsp;级</span>
                        <asp:TextBox ID="CategoryLevel" runat="server" CssClass="form-control max-width-100" Enabled="false"></asp:TextBox>
                    </div>
                    <p class="valid-group"></p>
                    <div class="input-group">
                        <span class="input-group-addon">排&nbsp;&nbsp;&nbsp;&nbsp;序</span>
                        <asp:TextBox ID="CategorySort" runat="server" CssClass="form-control max-width-100" Enabled="false"></asp:TextBox>
                    </div>
                    <p class="valid-group"></p>
                </div>
                <div class="input-group">
                    <asp:Button ID="SubmitButton" runat="server" Text="保存" CssClass="btn btn-primary margin-left-90" ValidationGroup="CategoryValid" OnClick="SubmitButton_Click"/>
                    <a class="btn btn-default margin-left-15" data-module="good/category" data-action="list" data-parameters='<%=string.Format("pid={0}", ParentId)%>'>返回列表</a>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

