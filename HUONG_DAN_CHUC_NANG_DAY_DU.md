# ?? HÝ?NG D?N Ð?Y Ð? CÁC CH?C NÃNG
## RESOLE SNEAKER SHOP

---

## ?? M?C L?C
1. [T?ng quan h? th?ng](#1-t?ng-quan-h?-th?ng)
2. [Ch?c nãng phía User](#2-ch?c-nãng-phía-user-khách-hàng)
3. [Ch?c nãng phía Admin](#3-ch?c-nãng-phía-admin-qu?n-tr?-viên)
4. [Ki?n trúc h? th?ng](#4-ki?n-trúc-h?-th?ng)
5. [Cõ s? d? li?u](#5-cõ-s?-d?-li?u)
6. [Các công ngh? s? d?ng](#6-các-công-ngh?-s?-d?ng)
7. [Workflow chi ti?t](#7-workflow-chi-ti?t)
8. [Helpers & Utilities](#8-helpers--utilities)
9. [Layouts & Partials](#9-layouts--partials)
10. [Error Handling](#10-error-handling)
11. [Testing & Debugging](#11-testing--debugging)
12. [Best Practices](#12-best-practices)
13. [Deployment](#13-deployment)
14. [Future Enhancements](#14-future-enhancements)
15. [Troubleshooting](#15-troubleshooting)
16. [Contact & Support](#16-contact--support)
17. [CHI TI?T CODE T?NG D?NG](#17-chi-ti?t-code-t?ng-d?ng)

---

# 1. T?NG QUAN H? TH?NG

## Mô t? d? án
**ReSole Sneaker Shop** là website thýõng m?i ði?n t? bán giày sneaker v?i 2 phân h?:
- **User Site**: Khách hàng xem, mua s?n ph?m
- **Admin Site**: Qu?n tr? viên qu?n l? s?n ph?m, ðõn hàng, ngý?i dùng

## M?c ðích
- Cung c?p tr?i nghi?m mua s?m tr?c tuy?n
- Qu?n l? ðõn hàng hi?u qu?
- Qu?n l? s?n ph?m và kho hàng
- Qu?n l? khách hàng

---

# 2. CH?C NÃNG PHÍA USER (KHÁCH HÀNG)

## 2.1. TRANG CH? (UserHome)

### URL
`/Home/UserHome`

### Mô t?
Trang ch? hi?n th? các s?n ph?m n?i b?t và m?i nh?t

### Các ph?n hi?n th?

#### A. SPECIAL SNEAKER COLLECTION
**M?c ðích:** Hi?n th? các s?n ph?m ð?c bi?t  
**S? lý?ng:** 3 s?n ph?m  
**Ði?u ki?n:** Ch? s?n ph?m có `PhanLoai = SpecialShoe`

**Code logic:**
```csharp
var specialCollection = _giaySvc.GetByCategory(PhanLoai.SpecialShoe);
```

**L? do:** T?o ði?m nh?n cho BST ð?c bi?t, thu hút khách hàng

#### B. OUR LATEST PRODUCTS
**M?c ðích:** Hi?n th? 4 s?n ph?m m?i nh?t  
**S? lý?ng:** 4 s?n ph?m  
**Ði?u ki?n:**
- Lo?i tr? `SpecialShoe`
- S?p x?p theo Id gi?m d?n (m?i nh?t)

**Code logic:**
```csharp
var allProducts = _giaySvc.GetAll()
    .Where(g => g.PhanLoai != PhanLoai.SpecialShoe)
    .OrderByDescending(g => g.Id)
    .Take(4)
    .ToList();
```

**L? do:** Luôn c?p nh?t s?n ph?m m?i nh?t cho khách

#### C. BRAND SHOWCASE (Nike & Adidas)
**M?c ðích:** Gi?i thi?u các thýõng hi?u l?n  
**N?i dung:** Logo và mô t? ng?n v? brand  
**L? do:** Tãng uy tín, th? hi?n ð?i tác

---

## 2.2. DANH SÁCH S?N PH?M (Shop)

### URL
`/ThucDon/Index`

### Mô t?
Hi?n th? t?t c? s?n ph?m d?ng grid

### Tính nãng

#### 1. Hi?n th? thông tin s?n ph?m
- H?nh ?nh (crop t? ð?ng)
- Tên s?n ph?m (gi?i h?n 2 d?ng)
- Giá ti?n
- Phân lo?i (GiaySneaker, GiayDa, GiayTheThao, SpecialShoe)
- Mô t? (gi?i h?n 2 d?ng)
- Tr?ng thái kho (In Stock / Out of Stock)

#### 2. Favorite Icon (Tim yêu thích)
**M?c ðích:** Ðánh d?u s?n ph?m yêu thích  
**Công ngh?:** JavaScript toggle class

**Code:**
```javascript
function toggleFavorite(element) {
    const icon = element.querySelector('i');
    if (icon.classList.contains('bi-heart')) {
        icon.classList.remove('bi-heart');
        icon.classList.add('bi-heart-fill');
    } else {
        icon.classList.remove('bi-heart-fill');
        icon.classList.add('bi-heart');
    }
}
```

#### 3. Hover Effects
- Card n?i lên khi hover
- Zoom ?nh nh?
- Border ð?i màu

### CSS Quan tr?ng
```css
.product-description {
    display: -webkit-box;
    -webkit-line-clamp: 2;
    -webkit-box-orient: vertical;
    overflow: hidden;
    min-height: 40px;
    max-height: 40px;
}
```
**L? do:** Ð?m b?o t?t c? card có chi?u cao ð?ng ð?u

---

## 2.3. CHI TI?T S?N PH?M

### URL
`/ThucDon/Details/{id}`

### Mô t?
Hi?n th? thông tin chi ti?t 1 s?n ph?m

### Tính nãng

#### A. Ch?n Size
**M?c ðích:** Khách ch?n size giày  
**UI:** Radio buttons (39, 40, 41, 42, 43)  
**B?t bu?c:** Ph?i ch?n size m?i thêm ðý?c vào gi?

**Code validation:**
```javascript
if (!selectedSize) {
    alert('Vui l?ng ch?n size');
    return;
}
```

#### B. Add to Cart (Thêm vào gi?)
**Endpoint:** `POST /Cart/Add`

**D? li?u g?i:**
```json
{
    "giayId": 1,
    "size": "42",
    "quantity": 1
}
```

**Logic x? l?:**
1. Ki?m tra ðãng nh?p
2. Ki?m tra s?n ph?m t?n t?i
3. Thêm vào b?ng `Carts`
4. C?p nh?t s? lý?ng gi? hàng

**Code:**
```csharp
var cart = new Cart
{
    KhachHangId = khachHangId,
    GiayId = giay.Id,
    Size = request.Size,
    SoLuong = request.Quantity
};
_cartSvc.Add(cart);
```

#### C. Buy Now (Mua ngay)
**Endpoint:** `POST /Checkout/BuyNow`

**Khác bi?t v?i Add to Cart:**
- Không lýu vào gi? hàng
- Chuy?n th?ng ð?n trang thanh toán
- S? d?ng Session ð? lýu t?m

**D? li?u Session:**
```csharp
var buyNowItem = new BuyNowItem
{
    GiayId = request.GiayId,
    Ten = giay.Ten,
    Hinh = giay.Hinh,
    Gia = (decimal)giay.Gia,
    Size = request.Size,
    SoLuong = request.Quantity
};
HttpContext.Session.SetObjectAsJson("BuyNowItem", buyNowItem);
```

#### D. Thông tin hi?n th?
- H?nh ?nh l?n
- Tên ð?y ð?
- Giá chi ti?t
- Mô t? ð?y ð? (không gi?i h?n)
- Phân lo?i
- Tr?ng thái c?n hàng

---

## 2.4. GI? HÀNG

### URL
`/Cart/Index`

### Mô t?
Qu?n l? s?n ph?m trong gi? hàng

### Tính nãng

#### A. Hi?n th? danh sách
**D? li?u:** L?y t? b?ng `Carts` theo `KhachHangId`

**Code:**
```csharp
var carts = _cartSvc.GetCartByKhachHangId(khachHangId);
var cartItems = carts.Select(c => {
    var giay = c.Giay ?? _giaySvc.Get(c.GiayId);
    return new CartItem
    {
        GiayId = c.GiayId,
        Ten = giay?.Ten,
        Hinh = giay?.Hinh,
        Gia = (decimal)(giay?.Gia ?? 0),
        SoLuong = c.SoLuong,
        Size = c.Size
    };
}).ToList();
```

#### B. Update Quantity (C?p nh?t s? lý?ng)
**Endpoint:** `POST /Cart/UpdateQuantity`  
**UI:** Input number v?i nút +/-

**Logic:**
- N?u quantity > 0: Update
- N?u quantity = 0: Xóa kh?i gi?

**Code:**
```csharp
if (request.Quantity > 0)
{
    cart.SoLuong = request.Quantity;
    _cartSvc.Update(cart);
}
else
{
    _cartSvc.DeleteByKhachHangIdAndGiayId(khachHangId, request.GiayId, request.Size);
}
```

#### C. Remove (Xóa s?n ph?m)
**Endpoint:** `POST /Cart/Remove`  
**UI:** Nút "Remove" v?i icon trash  
**Xác nh?n:** Có popup confirm

**Code:**
```csharp
_cartSvc.DeleteByKhachHangIdAndGiayId(khachHangId, request.GiayId, request.Size);
```

#### D. Clear Cart (Xóa toàn b?)
**Endpoint:** `POST /Cart/Clear`  
**UI:** Nút "Clear All"

**Code:**
```csharp
_cartSvc.ClearCart(khachHangId);
```

#### E. Tính t?ng ti?n
**Realtime:** T? ð?ng tính khi thay ð?i s? lý?ng

**Code:**
```javascript
let total = 0;
document.querySelectorAll('.cart-item').forEach(item => {
    const price = parseFloat(item.dataset.price);
    const quantity = parseInt(item.querySelector('.quantity').value);
    total += price * quantity;
});
document.querySelector('.cart-total').textContent = total.toLocaleString() + ' ð';
```

#### F. Cart Badge (S? lý?ng s?n ph?m)
**V? trí:** Icon gi? hàng trên navbar  
**Update:** M?i khi thêm/xóa/c?p nh?t  
**Endpoint:** `GET /Cart/GetCount`

**Code:**
```csharp
var cartCount = _cartSvc.GetCartCount(khachHangId);
return Json(new { cartCount = cartCount });
```

---

## 2.5. THANH TOÁN (Checkout)

### URL
`/Checkout/Index`

### Mô t?
Trang xác nh?n và ð?t hàng

### Tính nãng

#### A. Hi?n th? thông tin

**1. Thông tin khách hàng (auto-fill t? session):**
- H? tên
- Email
- S? ði?n tho?i
- Ð?a ch?

**2. Danh sách s?n ph?m:**
- T? gi? hàng (n?u checkout t? cart)
- T? Buy Now (n?u mua ngay)

**3. T?ng ti?n:** Tính t? ð?ng

#### B. X? l? ðõn hàng
**Endpoint:** `POST /Checkout/PlaceOrder`

**Flow:**
1. Validate thông tin
2. T?o `Donhang` m?i
3. T?o `DonhangChitiet` cho t?ng s?n ph?m
4. Xóa gi? hàng (n?u checkout t? cart)
5. Xóa BuyNow session (n?u mua ngay)
6. Redirect ð?n Success page

**Code:**
```csharp
// T?o ðõn hàng
var donhang = new Donhang
{
    KhachHangId = khachHangId,
    NgayDat = DateTime.Now,
    TongTien = totalAmount,
    TrangThai = "Pending"
};
_donhangSvc.Add(donhang);

// T?o chi ti?t ðõn hàng
foreach (var item in cartItems)
{
    var chitiet = new DonhangChitiet
    {
        DonhangId = donhang.Id,
        GiayId = item.GiayId,
        Size = item.Size,
        SoLuong = item.SoLuong,
        DonGia = item.Gia
    };
    _donhangChitietSvc.Add(chitiet);
}

// Xóa gi? hàng
_cartSvc.ClearCart(khachHangId);
```

#### C. Success Page
**URL:** `/Checkout/Success`

**Hi?n th?:**
- Thông báo thành công
- M? ðõn hàng
- Link v? trang ch?
- Link xem ðõn hàng

---

## 2.6. QU?N L? ÐÕN HÀNG (User)

### URL
`/Donhang/Index`

### Mô t?
Khách hàng xem danh sách ðõn hàng c?a m?nh

### Tính nãng

#### A. Danh sách ðõn hàng
**Filter:** Ch? hi?n ðõn c?a khách ðang ðãng nh?p

**Code:**
```csharp
var khachHangId = int.Parse(HttpContext.Session.GetString(SessionKey.KhachHang.KH_Id));
var donhangs = _donhangSvc.GetByKhachHangId(khachHangId);
```

#### B. Tr?ng thái ðõn hàng
- **Pending** - Ch? x? l?
- **Processing** - Ðang x? l?
- **Shipping** - Ðang giao
- **Completed** - Hoàn thành
- **Cancelled** - Ð? h?y

#### C. Chi ti?t ðõn hàng
**URL:** `/Donhang/Details/{id}`

**Hi?n th?:**
- Thông tin ðõn hàng
- Danh sách s?n ph?m
- T?ng ti?n
- Tr?ng thái
- L?ch s? thay ð?i

---

## 2.7. TÀI KHO?N KHÁCH HÀNG

### A. Ðãng k? (Register)
**URL:** `/Khachhang/Register`

**Fields:**
- H? tên (required)
- Email (required, unique)
- S? ði?n tho?i (required)
- Ð?a ch? (optional)
- M?t kh?u (required, min 6 k? t?)
- Xác nh?n m?t kh?u

**Code x? l?:**
```csharp
var khachhang = new Khachhang
{
    HoTen = model.HoTen,
    Email = model.Email,
    DienThoai = model.DienThoai,
    DiaChi = model.DiaChi,
    MatKhau = MahoaHelper.MaHoaMD5(model.MatKhau)
};
_khachhangSvc.Add(khachhang);
```

### B. Ðãng nh?p (Login)
**URL:** `/Khachhang/Login`  
**Validation:** Email + m?t kh?u

**Code:**
```csharp
var khachhang = _khachhangSvc.GetByEmail(model.Email);
if (khachhang != null && khachhang.MatKhau == MahoaHelper.MaHoaMD5(model.MatKhau))
{
    HttpContext.Session.SetString(SessionKey.KhachHang.KH_Id, khachhang.Id.ToString());
    HttpContext.Session.SetString(SessionKey.KhachHang.KH_Email, khachHang.Email);
    HttpContext.Session.SetString(SessionKey.KhachHang.KH_HoTen, khachHang.HoTen);
}
```

### C. Profile (Thông tin cá nhân)
**URL:** `/Khachhang/Profile`

**Tính nãng:**
- Xem thông tin
- C?p nh?t thông tin
- Ð?i m?t kh?u

**Code update:**
```csharp
var khachhang = _khachhangSvc.Get(khachHangId);
khachhang.HoTen = model.HoTen;
khachhang.DienThoai = model.DienThoai;
khachhang.DiaChi = model.DiaChi;
_khachhangSvc.Update(khachHang);
```

### D. Ð?i m?t kh?u

**Validation:**
- M?t kh?u c? ðúng
- M?t kh?u m?i khác m?t kh?u c?
- Xác nh?n m?t kh?u kh?p

**Code:**
```csharp
if (khachhang.MatKhau != MahoaHelper.MaHoaMD5(model.OldPassword))
{
    ModelState.AddModelError("", "M?t kh?u c? không ðúng");
    return View(model);
}

khachhang.MatKhau = MahoaHelper.MaHoaMD5(model.NewPassword);
_khachhangSvc.Update(khachHang);
```

---

# 3. CH?C NÃNG PHÍA ADMIN (QU?N TR? VIÊN)

## 3.1. ÐÃNG NH?P ADMIN

### URL
`/Admin/Login`

### Mô t?
Ðãng nh?p cho qu?n tr? viên

### Tính nãng
**Validation:** Username + Password  
**B?ng:** `Nguoidungs`

**Code:**
```csharp
var nguoidung = _nguoidungSvc.GetByUsername(model.Username);
if (nguoidung != null && nguoidung.Password == MahoaHelper.MaHoaMD5(model.Password))
{
    HttpContext.Session.SetString(SessionKey.NguoiDung.Username, nguoidung.Username);
    HttpContext.Session.SetString(SessionKey.NguoiDung.HoTen, nguoidung.HoTen);
    return RedirectToAction("Index", "Admin");
}
```

### Authentication Filter
**File:** `AuthenticationFilterAttribute.cs`  
**M?c ðích:** B?o v? các trang admin

**Code:**
```csharp
public class AuthenticationFilterAttribute : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        var username = context.HttpContext.Session.GetString(SessionKey.NguoiDung.Username);
        if (string.IsNullOrEmpty(username))
        {
            context.Result = new RedirectToActionResult("Login", "Admin", null);
        }
    }
}
```

---

## 3.2. QU?N L? S?N PH?M (GIÀY)

### A. Danh sách s?n ph?m
**URL:** `/Giay/Index`  
**Hi?n th?:** T?t c? s?n ph?m trong b?ng

**Features:**
- H?nh ?nh thumbnail (80x80px, crop)
- Tên, giá, phân lo?i, tr?ng thái
- Mô t? (truncate v?i hover)
- Actions: Chi ti?t, S?a, Xóa

**CSS cho Description:**
```css
.description-cell {
    max-width: 250px;
    white-space: nowrap;
    overflow: hidden;
    text-overflow: ellipsis;
}

.description-cell:hover {
    white-space: normal;
    overflow: visible;
    background: white;
    box-shadow: 0 4px 12px rgba(0,0,0,0.15);
}
```

### B. Thêm s?n ph?m
**URL:** `/Giay/Create`

**Form fields:**
- Tên giày (required, max 100)
- Mô t? (optional, max 500)
- Giá (required, 0-1,000,000)
- Phân lo?i (dropdown: GiayDa, GiaySneaker, GiayTheThao, SpecialShoe)
- H?nh ?nh (upload file)
- Tr?ng thái (checkbox: Ðang ph?c v?)

**Upload ?nh:**
```csharp
if (giay.ImageFile != null)
{
    giay.Hinh = await UploadHelper.UploadImage(giay.ImageFile, "Giay");
}
```

**UploadHelper.cs:**
```csharp
public static async Task<string> UploadImage(IFormFile file, string folder)
{
    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
    var path = Path.Combine("wwwroot/images", folder, fileName);
    
    Directory.CreateDirectory(Path.GetDirectoryName(path));
    
    using (var stream = new FileStream(path, FileMode.Create))
    {
        await file.CopyToAsync(stream);
    }
    
    return fileName;
}
```

**L? do:**
- GUID ð?m b?o tên file unique
- Tránh conflict khi upload nhi?u ?nh cùng tên

### C. S?a s?n ph?m
**URL:** `/Giay/Edit/{id}`

**Logic:**
- Load d? li?u hi?n t?i
- Cho phép update
- Gi? ?nh c? n?u không upload ?nh m?i

**Code:**
```csharp
var existingGiay = _giaySvc.Get(id);

if (giay.ImageFile != null)
{
    // Delete old image
    if (!string.IsNullOrEmpty(existingGiay.Hinh))
    {
        var oldPath = Path.Combine("wwwroot/images/Giay", existingGiay.Hinh);
        if (System.IO.File.Exists(oldPath))
        {
            System.IO.File.Delete(oldPath);
        }
    }
    
    // Upload new image
    giay.Hinh = await UploadHelper.UploadImage(giay.ImageFile, "Giay");
}
else
{
    giay.Hinh = existingGiay.Hinh;
}

_giaySvc.Update(giay);
```

### D. Xóa s?n ph?m
**URL:** `POST /Giay/Delete/{id}`  
**Confirm:** Popup JavaScript

**Logic:**
- Xóa ?nh kh?i server
- Xóa record kh?i DB

**Code:**
```csharp
var giay = _giaySvc.Get(id);

if (!string.IsNullOrEmpty(giay.Hinh))
{
    var imagePath = Path.Combine("wwwroot/images/Giay", giay.Hinh);
    if (System.IO.File.Exists(imagePath))
    {
        System.IO.File.Delete(imagePath);
    }
}

_giaySvc.Delete(id);
```

### E. Chi ti?t s?n ph?m
**URL:** `/Giay/Details/{id}`  
**Hi?n th?:** T?t c? thông tin chi ti?t  
**Read-only:** Không cho phép ch?nh s?a

---

## 3.3. QU?N L? ÐÕN HÀNG (ADMIN)

### A. Danh sách ðõn hàng
**URL:** `/Donhang/Index` (v?i filter Admin)  
**Hi?n th?:** T?t c? ðõn hàng c?a t?t c? khách

**Features:**
- Filter theo tr?ng thái
- Search theo m? ðõn, tên khách
- S?p x?p theo ngày

### B. Chi ti?t ðõn hàng
**URL:** `/Donhang/Details/{id}`

**Hi?n th?:**
- Thông tin khách hàng
- Danh sách s?n ph?m (partial view `_DonhangChitiet.cshtml`)
- T?ng ti?n
- Tr?ng thái hi?n t?i
- L?ch s? thay ð?i

**Partial View:**
```cshtml
@model List<DonhangChitiet>

<table class="table">
    <thead>
        <tr>
            <th>S?n ph?m</th>
            <th>Size</th>
            <th>S? lý?ng</th>
            <th>Ðõn giá</th>
            <th>Thành ti?n</th>
        </tr>
    </thead>
    <tbody>
        @foreach (var item in Model)
        {
            <tr>
                <td>@item.Giay.Ten</td>
                <td>@item.Size</td>
                <td>@item.SoLuong</td>
                <td>@item.DonGia.ToString("N0") ð</td>
                <td>@((item.DonGia * item.SoLuong).ToString("N0")) ð</td>
            </tr>
        }
    </tbody>
</table>
```

### C. C?p nh?t tr?ng thái
**URL:** `/Donhang/Edit/{id}`  
**UI:** Dropdown select tr?ng thái

**Code:**
```csharp
var donhang = _donhangSvc.Get(id);
donhang.TrangThai = newStatus;
donhang.NgayCapNhat = DateTime.Now;
_donhangSvc.Update(donhang);
```

---

## 3.4. QU?N L? NGÝ?I DÙNG (ADMIN)

### A. Danh sách ngý?i dùng
**URL:** `/NguoiDung/Index`  
**Hi?n th?:** T?t c? admin users

**Features:**
- T?m ki?m theo username, h? tên
- Filter theo tr?ng thái active

### B. Thêm ngý?i dùng
**URL:** `/NguoiDung/Create`

**Fields:**
- Username (unique)
- H? tên
- Email
- Password (m? hóa MD5)

**Code:**
```csharp
var nguoidung = new Nguoidung
{
    Username = model.Username,
    Password = MahoaHelper.MaHoaMD5(model.Password),
    HoTen = model.HoTen,
    Email = model.Email
};
_nguoidungSvc.Add(nguoidung);
```

### C. S?a ngý?i dùng
**URL:** `/NguoiDung/Update/{id}`  
**Note:** Không cho ð?i username  
**Password:** Ch? ð?i khi nh?p m?t kh?u m?i

### D. Xóa ngý?i dùng
**URL:** `POST /NguoiDung/Delete/{id}`  
**Validation:** Không cho xóa chính m?nh

---

## 3.5. QU?N L? KHÁCH HÀNG (ADMIN)

### A. Danh sách khách hàng
**URL:** `/Khachhang/Index`  
**Hi?n th?:** T?t c? khách hàng ð? ðãng k?

**Features:**
- Xem l?ch s? ðõn hàng
- Xem thông tin chi ti?t
- Khóa/M? khóa tài kho?n

### B. Chi ti?t khách hàng
**URL:** `/Khachhang/Details/{id}`

**Hi?n th?:**
- Thông tin cá nhân
- T?ng s? ðõn hàng
- T?ng giá tr? mua
- Danh sách ðõn hàng g?n ðây

---

# 4. KI?N TRÚC H? TH?NG

## 4.1. KI?N TRÚC MVC

```
???????????????????????????????????????????????
?                  BROWSER                     ?
???????????????????????????????????????????????
              ?
              ?
???????????????????????????????????????????????
?              CONTROLLERS                     ?
?  • HomeController                            ?
?  • GiayController                            ?
?  • CartController                            ?
?  • CheckoutController                        ?
?  • DonhangController                         ?
?  • KhachhangController                       ?
?  • NguoidungController                       ?
?  • AdminController                           ?
???????????????????????????????????????????????
              ?
              ?
???????????????????????????????????????????????
?               SERVICES                       ?
?  • GiaySvc (Business Logic)                 ?
?  • CartSvc                                   ?
?  • DonhangSvc                                ?
?  • DonhangChitietSvc                         ?
?  • KhachhangSvc                              ?
?  • NguoidungSvc                              ?
???????????????????????????????????????????????
              ?
              ?
???????????????????????????????????????????????
?            DATA CONTEXT                      ?
?         (Entity Framework)                   ?
???????????????????????????????????????????????
              ?
              ?
???????????????????????????????????????????????
?            SQL SERVER DATABASE               ?
???????????????????????????????????????????????
```

## 4.2. C?U TRÚC THÝ M?C

```
ASM/
??? Controllers/          # X? l? HTTP requests
??? Models/              # Entity classes, ViewModels
?   ??? ViewModels/      # DTOs cho Views
?   ??? DataContext.cs   # EF Core DbContext
??? Services/            # Business logic layer
??? Views/               # Razor Views
?   ??? Shared/          # Layouts, partials
?   ??? Home/
?   ??? Giay/
?   ??? Cart/
?   ??? Checkout/
?   ??? Donhang/
?   ??? Khachhang/
?   ??? NguoiDung/
??? wwwroot/             # Static files
?   ??? css/
?   ??? js/
?   ??? images/
??? Helpers/             # Utility classes
??? Filters/             # Custom filters
??? Constants/           # Constants, Enums
??? Migrations/          # EF Core migrations
```

---

# 5. CÕ S? D? LI?U

## 5.1. SÕ Ð? DATABASE

```
???????????????????       ???????????????????
?   Khachhangs    ?       ?   Nguoidungs    ?
???????????????????       ???????????????????
? Id (PK)         ?       ? Id (PK)         ?
? HoTen           ?       ? Username        ?
? Email (unique)  ?       ? Password        ?
? DienThoai       ?       ? HoTen           ?
? DiaChi          ?       ? Email           ?
? MatKhau         ?       ???????????????????
???????????????????
         ?
         ? 1:N
         ?
???????????????????       ???????????????????
?    Donhangs     ?       ?     Carts       ?
???????????????????       ???????????????????
? Id (PK)         ?????   ? Id (PK)         ?
? KhachHangId(FK) ?   ?   ? KhachHangId(FK) ?
? NgayDat         ?   ?   ? GiayId (FK)     ?
? TongTien        ?   ?   ? Size            ?
? TrangThai       ?   ?   ? SoLuong         ?
? NgayCapNhat     ?   ?   ???????????????????
???????????????????   ?            ?
         ?            ?            ? N:1
         ? 1:N        ?            ?
         ?            ?   ???????????????????
???????????????????   ?   ?     MonAns      ?
?DonhangChitiets  ?   ?   ?    (Giay)       ?
???????????????????   ?   ???????????????????
? Id (PK)         ?   ?   ? Id (PK)         ?
? DonhangId (FK)  ?????   ? Ten             ?
? GiayId (FK)     ????????? MoTa            ?
? Size            ?       ? Gia             ?
? SoLuong         ?       ? PhanLoai        ?
? DonGia          ?       ? Hinh            ?
???????????????????       ? TrangThai       ?
                          ???????????????????
```

## 5.2. CHI TI?T CÁC B?NG

### Khachhangs (Khách hàng)
```sql
CREATE TABLE Khachhangs (
    Id INT PRIMARY KEY IDENTITY(1,1),
    HoTen NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    DienThoai NVARCHAR(20),
    DiaChi NVARCHAR(200),
    MatKhau NVARCHAR(255) NOT NULL
)
```

### Nguoidungs (Admin)
```sql
CREATE TABLE Nguoidungs (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Username NVARCHAR(50) NOT NULL UNIQUE,
    Password NVARCHAR(255) NOT NULL,
    HoTen NVARCHAR(100),
    Email NVARCHAR(100)
)
```

### MonAns (Giày)
```sql
CREATE TABLE MonAns (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Ten NVARCHAR(100) NOT NULL,
    MoTa NVARCHAR(500),
    Gia FLOAT NOT NULL,
    PhanLoai INT NOT NULL,  -- 1: GiayDa, 2: GiaySneaker, 3: GiayTheThao, 4: SpecialShoe
    Hinh NVARCHAR(200),
    TrangThai BIT NOT NULL DEFAULT 1
)
```

### Carts (Gi? hàng)
```sql
CREATE TABLE Carts (
    Id INT PRIMARY KEY IDENTITY(1,1),
    KhachHangId INT NOT NULL,
    GiayId INT NOT NULL,
    Size NVARCHAR(10) NOT NULL,
    SoLuong INT NOT NULL DEFAULT 1,
    FOREIGN KEY (KhachHangId) REFERENCES Khachhangs(Id),
    FOREIGN KEY (GiayId) REFERENCES MonAns(Id)
)
```

### Donhangs (Ðõn hàng)
```sql
CREATE TABLE Donhangs (
    Id INT PRIMARY KEY IDENTITY(1,1),
    KhachHangId INT NOT NULL,
    NgayDat DATETIME NOT NULL DEFAULT GETDATE(),
    TongTien DECIMAL(18,2) NOT NULL,
    TrangThai NVARCHAR(50) NOT NULL DEFAULT 'Pending',
    NgayCapNhat DATETIME,
    FOREIGN KEY (KhachHangId) REFERENCES Khachhangs(Id)
)
```

### DonhangChitiets (Chi ti?t ðõn hàng)
```sql
CREATE TABLE DonhangChitiets (
    Id INT PRIMARY KEY IDENTITY(1,1),
    DonhangId INT NOT NULL,
    GiayId INT NOT NULL,
    Size NVARCHAR(10) NOT NULL,
    SoLuong INT NOT NULL,
    DonGia DECIMAL(18,2) NOT NULL,
    FOREIGN KEY (DonhangId) REFERENCES Donhangs(Id),
    FOREIGN KEY (GiayId) REFERENCES MonAns(Id)
)
```

---

# 6. CÁC CÔNG NGH? S? D?NG

## 6.1. BACKEND

#### **ASP.NET Core 8.0**
- Framework chính
- MVC pattern
- Dependency Injection

#### **Entity Framework Core**
- ORM cho database
- Code-First approach
- Migrations

**DataContext.cs:**
```csharp
public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }

    public DbSet<Giay> MonAns { get; set; }
    public DbSet<Khachhang> Khachhangs { get; set; }
    public DbSet<Nguoidung> Nguoidungs { get; set; }
    public DbSet<Donhang> Donhangs { get; set; }
    public DbSet<DonhangChitiet> DonhangChitiets { get; set; }
    public DbSet<Cart> Carts { get; set; }
}
```

#### **SQL Server**
- Database engine
- LocalDB cho development
- Connection String trong `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=ASM_DB;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
```

## 6.2. FRONTEND

#### **Bootstrap 5**
- Responsive layout
- UI components
- Grid system

#### **Bootstrap Icons**
- Icon library
- S? d?ng trong toàn b? UI

**Ví d?:**
```html
<i class="bi bi-cart"></i>        <!-- Gi? hàng -->
<i class="bi bi-heart"></i>       <!-- Yêu thích -->
<i class="bi bi-eye"></i>         <!-- Xem chi ti?t -->
<i class="bi bi-pencil"></i>      <!-- S?a -->
<i class="bi bi-trash"></i>       <!-- Xóa -->
```

#### **Custom CSS**
- `site.css`: Global styles
- `login.css`: Login pages
- `_WebLayout.cshtml.css`: User site
- Inline styles trong Views

#### **JavaScript/jQuery**
- AJAX requests
- Dynamic UI updates
- Form validation
- Cart functionality

**Ví d? AJAX:**
```javascript
function addToCart(giayId, size, quantity) {
    $.ajax({
        url: '/Cart/Add',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify({
            giayId: giayId,
            size: size,
            quantity: quantity
        }),
        success: function(response) {
            if (response.success) {
                updateCartBadge(response.cartCount);
                alert('Ð? thêm vào gi? hàng');
            }
        },
        error: function() {
            alert('Có l?i x?y ra');
        }
    });
}
```

## 6.3. B?O M?T

#### **MD5 Hashing (MahoaHelper.cs)**
```csharp
public static class MahoaHelper
{
    public static string MaHoaMD5(string input)
    {
        using (MD5 md5 = MD5.Create())
        {
            byte[] inputBytes = Encoding.ASCII.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);
            
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("X2"));
            }
            return sb.ToString();
        }
    }
}
```
- **L? do:** M? hóa m?t kh?u trý?c khi lýu vào DB
- **Note:** MD5 không an toàn tuy?t ð?i, nên dùng bcrypt/PBKDF2 cho production

#### **Session Management**
- Lýu thông tin ðãng nh?p
- Timeout: 30 phút
- Cookie-based

**Program.cs:**
```csharp
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
```

#### **Authentication Filter**
- B?o v? admin routes
- Ki?m tra session
- Redirect n?u chýa login

## 6.4. DEPENDENCY INJECTION

**Program.cs:**
```csharp
// Ðãng k? services
builder.Services.AddScoped<IGiaySvc, GiaySvc>();
builder.Services.AddScoped<ICartSvc, CartSvc>();
builder.Services.AddScoped<IDonhangSvc, DonhangSvc>();
builder.Services.AddScoped<IDonhangChitietSvc, DonhangChitietSvc>();
builder.Services.AddScoped<IKhachhangSvc, KhachhangSvc>();
builder.Services.AddScoped<INguoidungSvc, NguoidungSvc>();
```

**L?i ích:**
- Loose coupling
- D? test
- D? maintain
- Lifecycle management

---

# 7. WORKFLOW CHI TI?T

## 7.1. FLOW MUA HÀNG (ADD TO CART)

```
1. User ? trang Details
   ?
2. Ch?n size
   ?
3. Nh?n "Add to Cart"
   ?
4. JavaScript g?i AJAX POST request
   {
     giayId: 1,
     size: "42",
     quantity: 1
   }
   ?
5. CartController.Add() x? l?:
   - Ki?m tra login
   - Ki?m tra s?n ph?m t?n t?i
   - Thêm vào b?ng Carts
   - Return JSON {success: true, cartCount: 3}
   ?
6. JavaScript nh?n response:
   - Update cart badge (s? 3)
   - Hi?n th? thông báo
   - Không reload page
```

## 7.2. FLOW THANH TOÁN (CHECKOUT)

```
1. User ? trang Cart
   ?
2. Nh?n "Checkout"
   ?
3. Redirect ð?n /Checkout/Index
   ?
4. CheckoutController.Index():
   - Load thông tin khách hàng t? session
   - Load gi? hàng t? DB
   - Tính t?ng ti?n
   - Hi?n th? form xác nh?n
   ?
5. User nh?n "Place Order"
   ?
6. POST /Checkout/PlaceOrder:
   - Validate d? li?u
   - T?o record Donhang
   - T?o records DonhangChitiet
   - Xóa gi? hàng
   - Commit transaction
   ?
7. Redirect ð?n /Checkout/Success
   - Hi?n th? m? ðõn hàng
   - Link v? trang ch?
```

## 7.3. FLOW MUA NGAY (BUY NOW)

```
1. User ? trang Details
   ?
2. Ch?n size
   ?
3. Nh?n "Buy Now"
   ?
4. POST /Checkout/BuyNow:
   - T?o BuyNowItem
   - Lýu vào Session (không lýu DB)
   - Return JSON
   ?
5. JavaScript redirect ð?n /Checkout/Index
   ?
6. CheckoutController.Index():
   - Ki?m tra có BuyNowItem trong session không
   - N?u có: dùng BuyNowItem
   - N?u không: dùng Cart
   ?
7. User xác nh?n và ð?t hàng
   ?
8. PlaceOrder():
   - T?o ðõn hàng
   - Xóa BuyNowItem kh?i session
   - KHÔNG xóa gi? hàng
```

**L? do có Buy Now:**
- Mua nhanh không c?n gi? hàng
- Không ?nh hý?ng ð?n gi? hàng hi?n t?i
- Tr?i nghi?m mua s?m linh ho?t

---

# 8. HELPERS & UTILITIES

## 8.1. MahoaHelper.cs
```csharp
public static class MahoaHelper
{
    public static string MaHoaMD5(string input)
    {
        // M? hóa MD5 cho password
    }
}
```

## 8.2. UploadHelper.cs
```csharp
public static class UploadHelper
{
    public static async Task<string> UploadImage(IFormFile file, string folder)
    {
        // Upload ?nh vào wwwroot/images/{folder}
        // Return tên file ð? lýu
    }
}
```

## 8.3. CartHelper.cs
```csharp
public static class CartHelper
{
    public static void SetObjectAsJson(this ISession session, string key, object value)
    {
        session.SetString(key, JsonSerializer.Serialize(value));
    }

    public static T GetObjectFromJson<T>(this ISession session, string key)
    {
        var value = session.GetString(key);
        return value == null ? default : JsonSerializer.Deserialize<T>(value);
    }
}
```
- **M?c ðích:** Lýu object vào session (cho BuyNow)

## 8.4. SessionKey.cs
```csharp
public static class SessionKey
{
    public static class KhachHang
    {
        public const string KH_Id = "KH_Id";
        public const string KH_Email = "KH_Email";
        public const string KH_HoTen = "KH_HoTen";
    }

    public static class NguoiDung
    {
        public const string Username = "ND_Username";
        public const string HoTen = "ND_HoTen";
    }
}
```
- **L?i ích:** T?p trung qu?n l? session keys, tránh sai sót

---

# 9. LAYOUTS & PARTIALS

## 9.1. _Layout.cshtml (Admin)
- Navbar v?i logo, menu
- Sidebar (optional)
- Footer
- Script references

## 9.2. _WebLayout.cshtml (User)
- Sticky navbar
- Cart badge
- User menu (Login/Profile/Logout)
- Footer v?i social links

## 9.3. _LoginLayout.cshtml
- Minimal layout cho login pages
- Center aligned form
- Background image/gradient

## 9.4. _LoginPartial.cshtml
```cshtml
@if (Context.Session.GetString("KH_Email") != null)
{
    <li class="nav-item dropdown">
        <a class="nav-link dropdown-toggle" href="#" data-bs-toggle="dropdown">
            @Context.Session.GetString("KH_HoTen")
        </a>
        <ul class="dropdown-menu">
            <li><a class="dropdown-item" asp-controller="Khachhang" asp-action="Profile">Profile</a></li>
            <li><a class="dropdown-item" asp-controller="Donhang" asp-action="Index">Ðõn hàng</a></li>
            <li><hr class="dropdown-divider"></li>
            <li><a class="dropdown-item" asp-controller="Khachhang" asp-action="Logout">Logout</a></li>
        </ul>
    </li>
}
else
{
    <li class="nav-item">
        <a class="nav-link" asp-controller="Khachhang" asp-action="Login">Login</a>
    </li>
    <li class="nav-item">
        <a class="nav-link" asp-controller="Khachhang" asp-action="Register">Register</a>
    </li>
}
```

## 9.5. _MenuPartial.cshtml
- Admin sidebar menu
- Conditional rendering based on permissions

## 9.6. _ValidationScriptsPartial.cshtml
```cshtml
<script src="~/lib/jquery-validation/dist/jquery.validate.min.js"></script>
<script src="~/lib/jquery-validation-unobtrusive/jquery.validate.unobtrusive.min.js"></script>
```

---

# 10. ERROR HANDLING

## 10.1. Global Error Page
**Views/Shared/Error.cshtml**
```cshtml
@model ErrorViewModel

<h1 class="text-danger">Error</h1>
<h2 class="text-danger">An error occurred while processing your request.</h2>

@if (Model.ShowRequestId)
{
    <p>
        <strong>Request ID:</strong> <code>@Model.RequestId</code>
    </p>
}
```

## 10.2. Error Logging
```csharp
_logger.LogError(ex, "Error adding to cart");
```

## 10.3. 404 Not Found
- Custom 404 page
- Configured trong Program.cs:
```csharp
app.UseStatusCodePagesWithReExecute("/Home/Error", "?statusCode={0}");
```

---

# 11. TESTING & DEBUGGING

## 11.1. Test Files
- `debug-buynow.html`: Test Buy Now functionality
- `test-buynow.html`: Additional tests
- `TEST_BUY_NOW.md`: Documentation for testing

## 11.2. Debugging Tips
1. **Check Session**: Dùng browser dev tools > Application > Cookies
2. **Check Database**: Dùng SQL Server Management Studio
3. **Check Logs**: Console output trong Visual Studio
4. **Network Tab**: Xem AJAX requests/responses

---

# 12. BEST PRACTICES

## 12.1. CODE ORGANIZATION
- Controllers: Thin, ch? handle HTTP
- Services: Business logic
- Models: Data structure
- Helpers: Utilities

## 12.2. NAMING CONVENTIONS
- Controllers: `{Entity}Controller.cs`
- Views: `{Action}.cshtml`
- Services: `{Entity}Svc.cs`
- Models: `{Entity}.cs`

## 12.3. SECURITY
- Validate user input
- Use parameterized queries (EF Core)
- Hash passwords
- Use HTTPS in production
- Implement CSRF protection

## 12.4. PERFORMANCE
- Use async/await
- Lazy loading cho images
- Pagination cho danh sách l?n
- Cache static content

---

# 13. DEPLOYMENT

## 13.1. Build Production
```bash
dotnet publish -c Release -o ./publish
```

## 13.2. Database Migration
```bash
dotnet ef database update
```

## 13.3. Hosting Options
- Azure App Service
- IIS
- Docker container

## 13.4. appsettings.Production.json
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=production-server;Database=ASM_DB;..."
  },
  "Logging": {
    "LogLevel": {
      "Default": "Warning"
    }
  }
}
```

---

# 14. FUTURE ENHANCEMENTS

## 14.1. Tính nãng có th? thêm
1. **Payment Integration**: PayPal, Stripe, VNPay
2. **Email Notifications**: Xác nh?n ðõn hàng
3. **Product Reviews**: Ðánh giá s?n ph?m
4. **Wishlist**: Danh sách yêu thích
5. **Inventory Management**: Qu?n l? t?n kho
6. **Discount Codes**: M? gi?m giá
7. **Order Tracking**: Theo d?i ðõn hàng
8. **Advanced Search**: T?m ki?m nâng cao
9. **Product Filters**: L?c theo giá, brand, size
10. **Admin Dashboard**: Th?ng kê, báo cáo

## 14.2. Technical Improvements
1. **Unit Tests**: Vi?t test cases
2. **API Documentation**: Swagger
3. **Caching**: Redis
4. **CDN**: Cho static files
5. **Logging**: Serilog
6. **Authentication**: Identity Framework
7. **Real-time**: SignalR cho notifications

---

# 15. TROUBLESHOOTING

## 15.1. L?i thý?ng g?p

**A. Session không lýu**
```csharp
// Ð?m b?o trong Program.cs có:
app.UseSession();
// Ph?i ð?t TRÝ?C app.UseRouting()
```

**B. ?nh không hi?n th?**
```cshtml
<!-- Ki?m tra ðý?ng d?n -->
<img src="~/images/Giay/@Model.Hinh" />
<!-- Ð?m b?o file t?n t?i trong wwwroot/images/Giay -->
```

**C. Migration l?i**
```bash
# Xóa migration c?
dotnet ef migrations remove

# T?o l?i
dotnet ef migrations add InitialCreate

# C?p nh?t database
dotnet ef database update
```

**D. AJAX không ho?t ð?ng**
```javascript
// Ki?m tra Content-Type
contentType: 'application/json',

// Ki?m tra d? li?u g?i
data: JSON.stringify({ ... })
```

---

# 16. CONTACT & SUPPORT

## Developer
- **Name**: Tr?nh Thiên Ân
- **Student ID**: 422210281
- **GitHub**: https://github.com/Tin0907/ReSole

## Resources
- **ASP.NET Core Docs**: https://docs.microsoft.com/aspnet/core
- **Bootstrap Docs**: https://getbootstrap.com/docs/5.0
- **Entity Framework**: https://docs.microsoft.com/ef/core

---

## ?? NOTES

### Quan tr?ng
1. **Backup Database** thý?ng xuyên
2. **Test** trý?c khi deploy
3. **Document** code changes
4. **Review** security regularly

### Tips
1. S? d?ng **Git** ð? version control
2. **Comment** code ph?c t?p
3. Follow **coding standards**
4. Keep **dependencies** updated

---

# 17. CHI TI?T CODE T?NG D?NG

## 17.1. HOMECONTROLLER - UserHome Action

### Code ð?y ð?:
```csharp
public IActionResult UserHome()
{
    // 1. L?y danh sách s?n ph?m ð?c bi?t (Special Shoes)
    var specialCollection = _giaySvc.GetByCategory(PhanLoai.SpecialShoe);
    
    // 2. L?y t?t c? s?n ph?m không ph?i Special Shoes
    var allProducts = _giaySvc.GetAll()
        .Where(g => g.PhanLoai != PhanLoai.SpecialShoe)
        .OrderByDescending(g => g.Id)
        .Take(4)
        .ToList();
    
    // 3. Truy?n d? li?u vào ViewBag
    ViewBag.SpecialCollection = specialCollection;
    ViewBag.LatestProducts = allProducts;
    
    // 4. Tr? v? View
    return View();
}
```

### Gi?i thích t?ng d?ng:

**D?ng 1: `var specialCollection = _giaySvc.GetByCategory(PhanLoai.SpecialShoe);`**
- `_giaySvc`: Service ðý?c inject vào controller qua Dependency Injection
- `GetByCategory()`: Method trong GiaySvc ð? l?y s?n ph?m theo phân lo?i
- `PhanLoai.SpecialShoe`: Enum value = 4, ð?i di?n cho giày ð?c bi?t
- `specialCollection`: Bi?n ch?a danh sách các s?n ph?m Special Shoe
- **M?c ðích**: L?y 3 s?n ph?m ð?c bi?t ð? hi?n th? ? ð?u trang

**D?ng 3-6: L?y Latest Products**
```csharp
var allProducts = _giaySvc.GetAll()
    .Where(g => g.PhanLoai != PhanLoai.SpecialShoe)
    .OrderByDescending(g => g.Id)
    .Take(4)
    .ToList();
```
- `_giaySvc.GetAll()`: L?y T?T C? s?n ph?m t? database
- `.Where(g => g.PhanLoai != PhanLoai.SpecialShoe)`: L?c b? Special Shoes
  - `g`: Tham s? lambda ð?i di?n cho t?ng s?n ph?m (Giay)
  - `g.PhanLoai`: Thu?c tính phân lo?i c?a s?n ph?m
  - `!=`: Toán t? khác (not equal)
  - **L? do**: Tránh trùng l?p v?i Special Collection ? trên
- `.OrderByDescending(g => g.Id)`: S?p x?p gi?m d?n theo Id
  - Id cao nh?t = s?n ph?m m?i thêm vào g?n ðây nh?t
  - **L? do**: Hi?n th? s?n ph?m m?i nh?t trý?c
- `.Take(4)`: Ch? l?y 4 s?n ph?m ð?u tiên
  - **L? do**: Gi?i h?n hi?n th? ð? trang không quá dài
- `.ToList()`: Convert IQueryable sang List ð? s? d?ng
  - **Quan tr?ng**: Th?c thi query và load d? li?u vào memory

**D?ng 9: `ViewBag.SpecialCollection = specialCollection;`**
- `ViewBag`: Dynamic object ð? truy?n d? li?u t? Controller sang View
- `SpecialCollection`: Tên property ð?ng
- **L? do dùng ViewBag**: 
  - Ðõn gi?n, nhanh chóng
  - Không c?n t?o ViewModel riêng
  - Phù h?p v?i d? li?u hi?n th? ðõn gi?n

**D?ng 13: `return View();`**
- Tr? v? View týõng ?ng (Views/Home/UserHome.cshtml)
- View s? nh?n ViewBag và render HTML

---

## 17.2. CARTCONTROLLER - Add Action

### Code ð?y ð?:
```csharp
[HttpPost]
public IActionResult Add([FromBody] AddToCartRequest request)
{
    // 1. Ki?m tra ðãng nh?p
    var khachHangIdStr = HttpContext.Session.GetString(SessionKey.KhachHang.KH_Id);
    if (string.IsNullOrEmpty(khachHangIdStr))
    {
        return Json(new { success = false, message = "Vui l?ng ðãng nh?p" });
    }
    
    // 2. Parse Id khách hàng
    int khachHangId = int.Parse(khachHangIdStr);
    
    // 3. Ki?m tra s?n ph?m t?n t?i
    var giay = _giaySvc.Get(request.GiayId);
    if (giay == null)
    {
        return Json(new { success = false, message = "S?n ph?m không t?n t?i" });
    }
    
    // 4. Ki?m tra s?n ph?m ð? có trong gi? chýa
    var existingCart = _cartSvc.GetByKhachHangIdAndGiayId(khachHangId, request.GiayId, request.Size);
    
    if (existingCart != null)
    {
        // 4a. N?u ð? có: C?ng thêm s? lý?ng
        existingCart.SoLuong += request.Quantity;
        _cartSvc.Update(existingCart);
    }
    else
    {
        // 4b. N?u chýa có: Thêm m?i
        var cart = new Cart
        {
            KhachHangId = khachHangId,
            GiayId = request.GiayId,
            Size = request.Size,
            SoLuong = request.Quantity
        };
        _cartSvc.Add(cart);
    }
    
    // 5. L?y s? lý?ng items trong gi?
    var cartCount = _cartSvc.GetCartCount(khachHangId);
    
    // 6. Tr? v? JSON response
    return Json(new { 
        success = true, 
        message = "Ð? thêm vào gi? hàng",
        cartCount = cartCount 
    });
}
```

### Gi?i thích t?ng d?ng:

**D?ng 1: `[HttpPost]`**
- Attribute ch? ð?nh method ch? nh?n HTTP POST requests
- **L? do**: Thêm vào gi? là thao tác thay ð?i d? li?u, ph?i dùng POST
- GET request s? b? t? ch?i v?i l?i 405 Method Not Allowed

**D?ng 2: `public IActionResult Add([FromBody] AddToCartRequest request)`**
- `IActionResult`: Interface ð?i di?n cho k?t qu? tr? v?
- `Add`: Tên method/action
- `[FromBody]`: Attribute ch? ð?nh d? li?u ð?n t? request body (JSON)
- `AddToCartRequest`: Model class ch?a giayId, size, quantity
- **L? do dùng [FromBody]**: AJAX g?i JSON trong body, không ph?i form data

**D?ng 5: `var khachHangIdStr = HttpContext.Session.GetString(SessionKey.KhachHang.KH_Id);`**
- `HttpContext`: Object ch?a thông tin v? HTTP request/response hi?n t?i
- `Session`: Object qu?n l? session c?a user
- `GetString()`: L?y giá tr? string t? session
- `SessionKey.KhachHang.KH_Id`: Constant key = "KH_Id"
- `khachHangIdStr`: Bi?n ch?a Id d?ng string (ho?c null n?u chýa login)
- **L? do dùng Session**: Lýu thông tin ðãng nh?p t?m th?i, không c?n query DB m?i l?n

**D?ng 6-9: Ki?m tra ðãng nh?p**
```csharp
if (string.IsNullOrEmpty(khachHangIdStr))
{
    return Json(new { success = false, message = "Vui l?ng ðãng nh?p" });
}
```
- `string.IsNullOrEmpty()`: Ki?m tra string null ho?c r?ng
- **N?u chýa login**: Tr? v? JSON v?i success = false
- `new { ... }`: Anonymous object (không c?n ð?nh ngh?a class)
- **L? do**: B?o m?t, không cho user chýa login thêm vào gi?

**D?ng 12: `int khachHangId = int.Parse(khachHangIdStr);`**
- `int.Parse()`: Convert string sang int
- **Lýu ?**: Có th? ném FormatException n?u string không ph?i s?
- **An toàn**: V? session ch? lýu sau khi ð? validate khi login

**D?ng 15: `var giay = _giaySvc.Get(request.GiayId);`**
- `_giaySvc.Get()`: Method l?y s?n ph?m theo Id t? database
- **Tr? v?**: Object Giay ho?c null n?u không t?m th?y
- **M?c ðích**: Ð?m b?o s?n ph?m t?n t?i trý?c khi thêm vào gi?

**D?ng 16-19: Validate s?n ph?m**
```csharp
if (giay == null)
{
    return Json(new { success = false, message = "S?n ph?m không t?n t?i" });
}
```
- **N?u s?n ph?m không t?n t?i**: Tr? v? l?i
- **L? do**: Tránh thêm Id không h?p l? vào gi? hàng

**D?ng 22: `var existingCart = _cartSvc.GetByKhachHangIdAndGiayId(...)`**
- Ki?m tra s?n ph?m cùng size ð? có trong gi? chýa
- **Tham s?**: khachHangId, giayId, size
- **Tr? v?**: Object Cart ho?c null
- **L? do**: Tránh t?o nhi?u d?ng trùng l?p trong gi?

**D?ng 24-28: C?p nh?t s? lý?ng**
```csharp
if (existingCart != null)
{
    existingCart.SoLuong += request.Quantity;
    _cartSvc.Update(existingCart);
}
```
- **N?u ð? có**: C?ng thêm s? lý?ng vào item hi?n có
- `+=`: Toán t? c?ng g?p (addition assignment)
- `_cartSvc.Update()`: C?p nh?t database
- **L? do**: UX t?t hõn, user không c?n lo item b? trùng

**D?ng 29-39: Thêm item m?i**
```csharp
else
{
    var cart = new Cart
    {
        KhachHangId = khachHangId,
        GiayId = request.GiayId,
        Size = request.Size,
        SoLuong = request.Quantity
    };
    _cartSvc.Add(cart);
}
```
- **N?u chýa có**: T?o object Cart m?i
- `new Cart { ... }`: Object initializer syntax
- Gán giá tr? cho các properties
- `_cartSvc.Add()`: Insert vào database
- **Lýu ?**: Id s? t? ð?ng t?o b?i database (IDENTITY)

**D?ng 42: `var cartCount = _cartSvc.GetCartCount(khachHangId);`**
- Ð?m t?ng s? items trong gi? c?a khách hàng
- **M?c ðích**: C?p nh?t badge s? lý?ng trên navbar
- **Hi?u su?t**: Query nhanh v? ch? COUNT, không SELECT *

**D?ng 45-50: Tr? v? JSON**
```csharp
return Json(new { 
    success = true, 
    message = "Ð? thêm vào gi? hàng",
    cartCount = cartCount 
});
```
- `Json()`: Helper method t?o JsonResult
- Object ch?a 3 properties: success, message, cartCount
- JavaScript phía client s? parse JSON này
- **L? do**: AJAX c?n response d?ng JSON ð? x? l?

---

## 17.3. CHECKOUTCONTROLLER - PlaceOrder Action

### Code ð?y ð?:
```csharp
[HttpPost]
public IActionResult PlaceOrder(CheckoutViewModel model)
{
    // 1. Validate ModelState
    if (!ModelState.IsValid)
    {
        return View("Index", model);
    }
    
    // 2. L?y thông tin khách hàng t? session
    var khachHangIdStr = HttpContext.Session.GetString(SessionKey.KhachHang.KH_Id);
    if (string.IsNullOrEmpty(khachHangIdStr))
    {
        return RedirectToAction("Login", "Khachhang");
    }
    
    int khachHangId = int.Parse(khachHangIdStr);
    
    // 3. L?y danh sách s?n ph?m
    List<CartItem> cartItems;
    bool isBuyNow = false;
    
    // 3a. Ki?m tra Buy Now
    var buyNowItem = HttpContext.Session.GetObjectFromJson<BuyNowItem>("BuyNowItem");
    if (buyNowItem != null)
    {
        isBuyNow = true;
        cartItems = new List<CartItem>
        {
            new CartItem
            {
                GiayId = buyNowItem.GiayId,
                Ten = buyNowItem.Ten,
                Hinh = buyNowItem.Hinh,
                Gia = buyNowItem.Gia,
                Size = buyNowItem.Size,
                SoLuong = buyNowItem.SoLuong
            }
        };
    }
    else
    {
        // 3b. L?y t? gi? hàng
        var carts = _cartSvc.GetCartByKhachHangId(khachHangId);
        cartItems = carts.Select(c => new CartItem
        {
            GiayId = c.GiayId,
            Ten = c.Giay.Ten,
            Hinh = c.Giay.Hinh,
            Gia = (decimal)c.Giay.Gia,
            Size = c.Size,
            SoLuong = c.SoLuong
        }).ToList();
    }
    
    // 4. Tính t?ng ti?n
    decimal totalAmount = cartItems.Sum(item => item.Gia * item.SoLuong);
    
    // 5. T?o ðõn hàng
    var donhang = new Donhang
    {
        KhachHangId = khachHangId,
        NgayDat = DateTime.Now,
        TongTien = totalAmount,
        TrangThai = "Pending",
        NgayCapNhat = DateTime.Now
    };
    _donhangSvc.Add(donhang);
    
    // 6. T?o chi ti?t ðõn hàng
    foreach (var item in cartItems)
    {
        var chitiet = new DonhangChitiet
        {
            DonhangId = donhang.Id,
            GiayId = item.GiayId,
            Size = item.Size,
            SoLuong = item.SoLuong,
            DonGia = item.Gia
        };
        _donhangChitietSvc.Add(chitiet);
    }
    
    // 7. Xóa gi? hàng ho?c Buy Now session
    if (isBuyNow)
    {
        HttpContext.Session.Remove("BuyNowItem");
    }
    else
    {
        _cartSvc.ClearCart(khachHangId);
    }
    
    // 8. Lýu m? ðõn hàng vào TempData
    TempData["OrderId"] = donhang.Id;
    
    // 9. Redirect ð?n Success page
    return RedirectToAction("Success");
}
```

### Gi?i thích t?ng d?ng:

**D?ng 5-8: Validate ModelState**
```csharp
if (!ModelState.IsValid)
{
    return View("Index", model);
}
```
- `ModelState`: Object ch?a validation errors
- `IsValid`: Property boolean, true n?u không có l?i
- `!`: Toán t? NOT, ð?o ngý?c giá tr? boolean
- **N?u có l?i**: Tr? v? l?i form v?i l?i hi?n th?
- `View("Index", model)`: Render view Index.cshtml v?i model hi?n t?i
- **L? do**: Server-side validation, ð?m b?o d? li?u h?p l?

**D?ng 11-15: Ki?m tra ðãng nh?p**
```csharp
var khachHangIdStr = HttpContext.Session.GetString(SessionKey.KhachHang.KH_Id);
if (string.IsNullOrEmpty(khachHangIdStr))
{
    return RedirectToAction("Login", "Khachhang");
}
```
- L?y Id t? session (gi?ng CartController)
- **N?u chýa login**: Redirect ð?n trang Login
- `RedirectToAction()`: Chuy?n hý?ng ð?n action khác
- **L? do**: B?o m?t, ch? user ð? login m?i ð?t hàng ðý?c

**D?ng 20-21: Khai báo bi?n**
```csharp
List<CartItem> cartItems;
bool isBuyNow = false;
```
- `cartItems`: Danh sách s?n ph?m s? ð?t hàng
- Khai báo KHÔNG kh?i t?o (s? gán giá tr? sau)
- `isBuyNow`: Flag ð? phân bi?t Buy Now vs Checkout t? Cart
- **L? do**: X? l? khác nhau sau khi ð?t hàng

**D?ng 24: `var buyNowItem = HttpContext.Session.GetObjectFromJson<BuyNowItem>("BuyNowItem");`**
- `GetObjectFromJson<T>()`: Extension method custom (trong CartHelper)
- Deserialize JSON string thành object
- `<BuyNowItem>`: Generic type parameter
- **Tr? v?**: Object BuyNowItem ho?c null
- **L? do**: Session ch? lýu string, c?n convert v? object

**D?ng 25-39: X? l? Buy Now**
```csharp
if (buyNowItem != null)
{
    isBuyNow = true;
    cartItems = new List<CartItem>
    {
        new CartItem
        {
            GiayId = buyNowItem.GiayId,
            Ten = buyNowItem.Ten,
            Hinh = buyNowItem.Hinh,
            Gia = buyNowItem.Gia,
            Size = buyNowItem.Size,
            SoLuong = buyNowItem.SoLuong
        }
    };
}
```
- **N?u có Buy Now item**: Ch? l?y 1 s?n ph?m này
- `new List<CartItem> { ... }`: Collection initializer
- T?o 1 CartItem t? thông tin BuyNowItem
- **L? do**: Buy Now ch? mua 1 s?n ph?m, không c?n query database

**D?ng 40-52: X? l? Cart**
```csharp
else
{
    var carts = _cartSvc.GetCartByKhachHangId(khachHangId);
    cartItems = carts.Select(c => new CartItem
    {
        GiayId = c.GiayId,
        Ten = c.Giay.Ten,
        Hinh = c.Giay.Hinh,
        Gia = (decimal)c.Giay.Gia,
        Size = c.Size,
        SoLuong = c.SoLuong
    }).ToList();
}
```
- **N?u không ph?i Buy Now**: L?y t? gi? hàng
- `_cartSvc.GetCartByKhachHangId()`: L?y t?t c? items trong gi?
- `.Select()`: LINQ method, map m?i Cart thành CartItem
- `c.Giay.Ten`: Navigation property, EF t? ð?ng JOIN b?ng MonAns
- `(decimal)c.Giay.Gia`: Cast float sang decimal
- **L? do**: C?n thông tin chi ti?t s?n ph?m ð? t?o ðõn hàng

**D?ng 56: `decimal totalAmount = cartItems.Sum(item => item.Gia * item.SoLuong);`**
- `.Sum()`: LINQ aggregate function, tính t?ng
- Lambda: `item => item.Gia * item.SoLuong`
- Tính ti?n t?ng item r?i c?ng t?t c? l?i
- **Ki?u d? li?u**: decimal (chính xác cho ti?n t?)
- **L? do**: Lýu t?ng ti?n vào ðõn hàng ð? không ph?i tính l?i

**D?ng 59-67: T?o ðõn hàng**
```csharp
var donhang = new Donhang
{
    KhachHangId = khachHangId,
    NgayDat = DateTime.Now,
    TongTien = totalAmount,
    TrangThai = "Pending",
    NgayCapNhat = DateTime.Now
};
_donhangSvc.Add(donhang);
```
- T?o object Donhang m?i
- `DateTime.Now`: Th?i gian hi?n t?i
- `TrangThai = "Pending"`: Tr?ng thái ban ð?u
- `_donhangSvc.Add()`: Insert vào database
- **Quan tr?ng**: Sau Add, donhang.Id s? có giá tr? (do IDENTITY)
- **L? do**: C?n Id ð? t?o chi ti?t ðõn hàng

**D?ng 70-80: T?o chi ti?t ðõn hàng**
```csharp
foreach (var item in cartItems)
{
    var chitiet = new DonhangChitiet
    {
        DonhangId = donhang.Id,
        GiayId = item.GiayId,
        Size = item.Size,
        SoLuong = item.SoLuong,
        DonGia = item.Gia
    };
    _donhangChitietSvc.Add(chitiet);
}
```
- `foreach`: V?ng l?p qua t?ng item trong gi?
- T?o 1 DonhangChitiet cho m?i s?n ph?m
- `donhang.Id`: Foreign key liên k?t v?i ðõn hàng
- Lýu giá t?i th?i ði?m mua (`DonGia`)
- **L? do**: 
  - Lýu giá c? ð?nh, không b? ?nh hý?ng khi s?n ph?m thay ð?i giá
  - Lýu size c? th? khách ð? ch?n

**D?ng 83-91: Xóa d? li?u t?m**
```csharp
if (isBuyNow)
{
    HttpContext.Session.Remove("BuyNowItem");
}
else
{
    _cartSvc.ClearCart(khachHangId);
}
```
- **N?u Buy Now**: Xóa item kh?i session
- **N?u Cart**: Xóa t?t c? items kh?i gi? hàng (database)
- `Session.Remove()`: Xóa key kh?i session
- `ClearCart()`: DELETE all records v?i KhachHangId
- **L? do**: Ð? ð?t hàng r?i, không c?n gi? d? li?u t?m n?a

**D?ng 94: `TempData["OrderId"] = donhang.Id;`**
- `TempData`: Dictionary lýu d? li?u t?m cho 1 request k? ti?p
- Lýu OrderId ð? hi?n th? trong Success page
- **Khác v?i ViewBag**: TempData survive qua redirect
- **L? do**: Redirect sang action khác, không th? dùng ViewBag

**D?ng 97: `return RedirectToAction("Success");`**
- Chuy?n hý?ng ð?n action Success cùng controller
- Browser s? g?i GET request m?i ð?n /Checkout/Success
- **L? do**: 
  - Tránh POST l?i khi user refresh page
  - Best practice: POST-Redirect-GET pattern

---

## 17.4. GIAYSSVC - GetByCategory Method

### Code ð?y ð?:
```csharp
public List<Giay> GetByCategory(PhanLoai category)
{
    // 1. Query database
    return _context.MonAns
        .Where(g => g.PhanLoai == category)
        .ToList();
}
```

### Gi?i thích chi ti?t:

**D?ng 1: `public List<Giay> GetByCategory(PhanLoai category)`**
- `public`: Access modifier, method có th? g?i t? bên ngoài
- `List<Giay>`: Return type, danh sách các object Giay
- `PhanLoai`: Enum type (GiayDa=1, GiaySneaker=2, GiayTheThao=3, SpecialShoe=4)
- `category`: Tham s? ð?u vào

**D?ng 4: `_context.MonAns`**
- `_context`: DbContext instance (ðý?c inject)
- `MonAns`: DbSet<Giay>, ð?i di?n cho b?ng MonAns
- **Tr? v?**: IQueryable<Giay> (chýa th?c thi query)

**D?ng 5: `.Where(g => g.PhanLoai == category)`**
- LINQ method, filter d? li?u
- `g`: Tham s? lambda, ð?i di?n cho m?i Giay
- `g.PhanLoai == category`: Ði?u ki?n l?c
- **SQL týõng ðýõng**: `WHERE PhanLoai = @category`
- **Lýu ?**: Query v?n chýa ch?y (deferred execution)

**D?ng 6: `.ToList()`**
- Th?c thi query và load d? li?u vào memory
- **SQL Execute**: Lúc này m?i g?i query ð?n database
- Convert IQueryable thành List concrete
- **L? do**: 
  - C?n data ngay ð? tr? v?
  - List d? s? d?ng hõn IQueryable

**Quy tr?nh th?c thi:**
1. Method ðý?c g?i v?i category = PhanLoai.SpecialShoe (value = 4)
2. EF t?o SQL query: `SELECT * FROM MonAns WHERE PhanLoai = 4`
3. G?i query ð?n SQL Server
4. SQL Server th?c thi, tr? v? ResultSet
5. EF map m?i row thành object Giay
6. List<Giay> ðý?c tr? v?

---

## 17.5. AUTHENTICATION FILTER

### Code ð?y ð?:
```csharp
public class AuthenticationFilterAttribute : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        // 1. L?y username t? session
        var username = context.HttpContext.Session.GetString(SessionKey.NguoiDung.Username);
        
        // 2. Ki?m tra có ðãng nh?p không
        if (string.IsNullOrEmpty(username))
        {
            // 3. N?u chýa login: Redirect ð?n Login
            context.Result = new RedirectToActionResult("Login", "Admin", null);
        }
    }
    
    public void OnActionExecuted(ActionExecutedContext context)
    {
        // Không làm g? sau khi action th?c thi
    }
}
```

### Gi?i thích chi ti?t:

**D?ng 1: `public class AuthenticationFilterAttribute : IActionFilter`**
- `AuthenticationFilterAttribute`: Tên class (by convention, k?t thúc b?ng Attribute)
- `: IActionFilter`: Implement interface IActionFilter
- **M?c ðích**: T?o custom filter ð? b?o v? admin pages

**D?ng 3: `public void OnActionExecuting(ActionExecutingContext context)`**
- Method t? IActionFilter interface
- `OnActionExecuting`: Ch?y TRÝ?C action method
- `ActionExecutingContext`: Object ch?a thông tin v? action và request
- **Lýu ?**: Method này ch?y cho M?I request ð?n action có filter này

**D?ng 6: `var username = context.HttpContext.Session.GetString(SessionKey.NguoiDung.Username);`**
- `context.HttpContext`: HttpContext c?a request hi?n t?i
- L?y username t? session
- **Key**: "ND_Username" (t? SessionKey constant)
- **Tr? v?**: string username ho?c null

**D?ng 9-12: Ki?m tra và redirect**
```csharp
if (string.IsNullOrEmpty(username))
{
    context.Result = new RedirectToActionResult("Login", "Admin", null);
}
```
- **N?u chýa login**: username s? null ho?c empty
- `context.Result = ...`: Gán k?t qu? tr? v?
- `RedirectToActionResult`: Action result ð? redirect
- **Tham s?**: 
  - `"Login"`: Action name
  - `"Admin"`: Controller name
  - `null`: Route values (không có thêm params)
- **Quan tr?ng**: Khi gán Result, action method S? KHÔNG ch?y

**D?ng 16-19: OnActionExecuted**
```csharp
public void OnActionExecuted(ActionExecutedContext context)
{
    // Không làm g?
}
```
- Method ch?y SAU action method
- **Trong trý?ng h?p này**: Không c?n x? l? g?
- Ph?i implement v? là method c?a interface

**Cách s? d?ng:**
```csharp
[AuthenticationFilter]
public class GiayController : Controller
{
    // T?t c? actions trong controller này ð?u ðý?c b?o v?
}
```

**Ho?c ch? b?o v? 1 action:**
```csharp
[AuthenticationFilter]
public IActionResult Create()
{
    // Action này yêu c?u login
}
```

**Lu?ng th?c thi:**
1. Request ð?n `/Giay/Create`
2. AuthenticationFilter.OnActionExecuting() ch?y
3. Ki?m tra session
4. **N?u chýa login**: Redirect ð?n /Admin/Login (Create() KHÔNG ch?y)
5. **N?u ð? login**: Create() action ch?y b?nh thý?ng

---

## 17.6. UPLOAD HELPER

### Code ð?y ð?:
```csharp
public static class UploadHelper
{
    public static async Task<string> UploadImage(IFormFile file, string folder)
    {
        // 1. T?o tên file unique
        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
        
        // 2. T?o ðý?ng d?n ð?y ð?
        var path = Path.Combine("wwwroot/images", folder, fileName);
        
        // 3. T?o folder n?u chýa t?n t?i
        Directory.CreateDirectory(Path.GetDirectoryName(path));
        
        // 4. Upload file
        using (var stream = new FileStream(path, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }
        
        // 5. Tr? v? tên file
        return fileName;
    }
}
```

### Gi?i thích t?ng d?ng:

**D?ng 1: `public static class UploadHelper`**
- `static class`: Class ch? ch?a static members
- **Không th?**: T?o instance, inherit
- **L? do**: Helper methods, không c?n state

**D?ng 3: `public static async Task<string> UploadImage(IFormFile file, string folder)`**
- `static`: Method thu?c class, không thu?c instance
- `async`: Method b?t ð?ng b? (asynchronous)
- `Task<string>`: Promise tr? v? string khi hoàn thành
- `IFormFile`: Interface ð?i di?n cho file upload
- `folder`: Tên thý m?c con (vd: "Giay")

**D?ng 6: `var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);`**
- `Guid.NewGuid()`: T?o GUID m?i (128-bit unique identifier)
- **Ví d?**: `3f2504e0-4f89-11d3-9a0c-0305e82c3301`
- `.ToString()`: Convert GUID thành string
- `Path.GetExtension(file.FileName)`: L?y ph?n m? r?ng (.jpg, .png, ...)
- `file.FileName`: Tên file g?c user upload
- **K?t qu?**: `3f2504e0-4f89-11d3-9a0c-0305e82c3301.jpg`
- **L? do dùng GUID**: 
  - Ð?m b?o tên file không trùng 100%
  - Tránh conflict khi nhi?u user upload cùng lúc
  - B?o m?t: Không l? tên file g?c

**D?ng 9: `var path = Path.Combine("wwwroot/images", folder, fileName);`**
- `Path.Combine()`: N?i các ph?n ðý?ng d?n
- T? ð?ng thêm / ho?c \ phù h?p v?i OS
- **Ví d? k?t qu?**: `wwwroot/images/Giay/3f2504e0-4f89-11d3-9a0c-0305e82c3301.jpg`
- **L? do**: Cross-platform, tránh hardcode separator

**D?ng 12: `Directory.CreateDirectory(Path.GetDirectoryName(path));`**
- `Path.GetDirectoryName()`: L?y ph?n folder t? ðý?ng d?n
- **Ví d?**: `wwwroot/images/Giay`
- `Directory.CreateDirectory()`: T?o folder n?u chýa t?n t?i
- **Quan tr?ng**: Không ném exception n?u folder ð? t?n t?i
- **L? do**: Ð?m b?o folder t?n t?i trý?c khi lýu file

**D?ng 15-18: Upload file**
```csharp
using (var stream = new FileStream(path, FileMode.Create))
{
    await file.CopyToAsync(stream);
}
```
- `using`: Statement t? ð?ng dispose object sau khi dùng xong
- `FileStream`: Stream ð? ghi file vào disk
- `FileMode.Create`: T?o file m?i, ghi ðè n?u ð? t?n t?i
- `file.CopyToAsync(stream)`: Copy d? li?u t? upload vào file
- `await`: Ð?i operation hoàn thành (không block thread)
- **L? do dùng async**: 
  - File I/O ch?m, không nên block thread
  - Tãng throughput server

**D?ng 21: `return fileName;`**
- Tr? v? tên file ð? lýu
- **Không tr? v?**: Full path
- **L? do**: Ch? c?n lýu tên file vào database, folder c? ð?nh

**Cách s? d?ng:**
```csharp
if (giay.ImageFile != null)
{
    // Upload và l?y tên file
    giay.Hinh = await UploadHelper.UploadImage(giay.ImageFile, "Giay");
    // giay.Hinh = "3f2504e0-4f89-11d3-9a0c-0305e82c3301.jpg"
}
```

**Hi?n th? trong View:**
```html
<img src="~/images/Giay/@Model.Hinh" />
<!-- K?t qu?: /images/Giay/3f2504e0-4f89-11d3-9a0c-0305e82c3301.jpg -->
```

---

**Last Updated:** 2025-02-03  
**Version:** 2.0  
**Status:** Complete v?i UTF-8 encoding chu?n ?
