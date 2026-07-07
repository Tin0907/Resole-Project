-- Script ð? c?p nh?t m?t s? s?n ph?m thành "Special Shoe"
-- Ch?y script này trong SQL Server Management Studio ho?c Azure Data Studio

USE [your_database_name]; -- Thay your_database_name b?ng tên database c?a b?n
GO

-- C?p nh?t 3 s?n ph?m ð?u tiên thành Special Shoe (enum value = 4)
UPDATE TOP (3) MonAns
SET PhanLoai = 4
WHERE PhanLoai != 4 OR PhanLoai IS NULL;

-- Ho?c n?u b?n mu?n ch?n s?n ph?m c? th?, dùng:
-- UPDATE MonAns
-- SET PhanLoai = 4
-- WHERE Id IN (1, 2, 3); -- Thay 1, 2, 3 b?ng ID s?n ph?m b?n mu?n

-- Ki?m tra k?t qu?
SELECT Id, Ten, PhanLoai, Gia 
FROM MonAns 
WHERE PhanLoai = 4;

-- Xem t?t c? s?n ph?m v?i tên phân lo?i
SELECT 
    Id, 
    Ten, 
    CASE PhanLoai
        WHEN 1 THEN 'Giày Da'
        WHEN 2 THEN 'Giày Sneaker'
        WHEN 3 THEN 'Giày Th? Thao'
        WHEN 4 THEN 'Special Shoe'
        ELSE 'Unknown'
    END AS PhanLoaiName,
    Gia 
FROM MonAns;
GO
