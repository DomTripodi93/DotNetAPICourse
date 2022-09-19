TutorialAppSchema.spUsers_Get
    @UserId INT = NULL
    , @Active BIT = NULL


TutorialAppSchema.spUser_Upsert
	@FirstName NVARCHAR(50),
	@LastName NVARCHAR(50),
	@Email NVARCHAR(50),
	@Gender NVARCHAR(50),
	@JobTitle NVARCHAR(50),
	@Department NVARCHAR(50),
    @Salary DECIMAL(18, 4),
	@Active BIT = 1,
	@UserId INT = NULL


TutorialAppSchema.spUser_Delete
    @UserId INT




TutorialAppSchema.spPosts_Get
    @UserId INT = NULL
    , @SearchValue NVARCHAR(MAX) = NULL
    , @PostId INT = NULL


TutorialAppSchema.spPosts_Upsert
    @UserId INT
    , @PostTitle NVARCHAR(255)
    , @PostContent NVARCHAR(MAX)
    , @PostId INT = NULL


TutorialAppSchema.spPost_Delete
    @PostId INT
    , @UserId INT 


TutorialAppSchema.spRegistration_Upsert
    @Email NVARCHAR(50),
    @PasswordHash VARBINARY(MAX),
    @PasswordSalt VARBINARY(MAX)

TutorialAppSchema.spLoginConfirmation_Get
    @Email NVARCHAR(50)
