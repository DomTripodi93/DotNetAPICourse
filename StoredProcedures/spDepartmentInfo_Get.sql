USE DotNetCourseDatabase;
GO

CREATE OR ALTER PROCEDURE TutorialAppSchema.spDepartmentInfo_Get
	@Department NVARCHAR(50)
AS
BEGIN
    SELECT    UserJobInfo.Department
            , AVG (UserSalary.Salary) AS AverageSalaryInDepartment
            , MIN (UserSalary.Salary) AS MinSalaryInDepartment
            , MAX (UserSalary.Salary) AS MaxSalaryInDepartment
            , SUM (UserSalary.Salary) AS TotalSalaryPaidToDepartment
    FROM    TutorialAppSchema.UserJobInfo
        JOIN TutorialAppSchema.UserSalary
            ON UserSalary.UserId = UserJobInfo.UserId
    WHERE UserJobInfo.Department = ISNULL(@Department, UserJobInfo.Department)
    GROUP BY UserJobInfo.Department
END;
GO