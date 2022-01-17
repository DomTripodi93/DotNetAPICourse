USE DotNetCourseDatabase

CREATE TABLE TutorialAppSchema.Post(
	PostId INT IDENTITY(1,1),
	UserId INT NOT NULL,
	PostDate DATETIME2,
	ChangeDate DATETIME2,
	PostTitle NVARCHAR(255),
	PostContent NVARCHAR(MAX)
)

CREATE CLUSTERED INDEX cix_Post_UserId ON TutorialAppSchema.Post(UserId)