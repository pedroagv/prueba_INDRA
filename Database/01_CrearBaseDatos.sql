/* ============================================================================
   Proyecto 1 - Estudiantes y Calificaciones
   Script 01: Creacion de la base de datos
   Crea Indra_test si no existe.
   ============================================================================ */

-- ============================================================================
-- 1. BASE DE DATOS
-- ============================================================================
IF DB_ID(N'Indra_test') IS NULL
    CREATE DATABASE Indra_test;
GO
