Imports System.Data.SqlClient
Imports INDRA.Datos
Imports INDRA.Entidades

''' <summary>Reglas de negocio de Calificaciones: primero valido y despues llamo a DatCalificacion.</summary>
Public Class NegCalificacion

    Private Const NotaMinimaPermitida As Decimal = 0D
    Private Const NotaMaximaPermitida As Decimal = 10D
    Private Const MaxTamanoPagina As Integer = 1000

    Private ReadOnly _dat As New DatCalificacion()

    Public Function Listar(pagina As Integer, tamano As Integer, idEstudiante As Integer?) As EntPagina(Of EntCalificacion)
        Try
            Return _dat.Listar(Math.Max(1, pagina), Math.Min(Math.Max(1, tamano), MaxTamanoPagina), idEstudiante)
        Catch ex As SqlException
            Throw NegTraductorErrores.Traducir(ex)
        End Try
    End Function

    ''' <summary>Si el id es 0 crea, si es mayor a 0 actualiza. Devuelve el id.</summary>
    Public Function Guardar(c As EntCalificacion) As Integer
        Validar(c)
        Try
            If c.id_calificacion > 0 Then
                _dat.Actualizar(c)
                Return c.id_calificacion
            End If
            Return _dat.Insertar(c)
        Catch ex As SqlException
            Throw NegTraductorErrores.Traducir(ex)
        End Try
    End Function

    Public Sub Eliminar(idCalificacion As Integer)
        If idCalificacion <= 0 Then
            Throw New NegocioException("Indica la calificación a eliminar.")
        End If
        Try
            _dat.Eliminar(idCalificacion)
        Catch ex As SqlException
            Throw NegTraductorErrores.Traducir(ex, alEliminar:=True)
        End Try
    End Sub

    ''' <summary>Estudiantes con notas mayores o iguales a la que pido.</summary>
    Public Function ListarPorNota(notaMinima As Decimal) As List(Of EntNotaEstudiante)
        If notaMinima < NotaMinimaPermitida OrElse notaMinima > NotaMaximaPermitida Then
            Throw New NegocioException("La nota mínima debe estar entre 0 y 10.")
        End If
        Try
            Return _dat.ListarPorNota(notaMinima)
        Catch ex As SqlException
            Throw NegTraductorErrores.Traducir(ex)
        End Try
    End Function

    Private Shared Sub Validar(c As EntCalificacion)
        If c Is Nothing Then
            Throw New NegocioException("No se recibieron datos de la calificación.")
        End If

        c.materia = If(c.materia, "").Trim()
        c.periodo = If(c.periodo, "").Trim()
        c.observacion = If(String.IsNullOrWhiteSpace(c.observacion), Nothing, c.observacion.Trim())
        c.nota = Math.Round(c.nota, 2)

        If c.id_estudiante <= 0 Then Throw New NegocioException("Selecciona un estudiante.")
        If c.materia = "" Then Throw New NegocioException("La materia es obligatoria.")
        If c.materia.Length > 100 Then Throw New NegocioException("La materia no puede superar 100 caracteres.")
        If c.periodo = "" Then Throw New NegocioException("El periodo es obligatorio.")
        If c.periodo.Length > 30 Then Throw New NegocioException("El periodo no puede superar 30 caracteres.")
        If c.nota < NotaMinimaPermitida OrElse c.nota > NotaMaximaPermitida Then Throw New NegocioException("La nota debe estar entre 0 y 10.")
        If c.observacion IsNot Nothing AndAlso c.observacion.Length > 200 Then Throw New NegocioException("La observación no puede superar 200 caracteres.")
    End Sub

End Class
