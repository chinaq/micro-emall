<%@ Page Title="" Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" CodeFile="list.aspx.cs" Inherits="AdminGoodBrandList" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="container-fluid padding-left-0">
        <div class="row">
            <nav class="navbar navbar-default nav-select">
                <div class="container-fluid">
                    <div class="navbar-header">
                        <span class="navbar-brand">品牌列表</span>
                    </div>          
                    <div class="collapse navbar-collapse">
                        <ul class="nav navbar-nav navbar-left">
                            <li>
                                <webdiyer:AspNetPager ID="Pager" runat="server" HorizontalAlign="Center" Wrap="False" FirstPageText="&lt;&lt;" PageSize="15" 
                                    LastPageText="&gt;&gt;" PagingButtonType="Text" ShowMoreButtons="False" ShowPrevNext="True" CurrentPageButtonClass="active" 
                                    CustomInfoHTML="%CurrentPageIndex% / %PageCount%" CustomInfoSectionWidth="20%" CssClass="pagination" NextPageText="&gt;" 
                                    PagingButtonSpacing="0px" PrevPageText="&lt;" ShowFirstLast="True" OnPageChanging="Pager_PageChanging" NumericButtonCount="8">
                                </webdiyer:AspNetPager>
                            </li>
                        </ul>                                       
                        <ul class="nav navbar-nav navbar-right">
                            <li>
                                <a data-module="good/brand" data-action="edit">
                                    <i class="fa fa-plus"></i>&nbsp;添加
                                </a>
                            </li>
                        </ul>
                        <div class="nav navbar-nav navbar-right">
                            <div class="input-group">
                                <asp:TextBox ID="SearchText" runat="server" CssClass="form-control width-200" placeholder="品牌名称"></asp:TextBox>
                                <span class="input-group-btn">
                                    <asp:LinkButton ID="SearchLink" runat="server" CssClass="btn btn-default" OnClick="SearchLink_Click">
                                        <i class="fa fa-search text-muted"></i>
                                    </asp:LinkButton>
                                </span>
                            </div>
                        </div>
                    </div>
                </div>
            </nav>
            <div class="table-responsive overflow-y-auto">
                <table class="table table-hover ">
                    <thead>
                        <tr>
                            <th>图片</th>
                            <th>名称</th>
                            <th>链接</th>
                            <th>操作</th>
                        </tr>
                    </thead>
                    <asp:Repeater ID="ListConatner" runat="server" OnItemCommand="ListConatner_ItemCommand">
                        <HeaderTemplate>
                            <tbody class="middle">
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <asp:Image runat="server" ImageUrl='<%#Eval("Logo")%>' CssClass="img-thumbnail max-width-30"/>
                                </td>
                                <td>
                                    <%#Eval("Name")%>
                                </td>
                                <td>
                                    <asp:HyperLink runat="server" NavigateUrl='<%#Eval("URL")%>' Text='<%#Eval("URL") %>' Target="_blank"></asp:HyperLink>
                                </td>
                                <td>                                  
                                    <a class="btn btn-primary btn-xs" data-module="good/brand" data-action="edit" data-parameters='<%#Eval("Id","id={0}")%>'>编辑</a>
                                    <asp:LinkButton ID="DeleteLink" runat="server" CssClass="btn btn-xs btn-primary" CommandName="Delete" CommandArgument='<%#Eval("Id")%>' 
                                        OnClientClick="return window.confirm('确定要删除吗?');">删除</asp:LinkButton>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <AlternatingItemTemplate>
                            <tr class="active">
                                <td>
                                    <asp:Image runat="server" ImageUrl='<%#Eval("Logo")%>' CssClass="img-thumbnail max-width-100"/>
                                </td>
                                <td>
                                    <%#Eval("Name")%>
                                </td>
                                <td>
                                    <asp:HyperLink runat="server" NavigateUrl='<%#Eval("URL")%>' Text='<%#Eval("URL") %>' Target="_blank"></asp:HyperLink>
                                </td>
                                <td>                                  
                                    <a class="btn btn-primary btn-xs" data-module="good/brand" data-action="edit" data-parameters='<%#Eval("Id","id={0}")%>'>编辑</a>
                                    <asp:LinkButton ID="DeleteLink" runat="server" CssClass="btn btn-xs btn-primary" CommandName="Delete" CommandArgument='<%#Eval("Id")%>' 
                                        OnClientClick="return window.confirm('确定要删除吗?');">删除</asp:LinkButton>
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

