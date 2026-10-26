@Code
    ViewData("Title") = "Compose Report Template"
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

<h2>Compose Report Template</h2>
<p>Write the Subject and the report Template Body below. The Template Body supports placeholders
   such as {{CustomerName}}, {{OrderCount}} and {{ReportDate}}.</p>

@Using (Html.BeginForm("PreviewReport", "Reports", FormMethod.Post))
    @Html.AntiForgeryToken()
    @Html.Hidden("customerId", ViewBag.CustomerId)
    @<div class="form-group">
        <label>Subject</label>
        @Html.TextBox("reportTemplate.Subject", Nothing, New With {.class = "form-control"})
    </div>
    @<div class="form-group">
        <label>Template Body</label>
        @Html.TextArea("reportTemplate.TemplateBody", Nothing, 10, 60, New With {.class = "form-control"})
    </div>
    @<div class="form-group">
        <button type="submit" class="btn btn-primary">Preview (direct)</button>
    </div>
End Using

@Using (Html.BeginForm("SendReportToCustomer", "Reports", FormMethod.Post))
    @Html.AntiForgeryToken()
    @Html.Hidden("customerId", ViewBag.CustomerId)
    @Html.Hidden("reportTemplate.Subject", Nothing)
    @<div class="form-group">
        @Html.TextArea("reportTemplate.TemplateBody", Nothing, 4, 60, New With {.class = "form-control"})
    </div>
    @<div class="form-group">
        <button type="submit" class="btn btn-secondary">Send to customer</button>
    </div>
End Using

@Using (Html.BeginForm("GenerateOrderSummaryReport", "Reports", FormMethod.Post))
    @Html.AntiForgeryToken()
    @Html.Hidden("customerId", ViewBag.CustomerId)
    @Html.Hidden("reportTemplate.Subject", Nothing)
    @<div class="form-group">
        @Html.TextArea("reportTemplate.TemplateBody", Nothing, 4, 60, New With {.class = "form-control"})
    </div>
    @<div class="form-group">
        <button type="submit" class="btn btn-secondary">Generate order summary report</button>
    </div>
End Using

@Using (Html.BeginForm("GenerateCustomReport", "Reports", FormMethod.Post))
    @Html.AntiForgeryToken()
    @Html.Hidden("customerId", ViewBag.CustomerId)
    @Html.Hidden("reportTemplate.Subject", Nothing)
    @<div class="form-group">
        @Html.TextArea("reportTemplate.TemplateBody", Nothing, 4, 60, New With {.class = "form-control"})
    </div>
    @<div class="form-group">
        <button type="submit" class="btn btn-secondary">Generate custom report</button>
    </div>
End Using

@Using (Html.BeginForm("GenerateReportsForAllCustomers", "Reports", FormMethod.Post))
    @Html.AntiForgeryToken()
    @Html.Hidden("reportTemplate.Subject", Nothing)
    @<div class="form-group">
        @Html.TextArea("reportTemplate.TemplateBody", Nothing, 4, 60, New With {.class = "form-control"})
    </div>
    @<div class="form-group">
        <button type="submit" class="btn btn-secondary">Generate reports for all customers</button>
    </div>
End Using
