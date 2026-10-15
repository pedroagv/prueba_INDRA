/* ============================================================================
   Proyecto 1 - Estudiantes y Calificaciones
   Script 05: Ejemplos de uso
   Consultas de ejemplo y revision del log de errores.
   ============================================================================ */

USE Indra_test;
GO

EXEC dbo.sp_Estudiante     @Accion = 'S';                       -- primeros 5 estudiantes (pagina 1)
EXEC dbo.sp_Estudiante     @Accion = 'S', @id_estudiante = 1;   -- un estudiante
EXEC dbo.sp_Estudiante     @Accion = 'S', @Pagina = 2, @TamanoPagina = 5;  -- pagina 2, 5 por pagina
EXEC dbo.sp_Calificaciones @Accion = 'S';                       -- primeras 5 calificaciones (pagina 1)
EXEC dbo.sp_EstudiantesPorNota @NotaMinima = 5;                 -- estudiantes con nota >= 5

/* Insert / Update / Delete:
EXEC dbo.sp_Estudiante @Accion = 'I', @documento = '1011', @nombres = 'Nuevo', @apellidos = 'Estudiante',
                       @edad = 15, @sexo = 'M';
EXEC dbo.sp_Estudiante @Accion = 'U', @id_estudiante = 11, @documento = '1011', @nombres = 'Nuevo',
                       @apellidos = 'Editado', @edad = 16, @sexo = 'M';
EXEC dbo.sp_Estudiante @Accion = 'D', @id_estudiante = 11;
*/

-- registro de prueba correr varias veces y ver el error en LogErrores
exec dbo.sp_Estudiante
@Accion=I, @id_estudiante=NULL, @documento=1014216596,
@nombres = 'pedro alejandro', @apellidos = 'guerrero velasco', @edad=25, @Sexo='F';


-- Revisar el log de errores
SELECT * FROM dbo.LogErrores ORDER BY id_log DESC;
GO

