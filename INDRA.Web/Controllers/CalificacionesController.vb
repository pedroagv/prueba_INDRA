Imports INDRA.Entidades
Imports INDRA.Negocio

Public Class CalificacionesController
    Inherits BaseJsonController

    Private ReadOnly _negocio As New NegCalificacion()

    Function Index() As ActionResult
        Return View()
    End Function

    Function Consulta() As ActionResult
        Return View()
    End Function

    ' --- Endpoints JSON que usan calificaciones.js y consulta.js ---

    <HttpGet>
    Function Listar(pagina As Integer?, tamano As Integer?, idEstudiante As Integer?) As ActionResult
        Return Responder(Function()
                             Dim r As EntPagina(Of EntCalificacion) = _negocio.Listar(If(pagina, 1), If(tamano, 5), idEstudiante)
                             Return New With {.ok = True, .total = r.Total, .items = r.Items}
                         End Function)
    End Function

    <HttpPost>
    <ValidarTokenAjax>
    Function Guardar(entidad As EntCalificacion) As ActionResult
        Return Responder(Function()
                             Dim id As Integer = _negocio.Guardar(entidad)
                             Return New With {.ok = True, .id = id}
                         End Function)
    End Function

    <HttpPost>
    <ValidarTokenAjax>
    Function Eliminar(id_calificacion As Integer) As ActionResult
        Return Responder(Function()
                             _negocio.Eliminar(id_calificacion)
                             Return New With {.ok = True}
                         End Function)
    End Function

    <HttpGet>
    Function PorNota(notaMinima As Decimal?) As ActionResult
        Return Responder(Function()
                             Dim items As List(Of EntNotaEstudiante) = _negocio.ListarPorNota(If(notaMinima, 5D))
                             Return New With {.ok = True, .total = items.Count, .items = items}
                         End Function)
    End Function

End Class
