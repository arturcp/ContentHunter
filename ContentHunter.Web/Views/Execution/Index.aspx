<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Execute Instructions
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="JavascriptContent" runat="server">
    <script src="<%: Url.Content("~/Scripts/execution.js") %>" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<h2>Execute Instructions</h2>

<% if (ViewBag.Message != null){ %>
    <div id="message">
        <%= ViewBag.Message %>
    </div>
<% } %>
<p>
    
</p>

<% using (Html.BeginForm("Create", "Execution")){ %>
    <input type="submit" value="Start" />
<% } %>

</asp:Content>
