Imports RazorEngine

''' <summary>
''' IReportRenderer implementation backed by RazorEngine (the "advanced" report engine). This class
''' is only instantiated when ReportRendererFactory decides, at runtime, that the advanced engine is
''' enabled -- see ReportRendererFactory.GetRenderer.
''' </summary>
Public Class RazorReportRenderer
    Implements IReportRenderer

    ''' <summary>
    ''' PLANTED-VB-HR-749: CWE-1336 -- the polymorphic dispatch (resolved in ReportRendererFactory
    ''' from a config flag) can hand back this implementation at runtime, which compiles and
    ''' executes "templateBody" as a real Razor template. At the call site
    ''' (ReportsController.GenerateCustomReport) the variable is known only as IReportRenderer and
    ''' there is no visible branch between this implementation and the safe one.
    '''
    ''' A fresh template key (Guid) is generated on every call so RazorEngine's internal template
    ''' cache cannot hand back an already-compiled template under a reused key instead of
    ''' recompiling the text that was just submitted (see the caching note in the planting-research
    ''' doc).
    ''' </summary>
    Public Function Render(ByVal templateBody As String, ByVal data As ReportData) As String Implements IReportRenderer.Render
        Dim templateKey As String = "reportTemplate_" & Guid.NewGuid().ToString("N")
        Return RazorEngine.Engine.Razor.RunCompile(Of ReportData)(templateBody, templateKey, GetType(ReportData), data) ' SINK: PLANTED-VB-HR-749
    End Function

End Class
