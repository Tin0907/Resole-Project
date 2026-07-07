# ?? T?NG H?P Ð?Y Ð? CÁC CH?C NÃNG WEB RESOLE SNEAKER SHOP

**D? án:** ReSole Sneaker Shop - H? th?ng thýõng m?i ði?n t?  
**L?p tr?nh viên:** Tr?nh Thiên Ân (MSSV: 422210281)  
**Ngôn ng?:** C# - ASP.NET Core 8.0  
**Database:** SQL Server  
**Ngày hoàn thành:** 2025  
**Phiên b?n:** 2.0

---

## ?? M?C L?C

1. [T?ng quan h? th?ng](#1-t?ng-quan-h?-th?ng)
2. [Ch?c nãng phía User (Khách hàng)](#2-ch?c-nãng-phía-user-khách-hàng)
3. [Ch?c nãng phía Admin (Qu?n tr? viên)](#3-ch?c-nãng-phía-admin-qu?n-tr?-viên)
4. [Ki?n trúc h? th?ng](#4-ki?n-trúc-h?-th?ng)
5. [Cõ s? d? li?u](#5-cõ-s?-d?-li?u)
6. [Các công ngh? s? d?ng](#6-các-công-ngh?-s?-d?ng)
7. [Quy tr?nh chi ti?t](#7-quy-tr?nh-chi-ti?t)
8. [Helpers & Utilities](#8-helpers--utilities)
9. [Layouts & Partials](#9-layouts--partials)
10. [X? l? l?i & B?o m?t](#10-x?-l?-l?i--b?o-m?t)
11. [C?i ti?n týõng lai](#11-c?i-ti?n-týõng-lai)

---

## 1. T?NG QUAN H? TH?NG

### ?? Mô t? d? án
**ReSole Sneaker Shop** là website thýõng m?i ði?n t? bán giày sneaker chuyên nghi?p, ðý?c phát tri?n b?ng ASP.NET Core 8.0 v?i SQL Server. H? th?ng có 2 ph?n chính:

- **?? User Site (Khách hàng):** Xem s?n ph?m, mua hàng, theo d?i ðõn hàng
- **?? Admin Site (Qu?n tr? viên):** Qu?n l? s?n ph?m, ðõn hàng, ngý?i dùng

### ?? M?c ðích chính
- Cung c?p tr?i nghi?m mua s?m tr?c tuy?n chuyên nghi?p
- Qu?n l? kho hàng hi?u qu?
- X? l? ðõn hàng t? ð?ng
- B?o m?t thông tin khách hàng

### ?? Th?ng kê d? án
- **S? controllers:** 8
- **S? services:** 6
- **S? models:** 12
- **S? views:** 40+
- **S? database tables:** 6
- **D?ng code:** ~5000+

---

## 2. CH?C NÃNG PHÍA USER (KHÁCH HÀNG)

### 2.1 ?? TRANG CH? (UserHome)
**URL:** `/Home/UserHome`

#### Các ph?n hi?n th?:
1. **Special Sneaker Collection**
   - Hi?n th? 3 s?n ph?m ð?c bi?t
   - Ch? s?n ph?m có `PhanLoai = SpecialShoe`
   - Thu hút khách hàng vào BST ð?c quy?n

2. **Our Latest Products**
   - Hi?n th? 4 s?n ph?m m?i nh?t
   - Lo?i tr? SpecialShoe
   - S?p x?p theo th? t? ngý?c (m?i nh?t)

3. **Brand Showcase**
   - Gi?i thi?u Nike & Adidas
   - Logo, mô t? ng?n
   - Tãng uy tín thýõng hi?u

### 2.2 ??? DANH SÁCH S?N PH?M (Shop)
**URL:** `/ThucDon/Index`

#### Hi?n th? s?n ph?m:
- ?? H?nh ?nh (crop t? ð?ng)
- ?? Tên s?n ph?m (gi?i h?n 2 d?ng)
- ?? Giá ti?n
- ??? Phân lo?i (GiayDa, GiaySneaker, GiayTheThao, SpecialShoe)
- ?? Mô t? (gi?i h?n 2 d?ng)
- ?? Tr?ng thái kho (C?n hàng / H?t hàng)

#### Tính nãng:
- **Favorite Icon:** Ðánh d?u s?n ph?m yêu thích
- **Hover Effects:** Card n?i lên, zoom ?nh nh?
- **Responsive Grid:** T? ð?ng ði?u ch?nh theo kích thý?c màn h?nh

### 2.3 ?? CHI TI?T S?N PH?M
**URL:** `/ThucDon/Details/{id}`

#### Tính nãng chính:
1. **Ch?n Size**
   - Radio buttons: 39, 40, 41, 42, 43
   - B?t bu?c ph?i ch?n trý?c khi mua

2. **Add to Cart (Thêm vào gi?)**
   - AJAX request POST
   - Ki?m tra ðãng nh?p
   - Ki?m tra s?n ph?m t?n t?i
   - C?p nh?t s? lý?ng n?u s?n ph?m ð? có
   - C?p nh?t badge gi? hàng

3. **Buy Now (Mua ngay)**
   - B? qua gi? hàng
   - Lýu vào Session
   - Chuy?n th?ng ð?n thanh toán
   - Không ?nh hý?ng gi? hàng hi?n t?i

#### Hi?n th? chi ti?t:
- H?nh ?nh l?n (500x500px)
- Tên ð?y ð?
- Giá chi ti?t (có ð?nh d?ng ti?n t?)
- Mô t? ð?y ð? (không gi?i h?n)
- Phân lo?i
- Tr?ng thái cãn hàng

### 2.4 ?? GI? HÀÌNG
**URL:** `/Cart/Index`

#### Ch?c nãng:
1. **Hi?n th? danh sách s?n ph?m**
   - Lây t? database b?ng Carts
   - Filter theo KhachHangId
   - Hi?n th? ð?y ð? thông tin

2. **Update Quantity (C?p nh?t s? lý?ng)**
   - Input number v?i nút +/-
   - N?u quantity > 0: Update
   - N?u quantity = 0: Xóa kh?i gi?

3. **Remove (Xóa s?n ph?m)**
   - Nút Remove v?i icon trash
   - Có popup confirm
   - Xóa theo GiayId và Size

4. **Clear Cart (Xóa toàn b?)**
   - Xóa t?t c? items
   - Có confirm

5. **Tính t?ng ti?n**
   - Realtime khi thay ð?i
   - T?ng = sum(price × quantity)

6. **Cart Badge**
   - Hi?n th? s? lý?ng items
   - Update liên t?c qua AJAX
   - Endpoint: `GET /Cart/GetCount`

### 2.5 ?? THANH TOÁN (Checkout)
**URL:** `/Checkout/Index`

#### Quy tr?nh:
1. **Hi?n th? thông tin**
   - Auto-fill: Tên, Email, SÐT, Ð?a ch?
   - Danh sách s?n ph?m t? Cart ho?c Buy Now
   - T?ng ti?n

2. **X? l? ðõn hàng**
   - Validate thông tin (server-side)
   - T?o record Donhang m?i
   - T?o DonhangChitiet cho m?i s?n ph?m
   - Xóa gi? hàng (n?u from Cart)
   - Xóa BuyNow session (n?u from BuyNow)

3. **Success Page**
   - `/Checkout/Success`
   - Thông báo thành công
   - M? ðõn hàng
   - Link v? trang ch?
   - Link xem ðõn hàng

### 2.6 ?? QU?N L? ÐÕN HÀNG (User)
**URL:** `/Donhang/Index`

#### Ch?c nãng:
1. **Danh sách ðõn hàng**
   - Ch? hi?n th? ðõn c?a khách ðang ðãng nh?p
   - Filter theo KhachHangId

2. **Tr?ng thái ðõn hàng**
   - Pending: Ch? x? l?
   - Processing: Ðang x? l?
   - Shipping: Ðang giao
   - Completed: Hoàn thành
   - Cancelled: Ð? h?y

3. **Chi ti?t ðõn hàng**
   - `/Donhang/Details/{id}`
   - Danh sách s?n ph?m
   - Thông tin giao hàng
   - L?ch s? thay ð?i

### 2.7 ?? TÀI KHO?N KHÁCH HÀNG

#### Ðãng k? (Register)
- **URL:** `/Khachhang/Register`
- **Fields:** Tên, Email (unique), SÐT, Ð?a ch?, Password, Confirm Password
- **M? hóa:** MD5 cho m?t kh?u
- **Validation:** Server-side & Client-side

#### Ðãng nh?p (Login)
- **URL:** `/Khachhang/Login`
- **Xác th?c:** Email + Password
- **Session:** Lýu KH_Id, KH_Email, KH_HoTen
- **Timeout:** 24 gi?

#### H? sõ (Profile)
- **URL:** `/Khachhang/Profile`
- **Xem:** Thông tin cá nhân
- **C?p nh?t:** Tên, SÐT, Ð?a ch?
- **Ð?i m?t kh?u:** Verify m?t kh?u c?

#### Ð?i m?t kh?u
- **Validation:** 
  - M?t kh?u c? ðúng
  - M?t kh?u m?i khác c?
  - Xác nh?n kh?p
- **Lýu:** MD5 hash

---

## 3. CH?C NÃNG PHÍA ADMIN (QU?N TR? VIÊN)

### 3.1 ?? ÐÃNG NH?P ADMIN
**URL:** `/Admin/Login`

#### Xác th?c:
- Username + Password
- M? hóa MD5
- Session: ND_Username, ND_HoTen
- Filter b?o v?: AuthenticationFilterAttribute

### 3.2 ?? QU?N L? S?N PH?M (Giày)

#### 3.2.1 Danh sách s?n ph?m
- **URL:** `/Giay/Index`
- **Hi?n th?:** T?t c? s?n ph?m (table)
- **Columns:**
  - H?nh ?nh (80x80px thumbnail)
  - Tên
  - Giá
  - Phân lo?i
  - Tr?ng thái
  - Mô t? (truncate, hover to expand)
  - Actions: Chi ti?t, S?a, Xóa

#### 3.2.2 Thêm s?n ph?m
- **URL:** `/Giay/Create`
- **Form fields:**
  - Tên giày (required, max 100)
  - Mô t? (optional, max 500)
  - Giá (required, 0-1,000,000)
  - Phân lo?i (dropdown)
  - H?nh ?nh (upload file)
  - Tr?ng thái (checkbox)

- **Upload logic:**
  - GUID unique filename
  - Save to: `wwwroot/images/Giay/`
  - Lýu tên file vào DB

#### 3.2.3 S?a s?n ph?m
- **URL:** `/Giay/Edit/{id}`
- **Tính nãng:**
  - Load d? li?u hi?n t?i
  - Cho phép update
  - Gi? ?nh c? n?u không upload m?i
  - Xóa ?nh c? khi upload ?nh m?i

#### 3.2.4 Xóa s?n ph?m
- **URL:** `POST /Giay/Delete/{id}`
- **Logic:**
  - Xóa ?nh kh?i server
  - Xóa record kh?i DB
  - Có confirm popup

#### 3.2.5 Chi ti?t s?n ph?m
- **URL:** `/Giay/Details/{id}`
- **Read-only mode**
- Hi?n th? t?t c? thông tin

### 3.3 ?? QU?N L? ÐÕN HÀNG (Admin)

#### 3.3.1 Danh sách ðõn hàng
- **URL:** `/Donhang/Index` (Admin view)
- **Hi?n th?:** T?t c? ðõn hàng t? m?i khách
- **Tính nãng:**
  - Filter theo tr?ng thái
  - Search theo m? ðõn, tên khách
  - S?p x?p theo ngày

#### 3.3.2 Chi ti?t ðõn hàng
- **URL:** `/Donhang/Details/{id}`
- **Hi?n th?:**
  - Thông tin khách hàng
  - Danh sách s?n ph?m (partial view)
  - T?ng ti?n
  - Tr?ng thái hi?n t?i
  - L?ch s? thay ð?i

#### 3.3.3 C?p nh?t tr?ng thái
- **URL:** `/Donhang/Edit/{id}`
- **UI:** Dropdown select tr?ng thái
- **Update:** `donhang.TrangThai = newStatus`

### 3.4 ????? QU?N L? NGÝ?I DÙNG ADMIN

#### 3.4.1 Danh sách ngý?i dùng
- **URL:** `/NguoiDung/Index`
- **Hi?n th?:** T?t c? admin users
- **Tính nãng:** T?m ki?m, filter theo tr?ng thái

#### 3.4.2 Thêm ngý?i dùng
- **URL:** `/NguoiDung/Create`
- **Fields:**
  - Username (unique, required)
  - M?t kh?u (required, min 6)
  - H? tên (optional)
  - Email (optional)
- **Validation:** Server-side

#### 3.4.3 S?a ngý?i dùng
- **URL:** `/NguoiDung/Update/{id}`
- **Gi?i h?n:** Không cho ð?i username
- **M?t kh?u:** Ch? ð?i khi nh?p m?t kh?u m?i

#### 3.4.4 Xóa ngý?i dùng
- **URL:** `POST /NguoiDung/Delete/{id}`
- **Validation:** Không cho xóa chính m?nh

### 3.5 ?? QU?N L? KHÁCH HÀNG (Admin)

#### 3.5.1 Danh sách khách hàng
- **URL:** `/Khachhang/Index`
- **Hi?n th?:** T?t c? khách ð? ðãng k?
- **Tính nãng:**
  - Xem l?ch s? ðõn hàng
  - Xem thông tin chi ti?t
  - Khóa/M? khóa tài kho?n (n?u c?n)

#### 3.5.2 Chi ti?t khách hàng
- **URL:** `/Khachhang/Details/{id}`
- **Thông tin:**
  - D? li?u cá nhân
  - T?ng s? ðõn hàng
  - T?ng giá tr? mua
  - Danh sách ðõn hàng g?n ðây

---

## 4. KI?N TRÚC H? TH?NG

### 4.1 MVC Pattern

```
???????????????????????????????
?        BROWSER/CLIENT       ?
???????????????????????????????
               ? HTTP Request
???????????????????????????????
?      CONTROLLERS            ?
? • HomeController            ?
? • GiayController            ?
? • CartController            ?
? • CheckoutController        ?
? • DonhangController         ?
? • KhachhangController       ?
? • NguoidungController       ?
? • AdminController           ?
???????????????????????????????
               ? Business Logic
???????????????????????????????
?       SERVICES              ?
? • GiaySvc                   ?
? • CartSvc                   ?
? • DonhangSvc                ?
? • DonhangChitietSvc         ?
? • KhachhangSvc              ?
? • NguoidungSvc              ?
???????????????????????????????
               ? Data Access
???????????????????????????????
?    ENTITY FRAMEWORK         ?
?     CORE (DbContext)        ?
???????????????????????????????
               ?
???????????????????????????????
?    SQL SERVER DATABASE      ?
???????????????????????????????
```

### 4.2 C?u trúc thý m?c

```
ASM/
??? Controllers/              # HTTP request handlers
?   ??? AdminController.cs
?   ??? GiayController.cs
?   ??? CartController.cs
?   ??? CheckoutController.cs
?   ??? DonhangController.cs
?   ??? KhachhangController.cs
?   ??? NguoidungController.cs
?   ??? HomeController.cs
?
??? Models/                   # Business entities
?   ??? Giay.cs
?   ??? Khachhang.cs
?   ??? Nguoidung.cs
?   ??? Donhang.cs
?   ??? DonhangChitiet.cs
?   ??? Cart.cs
?   ??? CartItem.cs
?   ??? ViewModels/           # DTOs
?   ?   ??? ChangePasswordViewModel.cs
?   ?   ??? KhachHangProfileViewModel.cs
?   ?   ??? ViewLogin.cs
?   ??? DataContext.cs        # EF Core DbContext
?
??? Services/                 # Business logic layer
?   ??? GiaySvc.cs
?   ??? CartSvc.cs
?   ??? DonhangSvc.cs
?   ??? DonhangChitietSvc.cs
?   ??? KhachhangSvc.cs
?   ??? NguoidungSvc.cs
?
??? Views/                    # Razor Views
?   ??? Admin/
?   ??? Giay/
?   ??? Cart/
?   ??? Checkout/
?   ??? Donhang/
?   ??? Khachhang/
?   ??? NguoiDung/
?   ??? ThucDon/
?   ??? Home/
?   ??? Account/
?   ??? Shared/
?
??? wwwroot/                  # Static files
?   ??? css/
?   ?   ??? site.css          # Global styles
?   ?   ??? login.css         # Login pages
?   ?   ??? _WebLayout.css    # User site
?   ??? js/
?   ?   ??? toasts.js         # Notifications
?   ?   ??? site.js           # Global scripts
?   ??? images/               # Product images
?   ?   ??? Giay/
?   ??? lib/                  # Third-party libraries
?
??? Helpers/                  # Utility classes
?   ??? MahoaHelper.cs        # MD5 Hashing
?   ??? UploadHelper.cs       # File upload
?   ??? CartHelper.cs         # Session extensions
?
??? Filters/                  # Custom filters
?   ??? AuthenticationFilterAttribute.cs
?
??? Constants/                # Constants & Enums
?   ??? SessionKey.cs
?
??? Migrations/               # EF Core migrations
?   ??? [Migration files]
?
??? Program.cs               # Application setup
```

### 4.3 Dependency Injection

**Program.cs:**
```csharp
// Services
builder.Services.AddTransient<IGiaySvc, GiaySvc>();
builder.Services.AddTransient<INguoidungSvc, NguoidungSvc>();
builder.Services.AddTransient<IKhachhangSvc, KhachhangSvc>();
builder.Services.AddTransient<IDonHangSvc, DonHangSvc>();
builder.Services.AddTransient<IDonhangChitietSvc, DonhangChitietSvc>();
builder.Services.AddTransient<ICartSvc, CartSvc>();

// Helpers
builder.Services.AddTransient<IMahoaHelper, MahoaHelper>();
builder.Services.AddTransient<IUploadHelper, UploadHelper>();

// DbContext
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("defaultConnection")));

// Session
builder.Services.AddSession(option => {
    option.IdleTimeout = TimeSpan.FromHours(24);
    option.Cookie.HttpOnly = true;
    option.Cookie.IsEssential = true;
});
```

---

## 5. CÕ S? D? LI?U

### 5.1 Sõ ð? m?i quan h?

```
???????????????????
?   Khachhangs    ?
???????????????????
? Id (PK)         ?
? HoTen           ?
? Email (UNIQUE)  ?
? DienThoai       ?
? DiaChi          ?
? MatKhau         ?
???????????????????
         ? 1:N
         ???????????????????????????????????
         ?                 ?               ?
    ????????????????  ?????????????????  ??????????????
    ?   Donhangs   ?  ?    Carts      ?  ? ShopReviews?
    ????????????????  ?????????????????  ??????????????
    ? Id (PK)      ?  ? Id (PK)       ?  ? Id (PK)    ?
    ? KhachHangId  ?  ? KhachHangId   ?  ? KhachHangId?
    ? NgayDat      ?  ? GiayId (FK)   ?  ? GiayId (FK)?
    ? TongTien     ?  ? Size          ?  ? Rating     ?
    ? TrangThai    ?  ? SoLuong       ?  ? Comment    ?
    ? NgayCapNhat  ?  ?????????????????  ? NgayTao    ?
    ????????????????      ?               ??????????????
         ?                ? N:1
         ? 1:N            ?
         ?                ????????
         ?                       ?
    ?????????????????????   ?????????????????????
    ? DonhangChitiets   ?   ? MonAns (Giay)     ?
    ?????????????????????   ?????????????????????
    ? Id (PK)           ?   ? Id (PK)           ?
    ? DonhangId (FK)    ?   ? Ten               ?
    ? GiayId (FK) ?????????>? MoTa              ?
    ? Size              ?   ? Gia               ?
    ? SoLuong           ?   ? PhanLoai (Enum)   ?
    ? DonGia            ?   ? Hinh              ?
    ?????????????????????   ? TrangThai         ?
                            ? InventoryCount    ?
                            ? Size              ?
                            ?????????????????????

???????????????????
?  Nguoidungs     ?
???????????????????
? Id (PK)         ?
? Username(UNIQUE)?
? Password        ?
? HoTen           ?
? Email           ?
???????????????????
```

### 5.2 Chi ti?t các b?ng

#### Khachhangs (Khách hàng)
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

#### Nguoidungs (Admin)
```sql
CREATE TABLE Nguoidungs (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Username NVARCHAR(50) NOT NULL UNIQUE,
    Password NVARCHAR(255) NOT NULL,
    HoTen NVARCHAR(100),
    Email NVARCHAR(100)
)
```

#### MonAns (Giày)
```sql
CREATE TABLE MonAns (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Ten NVARCHAR(100) NOT NULL,
    MoTa NVARCHAR(500),
    Gia FLOAT NOT NULL,
    PhanLoai INT NOT NULL,  -- 1: GiayDa, 2: GiaySneaker, 3: GiayTheThao, 4: SpecialShoe
    Hinh NVARCHAR(200),
    TrangThai BIT NOT NULL DEFAULT 1,
    InventoryCount INT DEFAULT 0,
    Size NVARCHAR(50)
)
```

#### Carts (Gi? hàng)
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

#### Donhangs (Ðõn hàng)
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

#### DonhangChitiets (Chi ti?t ðõn hàng)
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

#### ShopReviews (Ðánh giá s?n ph?m)
```sql
CREATE TABLE ShopReviews (
    Id INT PRIMARY KEY IDENTITY(1,1),
    KhachHangId INT NOT NULL,
    GiayId INT NOT NULL,
    Rating INT NOT NULL CHECK (Rating >= 1 AND Rating <= 5),
    Comment NVARCHAR(500),
    NgayTao DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (KhachHangId) REFERENCES Khachhangs(Id),
    FOREIGN KEY (GiayId) REFERENCES MonAns(Id)
)
```

---

## 6. CÁC CÔNG NGH? S? D?NG

### 6.1 Backend

| Công ngh? | Phiên b?n | M?c ðích |
|-----------|----------|---------|
| **ASP.NET Core** | 8.0 | Web framework chính |
| **Entity Framework Core** | 8.0 | ORM & Data Access |
| **SQL Server** | 2019+ | Database engine |
| **C#** | 12.0 | Ngôn ng? l?p tr?nh |

### 6.2 Frontend

| Công ngh? | M?c ðích |
|-----------|---------|
| **Razor Pages** | Template engine |
| **Bootstrap 5** | Responsive UI Framework |
| **Bootstrap Icons** | Icon library |
| **jQuery** | DOM manipulation |
| **AJAX** | Async requests |
| **CSS3** | Styling |
| **JavaScript (Vanilla)** | Client-side logic |

### 6.3 B?o m?t

#### Password Hashing (MD5)
```csharp
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
```
**Lýu ?:** MD5 không an toàn tuy?t ð?i, nên dùng bcrypt/PBKDF2 cho production

#### Session Management
- **Timeout:** 24 gi?
- **HttpOnly:** true (b?o v? t? XSS)
- **Cookie:** IsEssential = true

#### Authentication Filter
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

## 7. QUY TR?NH CHI TI?T

### 7.1 Flow Mua Hàng (Add to Cart)

```
1. User ? trang Details (/ThucDon/Details/{id})
   ?
2. Ch?n size (b?t bu?c)
   ?
3. Nh?p s? lý?ng (m?c ð?nh 1)
   ?
4. Nh?n "Add to Cart"
   ?
5. JavaScript g?i AJAX POST request
   URL: /Cart/Add
   Data: {
     giayId: 1,
     size: "42",
     quantity: 1
   }
   ?
6. CartController.Add() x? l?:
   • Ki?m tra login (Session.KH_Id)
   • Ki?m tra s?n ph?m t?n t?i
   • T?m s?n ph?m cùng size trong gi?
   • N?u có: C?ng s? lý?ng
   • N?u chýa: Thêm item m?i
   • L?y s? lý?ng items
   • Return JSON {success: true, cartCount: 3}
   ?
7. JavaScript nh?n response:
   • Update cart badge (s? 3)
   • Hi?n th? thông báo
   • Không reload page
   ?
8. Hoàn t?t ?
```

### 7.2 Flow Thanh Toán (Checkout)

```
1. User ? trang Cart (/Cart/Index)
   ?
2. Nh?n "Checkout"
   ?
3. Redirect ð?n /Checkout/Index
   ?
4. CheckoutController.Index():
   • Load thông tin khách t? session
   • Load gi? hàng t? DB
   • Tính t?ng ti?n
   • Hi?n th? form xác nh?n
   ?
5. User ði?n thông tin (tên, email, sdt, ð?a ch?)
   ?
6. Nh?n "Place Order"
   ?
7. POST /Checkout/PlaceOrder:
   • Validate d? li?u
   • T?o record Donhang
   • T?o DonhangChitiet cho m?i s?n ph?m
   • Xóa gi? hàng
   • Commit transaction
   ?
8. Redirect ð?n /Checkout/Success
   ?
9. Success page hi?n th?:
   • Thông báo "C?m õn b?n ð? ð?t hàng"
   • M? ðõn hàng
   • Link v? trang ch?
   • Link xem ðõn hàng
   ?
10. Hoàn t?t ?
```

### 7.3 Flow Mua Ngay (Buy Now)

```
1. User ? trang Details (/ThucDon/Details/{id})
   ?
2. Ch?n size + s? lý?ng
   ?
3. Nh?n "Buy Now"
   ?
4. POST /Checkout/BuyNow:
   • T?o BuyNowItem
   • Lýu vào Session (không lýu DB)
   • Return JSON
   ?
5. JavaScript redirect ð?n /Checkout/Index
   ?
6. CheckoutController.Index():
   • Ki?m tra có BuyNowItem trong session không
   • N?u có: Dùng BuyNowItem
   • N?u không: Dùng Cart
   ?
7. User xác nh?n và ð?t hàng (gi?ng Checkout b?nh thý?ng)
   ?
8. PlaceOrder():
   • T?o ðõn hàng
   • Xóa BuyNowItem kh?i session
   • KHÔNG xóa gi? hàng
   ?
9. Redirect ð?n Success page
   ?
10. Hoàn t?t ?

Khác bi?t:
? Buy Now: Mua 1 s?n ph?m nhanh, không c?n gi? hàng
? Checkout: Mua nhi?u s?n ph?m, dùng gi? hàng
```

---

## 8. HELPERS & UTILITIES

### 8.1 MahoaHelper.cs
**M?c ðích:** M? hóa m?t kh?u b?ng MD5
```csharp
public static class MahoaHelper
{
    public static string MaHoaMD5(string input)
    {
        // M? hóa MD5 cho password
    }
}
```

### 8.2 UploadHelper.cs
**M?c ðích:** Upload h?nh ?nh s?n ph?m
```csharp
public static class UploadHelper
{
    public static async Task<string> UploadImage(IFormFile file, string folder)
    {
        // 1. T?o tên file unique (GUID + extension)
        // 2. T?o ðý?ng d?n ð?y ð?
        // 3. T?o folder n?u chýa t?n t?i
        // 4. Upload file async
        // 5. Tr? v? tên file
    }
}
```
**L?i ích:**
- GUID ð?m b?o tên file không trùng
- Tr?nh xây d?ng dý?i folder riêng
- Cross-platform path handling

### 8.3 CartHelper.cs
**M?c ð?:** Lýu object vào Session
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
**S? d?ng:** Session không th? lýu object tr?c ti?p, c?n serialize thành JSON

### 8.4 SessionKey.cs
**M?c ðích:** Qu?n l? session keys t?p trung
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
**L?i ích:**
- T?p trung qu?n l? keys
- Tr?nh typo
- IntelliSense support

---

## 9. LAYOUTS & PARTIALS

### 9.1 _Layout.cshtml (Admin)
- Navbar v?i logo, menu
- Main content area
- Footer
- Script references

### 9.2 _WebLayout.cshtml (User Site)
- **Header:** Logo, menu navigation
- **Sticky Navbar:** 
  - C? ð?nh khi scroll
  - Blur background effect
  - Transition smooth
- **Cart Badge:** S? lý?ng items
- **User Menu:** Dropdown (Profile, Orders, Logout)
- **Footer:** Ð?a ch?, liên h?, social links
- **Responsive:** Mobile-first design

### 9.3 _LoginLayout.cshtml
- Minimal layout cho login pages
- Center aligned form
- Background image/gradient
- No navigation

### 9.4 _LoginPartial.cshtml
```cshtml
@if (Context.Session.GetString(SessionKey.KhachHang.KH_Email) != null)
{
    <li class="nav-item dropdown">
        <a class="nav-link dropdown-toggle" data-bs-toggle="dropdown">
            @Context.Session.GetString(SessionKey.KhachHang.KH_HoTen)
        </a>
        <ul class="dropdown-menu">
            <li><a class="dropdown-item" asp-action="Profile">H? sõ</a></li>
            <li><a class="dropdown-item" asp-action="Index" asp-controller="Donhang">Ðõn hàng</a></li>
            <li><hr class="dropdown-divider"></li>
            <li><a class="dropdown-item" asp-action="Logout">Ðãng xu?t</a></li>
        </ul>
    </li>
}
else
{
    <li class="nav-item">
        <a class="nav-link" asp-action="Login">Ðãng nh?p</a>
    </li>
}
```

### 9.5 _MenuPartial.cshtml
- Admin sidebar menu
- Conditional rendering based on permissions

### 9.6 _ValidationScriptsPartial.cshtml
- jQuery Validation library
- Unobtrusive validation

### 9.7 _DonhangChitiet.cshtml
**Partial view** hi?n th? chi ti?t ðõn hàng:
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
                <td>@item.DonGia.ToString("N0") þ</td>
                <td>@((item.DonGia * item.SoLuong).ToString("N0")) þ</td>
            </tr>
        }
    </tbody>
</table>
```

---

## 10. X? L? L?I & B?O M?T

### 10.1 Global Error Handling
- **File:** `Views/Shared/Error.cshtml`
- Trang l?i chung cho toàn h? th?ng
- Hi?n th? Request ID cho debug

### 10.2 Authentication & Authorization
- **Filter:** `AuthenticationFilterAttribute`
- B?o v? trang admin
- Redirect sang login n?u chýa ðãng nh?p

### 10.3 Data Validation

#### Client-side
- HTML5 validation attributes
- jQuery Validation Plugin
- Real-time error messages

#### Server-side
```csharp
if (!ModelState.IsValid)
{
    return View(model);
}
```
- Validate t?t c? input
- Trý?c khi lýu database

### 10.4 SQL Injection Prevention
- Dùng Entity Framework (Parameterized Queries)
- Không dùng raw SQL
- Validate & sanitize input

### 10.5 XSS Prevention
- HTML encode output
- Session HttpOnly = true
- CSRF token (built-in Asp.NET Core)

### 10.6 Sensitive Data
- M?t kh?u m? hóa trý?c lýu DB
- Không log m?t kh?u
- Use HTTPS in production

---

## 11. CÁC CH?C NÃNG CÓ TH? THÊM TRONG TÝÕNG LAI

### 11.1 E-commerce Features
1. **Payment Integration**
   - PayPal
   - Stripe
   - VNPay (cho Vietnam)
   - Bank transfer

2. **Order Management**
   - Real-time order tracking
   - Email notifications
   - SMS notifications
   - Delivery partners integration

3. **Product Features**
   - Product reviews & ratings
   - Wishlish (favorites)
   - Detailed specifications
   - Size guide
   - Product recommendation

4. **User Features**
   - Wishlist management
   - Address book
   - Order history filtering
   - Return/Refund management
   - Account preferences

5. **Promotions**
   - Discount codes
   - Flash sales
   - Seasonal promotions
   - Bundle deals
   - Loyalty program

### 11.2 Administrative Features
1. **Dashboard Analytics**
   - Sales metrics
   - Revenue charts
   - Customer insights
   - Inventory alerts
   - Top-selling products

2. **Inventory Management**
   - Stock tracking
   - Low stock alerts
   - Automatic reordering
   - Warehouse management

3. **Marketing Tools**
   - Email campaigns
   - SMS marketing
   - Social media integration
   - SEO optimization

4. **Advanced Reporting**
   - Sales reports
   - Customer reports
   - Inventory reports
   - Export to Excel/PDF

### 11.3 Technical Improvements
1. **Performance**
   - Caching (Redis)
   - CDN for static files
   - Database indexing
   - Query optimization

2. **Testing**
   - Unit tests
   - Integration tests
   - End-to-end tests
   - Load testing

3. **DevOps**
   - Docker containerization
   - CI/CD pipeline
   - Automated deployment
   - Monitoring & logging

4. **Security Enhancements**
   - bcrypt password hashing
   - OAuth 2.0 authentication
   - Two-factor authentication
   - API rate limiting

---

## ?? TH?NG KÊ T?NG K?T

| Tiêu chí | S? lý?ng |
|----------|---------|
| **Controllers** | 8 |
| **Services** | 6 |
| **Models** | 12 |
| **Views** | 40+ |
| **Database Tables** | 6 |
| **Database Fields** | 50+ |
| **CSS Files** | 3 |
| **JavaScript Functions** | 50+ |
| **Helpers/Utilities** | 4 |
| **Filters** | 1 |
| **Migrations** | 4+ |
| **D?ng code** | ~5000+ |

---

## ?? ÐI?M N?I B?T

? **Giao di?n ðõn gi?n, thân thi?n**
? **B?o m?t cõ b?n (MD5, Session, Filter)**
? **Responsive design (Mobile-first)**
? **X? l? l?i toàn c?c**
? **Validation client-side & server-side**
? **AJAX cho tr?i nghi?m mý?t mà**
? **C?u trúc code s?ch (MVC, DI, Service Layer)**
? **Database transactions**
? **Entity Framework ORM**
? **Middleware & Filters**
? **Session management**
? **File upload handling**
? **Partial views reusable**
? **Bootstrap responsive framework**
? **Vietnamese localization 100%**

---

## ?? THÔNG TIN LIÊN H?

**L?p tr?nh viên:** Tr?nh Thiên Ân  
**MSSV:** 422210281  
**GitHub:** https://github.com/Tin0907/ReSole  
**Email:** [Liên h? qua GitHub]

---

## ?? TÀI LI?U THAM KH?O

- [ASP.NET Core Documentation](https://docs.microsoft.com/aspnet/core)
- [Entity Framework Core](https://docs.microsoft.com/ef/core)
- [Bootstrap 5 Documentation](https://getbootstrap.com/docs/5.0)
- [SQL Server Documentation](https://docs.microsoft.com/sql/sql-server)
- [jQuery Documentation](https://api.jquery.com)

---

**Phiên b?n:** 2.0  
**Ngày c?p nh?t cu?i cùng:** 2025  
**Tr?ng thái:** ? Hoàn thành & Build thành công

