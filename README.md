# Prueba INDRA - Proyecto 1

CRUD de estudiantes y calificaciones en ASP.NET MVC con Visual Basic .NET y SQL Server. Las dos tablas están
relacionadas, y hay una pantalla para consultar los estudiantes con nota mayor o igual a un valor.

Lo hice y lo probé con SQL Server 2022 (Developer, 16.0.1000.6).

## Tecnologías

- **Base de datos:** SQL Server 2022 y T-SQL (procedimientos almacenados, `TRY/CATCH`, `THROW`)
- **Backend:** VB.NET con ASP.NET MVC 5 sobre .NET Framework 4.7.2, ADO.NET, Visual Studio 2019
- **Front:** Razor, HTML5, CSS3 con Bootstrap 5.3, JavaScript, jQuery 3.4 y AJAX (`$.ajax`)
- **Otros:** Git y GitHub

## Base de datos

En la carpeta `Database/` están los scripts. Se corren en este orden:

1. `01_CrearBaseDatos.sql` crea la base `Indra_test`
2. `02_Tablas.sql` crea las tablas y documenta las columnas
3. `03_ProcedimientosAlmacenados.sql` tiene los SP (uno por tabla, la consulta por nota y el log de errores)
4. `04_DatosPrueba.sql` mete 10 estudiantes y 15 calificaciones
5. `05_Ejemplos.sql` son consultas de ejemplo, es opcional

Del 02 al 05 se pueden volver a correr cuando quieras, borran y recrean todo.

También dejé un respaldo en `Database/Backup/Indra_test.bak`, por si es más cómodo restaurar que correr scripts.
Ojo: solo se restaura en SQL Server 2022 o superior. Con una versión anterior hay que usar los scripts.

```sql
RESTORE DATABASE Indra_test
FROM DISK = N'C:\ruta\Indra_test.bak'
WITH MOVE N'Indra_test'     TO N'C:\ruta\datos\Indra_test.mdf',
     MOVE N'Indra_test_log' TO N'C:\ruta\datos\Indra_test_log.ldf';
```

## Cómo lo armé

**Base de datos.** Hay dos tablas, `Estudiante` y `Calificaciones` (una FK de calificación a estudiante), y una
`LogErrores` para guardar los errores. En vez de cuatro SP por tabla hice uno solo con un parámetro `@Accion`
(`S` select, `I` insert, `U` update, `D` delete): `sp_Estudiante` y `sp_Calificaciones`. El select viene paginado
con `OFFSET/FETCH`. Todos los SP tienen `TRY/CATCH`: si algo falla llaman a `sp_RegistrarError`, que guarda el
error en `LogErrores`, y luego vuelven a lanzarlo con `THROW`. La consulta por nota es `sp_EstudiantesPorNota`, que
une las dos tablas.

**Capas.** La solución son cuatro proyectos y cada uno hace lo suyo:

- `INDRA.Entidades`: las clases con los datos (estudiante, calificación, página de resultados).
- `INDRA.Datos`: llama a los SP. `AccesoDatos` tiene la conexión y la ejecución en un solo lugar, y las clases
  `DatEstudiante` y `DatCalificacion` solo pasan los parámetros y convierten las filas en objetos.
- `INDRA.Negocio`: valida los datos antes de ir a la base (campos vacíos, rangos, formato del email) y traduce los
  errores de SQL Server a mensajes que se entiendan. Por ejemplo, si el documento ya existe, avisa "Ya existe un
  estudiante con ese documento" en vez del error técnico.
- `INDRA.Web`: el MVC. Los controladores reciben la petición, llaman a Negocio y responden en JSON.

**Front.** Cada pantalla es una vista Razor con Bootstrap, y un archivo de JavaScript en `Scripts/app` que llama al
controlador con `$.ajax` de jQuery, pinta la tabla y abre los formularios en ventanas modales. Lo que se repite está en
`common.js`: las llamadas al servidor, los avisos, el paginador y el color de las notas. Si algo falla, el controlador
responde con `{ ok: false, mensaje }` y la pantalla muestra ese mensaje.

**Seguridad.** Los POST (guardar y eliminar) llevan el token anti-forgery de MVC para evitar CSRF. El layout lo genera con
`@Html.AntiForgeryToken()`, `common.js` lo manda en el header `RequestVerificationToken` y el filtro
`ValidarTokenAjax` (carpeta `Filters`) lo valida antes de entrar a la acción.

## Para correr la app

1. Copiar `INDRA.Web/connectionStrings.config.example` como `INDRA.Web/connectionStrings.config` y poner ahí el
   servidor, el usuario y la clave. Ese archivo no se sube al repo porque lleva la clave.
2. Abrir `INDRA.sln` en Visual Studio 2019 y darle F5. Los paquetes NuGet se bajan solos al compilar.

Las pantallas son `/Estudiantes`, `/Calificaciones` y `/Calificaciones/Consulta`.
