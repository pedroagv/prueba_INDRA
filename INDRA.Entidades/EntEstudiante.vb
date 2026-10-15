''' <summary>
''' Un estudiante (tabla Estudiante). Las propiedades se llaman igual que las columnas
''' y que las llaves del JSON del front, por eso no uso PascalCase.
''' </summary>
Public Class EntEstudiante
    Public Property id_estudiante As Integer
    Public Property documento As String
    Public Property nombres As String
    Public Property apellidos As String
    Public Property edad As Integer
    ''' <summary>M o F.</summary>
    Public Property sexo As String
    Public Property curso As String
    ''' <summary>Opcional, en la base acepta NULL.</summary>
    Public Property email As String
    Public Property activo As Boolean = True
    Public Property fecha_registro As DateTime
End Class
