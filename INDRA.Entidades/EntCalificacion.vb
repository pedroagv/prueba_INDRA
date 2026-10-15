''' <summary>
''' Una calificacion (tabla Calificaciones). Las propiedades se llaman igual que las columnas
''' y que las llaves del JSON que usa el front, por eso no van en PascalCase.
''' </summary>
Public Class EntCalificacion
    Public Property id_calificacion As Integer
    Public Property id_estudiante As Integer
    ''' <summary>Nombre completo del estudiante. Es solo para mostrar: lo trae el SP al consultar.</summary>
    Public Property estudiante As String
    Public Property materia As String
    Public Property periodo As String
    ''' <summary>De 0 a 10.</summary>
    Public Property nota As Decimal
    ''' <summary>Opcional, en la base acepta NULL.</summary>
    Public Property observacion As String
    Public Property fecha_registro As DateTime
End Class
