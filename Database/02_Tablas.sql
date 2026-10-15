/* ============================================================================
   Proyecto 1 - Estudiantes y Calificaciones
   Script 02: Tablas y documentacion
   Estudiante, Calificaciones y LogErrores. Se puede re-ejecutar (borra y recrea las tablas).
   ============================================================================ */

USE Indra_test;
GO

-- Limpieza: primero la tabla hija (FK), luego las demas
IF OBJECT_ID(N'dbo.Calificaciones',        N'U') IS NOT NULL DROP TABLE dbo.Calificaciones;
IF OBJECT_ID(N'dbo.Estudiante',            N'U') IS NOT NULL DROP TABLE dbo.Estudiante;
IF OBJECT_ID(N'dbo.LogErrores',            N'U') IS NOT NULL DROP TABLE dbo.LogErrores;
GO

-- ============================================================================
-- 2. TABLAS
-- ============================================================================

-- Estudiante: IDENTITY, PRIMARY KEY, UNIQUE, DEFAULT, NULL / NOT NULL
CREATE TABLE dbo.Estudiante
(
    id_estudiante  INT IDENTITY(1,1) NOT NULL,
    documento      VARCHAR(20)       NOT NULL,
    nombres        VARCHAR(100)      NOT NULL,
    apellidos      VARCHAR(100)      NOT NULL,
    edad           INT               NOT NULL,
    sexo           CHAR(1)           NOT NULL,
    curso          VARCHAR(50)       NOT NULL CONSTRAINT DF_Estudiante_curso DEFAULT ('Sin asignar'),
    email          VARCHAR(150)      NULL,
    activo         BIT               NOT NULL CONSTRAINT DF_Estudiante_activo DEFAULT (1),
    fecha_registro DATETIME          NOT NULL CONSTRAINT DF_Estudiante_fecha DEFAULT (GETDATE()),
    CONSTRAINT PK_Estudiante           PRIMARY KEY (id_estudiante),
    CONSTRAINT UQ_Estudiante_documento UNIQUE (documento),
    CONSTRAINT CK_Estudiante_edad      CHECK (edad BETWEEN 3 AND 100),
    CONSTRAINT CK_Estudiante_sexo      CHECK (sexo IN ('M', 'F'))
);
GO

-- Calificaciones: relacionada con Estudiante (1 estudiante : N calificaciones)
CREATE TABLE dbo.Calificaciones
(
    id_calificacion INT IDENTITY(1,1) NOT NULL,
    id_estudiante   INT               NOT NULL,
    materia         VARCHAR(100)      NOT NULL,
    periodo         VARCHAR(30)       NOT NULL,
    nota            DECIMAL(4,2)      NOT NULL,
    observacion     VARCHAR(200)      NULL,
    fecha_registro  DATETIME          NOT NULL CONSTRAINT DF_Calificaciones_fecha DEFAULT (GETDATE()),
    CONSTRAINT PK_Calificaciones            PRIMARY KEY (id_calificacion),
    CONSTRAINT FK_Calificaciones_Estudiante FOREIGN KEY (id_estudiante) REFERENCES dbo.Estudiante (id_estudiante),
    CONSTRAINT UQ_Calificaciones_unica      UNIQUE (id_estudiante, materia, periodo),
    CONSTRAINT CK_Calificaciones_nota       CHECK (nota BETWEEN 0 AND 10)
);
GO

-- LogErrores: bitacora de errores de los procedimientos almacenados
CREATE TABLE dbo.LogErrores
(
    id_log         INT IDENTITY(1,1) NOT NULL,
    fecha          DATETIME          NOT NULL CONSTRAINT DF_LogErrores_fecha DEFAULT (GETDATE()),
    usuario        SYSNAME           NOT NULL CONSTRAINT DF_LogErrores_usuario DEFAULT (SUSER_SNAME()),
    procedimiento  NVARCHAR(128)     NULL,
    numero_error   INT               NULL,
    severidad      INT               NULL,
    estado         INT               NULL,
    linea          INT               NULL,
    mensaje        NVARCHAR(4000)    NULL,
    parametros     NVARCHAR(1000)    NULL,
    CONSTRAINT PK_LogErrores PRIMARY KEY (id_log)
);
GO

-- ----------------------------------------------------------------------------
-- Documentacion de tablas y columnas (propiedad extendida MS_Description)
-- ----------------------------------------------------------------------------
DECLARE @doc TABLE (tabla SYSNAME, columna SYSNAME NULL, descripcion NVARCHAR(500));

INSERT INTO @doc (tabla, columna, descripcion) VALUES
(N'Estudiante', NULL,             N'Datos basicos de los estudiantes.'),
(N'Estudiante', N'id_estudiante', N'Llave primaria autoincremental (IDENTITY desde 1).'),
(N'Estudiante', N'documento',     N'Documento de identidad. Valor unico, no se puede repetir.'),
(N'Estudiante', N'nombres',       N'Nombres del estudiante.'),
(N'Estudiante', N'apellidos',     N'Apellidos del estudiante.'),
(N'Estudiante', N'edad',          N'Edad en anios (entre 3 y 100).'),
(N'Estudiante', N'sexo',          N'Sexo: M = masculino, F = femenino.'),
(N'Estudiante', N'curso',         N'Curso o grado. Por defecto: Sin asignar.'),
(N'Estudiante', N'email',         N'Correo electronico. Opcional (acepta NULL).'),
(N'Estudiante', N'activo',        N'1 = activo, 0 = inactivo. Por defecto 1.'),
(N'Estudiante', N'fecha_registro',N'Fecha y hora de creacion del registro. Por defecto GETDATE().'),

(N'Calificaciones', NULL,              N'Calificaciones de los estudiantes por materia y periodo.'),
(N'Calificaciones', N'id_calificacion',N'Llave primaria autoincremental (IDENTITY desde 1).'),
(N'Calificaciones', N'id_estudiante',  N'Llave foranea hacia Estudiante.id_estudiante.'),
(N'Calificaciones', N'materia',        N'Nombre de la materia.'),
(N'Calificaciones', N'periodo',        N'Periodo academico (ej. 2026-P1). Unico por estudiante y materia.'),
(N'Calificaciones', N'nota',           N'Nota de 0 a 10 con dos decimales.'),
(N'Calificaciones', N'observacion',    N'Comentario opcional (acepta NULL).'),
(N'Calificaciones', N'fecha_registro', N'Fecha y hora de creacion del registro. Por defecto GETDATE().'),

(N'LogErrores', NULL,             N'Bitacora de errores capturados en los procedimientos almacenados.'),
(N'LogErrores', N'id_log',        N'Llave primaria autoincremental.'),
(N'LogErrores', N'fecha',         N'Momento en que ocurrio el error. Por defecto GETDATE().'),
(N'LogErrores', N'usuario',       N'Usuario de SQL Server que ejecuto el procedimiento.'),
(N'LogErrores', N'procedimiento', N'Procedimiento donde se produjo el error.'),
(N'LogErrores', N'numero_error',  N'ERROR_NUMBER().'),
(N'LogErrores', N'severidad',     N'ERROR_SEVERITY().'),
(N'LogErrores', N'estado',        N'ERROR_STATE().'),
(N'LogErrores', N'linea',         N'ERROR_LINE().'),
(N'LogErrores', N'mensaje',       N'ERROR_MESSAGE().'),
(N'LogErrores', N'parametros',    N'Parametros con que se invoco el procedimiento.');

DECLARE @t SYSNAME, @c SYSNAME, @d NVARCHAR(500);
DECLARE cur CURSOR LOCAL FAST_FORWARD FOR SELECT tabla, columna, descripcion FROM @doc;
OPEN cur;
FETCH NEXT FROM cur INTO @t, @c, @d;
WHILE @@FETCH_STATUS = 0
BEGIN
    IF @c IS NULL
        EXEC sys.sp_addextendedproperty @name = N'MS_Description', @value = @d,
             @level0type = N'SCHEMA', @level0name = N'dbo',
             @level1type = N'TABLE',  @level1name = @t;
    ELSE
        EXEC sys.sp_addextendedproperty @name = N'MS_Description', @value = @d,
             @level0type = N'SCHEMA', @level0name = N'dbo',
             @level1type = N'TABLE',  @level1name = @t,
             @level2type = N'COLUMN', @level2name = @c;
    FETCH NEXT FROM cur INTO @t, @c, @d;
END
CLOSE cur;
DEALLOCATE cur;
GO
