<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.master" Inherits="System.Web.Mvc.ViewPage<ContentHunter.Web.Models.Instruction>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Edit
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<h2>Edit</h2>

<script src="<%: Url.Content("~/Scripts/jquery.validate.min.js") %>" type="text/javascript"></script>
<script src="<%: Url.Content("~/Scripts/jquery.validate.unobtrusive.min.js") %>" type="text/javascript"></script>

<% using (Html.BeginForm()) { %>
    <%: Html.ValidationSummary(true) %>
    <fieldset>
        <legend>Instruction</legend>

        <%: Html.HiddenFor(model => model.Id) %>

        <div class="editor-label">
            <%: Html.LabelFor(model => model.Url) %>
        </div>
        <div class="editor-field">
            <%: Html.EditorFor(model => model.Url) %>
            <%: Html.ValidationMessageFor(model => model.Url) %>
        </div>

        <div class="editor-label">
            <%: Html.LabelFor(model => model.Type) %>
        </div>
        <div class="editor-field">
            <%: Html.EditorFor(model => model.Type) %>
            <%: Html.ValidationMessageFor(model => model.Type) %>
        </div>

        <div class="editor-label">
            <%: Html.LabelFor(model => model.Engine) %>
        </div>
        <div class="editor-field">
            <%: Html.EditorFor(model => model.Engine) %>
            <%: Html.ValidationMessageFor(model => model.Engine) %>
        </div>

        <div class="editor-label">
            <%: Html.LabelFor(model => model.IsRecursive) %>
        </div>
        <div class="editor-field">
            <%: Html.EditorFor(model => model.IsRecursive) %>
            <%: Html.ValidationMessageFor(model => model.IsRecursive) %>
        </div>

        <div class="editor-label">
            <%: Html.LabelFor(model => model.StartedAt) %>
        </div>
        <div class="editor-field">
            <%: Html.EditorFor(model => model.StartedAt) %>
            <%: Html.ValidationMessageFor(model => model.StartedAt) %>
        </div>

        <div class="editor-label">
            <%: Html.LabelFor(model => model.FinishedAt) %>
        </div>
        <div class="editor-field">
            <%: Html.EditorFor(model => model.FinishedAt) %>
            <%: Html.ValidationMessageFor(model => model.FinishedAt) %>
        </div>

        <div class="editor-label">
            <%: Html.LabelFor(model => model.IsRecurrent) %>
        </div>
        <div class="editor-field">
            <%: Html.EditorFor(model => model.IsRecurrent) %>
            <%: Html.ValidationMessageFor(model => model.IsRecurrent) %>
        </div>

        <div class="editor-label">
            <%: Html.LabelFor(model => model.Category) %>
        </div>
        <div class="editor-field">
            <%: Html.EditorFor(model => model.Category) %>
            <%: Html.ValidationMessageFor(model => model.Category) %>
        </div>

        <div class="editor-label">
            <%: Html.LabelFor(model => model.IsOriginal) %>
        </div>
        <div class="editor-field">
            <%: Html.EditorFor(model => model.IsOriginal) %>
            <%: Html.ValidationMessageFor(model => model.IsOriginal) %>
        </div>

        <p>
            <input type="submit" value="Save" />
        </p>
    </fieldset>
<% } %>

<div>
    <%: Html.ActionLink("Back to List", "Index") %>
</div>

</asp:Content>
