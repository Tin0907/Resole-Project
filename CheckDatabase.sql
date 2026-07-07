-- Ki?m tra tài kho?n ngý?i dùng trong database
SELECT 
    NguoiDungID,
    [User],
    HoTen,
    Email,
    Admin,
    Locked,
    Password,
    LEN(Password) as PasswordLength
FROM Nguoidungs;

-- Ki?m tra m?t kh?u ð? m? hóa MD5 c?a "123456"
-- K?t qu? mong ð?i: E10ADC3949BA59ABBE56E057F20F883E
