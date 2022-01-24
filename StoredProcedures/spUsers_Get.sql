USE DotNetCourseDatabase;
GO

CREATE OR ALTER PROCEDURE TutorialAppSchema.spUsers_Get
AS
BEGIN
	/*EXEC TutorialAppSchema.spUsers_Get*/
    SELECT  Users.UserId
            , Users.FirstName
            , Users.LastName
            , Users.Gender
            , Users.Active
            , UJI.Department
            , UJI.JobTitle
            , ISNULL (UserSalary.Salary, AverageSalaryInDepartment.AverageSalaryInDepartment) AS Salary
            , CASE WHEN UserSalary.Salary IS NULL THEN 1 ELSE 0 END AS SalaryAssumed
      FROM  TutorialAppSchema.Users
          JOIN TutorialAppSchema.UserJobInfo AS UJI
              ON UJI.UserId = Users.UserId
          LEFT JOIN TutorialAppSchema.UserSalary
              ON UserSalary.UserId = Users.UserId
                 AND Users.Active = 1
          OUTER APPLY (
                          SELECT    UJI2.Department
                                    , AVG (UserSalary.Salary) AS AverageSalaryInDepartment
                            FROM    TutorialAppSchema.Users
                                JOIN TutorialAppSchema.UserJobInfo UJI2
                                    ON UJI2.UserId = Users.UserId
                                JOIN TutorialAppSchema.UserSalary
                                    ON UserSalary.UserId = Users.UserId
                           WHERE UJI2.Department = UJI.Department
                           GROUP BY UJI2.Department
                      ) AS AverageSalaryInDepartment;
END;
GO

CREATE OR ALTER PROCEDURE TutorialAppSchema.spUsers_Get
    @Department NVARCHAR(50) = NULL /* 'Accounting' */
AS
BEGIN
	/*EXEC TutorialAppSchema.spUsers_Get @Department = 'Accounting'*/
    SELECT  Users.UserId
            , Users.FirstName
            , Users.LastName
            , Users.Gender
            , Users.Active
            , UJI.Department
            , UJI.JobTitle
            , ISNULL (UserSalary.Salary, AverageSalaryInDepartment.AverageSalaryInDepartment) AS Salary
            , CASE WHEN UserSalary.Salary IS NULL THEN 1 ELSE 0 END AS SalaryAssumed
      FROM  TutorialAppSchema.Users
          JOIN TutorialAppSchema.UserJobInfo AS UJI
              ON UJI.UserId = Users.UserId
          LEFT JOIN TutorialAppSchema.UserSalary
              ON UserSalary.UserId = Users.UserId
                 AND Users.Active = 1
          OUTER APPLY (
                          SELECT    UJI2.Department
                                    , AVG (UserSalary.Salary) AS AverageSalaryInDepartment
                            FROM    TutorialAppSchema.Users
                                JOIN TutorialAppSchema.UserJobInfo UJI2
                                    ON UJI2.UserId = Users.UserId
                                JOIN TutorialAppSchema.UserSalary
                                    ON UserSalary.UserId = Users.UserId
                           WHERE UJI2.Department = UJI.Department
                           GROUP BY UJI2.Department
                      ) AS AverageSalaryInDepartment
     WHERE  UJI.Department = ISNULL (@Department, UJI.Department);
END;
GO

CREATE OR ALTER PROCEDURE TutorialAppSchema.spUsers_Get
    @Department NVARCHAR(50) = NULL
    , @JobTitle NVARCHAR(50) = NULL
AS
BEGIN
	/*EXEC TutorialAppSchema.spUsers_Get @Department = 'Accounting', @JobTitle='Sales Associate'*/
    DECLARE @ResultDate DATE;

    SET @ResultDate = CAST(GETDATE () AS DATE);

    SELECT  @ResultDate = CAST(GETDATE () AS DATE);

    SELECT  Users.UserId
            , Users.FirstName
            , Users.LastName
            , Users.Gender
            , Users.Active
            , UJI.Department
            , UJI.JobTitle
            , ISNULL (UserSalary.Salary, AverageSalaryInDepartment.AverageSalaryInDepartment) AS Salary
            , CASE WHEN UserSalary.Salary IS NULL THEN 1 ELSE 0 END AS SalaryAssumed
			, @ResultDate AS DateOfQuery
      FROM  TutorialAppSchema.Users
          JOIN TutorialAppSchema.UserJobInfo AS UJI
              ON UJI.UserId = Users.UserId
          LEFT JOIN TutorialAppSchema.UserSalary
              ON UserSalary.UserId = Users.UserId
                 AND Users.Active = 1
          OUTER APPLY (
                          SELECT    UJI2.Department
                                    , AVG (UserSalary.Salary) AS AverageSalaryInDepartment
                            FROM    TutorialAppSchema.Users
                                JOIN TutorialAppSchema.UserJobInfo UJI2
                                    ON UJI2.UserId = Users.UserId
                                JOIN TutorialAppSchema.UserSalary
                                    ON UserSalary.UserId = Users.UserId
                           WHERE UJI2.Department = UJI.Department
                           GROUP BY UJI2.Department
                      ) AS AverageSalaryInDepartment
     WHERE  UJI.Department = ISNULL (@Department, UJI.Department)
	 	AND UJI.JobTitle = ISNULL (@JobTitle, UJI.JobTitle);
END;
GO

CREATE OR ALTER PROCEDURE TutorialAppSchema.spUsers_Get
    @Department NVARCHAR(50) = NULL
    , @JobTitle NVARCHAR(50) = NULL
AS
BEGIN
	/*EXEC TutorialAppSchema.spUsers_Get @Department = 'Accounting', @JobTitle='Account'*/
    DECLARE @ResultDate DATE;

    SET @ResultDate = CAST(GETDATE () AS DATE);

    SELECT  @ResultDate = CAST(GETDATE () AS DATE);

    SELECT  UJI.Department
            , AVG (UserSalary.Salary) AS AverageSalaryInDepartment
            , MAX (UserSalary.Salary) AS MaximumSalaryInDepartment
            , MIN (UserSalary.Salary) AS MinimumSalaryInDepartment
            , SUM (UserSalary.Salary) AS TotalSalaryInDepartment
      INTO  #DepartmentAverages
      FROM  TutorialAppSchema.Users
          JOIN TutorialAppSchema.UserJobInfo UJI
              ON UJI.UserId = Users.UserId
          JOIN TutorialAppSchema.UserSalary
              ON UserSalary.UserId = Users.UserId
     GROUP BY UJI.Department;

    SELECT  Users.UserId
            , Users.FirstName
            , Users.LastName
            , Users.Gender
            , Users.Active
            , UJI.Department
            , UJI.JobTitle
            , ISNULL (UserSalary.Salary, AverageSalaryInDepartment.AverageSalaryInDepartment) AS Salary
            , CASE WHEN UserSalary.Salary IS NULL THEN 1 ELSE 0 END AS SalaryAssumed
			, @ResultDate AS DateOfQuery
      FROM  TutorialAppSchema.Users
          JOIN TutorialAppSchema.UserJobInfo AS UJI
              ON UJI.UserId = Users.UserId
          LEFT JOIN TutorialAppSchema.UserSalary
              ON UserSalary.UserId = Users.UserId
                 AND Users.Active = 1
          LEFT JOIN #DepartmentAverages AS AverageSalaryInDepartment
              ON AverageSalaryInDepartment.Department = UJI.Department
     WHERE  UJI.Department = ISNULL (@Department, UJI.Department)
	 	AND UJI.JobTitle LIKE '%' + ISNULL (@JobTitle, UJI.JobTitle) + '%';
END;
GO


CREATE OR ALTER   PROCEDURE TutorialAppSchema.UserInfo_Get
    @Department NVARCHAR(50) = NULL
    , @JobTitle NVARCHAR(50) = NULL
    , @UserId INT = NULL
AS
BEGIN
	/*EXEC TutorialAppSchema.UserInfo_Get @Department = 'Accounting', @JobTitle='Account'*/
    DECLARE @ResultDate DATE;

    SET @ResultDate = CAST(GETDATE () AS DATE);

    SELECT  @ResultDate = CAST(GETDATE () AS DATE);

    SELECT  UJI.Department
            , AVG (UserSalary.Salary) AS AverageSalaryInDepartment
            , MAX (UserSalary.Salary) AS MaximumSalaryInDepartment
            , MIN (UserSalary.Salary) AS MinimumSalaryInDepartment
            , SUM (UserSalary.Salary) AS TotalSalaryInDepartment
      INTO  #DepartmentAverages
      FROM  TutorialAppSchema.Users
          JOIN TutorialAppSchema.UserJobInfo UJI
              ON UJI.UserId = Users.UserId
          JOIN TutorialAppSchema.UserSalary
              ON UserSalary.UserId = Users.UserId
     GROUP BY UJI.Department;

    SELECT  Users.UserId
            , Users.FirstName
            , Users.LastName
            , Users.Gender
            , Users.Active
            , UJI.Department
            , UJI.JobTitle
            , ISNULL (UserSalary.Salary, AverageSalaryInDepartment.AverageSalaryInDepartment) AS Salary
            , CASE WHEN UserSalary.Salary IS NULL THEN 1 ELSE 0 END AS SalaryAssumed
			, @ResultDate AS DateOfQuery
      FROM  TutorialAppSchema.Users
          JOIN TutorialAppSchema.UserJobInfo AS UJI
              ON UJI.UserId = Users.UserId
          LEFT JOIN TutorialAppSchema.UserSalary
              ON UserSalary.UserId = Users.UserId
                 AND Users.Active = 1
          LEFT JOIN #DepartmentAverages AS AverageSalaryInDepartment
              ON AverageSalaryInDepartment.Department = UJI.Department
     WHERE  UJI.Department = ISNULL (@Department, UJI.Department)
	 	AND UJI.JobTitle LIKE '%' + ISNULL (@JobTitle, UJI.JobTitle) + '%'
		AND UJI.UserId = ISNULL (@UserId, UJI.UserId);
END;
GO

