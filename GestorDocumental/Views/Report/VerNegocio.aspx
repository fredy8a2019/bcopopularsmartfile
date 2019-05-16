<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <title>Ver Negocio</title>
    
    <style>
        div
        {
            width:100%;
            height:100%;    
        }
    </style>
</head>
<body>
    <div>
        <h1>
            Negocio <%=ViewData["NEG"]%>
        </h1>
          <object width="100%" height="100%" id="tiffobj0" classid="CLSID:106E49CF-797A-11D2-81A2-00E02C015623">
        <%--<param name="src" value="/Content/ArchivosCliente/405/405.tif">--%>
        <param name="src" value="<%: Session["IMG_VISOR"] %>">
        <param name="enableevents" value="32">
        <embed toolbaritems="<%: Session["TOOL_BAR"] %>" width="100%" height="100%" id="tiffemb0"
            enableevents="32" src="<%: Session["IMG_VISOR"] %>" access="8" type="application/x-alternatiff">
    </object>
    </div>
</body>
</html>
