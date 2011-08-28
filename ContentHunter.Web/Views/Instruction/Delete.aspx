<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.master" Inherits="System.Web.Mvc.ViewPage<ContentHunter.Web.Models.Instruction>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Delete
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<h2>Delete</h2>

<h3>Are you sure you want to delete this?</h3>
<fieldset id="deleteConfirmation">
    <legend>Instruction</legend>

    <% var item = ViewData.Model; %>

    <div class="display-label">Url</div>
    <div class="display-field">
        <%: Html.DisplayFor(model => model.Url) %>
    </div>

    <div class="display-label">Type</div>
    <div class="display-field">
        <%: item.Type == 0? "Rss" : item.Type == 1 ? "Html" : "Xml" %>
    </div>

    <div class="display-label">Engine</div>
    <div class="display-field">
        <%: Html.DisplayFor(model => model.Engine) %>
    </div>

    <div class="display-label">StartedAt</div>
    <div class="display-field">
        <%: Html.DisplayFor(model => model.StartedAt) %>
    </div>

    <div class="display-label">FinishedAt</div>
    <div class="display-field">
        <%: Html.DisplayFor(model => model.FinishedAt) %>
    </div>

    <div class="display-label">IsRecursive</div>
    <div class="display-field">
        <%: item.IsRecursive? "Yes": "No" %>
    </div>

    <div class="display-label">IsRecurrent</div>
    <div class="display-field">
        <%: item.IsRecurrent? "Yes": "No" %>
    </div>

    <div class="display-label">Category</div>
    <div class="display-field">
        <%: Html.DisplayFor(model => model.Category) %>
    </div>
       
</fieldset>
<% using (Html.BeginForm()) { %>
    <p>
        <input type="submit" value="Delete" /> |
        <%: Html.ActionLink("Back to List", "Index") %>
    </p>
<% } %>

</asp:Content>
