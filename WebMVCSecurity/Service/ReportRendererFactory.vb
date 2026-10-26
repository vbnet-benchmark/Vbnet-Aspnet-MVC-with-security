Imports System.Configuration

''' <summary>
''' Resolves, at runtime, which IReportRenderer implementation to use. The decision rests solely on
''' the "AdvancedReportEngineEnabled" Web.config app-setting flag -- no UI control (dropdown, list,
''' etc.) is involved in this decision, only a server-side configuration setting.
''' </summary>
Public Class ReportRendererFactory

    Public Shared Function GetRenderer() As IReportRenderer
        Dim advancedEngineEnabled As String = ConfigurationManager.AppSettings("AdvancedReportEngineEnabled")
        If String.Equals(advancedEngineEnabled, "true", StringComparison.OrdinalIgnoreCase) Then
            Return New RazorReportRenderer
        Else
            Return New StaticReportRenderer
        End If
    End Function

End Class
