Imports INDRA.Entidades
Imports INDRA.Negocio

Public Class EstudiantesController
    Inherits BaseJsonController

    Private ReadOnly _negocio As New NegEstudiante()

    Function Index() As ActionResult
        Return View()
    End Function

    ' --- Endpoints JSON que usa estudiantes.js ---

    <HttpGet>
    Function Listar(pagina As Integer?, tamano As Integer?) As ActionResult
        Return Responder(Function()
                             Dim r As EntPagina(Of EntEstudiante) = _negocio.Listar(If(pagina, 1), If(tamano, 5))
                             Return New With {.ok = True, .total = r.Total, .items = r.Items}
                         End Function)
    End Function

    <HttpPost>
    <ValidarTokenAjax>
    Function Guardar(entidad As EntEstudiante) As ActionResult
        Return Responder(Function()
                             Dim id As Integer = _negocio.Guardar(entidad)
                             Return New With {.ok = True, .id = id}
                         End Function)
    End Function

    <HttpPost>
    <ValidarTokenAjax>
    Function Eliminar(id_estudiante As Integer) As ActionResult
        Return Responder(Function()
                             _negocio.Eliminar(id_estudiante)
                             Return New With {.ok = True}
                         End Function)
    End Function

End Class
