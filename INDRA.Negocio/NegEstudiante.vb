Imports System.Data.SqlClient
Imports System.Net.Mail
Imports INDRA.Datos
Imports INDRA.Entidades

''' <summary>Reglas de negocio de Estudiante: primero valido y despues llamo a DatEstudiante.</summary>
Public Class NegEstudiante

    Private Const MaxTamanoPagina As Integer = 1000

    Private ReadOnly _dat As New DatEstudiante()

    Public Function Listar(pagina As Integer, tamano As Integer) As EntPagina(Of EntEstudiante)
        Try
            Return _dat.Listar(Math.Max(1, pagina), Math.Min(Math.Max(1, tamano), MaxTamanoPagina))
        Catch ex As SqlException
            Throw NegTraductorErrores.Traducir(ex)
        End Try
    End Function

    ''' <summary>Si el id es 0 crea, si es mayor a 0 actualiza. Devuelve el id.</summary>
    Public Function Guardar(e As EntEstudiante) As Integer
        Validar(e)
        Try
            If e.id_estudiante > 0 Then
                _dat.Actualizar(e)
                Return e.id_estudiante
            End If
            Return _dat.Insertar(e)
        Catch ex As SqlException
            Throw NegTraductorErrores.Traducir(ex)
        End Try
    End Function

    Public Sub Eliminar(idEstudiante As Integer)
        If idEstudiante <= 0 Then
            Throw New NegocioException("Indica el estudiante a eliminar.")
        End If
        Try
            _dat.Eliminar(idEstudiante)
        Catch ex As SqlException
            Throw NegTraductorErrores.Traducir(ex, alEliminar:=True)
        End Try
    End Sub

    Private Shared Sub Validar(e As EntEstudiante)
        If e Is Nothing Then
            Throw New NegocioException("No se recibieron datos del estudiante.")
        End If

        e.documento = If(e.documento, "").Trim()
        e.nombres = If(e.nombres, "").Trim()
        e.apellidos = If(e.apellidos, "").Trim()
        e.curso = If(String.IsNullOrWhiteSpace(e.curso), Nothing, e.curso.Trim())
        e.email = If(String.IsNullOrWhiteSpace(e.email), Nothing, e.email.Trim())
        e.sexo = If(e.sexo, "").Trim().ToUpperInvariant()

        If e.documento = "" Then Throw New NegocioException("El documento es obligatorio.")
        If e.documento.Length > 20 Then Throw New NegocioException("El documento no puede superar 20 caracteres.")
        If e.nombres = "" Then Throw New NegocioException("Los nombres son obligatorios.")
        If e.nombres.Length > 100 Then Throw New NegocioException("Los nombres no pueden superar 100 caracteres.")
        If e.apellidos = "" Then Throw New NegocioException("Los apellidos son obligatorios.")
        If e.apellidos.Length > 100 Then Throw New NegocioException("Los apellidos no pueden superar 100 caracteres.")
        If e.edad < 3 OrElse e.edad > 100 Then Throw New NegocioException("La edad debe estar entre 3 y 100.")
        If e.sexo <> "M" AndAlso e.sexo <> "F" Then Throw New NegocioException("El sexo debe ser M o F.")
        If e.curso IsNot Nothing AndAlso e.curso.Length > 50 Then Throw New NegocioException("El curso no puede superar 50 caracteres.")
        If e.email IsNot Nothing Then
            If e.email.Length > 150 Then Throw New NegocioException("El email no puede superar 150 caracteres.")
            If Not EmailValido(e.email) Then Throw New NegocioException("El email no tiene un formato válido.")
        End If
    End Sub

    Private Shared Function EmailValido(email As String) As Boolean
        Try
            Dim direccion As New MailAddress(email)
            Return direccion.Address = email
        Catch ex As FormatException
            Return False
        End Try
    End Function

End Class
