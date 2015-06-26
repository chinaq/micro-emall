<%@ Application Language="C#" %>
<%@ Import Namespace="Jumpcity.Web" %>

<script runat="server">

    void Application_Start(object sender, EventArgs e) 
    {
    }

    void Application_AcquireRequestState(object sender, EventArgs e)
    {
        Helper.ApplicationAcquireRequestState(); 
    }
    
    void Application_End(object sender, EventArgs e) 
    {
    }
        
    void Application_Error(object sender, EventArgs e) 
    { 
    }

    void Session_Start(object sender, EventArgs e) 
    {
        Helper.SessionStart();
    }

    void Session_End(object sender, EventArgs e) 
    {
        // 在会话结束时运行的代码。 
        // 注意: 只有在 Web.config 文件中的 sessionstate 模式设置为
        // InProc 时，才会引发 Session_End 事件。如果会话模式设置为 StateServer
        // 或 SQLServer，则不引发该事件。
    }
       
</script>
