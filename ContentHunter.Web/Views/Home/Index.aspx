<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Content Hunter
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Home Page</h2>   

    <p>
        Welcome to Content Hunter Admin Page. Use the tabs above to configure your application.
    </p>
    
    <% if (ViewBag.Message != null)
       { %>
        <p>
            <%= ViewBag.Message%>
        </p>
    <% } %>
</asp:Content>
