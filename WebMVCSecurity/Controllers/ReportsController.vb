Imports RazorEngine

Namespace Controllers

    ''' <summary>
    ''' Admin "custom report template" feature: an admin composes a free-text order-summary report
    ''' template (never a dropdown of fixed templates) that the actions below compile and render
    ''' against real customer/order data read from DatabaseContext. See
    ''' ./benchmarking/vbnet/planting-research/vbnet-cwe-1336-planting-research.md for the CWE-1336
    ''' background this feature is built to carry.
    ''' </summary>
    <Authorize(Roles:="Admin")>
    Public Class ReportsController
        Inherits Controller

        Private db As DatabaseContext = New DatabaseContext()

        ' GET: Reports
        Function Index() As ActionResult
            Dim customers As List(Of AspNetUsers) = db.AspNetUsers.ToList()
            Return View(customers)
        End Function

        ' GET: Reports/ComposeTemplate
        Function ComposeTemplate(ByVal customerId As String) As ActionResult
            ViewBag.CustomerId = customerId
            Return View()
        End Function

        Private Function BuildReportData(ByVal customerId As String) As ReportData
            Dim customer As AspNetUsers = db.AspNetUsers.Find(customerId)
            Dim customerName As String = If(customer Is Nothing, customerId, customer.UserName)
            Dim orderCount As Integer = db.OdersView.Where(Function(o) o.user_id = customerId).Count()
            Return New ReportData With {.CustomerName = customerName, .OrderCount = orderCount, .ReportDate = Date.Now}
        End Function

        ' ------------------------------------------------------------------
        ' DIRECT tier (PLANTED-VB-HR-746 / -safe)
        ' ------------------------------------------------------------------

        <HttpPost()>
        Function PreviewReport(ByVal reportTemplate As ReportTemplateRequest, ByVal customerId As String) As ActionResult
            Dim data As ReportData = BuildReportData(customerId)
            ' PLANTED-VB-HR-746: CWE-1336 - compiles and executes, straight-line with no intermediate
            ' helper, the report template text the admin just typed into the compose-template form.
            Dim renderedBody As String = RazorEngine.Razor.Parse(Of ReportData)(reportTemplate.TemplateBody, data) ' SINK: PLANTED-VB-HR-746
            ViewData("Subject") = reportTemplate.Subject
            ViewData("RenderedBody") = renderedBody
            Return View("PreviewReport")
        End Function

        <HttpPost()>
        Function PreviewReportSafe(ByVal reportTemplate As ReportTemplateRequest, ByVal customerId As String) As ActionResult
            Dim data As ReportData = BuildReportData(customerId)
            ' PLANTED-VB-HR-746-safe: safe twin - literal placeholder substitution, the admin's text
            ' is never compiled or executed.
            Dim renderedBody As String = SafeReportSubstitution.Substitute(reportTemplate.TemplateBody, data) ' SAFE_SINK: PLANTED-VB-HR-746-safe
            ViewData("Subject") = reportTemplate.Subject
            ViewData("RenderedBody") = renderedBody
            Return View("PreviewReport")
        End Function

        ' ------------------------------------------------------------------
        ' INDIRECT tier (PLANTED-VB-HR-747 / -safe) - one hop through a private same-class helper.
        ' ------------------------------------------------------------------

        <HttpPost()>
        Function SendReportToCustomer(ByVal reportTemplate As ReportTemplateRequest, ByVal customerId As String) As ActionResult
            Dim data As ReportData = BuildReportData(customerId)
            Dim renderedBody As String = RenderReportForCustomer(reportTemplate.TemplateBody, data)
            ViewData("Subject") = reportTemplate.Subject
            ViewData("RenderedBody") = renderedBody
            Return View("PreviewReport")
        End Function

        <HttpPost()>
        Function SendReportToCustomerSafe(ByVal reportTemplate As ReportTemplateRequest, ByVal customerId As String) As ActionResult
            Dim data As ReportData = BuildReportData(customerId)
            Dim renderedBody As String = RenderReportForCustomerSafe(reportTemplate.TemplateBody, data)
            ViewData("Subject") = reportTemplate.Subject
            ViewData("RenderedBody") = renderedBody
            Return View("PreviewReport")
        End Function

        ''' <summary>
        ''' PLANTED-VB-HR-747: CWE-1336 - the public action above delegates here, and this private
        ''' same-class helper is the one that actually compiles and executes the admin's template.
        ''' </summary>
        Private Function RenderReportForCustomer(ByVal templateBody As String, ByVal data As ReportData) As String
            Return RazorEngine.Razor.Parse(Of ReportData)(templateBody, data) ' SINK: PLANTED-VB-HR-747
        End Function

        ''' <summary>
        ''' PLANTED-VB-HR-747-safe: safe twin of the private helper above.
        ''' </summary>
        Private Function RenderReportForCustomerSafe(ByVal templateBody As String, ByVal data As ReportData) As String
            Return SafeReportSubstitution.Substitute(templateBody, data) ' SAFE_SINK: PLANTED-VB-HR-747-safe
        End Function

        ' ------------------------------------------------------------------
        ' INTERPROCEDURAL tier (PLANTED-VB-HR-748 / -safe) - the real sink lives two hops down, in
        ' Service\ReportTemplateEngine.vb, reached through Service\ReportBusinessLogic.vb.
        ' ------------------------------------------------------------------

        <HttpPost()>
        Function GenerateOrderSummaryReport(ByVal reportTemplate As ReportTemplateRequest, ByVal customerId As String) As ActionResult
            Dim customer As AspNetUsers = db.AspNetUsers.Find(customerId)
            Dim customerName As String = If(customer Is Nothing, customerId, customer.UserName)
            Dim orderCount As Integer = db.OdersView.Where(Function(o) o.user_id = customerId).Count()
            Dim businessLogic As New ReportBusinessLogic
            Dim renderedBody As String = businessLogic.BuildOrderSummaryReport(reportTemplate.TemplateBody, customerName, orderCount)
            ViewData("Subject") = reportTemplate.Subject
            ViewData("RenderedBody") = renderedBody
            Return View("PreviewReport")
        End Function

        <HttpPost()>
        Function GenerateOrderSummaryReportSafe(ByVal reportTemplate As ReportTemplateRequest, ByVal customerId As String) As ActionResult
            Dim customer As AspNetUsers = db.AspNetUsers.Find(customerId)
            Dim customerName As String = If(customer Is Nothing, customerId, customer.UserName)
            Dim orderCount As Integer = db.OdersView.Where(Function(o) o.user_id = customerId).Count()
            Dim businessLogic As New ReportBusinessLogic
            Dim renderedBody As String = businessLogic.BuildOrderSummaryReportSafe(reportTemplate.TemplateBody, customerName, orderCount)
            ViewData("Subject") = reportTemplate.Subject
            ViewData("RenderedBody") = renderedBody
            Return View("PreviewReport")
        End Function

        ' ------------------------------------------------------------------
        ' TYPE/POLYMORPHISM-DEPENDENT tier (PLANTED-VB-HR-749 / -safe) - the concrete implementation
        ' ReportRendererFactory hands back depends only on the "AdvancedReportEngineEnabled"
        ' Web.config setting; no branch is visible at this call site.
        ' ------------------------------------------------------------------

        <HttpPost()>
        Function GenerateCustomReport(ByVal reportTemplate As ReportTemplateRequest, ByVal customerId As String) As ActionResult
            Dim data As ReportData = BuildReportData(customerId)
            Dim renderer As IReportRenderer = ReportRendererFactory.GetRenderer()
            Dim renderedBody As String = renderer.Render(reportTemplate.TemplateBody, data)
            ViewData("Subject") = reportTemplate.Subject
            ViewData("RenderedBody") = renderedBody
            Return View("PreviewReport")
        End Function

        ' ------------------------------------------------------------------
        ' DIVERSITY tier (PLANTED-VB-HR-750 / -safe) - the same admin template is recompiled and
        ' re-rendered once per customer inside a batch loop.
        ' ------------------------------------------------------------------

        <HttpPost()>
        Function GenerateReportsForAllCustomers(ByVal reportTemplate As ReportTemplateRequest) As ActionResult
            Dim renderedResults As New List(Of String)
            For Each customer As AspNetUsers In db.AspNetUsers.ToList()
                Dim orderCount As Integer = db.OdersView.Where(Function(o) o.user_id = customer.Id).Count()
                Dim data As New ReportData With {.CustomerName = customer.UserName, .OrderCount = orderCount, .ReportDate = Date.Now}
                ' PLANTED-VB-HR-750: CWE-1336 - inside the loop, the admin's report template is
                ' recompiled and re-executed once per customer.
                Dim renderedBody As String = RazorEngine.Razor.Parse(Of ReportData)(reportTemplate.TemplateBody, data) ' SINK: PLANTED-VB-HR-750
                renderedResults.Add(renderedBody)
            Next
            ViewData("Subject") = reportTemplate.Subject
            ViewData("RenderedResults") = renderedResults
            Return View("PreviewReportBatch")
        End Function

        <HttpPost()>
        Function GenerateReportsForAllCustomersSafe(ByVal reportTemplate As ReportTemplateRequest) As ActionResult
            Dim renderedResults As New List(Of String)
            For Each customer As AspNetUsers In db.AspNetUsers.ToList()
                Dim orderCount As Integer = db.OdersView.Where(Function(o) o.user_id = customer.Id).Count()
                Dim data As New ReportData With {.CustomerName = customer.UserName, .OrderCount = orderCount, .ReportDate = Date.Now}
                ' PLANTED-VB-HR-750-safe: safe twin - literal placeholder substitution inside the loop.
                Dim renderedBody As String = SafeReportSubstitution.Substitute(reportTemplate.TemplateBody, data) ' SAFE_SINK: PLANTED-VB-HR-750-safe
                renderedResults.Add(renderedBody)
            Next
            ViewData("Subject") = reportTemplate.Subject
            ViewData("RenderedResults") = renderedResults
            Return View("PreviewReportBatch")
        End Function

        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If disposing Then
                db.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub

    End Class
End Namespace
