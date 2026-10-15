Imports INDRA.Entidades

''' <summary>Aqui llamo a sp_Estudiante.</summary>
Public Class DatEstudiante

    Private Const Sp As String = "dbo.sp_Estudiante"

    Private ReadOnly _bd As AccesoDatos

    Public Sub New()
        _bd = New AccesoDatos()
    End Sub

    Public Function Listar(pagina As Integer, tamano As Integer) As EntPagina(Of EntEstudiante)
        Return _bd.EjecutarPagina(Of EntEstudiante)(Sp, pagina, tamano, AddressOf Mapear,
            AccesoDatos.Parametro("@Accion", "S"))
    End Function

    ''' <summary>Trae un estudiante, o Nothing si no existe.</summary>
    Public Function Obtener(idEstudiante As Integer) As EntEstudiante
        Dim lista As List(Of EntEstudiante) = _bd.EjecutarLista(Of EntEstudiante)(Sp, AddressOf Mapear,
            AccesoDatos.Parametro("@Accion", "S"),
            AccesoDatos.Parametro("@id_estudiante", idEstudiante))
        Return lista.FirstOrDefault()
    End Function

    ''' <summary>Inserta y devuelve el id nuevo.</summary>
    Public Function Insertar(e As EntEstudiante) As Integer
        Return _bd.EjecutarEscalar(Of Integer)(Sp,
            AccesoDatos.Parametro("@Accion", "I"),
            AccesoDatos.Parametro("@documento", e.documento),
            AccesoDatos.Parametro("@nombres", e.nombres),
            AccesoDatos.Parametro("@apellidos", e.apellidos),
            AccesoDatos.Parametro("@edad", e.edad),
            AccesoDatos.Parametro("@sexo", e.sexo),
            AccesoDatos.Parametro("@curso", e.curso),
            AccesoDatos.Parametro("@email", e.email),
            AccesoDatos.Parametro("@activo", e.activo))
    End Function

    Public Sub Actualizar(e As EntEstudiante)
        _bd.EjecutarNoConsulta(Sp,
            AccesoDatos.Parametro("@Accion", "U"),
            AccesoDatos.Parametro("@id_estudiante", e.id_estudiante),
            AccesoDatos.Parametro("@documento", e.documento),
            AccesoDatos.Parametro("@nombres", e.nombres),
            AccesoDatos.Parametro("@apellidos", e.apellidos),
            AccesoDatos.Parametro("@edad", e.edad),
            AccesoDatos.Parametro("@sexo", e.sexo),
            AccesoDatos.Parametro("@curso", e.curso),
            AccesoDatos.Parametro("@email", e.email),
            AccesoDatos.Parametro("@activo", e.activo))
    End Sub

    Public Sub Eliminar(idEstudiante As Integer)
        _bd.EjecutarNoConsulta(Sp,
            AccesoDatos.Parametro("@Accion", "D"),
            AccesoDatos.Parametro("@id_estudiante", idEstudiante))
    End Sub

    Private Shared Function Mapear(r As IDataRecord) As EntEstudiante
        Return New EntEstudiante With {
            .id_estudiante = Convert.ToInt32(r("id_estudiante")),
            .documento = Convert.ToString(r("documento")),
            .nombres = Convert.ToString(r("nombres")),
            .apellidos = Convert.ToString(r("apellidos")),
            .edad = Convert.ToInt32(r("edad")),
            .sexo = Convert.ToString(r("sexo")),
            .curso = Convert.ToString(r("curso")),
            .email = AccesoDatos.LeerNulo(Of String)(r, "email"),
            .activo = Convert.ToBoolean(r("activo")),
            .fecha_registro = Convert.ToDateTime(r("fecha_registro"))
        }
    End Function

End Class
