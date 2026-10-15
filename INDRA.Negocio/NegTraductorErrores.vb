Imports System.Data.SqlClient

''' <summary>
''' Paso los errores de SQL Server a un NegocioException con un mensaje que se entienda.
''' El detalle tecnico ya lo guardan los SP en LogErrores.
''' </summary>
Friend Module NegTraductorErrores

    ''' <param name="alEliminar">True si lo que fallo fue un delete.</param>
    Public Function Traducir(ex As SqlException, Optional alEliminar As Boolean = False) As NegocioException
        Dim msg As String = ex.Message

        ' Los errores que lanzan mis SP (THROW 50001, 50002...) ya traen un mensaje legible.
        If ex.Number >= 50000 Then
            Return New NegocioException(msg, ex)
        End If

        ' Busco la restriccion por su nombre porque eso no cambia con el idioma del servidor.
        If msg.Contains("UQ_Estudiante_documento") Then
            Return New NegocioException("Ya existe un estudiante con ese documento.", ex)
        End If
        If msg.Contains("UQ_Calificaciones_unica") Then
            Return New NegocioException("Ya existe una nota para ese estudiante, materia y periodo.", ex)
        End If
        If msg.Contains("FK_Calificaciones_Estudiante") Then
            Return New NegocioException(If(alEliminar,
                "No se puede eliminar el estudiante porque tiene calificaciones registradas.",
                "El estudiante seleccionado no existe."), ex)
        End If
        If msg.Contains("CK_Calificaciones_nota") Then
            Return New NegocioException("La nota debe estar entre 0 y 10.", ex)
        End If
        If msg.Contains("CK_Estudiante_edad") Then
            Return New NegocioException("La edad debe estar entre 3 y 100.", ex)
        End If
        If msg.Contains("CK_Estudiante_sexo") Then
            Return New NegocioException("El sexo debe ser M o F.", ex)
        End If

        Return New NegocioException("No se pudo completar la operación en la base de datos. Intenta de nuevo.", ex)
    End Function

End Module
