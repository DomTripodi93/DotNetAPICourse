CREATE OR ALTER PROCEDURE TutorialAppSchema.spPost_Delete
    @PostId INT = NULL
    , @UserId INT
AS
BEGIN
    IF EXISTS (
                  SELECT    *
                    FROM    TutorialAppSchema.Post
                   WHERE Post.PostId = @PostId
                         AND Post.UserId = @UserId
              )
    BEGIN
        DELETE  FROM TutorialAppSchema.Post
         WHERE  Post.PostId = @PostId
                AND Post.UserId = @UserId;
    END;
END;
