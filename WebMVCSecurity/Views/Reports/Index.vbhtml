@ModelType IEnumerable(Of WebMVCSecurity.AspNetUsers)
@Code
    ViewData("Title") = "Custom Report Templates"
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

<h2>Custom Report Templates</h2>
<p>Pick a customer and compose a report template to preview against their order data.</p>

<table class="table">
    <tr>
        <th>Customer</th>
        <th>Email</th>
        <th></th>
    </tr>

    @For Each item In Model
        @<tr>
            <td>
                @Html.DisplayFor(Function(modelItem) item.UserName)
            </td>
            <td>
                @Html.DisplayFor(Function(modelItem) item.Email)
            </td>
            <td>
                <button type="button" class="btn btn-primary" onclick="window.location.href='@Url.Action("ComposeTemplate", "Reports", New With {.customerId = item.Id})'">
                    <i class="fa fa-pencil" aria-hidden="true"></i> Compose report
                </button>
            </td>
        </tr>
    Next

</table>
