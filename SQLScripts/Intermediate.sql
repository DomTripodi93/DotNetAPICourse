USE DotNetCourseDatabase;
GO

SELECT  Users.UserId
        , Users.FirstName
        , Users.LastName
        , Users.Gender
        , Users.Active
  FROM  TutorialAppSchema.Users
 WHERE  Users.Active = 1;

SELECT  Users.UserId
        , Users.FirstName
        , Users.LastName
        , Users.Gender
        , Users.Active
        , UserJobInfo.Department
        , UserJobInfo.JobTitle
        , UserSalary.Salary
  FROM  TutorialAppSchema.Users
      JOIN TutorialAppSchema.UserJobInfo
          ON UserJobInfo.UserId = Users.UserId
      JOIN TutorialAppSchema.UserSalary
          ON UserSalary.UserId = Users.UserId
             AND Users.Active = 1;

SELECT  UserSalary.UserId
        , UserSalary.Salary
  FROM  TutorialAppSchema.UserSalary
 WHERE  EXISTS (
                   SELECT   *
                     FROM   TutorialAppSchema.Users
                    WHERE   Users.UserId = UserSalary.UserId
                            AND Users.Active = 1
               );

SELECT  Users.UserId
        , Users.FirstName
        , Users.LastName
        , Users.Gender
        , Users.Active
        , UserJobInfo.Department
        , UserJobInfo.JobTitle
        , UserSalary.Salary
  FROM  TutorialAppSchema.Users
      JOIN TutorialAppSchema.UserJobInfo
          ON UserJobInfo.UserId = Users.UserId
      LEFT JOIN TutorialAppSchema.UserSalary
          ON UserSalary.UserId = Users.UserId
             AND Users.Active = 1;

SELECT  UserSalary.UserId
        , UserSalary.Salary
  FROM  TutorialAppSchema.UserSalary
UNION ALL
SELECT  UserSalary.UserId
        , UserSalary.Salary
  FROM  TutorialAppSchema.UserSalary;

SELECT  Users.UserId
        , Users.FirstName
        , Users.LastName
        , Users.Gender
        , Users.Active
        , UserJobInfo.Department
        , UserJobInfo.JobTitle
        , UserSalary.Salary
  FROM  TutorialAppSchema.Users
      JOIN TutorialAppSchema.UserJobInfo
          ON UserJobInfo.UserId = Users.UserId
      LEFT JOIN (
                    SELECT  UserSalary.UserId
                            , UserSalary.Salary
                      FROM  TutorialAppSchema.UserSalary
                    UNION ALL
                    SELECT  UserSalary.UserId
                            , UserSalary.Salary
                      FROM  TutorialAppSchema.UserSalary
                ) UserSalary
          ON UserSalary.UserId = Users.UserId
             AND Users.Active = 1;

SELECT  UserSalary.UserId
        , UserSalary.Salary
        , Users.UserId
        , Users.FirstName
        , Users.LastName
        , Users.Gender
        , Users.Active
  FROM  TutorialAppSchema.UserSalary
      RIGHT JOIN TutorialAppSchema.Users
          ON Users.UserId = UserSalary.UserId
             AND Users.Active = 1;

--CREATE NONCLUSTERED INDEX ix_UserSalary_UserId ON TutorialAppSchema.UserSalary(UserId) INCLUDE(Salary)

--CREATE CLUSTERED INDEX cix_UserJobInfo_UserId ON TutorialAppSchema.UserJobInfo(UserId)
SELECT  UserJobInfo.Department
        , SUM (UserSalary.Salary)
  FROM  TutorialAppSchema.Users
      JOIN TutorialAppSchema.UserJobInfo
          ON UserJobInfo.UserId = Users.UserId
      JOIN TutorialAppSchema.UserSalary
          ON UserSalary.UserId = Users.UserId
 GROUP BY UserJobInfo.Department;

SELECT  UserJobInfo.JobTitle
        , SUM (UserSalary.Salary)
  FROM  TutorialAppSchema.Users
      JOIN TutorialAppSchema.UserJobInfo
          ON UserJobInfo.UserId = Users.UserId
      JOIN TutorialAppSchema.UserSalary
          ON UserSalary.UserId = Users.UserId
 GROUP BY UserJobInfo.JobTitle;

SELECT  UserJobInfo.Department
        , SUM (UserSalary.Salary) SalarySum
        , AVG (UserSalary.Salary) AS SalaryAverage
        , MIN (UserSalary.Salary) AS SalaryMinimum
        , MAX (UserSalary.Salary) AS SalaryMaximum
        , STRING_AGG (UserSalary.UserId, ',') AS UsersInDepartment
  FROM  TutorialAppSchema.Users
      JOIN TutorialAppSchema.UserJobInfo
          ON UserJobInfo.UserId = Users.UserId
      JOIN TutorialAppSchema.UserSalary
          ON UserSalary.UserId = Users.UserId
 GROUP BY UserJobInfo.Department;

SELECT  CASE WHEN UserJobInfo.Department = 'Accounting' THEN 1 ELSE 0 END AS IsAccounting
        , SUM (UserSalary.Salary) SalarySum
        , AVG (UserSalary.Salary) AS SalaryAverage
        , MIN (UserSalary.Salary) AS SalaryMinimum
        , MAX (UserSalary.Salary) AS SalaryMaximum
        , STRING_AGG (UserSalary.UserId, ',') AS UsersInDepartment
  FROM  TutorialAppSchema.Users
      JOIN TutorialAppSchema.UserJobInfo
          ON UserJobInfo.UserId = Users.UserId
      JOIN TutorialAppSchema.UserSalary
          ON UserSalary.UserId = Users.UserId
 GROUP BY UserJobInfo.Department;

--GROUP BY CASE WHEN UserJobInfo.Department = 'Accounting' THEN 1 ELSE 0 END;
SELECT  UserJobInfo.JobTitle
        , SUM (UserSalary.Salary) SalarySum
        , AVG (UserSalary.Salary) AS SalaryAverage
        , MIN (UserSalary.Salary) AS SalaryMinimum
        , MAX (UserSalary.Salary) AS SalaryMaximum
        , STRING_AGG (UserSalary.UserId, ',') AS UsersInJobTitle
  FROM  TutorialAppSchema.Users
      JOIN TutorialAppSchema.UserJobInfo
          ON UserJobInfo.UserId = Users.UserId
      JOIN TutorialAppSchema.UserSalary
          ON UserSalary.UserId = Users.UserId
 GROUP BY UserJobInfo.JobTitle;

SELECT  Users.UserId
        , Users.FirstName
        , Users.LastName
        , Users.Gender
        , Users.Active
        , UJI.Department
        , UJI.JobTitle
        , ISNULL (UserSalary.Salary, AvgSalaryInDepartment.AvgSalaryInDepartment) AS Salary
        , CASE WHEN UserSalary.Salary IS NULL THEN 1 ELSE 0 END AS SalaryAssumed
  FROM  TutorialAppSchema.Users
      JOIN TutorialAppSchema.UserJobInfo AS UJI
          ON UJI.UserId = Users.UserId
      LEFT JOIN TutorialAppSchema.UserSalary
          ON UserSalary.UserId = Users.UserId
             AND Users.Active = 1
      OUTER APPLY (
                      SELECT    UJI2.Department
                                , AVG (UserSalary.Salary) AS AvgSalaryInDepartment
                        FROM    TutorialAppSchema.Users
                            JOIN TutorialAppSchema.UserJobInfo UJI2
                                ON UJI2.UserId = Users.UserId
                            JOIN TutorialAppSchema.UserSalary
                                ON UserSalary.UserId = Users.UserId
                       WHERE UJI2.Department = UJI.Department
                       GROUP BY UJI2.Department
                  ) AS AvgSalaryInDepartment;

SELECT  Users.UserId
        , Users.FirstName
        , Users.LastName
        , Users.Gender
        , Users.Active
        , UJI.Department
        , UJI.JobTitle
        , ISNULL (UserSalary.Salary, AvgSalaryInDepartment.AvgSalaryInDepartment) AS Salary
        , CASE WHEN UserSalary.Salary IS NULL THEN 1 ELSE 0 END AS SalaryAssumed
  FROM  TutorialAppSchema.Users
      JOIN TutorialAppSchema.UserJobInfo AS UJI
          ON UJI.UserId = Users.UserId
      LEFT JOIN TutorialAppSchema.UserSalary
          ON UserSalary.UserId = Users.UserId
             AND Users.Active = 1
      CROSS APPLY (
                      SELECT    UJI2.Department
                                , AVG (UserSalary.Salary) AS AvgSalaryInDepartment
                        FROM    TutorialAppSchema.Users
                            JOIN TutorialAppSchema.UserJobInfo UJI2
                                ON UJI2.UserId = Users.UserId
                            JOIN TutorialAppSchema.UserSalary
                                ON UserSalary.UserId = Users.UserId
                       WHERE UJI2.Department = UJI.Department
                             AND UJI2.Department = 'Accounting'
                       GROUP BY UJI2.Department
                  ) AS AvgSalaryInDepartment;

SELECT  GETDATE ();

SELECT  DATEADD (DAY, 25, GETDATE ());

SELECT  DATEADD (DAY, -25, GETDATE ());

SELECT  DATEDIFF (DAY, GETDATE (), DATEADD (DAY, -25, GETDATE ()));

ALTER TABLE TutorialAppSchema.UserSalary
ADD AvgForDepartment DECIMAL(18, 4);

ALTER TABLE TutorialAppSchema.UserSalary ADD AvgForJobTitle DECIMAL(18, 4);

UPDATE  UserSalary
   SET  UserSalary.AvgForDepartment = AvgSalaryInDepartment.AvgSalaryInDepartment
        , UserSalary.AvgForJobTitle = AvgSalaryForJobTitle.AvgSalaryForJobTitle
  FROM  TutorialAppSchema.UserSalary
      JOIN TutorialAppSchema.UserJobInfo AS UJI
          ON UJI.UserId = UserSalary.UserId
      OUTER APPLY (
                      SELECT    UJI2.Department
                                , AVG (UserSalary.Salary) AS AvgSalaryInDepartment
                        FROM    TutorialAppSchema.Users
                            JOIN TutorialAppSchema.UserJobInfo UJI2
                                ON UJI2.UserId = Users.UserId
                            JOIN TutorialAppSchema.UserSalary
                                ON UserSalary.UserId = Users.UserId
                       WHERE UJI2.Department = UJI.Department
                       GROUP BY UJI2.Department
                  ) AS AvgSalaryInDepartment
      OUTER APPLY (
                      SELECT    UJI2.JobTitle
                                , AVG (UserSalary.Salary) AS AvgSalaryForJobTitle
                        FROM    TutorialAppSchema.Users
                            JOIN TutorialAppSchema.UserJobInfo UJI2
                                ON UJI2.UserId = Users.UserId
                            JOIN TutorialAppSchema.UserSalary
                                ON UserSalary.UserId = Users.UserId
                       WHERE UJI2.JobTitle = UJI.JobTitle
                       GROUP BY UJI2.JobTitle
                  ) AS AvgSalaryForJobTitle;