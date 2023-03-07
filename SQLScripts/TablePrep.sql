USE DotNetCourseDatabase;
GO

DROP TABLE IF EXISTS TutorialAppSchema.Users

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

DROP TABLE IF EXISTS TutorialAppSchema.UserSalary

-- IF OBJECT_ID('TutorialAppSchema.UserSalary') IS NOT NULL
--     DROP TABLE TutorialAppSchema.UserSalary;
CREATE TABLE TutorialAppSchema.UserSalary
(
    UserId INT
    , Salary DECIMAL(18, 4)
);

DROP TABLE IF EXISTS TutorialAppSchema.UserJobInfo

-- IF OBJECT_ID('TutorialAppSchema.UserJobInfo') IS NOT NULL
--     DROP TABLE TutorialAppSchema.UserJobInfo;
CREATE TABLE TutorialAppSchema.UserJobInfo
(
    UserId INT
    , JobTitle NVARCHAR(50)
    , Department NVARCHAR(50),
);


DROP TABLE IF EXISTS TutorialAppSchema.Auth

-- IF OBJECT_ID('TutorialAppSchema.Auth') IS NOT NULL
--     DROP TABLE TutorialAppSchema.Auth;
CREATE TABLE TutorialAppSchema.Auth
(
	Email NVARCHAR(50) PRIMARY KEY,
	PasswordHash VARBINARY(MAX),
	PasswordSalt VARBINARY(MAX)
)


DROP TABLE IF EXISTS TutorialAppSchema.Posts

-- IF OBJECT_ID('TutorialAppSchema.Posts') IS NOT NULL
--     DROP TABLE TutorialAppSchema.Posts;
CREATE TABLE TutorialAppSchema.Posts 
(
    PostId INT IDENTITY(1,1),
    UserId INT,
    PostTitle NVARCHAR(255),
    PostContent NVARCHAR(MAX),
    PostCreated DATETIME,
    PostUpdated DATETIME
)

CREATE CLUSTERED INDEX cix_Posts_UserId_PostId ON TutorialAppSchema.Posts(UserId, PostId)



CREATE CLUSTERED INDEX cix_UserSalary_UserId
    ON TutorialAppSchema.UserSalary (UserId);

CREATE NONCLUSTERED INDEX ix_UserJobInfo_JobTitle
    ON TutorialAppSchema.UserJobInfo (JobTitle)
    INCLUDE (Department);

CREATE NONCLUSTERED INDEX fix_Users_Active
    ON TutorialAppSchema.Users (active)
    INCLUDE (Email, FirstName, LastName) --Also Includes UserId because it is our clustered Index 
    WHERE active = 1;