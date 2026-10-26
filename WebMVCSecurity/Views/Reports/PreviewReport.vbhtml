@Code
    ViewData("Title") = "Report Preview"
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

<h2>Report Preview</h2>
<h4>@ViewData("Subject")</h4>

<div class="panel panel-default">
    <div class="panel-body">
        @Html.Raw(ViewData("RenderedBody"))
    </div>
</div>

<a href="@Url.Action("Index", "Reports")" class="btn btn-default">Back to list</a>
