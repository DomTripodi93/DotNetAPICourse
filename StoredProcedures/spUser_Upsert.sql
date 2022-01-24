USE DotNetCourseDatabase;
GO

CREATE OR ALTER PROCEDURE TutorialAppSchema.spUser_Upsert
    @UserId INT = NULL
    , @FirstName NVARCHAR(50)
    , @LastName NVARCHAR(50)
    , @Email NVARCHAR(50)
    , @Gender NVARCHAR(50)
    , @Salary DECIMAL(18, 4)
    , @JobTitle NVARCHAR(50)
    , @Department NVARCHAR(50)
    , @Active BIT = 1
AS
BEGIN
    IF @UserId IS NULL
    BEGIN
        INSERT INTO TutorialAppSchema.Users (FirstName
                                             , LastName
                                             , Email
                                             , Gender
                                             , Active)
        VALUES (@FirstName, @LastName, @Email, @Gender, @Active);

        SET @UserId = @@IDENTITY;

        INSERT INTO TutorialAppSchema.UserSalary (UserId
                                                  , Salary)
        VALUES (@UserId, @Salary);

        INSERT INTO TutorialAppSchema.UserJobInfo (UserId
                                                   , JobTitle
                                                   , Department)
        VALUES (@UserId, @JobTitle, @Department);
    END;
    ELSE
    BEGIN
        DECLARE @OriginalEmail NVARCHAR(50);

        SELECT  @OriginalEmail = Users.Email
          FROM  TutorialAppSchema.Users
         WHERE  Users.UserId = @UserId;

        UPDATE  TutorialAppSchema.Auth
           SET  Auth.Email = @Email
         WHERE  Auth.Email = @OriginalEmail;

        UPDATE  TutorialAppSchema.Users
           SET  Users.FirstName = @FirstName
                , Users.LastName = @LastName
                , Users.Email = @Email
                , Users.Gender = @Gender
                , Users.Active = @Active
         WHERE  Users.UserId = @UserId;

        UPDATE  TutorialAppSchema.UserSalary
           SET  UserSalary.Salary = @Salary
         WHERE  UserSalary.UserId = @UserId;

        UPDATE  TutorialAppSchema.UserJobInfo
           SET  UserJobInfo.Department = @Department
                , UserJobInfo.JobTitle = @JobTitle
         WHERE  UserJobInfo.UserId = @UserId;
    END;
END;
GO

CREATE OR ALTER PROCEDURE TutorialAppSchema.spUser_Upsert
    @UserId INT = NULL
    , @FirstName NVARCHAR(50)
    , @LastName NVARCHAR(50)
    , @Email NVARCHAR(50)
    , @Gender NVARCHAR(50)
    , @Salary DECIMAL(18, 4)
    , @JobTitle NVARCHAR(50)
    , @Department NVARCHAR(50)
    , @Active BIT = 1
AS
BEGIN
    DECLARE @HasMainRecord BIT = 0
            , @HasSalaryRecord BIT = 0
            , @HasJobInfoRecord BIT = 0;

    IF (
           @UserId IS NOT NULL
           AND  EXISTS (
                           SELECT   *
                             FROM   TutorialAppSchema.Users
                            WHERE   Users.UserId = @UserId
                       )
       )
       OR   (EXISTS (
                        SELECT  *
                          FROM  TutorialAppSchema.Users
                         WHERE  Users.Email = @Email
                    )
            )
    BEGIN
        SET @HasMainRecord = 1;

        IF EXISTS (
                      SELECT    *
                        FROM    TutorialAppSchema.UserSalary
                       WHERE UserSalary.UserId = @UserId
                  )
        BEGIN
            SET @HasSalaryRecord = 1;
        END;

        IF EXISTS (
                      SELECT    *
                        FROM    TutorialAppSchema.UserJobInfo
                       WHERE UserJobInfo.UserId = @UserId
                  )
        BEGIN
            SET @HasJobInfoRecord = 1;
        END;
    END;

    IF @HasMainRecord = 0
    BEGIN
        INSERT INTO TutorialAppSchema.Users (FirstName
                                             , LastName
                                             , Email
                                             , Gender
                                             , Active)
        VALUES (@FirstName, @LastName, @Email, @Gender, @Active);

        SET @UserId = @@IDENTITY;
    END;
    ELSE
    BEGIN
        DECLARE @OriginalEmail NVARCHAR(50);

        SELECT  @OriginalEmail = Users.Email
          FROM  TutorialAppSchema.Users
         WHERE  Users.UserId = @UserId;

        UPDATE  TutorialAppSchema.Auth
           SET  Auth.Email = @Email
         WHERE  Auth.Email = @OriginalEmail;

        UPDATE  TutorialAppSchema.Users
           SET  Users.FirstName = @FirstName
                , Users.LastName = @LastName
                , Users.Email = @Email
                , Users.Gender = @Gender
                , Users.Active = @Active
         WHERE  Users.UserId = @UserId;
    END;

    IF @HasSalaryRecord = 0
    BEGIN
        INSERT INTO TutorialAppSchema.UserSalary (UserId
                                                  , Salary)
        VALUES (@UserId, @Salary);
    END;
    ELSE
    BEGIN
        UPDATE  TutorialAppSchema.UserSalary
           SET  UserSalary.Salary = @Salary
         WHERE  UserSalary.UserId = @UserId;
    END;

    IF @HasJobInfoRecord = 0
    BEGIN
        INSERT INTO TutorialAppSchema.UserJobInfo (UserId
                                                   , JobTitle
                                                   , Department)
        VALUES (@UserId, @JobTitle, @Department);
    END;
    ELSE
    BEGIN
        UPDATE  TutorialAppSchema.UserJobInfo
           SET  UserJobInfo.Department = @Department
                , UserJobInfo.JobTitle = @JobTitle
         WHERE  UserJobInfo.UserId = @UserId;
    END;
END;
GO

CREATE OR ALTER PROCEDURE TutorialAppSchema.spUser_Upsert
    @UserId INT = NULL
    , @FirstName NVARCHAR(50)
    , @LastName NVARCHAR(50)
    , @Email NVARCHAR(50)
    , @Gender NVARCHAR(50)
    , @Salary DECIMAL(18, 4)
    , @JobTitle NVARCHAR(50)
    , @Department NVARCHAR(50)
    , @Active BIT = 1
AS
BEGIN
    DECLARE @HasMainRecord BIT = 0
            , @HasSalaryRecord BIT = 0
            , @HasJobInfoRecord BIT = 0;

    IF (
           @UserId IS NOT NULL
           AND  EXISTS (
                           SELECT   *
                             FROM   TutorialAppSchema.Users
                            WHERE   Users.UserId = @UserId
                       )
       )
       OR   (EXISTS (
                        SELECT  *
                          FROM  TutorialAppSchema.Users
                         WHERE  Users.Email = @Email
                    )
            )
    BEGIN
        SET @HasMainRecord = 1;

        IF EXISTS (
                      SELECT    *
                        FROM    TutorialAppSchema.UserSalary
                       WHERE UserSalary.UserId = @UserId
                  )
        BEGIN
            SET @HasSalaryRecord = 1;
        END;

        IF EXISTS (
                      SELECT    *
                        FROM    TutorialAppSchema.UserJobInfo
                       WHERE UserJobInfo.UserId = @UserId
                  )
        BEGIN
            SET @HasJobInfoRecord = 1;
        END;
    END;

    IF @HasMainRecord = 0
    BEGIN
        INSERT INTO TutorialAppSchema.Users (FirstName
                                             , LastName
                                             , Email
                                             , Gender
                                             , Active)
        VALUES (@FirstName, @LastName, @Email, @Gender, @Active);

        SET @UserId = @@IDENTITY;
    END;
    ELSE
    BEGIN
        DECLARE @OriginalEmail NVARCHAR(50);

        SELECT  @OriginalEmail = Users.Email
          FROM  TutorialAppSchema.Users
         WHERE  Users.UserId = @UserId;

        UPDATE  TutorialAppSchema.Auth
           SET  Auth.Email = @Email
         WHERE  Auth.Email = @OriginalEmail;

        UPDATE  TutorialAppSchema.Users
           SET  Users.FirstName = @FirstName
                , Users.LastName = @LastName
                , Users.Email = @Email
                , Users.Gender = @Gender
                , Users.Active = @Active
         WHERE  Users.UserId = @UserId;
    END;

    IF @HasSalaryRecord = 0
    BEGIN
        INSERT INTO TutorialAppSchema.UserSalary (UserId
                                                  , Salary)
        VALUES (@UserId, @Salary);
    END;
    ELSE
    BEGIN
        UPDATE  TutorialAppSchema.UserSalary
           SET  UserSalary.Salary = @Salary
         WHERE  UserSalary.UserId = @UserId;
    END;

    IF @HasJobInfoRecord = 0
    BEGIN
        INSERT INTO TutorialAppSchema.UserJobInfo (UserId
                                                   , JobTitle
                                                   , Department)
        VALUES (@UserId, @JobTitle, @Department);
    END;
    ELSE
    BEGIN
        UPDATE  TutorialAppSchema.UserJobInfo
           SET  UserJobInfo.Department = @Department
                , UserJobInfo.JobTitle = @JobTitle
         WHERE  UserJobInfo.UserId = @UserId;
    END;

	SELECT @UserId
END;
GO