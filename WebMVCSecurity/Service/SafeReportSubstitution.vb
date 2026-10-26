''' <summary>
''' SAFE substitution scheme for report templates: a closed, enumerable set of placeholder tokens
''' ("{{CustomerName}}", "{{OrderCount}}", "{{ReportDate}}") replaced via literal String.Replace.
''' There is no expression evaluation, no loop, no conditional inside the template, and no text
''' other than these three tokens can have any effect -- unlike a template engine (RazorEngine),
''' this never compiles or executes the text the admin wrote.
''' </summary>
Public Class SafeReportSubstitution

    Private Const TOKEN_CUSTOMER As String = "{{CustomerName}}"
    Private Const TOKEN_ORDER_COUNT As String = "{{OrderCount}}"
    Private Const TOKEN_REPORT_DATE As String = "{{ReportDate}}"

    Public Shared Function Substitute(ByVal templateBody As String, ByVal data As ReportData) As String
        If templateBody Is Nothing Then
            Return String.Empty
        End If
        Dim result As String = templateBody
        result = result.Replace(TOKEN_CUSTOMER, data.CustomerName)
        result = result.Replace(TOKEN_ORDER_COUNT, data.OrderCount.ToString())
        result = result.Replace(TOKEN_REPORT_DATE, data.ReportDate.ToString("yyyy-MM-dd"))
        Return result
    End Function

End Class
