<%@ Page Title="" Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" CodeFile="upload.aspx.cs" Inherits="AdminGoodUpload" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="container-fluid padding-left-0">
        <div class="row">
            <div class="col-lg-12 col-md-12 col-sm-12 input-warp">
                <h3 class="title-edit">
                    商品图片
                </h3>
                <div class="form-group margin-bottom-10 pull-left max-width-300">
                    <label>上传图片</label>
                    <div class="max-width-200">
                        <asp:FileUpload ID="ImageUpload" runat="server" CssClass="max-width-140 pull-left"/>
                        <asp:Button ID="SubmitButton" runat="server" Text="上传" CssClass="btn btn-primary btn-xs pull-right" OnClick="SubmitButton_Click"/>
                    </div>                   
                </div>          
                <p class="valid-group margin-left-15">
                    <asp:Label ID="PicValid" runat="server" CssClass="alert alert-warning hide" role="alert">
                        <i class="fa fa-ban text-danger"></i>&nbsp;&nbsp;必须上传图片文件
                    </asp:Label>
                </p>
                <div class="max-width-b100 clear-both">         
                    <div class="form-group">
                        <label>图片列表</label>
                    </div>
                    <asp:Repeater ID="ImageRepeater" runat="server" OnItemCommand="ImageRepeater_ItemCommand">
                        <HeaderTemplate>
                            <div>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <div class="max-width-300 pull-left margin-right-15">
                                <asp:HyperLink runat="server" CssClass="thumbnail max-width-300 margin-bottom-10 margin-top-10" NavigateUrl='<%#Eval("URL", "~{0}")%>' Target="_blank">
                                    <asp:Image runat="server" ImageUrl='<%#Eval("URL", "~{0}")%>'/>
                                </asp:HyperLink>
                                <asp:LinkButton ID="DeleteButton" runat="server" CssClass="btn btn-xs btn-primary" CommandName="Delete" CommandArgument='<%#Eval("Id")%>' OnClientClick="return window.confirm('确定要删除吗?');">
                                    <i class="fa fa-trash-o fa-fw"></i>&nbsp;删除
                                </asp:LinkButton>
                                <asp:LinkButton ID="CoverButton" runat="server" CssClass="btn btn-xs btn-primary margin-left-10" Text="设为封面" CommandName="Cover" CommandArgument='<%#Eval("Id")%>' Visible='<%#Eval("IsCover").ToString() == "False" %>'>
                                    <i class="fa fa-laptop fa-fw"></i>&nbsp;设为封面
                                </asp:LinkButton>
                            </div>
                        </ItemTemplate>
                        <FooterTemplate>
                            </div>
                        </FooterTemplate>
                    </asp:Repeater>
                </div>                  
            </div>
        </div>
    </div>
</asp:Content>

