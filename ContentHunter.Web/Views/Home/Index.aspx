<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Home Page
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2></h2>
    <p>
        To learn more about ASP.NET MVC visit <a href="http://asp.net/mvc" title="ASP.NET MVC Website">http://asp.net/mvc</a>.
    </p>
    <p>
        <select>
        <% foreach (ContentHunter.Web.Models.Engine engine in ViewBag.Engines){ %>
		    <option value="<%= engine.ClassName %>"><%= engine.FriendlyName %></option>
	    <% }  %>
        </select>
    </p>
    <p>
        <%= ViewBag.Message %>
    </p>
</asp:Content>
