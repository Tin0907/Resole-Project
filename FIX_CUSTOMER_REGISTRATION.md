# HÝ?NG D?N S?A L?I ÐÃNG K? KHÁCH HÀNG

## ? V?n ð?
- Không ðãng k? ðý?c khách hàng
- Thông tin không lýu vào database
- Không có thông báo l?i r? ràng

## ? Nguyên nhân ð? t?m th?y

### 1. **Model KhachHang có l?i**
- `ConfirmPassword` có attribute `[Column]` ? EF c? lýu vào database
- Nhýng trong database KHÔNG có c?t này
- K?t qu?: SaveChanges() th?t b?i

### 2. **Service hash sai**
- `KhachhangSvc.AddKhachhang()` c? hash `ConfirmPassword`
- Nhýng `ConfirmPassword` là `[NotMapped]` ? không c?n hash

## ?? Ð? s?a

### **File 1: Models\Khachhang.cs**
```csharp
// confirmpassword - KHÔNG LÝU VÀO DATABASE
[Required(ErrorMessage = "B?n c?n nh?p l?i m?t kh?u")]
[Display(Name = "Xác nh?n m?t kh?u")]
[Compare("Password", ErrorMessage = "M?t kh?u không kh?p")]
[NotMapped]  // ? Thêm attribute này
public string ConfirmPassword { get; set; }
```

### **File 2: Services\KhachhangSvc.cs**
```csharp
public int AddKhachhang(KhachHang khachHang)
{
    int ret = 0;
    try
    {
        // Hash password trý?c khi lýu
        khachHang.Password = _mahoaHelper.Mahoa(khachHang.Password);
        // ConfirmPassword không c?n hash v? là [NotMapped]
        
        _context.Add(khachHang);
        _context.SaveChanges();
        ret = khachHang.Id;
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
        ret = 0;
    }
    return ret;
}
```

### **File 3: Controllers\KhachhangController.cs**
- Thêm logging chi ti?t
- Hi?n th? exception khi có l?i

### **File 4: Views\Khachhang\Register.cshtml**
- Hi?n th? t?t c? l?i validation

---

## ?? Ki?m tra và Test

### **Bý?c 1: Ch?y ?ng d?ng**
```bash
dotnet run
```
ho?c nh?n **F5**

### **Bý?c 2: Truy c?p Test Dashboard**
```
https://localhost:xxxx/Test/Index
```

### **Bý?c 3: Ch?y các test theo th? t?**

#### Test 1: Check Database Structure
```
/Test/CheckCustomers
```
- Xem danh sách khách hàng hi?n có
- Ki?m tra c?u trúc table

#### Test 2: Test Registration
```
/Test/TestRegisterCustomer
```
- T? ð?ng t?o 1 khách hàng test
- Email: test@customer.com
- Password: 123456

**K?t qu? mong ð?i:**
- ? "ÐÃNG K? THÀNH CÔNG!" màu xanh
- Hi?n th? Customer ID
- Nút "Ðãng nh?p ngay"

#### Test 3: Verify in Database
```sql
SELECT * FROM KhachHangs WHERE Email = 'test@customer.com';
```

**Nên th?y:**
- Id: (s?)
- Fullname: Test Customer
- Email: test@customer.com
- Password: E10ADC3949BA59ABBE56E057F20F883E (MD5 c?a "123456")
- PhoneNumber: 0123456789

---

## ?? N?u v?n l?i

### **L?i 1: "Cannot insert NULL into column 'ConfirmPassword'"**
**Nguyên nhân:** Database v?n có c?t `ConfirmPassword`

**Gi?i pháp:**
1. Ch?y script SQL:
```sql
-- Xóa c?t ConfirmPassword n?u t?n t?i
IF EXISTS (
    SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
    WHERE TABLE_NAME = 'KhachHangs' 
    AND COLUMN_NAME = 'ConfirmPassword'
)
BEGIN
    ALTER TABLE KhachHangs DROP COLUMN ConfirmPassword;
END
```

2. Ho?c ch?y l?i migration:
```bash
dotnet ef database drop
dotnet ef database update
```

### **L?i 2: "Email already exists"**
**Gi?i pháp:**
```sql
DELETE FROM KhachHangs WHERE Email = 'test@customer.com';
```

### **L?i 3: "ModelState is invalid"**
**Debug:**
1. Xem log trong Output window (View ? Output ? ASP.NET Core Web Server)
2. T?m d?ng "Validation error: ..."
3. S?a theo l?i hi?n th?

---

## ?? Test th? công qua UI

### **Bý?c 1: Truy c?p trang ðãng k?**
```
https://localhost:xxxx/Khachhang/Register
```

### **Bý?c 2: Ði?n form**
- **H? và tên:** Nguy?n Vãn A
- **Email:** test2@gmail.com
- **S? ði?n tho?i:** 0987654321
- **Ngày sinh:** 01/01/1995
- **M?t kh?u:** 123456
- **Xác nh?n m?t kh?u:** 123456

### **Bý?c 3: Submit**
- Nh?n "Ðãng k?"

### **K?t qu? mong ð?i:**
1. Redirect ð?n `/Khachhang/Login`
2. Hi?n th? thông báo xanh: "Ðãng k? thành công! Vui l?ng ðãng nh?p."
3. Có th? ðãng nh?p v?i:
   - Email: test2@gmail.com
   - Password: 123456

---

## ?? Debug Logs

### **Xem logs khi ðãng k?:**
1. M? **Output** window (View ? Output)
2. Ch?n "ASM - ASP.NET Core Web Server" trong dropdown
3. T?m các d?ng:
```
Registration attempt for email: ...
ModelState is valid, calling service...
Registration successful! Customer ID: ...
```

### **N?u th?t b?i, s? th?y:**
```
Registration exception: ...
Inner exception: ...
```

---

## ? Checklist hoàn thành

- [x] Thêm `[NotMapped]` cho `ConfirmPassword`
- [x] Xóa `[Column]` attribute kh?i `ConfirmPassword`
- [x] S?a `AddKhachhang()` - không hash `ConfirmPassword`
- [x] S?a `EditKhachhang()` - không reference `ConfirmPassword`
- [x] Thêm logging chi ti?t vào Controller
- [x] C?p nh?t Register view - hi?n th? l?i
- [x] T?o Test tools
- [x] Build thành công

---

## ?? Sau khi s?a

**Ðãng k? nên ho?t ð?ng:**
1. Vào `/Khachhang/Register`
2. Ði?n form ð?y ð?
3. Submit ? Chuy?n ð?n `/Khachhang/Login`
4. Ðãng nh?p thành công
5. Redirect ð?n `/ThucDon` (danh sách món ãn)

**N?u Admin thêm món m?i:**
- Khách hàng F5 trang ? Th?y món m?i ngay

---

## ?? Support

N?u v?n g?p v?n ð?:
1. Ch?y `/Test/TestRegisterCustomer`
2. Ch?p màn h?nh l?i
3. Xem log trong Output window
4. Ki?m tra c?u trúc table v?i script `FixKhachHangTable.sql`
