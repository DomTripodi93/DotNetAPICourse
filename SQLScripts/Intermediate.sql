USE DotNetCourseDatabase;
GO

SELECT  Users.UserId
        , Users.FirstName + ' ' + Users.LastName AS FullName
        , Users.Email
        , Users.Gender
        , Users.Active
  FROM  TutorialAppSchema.Users AS Users
 WHERE  Users.Active = 1
 ORDER BY Users.UserId DESC;

SELECT  Users.UserId
        , Users.FirstName + ' ' + Users.LastName AS FullName
        , UserJobInfo.JobTitle
        , UserJobInfo.Department
        , UserSalary.Salary
        , Users.Email
        , Users.Gender
        , Users.Active
  FROM  TutorialAppSchema.Users AS Users
      --INNER JOIN
      JOIN TutorialAppSchema.UserSalary AS UserSalary
          ON UserSalary.UserId = Users.UserId
      LEFT JOIN TutorialAppSchema.UserJobInfo AS UserJobInfo
          ON UserJobInfo.UserId = Users.UserId
 WHERE  Users.Active = 1
 ORDER BY Users.UserId DESC;

-- DELETE FROM TutorialAppSchema.UserSalary WHERE UserId BETWEEN 250 AND 750 --501 Rows
-- --When we use BETWEEN we also include the Lower and Upper Bound of the value we're checking
SELECT  UserSalary.UserId
        , UserSalary.Salary
  FROM  TutorialAppSchema.UserSalary AS UserSalary
 WHERE  EXISTS (
                   SELECT   *
                     FROM   TutorialAppSchema.UserJobInfo AS UserJobInfo
                    WHERE   UserJobInfo.UserId = UserSalary.UserId
               )
        AND UserId <> 7;

SELECT  UserId
        , Salary
  FROM  TutorialAppSchema.UserSalary
-- UNION --Distinct /*Between the two queries*/
UNION ALL
SELECT  UserId
        , Salary
  FROM  TutorialAppSchema.UserSalary;

SELECT  Users.UserId
        , Users.FirstName + ' ' + Users.LastName AS FullName
        , UserJobInfo.JobTitle
        , UserJobInfo.Department
        , UserSalary.Salary
        , Users.Email
        , Users.Gender
        , Users.Active
  FROM  TutorialAppSchema.Users AS Users
      --INNER JOIN
      JOIN TutorialAppSchema.UserSalary AS UserSalary
          ON UserSalary.UserId = Users.UserId
      LEFT JOIN TutorialAppSchema.UserJobInfo AS UserJobInfo
          ON UserJobInfo.UserId = Users.UserId
 WHERE  Users.Active = 1
 ORDER BY Users.UserId DESC;

CREATE CLUSTERED INDEX cix_UserSalary_UserId
    ON TutorialAppSchema.UserSalary (UserId);

CREATE NONCLUSTERED INDEX ix_UserJobInfo_JobTitle
    ON TutorialAppSchema.UserJobInfo (JobTitle)
    INCLUDE (Department);

CREATE NONCLUSTERED INDEX fix_Users_Active
    ON TutorialAppSchema.Users (active)
    INCLUDE (Email, FirstName, LastName) --Also Includes UserId because it is our clustered Index 
    WHERE active = 1;

SELECT  ISNULL (UserJobInfo.Department, 'No Department Listed') AS Deparment
        , SUM (UserSalary.Salary) AS Salary
        , MIN (UserSalary.Salary) AS MinSalary
        , MAX (UserSalary.Salary) AS MaxSalary
        , AVG (UserSalary.Salary) AS AvgSalary
        , COUNT (*) AS PeopleInDepartment
        , STRING_AGG (Users.UserId, ', ') AS UserIds
        -- , STUFF ((
        --                 SELECT   DISTINCT
        --                             ', ' + InnerUsers.UserId
        --                     FROM   TutorialAppSchema.Users AS InnerUsers
        --                         LEFT JOIN TutorialAppSchema.UserJobInfo AS UserJobInfoInner
        --                             ON UserJobInfoInner.UserId = InnerUsers.UserId
        --                     WHERE   UserJobInfoInner.Department = UserJobInfo.Department
        --                 FOR XML PATH ('')
        --             )
        --             , 1
        --             , 1
        --             , ''
        --             ) AS UserIds
  FROM  TutorialAppSchema.Users AS Users
      --INNER JOIN
      JOIN TutorialAppSchema.UserSalary AS UserSalary
          ON UserSalary.UserId = Users.UserId
      LEFT JOIN TutorialAppSchema.UserJobInfo AS UserJobInfo
          ON UserJobInfo.UserId = Users.UserId
 WHERE  Users.Active = 1
 GROUP BY UserJobInfo.Department
 ORDER BY ISNULL (UserJobInfo.Department, 'No Department Listed') DESC;

SELECT  Users.UserId
        , Users.FirstName + ' ' + Users.LastName AS FullName
        , UserJobInfo.JobTitle
        , UserJobInfo.Department
        , DepartmentAverage.AvgSalary
        , UserSalary.Salary
        , Users.Email
        , Users.Gender
        , Users.Active
  FROM  TutorialAppSchema.Users AS Users
      --INNER JOIN
      JOIN TutorialAppSchema.UserSalary AS UserSalary
          ON UserSalary.UserId = Users.UserId
      LEFT JOIN TutorialAppSchema.UserJobInfo AS UserJobInfo
          ON UserJobInfo.UserId = Users.UserId
      -- OUTER APPLY ( -- Similar to LEFT JOIN
      CROSS APPLY ( -- Similar to JOIN
                      -- SELECT TOP 1 
                      SELECT    ISNULL (UserJobInfo2.Department, 'No Department Listed') AS Deparment
                                , AVG (UserSalary2.Salary) AS AvgSalary
                        FROM    TutorialAppSchema.UserSalary AS UserSalary2
                            LEFT JOIN TutorialAppSchema.UserJobInfo AS UserJobInfo2
                                ON UserJobInfo2.UserId = UserSalary2.UserId
                       WHERE UserJobInfo2.Department = UserJobInfo.Department
                       GROUP BY UserJobInfo2.Department
                  ) AS DepartmentAverage
 WHERE  Users.Active = 1
 ORDER BY Users.UserId DESC;

SELECT  GETDATE ();

SELECT  DATEADD (YEAR, -5, GETDATE ());

SELECT  DATEDIFF (MINUTE, DATEADD (YEAR, -5, GETDATE ()), GETDATE ());  -- Returns Positive

SELECT  DATEDIFF (MINUTE, GETDATE (), DATEADD (YEAR, -5, GETDATE ()));  -- Returns Negative

ALTER TABLE TutorialAppSchema.UserSalary ADD AvgSalary DECIMAL(18, 4);

SELECT  *
  FROM  TutorialAppSchema.UserSalary;

UPDATE  UserSalary
   SET  UserSalary.AvgSalary = DepartmentAverage.AvgSalary
  FROM  TutorialAppSchema.UserSalary AS UserSalary
      LEFT JOIN TutorialAppSchema.UserJobInfo AS UserJobInfo
          ON UserJobInfo.UserId = UserSalary.UserId
      CROSS APPLY ( -- Similar to JOIN
                      -- SELECT TOP 1 
                      SELECT    ISNULL (UserJobInfo2.Department, 'No Department Listed') AS Deparment
                                , AVG (UserSalary2.Salary) AS AvgSalary
                        FROM    TutorialAppSchema.UserSalary AS UserSalary2
                            LEFT JOIN TutorialAppSchema.UserJobInfo AS UserJobInfo2
                                ON UserJobInfo2.UserId = UserSalary2.UserId
                       WHERE ISNULL (UserJobInfo2.Department, 'No Department Listed') = ISNULL (UserJobInfo.Department, 'No Department Listed')
                       GROUP BY UserJobInfo2.Department
                  ) AS DepartmentAverage;

SELECT  Users.UserId
        , Users.FirstName + ' ' + Users.LastName AS FullName
        , UserJobInfo.JobTitle
        , UserJobInfo.Department
        , UserSalary.AvgSalary
        , UserSalary.Salary
        , Users.Email
        , Users.Gender
        , Users.Active
  FROM  TutorialAppSchema.Users AS Users
      --INNER JOIN
      JOIN TutorialAppSchema.UserSalary AS UserSalary
          ON UserSalary.UserId = Users.UserId
      LEFT JOIN TutorialAppSchema.UserJobInfo AS UserJobInfo
          ON UserJobInfo.UserId = Users.UserId
 WHERE  Users.Active = 1
 ORDER BY Users.UserId DESC;
