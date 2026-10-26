@Code
    ViewData("Title") = "Batch Report Preview"
    Layout = "~/Views/Shared/_Layout.vbhtml"
    Dim renderedResults = CType(ViewData("RenderedResults"), List(Of String))
End Code

<h2>Batch Report Preview</h2>
<h4>@ViewData("Subject")</h4>

@For Each renderedBody In renderedResults
    @<div class="panel panel-default">
        <div class="panel-body">
            @Html.Raw(renderedBody)
        </div>
    </div>
Next

<a href="@Url.Action("Index", "Reports")" class="btn btn-default">Back to list</a>
