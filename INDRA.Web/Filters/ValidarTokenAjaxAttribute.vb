Imports System.Web.Helpers

''' <summary>
''' Protege mis POST contra CSRF. El token anti-forgery lo mando desde el JS en el header
''' RequestVerificationToken, y la cookie del token la manda el navegador sola.
''' Si el token falta o no coincide, no dejo pasar la peticion y contesto con mi JSON de error.
''' </summary>
<AttributeUsage(AttributeTargets.Class Or AttributeTargets.Method)>
Public Class ValidarTokenAjaxAttribute
    Inherits FilterAttribute
    Implements IAuthorizationFilter

    Public Const NombreHeader As String = "RequestVerificationToken"

    Public Sub OnAuthorization(filterContext As AuthorizationContext) Implements IAuthorizationFilter.OnAuthorization
        Dim request As HttpRequestBase = filterContext.HttpContext.Request

        Try
            Dim cookie As HttpCookie = request.Cookies(AntiForgeryConfig.CookieName)
            Dim tokenCookie As String = If(cookie Is Nothing, Nothing, cookie.Value)
            AntiForgery.Validate(tokenCookie, request.Headers(NombreHeader))
        Catch ex As Exception
            ' Cualquier problema con el token (no viene, no coincide, esta vencido) lo trato como rechazo.
            Dim respuesta As HttpResponseBase = filterContext.HttpContext.Response
            respuesta.StatusCode = 400
            respuesta.TrySkipIisCustomErrors = True
            filterContext.Result = New JsonResult With {
                .Data = New With {.ok = False, .mensaje = "La sesión de la página venció o la petición no es válida. Recarga la página e intenta de nuevo."},
                .JsonRequestBehavior = JsonRequestBehavior.AllowGet
            }
        End Try
    End Sub

End Class
