Imports INDRA.Negocio

''' <summary>
''' Base de los controladores que contestan JSON. Si todo sale bien devuelvo { ok: true, ... },
''' y si falla { ok: false, mensaje } con codigo 4xx o 5xx.
''' </summary>
Public MustInherit Class BaseJsonController
    Inherits System.Web.Mvc.Controller

    ''' <summary>Corre la accion y, si algo se rompe, lo convierto en un JSON de error.</summary>
    Protected Function Responder(accion As Func(Of Object)) As ActionResult
        ' Para que IIS no cambie mi JSON de error por su pagina de error.
        Response.TrySkipIisCustomErrors = True
        Try
            Return Json(accion(), JsonRequestBehavior.AllowGet)
        Catch ex As NegocioException
            ' Error esperado (validacion, duplicado...): este mensaje si va para el usuario.
            Response.StatusCode = 400
            Return Json(New With {.ok = False, .mensaje = ex.Message}, JsonRequestBehavior.AllowGet)
        Catch ex As Exception
            Response.StatusCode = 500
            Dim mensaje As String = "Ocurrió un error inesperado en el servidor."
            If HttpContext.IsDebuggingEnabled Then
                mensaje &= " (" & ex.GetType().Name & ": " & ex.Message & ")"
            End If
            Return Json(New With {.ok = False, .mensaje = mensaje}, JsonRequestBehavior.AllowGet)
        End Try
    End Function

End Class
