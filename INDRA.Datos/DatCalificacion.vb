Imports INDRA.Entidades

''' <summary>Aqui llamo a sp_Calificaciones y a sp_EstudiantesPorNota.</summary>
Public Class DatCalificacion

    Private Const Sp As String = "dbo.sp_Calificaciones"
    Private Const SpPorNota As String = "dbo.sp_EstudiantesPorNota"

    Private ReadOnly _bd As AccesoDatos

    Public Sub New()
        _bd = New AccesoDatos()
    End Sub

    ''' <param name="idEstudiante">Si lo mando filtro por ese estudiante; si es Nothing traigo todos.</param>
    Public Function Listar(pagina As Integer, tamano As Integer, idEstudiante As Integer?) As EntPagina(Of EntCalificacion)
        Return _bd.EjecutarPagina(Of EntCalificacion)(Sp, pagina, tamano, AddressOf Mapear,
            AccesoDatos.Parametro("@Accion", "S"),
            AccesoDatos.Parametro("@id_estudiante", idEstudiante))
    End Function

    ''' <summary>Inserta y devuelve el id nuevo.</summary>
    Public Function Insertar(c As EntCalificacion) As Integer
        Return _bd.EjecutarEscalar(Of Integer)(Sp,
            AccesoDatos.Parametro("@Accion", "I"),
            AccesoDatos.Parametro("@id_estudiante", c.id_estudiante),
            AccesoDatos.Parametro("@materia", c.materia),
            AccesoDatos.Parametro("@periodo", c.periodo),
            AccesoDatos.Parametro("@nota", c.nota),
            AccesoDatos.Parametro("@observacion", c.observacion))
    End Function

    Public Sub Actualizar(c As EntCalificacion)
        _bd.EjecutarNoConsulta(Sp,
            AccesoDatos.Parametro("@Accion", "U"),
            AccesoDatos.Parametro("@id_calificacion", c.id_calificacion),
            AccesoDatos.Parametro("@id_estudiante", c.id_estudiante),
            AccesoDatos.Parametro("@materia", c.materia),
            AccesoDatos.Parametro("@periodo", c.periodo),
            AccesoDatos.Parametro("@nota", c.nota),
            AccesoDatos.Parametro("@observacion", c.observacion))
    End Sub

    Public Sub Eliminar(idCalificacion As Integer)
        _bd.EjecutarNoConsulta(Sp,
            AccesoDatos.Parametro("@Accion", "D"),
            AccesoDatos.Parametro("@id_calificacion", idCalificacion))
    End Sub

    ''' <summary>La consulta relacionada: estudiantes con al menos una nota mayor o igual a la que pido.</summary>
    Public Function ListarPorNota(notaMinima As Decimal) As List(Of EntNotaEstudiante)
        Return _bd.EjecutarLista(Of EntNotaEstudiante)(SpPorNota,
            Function(r) New EntNotaEstudiante With {
                .id_estudiante = Convert.ToInt32(r("id_estudiante")),
                .documento = Convert.ToString(r("documento")),
                .nombres = Convert.ToString(r("nombres")),
                .apellidos = Convert.ToString(r("apellidos")),
                .curso = Convert.ToString(r("curso")),
                .materia = Convert.ToString(r("materia")),
                .periodo = Convert.ToString(r("periodo")),
                .nota = Convert.ToDecimal(r("nota"))
            },
            AccesoDatos.Parametro("@NotaMinima", notaMinima))
    End Function

    Private Shared Function Mapear(r As IDataRecord) As EntCalificacion
        Return New EntCalificacion With {
            .id_calificacion = Convert.ToInt32(r("id_calificacion")),
            .id_estudiante = Convert.ToInt32(r("id_estudiante")),
            .estudiante = Convert.ToString(r("estudiante")),
            .materia = Convert.ToString(r("materia")),
            .periodo = Convert.ToString(r("periodo")),
            .nota = Convert.ToDecimal(r("nota")),
            .observacion = AccesoDatos.LeerNulo(Of String)(r, "observacion"),
            .fecha_registro = Convert.ToDateTime(r("fecha_registro"))
        }
    End Function

End Class
