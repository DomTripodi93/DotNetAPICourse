CREATE DATABASE TestDatabase;
GO

USE TestDatabase;
GO

CREATE SCHEMA TestAppSchema;
GO

CREATE TABLE TestAppSchema.Computer
(
    ComputerId INT IDENTITY(1, 1) PRIMARY KEY
    , Motherboard NVARCHAR(50)
    , CPUCores INT
    , HasWifi BIT
    , HasLTE BIT
    , ReleaseDate DATE
    , Price DECIMAL(18, 4)
    , VideoCard NVARCHAR(50)
);

CREATE TABLE TestAppSchema.ComputerForTestApp
(
    ComputerId INT IDENTITY(1, 1) PRIMARY KEY
    , Motherboard NVARCHAR(50)
    , CPUCores INT
    , HasWifi BIT
    , HasLTE BIT
    , ReleaseDate DATE
    , Price DECIMAL(18, 4)
    , VideoCard NVARCHAR(50)
);

INSERT INTO TestAppSchema.Computer (Motherboard
                                    , CPUCores
                                    , HasWifi
                                    , HasLTE
                                    , ReleaseDate
                                    , Price
                                    , VideoCard)
							VALUES ('Z690'
									, 4
									, 'True'
									, 'False'
									, '2021-12-27'
									, 859.95
									, 'rtx 2060');

-- SELECT * FROM TestAppSchema.Computer

SELECT  Motherboard
        , CPUCores
        , HasWifi
        , HasLTE
        , ReleaseDate
        , Price
        , VideoCard
  FROM  TestAppSchema.Computer;

DELETE FROM TestAppSchema.Computer WHERE ComputerId = 1

TRUNCATE TABLE TestAppSchema.Computer