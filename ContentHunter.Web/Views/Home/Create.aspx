﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Execute Instructions
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<h2>Execute Instructions</h2>

<% if (ViewBag.Message != null){ %>
    <%= ViewBag.Message %>
<% } %>
<p>
    
</p>

<% using (Html.BeginForm()){ %>
    <input type="submit" value="Start" />
<% } %>

</asp:Content>