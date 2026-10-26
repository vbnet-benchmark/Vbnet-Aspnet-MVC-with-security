Imports RazorEngine

''' <summary>
''' Report-template rendering engine. This is the second hop of the interprocedural chain that
''' starts at ReportBusinessLogic: ReportBusinessLogic never calls RazorEngine directly, it
''' delegates to this class.
''' </summary>
Public Class ReportTemplateEngine

    ''' <summary>
    ''' PLANTED-VB-HR-748: CWE-1336 -- compiles and executes as a real Razor template a string that
    ''' arrives here, ultimately, from the admin-composed report template text (see
    ''' ReportBusinessLogic.BuildOrderSummaryReport and ReportsController.GenerateOrderSummaryReport).
    ''' Any "@{ ... }" block or "@(...)" expression the admin includes in "templateBody" executes
    ''' with the full privileges of the hosting process.
    ''' </summary>
    Public Function Render(ByVal templateBody As String, ByVal data As ReportData) As String
        Return RazorEngine.Razor.Parse(Of ReportData)(templateBody, data) ' SINK: PLANTED-VB-HR-748
    End Function

    ''' <summary>
    ''' PLANTED-VB-HR-748-safe: safe twin -- never compiles or executes "templateBody"; only
    ''' substitutes the closed set of known placeholder tokens.
    ''' </summary>
    Public Function RenderSafe(ByVal templateBody As String, ByVal data As ReportData) As String
        Return SafeReportSubstitution.Substitute(templateBody, data) ' SAFE_SINK: PLANTED-VB-HR-748-safe
    End Function

End Class
