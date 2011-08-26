<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<ContentHunter.Web.Models.Instruction>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Index
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<h2>Index</h2>

<p>
    <%: Html.ActionLink("Create New", "Create") %>
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
            IsRecursive
        </th>
        <th>
            StartedAt
        </th>
        <th>
            FinishedAt
        </th>
        <th>
            IsRecurrent
        </th>
        <th>
            Category
        </th>
        <th>
            IsOriginal
        </th>
        <th></th>
    </tr>

<% foreach (var item in Model) { %>
    <tr>
        <td>
            <%: Html.DisplayFor(modelItem => item.Url) %>
        </td>
        <td>
            <%: Html.DisplayFor(modelItem => item.Type) %>
        </td>
        <td>
            <%: Html.DisplayFor(modelItem => item.Engine) %>
        </td>
        <td>
            <%: Html.DisplayFor(modelItem => item.IsRecursive) %>
        </td>
        <td>
            <%: Html.DisplayFor(modelItem => item.StartedAt) %>
        </td>
        <td>
            <%: Html.DisplayFor(modelItem => item.FinishedAt) %>
        </td>
        <td>
            <%: Html.DisplayFor(modelItem => item.IsRecurrent) %>
        </td>
        <td>
            <%: Html.DisplayFor(modelItem => item.Category) %>
        </td>
        <td>
            <%: Html.DisplayFor(modelItem => item.IsOriginal) %>
        </td>
        <td>
            <%: Html.ActionLink("Edit", "Edit", new { id=item.Id }) %> |
            <%: Html.ActionLink("Details", "Details", new { id=item.Id }) %> |
            <%: Html.ActionLink("Delete", "Delete", new { id=item.Id }) %>
        </td>
    </tr>
<% } %>

</table>

</asp:Content>
