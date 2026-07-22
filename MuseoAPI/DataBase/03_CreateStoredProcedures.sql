CREATE PROCEDURE sp_GetAllCategories
AS
BEGIN
    SET NOCOUNT ON;

    SELECT *
    FROM Categories;
END;
GO

CREATE PROCEDURE sp_GetMuseum
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP 1 *
    FROM Museums;
END;
GO

CREATE PROCEDURE sp_GetAllAnnouncements
AS
BEGIN
    SET NOCOUNT ON;

    SELECT *
    FROM Announcements
    ORDER BY CreatedAt DESC;
END;
GO

CREATE PROCEDURE sp_CreateAnnouncement
    @Title NVARCHAR(150),
    @Content NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Announcements (Title, Content)
    VALUES (@Title, @Content);

    SELECT CAST(SCOPE_IDENTITY() AS INT);
END;
GO

CREATE PROCEDURE sp_GetMediaByExhibitId
    @ExhibitId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT *
    FROM MediaItems
    WHERE ExhibitId = @ExhibitId;
END;
GO

CREATE PROCEDURE sp_CreateMediaItem
    @ExhibitId INT,
    @FileName NVARCHAR(150),
    @FileType NVARCHAR(50),
    @Url NVARCHAR(300)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO MediaItems (ExhibitId, FileName, FileType, Url)
    VALUES (@ExhibitId, @FileName, @FileType, @Url);

    SELECT CAST(SCOPE_IDENTITY() AS INT);
END;
GO

CREATE PROCEDURE sp_GetUserByUsername
    @Username NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT *
    FROM Users
    WHERE Username = @Username;
END;
GO

CREATE OR ALTER PROCEDURE sp_GetCategoryById
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, Name
    FROM Categories
    WHERE Id = @Id;
END;
GO

CREATE OR ALTER PROCEDURE sp_CreateCategory
    @Name NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Categories (Name)
    VALUES (@Name);

    SELECT CAST(SCOPE_IDENTITY() AS INT);
END;
GO

CREATE OR ALTER PROCEDURE sp_UpdateCategory
    @Id INT,
    @Name NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Categories
    SET Name = @Name
    WHERE Id = @Id;

    SELECT @@ROWCOUNT;
END;
GO

CREATE OR ALTER PROCEDURE sp_DeleteCategory
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM Categories
    WHERE Id = @Id;

    SELECT @@ROWCOUNT;
END;
GO

CREATE OR ALTER PROCEDURE sp_GetAllAnnouncements
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        Id,
        Title,
        Content,
        CreatedAt,
        Visible,
        StartDate,
        EndDate
    FROM Announcements
    ORDER BY CreatedAt DESC;
END;
GO

CREATE OR ALTER PROCEDURE sp_GetVisibleAnnouncements
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        Id,
        Title,
        Content,
        CreatedAt,
        Visible,
        StartDate,
        EndDate
    FROM Announcements
    WHERE Visible = 1
      AND (StartDate IS NULL OR StartDate <= GETDATE())
      AND (EndDate IS NULL OR EndDate >= GETDATE())
    ORDER BY CreatedAt DESC;
END;
GO

CREATE OR ALTER PROCEDURE sp_GetAnnouncementById
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        Id,
        Title,
        Content,
        CreatedAt,
        Visible,
        StartDate,
        EndDate
    FROM Announcements
    WHERE Id = @Id;
END;
GO

CREATE OR ALTER PROCEDURE sp_CreateAnnouncement
    @Title NVARCHAR(150),
    @Content NVARCHAR(MAX),
    @Visible BIT,
    @StartDate DATETIME2 = NULL,
    @EndDate DATETIME2 = NULL
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Announcements
    (
        Title,
        Content,
        Visible,
        StartDate,
        EndDate
    )
    VALUES
    (
        @Title,
        @Content,
        @Visible,
        @StartDate,
        @EndDate
    );

    SELECT CAST(SCOPE_IDENTITY() AS INT);
END;
GO

CREATE OR ALTER PROCEDURE sp_UpdateAnnouncement
    @Id INT,
    @Title NVARCHAR(150),
    @Content NVARCHAR(MAX),
    @Visible BIT,
    @StartDate DATETIME2 = NULL,
    @EndDate DATETIME2 = NULL
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Announcements
    SET
        Title = @Title,
        Content = @Content,
        Visible = @Visible,
        StartDate = @StartDate,
        EndDate = @EndDate
    WHERE Id = @Id;

    SELECT @@ROWCOUNT;
END;
GO

CREATE OR ALTER PROCEDURE sp_DeleteAnnouncement
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM Announcements
    WHERE Id = @Id;

    SELECT @@ROWCOUNT;
END;
GO

CREATE OR ALTER PROCEDURE sp_GetAllMediaItems
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        Id,
        ExhibitId,
        FileName,
        FileType,
        Url
    FROM MediaItems;
END;
GO

CREATE OR ALTER PROCEDURE sp_GetMediaItemById
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        Id,
        ExhibitId,
        FileName,
        FileType,
        Url
    FROM MediaItems
    WHERE Id = @Id;
END;
GO

CREATE OR ALTER PROCEDURE sp_UpdateMediaItem
    @Id INT,
    @ExhibitId INT,
    @FileName NVARCHAR(150),
    @FileType NVARCHAR(50),
    @Url NVARCHAR(300)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE MediaItems
    SET
        ExhibitId = @ExhibitId,
        FileName = @FileName,
        FileType = @FileType,
        Url = @Url
    WHERE Id = @Id;

    SELECT @@ROWCOUNT;
END;
GO

CREATE OR ALTER PROCEDURE sp_DeleteMediaItem
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM MediaItems
    WHERE Id = @Id;

    SELECT @@ROWCOUNT;
END;
GO

CREATE OR ALTER PROCEDURE sp_UpdateMuseum
    @Name NVARCHAR(150),
    @Description NVARCHAR(MAX),
    @Location NVARCHAR(200),
    @OpeningHours NVARCHAR(100),
    @Phone NVARCHAR(50),
    @Email NVARCHAR(100),
    @Website NVARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Museums
    SET
        Name = @Name,
        Description = @Description,
        Location = @Location,
        OpeningHours = @OpeningHours,
        Phone = @Phone,
        Email = @Email,
        Website = @Website;

    SELECT @@ROWCOUNT;
END;
GO

CREATE OR ALTER PROCEDURE sp_GetExhibitsByCategoryId
    @CategoryId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        Id,
        Title,
        Description,
        Year,
        CategoryId,
        ImageUrl
    FROM Exhibits
    WHERE CategoryId = @CategoryId;
END;
GO

CREATE OR ALTER PROCEDURE sp_UpdateExhibit
    @Id INT,
    @Title NVARCHAR(200),
    @Description NVARCHAR(MAX),
    @Year INT,
    @ImageUrl NVARCHAR(500),
    @CategoryId INT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Exhibits
    SET
        Title = @Title,
        Description = @Description,
        Year = @Year,
        ImageUrl = @ImageUrl,
        CategoryId = @CategoryId
    WHERE Id = @Id;

    SELECT @@ROWCOUNT;
END;
GO

CREATE OR ALTER PROCEDURE sp_DeleteExhibit
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM Exhibits
    WHERE Id = @Id;

    SELECT @@ROWCOUNT;
END;
GO