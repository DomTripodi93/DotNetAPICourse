USE DotNetCourseDatabase;
GO

CREATE OR ALTER PROCEDURE TutorialAppSchema.spRegistration_Upsert
    @Email NVARCHAR(50)
	, @UserId INT
    , @PasswordHash VARBINARY(MAX)
    , @PasswordSalt VARBINARY(MAX)
AS
BEGIN
    IF EXISTS (
                  SELECT    *
                    FROM    TutorialAppSchema.Auth
                   WHERE Auth.UserId = @UserId
              )
    BEGIN
        UPDATE  TutorialAppSchema.Auth
           SET  Auth.PasswordHash = @PasswordHash
                , Auth.PasswordSalt = @PasswordSalt
         WHERE  Auth.UserId = @UserId;
    END;
    ELSE
    BEGIN
        INSERT INTO TutorialAppSchema.Auth (UserId
											, Email
                                            , PasswordHash
                                            , PasswordSalt)
        VALUES (@UserId, @Email, @PasswordHash, @PasswordSalt);
    END;
END;
GO
