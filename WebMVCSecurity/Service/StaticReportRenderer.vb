''' <summary>
''' SAFE implementation of IReportRenderer: never compiles or executes "templateBody", only
''' substitutes the closed placeholder set via SafeReportSubstitution. This is what
''' ReportRendererFactory hands back when the advanced-engine config flag is disabled.
''' </summary>
Public Class StaticReportRenderer
    Implements IReportRenderer

    ''' <summary>
    ''' PLANTED-VB-HR-749-safe: safe twin of the polymorphic dispatch -- same interface, same call
    ''' site, but this implementation never invokes RazorEngine.
    ''' </summary>
    Public Function Render(ByVal templateBody As String, ByVal data As ReportData) As String Implements IReportRenderer.Render
        Return SafeReportSubstitution.Substitute(templateBody, data) ' SAFE_SINK: PLANTED-VB-HR-749-safe
    End Function

End Class
