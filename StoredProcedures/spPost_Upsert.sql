CREATE OR ALTER PROCEDURE TutorialAppSchema.spPost_Upsert
    @PostId INT = NULL
	, @UserId INT
	, @PostTitle NVARCHAR(255)
	, @PostContent NVARCHAR(MAX)
AS
BEGIN
	IF EXISTS (SELECT * FROM TutorialAppSchema.Post WHERE Post.PostId = @PostId AND Post.UserId = @UserId)
	BEGIN
		UPDATE TutorialAppSchema.Post 
			SET Post.PostTitle = @PostTitle
				, Post.PostContent = @PostContent
				, Post.ChangeDate = GETDATE()
			WHERE Post.PostId = @PostId AND Post.UserId = @UserId
	END
	ELSE
	BEGIN
		INSERT INTO TutorialAppSchema.Post (UserId
		                                    , PostDate
		                                    , ChangeDate
		                                    , PostTitle
		                                    , PostContent)
		VALUES (@UserId
		        , GETDATE()
		        , GETDATE()
		        , @PostTitle
		        , @PostContent
		    )
	END
END;
