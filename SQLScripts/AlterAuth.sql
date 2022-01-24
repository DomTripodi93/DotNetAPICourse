ALTER TABLE TutorialAppSchema.Auth ADD UserId INT;

UPDATE  Auth
   SET  Auth.UserId = Users.UserId
  FROM  TutorialAppSchema.Auth
      JOIN TutorialAppSchema.Users
          ON Users.Email = Auth.Email;