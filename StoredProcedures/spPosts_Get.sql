USE DotNetCourseDatabase
GO

CREATE OR ALTER PROCEDURE TutorialAppSchema.spPosts_Get
/*EXEC TutorialAppSchema.spPosts_Get @UserId = 1003, @SearchValue='Second'*/
/*EXEC TutorialAppSchema.spPosts_Get @PostId = 2*/
    @UserId INT = NULL
    , @SearchValue NVARCHAR(MAX) = NULL
    , @PostId INT = NULL
AS
BEGIN
    SELECT [Posts].[PostId],
        [Posts].[UserId],
        [Posts].[PostTitle],
        [Posts].[PostContent],
        [Posts].[PostCreated],
        [Posts].[PostUpdated] 
    FROM TutorialAppSchema.Posts AS Posts
        WHERE Posts.UserId = ISNULL(@UserId, Posts.UserId)
            AND Posts.PostId = ISNULL(@PostId, Posts.PostId)
            AND (@SearchValue IS NULL
                OR Posts.PostContent LIKE '%' + @SearchValue + '%'
                OR Posts.PostTitle LIKE '%' + @SearchValue + '%')
END