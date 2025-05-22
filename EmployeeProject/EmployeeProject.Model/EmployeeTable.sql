CREATE TABLE Employees
(
    EmpID INT IDENTITY(1,1) PRIMARY KEY,
    EmpCode NVARCHAR(50) NOT NULL UNIQUE,
    Name NVARCHAR(100) NOT NULL,
    Salary INT NOT NULL,
    Mobile NVARCHAR(15) NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    CreatedOn DATETIME NOT NULL DEFAULT GETDATE(),
    ModifiedOn DATETIME NOT NULL DEFAULT GETDATE(),

    -- Check constraints for validation
    CONSTRAINT CHK_Mobile_Valid CHECK (Mobile LIKE '[0-9]%' AND LEN(Mobile) >= 10 AND LEN(Mobile) <= 15),
    CONSTRAINT CHK_Email_Valid CHECK (
        Email LIKE '_%@_%._%' AND CHARINDEX(' ', Email) = 0
    )
);

--INSERT INTO Employees (EmpCode, Name, Salary, Mobile, Email, CreatedOn, ModifiedOn)
--VALUES 
--('EMP001', 'Ravi Chandra', 60000, '9795889289', 'Ravi.yadav@businessnext.com', GETDATE(), GETDATE()),
--('EMP002', 'Tanmay Shrivastava', 75000, '9123456789', 'jtanmay.shrivastava@businessnext.com', GETDATE(), GETDATE());

--ALTER TABLE Employees
--ADD
--    Gender VARCHAR(10),
--    IsActive BIT DEFAULT 1,
--    Department VARCHAR(50);

--Update Employees set Gender='Male', IsActive=1,Department='Engineering'