-- Script ð? thêm user test vào database
-- M?t kh?u: 123456 (ð? m? hóa MD5 = E10ADC3949BA59ABBE56E057F20F883E)

-- Xóa user test n?u ð? t?n t?i
DELETE FROM Nguoidungs WHERE Email = 'a@gmail.com';

-- Thêm user test m?i
INSERT INTO Nguoidungs ([User], HoTen, Email, ChucDanh, NgaySinh, Admin, Locked, Password)
VALUES 
('admin', 
 'Admin Test', 
 'a@gmail.com', 
 'Administrator', 
 '1990-01-01', 
 1,  -- Admin = true
 1,  -- Locked = true (tài kho?n ðý?c kích ho?t)
 'E10ADC3949BA59ABBE56E057F20F883E'  -- Password: 123456
);

-- Ki?m tra k?t qu?
SELECT * FROM Nguoidungs WHERE Email = 'a@gmail.com';
