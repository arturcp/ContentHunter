<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<ContentHunter.Web.Models.Instruction>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Index
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<h2>Index</h2>

<p>
    <%: Html.ActionLink("Create Instruction", "Create") %>
</p>
<table>
    <tr>
        <th>
            Url
        </th>
        <th>
            Type
        </th>
        <th>
            Engine
        </th>
        <th>
            Started at
        </th>
        <th>
            Finished at
        </th>
        <th>
            Recursive?
        </th>
        <th>
            Recurrent?
        </th>
        <th>
            Categories
        </th>
        <th></th>
    </tr>

<% foreach (var item in Model) { %>
    <tr>
        <td>
            <a href="<%= item.Url %>"><%= item.Url %></a>
        </td>
        <td>
            <%: item.Type == 0? "Rss" : item.Type == 1 ? "Html" : "Xml" %>
        </td>
        <td>
            <%: Html.DisplayFor(modelItem => item.Engine) %>
        </td>
        <td>
            <%: Html.DisplayFor(modelItem => item.StartedAt) %>
        </td>
        <td>
            <%: Html.DisplayFor(modelItem => item.FinishedAt) %>
        </td>
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
            <%: Html.ActionLink("Edit", "Edit", new { id=item.Id }) %> |
            <%: Html.ActionLink("Delete", "Delete", new { id=item.Id }) %>
        </td>
    </tr>
<% } %>

</table>

</asp:Content>
