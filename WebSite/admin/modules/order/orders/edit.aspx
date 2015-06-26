<%@ Page Title="" Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" CodeFile="edit.aspx.cs" Inherits="AdminOrderEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="container-fluid padding-left-0">
        <div class="row">
            <div id="orderEditTabs" class="pull-left width-b100" role="tabpanel" data-role="tabContainer">
                <div class="padding-left-15 pull-left padding-right-15">
                    <h5>订单详情</h5>
                </div>
                <ul class="nav nav-tabs padding-left-15" role="tablist">
                    <li role="presentation" class="active">
                        <a href="#edit" aria-controls="edit" role="tab" data-toggle="tab">基本信息</a>
                    </li>
                    <li role="presentation">
                        <a href="#receipts" aria-controls="receipts" role="tab" data-toggle="tab">配送信息</a>
                    </li>
                    <li role="presentation">
                        <a href="#goods" aria-controls="goods" role="tab" data-toggle="tab">商品明细</a>
                    </li>
                </ul>
                <div class="tab-content padding-left-15">
                    <div role="tabpanel" class="tab-pane active" id="edit">
                        <div class="col-lg-6 col-md-8 col-sm-12 input-warp margin-top-20">
                            <ul class="list-group max-width-500">
                                <li class="list-group-item active font-size-14">
                                    <i class="fa fa-list-alt fa-fw"></i>&nbsp;订单的基本信息
                                </li>
                                <li class="list-group-item">                                   
                                    <label>订单编号：</label>
                                    <asp:Label ID="OrderId" runat="server"></asp:Label> 
                                </li>
                                <li class="list-group-item">                                   
                                    <label>下单时间：</label>
                                    <asp:Label ID="AddDate" runat="server"></asp:Label> 
                                </li>
                                <li class="list-group-item">
                                    <label>所属用户：</label>
                                    <asp:Label ID="NickName" runat="server"></asp:Label>
                                </li>
                                <li class="list-group-item">
                                    <label>支付方式：</label>
                                    <asp:Label ID="PayName" runat="server"></asp:Label>
                                </li>
                                <li class="list-group-item">
                                    <label>运费金额：</label>
                                    <asp:Label ID="Freight" runat="server"></asp:Label>
                                </li>
                                <li class="list-group-item">
                                    <label>商品金额：</label>
                                    <asp:Label ID="GoodSum" runat="server"></asp:Label>
                                </li>
                                <li class="list-group-item">
                                    <label>总计金额：</label>
                                    <asp:Label ID="OrderSum" runat="server"></asp:Label>
                                </li>
                                <li class="list-group-item">
                                    <label>付&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;款：</label>
                                    <asp:Label ID="PaidState" runat="server"></asp:Label>
                                </li>
                                <li class="list-group-item">
                                    <label>当前状态：</label>
                                    <asp:Label ID="StatusName" runat="server"></asp:Label>
                                </li>
                                <li class="list-group-item">
                                    <label>附加信息：</label>
                                    <p>
                                        <asp:Literal ID="OrderMessage" runat="server"></asp:Literal>
                                    </p>
                                </li>
                            </ul>
                        </div>
                    </div>
                    <div role="tabpanel" class="tab-pane" id="receipts">
                        <div class="col-lg-6 col-md-8 col-sm-12 input-warp margin-top-20">
                            <ul class="list-group max-width-500">
                                <li class="list-group-item active font-size-14">
                                    <i class="fa fa-truck fa-fw"></i>&nbsp;订单的配送信息
                                </li>
                                <li class="list-group-item">                                   
                                    <label>联系人：</label>
                                    <asp:Label ID="OrderContact" runat="server"></asp:Label> 
                                </li>
                                <li class="list-group-item">                                   
                                    <label>所在省/直辖市：</label>
                                    <asp:Label ID="OrderProvince" runat="server"></asp:Label> 
                                </li>
                                <li class="list-group-item">
                                    <label>详细地址：</label>
                                    <asp:Label ID="OrderAddress" runat="server"></asp:Label>
                                </li>
                                <li class="list-group-item">
                                    <label>联系电话：</label>
                                    <asp:Label ID="OrderPhone" runat="server"></asp:Label>
                                </li>
                                <li class="list-group-item">
                                    <label>邮政编码：</label>
                                    <asp:Label ID="OrderZipCode" runat="server"></asp:Label>
                                </li>
                            </ul>
                        </div> 
                    </div>
                    <div role="tabpanel" class="tab-pane" id="goods">
                        <div class="col-lg-10 col-md-12 col-sm-12 input-warp margin-top-20">
                            <div class="table-responsive overflow-y-auto">
                                <table class="table table-hover ">
                                    <thead>
                                        <tr>
                                            <th>封面</th>
                                            <th>名称</th>
                                            <th>单价</th>
                                            <th>数量</th>
                                            <th>积分</th>
                                            <th>金额小计</th>
                                        </tr>
                                    </thead>
                                    <asp:Repeater ID="ListConatner" runat="server">
                                        <HeaderTemplate>
                                            <tbody class="middle">
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td>
                                                    <asp:Image runat="server" ImageUrl='<%#Eval("GoodFigure") %>' CssClass="img-thumbnail max-width-60"/>
                                                </td>
                                                <td>
                                                    <asp:HyperLink runat="server" Text='<%#Eval("GoodName")%>' data-module="good/goods" data-action="edit" data-parameters='<%#Eval("GoodId","id={0}")%>' Target="_blank"></asp:HyperLink> 
                                                </td>
                                                <td>
                                                    <%#Eval("Price","{0:N2}")%>
                                                </td>
                                                <td>
                                                    <%#Eval("Count")%>
                                                </td>
                                                <td>
                                                    <%#Eval("GoodInteSubTotal")%>
                                                </td>
                                                <td>
                                                    <%#Eval("SubTotal","{0:N2}")%>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                        <AlternatingItemTemplate>
                                            <tr class="active">
                                                <td>
                                                    <asp:Image runat="server" ImageUrl='<%#Eval("GoodFigure") %>' CssClass="img-thumbnail max-width-60"/>
                                                </td>
                                                <td>
                                                    <asp:HyperLink runat="server" Text='<%#Eval("GoodName")%>' data-module="good/goods" data-action="edit" data-parameters='<%#Eval("GoodId","id={0}")%>' Target="_blank"></asp:HyperLink> 
                                                </td>
                                                <td>
                                                    <%#Eval("Price","{0:N2}")%>
                                                </td>
                                                <td>
                                                    <%#Eval("Count")%>
                                                </td>
                                                <td>
                                                    <%#Eval("GoodInteSubTotal")%>
                                                </td>
                                                <td>
                                                    <%#Eval("SubTotal","{0:N2}")%>
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
</asp:Content>

