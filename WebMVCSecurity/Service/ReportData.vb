''' <summary>
''' Data exposed to a report template as Model (@Model.CustomerName / @Model.OrderCount / ...).
''' These values are not the problem -- the problem is that the TEMPLATE TEXT itself (see
''' ReportTemplateRequest) is compiled and executed as Razor code instead of being treated as
''' static display markup.
''' </summary>
Public Class ReportData
    Public Property CustomerName As String
    Public Property OrderCount As Integer
    Public Property ReportDate As Date
End Class
