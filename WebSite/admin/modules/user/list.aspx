<%@ Page Title="" Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" CodeFile="list.aspx.cs" Inherits="AdminUserList" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="container-fluid padding-left-0">
        <div class="row">
            <nav class="navbar navbar-default nav-select">
                <div class="container-fluid">
                    <div class="navbar-header">
                        <span class="navbar-brand">用户列表</span>
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
                            <li class="dropdown">
                                <a href="javascript:;" class="dropdown-toggle" data-toggle="dropdown" role="button" aria-expanded="false">
                                    <asp:Label ID="UserRoleName" runat="server" Text="类型"></asp:Label>
                                    <span class="caret"></span>
                                </a>
                                <asp:Repeater ID="RoleList" runat="server" OnItemCommand="RoleList_ItemCommand">
                                    <HeaderTemplate>
                                        <ul class="dropdown-menu" role="menu">
                                            <li>
                                                <asp:LinkButton runat="server" Text="查看全部" CommandName="Select" CommandArgument="0"></asp:LinkButton>
                                            </li>
                                            <li class="divider"></li>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                            <li>
                                                <asp:LinkButton runat="server" Text='<%#Eval("RoleName")%>' CommandName="Select" CommandArgument='<%#Eval("RoleId")%>'></asp:LinkButton>
                                            </li>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </ul>
                                    </FooterTemplate>
                                </asp:Repeater>
                            </li>
                            <li class="dropdown">
                                <a href="javascript:;" class="dropdown-toggle" data-toggle="dropdown" role="button" aria-expanded="false">
                                    <asp:Label ID="UserStateName" runat="server" Text="状态"></asp:Label>
                                    <span class="caret"></span>
                                </a>
                                <asp:Repeater ID="StateList" runat="server" OnItemCommand="StateList_ItemCommand">
                                    <HeaderTemplate>
                                        <ul class="dropdown-menu" role="menu">
                                            <li>
                                                <asp:LinkButton runat="server" Text="查看全部" CommandName="Select" CommandArgument="0"></asp:LinkButton>
                                            </li>
                                            <li class="divider"></li>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                            <li>
                                                <asp:LinkButton runat="server" Text='<%#Eval("StateName")%>' CommandName="Select" CommandArgument='<%#Eval("StateId")%>'></asp:LinkButton>
                                            </li>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </ul>
                                    </FooterTemplate>
                                </asp:Repeater>
                            </li>
                            <li>
                                <a data-module="user" data-action="add">
                                    <i class="fa fa-plus"></i>&nbsp;添加
                                </a>
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
                            <th>昵称</th>
                            <th>类型</th>
                            <th>手机</th>
                            <th>积分</th>
                            <th>添加时间</th>
                            <th>状态</th>
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
                                    <%#Eval("NickName")%>
                                </td>
                                <td>
                                    <%#Eval("RoleName")%>
                                </td>
                                <td>
                                    <%#Eval("Mobile")%>
                                </td>
                                <td>
                                    <%#Eval("Integral")%>
                                </td>
                                <td>
                                    <%#Eval("AddDate","{0:yyyy-MM-dd HH:mm}")%>
                                </td>
                                <td>
                                    <%#Eval("StatusName")%>
                                </td>
                                <td>
                                    <asp:HyperLink ID="EditLink" runat="server" CssClass="btn btn-primary btn-xs" data-module="user" data-action="edit" data-parameters='<%#Eval("Id","id={0}")%>'>编辑</asp:HyperLink>
                                    <asp:LinkButton ID="DeleteLink" runat="server" CssClass="btn btn-xs btn-primary" CommandName="Delete" CommandArgument='<%#Eval("Id")%>' 
                                        OnClientClick="return window.confirm('确定要删除吗?');">删除</asp:LinkButton>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <AlternatingItemTemplate>
                            <tr class="active">
                                <td>
                                    <%#Container.ItemIndex + 1%>
                                </td>
                                <td>
                                    <%#Eval("NickName")%>
                                </td>
                                <td>
                                    <%#Eval("RoleName")%>
                                </td>
                                <td>
                                    <%#Eval("Mobile")%>
                                </td>
                                <td>
                                    <%#Eval("Integral")%>
                                </td>
                                <td>
                                    <%#Eval("AddDate","{0:yyyy-MM-dd HH:mm}")%>
                                </td>
                                <td>
                                    <%#Eval("StatusName")%>
                                </td>
                                <td>
                                    <asp:HyperLink ID="EditLink" runat="server" CssClass="btn btn-primary btn-xs" data-module="user" data-action="edit" data-parameters='<%#Eval("Id","id={0}")%>'>编辑</asp:HyperLink>
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

