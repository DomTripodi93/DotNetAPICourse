USE DotNetCourseDatabase
GO

CREATE PROCEDURE TutorialAppSchema.spUser_Delete
    @UserId INT
AS
BEGIN
    DELETE FROM TutorialAppSchema.Users 
        WHERE UserId = @UserId
        
    DELETE FROM TutorialAppSchema.UserSalary 
        WHERE UserId = @UserId

    DELETE FROM TutorialAppSchema.UserJobInfo 
        WHERE UserId = @UserId
END
GO

CREATE OR ALTER PROCEDURE TutorialAppSchema.spUser_Delete
    @UserId INT
AS
BEGIN
    DECLARE @Email NVARCHAR(50);

    SELECT  @Email = Users.Email
      FROM  TutorialAppSchema.Users
     WHERE  Users.UserId = @UserId;

    DELETE  FROM TutorialAppSchema.UserSalary
     WHERE  UserSalary.UserId = @UserId;

    DELETE  FROM TutorialAppSchema.UserJobInfo
     WHERE  UserJobInfo.UserId = @UserId;

    DELETE  FROM TutorialAppSchema.Users
     WHERE  Users.UserId = @UserId;

    DELETE  FROM TutorialAppSchema.Auth
     WHERE  Auth.Email = @Email;
END;
GO