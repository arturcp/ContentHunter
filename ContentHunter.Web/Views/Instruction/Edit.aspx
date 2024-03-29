﻿<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.master" Inherits="System.Web.Mvc.ViewPage<ContentHunter.Web.Models.Instruction>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Edit Instruction
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<h2>Edit Instruction</h2>

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
            <%: Html.TextBoxFor(model => model.Url, new {@class = "urlInput", maxlength = 200 }) %>
            <%: Html.ValidationMessageFor(model => model.Url) %>
        </div>

        <div class="editor-label">
            <%: Html.LabelFor(model => model.Type) %>
        </div>
        <div class="editor-field">
            <select name="Type">
                <option value="0" <%= ViewData.Model.Type == 0 ? "selected='selected'": "" %>>Rss</option>
                <option value="1" <%= ViewData.Model.Type == 1 ? "selected='selected'" : "" %>>Html</option>
                <option value="2" <%= ViewData.Model.Type == 2 ? "selected='selected'": "" %>>Xml</option>
            </select>
            <%: Html.ValidationMessageFor(model => model.Type) %>
        </div>

        <div class="editor-label">
            <%: Html.LabelFor(model => model.Engine) %>
        </div>
        <div class="editor-field">
            <select name="Engine">
            <% foreach (ContentHunter.Web.Models.Engine engine in ViewBag.Engines){ %>
		        <option value="<%= engine.ClassName %>" <%= ViewData.Model.Engine == engine.ClassName ? "selected='selected'" : ""%>><%= engine.FriendlyName %></option>
                
	        <% }  %>
            </select>
            
            <%: Html.ValidationMessageFor(model => model.Engine) %>
        </div>

        <%--<div class="editor-label">
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
        </div>--%>

        <div class="editor-label">
            Is Recursive?
        </div>
        <div class="editor-field">
            <select name="IsRecursive">
                <option value="false" <%= ViewData.Model.IsRecursive? "" : "selected='selected'" %>>No</option>
                <option value="true" <%= ViewData.Model.IsRecursive? "selected='selected'" : "" %>>Yes</option>
            </select>
        </div>

        <div class="editor-label">
            Is Recurrent?
        </div>
        <div class="editor-field">
            <select name="IsRecurrent">
                <option value="false" <%= ViewData.Model.IsRecurrent? "" : "selected='selected'" %>>No</option>
                <option value="true" <%= ViewData.Model.IsRecurrent? "selected='selected'" : "" %>>Yes</option>
            </select>

            <%: Html.ValidationMessageFor(model => model.IsRecurrent) %>
        </div>

        <div class="editor-label">
            <%: Html.LabelFor(model => model.Categories) %> (split by commas)
        </div>
        <div class="editor-field">
            <%: Html.EditorFor(model => model.Categories)%>
            <%: Html.ValidationMessageFor(model => model.Categories)%>
        </div> 
        
        <div class="editor-label">
            <input type="checkbox" id="schedule" name="schedule" value="true"/> Advanced Settings
        </div> 
        <div class="editor-field" id="scheduleSettings">
            Run this instruction each <%: Html.TextBoxFor(model => model.FrequencyValue, new {@class = "frenquencyValue", maxlength = 5 }) %> 
            <select name="frequencyUnit">
                <option value="0">Never</option>
                <option value="1" <%: Model.FrequencyUnit == 1? "selected='selected'" : "" %>>Minute(s)</option>
                <option value="2" <%: Model.FrequencyUnit == 2? "selected='selected'" : "" %>>Hour(s)</option>
                <option value="3" <%: Model.FrequencyUnit == 3? "selected='selected'" : "" %>>Day(s)</option>
                <option value="4" <%: Model.FrequencyUnit == 4? "selected='selected'" : "" %>>Month(s)</option>
            </select><br />         
            Starting executions on <input id="ScheduledTo" class="scheduledTo" type="text" value="<%= Model.ScheduledTo.HasValue? Model.ScheduledTo.Value.ToString("dd/MM/yyyy HH:mm") : string.Empty %>" name="ScheduledTo" maxlength="16" /> (dd/mm/aaaa hh:mm)
        </div>            
        <% if (Model.FrequencyUnit > 0 && Model.FrequencyValue > 0) {%>
            <script type="text/javascript">
                $('#schedule').attr('checked', 'checked');
                $('#scheduleSettings').show();
            </script>
        <% } %>

        <p>
            <input type="submit" value="Save" />
        </p>
    </fieldset>
<% } %>

<div>
    <%: Html.ActionLink("Back to List", "Index") %>
</div>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="JavascriptContent" runat="server">
    <script src="<%: Url.Content("~/Scripts/instruction.js") %>" type="text/javascript"></script>
    <script src="<%: Url.Content("~/Scripts/jquery-ui-1.8.11.min.js") %>" type="text/javascript"></script>    
</asp:Content>

