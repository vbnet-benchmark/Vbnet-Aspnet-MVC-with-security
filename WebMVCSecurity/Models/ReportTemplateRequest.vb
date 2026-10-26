''' <summary>
''' Form model for the admin "custom report template" feature: an admin types the Subject and
''' TemplateBody (a free-text &lt;textarea&gt;, never a dropdown) of a report template that the
''' actions in ReportsController compile and render against real order/customer data.
''' </summary>
Public Class ReportTemplateRequest
    Public Property Subject As String
    Public Property TemplateBody As String
End Class
