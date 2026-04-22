-- ==========================================
-- 1. CREACIÓN DEL MODELO DE DATOS
-- ==========================================

-- Tabla de Usuarios
CREATE TABLE Users (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL, 
    Email NVARCHAR(100) NOT NULL UNIQUE, 
	Contraseña NVARCHAR(100) NOT NULL,
    Eliminado bit default 0,
	FechaCreacion DATETIME NOT NULL DEFAULT GETDATE()
);

--drop table Tasks-- Tabla de Tareas
CREATE  TABLE Tasks (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Titulo NVARCHAR(200) NOT NULL, -- 
    UserId INT NOT NULL, -- 
    Status VARCHAR(20) NOT NULL DEFAULT 'Pending' 
        CHECK (Status IN ('Pending', 'InProgress', 'Done' )), 
    Informacion NVARCHAR(MAX) NULL CHECK (ISJSON(Informacion) = 1), 
    FechaCreacion DATETIME NOT NULL DEFAULT GETDATE(),
    FechaModificacion DATETIME ,
	UserIdCrea  INT NOT NULL,
	UserIdMod INT,
    Eliminado bit default 0, -- [(0)False Activo // (1)True Eliminado]
    -- Clave foránea
    CONSTRAINT FK_Tasks_Users FOREIGN KEY (UserId) REFERENCES Users(Id),
	CONSTRAINT FK_Tasks_Users2 FOREIGN KEY (UserIdCrea) REFERENCES Users(Id),
	CONSTRAINT FK_Tasks_Users3 FOREIGN KEY (UserIdMod) REFERENCES Users(Id)
);

CREATE INDEX Index_Tasks_UserId ON Tasks(UserId);
CREATE INDEX Index_Tasks_Status ON Tasks(Status);
CREATE INDEX Index_Tasks_FechaCreacion ON Tasks(FechaCreacion);
GO
select * from Users
-- ==========================================
--DATOS DE PRUEBA 
-- ==========================================
INSERT INTO Users (Nombre, Email,Contraseña) VALUES
('Juan Perez', 'juan@example.com','123456'),
('Maria Lopez', 'maria@example.com','123456');



-- ==========================================
-- CONSULTAS REQUERIDAS DE NEGOCIO Y JSON
-- ==========================================
select * from Users
SELECT 
    t.Id, 
    t.Titulo, 
    t.UserId, 
    u.Nombre AS UserName,
    t.Status, 
    JSON_VALUE(t.Informacion, '$.prioridad') AS Prioridad,
    JSON_VALUE(t.Informacion, '$.fechaEstimada') AS FechaEstimada,
    JSON_VALUE(t.Informacion, '$.descripcion') AS Descripcion,
	t.UserIdCrea,
	t.UserIdMod,
    t.FechaCreacion, 
    t.FechaModificacion
FROM Tasks t
INNER JOIN Users u ON t.UserId = u.Id
WHERE t.Eliminado = 0;

-- Ejemplo de filtrado de los campo json
SELECT 
    t.Id, 
    t.Titulo, 
    t.UserId, 
    u.Nombre AS UserName,
    t.Status, 
    JSON_VALUE(t.Informacion, '$.prioridad') AS Prioridad,
    JSON_VALUE(t.Informacion, '$.fechaEstimada') AS FechaEstimada,
    JSON_VALUE(t.Informacion, '$.descripcion') AS Descripcion,
    t.UserIdCrea,
    t.UserIdMod,
    t.FechaCreacion, 
    t.FechaModificacion
FROM Tasks t
INNER JOIN Users u ON t.UserId = u.Id
WHERE t.Eliminado = 0
AND JSON_VALUE(t.Informacion, '$.prioridad') = 'Media'
AND CAST(JSON_VALUE(t.Informacion, '$.fechaEstimada') AS DATETIME) >= GETDATE()
AND JSON_VALUE(t.Informacion, '$.descripcion') LIKE '%CANDIDATO%';


exec Get_ListAllTask_byfilters '{"UserId":null,"Status":null,"Prioridad":null,"FechaEstimada":"0001-01-01T00:00:00"}'
	ALTER PROCEDURE Get_ListAllTask_byfilters
    @Filters NVARCHAR(MAX)
AS
BEGIN
    DECLARE 
        @Status VARCHAR(100),
        @UserId INT,
        @FechaEstimada DATE,
        @Prioridad VARCHAR(100);

    SELECT 
        @Status = Status,
        @UserId = UserId,
        @FechaEstimada = FechaEstimada,
        @Prioridad = Prioridad
    FROM OPENJSON(@Filters)
    WITH (
        Status VARCHAR(100) '$.Status',
        UserId INT '$.UserId',
        FechaEstimada DATE '$.FechaEstimada',
        Prioridad VARCHAR(100) '$.Prioridad'
    );

    SELECT 
        t.Id, 
        t.Titulo, 
        t.UserId, 
        u.Nombre AS UserName,
        t.Status, 
        t.Informacion, 
        JSON_VALUE(t.Informacion, '$.prioridad') AS Prioridad,
        JSON_VALUE(t.Informacion, '$.fechaEstimada') AS FechaEstimada,
        JSON_VALUE(t.Informacion, '$.descripcion') AS Descripcion,
        t.FechaCreacion, 
        t.FechaModificacion
    FROM Tasks t
    INNER JOIN Users u ON t.UserId = u.Id
    WHERE 
        t.Eliminado = 0
        AND (@Status IS NULL OR TRIM(@Status) = '' OR t.Status = @Status)
        AND (@UserId IS NULL OR t.UserId = @UserId)
        AND (@Prioridad IS NULL OR TRIM(@Prioridad) = '' OR JSON_VALUE(t.Informacion, '$.prioridad') = @Prioridad)
        AND ((@FechaEstimada IS NULL OR @FechaEstimada = '0001-01-01' )OR TRY_CONVERT(DATE, JSON_VALUE(t.Informacion, '$.fechaEstimada')) = @FechaEstimada)
    ORDER BY t.FechaCreacion DESC;
END

