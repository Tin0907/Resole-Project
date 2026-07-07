-- Create Carts table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Carts')
BEGIN
    CREATE TABLE [dbo].[Carts] (
        [Id] INT PRIMARY KEY IDENTITY(1,1),
        [KhachHangId] INT NOT NULL,
        [GiayId] INT NOT NULL,
        [Size] NVARCHAR(10) NOT NULL,
        [SoLuong] INT NOT NULL DEFAULT 1,
        [CreatedAt] DATETIME2 NOT NULL DEFAULT GETDATE(),
        
        CONSTRAINT [FK_Carts_KhachHangs] FOREIGN KEY ([KhachHangId]) 
            REFERENCES [dbo].[KhachHangs] ([KhachHangID]),
        
        CONSTRAINT [FK_Carts_Giays] FOREIGN KEY ([GiayId]) 
            REFERENCES [dbo].[MonAns] ([Id]),
            
        CONSTRAINT [CHK_SoLuong] CHECK ([SoLuong] >= 1 AND [SoLuong] <= 999)
    );
    
    -- Create index for faster queries
    CREATE INDEX [IX_Carts_KhachHangId] ON [dbo].[Carts] ([KhachHangId]);
    CREATE INDEX [IX_Carts_GiayId] ON [dbo].[Carts] ([GiayId]);
    
    PRINT 'Carts table created successfully!';
END
ELSE
BEGIN
    PRINT 'Carts table already exists.';
END
GO
