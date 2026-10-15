''' <summary>
''' Una fila de sp_EstudiantesPorNota: los datos del estudiante con una de sus notas.
''' </summary>
Public Class EntNotaEstudiante
    Public Property id_estudiante As Integer
    Public Property documento As String
    Public Property nombres As String
    Public Property apellidos As String
    Public Property curso As String
    Public Property materia As String
    Public Property periodo As String
    Public Property nota As Decimal
End Class
