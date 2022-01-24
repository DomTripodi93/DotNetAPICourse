CREATE OR ALTER PROCEDURE TutorialAppSchema.spPosts_Get
    @UserId INT = NULL
AS
BEGIN
    SELECT  Post.PostId
            , Post.UserId
            , Post.PostDate
            , Post.ChangeDate
            , Post.PostTitle
            , Post.PostContent
      FROM  TutorialAppSchema.Post
     WHERE  Post.UserId = ISNULL (@UserId, Post.UserId);
END;
GO

CREATE OR ALTER PROCEDURE TutorialAppSchema.spPosts_Get
    @UserId INT = NULL
	, @SearchFor NVARCHAR(MAX) =''
AS
BEGIN
    SELECT  Post.PostId
            , Post.UserId
            , Post.PostDate
            , Post.ChangeDate
            , Post.PostTitle
            , Post.PostContent
      FROM  TutorialAppSchema.Post
     WHERE  Post.UserId = ISNULL (@UserId, Post.UserId)
		AND (Post.PostTitle LIKE '%' + @SearchFor + '%'
			OR Post.PostContent LIKE '%' + @SearchFor + '%');
END;
