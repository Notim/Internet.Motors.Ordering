CREATE DATABASE ORDERING;
GO

USE ORDERING;
GO

CREATE TABLE VEHICLE_ORDER (
    Id BIGINT IDENTITY(1,1) NOT NULL PRIMARY KEY, -- Corresponds to Order.Id
    OrderId UNIQUEIDENTIFIER NOT NULL UNIQUE,     -- Corresponds to Order.OrderId
    CustomerDocument NVARCHAR(14) NOT NULL, -- Corresponds to Order.CustomerDocument
    OrderedAt DATETIME2 NOT NULL, -- Corresponds to Order.OrderedAt
    VehicleId UNIQUEIDENTIFIER NOT NULL, -- Corresponds to Vehicle.VehicleId
    CarName NVARCHAR(MAX), -- Corresponds to Vehicle.CarName (nullable)
    Price DECIMAL(18, 2) NOT NULL, -- Corresponds to Vehicle.Price
    Status VARCHAR(20) NOT NULL, -- Corresponds to OrderStatus (enum mapped to string)
    Observation VARCHAR(200) NULL
);

EXEC sys.sp_addextendedproperty
    @name = N'MS_Description',
    @value = N'Table to store vehicle orders.',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'VEHICLE_ORDER';
GO