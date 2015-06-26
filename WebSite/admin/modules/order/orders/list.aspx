<%@ Page Title="" Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" CodeFile="list.aspx.cs" Inherits="AdminOrderList" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="container-fluid padding-left-0">
        <div class="row">
            <nav class="navbar navbar-default nav-select">
                <div class="container-fluid">
                    <div class="navbar-header">
                        <span class="navbar-brand">订单列表</span>
                    </div>          
                    <div class="collapse navbar-collapse">
                        <ul class="nav navbar-nav navbar-left">
                            <li>
                                <webdiyer:AspNetPager ID="Pager" runat="server" HorizontalAlign="Center" Wrap="False" FirstPageText="&lt;&lt;" PageSize="15" 
                                    LastPageText="&gt;&gt;" PagingButtonType="Text" ShowMoreButtons="False" ShowPrevNext="True" CurrentPageButtonClass="active" 
                                    CustomInfoHTML="%CurrentPageIndex% / %PageCount%" CustomInfoSectionWidth="20%" CssClass="pagination" NextPageText="&gt;" 
                                    PagingButtonSpacing="0px" PrevPageText="&lt;" ShowFirstLast="True" OnPageChanging="Pager_PageChanging" NumericButtonCount="5">
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
                            <li class="dropdown">
                                <a href="javascript:;" class="dropdown-toggle" data-toggle="dropdown" role="button" aria-expanded="false">
                                    <asp:Label ID="PaidState" runat="server" Text="付款"></asp:Label>
                                    <span class="caret"></span>
                                </a>
                                <ul class="dropdown-menu" role="menu">
                                    <li>
                                        <asp:LinkButton runat="server" Text="查看全部" OnCommand="Paid_Command" CommandName="Select" CommandArgument="-1"></asp:LinkButton>
                                    </li>
                                    <li class="divider"></li>
                                    <li>
                                        <asp:LinkButton runat="server" Text="已付款" OnCommand="Paid_Command" CommandName="Select" CommandArgument="1"></asp:LinkButton>
                                    </li>
                                    <li>
                                        <asp:LinkButton runat="server" Text="未付款" OnCommand="Paid_Command" CommandName="Select" CommandArgument="0"></asp:LinkButton>
                                    </li>
                                </ul>
                            </li>
                            <li class="dropdown">
                                <a href="javascript:;" class="dropdown-toggle" data-toggle="dropdown" role="button" aria-expanded="false">
                                    <asp:Label ID="PayName" runat="server" Text="支付方式"></asp:Label>
                                    <span class="caret"></span>
                                </a>
                                <asp:Repeater ID="PayList" runat="server" OnItemCommand="PayList_ItemCommand">
                                    <HeaderTemplate>
                                        <ul class="dropdown-menu" role="menu">
                                            <li>
                                                <asp:LinkButton runat="server" Text="查看全部" CommandName="Select" CommandArgument="0"></asp:LinkButton>
                                            </li>
                                            <li class="divider"></li>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                            <li>
                                                <asp:LinkButton runat="server" Text='<%#Eval("PayName")%>' CommandName="Select" CommandArgument='<%#Eval("PayId")%>'>
                                                </asp:LinkButton>
                                            </li>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </ul>
                                    </FooterTemplate>
                                </asp:Repeater>
                            </li>
                            <li class="dropdown">
                                <a href="javascript:;" class="dropdown-toggle" data-toggle="dropdown" role="button" aria-expanded="false">
                                    <asp:Label ID="StateName" runat="server" Text="状态"></asp:Label>
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
                                                <asp:LinkButton runat="server" CommandName="Select" CommandArgument='<%#Eval("StateId")%>'>
                                                    <asp:Label runat="server" Text='<%#Eval("StateName")%>'></asp:Label>&nbsp;&nbsp;
                                                    <asp:Label runat="server" Text='<%#Eval("StateCount")%>' CssClass="label label-info"></asp:Label>
                                                </asp:LinkButton>
                                            </li>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </ul>
                                    </FooterTemplate>
                                </asp:Repeater>
                            </li>
                        </ul>
                        <div class="nav navbar-nav navbar-right margin-right-15">
                            <div class="input-group">
                                <asp:TextBox ID="IdSearch" runat="server" CssClass="form-control width-160" placeholder="订单编号"></asp:TextBox>
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
                <table class="table table-hover">
                    <thead>
                        <tr>
                            <th>#</th>
                            <th>订单号</th>
                            <th>用户</th>
                            <th>商品数</th>
                            <th>总金额</th>
                            <th>支付方式</th>
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
                                    <%#Eval("Id")%>
                                </td>
                                <td>
                                    <%#Eval("NickName")%>
                                </td>
                                <td>
                                    <%#Eval("GoodCount")%>
                                </td>
                                <td>
                                    <%#Eval("OrderSum")%>
                                </td>
                                <td>
                                    <%#Eval("PayName")%>
                                </td>
                                <td>
                                    <%#Eval("AddDate","{0:yyyy-MM-dd HH:mm}")%>
                                </td>
                                <td>
                                    <%#Eval("StatusName")%>
                                </td>
                                <td>
                                    <asp:HyperLink ID="EditLink" runat="server" CssClass="btn btn-primary btn-xs" data-module="order/orders" data-action="edit" data-parameters='<%#Eval("Id","id={0}")%>'>详情</asp:HyperLink>
                                    <asp:LinkButton ID="DeliverLink" runat="server" CssClass="btn btn-xs btn-primary" CommandName="Deliver" CommandArgument='<%#Eval("Id")%>' 
                                        OnClientClick="return window.confirm('确定要发货吗?');">发货</asp:LinkButton>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <AlternatingItemTemplate>
                            <tr class="active">
                                <td>
                                    <%#Container.ItemIndex + 1%>
                                </td>
                                <td>
                                    <%#Eval("Id")%>
                                </td>
                                <td>
                                    <%#Eval("NickName")%>
                                </td>
                                <td>
                                    <%#Eval("GoodCount")%>
                                </td>
                                <td>
                                    <%#Eval("OrderSum")%>
                                </td>
                                <td>
                                    <%#Eval("PayName")%>
                                </td>
                                <td>
                                    <%#Eval("AddDate","{0:yyyy-MM-dd HH:mm}")%>
                                </td>
                                <td>
                                    <%#Eval("StatusName")%>
                                </td>
                                <td>
                                    <asp:HyperLink ID="EditLink" runat="server" CssClass="btn btn-primary btn-xs" data-module="order/orders" data-action="edit" data-parameters='<%#Eval("Id","id={0}")%>'>详情</asp:HyperLink>
                                    <asp:LinkButton ID="DeliverLink" runat="server" CssClass="btn btn-xs btn-primary" CommandName="Deliver" CommandArgument='<%#Eval("Id")%>' 
                                        OnClientClick="return window.confirm('确定要发货吗?');">发货</asp:LinkButton>
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

