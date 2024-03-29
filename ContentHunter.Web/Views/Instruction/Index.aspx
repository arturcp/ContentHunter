﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<ContentHunter.Web.Models.Instruction>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Instructions
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="JavascriptContent" runat="server">
    <script src="<%: Url.Content("~/Scripts/execution.js") %>" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<h2>Instructions</h2>

<p>
    <%: Html.ActionLink("Create Instruction", "Create") %> | <a href="javascript:;" onclick="$('#form1').submit();">Start All</a>
</p>

<p class="pSubtitle">
    <span class="subtitleOn subtitle" title="Running instructions">Running</span> 
    <span class="subtitleNotRecurrent subtitle" title="These instructions are not recurrent and have run already. They will not execute anymore unless you change its configuration">Expected once and executed</span>
    <span class="subtitleOff subtitle" title="Stopped instructions">Stopped</span>
</p>


<% using (Html.BeginForm("Start", "Execution", new { Id = 0}, FormMethod.Post, new { Id = "form1" }))
   { %>
<% } %>


<table>
    <tr>
        <th>
            Status
        </th>
        <th>
            Url
        </th>
        <th>
            Type
        </th>
        <th>
            Engine
        </th>
        <%--<th>
            Started at
        </th>
        <th>
            Finished at
        </th>--%>
        <th>
            Recursive?
        </th>
        <th>
            Recurrent?
        </th>
        <th>
            Categories
        </th>
        <th>
            Execution Plan
        </th>
        <th>
            Schedule
        </th>
        <th></th>
    </tr>

<% foreach (var item in Model) { %>
    <tr>
        <td align="center">
            <% if (item.State)
               { %>
                <a href="javascript:;" id="status<%= item.Id %>" class="status on" title="Running"></a>
            <% }
               else if (!item.IsRecurrent && item.FinishedAt.HasValue)
               { %>
                <a href="javascript:;" id="status<%= item.Id %>" class="status notRecurrent" title="This instruction is not recurrent and has already run"></a>
            <% } else {%>
                <a href="javascript:;" id="status<%= item.Id %>" class="status off" title="Click to start"></a>   
            <% } %>
        </td>

        <td>
            <a href="<%= item.Url %>"><%= item.Url %></a>
        </td>
        <td>
            <%: item.Type == 0? "Rss" : item.Type == 1 ? "Html" : "Xml" %>
        </td>
        <td>
            <%: Html.DisplayFor(modelItem => item.Engine) %>
        </td>
        <%--<td>
            <%: Html.DisplayFor(modelItem => item.StartedAt) %>
        </td>
        <td>
            <%: Html.DisplayFor(modelItem => item.FinishedAt) %>
        </td>--%>
        <td>
            <%: item.IsRecursive? "Yes": "No" %>
        </td>
        <td>
            <%: item.IsRecurrent? "Yes": "No" %>
        </td>
        <td>
            <%: Html.DisplayFor(modelItem => item.Categories) %>
        </td>
        <td>
            <%: item.GetExecutionPlan() %>
        </td>
        <td>
            <%: Html.DisplayFor(modelItem => item.ScheduledTo) %>
        </td>
        <td>
            <%: Html.ActionLink("Edit", "Edit", new { id = item.Id }, new  { @class= "edit", title="edit" })%> 
            <%: Html.ActionLink("Delete", "Delete", new { id = item.Id }, new { @class = "delete", title = "delete" })%> 
            <%: Html.ActionLink("Log", "Details", new { id = item.Id }, new { @class = "log", title = "log" })%>
        </td>        
    </tr>
<% } %>

</table>

</asp:Content>
