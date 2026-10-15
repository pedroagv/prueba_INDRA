/* ============================================================================
   Proyecto 1 - Estudiantes y Calificaciones
   Script 04: Datos de prueba
   10 estudiantes y 15 calificaciones, insertados mediante los procedimientos almacenados.
   ============================================================================ */

USE Indra_test;
GO

-- Los SP de insert devuelven el id generado; se captura en @r para no mostrar
-- una grilla de resultados por cada insert.
-- Los estudiantes se crean en este orden, asi que sus id son 1 a 10 (las calificaciones usan esos id).
DECLARE @r TABLE (id INT);

-- ----------------------------------------------------------------------------
-- Estudiantes: cursos de 9 a 11, edades acordes al curso, algunos sin email.
-- El ultimo es una matricula reciente: sin curso (toma el DEFAULT 'Sin asignar') y sin email.
-- ----------------------------------------------------------------------------
INSERT INTO @r EXEC dbo.sp_Estudiante @Accion = 'I', @documento = '1105781234', @nombres = 'Valentina',           @apellidos = 'Gomez Rincon',      @edad = 15, @sexo = 'F', @curso = '10-A', @email = 'valentina.gomez@colegio.test';
INSERT INTO @r EXEC dbo.sp_Estudiante @Accion = 'I', @documento = '1098650217', @nombres = 'Santiago',            @apellidos = 'Rodriguez Pardo',   @edad = 16, @sexo = 'M', @curso = '10-A', @email = 'santiago.rodriguez@colegio.test';
INSERT INTO @r EXEC dbo.sp_Estudiante @Accion = 'I', @documento = '1007432981', @nombres = 'Mariana',             @apellidos = 'Castano Ospina',    @edad = 14, @sexo = 'F', @curso = '9-B';
INSERT INTO @r EXEC dbo.sp_Estudiante @Accion = 'I', @documento = '1103219876', @nombres = 'Juan David',          @apellidos = 'Herrera Mejia',     @edad = 17, @sexo = 'M', @curso = '11-A', @email = 'jd.herrera@colegio.test';
INSERT INTO @r EXEC dbo.sp_Estudiante @Accion = 'I', @documento = '1099034455', @nombres = 'Sara Sofia',         @apellidos = 'Villamizar Duarte', @edad = 15, @sexo = 'F', @curso = '10-B';
INSERT INTO @r EXEC dbo.sp_Estudiante @Accion = 'I', @documento = '1106442019', @nombres = 'Andres Felipe',      @apellidos = 'Cardenas Rueda',    @edad = 16, @sexo = 'M', @curso = '11-A', @email = 'andres.cardenas@colegio.test';
INSERT INTO @r EXEC dbo.sp_Estudiante @Accion = 'I', @documento = '1002785603', @nombres = 'Laura Camila',      @apellidos = 'Moreno Salazar',    @edad = 14, @sexo = 'F', @curso = '9-A',  @email = 'laura.moreno@colegio.test';
INSERT INTO @r EXEC dbo.sp_Estudiante @Accion = 'I', @documento = '1101558724', @nombres = 'Miguel Angel',       @apellidos = 'Torres Bautista',   @edad = 17, @sexo = 'M', @curso = '11-B';
INSERT INTO @r EXEC dbo.sp_Estudiante @Accion = 'I', @documento = '1097336118', @nombres = 'Daniela Paola',     @apellidos = 'Acosta Vargas',     @edad = 15, @sexo = 'F', @curso = '10-B', @email = 'daniela.acosta@colegio.test';
INSERT INTO @r EXEC dbo.sp_Estudiante @Accion = 'I', @documento = '1004871530', @nombres = 'Samuel Esteban',    @apellidos = 'Quintero Leon',     @edad = 14, @sexo = 'M';

-- ----------------------------------------------------------------------------
-- Calificaciones (escala 0 a 10): notas variadas, algunas con observacion, dos periodos,
-- una nota exactamente en 5.00 (para ver el limite de >= 5) y estudiantes con una o varias materias.
-- ----------------------------------------------------------------------------
INSERT INTO @r EXEC dbo.sp_Calificaciones @Accion = 'I', @id_estudiante = 1,  @materia = 'Matematicas',        @periodo = '2026-P1', @nota = 8.40, @observacion = 'Participa activamente en clase';
INSERT INTO @r EXEC dbo.sp_Calificaciones @Accion = 'I', @id_estudiante = 1,  @materia = 'Ingles',             @periodo = '2026-P1', @nota = 9.10;
INSERT INTO @r EXEC dbo.sp_Calificaciones @Accion = 'I', @id_estudiante = 2,  @materia = 'Matematicas',        @periodo = '2026-P1', @nota = 4.30, @observacion = 'Debe reforzar algebra';
INSERT INTO @r EXEC dbo.sp_Calificaciones @Accion = 'I', @id_estudiante = 2,  @materia = 'Matematicas',        @periodo = '2026-P2', @nota = 5.80, @observacion = 'Mejoro frente al periodo anterior';
INSERT INTO @r EXEC dbo.sp_Calificaciones @Accion = 'I', @id_estudiante = 2,  @materia = 'Ciencias Naturales', @periodo = '2026-P1', @nota = 6.70;
INSERT INTO @r EXEC dbo.sp_Calificaciones @Accion = 'I', @id_estudiante = 3,  @materia = 'Lenguaje',           @periodo = '2026-P1', @nota = 7.90;
INSERT INTO @r EXEC dbo.sp_Calificaciones @Accion = 'I', @id_estudiante = 4,  @materia = 'Fisica',             @periodo = '2026-P1', @nota = 5.00;
INSERT INTO @r EXEC dbo.sp_Calificaciones @Accion = 'I', @id_estudiante = 4,  @materia = 'Matematicas',        @periodo = '2026-P1', @nota = 3.60, @observacion = 'No entrego el taller final';
INSERT INTO @r EXEC dbo.sp_Calificaciones @Accion = 'I', @id_estudiante = 5,  @materia = 'Ciencias Sociales',  @periodo = '2026-P1', @nota = 8.80, @observacion = 'Excelente trabajo de investigacion';
INSERT INTO @r EXEC dbo.sp_Calificaciones @Accion = 'I', @id_estudiante = 6,  @materia = 'Ingles',             @periodo = '2026-P1', @nota = 6.20;
INSERT INTO @r EXEC dbo.sp_Calificaciones @Accion = 'I', @id_estudiante = 7,  @materia = 'Matematicas',        @periodo = '2026-P1', @nota = 9.40;
INSERT INTO @r EXEC dbo.sp_Calificaciones @Accion = 'I', @id_estudiante = 7,  @materia = 'Lenguaje',           @periodo = '2026-P1', @nota = 7.10;
INSERT INTO @r EXEC dbo.sp_Calificaciones @Accion = 'I', @id_estudiante = 8,  @materia = 'Quimica',            @periodo = '2026-P1', @nota = 4.80, @observacion = 'Pendiente por recuperar';
INSERT INTO @r EXEC dbo.sp_Calificaciones @Accion = 'I', @id_estudiante = 9,  @materia = 'Educacion Fisica',  @periodo = '2026-P1', @nota = 9.70;
INSERT INTO @r EXEC dbo.sp_Calificaciones @Accion = 'I', @id_estudiante = 9,  @materia = 'Ciencias Naturales', @periodo = '2026-P2', @nota = 7.50;

-- ----------------------------------------------------------------------------
-- Fechas de registro repartidas en el anio escolar (si no, todas quedarian con la hora de ejecucion):
-- estudiantes matriculados a comienzos de febrero, notas del P1 en abril y del P2 en julio.
-- ----------------------------------------------------------------------------
UPDATE dbo.Estudiante
SET fecha_registro = DATEADD(MINUTE, 480 + (id_estudiante * 97) % 540,
                             DATEADD(DAY, (id_estudiante * 2) % 9, CAST('20260202' AS DATETIME)));

UPDATE dbo.Calificaciones
SET fecha_registro = DATEADD(MINUTE, 480 + (id_calificacion * 131) % 540,
                             DATEADD(DAY, (id_calificacion * 3) % 12,
                                     CAST(CASE periodo WHEN '2026-P1' THEN '20260420' ELSE '20260720' END AS DATETIME)));
GO
