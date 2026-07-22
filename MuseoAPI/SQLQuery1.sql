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