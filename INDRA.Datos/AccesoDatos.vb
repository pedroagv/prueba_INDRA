Imports System.Configuration
Imports System.Data.SqlClient
Imports INDRA.Entidades

''' <summary>
''' Aqui centralizo todo lo de SQL Server: la conexion, el comando y los parametros,
''' para no repetirlo en cada clase Dat*. Siempre trabajo con procedimientos almacenados.
''' La cadena de conexion sale del Web.config (connectionStrings.config).
''' </summary>
Public Class AccesoDatos

    Public Const NombreConexionPorDefecto As String = "IndraDB"

    Private ReadOnly _cadenaConexion As String

    ''' <param name="nombreConexion">Nombre de la conexion en el config (por defecto IndraDB).</param>
    Public Sub New(Optional nombreConexion As String = NombreConexionPorDefecto)
        Dim config As ConnectionStringSettings = ConfigurationManager.ConnectionStrings(nombreConexion)
        If config Is Nothing OrElse String.IsNullOrWhiteSpace(config.ConnectionString) Then
            Throw New ConfigurationErrorsException($"No se encontró la cadena de conexión '{nombreConexion}'.")
        End If
        _cadenaConexion = config.ConnectionString
    End Sub

    ''' <summary>Corro un SP y devuelvo el resultado como DataTable.</summary>
    Public Function EjecutarTabla(nombreSp As String, ParamArray parametros As SqlParameter()) As DataTable
        Dim tabla As New DataTable()
        Using cn As New SqlConnection(_cadenaConexion)
            Using cmd As SqlCommand = CrearComando(cn, nombreSp, parametros)
                Using adaptador As New SqlDataAdapter(cmd)
                    adaptador.Fill(tabla)
                End Using
            End Using
        End Using
        Return tabla
    End Function

    ''' <summary>Corro un SP y convierto cada fila en un objeto con la funcion que me pasan.</summary>
    Public Function EjecutarLista(Of T)(nombreSp As String, mapeo As Func(Of IDataRecord, T), ParamArray parametros As SqlParameter()) As List(Of T)
        Dim lista As New List(Of T)()
        Using cn As New SqlConnection(_cadenaConexion)
            Using cmd As SqlCommand = CrearComando(cn, nombreSp, parametros)
                cn.Open()
                Using lector As SqlDataReader = cmd.ExecuteReader()
                    While lector.Read()
                        lista.Add(mapeo(lector))
                    End While
                End Using
            End Using
        End Using
        Return lista
    End Function

    ''' <summary>
    ''' Corro un SP paginado. El SP tiene que recibir @Pagina y @TamanoPagina y devolver total_registros.
    ''' Si la pagina queda vacia (por ejemplo borre el ultimo registro de la ultima pagina),
    ''' igual saco el total real para que la pantalla pueda volver a la pagina anterior.
    ''' </summary>
    Public Function EjecutarPagina(Of T)(nombreSp As String, pagina As Integer, tamano As Integer, mapeo As Func(Of IDataRecord, T), ParamArray parametros As SqlParameter()) As EntPagina(Of T)
        Dim resultado As New EntPagina(Of T)()

        ' Un SqlParameter no se puede usar en dos comandos, asi que para la segunda consulta le hago una copia.
        Dim copiaParaReintento As SqlParameter() = parametros.Select(Function(p) CType(CType(p, ICloneable).Clone(), SqlParameter)).ToArray()

        Dim todos As New List(Of SqlParameter)(parametros)
        todos.Add(Parametro("@Pagina", pagina))
        todos.Add(Parametro("@TamanoPagina", tamano))

        resultado.Items = EjecutarLista(Of T)(nombreSp,
            Function(registro)
                resultado.Total = Convert.ToInt32(registro("total_registros"))
                Return mapeo(registro)
            End Function,
            todos.ToArray())

        If resultado.Items.Count = 0 AndAlso pagina > 1 Then
            resultado.Total = EjecutarPagina(Of T)(nombreSp, 1, 1, mapeo, copiaParaReintento).Total
        End If

        Return resultado
    End Function

    ''' <summary>Corro un SP que no devuelve datos (update, delete) y devuelvo cuantas filas toco.</summary>
    Public Function EjecutarNoConsulta(nombreSp As String, ParamArray parametros As SqlParameter()) As Integer
        Using cn As New SqlConnection(_cadenaConexion)
            Using cmd As SqlCommand = CrearComando(cn, nombreSp, parametros)
                cn.Open()
                Return cmd.ExecuteNonQuery()
            End Using
        End Using
    End Function

    ''' <summary>Devuelvo el primer valor de la primera fila; lo uso para el id que sale del insert.</summary>
    Public Function EjecutarEscalar(Of T)(nombreSp As String, ParamArray parametros As SqlParameter()) As T
        Using cn As New SqlConnection(_cadenaConexion)
            Using cmd As SqlCommand = CrearComando(cn, nombreSp, parametros)
                cn.Open()
                Dim resultado As Object = cmd.ExecuteScalar()
                If resultado Is Nothing OrElse resultado Is DBNull.Value Then
                    Return Nothing
                End If
                Return CType(Convert.ChangeType(resultado, GetType(T)), T)
            End Using
        End Using
    End Function

    ''' <summary>Crea el parametro; si el valor es Nothing lo mando como NULL.</summary>
    Public Shared Function Parametro(nombre As String, valor As Object) As SqlParameter
        Return New SqlParameter(nombre, If(valor, DBNull.Value))
    End Function

    ''' <summary>Lee una columna que puede venir NULL; si viene NULL devuelvo Nothing.</summary>
    Public Shared Function LeerNulo(Of T)(registro As IDataRecord, columna As String) As T
        Dim indice As Integer = registro.GetOrdinal(columna)
        If registro.IsDBNull(indice) Then
            Return Nothing
        End If
        Return CType(Convert.ChangeType(registro.GetValue(indice), GetType(T)), T)
    End Function

    Private Shared Function CrearComando(cn As SqlConnection, nombreSp As String, parametros As SqlParameter()) As SqlCommand
        Dim cmd As New SqlCommand(nombreSp, cn)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandTimeout = 30
        If parametros IsNot Nothing AndAlso parametros.Length > 0 Then
            cmd.Parameters.AddRange(parametros)
        End If
        Return cmd
    End Function

End Class
