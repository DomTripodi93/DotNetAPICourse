USE DotNetCourseDatabase;
GO

CREATE OR ALTER PROCEDURE TutorialAppSchema.spLoginConfirmation_Get
    @Email NVARCHAR(50)
AS
BEGIN
    SELECT  Auth.Email
            , Auth.PasswordHash
            , Auth.PasswordSalt
            , Users.UserId
      FROM  TutorialAppSchema.Auth
          JOIN TutorialAppSchema.Users
              ON Users.Email = Auth.Email
     WHERE  Auth.Email = @Email;
END;
GO

CREATE OR ALTER PROCEDURE TutorialAppSchema.spLoginConfirmation_Get
    @Email NVARCHAR(50)
AS
BEGIN
    SELECT  Auth.Email
            , Auth.PasswordHash
            , Auth.PasswordSalt
            , Auth.UserId
      FROM  TutorialAppSchema.Auth
     WHERE  Auth.Email = @Email;
END;