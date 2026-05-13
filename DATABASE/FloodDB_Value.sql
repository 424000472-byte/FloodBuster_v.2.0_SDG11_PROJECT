USE FloodBusterDB;
GO

-- 1. Ensure the column exists BEFORE we try to insert data into it
IF NOT EXISTS (SELECT * FROM sys.columns 
               WHERE object_id = OBJECT_ID('BarangayConnections') 
               AND name = 'RecommendedRoute')
BEGIN
    ALTER TABLE BarangayConnections ADD RecommendedRoute NVARCHAR(255) NULL;
END
GO -- This GO is critical; it finishes the table update before moving to the inserts

-- 2. Wipe existing data in correct order (Child tables first)
DELETE FROM BarangayConnections;
DELETE FROM FloodAlerts;
DELETE FROM EvacuationCenters;
DELETE FROM Users;
DELETE FROM Barangays;

-- 3. Reset the Identity counters
DBCC CHECKIDENT ('Barangays', RESEED, 0);
DBCC CHECKIDENT ('Users', RESEED, 0);
DBCC CHECKIDENT ('EvacuationCenters', RESEED, 0);
DBCC CHECKIDENT ('FloodAlerts', RESEED, 0);
DBCC CHECKIDENT ('BarangayConnections', RESEED, 0);
GO

BEGIN TRANSACTION;
BEGIN TRY
    -- 4. Insert Barangays (Parent Table)
    INSERT INTO Barangays (BarangayName, IsFlooded) VALUES 
    ('San Jose', 0), ('Poblacion', 1), ('Santo Niño', 0), ('Concepcion', 0), ('San Roque', 1), 
    ('Bagong Silang', 0), ('Malanday', 1), ('Tumana', 1), ('Marikina Heights', 0), ('Fortune', 0), 
    ('Parang', 0), ('Nangka', 1), ('Sta. Elena', 1), ('Calumpang', 1), ('Taong Panulukan', 0),
    ('Barangay 16', 0), ('Barangay 17', 1), ('Santa Lucia', 0), ('Maginhawa', 0), ('Mayamot', 1),
    ('Dela Peña', 0), ('Industrial Valley', 0), ('Barangka', 1), ('Loyola Heights', 0), ('Katipunan', 0),
    ('Old Balara', 1), ('Pansol', 0), ('San Vicente', 0), ('U.P. Campus', 0), ('Krus na Ligas', 1),
    ('Central', 0), ('Pinyahan', 1), ('Malaya', 0), ('Sikatuna Village', 0), ('Teachers Village', 0),
    ('Holy Spirit', 1), ('Pasong Tamo', 1), ('Culiat', 0), ('Tandang Sora', 0), ('Sangandaan', 1),
    ('Baesa', 0), ('Talipapa', 0), ('Unang Sigaw', 1), ('Balingasa', 0), ('Pag-ibig sa Nayon', 0),
    ('Loma de Gato', 1), ('Muzon', 0), ('San Rafael', 0), ('Graceville', 1), ('Kaypian', 0);

    -- 5. Insert FloodAlerts
    INSERT INTO FloodAlerts (BarangayID, AlertMessage, AlertLevel, IsCleared) VALUES 
    (1, 'Water level stable.', 'LOW', 0),
    (2, 'Submerged streets in Zone 4.', 'CRITICAL', 0),
    (5, 'Heavy rain; expect rising water.', 'MODERATE', 0),
    (7, 'Riverside overflow detected.', 'CRITICAL', 0),
    (8, 'Pre-emptive evacuation advised.', 'MODERATE', 0),
    (12, 'Roads impassable to light vehicles.', 'CRITICAL', 0),
    (17, 'Minor flooding in low-lying areas.', 'LOW', 0);

    -- 6. Insert Users
    INSERT INTO Users (Username, PasswordHash, Role) VALUES 
    ('Maria Victoria', 'hash_v82n291x', 'Admin'),
    ('Don christian', 'hash_m91k38sz', 'Admin'),
    ('Riza', 'hash_b22n91p1', 'Admin'),
    ('Kurt', 'hash_c33m82q2', 'StandardUser'),
    ('Arjane', 'hash_d44l73r3', 'StandardUser');

    -- 7. Insert Evacuation Centers
    INSERT INTO EvacuationCenters (CenterName, BarangayID, MaxCapacity, CurrentOccupancy, IsOperational) VALUES 
    ('San Jose Elementary Gym', 1, 50, 0, 1), ('Poblacion Multi-Purpose Hall', 2, 50, 0, 1), 
    ('Sto. Niño Church Basement', 3, 50, 0, 1), ('Concepcion Integrated School', 4, 50, 0, 1), 
    ('San Roque Covered Court', 5, 50, 0, 1), ('Bagong Silang School', 6, 50, 0, 1), 
    ('Malanday High School', 7, 50, 0, 1), ('Tumana Disaster Center', 8, 50, 0, 1), 
    ('Marikina Heights Gym', 9, 50, 0, 1), ('Fortune Covered Court', 10, 50, 0, 1);

    -- 8. Insert Barangay Connections (WITH RecommendedRoute)
    INSERT INTO BarangayConnections (FromBarangayID, ToBarangayID, DistanceMeters, RecommendedRoute) VALUES 
    (1, 2, 850, 'JP Rizal St. via Main Gate'),
    (2, 3, 1200, 'Sumulong Highway Eastbound'),
    (3, 4, 950, 'A. Bonifacio Ave (Flood-Free Route)'),
    (4, 9, 1500, 'Shoe Ave Extension'),
    (7, 8, 500, 'Malanday-Tumana Bridge'),
    (8, 12, 1100, 'General Ordonez St.'),
    (12, 11, 2000, 'Marikina-Infanta Hwy'),
    (5, 14, 750, 'Bayan-Bayanan Ave'),
    (14, 13, 1300, 'Fortune Avenue'),
    (15, 1, 2200, 'Katipunan Ext. via Panorama');

    COMMIT TRANSACTION;
    PRINT 'Database reset and recommendation paths inserted successfully!';
END TRY
BEGIN CATCH
    ROLLBACK TRANSACTION;
    SELECT ERROR_MESSAGE() AS ErrorMessage;
END CATCH
GO

-- 9. Final Verification
SELECT 'BarangayConnections' as TableName, COUNT(*) as TotalRows FROM BarangayConnections;
SELECT TOP 5 * FROM BarangayConnections;

