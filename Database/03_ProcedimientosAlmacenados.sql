/* ============================================================================
   Proyecto 1 - Estudiantes y Calificaciones
   Script 03: Procedimientos almacenados
   CRUD por tabla (sp_Estudiante, sp_Calificaciones), consulta relacionada y registro de errores.
   ============================================================================ */

USE Indra_test;
GO

-- Limpieza para poder re-ejecutar el script
IF OBJECT_ID(N'dbo.sp_EstudiantesPorNota', N'P') IS NOT NULL DROP PROCEDURE dbo.sp_EstudiantesPorNota;
IF OBJECT_ID(N'dbo.sp_Calificaciones',     N'P') IS NOT NULL DROP PROCEDURE dbo.sp_Calificaciones;
IF OBJECT_ID(N'dbo.sp_Estudiante',         N'P') IS NOT NULL DROP PROCEDURE dbo.sp_Estudiante;
IF OBJECT_ID(N'dbo.sp_RegistrarError',     N'P') IS NOT NULL DROP PROCEDURE dbo.sp_RegistrarError;
GO

-- ============================================================================
-- 3. PROCEDIMIENTOS ALMACENADOS
-- ============================================================================

-- Registra en LogErrores el error actual. Se invoca solo desde un bloque CATCH.
CREATE PROCEDURE dbo.sp_RegistrarError
    @Parametros NVARCHAR(1000) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO dbo.LogErrores (procedimiento, numero_error, severidad, estado, linea, mensaje, parametros)
    VALUES (ERROR_PROCEDURE(), ERROR_NUMBER(), ERROR_SEVERITY(), ERROR_STATE(),
            ERROR_LINE(), ERROR_MESSAGE(), @Parametros);
END
GO

/* CRUD de Estudiante
   @Accion: 'S' = Select (todos, o uno si se envia @id_estudiante)
            'I' = Insert  (devuelve el id generado)
            'U' = Update
            'D' = Delete                                                       */
CREATE PROCEDURE dbo.sp_Estudiante
    @Accion        CHAR(1),
    @id_estudiante INT          = NULL,
    @documento     VARCHAR(20)  = NULL,
    @nombres       VARCHAR(100) = NULL,
    @apellidos     VARCHAR(100) = NULL,
    @edad          INT          = NULL,
    @sexo          CHAR(1)      = NULL,
    @curso         VARCHAR(50)  = NULL,
    @email         VARCHAR(150) = NULL,
    @activo        BIT          = NULL,
    @Pagina        INT          = NULL,   -- solo 'S': pagina a mostrar (por defecto 1)
    @TamanoPagina  INT          = NULL    -- solo 'S': filas por pagina (por defecto 5)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        IF @Accion = 'S'
        BEGIN
            SET @Pagina       = ISNULL(@Pagina, 1);
            SET @TamanoPagina = ISNULL(@TamanoPagina, 5);
            IF @Pagina < 1 OR @TamanoPagina < 1
                THROW 50003, 'Pagina y TamanoPagina deben ser mayores a 0.', 1;

            SELECT id_estudiante, documento, nombres, apellidos, edad, sexo,
                   curso, email, activo, fecha_registro,
                   COUNT(*) OVER() AS total_registros
            FROM dbo.Estudiante
            WHERE @id_estudiante IS NULL OR id_estudiante = @id_estudiante
            ORDER BY id_estudiante
            OFFSET CAST(@Pagina - 1 AS BIGINT) * @TamanoPagina ROWS
            FETCH NEXT @TamanoPagina ROWS ONLY;
        END
        ELSE IF @Accion = 'I'
        BEGIN
            INSERT INTO dbo.Estudiante (documento, nombres, apellidos, edad, sexo, curso, email, activo)
            VALUES (@documento, @nombres, @apellidos, @edad, @sexo,
                    ISNULL(@curso, 'Sin asignar'), @email, ISNULL(@activo, 1));

            SELECT SCOPE_IDENTITY() AS id_estudiante;
        END
        ELSE IF @Accion = 'U'
        BEGIN
            UPDATE dbo.Estudiante
            SET documento = @documento,
                nombres   = @nombres,
                apellidos = @apellidos,
                edad      = @edad,
                sexo      = @sexo,
                curso     = ISNULL(@curso, curso),
                email     = @email,
                activo    = ISNULL(@activo, activo)
            WHERE id_estudiante = @id_estudiante;

            IF @@ROWCOUNT = 0
                THROW 50001, 'El estudiante no existe.', 1;
        END
        ELSE IF @Accion = 'D'
        BEGIN
            DELETE FROM dbo.Estudiante WHERE id_estudiante = @id_estudiante;

            IF @@ROWCOUNT = 0
                THROW 50001, 'El estudiante no existe.', 1;
        END
        ELSE
            THROW 50000, 'Accion no valida. Use S, I, U o D.', 1;
    END TRY
    BEGIN CATCH
        DECLARE @p1 NVARCHAR(1000) =
            N'@Accion=' + ISNULL(@Accion, N'NULL') +
            N', @id_estudiante=' + ISNULL(CAST(@id_estudiante AS NVARCHAR(20)), N'NULL') +
            N', @documento=' + ISNULL(@documento, N'NULL');
        EXEC dbo.sp_RegistrarError @Parametros = @p1;
        THROW;
    END CATCH
END
GO

/* CRUD de Calificaciones
   @Accion: 'S' = Select (todas, o filtradas por @id_calificacion / @id_estudiante)
            'I' = Insert  (devuelve el id generado)
            'U' = Update
            'D' = Delete                                                       */
CREATE PROCEDURE dbo.sp_Calificaciones
    @Accion          CHAR(1),
    @id_calificacion INT          = NULL,
    @id_estudiante   INT          = NULL,
    @materia         VARCHAR(100) = NULL,
    @periodo         VARCHAR(30)  = NULL,
    @nota            DECIMAL(4,2) = NULL,
    @observacion     VARCHAR(200) = NULL,
    @Pagina          INT          = NULL,   -- solo 'S': pagina a mostrar (por defecto 1)
    @TamanoPagina    INT          = NULL    -- solo 'S': filas por pagina (por defecto 5)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        IF @Accion = 'S'
        BEGIN
            SET @Pagina       = ISNULL(@Pagina, 1);
            SET @TamanoPagina = ISNULL(@TamanoPagina, 5);
            IF @Pagina < 1 OR @TamanoPagina < 1
                THROW 50003, 'Pagina y TamanoPagina deben ser mayores a 0.', 1;

            SELECT c.id_calificacion, c.id_estudiante,
                   e.nombres + ' ' + e.apellidos AS estudiante,
                   c.materia, c.periodo, c.nota, c.observacion, c.fecha_registro,
                   COUNT(*) OVER() AS total_registros
            FROM dbo.Calificaciones c
            INNER JOIN dbo.Estudiante e ON e.id_estudiante = c.id_estudiante
            WHERE (@id_calificacion IS NULL OR c.id_calificacion = @id_calificacion)
              AND (@id_estudiante   IS NULL OR c.id_estudiante   = @id_estudiante)
            ORDER BY c.id_calificacion
            OFFSET CAST(@Pagina - 1 AS BIGINT) * @TamanoPagina ROWS
            FETCH NEXT @TamanoPagina ROWS ONLY;
        END
        ELSE IF @Accion = 'I'
        BEGIN
            INSERT INTO dbo.Calificaciones (id_estudiante, materia, periodo, nota, observacion)
            VALUES (@id_estudiante, @materia, @periodo, @nota, @observacion);

            SELECT SCOPE_IDENTITY() AS id_calificacion;
        END
        ELSE IF @Accion = 'U'
        BEGIN
            UPDATE dbo.Calificaciones
            SET id_estudiante = @id_estudiante,
                materia       = @materia,
                periodo       = @periodo,
                nota          = @nota,
                observacion   = @observacion
            WHERE id_calificacion = @id_calificacion;

            IF @@ROWCOUNT = 0
                THROW 50002, 'La calificacion no existe.', 1;
        END
        ELSE IF @Accion = 'D'
        BEGIN
            DELETE FROM dbo.Calificaciones WHERE id_calificacion = @id_calificacion;

            IF @@ROWCOUNT = 0
                THROW 50002, 'La calificacion no existe.', 1;
        END
        ELSE
            THROW 50000, 'Accion no valida. Use S, I, U o D.', 1;
    END TRY
    BEGIN CATCH
        DECLARE @p2 NVARCHAR(1000) =
            N'@Accion=' + ISNULL(@Accion, N'NULL') +
            N', @id_calificacion=' + ISNULL(CAST(@id_calificacion AS NVARCHAR(20)), N'NULL') +
            N', @id_estudiante=' + ISNULL(CAST(@id_estudiante AS NVARCHAR(20)), N'NULL');
        EXEC dbo.sp_RegistrarError @Parametros = @p2;
        THROW;
    END CATCH
END
GO

-- Consulta relacionada: estudiantes con calificaciones >= @NotaMinima
-- Ejemplo del enunciado: EXEC dbo.sp_EstudiantesPorNota @NotaMinima = 5
CREATE PROCEDURE dbo.sp_EstudiantesPorNota
    @NotaMinima DECIMAL(4,2) = 5
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        SELECT e.id_estudiante, e.documento, e.nombres, e.apellidos, e.curso,
               c.materia, c.periodo, c.nota
        FROM dbo.Estudiante e
        INNER JOIN dbo.Calificaciones c ON c.id_estudiante = e.id_estudiante
        WHERE c.nota >= @NotaMinima
        ORDER BY e.apellidos, e.nombres, c.materia;
    END TRY
    BEGIN CATCH
        DECLARE @p3 NVARCHAR(1000) = N'@NotaMinima=' + ISNULL(CAST(@NotaMinima AS NVARCHAR(10)), N'NULL');
        EXEC dbo.sp_RegistrarError @Parametros = @p3;
        THROW;
    END CATCH
END
GO
