USE DotNetCourseDatabase;
GO

CREATE TABLE TestAppSchema.Users
(
    UserId INT IDENTITY(1, 1) PRIMARY KEY
    , FirstName NVARCHAR(50)
    , LastName NVARCHAR(50)
    , Gender NVARCHAR(50)
    , Active BIT
);

CREATE TABLE TestAppSchema.UserSalary
(
    UserId INT
    , Salary DECIMAL(18, 4)
);

CREATE TABLE TestAppSchema.UserJobInfo
(
    UserId INT
    , JobTitle NVARCHAR(50)
    , Department NVARCHAR(50),
);

SELECT  Users.UserId
        , Users.FirstName
        , Users.LastName
        , Users.Gender
        , Users.Active
  FROM  TestAppSchema.Users
 WHERE  Users.Active = 1;

SELECT  Users.UserId
        , Users.FirstName
        , Users.LastName
        , Users.Gender
        , Users.Active
        , UserJobInfo.Department
        , UserJobInfo.JobTitle
        , UserSalary.Salary
  FROM  TestAppSchema.Users
      JOIN TestAppSchema.UserJobInfo
          ON UserJobInfo.UserId = Users.UserId
      JOIN TestAppSchema.UserSalary
          ON UserSalary.UserId = Users.UserId
             AND Users.Active = 1;

SELECT  UserSalary.UserId
        , UserSalary.Salary
  FROM  TestAppSchema.UserSalary
 WHERE  EXISTS (
                   SELECT   *
                     FROM   TestAppSchema.Users
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
  FROM  TestAppSchema.Users
      JOIN TestAppSchema.UserJobInfo
          ON UserJobInfo.UserId = Users.UserId
      LEFT JOIN TestAppSchema.UserSalary
          ON UserSalary.UserId = Users.UserId
             AND Users.Active = 1;

SELECT  UserSalary.UserId
        , UserSalary.Salary
  FROM  TestAppSchema.UserSalary
UNION ALL
SELECT  UserSalary.UserId
        , UserSalary.Salary
  FROM  TestAppSchema.UserSalary;

SELECT  Users.UserId
        , Users.FirstName
        , Users.LastName
        , Users.Gender
        , Users.Active
        , UserJobInfo.Department
        , UserJobInfo.JobTitle
        , UserSalary.Salary
  FROM  TestAppSchema.Users
      JOIN TestAppSchema.UserJobInfo
          ON UserJobInfo.UserId = Users.UserId
      LEFT JOIN (
                    SELECT  UserSalary.UserId
                            , UserSalary.Salary
                      FROM  TestAppSchema.UserSalary
                    UNION ALL
                    SELECT  UserSalary.UserId
                            , UserSalary.Salary
                      FROM  TestAppSchema.UserSalary
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
  FROM  TestAppSchema.UserSalary
      RIGHT JOIN TestAppSchema.Users
          ON Users.UserId = UserSalary.UserId
             AND Users.Active = 1;

--CREATE NONCLUSTERED INDEX ix_UserSalary_UserId ON TestAppSchema.UserSalary(UserId) INCLUDE(Salary)

--CREATE CLUSTERED INDEX cix_UserJobInfo_UserId ON TestAppSchema.UserJobInfo(UserId)
SELECT  UserJobInfo.Department
        , SUM (UserSalary.Salary)
  FROM  TestAppSchema.Users
      JOIN TestAppSchema.UserJobInfo
          ON UserJobInfo.UserId = Users.UserId
      JOIN TestAppSchema.UserSalary
          ON UserSalary.UserId = Users.UserId
 GROUP BY UserJobInfo.Department;

SELECT  UserJobInfo.JobTitle
        , SUM (UserSalary.Salary)
  FROM  TestAppSchema.Users
      JOIN TestAppSchema.UserJobInfo
          ON UserJobInfo.UserId = Users.UserId
      JOIN TestAppSchema.UserSalary
          ON UserSalary.UserId = Users.UserId
 GROUP BY UserJobInfo.JobTitle;

SELECT  UserJobInfo.Department
        , SUM (UserSalary.Salary) SalarySum
        , AVG (UserSalary.Salary) AS SalaryAverage
        , MIN (UserSalary.Salary) AS SalaryMinimum
        , MAX (UserSalary.Salary) AS SalaryMaximum
        , STRING_AGG (UserSalary.UserId, ',') AS UsersInDepartment
  FROM  TestAppSchema.Users
      JOIN TestAppSchema.UserJobInfo
          ON UserJobInfo.UserId = Users.UserId
      JOIN TestAppSchema.UserSalary
          ON UserSalary.UserId = Users.UserId
 GROUP BY UserJobInfo.Department;

SELECT  CASE WHEN UserJobInfo.Department = 'Accounting' THEN 1 ELSE 0 END AS IsAccounting
        , SUM (UserSalary.Salary) SalarySum
        , AVG (UserSalary.Salary) AS SalaryAverage
        , MIN (UserSalary.Salary) AS SalaryMinimum
        , MAX (UserSalary.Salary) AS SalaryMaximum
        , STRING_AGG (UserSalary.UserId, ',') AS UsersInDepartment
  FROM  TestAppSchema.Users
      JOIN TestAppSchema.UserJobInfo
          ON UserJobInfo.UserId = Users.UserId
      JOIN TestAppSchema.UserSalary
          ON UserSalary.UserId = Users.UserId
 GROUP BY UserJobInfo.Department;

--GROUP BY CASE WHEN UserJobInfo.Department = 'Accounting' THEN 1 ELSE 0 END;
SELECT  UserJobInfo.JobTitle
        , SUM (UserSalary.Salary) SalarySum
        , AVG (UserSalary.Salary) AS SalaryAverage
        , MIN (UserSalary.Salary) AS SalaryMinimum
        , MAX (UserSalary.Salary) AS SalaryMaximum
        , STRING_AGG (UserSalary.UserId, ',') AS UsersInJobTitle
  FROM  TestAppSchema.Users
      JOIN TestAppSchema.UserJobInfo
          ON UserJobInfo.UserId = Users.UserId
      JOIN TestAppSchema.UserSalary
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
  FROM  TestAppSchema.Users
      JOIN TestAppSchema.UserJobInfo AS UJI
          ON UJI.UserId = Users.UserId
      LEFT JOIN TestAppSchema.UserSalary
          ON UserSalary.UserId = Users.UserId
             AND Users.Active = 1
      OUTER APPLY (
                      SELECT    UJI2.Department
                                , AVG (UserSalary.Salary) AS AvgSalaryInDepartment
                        FROM    TestAppSchema.Users
                            JOIN TestAppSchema.UserJobInfo UJI2
                                ON UJI2.UserId = Users.UserId
                            JOIN TestAppSchema.UserSalary
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
  FROM  TestAppSchema.Users
      JOIN TestAppSchema.UserJobInfo AS UJI
          ON UJI.UserId = Users.UserId
      LEFT JOIN TestAppSchema.UserSalary
          ON UserSalary.UserId = Users.UserId
             AND Users.Active = 1
      CROSS APPLY (
                      SELECT    UJI2.Department
                                , AVG (UserSalary.Salary) AS AvgSalaryInDepartment
                        FROM    TestAppSchema.Users
                            JOIN TestAppSchema.UserJobInfo UJI2
                                ON UJI2.UserId = Users.UserId
                            JOIN TestAppSchema.UserSalary
                                ON UserSalary.UserId = Users.UserId
                       WHERE UJI2.Department = UJI.Department
                             AND UJI2.Department = 'Accounting'
                       GROUP BY UJI2.Department
                  ) AS AvgSalaryInDepartment;

SELECT  GETDATE ();

SELECT  DATEADD (DAY, 25, GETDATE ());

SELECT  DATEADD (DAY, -25, GETDATE ());

SELECT  DATEDIFF (DAY, GETDATE (), DATEADD (DAY, -25, GETDATE ()));

ALTER TABLE TestAppSchema.UserSalary
ADD AvgForDepartment DECIMAL(18, 4);

ALTER TABLE TestAppSchema.UserSalary ADD AvgForJobTitle DECIMAL(18, 4);

UPDATE  UserSalary
   SET  UserSalary.AvgForDepartment = AvgSalaryInDepartment.AvgSalaryInDepartment
        , UserSalary.AvgForJobTitle = AvgSalaryForJobTitle.AvgSalaryForJobTitle
  FROM  TestAppSchema.UserSalary
      JOIN TestAppSchema.UserJobInfo AS UJI
          ON UJI.UserId = UserSalary.UserId
      OUTER APPLY (
                      SELECT    UJI2.Department
                                , AVG (UserSalary.Salary) AS AvgSalaryInDepartment
                        FROM    TestAppSchema.Users
                            JOIN TestAppSchema.UserJobInfo UJI2
                                ON UJI2.UserId = Users.UserId
                            JOIN TestAppSchema.UserSalary
                                ON UserSalary.UserId = Users.UserId
                       WHERE UJI2.Department = UJI.Department
                       GROUP BY UJI2.Department
                  ) AS AvgSalaryInDepartment
      OUTER APPLY (
                      SELECT    UJI2.JobTitle
                                , AVG (UserSalary.Salary) AS AvgSalaryForJobTitle
                        FROM    TestAppSchema.Users
                            JOIN TestAppSchema.UserJobInfo UJI2
                                ON UJI2.UserId = Users.UserId
                            JOIN TestAppSchema.UserSalary
                                ON UserSalary.UserId = Users.UserId
                       WHERE UJI2.JobTitle = UJI.JobTitle
                       GROUP BY UJI2.JobTitle
                  ) AS AvgSalaryForJobTitle;