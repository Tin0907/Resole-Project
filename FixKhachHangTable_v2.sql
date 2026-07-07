-- Ki?m tra c?u trúc b?ng KhachHangs
USE QuanLyMonAn;
GO

-- Xem c?u trúc b?ng
SELECT 
    COLUMN_NAME,
    DATA_TYPE,
    CHARACTER_MAXIMUM_LENGTH,
    IS_NULLABLE,
    COLUMN_DEFAULT
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'KhachHangs'
ORDER BY ORDINAL_POSITION;

-- Xem d? li?u hi?n có
SELECT * FROM KhachHangs;

-- N?u có c?t ConfirmPassword, xóa nó ði
IF EXISTS (
    SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
    WHERE TABLE_NAME = 'KhachHangs' 
    AND COLUMN_NAME = 'ConfirmPassword'
)
BEGIN
    PRINT '=== T?M TH?Y C?T ConfirmPassword - ÐANG XÓA... ==='
    ALTER TABLE KhachHangs DROP COLUMN ConfirmPassword;
    PRINT '=== Ð? XÓA C?T ConfirmPassword THÀNH CÔNG ==='
END
ELSE
BEGIN
    PRINT '=== C?T ConfirmPassword KHÔNG T?N T?I - OK! ==='
END

-- Ki?m tra l?i c?u trúc sau khi xóa
PRINT '=== C?U TRÚC B?NG SAU KHI XÓA: ==='
SELECT 
    COLUMN_NAME,
    DATA_TYPE,
    CHARACTER_MAXIMUM_LENGTH,
    IS_NULLABLE
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'KhachHangs'
ORDER BY ORDINAL_POSITION;
