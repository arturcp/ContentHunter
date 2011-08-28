<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Content Hunter
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Home Page</h2>   
    <p>
        <select>
        <% foreach (ContentHunter.Web.Models.Engine engine in ViewBag.Engines){ %>
		    <option value="<%= engine.ClassName %>"><%= engine.FriendlyName %></option>
	    <% }  %>
        </select>
    </p>
    <% if (ViewBag.Message != null)
       { %>
        <p>
            <%= ViewBag.Message%>
        </p>
    <% } %>
</asp:Content>
