USE DotNetCourseDatabase
GO

CREATE OR ALTER PROCEDURE TutorialAppSchema.spPosts_Upsert
    @UserId INT
    , @PostTitle NVARCHAR(255)
    , @PostContent NVARCHAR(MAX)
    , @PostId INT = NULL
AS
BEGIN
    IF NOT EXISTS (SELECT * FROM TutorialAppSchema.Posts WHERE PostId = @PostId)
        BEGIN
            INSERT INTO TutorialAppSchema.Posts(
                [UserId],
                [PostTitle],
                [PostContent],
                [PostCreated],
                [PostUpdated]
            ) VALUES (
                @UserId,
                @PostTitle,
                @PostContent,
                GETDATE(),
                GETDATE()
            )
        END
    ELSE
        BEGIN
            UPDATE TutorialAppSchema.Posts 
                SET PostTitle = @PostTitle,
                    PostContent = @PostContent,
                    PostUpdated = GETDATE()
                WHERE PostId = @PostId
        END
END

