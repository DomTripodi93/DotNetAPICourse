USE DotNetCourseDatabase
GO

CREATE PROCEDURE TutorialAppSchema.spPost_Delete
    @PostId INT
AS
BEGIN
    DELETE FROM TutorialAppSchema.Posts 
        WHERE PostId = @PostId
END
GO


CREATE OR ALTER PROCEDURE TutorialAppSchema.spPost_Delete
    @PostId INT
    , @UserId INT 
AS
BEGIN
    DELETE FROM TutorialAppSchema.Posts 
        WHERE PostId = @PostId
            AND UserId = @UserId
END