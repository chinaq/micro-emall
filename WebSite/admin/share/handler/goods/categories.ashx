<%@ WebHandler Language="C#" Class="GoodCategoriesHandler" %>

using System;
using System.Web;
using System.Text;
using MicroEmall.Models;
using System.Collections.Generic;
using Jumpcity.Utility.Extend;

public class GoodCategoriesHandler : IHttpHandler 
{    
    public void ProcessRequest (HttpContext context) 
    {
        string parentId = context.Request.GetString("pid");
        string writter = string.Empty;
        if (!General.IsNullable(parentId))
        {
            List<WMGoodCategories> list = WMGoodCategories.GetList(parentId);
            if (!General.IsNullable(list))
            {
                StringBuilder html = new StringBuilder();
                foreach (WMGoodCategories gc in list)
                {
                    html.Append("<li>");
                    html.AppendFormat("<a href=\"javascript:;\" data-value=\"{0}\" data-toggle=\"categorySubSave\">{1}</a>", gc.Id, gc.Name);
                    html.Append("</li>"); 
                }

                writter = html.ToString();     
            }     
        }
        
        context.Response.Write(writter);
    }
 
    public bool IsReusable 
    {
        get { return true; }
    }
}