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