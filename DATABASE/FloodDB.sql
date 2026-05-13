DROP TABLE IF EXISTS BarangayConnections;
DROP TABLE IF EXISTS FloodAlerts;
DROP TABLE IF EXISTS EvacuationCenters;
DROP TABLE IF EXISTS Users;
DROP TABLE IF EXISTS Barangays;
GO

USE FloodBusterDB;
GO

-- BARANGAYS
CREATE TABLE Barangays (
    BarangayID   INT PRIMARY KEY IDENTITY(1,1),
    BarangayName NVARCHAR(100) NOT NULL UNIQUE,
    IsFlooded    BIT NOT NULL DEFAULT 0,
    LastUpdated  DATETIME NOT NULL DEFAULT GETDATE()
);

-- EVACUATION CENTERS
CREATE TABLE EvacuationCenters (
    CenterID         INT PRIMARY KEY IDENTITY(1,1),
    CenterName       NVARCHAR(150) NOT NULL,
    BarangayID       INT NOT NULL FOREIGN KEY REFERENCES Barangays(BarangayID),
    MaxCapacity      INT NOT NULL,
    CurrentOccupancy INT NOT NULL DEFAULT 0,
    IsOperational    BIT NOT NULL DEFAULT 1
);

-- FLOOD ALERTS
CREATE TABLE FloodAlerts (
    AlertID      INT PRIMARY KEY IDENTITY(1,1),
    BarangayID   INT NOT NULL FOREIGN KEY REFERENCES Barangays(BarangayID),
    AlertMessage NVARCHAR(500) NOT NULL,
    AlertLevel   NVARCHAR(20) NOT NULL CHECK (AlertLevel IN ('LOW','MODERATE','CRITICAL')),
    DateIssued   DATETIME NOT NULL DEFAULT GETDATE(),
    IsCleared    BIT NOT NULL DEFAULT 0
);

-- USERS
CREATE TABLE Users (
    UserID       INT PRIMARY KEY IDENTITY(1,1),
    Username     NVARCHAR(50) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(256) NOT NULL,
    Role         NVARCHAR(20) NOT NULL CHECK (Role IN ('Admin','StandardUser')),
    DateCreated  DATETIME NOT NULL DEFAULT GETDATE()
);

-- BARANGAY ROAD CONNECTIONS (Graph edges)
CREATE TABLE BarangayConnections (
    ConnectionID    INT PRIMARY KEY IDENTITY(1,1),
    FromBarangayID  INT NOT NULL FOREIGN KEY REFERENCES Barangays(BarangayID),
    ToBarangayID    INT NOT NULL FOREIGN KEY REFERENCES Barangays(BarangayID),
    DistanceMeters  INT NOT NULL
);
GO