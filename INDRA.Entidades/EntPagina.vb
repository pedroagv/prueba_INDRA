''' <summary>
''' Una pagina de resultados con el total de registros (el total real, sin paginar).
''' </summary>
Public Class EntPagina(Of T)
    Public Property Items As List(Of T) = New List(Of T)()
    Public Property Total As Integer
End Class
