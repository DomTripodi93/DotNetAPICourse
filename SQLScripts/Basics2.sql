USE DotNetCourseDatabase;
GO

CREATE TABLE TutorialAppSchema.Users
(
    UserId INT IDENTITY(1, 1) PRIMARY KEY
    , FirstName NVARCHAR(50)
    , LastName NVARCHAR(50)
    , Email NVARCHAR(50)
    , Gender NVARCHAR(50)
    , Active BIT
);

CREATE TABLE TutorialAppSchema.UserSalary
(
    UserId INT
    , Salary DECIMAL(18, 4)
);

CREATE TABLE TutorialAppSchema.UserJobInfo
(
    UserId INT
    , JobTitle NVARCHAR(50)
    , Department NVARCHAR(50),
);