-- Alternative: Manual SQL Script to Create Carts Table
-- Run this script if you prefer not to use Entity Framework migrations

-- Check if Carts table exists, if not create it
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Carts')
BEGIN
    CREATE TABLE [dbo].[Carts] (
        [Id] INT PRIMARY KEY IDENTITY(1,1),
        [KhachHangId] INT NOT NULL,
        [GiayId] INT NOT NULL,
        [Size] NVARCHAR(10) NOT NULL,
        [SoLuong] INT NOT NULL DEFAULT 1,
        [CreatedAt] DATETIME2 NOT NULL DEFAULT GETDATE(),
        
        -- Foreign key to KhachHangs table
        CONSTRAINT [FK_Carts_KhachHangs_KhachHangId] FOREIGN KEY ([KhachHangId]) 
            REFERENCES [dbo].[KhachHangs] ([Id])
            ON DELETE CASCADE,
        
        -- Foreign key to MonAns table (Giay table)
        CONSTRAINT [FK_Carts_MonAns_GiayId] FOREIGN KEY ([GiayId]) 
            REFERENCES [dbo].[MonAns] ([Id])
            ON DELETE CASCADE,
            
        -- Check constraint for quantity
        CONSTRAINT [CHK_Carts_SoLuong] CHECK ([SoLuong] >= 1 AND [SoLuong] <= 999)
    );
    
    -- Create indexes for better query performance
    CREATE INDEX [IX_Carts_KhachHangId] ON [dbo].[Carts] ([KhachHangId]);
    CREATE INDEX [IX_Carts_GiayId] ON [dbo].[Carts] ([GiayId]);
    
    PRINT 'Carts table created successfully!';
END
ELSE
BEGIN
    PRINT 'Carts table already exists.';
END
GO

-- Verify the table was created
SELECT 
    TABLE_NAME,
    COLUMN_NAME,
    DATA_TYPE,
    IS_NULLABLE,
    CHARACTER_MAXIMUM_LENGTH
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'Carts'
ORDER BY ORDINAL_POSITION;
GO
