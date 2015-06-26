<%@ Page Language="C#" AutoEventWireup="true" CodeFile="index.aspx.cs" Inherits="AdminIndex" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta http-equiv="X-UA-Compatible" content="IE=edge"/>
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no"/>
    <title>后台数据中心</title>
    <link href="/share/css/bootstrap.min.css" rel="stylesheet" />
    <link href="/share/css/font-awesome.min.css" rel="stylesheet" />
</head>
<body>
    <div class="col-sm-3 col-md-3 col-lg-4"></div>
    <div class="col-sm-6 col-md-5 col-lg-3" style="margin-top:15%;">
        <div class="panel panel-primary">
            <div class="panel-heading">
                请先登陆
            </div>
            <div id="panel-body" class="panel-body">
                 <form id="from1" runat="server">
                     <div class="form-group">
                         <label>用户名</label>
                         <asp:TextBox ID="AdminName" runat="server" CssClass="form-control" placeholder="输入用户名" TabIndex="1"></asp:TextBox>
                     </div>
                     <div class="form-group">
                         <label>密码</label>
                         <asp:TextBox ID="AdminPassword" runat="server" CssClass="form-control" placeholder="输入登陆密码" TabIndex="2" TextMode="Password"></asp:TextBox>
                     </div>
                     <asp:Button ID="LoginButton" runat="server" Text="登陆" CssClass="btn btn-primary" TabIndex="3" OnClick="LoginButton_Click"/>
                 </form>  
            </div>
        </div>
    </div>
</body>
</html>
