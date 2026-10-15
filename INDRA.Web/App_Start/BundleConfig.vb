Imports System.Web.Optimization

Public Module BundleConfig
    ' Los estilos y scripts los pongo directo en Views/Shared/_Layout.vbhtml.
    ' No uso bundles porque el minificador de System.Web.Optimization 1.1.3 se rompe con el
    ' JavaScript de Bootstrap 5.
    Public Sub RegisterBundles(ByVal bundles As BundleCollection)
    End Sub
End Module
