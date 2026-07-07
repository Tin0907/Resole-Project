-- Ki?m tra c?u trúc b?ng KhachHangs
SELECT 
    COLUMN_NAME,
    DATA_TYPE,
    CHARACTER_MAXIMUM_LENGTH,
    IS_NULLABLE
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'KhachHangs'
ORDER BY ORDINAL_POSITION;

-- Xem d? li?u hi?n có
SELECT * FROM KhachHangs;

-- N?u c?t ConfirmPassword t?n t?i, xóa nó
IF EXISTS (
    SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
    WHERE TABLE_NAME = 'KhachHangs' 
    AND COLUMN_NAME = 'ConfirmPassword'
)
BEGIN
    ALTER TABLE KhachHangs DROP COLUMN ConfirmPassword;
    PRINT 'Ð? xóa c?t ConfirmPassword';
END
ELSE
BEGIN
    PRINT 'C?t ConfirmPassword không t?n t?i';
END
