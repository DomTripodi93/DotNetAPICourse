USE DotNetCourseDatabase
GO

CREATE OR ALTER PROCEDURE TutorialAppSchema.spUser_Delete
    @UserId INT
AS
BEGIN
    /*EXEC TutorialAppSchema.spUser_Delete @@UserId = '12'*/
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