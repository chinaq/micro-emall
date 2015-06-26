<%@ Page Title="" Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" CodeFile="list.aspx.cs" Inherits="AdminGoodCategoryList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="container-fluid padding-left-0">
        <div class="row">
            <nav class="navbar navbar-default nav-select">
                <div class="container-fluid">
                    <div class="navbar-header">
                        <div class="navbar-brand">
                            <asp:Label ID="NavbarBrandLabel" runat="server" Text="商品分类"></asp:Label>
                            <asp:Repeater ID="JoinConatner" runat="server">
                                <HeaderTemplate>
                                    <ol class="breadcrumb line-height-38 padding-0-10 font-size-13">
                                </HeaderTemplate>
                                <ItemTemplate>
                                        <li class='<%#Eval("Id").ToString().Equals(ParentId) ? "active" : ""%>'>
                                            <asp:HyperLink runat="server" Text='<%#Eval("Name")%>' NavigateUrl='<%#Eval("Id", "~/admin/modules/good/category/list.aspx?pid={0}") %>'></asp:HyperLink>
                                        </li> 
                                </ItemTemplate>
                                <FooterTemplate>
                                    </ol>
                                </FooterTemplate>
                            </asp:Repeater>
                        </div>
                    </div>          
                    <div class="collapse navbar-collapse">
                        <ul class="nav navbar-nav navbar-right">
                            <li>
                                <asp:HyperLink ID="AddLink" runat="server">
                                    <i class="fa fa-plus"></i>&nbsp;添加分类项
                                </asp:HyperLink>
                            </li>
                        </ul>
                    </div>
                </div>
            </nav>
            <div class="table-responsive overflow-y-auto">
                <table class="table table-hover ">
                    <thead>
                        <tr>
                            <th>#</th>
                            <th>名称</th>
                            <th>上级分类</th>
                            <th>层级</th>
                            <th>操作</th>
                        </tr>
                    </thead>
                    <asp:Repeater ID="ListConatner" runat="server" OnItemCommand="ListConatner_ItemCommand">
                        <HeaderTemplate>
                            <tbody>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <%#Container.ItemIndex + 1%>
                                </td>
                                <td>
                                    <%#Eval("Name")%>
                                </td>
                                <td>
                                    <%#Eval("ParentName")%>
                                </td>
                                <td>
                                    <%#Eval("Level")%>
                                </td>
                                <td>                                  
                                    <asp:LinkButton ID="EditLink" runat="server" CssClass="btn btn-xs btn-primary" CommandName="Edit" CommandArgument='<%#Eval("Id")%>'>编辑</asp:LinkButton>
                                    <asp:HyperLink ID="SelectLink" runat="server" CssClass="btn btn-primary btn-xs" data-module="good/category" data-action="list" data-parameters='<%#Eval("Id","pid={0}")%>' 
                                        Visible='<%#Convert.ToInt32(Eval("Level")) == 1%>'>
                                        <span>子分类</span> 
                                        <i class="fa fa-sign-out"></i>
                                    </asp:HyperLink>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <AlternatingItemTemplate>
                            <tr class="active">
                                <td>
                                    <%#Container.ItemIndex + 1%>
                                </td>
                                <td>
                                    <%#Eval("Name")%>
                                </td>
                                <td>
                                    <%#Eval("ParentName")%>
                                </td>
                                <td>
                                    <%#Eval("Level")%>
                                </td>
                                <td>                                  
                                    <asp:LinkButton ID="EditLink" runat="server" CssClass="btn btn-xs btn-primary" CommandName="Edit" CommandArgument='<%#Eval("Id")%>'>编辑</asp:LinkButton>
                                    <asp:HyperLink ID="SelectLink" runat="server" CssClass="btn btn-primary btn-xs" data-module="good/category" data-action="list" data-parameters='<%#Eval("Id","pid={0}")%>'
                                        Visible='<%#Convert.ToInt32(Eval("Level")) == 1%>'>
                                        <span>子分类</span> 
                                        <i class="fa fa-sign-out"></i>
                                    </asp:HyperLink>
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
</asp:Content>

