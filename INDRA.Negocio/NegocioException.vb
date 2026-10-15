''' <summary>
''' Error de negocio con un mensaje que si se puede mostrar al usuario
''' (validaciones, duplicados, etc.).
''' </summary>
Public Class NegocioException
    Inherits Exception

    Public Sub New(mensaje As String)
        MyBase.New(mensaje)
    End Sub

    Public Sub New(mensaje As String, interna As Exception)
        MyBase.New(mensaje, interna)
    End Sub
End Class
