CREATE DATABASE DotNetCourseDatabase;
GO

USE DotNetCourseDatabase;
GO

CREATE SCHEMA TutorialAppSchema;
GO

CREATE TABLE TutorialAppSchema.Computer
(
    -- TableId INT  IDENTITY(Starting, Increment By) 
    ComputerId INT IDENTITY(1, 1) PRIMARY KEY
    -- , Motherboard CHAR(10) 'x' 'x         '
    -- , Motherboard VARCHAR(10) 'x' 'x'
    -- , Motherboard NVARCHAR(255) --'x'
    , Motherboard NVARCHAR(50)  --'x'
    , CPUCores INT              --NOT NULL
    , HasWifi BIT
    , HasLTE BIT
    , ReleaseDate DATETIME
    , Price DECIMAL(18, 4)
    , VideoCard NVARCHAR(50)
);
GO

SELECT  [ComputerId]
        , [Motherboard]
        , [CPUCores]
        , [HasWifi]
        , [HasLTE]
        , [ReleaseDate]
        , [Price]
        , [VideoCard]
  FROM  TutorialAppSchema.Computer;

INSERT INTO TutorialAppSchema.Computer ([Motherboard]
                                        , [CPUCores]
                                        , [HasWifi]
                                        , [HasLTE]
                                        , [ReleaseDate]
                                        , [Price]
                                        , [VideoCard])
VALUES ('Sample-Motherboard'
        , 4
        , 1  -- true
        , 0                         -- false
        , GETDATE ()
        , 1000.28
        , 'Sample-VideoCard');

-- DELETE FROM TutorialAppSchema.Computer WHERE ReleaseDate > '2018-10-31'
DELETE  FROM TutorialAppSchema.Computer
 WHERE  ComputerId = 1003;

UPDATE  TutorialAppSchema.Computer
   SET  Motherboard = 'Obsolete'
 WHERE  HasWifi = 0;

SELECT  [ComputerId]
        , [Motherboard]
        , ISNULL ([CPUCores], 4) AS CPUCores
        , [HasWifi]
        , [HasLTE]
        , [ReleaseDate]
        , [Price]
        , [VideoCard]
  FROM  TutorialAppSchema.Computer;

UPDATE  TutorialAppSchema.Computer
   SET  CPUCores = 4
 WHERE  CPUCores IS NULL;

SELECT  [ComputerId]
        , [Motherboard]
        , ISNULL ([CPUCores], 4) AS CPUCores
        , [HasWifi]
        , [HasLTE]
        , [ReleaseDate]
        , [Price]
        , [VideoCard]
  FROM  TutorialAppSchema.Computer
 ORDER BY
    HasLTE DESC
    , ReleaseDate DESC;

TRUNCATE TABLE TutorialAppSchema.Computer;
