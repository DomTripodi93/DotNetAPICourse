CREATE DATABASE DotNetCourseDatabase;
GO

USE DotNetCourseDatabase;
GO

CREATE SCHEMA TutorialAppSchema;
GO

CREATE TABLE TutorialAppSchema.Computer
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

CREATE TABLE TutorialAppSchema.ComputerForTestApp
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

INSERT INTO TutorialAppSchema.Computer (Motherboard
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

-- SELECT * FROM TutorialAppSchema.Computer

SELECT  Motherboard
        , CPUCores
        , HasWifi
        , HasLTE
        , ReleaseDate
        , Price
        , VideoCard
  FROM  TutorialAppSchema.Computer;

DELETE FROM TutorialAppSchema.Computer WHERE ComputerId = 1

TRUNCATE TABLE TutorialAppSchema.Computer

USE DotNetCourseDatabase;

INSERT INTO TutorialAppSchema.Computer (Motherboard
                                    , CPUCores
                                    , HasWifi
                                    , HasLTE
                                    , ReleaseDate
                                    , Price
                                    , VideoCard)

SELECT  Computer.Motherboard
      , Computer.CPUCores
      , Computer.HasWifi
      , Computer.HasLTE
      , Computer.ReleaseDate
      , Computer.Price
      , Computer.VideoCard
  FROM  TutorialAppSchema.Computer
 ORDER BY
    Computer.HasWifi
    , Computer.HasLTE DESC
    , Computer.ReleaseDate DESC;




