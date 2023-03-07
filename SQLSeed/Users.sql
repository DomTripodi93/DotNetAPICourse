DROP TABLE IF EXISTS TutorialAppSchema.Users;

-- IF OBJECT_ID('TutorialAppSchema.Users') IS NOT NULL
--     DROP TABLE TutorialAppSchema.Users;

CREATE TABLE TutorialAppSchema.Users
(
    UserId INT IDENTITY(1, 1) PRIMARY KEY
    , FirstName NVARCHAR(50)
    , LastName NVARCHAR(50)
    , Email NVARCHAR(50)
    , Gender NVARCHAR(50)
    , Active BIT
);

DROP TABLE IF EXISTS TutorialAppSchema.UserSalary;

-- IF OBJECT_ID('TutorialAppSchema.UserSalary') IS NOT NULL
--     DROP TABLE TutorialAppSchema.UserSalary;

CREATE TABLE TutorialAppSchema.UserSalary
(
    UserId INT
    , Salary DECIMAL(18, 4)
);

DROP TABLE IF EXISTS TutorialAppSchema.UserJobInfo;

-- IF OBJECT_ID('TutorialAppSchema.UserJobInfo') IS NOT NULL
--     DROP TABLE TutorialAppSchema.UserJobInfo;

CREATE TABLE TutorialAppSchema.UserJobInfo
(
    UserId INT
    , JobTitle NVARCHAR(50)
    , Department NVARCHAR(50),
);

-- USE DotNetCourseDatabase;
-- GO

-- SELECT  [UserId]
--         , [FirstName]
--         , [LastName]
--         , [Email]
--         , [Gender]
--         , [Active]
--   FROM  TutorialAppSchema.Users;

-- SELECT  [UserId]
--         , [Salary]
--   FROM  TutorialAppSchema.UserSalary;

-- SELECT  [UserId]
--         , [JobTitle]
--         , [Department]
--   FROM  TutorialAppSchema.UserJobInfo;