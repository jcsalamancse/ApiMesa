-- Script para crear las tablas de Workflow en la base de datos
-- Este script complementa la migración inicial con tablas específicas para workflows

-- Tabla de Workflows
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[pro_workflow]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[pro_workflow] (
        [idworkflow] INT IDENTITY(1,1) NOT NULL,
        [nombre] NVARCHAR(100) NOT NULL,
        [descripcion] NVARCHAR(500) NULL,
        [categoria] NVARCHAR(100) NULL,
        [activo] BIT DEFAULT 1 NOT NULL,
        [created_at] DATETIME2 DEFAULT GETUTCDATE() NOT NULL,
        [updated_at] DATETIME2 NULL,
        [created_by] NVARCHAR(50) NULL,
        [updated_by] NVARCHAR(50) NULL,
        [is_deleted] BIT DEFAULT 0 NOT NULL,
        CONSTRAINT [PK_pro_workflow] PRIMARY KEY ([idworkflow])
    );

    CREATE INDEX [IX_pro_workflow_activo] ON [dbo].[pro_workflow] ([activo]);
    CREATE INDEX [IX_pro_workflow_categoria] ON [dbo].[pro_workflow] ([categoria]);
END

-- Tabla de Pasos de Workflow
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[pro_workflowstep]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[pro_workflowstep] (
        [idstep] INT IDENTITY(1,1) NOT NULL,
        [idworkflow] INT NOT NULL,
        [nombre_paso] NVARCHAR(100) NOT NULL,
        [tipo_paso] NVARCHAR(50) NULL,
        [orden] INT NOT NULL,
        [idrol] INT NULL,
        [created_at] DATETIME2 DEFAULT GETUTCDATE() NOT NULL,
        [updated_at] DATETIME2 NULL,
        [created_by] NVARCHAR(50) NULL,
        [updated_by] NVARCHAR(50) NULL,
        [is_deleted] BIT DEFAULT 0 NOT NULL,
        CONSTRAINT [PK_pro_workflowstep] PRIMARY KEY ([idstep]),
        CONSTRAINT [FK_pro_workflowstep_pro_workflow] FOREIGN KEY ([idworkflow]) REFERENCES [dbo].[pro_workflow] ([idworkflow]) ON DELETE CASCADE,
        CONSTRAINT [FK_pro_workflowstep_pro_rol] FOREIGN KEY ([idrol]) REFERENCES [dbo].[pro_rol] ([idrol]) ON DELETE SET NULL
    );

    CREATE INDEX [IX_pro_workflowstep_idworkflow] ON [dbo].[pro_workflowstep] ([idworkflow]);
    CREATE INDEX [IX_pro_workflowstep_idrol] ON [dbo].[pro_workflowstep] ([idrol]);
    CREATE INDEX [IX_pro_workflowstep_orden] ON [dbo].[pro_workflowstep] ([orden]);
END

-- Insertar workflows iniciales
IF NOT EXISTS (SELECT * FROM [dbo].[pro_workflow] WHERE [nombre] = 'Workflow de Soporte Básico')
BEGIN
    DECLARE @workflowId INT;
    
    INSERT INTO [dbo].[pro_workflow] ([nombre], [descripcion], [categoria], [activo])
    VALUES ('Workflow de Soporte Básico', 'Proceso estándar para atención de incidentes básicos', 'Soporte', 1);
    
    SET @workflowId = SCOPE_IDENTITY();
    
    -- Insertar pasos para el workflow
    INSERT INTO [dbo].[pro_workflowstep] ([idworkflow], [nombre_paso], [tipo_paso], [orden], [idrol])
    VALUES 
        (@workflowId, 'Diagnóstico inicial', 'Task', 1, (SELECT [idrol] FROM [dbo].[pro_rol] WHERE [rol] = 'Técnico')),
        (@workflowId, 'Resolución', 'Task', 2, (SELECT [idrol] FROM [dbo].[pro_rol] WHERE [rol] = 'Técnico')),
        (@workflowId, 'Verificación', 'Approval', 3, (SELECT [idrol] FROM [dbo].[pro_rol] WHERE [rol] = 'Técnico')),
        (@workflowId, 'Cierre', 'Task', 4, (SELECT [idrol] FROM [dbo].[pro_rol] WHERE [rol] = 'Técnico'));
END

IF NOT EXISTS (SELECT * FROM [dbo].[pro_workflow] WHERE [nombre] = 'Workflow de Escalamiento')
BEGIN
    DECLARE @workflowId INT;
    
    INSERT INTO [dbo].[pro_workflow] ([nombre], [descripcion], [categoria], [activo])
    VALUES ('Workflow de Escalamiento', 'Proceso para escalamiento de incidentes complejos', 'Soporte', 1);
    
    SET @workflowId = SCOPE_IDENTITY();
    
    -- Insertar pasos para el workflow
    INSERT INTO [dbo].[pro_workflowstep] ([idworkflow], [nombre_paso], [tipo_paso], [orden], [idrol])
    VALUES 
        (@workflowId, 'Diagnóstico inicial', 'Task', 1, (SELECT [idrol] FROM [dbo].[pro_rol] WHERE [rol] = 'Técnico')),
        (@workflowId, 'Escalamiento', 'Task', 2, (SELECT [idrol] FROM [dbo].[pro_rol] WHERE [rol] = 'Administrador')),
        (@workflowId, 'Análisis avanzado', 'Task', 3, (SELECT [idrol] FROM [dbo].[pro_rol] WHERE [rol] = 'Administrador')),
        (@workflowId, 'Resolución', 'Task', 4, (SELECT [idrol] FROM [dbo].[pro_rol] WHERE [rol] = 'Administrador')),
        (@workflowId, 'Verificación', 'Approval', 5, (SELECT [idrol] FROM [dbo].[pro_rol] WHERE [rol] = 'Técnico')),
        (@workflowId, 'Documentación', 'Task', 6, (SELECT [idrol] FROM [dbo].[pro_rol] WHERE [rol] = 'Técnico')),
        (@workflowId, 'Cierre', 'Task', 7, (SELECT [idrol] FROM [dbo].[pro_rol] WHERE [rol] = 'Técnico'));
END