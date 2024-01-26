CREATE TABLE Scooters
(
	Id INT NOT NULL PRIMARY KEY IDENTITY,
	Nombre VARCHAR(50) NOT NULL,
	Precio NUMERIC(12,2) NOT NULL,
	Stock INT,
	Estado INT
)
CREATE PROCEDURE spConsultarScooters
AS
BEGIN
	SELECT * FROM Scooters;
END
CREATE TABLE Usuarios
(
	Id INT NOT NULL PRIMARY KEY IDENTITY,
	Usuario VARCHAR(50) NOT NULL,
	Contrasena VARCHAR(50)
)
ALTER PROCEDURE spRegistrarUsuario
@Id INT,
@Usuario VARCHAR(50),
@Contrasena VARCHAR(50)
AS
BEGIN
	IF NOT EXISTS (SELECT Id FROM Usuarios WHERE Id=@Id)
	BEGIN
		INSERT INTO Usuarios VALUES (@Usuario, @Contrasena);
		SELECT '1' AS [Id], 'Se ha registrado correctamente' AS [Mensaje];
	END ELSE
		BEGIN
			UPDATE Usuarios SET Usuario=@Usuario, Contrasena=@Contrasena WHERE Id=@Id;
			SELECT '2' AS [Id], 'Se ha modificado correctamente' AS [Mensaje];
		END
END
ALTER PROCEDURE spBorrarUsuario
@Id INT
AS
BEGIN
	DELETE FROM Usuarios WHERE Id=@Id
	SELECT '1' AS [Id], 'Se ha borrado correctamente' AS [Mensaje];
END
ALTER PROCEDURE spValidarUsuario
@Usuario VARCHAR(50),
@Contrasena VARCHAR(50)
AS
BEGIN
	IF EXISTS(SELECT Id FROM Usuarios WHERE Usuario=@Usuario AND Contrasena=@Contrasena)
	BEGIN
		DECLARE @Id INT SELECT @Id = Id FROM Usuarios WHERE Usuario=@Usuario AND Contrasena=@Contrasena
		SELECT 1 AS [Id], 'Bienvenido, has iniciado sesión'+'|'+CONVERT(VARCHAR(20),@Id) + '|' + @Usuario AS [Nombre];
	END ELSE
		BEGIN
			SELECT '2' AS [Id], 'Usuario y/o contraseña incorrecto' AS [Nombre];
		END
END
CREATE TABLE TipoPagos
(
	Id INT NOT NULL PRIMARY KEY IDENTITY,
	Nombre VARCHAR(50) NOT NULL,
	Comision NUMERIC(12,2) NOT NULL
)
ALTER PROCEDURE spTablaTipoPagos
AS
BEGIN
	SELECT * FROM TipoPagos
END	
DELETE FROM TipoPagos
ALTER PROCEDURE spRegistrarTipoPagos
@Id INT,
@Nombre VARCHAR(50),
@Comision NUMERIC(12,2)
AS
BEGIN
	IF NOT EXISTS(SELECT Id FROM TipoPagos WHERE Id=@Id)
	BEGIN
		INSERT INTO TipoPagos VALUES (@Nombre, @Comision)
		SELECT '1' AS [Id], 'Se ha registrado correctamente' AS [Nombre];
	END	ELSE
		BEGIN
			UPDATE TipoPagos SET Nombre=@Nombre, Comision=@Comision WHERE Id=@Id;
			SELECT '2'AS [Id], 'Se ha modificado correctamente' AS [Nombre];
		END
END
CREATE TABLE TemporalRentas
(
	Id INT NOT NULL,
	Renglon INT NOT NULL,
	IdScooter INT REFERENCES Scooters(Id) ON UPDATE NO ACTION ON DELETE NO ACTION,
	Tiempo NUMERIC(12,2) NOT NULL,
	Cantidad INT NOT NULL,
	Precio NUMERIC(12,2) NOT NULL,
	PRIMARY KEY(Id, Renglon)
)


CREATE PROCEDURE spPostTemporalRentas
@Tiempo NUMERIC(12,2),
@Cantidad INT,
@Precio NUMERIC(12,2),
@IdScooter INT
AS
BEGIN
	IF NOT EXISTS (SELECT IdScooter FROM TemporalRentas WHERE IdScooter=@IdScooter)
	BEGIN
		DECLARE @Renglon INT SELECT @Renglon = ISNULL(MAX(Renglon + 1), 1) FROM TemporalRentas
		INSERT INTO TemporalRentas (Id, Renglon ,IdScooter, Tiempo, Cantidad, Precio) VALUES (1, @Renglon,@IdScooter, @Tiempo, @Cantidad, @Precio)
	END
	SELECT '1' AS [Id], SUM(Precio) AS [Nombre] FROM TemporalRentas
END

ALTER PROCEDURE spTablaTemporalRentas
AS
BEGIN
	SELECT TemporalRentas.Id,TemporalRentas.Renglon, TemporalRentas.IdScooter, Scooters.Nombre AS [NombreScooter], TemporalRentas.Tiempo,
	TemporalRentas.Cantidad, TemporalRentas.Precio FROM TemporalRentas INNER JOIN Scooters ON TemporalRentas.IdScooter = Scooters.Id
END
ALTER PROCEDURE spBorrarTemporalRentas
@IdScooter INT
AS
BEGIN
	DELETE FROM TemporalRentas WHERE IdScooter=@IdScooter;
	SELECT '1' AS [Id], SUM(Precio) AS [Nombre] FROM TemporalRentas;
END

EXECUTE spBorrarTemporalRentas 2

CREATE TABLE Rentas
(
	Id INT NOT NULL PRIMARY KEY IDENTITY,
	IdPago INT NOT NULL REFERENCES TipoPagos(Id) ON UPDATE NO ACTION ON DELETE NO ACTION,
	Fecha DATETIME,
	DineroPago NUMERIC(12,2),
	Cambio NUMERIC(12,2),
	Comision NUMERIC(12,2),
	SubTotal NUMERIC(12,2),
	Total NUMERIC(12,2)
)
ALTER PROCEDURE spProcesarPago
@IdPago INT,
@DineroPago NUMERIC(12,2),
@Cambio NUMERIC(12,2),
@Comision NUMERIC(12,2),
@SubTotal NUMERIC(12,2),
@Total NUMERIC(12,2)
AS
BEGIN
	INSERT INTO Rentas (IdPago, Fecha, DineroPago, Cambio, Comision, SubTotal, Total) VALUES
	(@IdPago, SYSDATETIME(), @DineroPago, @Cambio, @Comision, @SubTotal, @Total);
	DECLARE @SiguienteId INT;
	SELECT @SiguienteId = ISNULL(MAX(Id), 1) FROM Rentas;
	SELECT '1' AS [Id],
		(
			SELECT CONVERT(VARCHAR(10), IdScooter) + '|' + CONVERT(VARCHAR(10), Tiempo) AS [Nombre]
			FROM DetalleRentas 
			WHERE IdRenta = @SiguienteId
			FOR JSON PATH, ROOT('Scooters')
		) AS [Nombre]
END

ALTER TRIGGER trAfterInsertRentas
ON Rentas
AFTER INSERT
AS
BEGIN
 -- Variables para almacenar datos
    DECLARE @IdRenta INT;
    DECLARE @Fecha DATETIME;
    DECLARE @DineroPago NUMERIC(12,2);
    DECLARE @Cambio NUMERIC(12,2);
    DECLARE @Comision NUMERIC(12,2);
    DECLARE @SubTotal NUMERIC(12,2);
    DECLARE @Total NUMERIC(12,2);

    -- Obtener valores de la tabla Rentas
    SELECT @IdRenta = Id,
           @Fecha = Fecha,
           @DineroPago = DineroPago,
           @Cambio = Cambio,
           @Comision = Comision,
           @SubTotal = SubTotal,
           @Total = Total
    FROM inserted;
	
	-- Obtener los detalles de la tabla TemporalRentas
	DECLARE @Detalles TABLE
    (
        Renglon INT,
        IdScooter INT,
        Cantidad INT,
        Tiempo NUMERIC(12,2),
        Precio NUMERIC(12,2)
    );

	INSERT INTO @Detalles (Renglon, IdScooter, Cantidad, Tiempo, Precio)
    SELECT Renglon, IdScooter, Cantidad, Tiempo, Precio
    FROM TemporalRentas;

	DECLARE @MaxId INT SELECT @MaxId = ISNULL(MAX(Id+1),1) FROM DetalleRentas

    -- Insertar en la tabla DetalleRentas
    INSERT INTO DetalleRentas (Id, IdRenta, Renglon, IdScooter, Cantidad, Tiempo, Fecha, RentaTerminada, MinutosExtra, CargosPorExtra, SubTotal)
    SELECT
		@MaxId,
        @IdRenta,
        Renglon,
        IdScooter,
        Cantidad,
        Tiempo,
        @Fecha,
        NULL,
        NULL,
        NULL,
        Precio
    FROM @Detalles;

    -- Eliminar datos de TemporalRentas
    DELETE FROM TemporalRentas;

END


CREATE TABLE DetalleRentas
(
	Id INT NOT NULL,
	IdRenta INT NOT NULL REFERENCES Rentas(Id) ON UPDATE NO ACTION ON DELETE NO ACTION,
	Renglon INT NOT NULL,
	IdScooter INT NOT NULL REFERENCES Scooters(Id) ON UPDATE NO ACTION ON DELETE NO ACTION,
	Cantidad INT,
	Tiempo NUMERIC(12,2),
	Fecha DATETIME,
	RentaTerminada DATETIME,
	MinutosExtra INT,
	CargosPorExtra NUMERIC(12,2),
	SubTotal NUMERIC(12,2),
	PRIMARY KEY(Id, IdRenta, Renglon)
)

SELECT SUM(SubTotal) FROM DetalleRentas
