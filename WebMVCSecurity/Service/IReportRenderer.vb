''' <summary>
''' Common contract for the different report-template rendering engines. This project's only
''' existing interface (Service/ProductRepo.vb) has a single implementer and is unrelated to
''' templating/rendering (see the CWE-1336 planting-research doc's survey) -- this is a brand-new
''' hierarchy introduced specifically to carry this round's "type/polymorphism-dependent" tier.
''' </summary>
Public Interface IReportRenderer
    Function Render(ByVal templateBody As String, ByVal data As ReportData) As String
End Interface
