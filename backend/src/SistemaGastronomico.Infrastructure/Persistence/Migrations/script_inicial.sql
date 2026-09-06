IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260905233214_InitialCreate'
)
BEGIN
    CREATE TABLE [Usuarios] (
        [Id] uniqueidentifier NOT NULL,
        [NombreCompleto] nvarchar(150) NOT NULL,
        [Email] nvarchar(150) NOT NULL,
        [PasswordHash] nvarchar(max) NOT NULL,
        [Telefono] nvarchar(20) NULL,
        [Rol] nvarchar(30) NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NULL,
        [IsActive] bit NOT NULL,
        CONSTRAINT [PK_Usuarios] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260905233214_InitialCreate'
)
BEGIN
    CREATE TABLE [Locales] (
        [Id] uniqueidentifier NOT NULL,
        [Nombre] nvarchar(150) NOT NULL,
        [Descripcion] nvarchar(1000) NOT NULL,
        [Direccion] nvarchar(250) NOT NULL,
        [Referencia] nvarchar(250) NULL,
        [CampusCercano] nvarchar(50) NOT NULL,
        [Latitud] float NOT NULL,
        [Longitud] float NOT NULL,
        [TelefonoContacto] nvarchar(20) NULL,
        [WhatsApp] nvarchar(20) NULL,
        [HorarioApertura] nvarchar(100) NULL,
        [LogoUrl] nvarchar(500) NULL,
        [PortadaUrl] nvarchar(500) NULL,
        [Categoria] nvarchar(50) NOT NULL,
        [EstaAbierto] bit NOT NULL,
        [PropietarioId] uniqueidentifier NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NULL,
        [IsActive] bit NOT NULL,
        CONSTRAINT [PK_Locales] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Locales_Usuarios_PropietarioId] FOREIGN KEY ([PropietarioId]) REFERENCES [Usuarios] ([Id]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260905233214_InitialCreate'
)
BEGIN
    CREATE TABLE [Coordenadas] (
        [Id] uniqueidentifier NOT NULL,
        [Latitud] float NOT NULL,
        [Longitud] float NOT NULL,
        [DireccionReferencial] nvarchar(250) NULL,
        [LocalId] uniqueidentifier NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NULL,
        [IsActive] bit NOT NULL,
        CONSTRAINT [PK_Coordenadas] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Coordenadas_Locales_LocalId] FOREIGN KEY ([LocalId]) REFERENCES [Locales] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260905233214_InitialCreate'
)
BEGIN
    CREATE TABLE [EstadisticasEventos] (
        [Id] uniqueidentifier NOT NULL,
        [LocalId] uniqueidentifier NOT NULL,
        [TipoEvento] nvarchar(50) NOT NULL,
        [EstudianteId] uniqueidentifier NULL,
        [FechaRegistro] datetime2 NOT NULL,
        [Detalle] nvarchar(500) NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NULL,
        [IsActive] bit NOT NULL,
        CONSTRAINT [PK_EstadisticasEventos] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_EstadisticasEventos_Locales_LocalId] FOREIGN KEY ([LocalId]) REFERENCES [Locales] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260905233214_InitialCreate'
)
BEGIN
    CREATE TABLE [MenusFavoritos] (
        [Id] uniqueidentifier NOT NULL,
        [EstudianteId] uniqueidentifier NOT NULL,
        [LocalId] uniqueidentifier NOT NULL,
        [FechaGuardado] datetime2 NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NULL,
        [IsActive] bit NOT NULL,
        CONSTRAINT [PK_MenusFavoritos] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_MenusFavoritos_Locales_LocalId] FOREIGN KEY ([LocalId]) REFERENCES [Locales] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_MenusFavoritos_Usuarios_EstudianteId] FOREIGN KEY ([EstudianteId]) REFERENCES [Usuarios] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260905233214_InitialCreate'
)
BEGIN
    CREATE TABLE [Platos] (
        [Id] uniqueidentifier NOT NULL,
        [LocalId] uniqueidentifier NOT NULL,
        [Nombre] nvarchar(150) NOT NULL,
        [Descripcion] nvarchar(500) NOT NULL,
        [Precio] decimal(18,2) NOT NULL,
        [ImagenUrl] nvarchar(500) NULL,
        [Categoria] nvarchar(50) NOT NULL,
        [EstaDisponible] bit NOT NULL DEFAULT CAST(1 AS bit),
        [EsPlatoDelDia] bit NOT NULL DEFAULT CAST(0 AS bit),
        [OrdenVisualizacion] int NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NULL,
        [IsActive] bit NOT NULL,
        CONSTRAINT [PK_Platos] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Platos_Locales_LocalId] FOREIGN KEY ([LocalId]) REFERENCES [Locales] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260905233214_InitialCreate'
)
BEGIN
    CREATE TABLE [Promociones] (
        [Id] uniqueidentifier NOT NULL,
        [LocalId] uniqueidentifier NOT NULL,
        [Titulo] nvarchar(150) NOT NULL,
        [Descripcion] nvarchar(500) NOT NULL,
        [PrecioOriginal] decimal(18,2) NULL,
        [PrecioPromocional] decimal(18,2) NOT NULL,
        [ImagenUrl] nvarchar(500) NULL,
        [FechaInicio] datetime2 NOT NULL,
        [FechaFin] datetime2 NOT NULL,
        [EstaActiva] bit NOT NULL DEFAULT CAST(1 AS bit),
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NULL,
        [IsActive] bit NOT NULL,
        CONSTRAINT [PK_Promociones] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Promociones_Locales_LocalId] FOREIGN KEY ([LocalId]) REFERENCES [Locales] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260905233214_InitialCreate'
)
BEGIN
    CREATE TABLE [Resenas] (
        [Id] uniqueidentifier NOT NULL,
        [LocalId] uniqueidentifier NOT NULL,
        [EstudianteId] uniqueidentifier NOT NULL,
        [Calificacion] int NOT NULL,
        [Comentario] nvarchar(1000) NULL,
        [Fecha] datetime2 NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NULL,
        [IsActive] bit NOT NULL,
        CONSTRAINT [PK_Resenas] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Resenas_Locales_LocalId] FOREIGN KEY ([LocalId]) REFERENCES [Locales] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_Resenas_Usuarios_EstudianteId] FOREIGN KEY ([EstudianteId]) REFERENCES [Usuarios] ([Id]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260905233214_InitialCreate'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Coordenadas_LocalId] ON [Coordenadas] ([LocalId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260905233214_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_EstadisticasEventos_LocalId] ON [EstadisticasEventos] ([LocalId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260905233214_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Locales_PropietarioId] ON [Locales] ([PropietarioId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260905233214_InitialCreate'
)
BEGIN
    CREATE UNIQUE INDEX [IX_MenusFavoritos_EstudianteId_LocalId] ON [MenusFavoritos] ([EstudianteId], [LocalId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260905233214_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_MenusFavoritos_LocalId] ON [MenusFavoritos] ([LocalId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260905233214_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Platos_LocalId] ON [Platos] ([LocalId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260905233214_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Promociones_LocalId] ON [Promociones] ([LocalId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260905233214_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Resenas_EstudianteId] ON [Resenas] ([EstudianteId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260905233214_InitialCreate'
)
BEGIN
    CREATE INDEX [IX_Resenas_LocalId] ON [Resenas] ([LocalId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260905233214_InitialCreate'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Usuarios_Email] ON [Usuarios] ([Email]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260905233214_InitialCreate'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260905233214_InitialCreate', N'10.0.11');
END;

COMMIT;
GO

