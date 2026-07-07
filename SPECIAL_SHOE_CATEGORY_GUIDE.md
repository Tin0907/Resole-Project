# Hý?ng D?n Thêm Special Shoe Category

## T?ng quan
Ð? thêm phân lo?i "Special Shoe" vào h? th?ng. Ph?n **SPECIAL SNEAKER COLLECTION** ? trang Home s? ch? hi?n th? nh?ng s?n ph?m có phân lo?i này.

## Các thay ð?i ð? th?c hi?n

### 1. Thêm enum value m?i vào Models/Giay.cs
```csharp
public enum PhanLoai
{
    [Display(Name="Giày Da")]
    GiayDa = 1,
    [Display(Name = "Giày Sneaker")]
    GiaySneaker = 2,
    [Display(Name = "Giày Th? Thao")]
    GiayTheThao = 3,
    [Display(Name = "Special Shoe")]
    SpecialShoe = 4  // M?I
}
```

### 2. Thêm method GetByCategory vào Services/GiaySvc.cs
```csharp
public List<Giay> GetByCategory(PhanLoai category)
{
    return _context.Giays
        .Where(x => x.PhanLoai == category)
        .OrderByDescending(x => x.Id)
        .ToList();
}
```

### 3. C?p nh?t HomeController.cs
```csharp
public IActionResult UserHome()
{
    // L?y s?n ph?m có phân lo?i "Special Shoe"
    var specialCollection = _giaySvc.GetByCategory(PhanLoai.SpecialShoe);
    
    // L?y t?t c? s?n ph?m
    var allProducts = _giaySvc.GetAll();
    
    ViewBag.SpecialCollection = specialCollection;
    ViewBag.AllProducts = allProducts;
    
    return View();
}
```

## Cách c?p nh?t s?n ph?m thành Special Shoe

### Cách 1: Qua SQL (Nhanh)

1. M? SQL Server Management Studio ho?c Azure Data Studio
2. K?t n?i ð?n database c?a b?n
3. Ch?y script `UpdateSpecialShoeCategory.sql`:

```sql
-- C?p nh?t 3 s?n ph?m ð?u tiên
UPDATE TOP (3) MonAns
SET PhanLoai = 4
WHERE PhanLoai != 4;

-- Ho?c ch?n s?n ph?m c? th?
UPDATE MonAns
SET PhanLoai = 4
WHERE Id IN (1, 2, 3); -- Thay b?ng ID s?n ph?m b?n mu?n
```

### Cách 2: Qua Admin Panel

1. **Restart ?ng d?ng** (Stop ? Run l?i)
2. Ðãng nh?p v?i tài kho?n Admin
3. Vào **Qu?n l? S?n ph?m** (Giày)
4. Ch?n s?n ph?m mu?n ch?nh s?a ? **Edit**
5. ? dropdown **Phân lo?i**, ch?n **"Special Shoe"**
6. Lýu l?i

## Ki?m tra k?t qu?

1. **Restart ?ng d?ng** (quan tr?ng v? ð? thêm enum value m?i)
2. Vào trang Home (ho?c UserHome)
3. Ki?m tra ph?n **SPECIAL SNEAKER COLLECTION**:
   - Ph?i ch? hi?n th? nh?ng s?n ph?m có phân lo?i "Special Shoe"
   - N?u chýa có s?n ph?m nào ? Hi?n th? "Chýa có s?n ph?m ð?c bi?t nào"
4. Ph?n **OUR LATEST PRODUCTS** v?n hi?n th? t?t c? s?n ph?m

## Lýu ? quan tr?ng

?? **PH?I RESTART ?NG D?NG** sau khi thay ð?i enum!

- Vi?c thêm enum value yêu c?u restart ?ng d?ng
- Hot reload không th? áp d?ng cho thay ð?i enum
- Sau khi restart, dropdown trong form Edit s? có option "Special Shoe"

## Ki?m tra trong Database

```sql
-- Xem t?t c? s?n ph?m và phân lo?i
SELECT 
    Id, 
    Ten, 
    CASE PhanLoai
        WHEN 1 THEN 'Giày Da'
        WHEN 2 THEN 'Giày Sneaker'
        WHEN 3 THEN 'Giày Th? Thao'
        WHEN 4 THEN 'Special Shoe'
        ELSE 'Unknown'
    END AS Category,
    Gia 
FROM MonAns
ORDER BY PhanLoai, Id;
```

## Troubleshooting

### V?n ð?: Special Collection không hi?n th? s?n ph?m

**Nguyên nhân:** Chýa có s?n ph?m nào có `PhanLoai = 4` (Special Shoe)

**Gi?i pháp:**
1. Ch?y SQL script ð? c?p nh?t s?n ph?m
2. Ho?c dùng Admin panel ð? edit s?n ph?m

### V?n ð?: Dropdown không có "Special Shoe"

**Nguyên nhân:** Chýa restart ?ng d?ng sau khi thêm enum

**Gi?i pháp:**
1. Stop ?ng d?ng
2. Build l?i (Ctrl + Shift + B)
3. Run l?i (F5)

### V?n ð?: L?i "Cannot convert int to PhanLoai"

**Nguyên nhân:** Database có giá tr? không h?p l?

**Gi?i pháp:**
```sql
-- Ki?m tra giá tr? không h?p l?
SELECT * FROM MonAns WHERE PhanLoai NOT IN (1, 2, 3, 4);

-- C?p nh?t thành giá tr? m?c ð?nh
UPDATE MonAns SET PhanLoai = 2 WHERE PhanLoai NOT IN (1, 2, 3, 4);
```

## T?ng k?t

? Ð? thêm phân lo?i "Special Shoe"
? SPECIAL SNEAKER COLLECTION hi?n th? ðúng s?n ph?m ð?c bi?t
? OUR LATEST PRODUCTS hi?n th? t?t c? s?n ph?m
? Admin có th? ch?n "Special Shoe" khi thêm/s?a s?n ph?m

---
**C?p nh?t:** 2025-02-02
