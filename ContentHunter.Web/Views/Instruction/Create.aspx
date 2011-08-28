<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.master" Inherits="System.Web.Mvc.ViewPage<ContentHunter.Web.Models.Instruction>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Create
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<h2>Create</h2>

<script src="<%: Url.Content("~/Scripts/jquery.validate.min.js") %>" type="text/javascript"></script>
<script src="<%: Url.Content("~/Scripts/jquery.validate.unobtrusive.min.js") %>" type="text/javascript"></script>

<% using (Html.BeginForm()) { %>
    <%: Html.ValidationSummary(true) %>
    <fieldset>
        <legend>Instruction</legend>

        <div class="editor-label">
            <%: Html.LabelFor(model => model.Url) %>
        </div>
        <div class="editor-field">
            <%: Html.TextBoxFor(model => model.Url, new {@class = "urlInput", maxlength = 200 }) %>
            <%: Html.ValidationMessageFor(model => model.Url) %>
        </div>

        <div class="editor-label">
            <%: Html.LabelFor(model => model.Type) %>
        </div>
        <div class="editor-field">
            <select name="Type">
                <option value="0">Rss</option>
                <option value="1">Html</option>
                <option value="2">Xml</option>
            </select>
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
            Is Recursive?
        </div>
        <div class="editor-field">
            <select name="IsRecursive">
                <option value="false">No</option>
                <option value="true">Yes</option>
            </select>
            <%: Html.ValidationMessageFor(model => model.IsRecursive) %>
        </div>

        <div class="editor-label">
            Is Recurrent?
        </div>
        <div class="editor-field">
            <select name="IsRecurrent">
                <option value="false">No</option>
                <option value="true">Yes</option>
            </select>
            <%: Html.ValidationMessageFor(model => model.IsRecurrent) %>
        </div>

        <div class="editor-label">
            <%: Html.LabelFor(model => model.Category) %>
        </div>
        <div class="editor-field">
            <%: Html.EditorFor(model => model.Category) %>
            <%: Html.ValidationMessageFor(model => model.Category) %>
        </div>   

        <p>
            <input type="submit" value="Create" />
        </p>
    </fieldset>
<% } %>

<div>
    <%: Html.ActionLink("Back to List", "Index") %>
</div>

</asp:Content>
