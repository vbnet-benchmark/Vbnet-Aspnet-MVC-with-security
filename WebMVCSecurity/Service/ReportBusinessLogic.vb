''' <summary>
''' Business logic for building the body of an order-summary report. First hop of the
''' interprocedural chain: receives the template text the admin composed and forwards it to
''' ReportTemplateEngine, which is the class that actually invokes the template engine.
''' </summary>
Public Class ReportBusinessLogic

    Dim templateEngine As New ReportTemplateEngine

    ''' <summary>
    ''' First hop of PLANTED-VB-HR-748 (CWE-1336, interprocedural): "templateBody" comes, with no
    ''' check at all, from the admin-composed report template (see
    ''' ReportsController.GenerateOrderSummaryReport) and is forwarded as-is to
    ''' ReportTemplateEngine.Render.
    ''' </summary>
    Public Function BuildOrderSummaryReport(ByVal templateBody As String, ByVal customerName As String, ByVal orderCount As Integer) As String
        Dim data As New ReportData With {.CustomerName = customerName, .OrderCount = orderCount, .ReportDate = Date.Now}
        Return templateEngine.Render(templateBody, data)
    End Function

    ''' <summary>
    ''' First hop of the safe twin PLANTED-VB-HR-748-safe.
    ''' </summary>
    Public Function BuildOrderSummaryReportSafe(ByVal templateBody As String, ByVal customerName As String, ByVal orderCount As Integer) As String
        Dim data As New ReportData With {.CustomerName = customerName, .OrderCount = orderCount, .ReportDate = Date.Now}
        Return templateEngine.RenderSafe(templateBody, data)
    End Function

End Class
