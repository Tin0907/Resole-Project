# ?? HÝ?NG D?N CHI TI?T CÁCH TH?C HI?N T?ng CH?C NÃNG

**D? án:** ReSole Sneaker Shop  
**Phiên b?n:** 2.0  
**C?p nh?t:** 2025  
**M?c ðích:** Hý?ng d?n t?ng bý?c cách xây d?ng và th?c hi?n t?t c? ch?c nãng

---

## ?? M?C L?C

1. [Ph?n I: Chu?n b? môi trý?ng](#ph?n-i-chu?n-b?-môi-trý?ng)
2. [Ph?n II: Xây d?ng cõ s? d? li?u](#ph?n-ii-xây-d?ng-cõ-s?-d?-li?u)
3. [Ph?n III: T?o Models](#ph?n-iii-t?o-models)
4. [Ph?n IV: T?o Services](#ph?n-iv-t?o-services)
5. [Ph?n V: Xây d?ng Controllers](#ph?n-v-xây-d?ng-controllers)
6. [Ph?n VI: T?o Views & Layouts](#ph?n-vi-t?o-views--layouts)
7. [Ph?n VII: Tính nãng Gi? Hàng](#ph?n-vii-tính-nãng-gi?-hàng)
8. [Ph?n VIII: Thanh Toán & Ð?t Hàng](#ph?n-viii-thanh-toán--ð?t-hàng)
9. [Ph?n IX: Qu?n L? Admin](#ph?n-ix-qu?n-l?-admin)
10. [Ph?n X: Styling & UX](#ph?n-x-styling--ux)

---

## PH?N I: CHU?N B? MÔI TRÝ?NG

### Bý?c 1: T?o Project ASP.NET Core Razor Pages

```bash
# M? Command Prompt/PowerShell
dotnet new webapp -n ReSole -f net8.0
cd ReSole
```

### Bý?c 2: Cài ð?t Required NuGet Packages

```bash
# Entity Framework Core
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Tools
dotnet add package Microsoft.EntityFrameworkCore.Design

# Validation
dotnet add package Microsoft.AspNetCore.Mvc.DataAnnotations
```

### Bý?c 3: C?u h?nh appsettings.json

**File:** `appsettings.json`
```json
{
  "ConnectionStrings": {
    "defaultConnection": "Server=(localdb)\\mssqllocaldb;Database=ReSole_DB;Trusted_Connection=True;MultipleActiveResultSets=true"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information"
    }
  },
  "AllowedHosts": "*"
}
```

### Bý?c 4: C?u h?nh Program.cs

**File:** `Program.cs`
```csharp
using ReSole.Models;
using ReSole.Services;

var builder = WebApplication.CreateBuilder(args);

// 1. Thêm DbContext
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("defaultConnection")));

// 2. Thêm Controllers & Views
builder.Services.AddControllersWithViews();

// 3. Thêm Services
builder.Services.AddTransient<IGiaySvc, GiaySvc>();
builder.Services.AddTransient<INguoidungSvc, NguoidungSvc>();
builder.Services.AddTransient<IKhachhangSvc, KhachhangSvc>();
builder.Services.AddTransient<IDonHangSvc, DonHangSvc>();
builder.Services.AddTransient<IDonhangChitietSvc, DonhangChitietSvc>();
builder.Services.AddTransient<ICartSvc, CartSvc>();

// 4. Thêm Helpers
builder.Services.AddTransient<IMahoaHelper, MahoaHelper>();
builder.Services.AddTransient<IUploadHelper, UploadHelper>();

// 5. Thêm Session
builder.Services.AddSession(option => {
    option.IdleTimeout = TimeSpan.FromHours(24);
    option.Cookie.HttpOnly = true;
    option.Cookie.IsEssential = true;
});

var app = builder.Build();

// Middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
```

---

## PH?N II: XÂY D?NG CÕ S? D? LI?U

### Bý?c 1: T?o Models

#### Model 1: Giay (S?n ph?m)
**File:** `Models/Giay.cs`
```csharp
namespace ReSole.Models
{
    public class Giay
    {
        public int Id { get; set; }
        public string Ten { get; set; } // Tên giày
        public string MoTa { get; set; } // Mô t?
        public float Gia { get; set; } // Giá
        public int PhanLoai { get; set; } // Phân lo?i: 1=GiayDa, 2=GiaySneaker, 3=GiayTheThao, 4=SpecialShoe
        public string Hinh { get; set; } // Tên file h?nh ?nh
        public bool TrangThai { get; set; } = true; // Tr?ng thái: true=C?n hàng
        
        // Relationships
        public List<Cart> Carts { get; set; } = new List<Cart>();
        public List<DonhangChitiet> DonhangChitiets { get; set; } = new List<DonhangChitiet>();
    }
}
```

**Gi?i thích:**
- `Id`: Primary Key, t? ð?ng tãng
- `PhanLoai`: Enum d?ng int (1-4)
- `Hinh`: Lýu tên file, không lýu full path
- `TrangThai`: true = C?n hàng, false = H?t hàng
- Relationships: M?t s?n ph?m có nhi?u carts và donhang chitiets

#### Model 2: Khachhang (Khách hàng)
**File:** `Models/Khachhang.cs`
```csharp
namespace ReSole.Models
{
    public class Khachhang
    {
        public int Id { get; set; }
        public string HoTen { get; set; } // H? và tên
        public string Email { get; set; } // Email (unique)
        public string DienThoai { get; set; } // Ði?n tho?i
        public string DiaChi { get; set; } // Ð?a ch?
        public string MatKhau { get; set; } // M?t kh?u (m? hóa MD5)
        
        // Relationships
        public List<Donhang> Donhangs { get; set; } = new List<Donhang>();
        public List<Cart> Carts { get; set; } = new List<Cart>();
    }
}
```

#### Model 3: Donhang (Ðõn hàng)
**File:** `Models/Donhang.cs`
```csharp
namespace ReSole.Models
{
    public class Donhang
    {
        public int Id { get; set; }
        public int KhachHangId { get; set; } // FK
        public DateTime NgayDat { get; set; } = DateTime.Now; // Ngày ð?t
        public decimal TongTien { get; set; } // T?ng ti?n
        public string TrangThai { get; set; } = "Pending"; // Tr?ng thái: Pending, Processing, Shipping, Completed, Cancelled
        public DateTime? NgayCapNhat { get; set; } // Ngày c?p nh?t
        
        // Relationships
        public Khachhang Khachhang { get; set; }
        public List<DonhangChitiet> DonhangChitiets { get; set; } = new List<DonhangChitiet>();
    }
}
```

#### Model 4: DonhangChitiet (Chi ti?t ðõn hàng)
**File:** `Models/DonhangChitiet.cs`
```csharp
namespace ReSole.Models
{
    public class DonhangChitiet
    {
        public int Id { get; set; }
        public int DonhangId { get; set; } // FK
        public int GiayId { get; set; } // FK
        public string Size { get; set; } // Size: 39, 40, 41, 42, 43
        public int SoLuong { get; set; } // S? lý?ng
        public decimal DonGia { get; set; } // Ðõn giá (lýu giá c?)
        
        // Relationships
        public Donhang Donhang { get; set; }
        public Giay Giay { get; set; }
    }
}
```

#### Model 5: Cart (Gi? hàng)
**File:** `Models/Cart.cs`
```csharp
namespace ReSole.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public int KhachHangId { get; set; } // FK
        public int GiayId { get; set; } // FK
        public string Size { get; set; } // Size: 39, 40, 41, 42, 43
        public int SoLuong { get; set; } = 1; // S? lý?ng (m?c ð?nh 1)
        
        // Relationships
        public Khachhang Khachhang { get; set; }
        public Giay Giay { get; set; }
    }
}
```

#### Model 6: Nguoidung (Admin)
**File:** `Models/Nguoidung.cs`
```csharp
namespace ReSole.Models
{
    public class Nguoidung
    {
        public int Id { get; set; }
        public string Username { get; set; } // Tên ðãng nh?p (unique)
        public string Password { get; set; } // M?t kh?u (m? hóa MD5)
        public string HoTen { get; set; } // H? và tên
        public string Email { get; set; } // Email
    }
}
```

### Bý?c 2: T?o DataContext

**File:** `Models/DataContext.cs`
```csharp
using Microsoft.EntityFrameworkCore;

namespace ReSole.Models
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Giay> MonAns { get; set; } // B?ng giày
        public DbSet<Khachhang> Khachhangs { get; set; } // B?ng khách hàng
        public DbSet<Nguoidung> Nguoidungs { get; set; } // B?ng admin
        public DbSet<Donhang> Donhangs { get; set; } // B?ng ðõn hàng
        public DbSet<DonhangChitiet> DonhangChitiets { get; set; } // B?ng chi ti?t ðõn hàng
        public DbSet<Cart> Carts { get; set; } // B?ng gi? hàng

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Constraints cho Email
            modelBuilder.Entity<Khachhang>()
                .HasIndex(k => k.Email)
                .IsUnique();

            // Constraints cho Username
            modelBuilder.Entity<Nguoidung>()
                .HasIndex(n => n.Username)
                .IsUnique();

            // Relationships
            modelBuilder.Entity<Donhang>()
                .HasOne(d => d.Khachhang)
                .WithMany(k => k.Donhangs)
                .HasForeignKey(d => d.KhachHangId);

            modelBuilder.Entity<DonhangChitiet>()
                .HasOne(d => d.Donhang)
                .WithMany(don => don.DonhangChitiets)
                .HasForeignKey(d => d.DonhangId);

            modelBuilder.Entity<DonhangChitiet>()
                .HasOne(d => d.Giay)
                .WithMany(g => g.DonhangChitiets)
                .HasForeignKey(d => d.GiayId);

            modelBuilder.Entity<Cart>()
                .HasOne(c => c.Khachhang)
                .WithMany(k => k.Carts)
                .HasForeignKey(c => c.KhachHangId);

            modelBuilder.Entity<Cart>()
                .HasOne(c => c.Giay)
                .WithMany(g => g.Carts)
                .HasForeignKey(c => c.GiayId);
        }
    }
}
```

### Bý?c 3: T?o Migration & Database

```bash
# T?o migration ð?u tiên
dotnet ef migrations add InitialCreate

# Update database
dotnet ef database update
```

**K?t qu?:** Database ReSole_DB ðý?c t?o v?i 6 b?ng: MonAns, Khachhangs, Nguoidungs, Donhangs, DonhangChitiets, Carts

---

## PH?N III: T?O SERVICES

### Bý?c 1: T?o Service Interface & Implementation

#### Service 1: GiaySvc (S?n ph?m)

**File:** `Services/IGiaySvc.cs`
```csharp
namespace ReSole.Services
{
    public interface IGiaySvc
    {
        List<Giay> GetAll();
        Giay Get(int id);
        List<Giay> GetByCategory(int phanLoai);
        void Add(Giay giay);
        void Update(Giay giay);
        void Delete(int id);
    }
}
```

**File:** `Services/GiaySvc.cs`
```csharp
using ReSole.Models;

namespace ReSole.Services
{
    public class GiaySvc : IGiaySvc
    {
        private readonly DataContext _context;

        public GiaySvc(DataContext context)
        {
            _context = context;
        }

        public List<Giay> GetAll()
        {
            return _context.MonAns.ToList();
        }

        public Giay Get(int id)
        {
            return _context.MonAns.FirstOrDefault(g => g.Id == id);
        }

        public List<Giay> GetByCategory(int phanLoai)
        {
            return _context.MonAns.Where(g => g.PhanLoai == phanLoai).ToList();
        }

        public void Add(Giay giay)
        {
            _context.MonAns.Add(giay);
            _context.SaveChanges();
        }

        public void Update(Giay giay)
        {
            _context.MonAns.Update(giay);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var giay = Get(id);
            if (giay != null)
            {
                _context.MonAns.Remove(giay);
                _context.SaveChanges();
            }
        }
    }
}
```

**Gi?i thích:**
- `GetAll()`: L?y t?t c? s?n ph?m
- `Get(id)`: L?y 1 s?n ph?m theo ID
- `GetByCategory()`: L?y s?n ph?m theo phân lo?i
- `Add()`: Thêm s?n ph?m m?i
- `Update()`: C?p nh?t s?n ph?m
- `Delete()`: Xóa s?n ph?m

#### Service 2: CartSvc (Gi? hàng)

**File:** `Services/ICartSvc.cs`
```csharp
namespace ReSole.Services
{
    public interface ICartSvc
    {
        List<Cart> GetCartByKhachHangId(int khachHangId);
        Cart GetByKhachHangIdAndGiayId(int khachHangId, int giayId, string size);
        int GetCartCount(int khachHangId);
        void Add(Cart cart);
        void Update(Cart cart);
        void Delete(int id);
        void DeleteByKhachHangIdAndGiayId(int khachHangId, int giayId, string size);
        void ClearCart(int khachHangId);
    }
}
```

**File:** `Services/CartSvc.cs`
```csharp
using ReSole.Models;

namespace ReSole.Services
{
    public class CartSvc : ICartSvc
    {
        private readonly DataContext _context;

        public CartSvc(DataContext context)
        {
            _context = context;
        }

        public List<Cart> GetCartByKhachHangId(int khachHangId)
        {
            return _context.Carts
                .Where(c => c.KhachHangId == khachHangId)
                .Include(c => c.Giay) // Load Giay data
                .ToList();
        }

        public Cart GetByKhachHangIdAndGiayId(int khachHangId, int giayId, string size)
        {
            return _context.Carts.FirstOrDefault(c =>
                c.KhachHangId == khachHangId &&
                c.GiayId == giayId &&
                c.Size == size);
        }

        public int GetCartCount(int khachHangId)
        {
            return _context.Carts
                .Where(c => c.KhachHangId == khachHangId)
                .Sum(c => c.SoLuong);
        }

        public void Add(Cart cart)
        {
            _context.Carts.Add(cart);
            _context.SaveChanges();
        }

        public void Update(Cart cart)
        {
            _context.Carts.Update(cart);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var cart = _context.Carts.Find(id);
            if (cart != null)
            {
                _context.Carts.Remove(cart);
                _context.SaveChanges();
            }
        }

        public void DeleteByKhachHangIdAndGiayId(int khachHangId, int giayId, string size)
        {
            var cart = GetByKhachHangIdAndGiayId(khachHangId, giayId, size);
            if (cart != null)
            {
                _context.Carts.Remove(cart);
                _context.SaveChanges();
            }
        }

        public void ClearCart(int khachHangId)
        {
            var carts = _context.Carts.Where(c => c.KhachHangId == khachHangId).ToList();
            _context.Carts.RemoveRange(carts);
            _context.SaveChanges();
        }
    }
}
```

**Gi?i thích:**
- `.Include(c => c.Giay)`: Eager loading ð? l?y thông tin s?n ph?m
- `GetCartCount()`: Dùng Sum ð? tính t?ng s? lý?ng nhanh chóng
- `ClearCart()`: Xóa toàn b? cart c?a 1 khách

#### Service 3: KhachhangSvc, DonhangSvc, NguoidungSvc
(Týõng t? cách t?o, xem file ð?y ð? trong repository)

---

## PH?N IV: XÂY D?NG CONTROLLERS

### Bý?c 1: HomeController - Trang Ch?

**File:** `Controllers/HomeController.cs`
```csharp
using Microsoft.AspNetCore.Mvc;
using ReSole.Models;
using ReSole.Services;

namespace ReSole.Controllers
{
    public class HomeController : Controller
    {
        private readonly IGiaySvc _giaySvc;

        public HomeController(IGiaySvc giaySvc)
        {
            _giaySvc = giaySvc;
        }

        // Trang ch? khách hàng
        public IActionResult UserHome()
        {
            // 1. L?y s?n ph?m ð?c bi?t (PhanLoai = 4)
            var specialCollection = _giaySvc.GetByCategory(4);
            
            // 2. L?y 4 s?n ph?m m?i nh?t (tr? special)
            var allProducts = _giaySvc.GetAll()
                .Where(g => g.PhanLoai != 4)
                .OrderByDescending(g => g.Id)
                .Take(4)
                .ToList();

            ViewBag.SpecialCollection = specialCollection;
            ViewBag.LatestProducts = allProducts;

            return View();
        }

        // Trang About
        public IActionResult About()
        {
            return View();
        }

        // Trang Contact
        public IActionResult Contact()
        {
            return View();
        }

        // Trang Privacy
        public IActionResult Privacy()
        {
            return View();
        }

        // Trang Welcome
        public IActionResult Welcome()
        {
            return View();
        }
    }
}
```

**Gi?i thích các Actions:**
- `UserHome()`: L?y special shoes (PhanLoai=4) và 4 s?n ph?m m?i nh?t
  - `ViewBag`: Dùng ð? truy?n d? li?u t? Controller sang View
  - `OrderByDescending(g => g.Id)`: S?p x?p gi?m d?n (m?i nh?t lên ð?u)
  - `.Take(4)`: Ch? l?y 4 s?n ph?m

### Bý?c 2: GiayController - Danh Sách & Chi Ti?t S?n Ph?m

**File:** `Controllers/GiayController.cs`
```csharp
using Microsoft.AspNetCore.Mvc;
using ReSole.Models;
using ReSole.Services;
using ReSole.Helpers;

namespace ReSole.Controllers
{
    public class GiayController : Controller
    {
        private readonly IGiaySvc _giaySvc;
        private readonly IUploadHelper _uploadHelper;

        public GiayController(IGiaySvc giaySvc, IUploadHelper uploadHelper)
        {
            _giaySvc = giaySvc;
            _uploadHelper = uploadHelper;
        }

        // Danh sách s?n ph?m (Shop)
        public IActionResult Index()
        {
            var giays = _giaySvc.GetAll();
            return View(giays);
        }

        // Chi ti?t s?n ph?m
        public IActionResult Details(int id)
        {
            var giay = _giaySvc.Get(id);
            if (giay == null)
                return NotFound();

            return View(giay);
        }

        // T?o s?n ph?m (Admin)
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Giay giay, IFormFile imageFile)
        {
            if (!ModelState.IsValid)
                return View(giay);

            // Upload h?nh ?nh
            if (imageFile != null)
            {
                giay.Hinh = await _uploadHelper.UploadImage(imageFile, "Giay");
            }

            _giaySvc.Add(giay);
            return RedirectToAction("Index");
        }

        // S?a s?n ph?m (Admin)
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var giay = _giaySvc.Get(id);
            if (giay == null)
                return NotFound();

            return View(giay);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Giay giay, IFormFile imageFile)
        {
            var existingGiay = _giaySvc.Get(id);
            if (existingGiay == null)
                return NotFound();

            if (!ModelState.IsValid)
                return View(giay);

            existingGiay.Ten = giay.Ten;
            existingGiay.MoTa = giay.MoTa;
            existingGiay.Gia = giay.Gia;
            existingGiay.PhanLoai = giay.PhanLoai;
            existingGiay.TrangThai = giay.TrangThai;

            // Upload ?nh m?i
            if (imageFile != null)
            {
                existingGiay.Hinh = await _uploadHelper.UploadImage(imageFile, "Giay");
            }

            _giaySvc.Update(existingGiay);
            return RedirectToAction("Index");
        }

        // Xóa s?n ph?m (Admin)
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var giay = _giaySvc.Get(id);
            if (giay != null)
            {
                _giaySvc.Delete(id);
            }

            return RedirectToAction("Index");
        }
    }
}
```

**Gi?i thích:**
- `Create()`: T?o s?n ph?m m?i, upload ?nh
- `Edit()`: C?p nh?t s?n ph?m, có th? thay ð?i ?nh
- `Delete()`: Xóa s?n ph?m

### Bý?c 3: CartController - Gi? Hàng

**File:** `Controllers/CartController.cs`
```csharp
using Microsoft.AspNetCore.Mvc;
using ReSole.Models;
using ReSole.Services;

namespace ReSole.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartSvc _cartSvc;
        private readonly IGiaySvc _giaySvc;

        public CartController(ICartSvc cartSvc, IGiaySvc giaySvc)
        {
            _cartSvc = cartSvc;
            _giaySvc = giaySvc;
        }

        // Xem gi? hàng
        public IActionResult Index()
        {
            var khachHangIdStr = HttpContext.Session.GetString("KH_Id");
            if (string.IsNullOrEmpty(khachHangIdStr))
            {
                return RedirectToAction("Login", "Khachhang");
            }

            int khachHangId = int.Parse(khachHangIdStr);
            var carts = _cartSvc.GetCartByKhachHangId(khachHangId);

            return View(carts);
        }

        // Thêm vào gi? (AJAX)
        [HttpPost]
        public IActionResult Add([FromBody] dynamic request)
        {
            try
            {
                var khachHangIdStr = HttpContext.Session.GetString("KH_Id");
                if (string.IsNullOrEmpty(khachHangIdStr))
                {
                    return Json(new { success = false, message = "Vui l?ng ðãng nh?p" });
                }

                int khachHangId = int.Parse(khachHangIdStr);
                int giayId = request.giayId;
                string size = request.size;
                int quantity = request.quantity;

                // Ki?m tra s?n ph?m
                var giay = _giaySvc.Get(giayId);
                if (giay == null)
                {
                    return Json(new { success = false, message = "S?n ph?m không t?n t?i" });
                }

                // T?m item cùng size trong gi?
                var existingCart = _cartSvc.GetByKhachHangIdAndGiayId(khachHangId, giayId, size);

                if (existingCart != null)
                {
                    // C?ng s? lý?ng
                    existingCart.SoLuong += quantity;
                    _cartSvc.Update(existingCart);
                }
                else
                {
                    // Thêm item m?i
                    var cart = new Cart
                    {
                        KhachHangId = khachHangId,
                        GiayId = giayId,
                        Size = size,
                        SoLuong = quantity
                    };
                    _cartSvc.Add(cart);
                }

                var cartCount = _cartSvc.GetCartCount(khachHangId);

                return Json(new { success = true, cartCount = cartCount });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // C?p nh?t s? lý?ng
        [HttpPost]
        public IActionResult UpdateQuantity([FromBody] dynamic request)
        {
            try
            {
                int cartId = request.cartId;
                int quantity = request.quantity;

                var cart = _cartSvc.Get(cartId);
                if (cart == null)
                    return Json(new { success = false });

                if (quantity > 0)
                {
                    cart.SoLuong = quantity;
                    _cartSvc.Update(cart);
                }
                else
                {
                    _cartSvc.Delete(cartId);
                }

                return Json(new { success = true });
            }
            catch
            {
                return Json(new { success = false });
            }
        }

        // Xóa kh?i gi?
        [HttpPost]
        public IActionResult Remove([FromBody] dynamic request)
        {
            try
            {
                int giayId = request.giayId;
                string size = request.size;

                var khachHangIdStr = HttpContext.Session.GetString("KH_Id");
                int khachHangId = int.Parse(khachHangIdStr);

                _cartSvc.DeleteByKhachHangIdAndGiayId(khachHangId, giayId, size);

                return Json(new { success = true });
            }
            catch
            {
                return Json(new { success = false });
            }
        }

        // Xóa toàn b? gi?
        [HttpPost]
        public IActionResult Clear()
        {
            try
            {
                var khachHangIdStr = HttpContext.Session.GetString("KH_Id");
                int khachHangId = int.Parse(khachHangIdStr);

                _cartSvc.ClearCart(khachHangId);

                return Json(new { success = true });
            }
            catch
            {
                return Json(new { success = false });
            }
        }

        // L?y s? lý?ng items
        [HttpGet]
        public IActionResult GetCount()
        {
            try
            {
                var khachHangIdStr = HttpContext.Session.GetString("KH_Id");
                if (string.IsNullOrEmpty(khachHangIdStr))
                    return Json(new { cartCount = 0 });

                int khachHangId = int.Parse(khachHangIdStr);
                var cartCount = _cartSvc.GetCartCount(khachHangId);

                return Json(new { cartCount = cartCount });
            }
            catch
            {
                return Json(new { cartCount = 0 });
            }
        }
    }
}
```

**Gi?i thích AJAX Requests:**
- `[FromBody]`: L?y d? li?u JSON t? request body
- `Add()`: Thêm vào gi?, ki?m tra duplicate (cùng size)
- `UpdateQuantity()`: C?p nh?t s? lý?ng ho?c xóa n?u = 0
- `GetCount()`: L?y t?ng s? items (dùng cho badge)

---

## PH?N V: T?O VIEWS & LAYOUTS

### Bý?c 1: Layout Chính (_WebLayout.cshtml)

**File:** `Views/Shared/_WebLayout.cshtml`
```html
<!DOCTYPE html>
<html lang="vi">
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>@ViewData["Title"] - ReSole</title>
    
    <!-- Bootstrap CSS -->
    <link href="~/lib/bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <!-- Bootstrap Icons -->
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.0/font/bootstrap-icons.css">
    <!-- Custom CSS -->
    <link rel="stylesheet" href="~/css/site.css" />
</head>
<body>
    <!-- Sticky Navbar -->
    <nav class="navbar navbar-expand-lg rs-sticky-nav">
        <div class="container-xl">
            <a class="navbar-brand" href="/">
                <i class="bi bi-shoe"></i> ReSole
            </a>
            <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarNav">
                <span class="navbar-toggler-icon"></span>
            </button>
            <div class="collapse navbar-collapse" id="navbarNav">
                <ul class="navbar-nav ms-auto">
                    <li class="nav-item">
                        <a class="nav-link" href="/">Trang Ch?</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" href="/Giay/Index">C?a Hàng</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" href="/Home/About">V? Chúng Tôi</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" href="/Home/Contact">Liên H?</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" href="/Cart/Index">
                            <i class="bi bi-cart"></i> Gi? Hàng
                            <span class="badge bg-danger" id="cartBadge">0</span>
                        </a>
                    </li>
                    <!-- Login Partial -->
                    @await Html.PartialAsync("_LoginPartial")
                </ul>
            </div>
        </div>
    </nav>

    <!-- Main Content -->
    <main>
        @RenderBody()
    </main>

    <!-- Footer -->
    <footer class="rs-footer">
        <div class="container-xl">
            <div class="row">
                <div class="col-md-4">
                    <h5>ReSole Sneaker Shop</h5>
                    <p>T?m nh?ng ðôi giày hoàn h?o cho m?i bý?c ði c?a b?n.</p>
                </div>
                <div class="col-md-4">
                    <h5>Liên K?t</h5>
                    <ul>
                        <li><a href="/">Trang Ch?</a></li>
                        <li><a href="/Giay/Index">S?n Ph?m</a></li>
                        <li><a href="/Home/Contact">Liên H?</a></li>
                    </ul>
                </div>
                <div class="col-md-4">
                    <h5>Liên L?c</h5>
                    <p>Email: info@resole.com<br>
                    SÐT: +84 123 456 789</p>
                </div>
            </div>
            <hr />
            <p class="text-center mb-0">&copy; 2025 ReSole. All rights reserved.</p>
        </div>
    </footer>

    <!-- Scripts -->
    <script src="~/lib/bootstrap/js/bootstrap.bundle.min.js"></script>
    <script src="~/js/site.js"></script>
    @await RenderSectionAsync("Scripts", required: false)
</body>
</html>
```

### Bý?c 2: View Danh Sách S?n Ph?m (Giay/Index.cshtml)

**File:** `Views/Giay/Index.cshtml`
```html
@model List<Giay>
@{
    ViewData["Title"] = "C?a Hàng";
    Layout = "~/Views/Shared/_WebLayout.cshtml";
}

<div class="container-xl" style="padding: 40px 0;">
    <h1 class="mb-4">Danh Sách S?n Ph?m</h1>

    <div class="product-grid">
        @foreach (var giay in Model)
        {
            <div class="card product-card">
                <div class="product-image-container">
                    <img src="~/images/Giay/@giay.Hinh" alt="@giay.Ten" class="product-image">
                    <div class="product-overlay">
                        <a href="/Giay/Details/@giay.Id" class="btn btn-sm btn-primary">
                            <i class="bi bi-eye"></i> Xem Chi Ti?t
                        </a>
                    </div>
                </div>
                <div class="card-body">
                    <h5 class="card-title">@giay.Ten</h5>
                    <p class="card-text product-price">
                        <strong>@giay.Gia.ToString("N0") þ</strong>
                    </p>
                    <p class="card-text product-description">@giay.MoTa</p>
                    <div class="d-flex justify-content-between">
                        <button class="btn btn-outline-primary btn-sm" onclick="toggleFavorite(this)">
                            <i class="bi bi-heart"></i>
                        </button>
                        <span class="badge" style="background: @(giay.TrangThai ? "green" : "red");">
                            @(giay.TrangThai ? "C?n Hàng" : "H?t Hàng")
                        </span>
                    </div>
                </div>
            </div>
        }
    </div>
</div>

<style>
    .product-grid {
        display: grid;
        grid-template-columns: repeat(auto-fill, minmax(250px, 1fr));
        gap: 2rem;
    }

    .product-card {
        transition: transform 0.3s, box-shadow 0.3s;
    }

    .product-card:hover {
        transform: translateY(-10px);
        box-shadow: 0 10px 30px rgba(0,0,0,0.2);
    }

    .product-image-container {
        position: relative;
        height: 250px;
        overflow: hidden;
    }

    .product-image {
        width: 100%;
        height: 100%;
        object-fit: cover;
        transition: transform 0.3s;
    }

    .product-card:hover .product-image {
        transform: scale(1.1);
    }

    .product-overlay {
        position: absolute;
        top: 0;
        left: 0;
        right: 0;
        bottom: 0;
        background: rgba(0,0,0,0.5);
        display: flex;
        align-items: center;
        justify-content: center;
        opacity: 0;
        transition: opacity 0.3s;
    }

    .product-card:hover .product-overlay {
        opacity: 1;
    }
</style>

<script>
function toggleFavorite(button) {
    const icon = button.querySelector('i');
    if (icon.classList.contains('bi-heart')) {
        icon.classList.remove('bi-heart');
        icon.classList.add('bi-heart-fill');
        icon.style.color = 'red';
    } else {
        icon.classList.remove('bi-heart-fill');
        icon.classList.add('bi-heart');
        icon.style.color = 'black';
    }
}
</script>
```

### Bý?c 3: View Chi Ti?t S?n Ph?m (Giay/Details.cshtml)

**File:** `Views/Giay/Details.cshtml`
```html
@model Giay
@{
    ViewData["Title"] = @Model.Ten;
    Layout = "~/Views/Shared/_WebLayout.cshtml";
}

<div class="container-xl" style="padding: 40px 0;">
    <div class="row">
        <!-- H?nh ?nh -->
        <div class="col-md-6">
            <img src="~/images/Giay/@Model.Hinh" alt="@Model.Ten" 
                 style="width: 100%; height: 500px; object-fit: cover; border-radius: 10px;">
        </div>

        <!-- Thông tin s?n ph?m -->
        <div class="col-md-6">
            <h1>@Model.Ten</h1>
            <p class="lead">
                <strong style="color: #e74c3c; font-size: 1.5em;">
                    @Model.Gia.ToString("N0") þ
                </strong>
            </p>

            <p class="text-muted">
                <strong>Mô t?:</strong><br>
                @Model.MoTa
            </p>

            <p>
                <strong>Tr?ng thái:</strong>
                <span class="badge" style="background: @(Model.TrangThai ? "green" : "red");">
                    @(Model.TrangThai ? "C?n Hàng" : "H?t Hàng")
                </span>
            </p>

            <!-- Ch?n Size -->
            <div class="mb-3">
                <label class="form-label"><strong>Ch?n Size:</strong></label>
                <div>
                    @foreach (var size in new[] { "39", "40", "41", "42", "43" })
                    {
                        <label class="form-check form-check-inline">
                            <input class="form-check-input size-radio" type="radio" name="size" value="@size">
                            <span class="form-check-label">@size</span>
                        </label>
                    }
                </div>
            </div>

            <!-- S? lý?ng -->
            <div class="mb-3">
                <label class="form-label"><strong>S? lý?ng:</strong></label>
                <input type="number" id="quantity" value="1" min="1" max="10" class="form-control" style="width: 100px;">
            </div>

            <!-- Buttons -->
            <div class="d-grid gap-2 d-md-flex">
                <button class="btn btn-primary btn-lg" onclick="addToCart(@Model.Id)">
                    <i class="bi bi-cart"></i> Thêm Vào Gi?
                </button>
                <button class="btn btn-success btn-lg" onclick="buyNow(@Model.Id)">
                    <i class="bi bi-lightning"></i> Mua Ngay
                </button>
            </div>
        </div>
    </div>
</div>

<script>
function addToCart(giayId) {
    const size = document.querySelector('input[name="size"]:checked')?.value;
    if (!size) {
        alert('Vui l?ng ch?n size');
        return;
    }

    const quantity = parseInt(document.getElementById('quantity').value);

    fetch('/Cart/Add', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ giayId, size, quantity })
    })
    .then(r => r.json())
    .then(data => {
        if (data.success) {
            alert('Ð? thêm vào gi? hàng');
            updateCartBadge(data.cartCount);
        } else {
            alert(data.message || 'Có l?i x?y ra');
        }
    });
}

function buyNow(giayId) {
    const size = document.querySelector('input[name="size"]:checked')?.value;
    if (!size) {
        alert('Vui l?ng ch?n size');
        return;
    }

    const quantity = parseInt(document.getElementById('quantity').value);

    fetch('/Checkout/BuyNow', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ giayId, size, quantity })
    })
    .then(r => r.json())
    .then(data => {
        if (data.success) {
            window.location.href = '/Checkout/Index';
        }
    });
}

function updateCartBadge(count) {
    document.getElementById('cartBadge').textContent = count;
}
</script>
```

---

## PH?N VI: TÍNH NÃNG GI? HÀNG

### Quy Tr?nh Thêm Vào Gi?

```
1. User ch?n size + s? lý?ng
2. Nh?n "Thêm Vào Gi?"
3. JavaScript g?i AJAX POST /Cart/Add
4. CartController.Add():
   a. Ki?m tra login
   b. T?m s?n ph?m cùng size
   c. N?u có ? C?ng s? lý?ng
   d. N?u chýa ? Thêm record m?i
5. Tr? JSON {success: true, cartCount: 3}
6. JavaScript c?p nh?t badge
```

### Code Frontend (JavaScript)

**File:** `wwwroot/js/site.js`
```javascript
// C?p nh?t cart badge khi page load
document.addEventListener('DOMContentLoaded', function() {
    updateCartBadge();
});

function updateCartBadge() {
    fetch('/Cart/GetCount')
        .then(r => r.json())
        .then(data => {
            const badge = document.getElementById('cartBadge');
            if (badge) {
                badge.textContent = data.cartCount;
            }
        });
}

// Scroll event ð? sticky navbar
window.addEventListener('scroll', function() {
    const nav = document.querySelector('.rs-sticky-nav');
    if (window.scrollY > 100) {
        nav.classList.add('scrolled');
    } else {
        nav.classList.remove('scrolled');
    }
});
```

---

## PH?N VII: THANH TOÁN & Ð?T HÀNG

### Bý?c 1: CheckoutController

**File:** `Controllers/CheckoutController.cs`
```csharp
using Microsoft.AspNetCore.Mvc;
using ReSole.Models;
using ReSole.Services;

namespace ReSole.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly ICartSvc _cartSvc;
        private readonly IGiaySvc _giaySvc;
        private readonly IDonHangSvc _donhangSvc;
        private readonly IDonhangChitietSvc _donhangChitietSvc;

        public CheckoutController(
            ICartSvc cartSvc,
            IGiaySvc giaySvc,
            IDonHangSvc donhangSvc,
            IDonhangChitietSvc donhangChitietSvc)
        {
            _cartSvc = cartSvc;
            _giaySvc = giaySvc;
            _donhangSvc = donhangSvc;
            _donhangChitietSvc = donhangChitietSvc;
        }

        // Hi?n th? trang thanh toán
        [HttpGet]
        public IActionResult Index()
        {
            var khachHangIdStr = HttpContext.Session.GetString("KH_Id");
            if (string.IsNullOrEmpty(khachHangIdStr))
                return RedirectToAction("Login", "Khachhang");

            int khachHangId = int.Parse(khachHangIdStr);

            // L?y gi? hàng
            var carts = _cartSvc.GetCartByKhachHangId(khachHangId);
            
            // Tính t?ng ti?n
            decimal totalAmount = carts.Sum(c => (c.Giay.Gia * c.SoLuong));

            ViewBag.CartItems = carts;
            ViewBag.TotalAmount = totalAmount;

            return View();
        }

        // Ð?t hàng
        [HttpPost]
        public IActionResult PlaceOrder(string fullName, string email, string phone, string address)
        {
            var khachHangIdStr = HttpContext.Session.GetString("KH_Id");
            if (string.IsNullOrEmpty(khachHangIdStr))
                return RedirectToAction("Login", "Khachhang");

            int khachHangId = int.Parse(khachHangIdStr);

            // L?y gi? hàng
            var carts = _cartSvc.GetCartByKhachHangId(khachHangId);
            if (carts.Count == 0)
                return RedirectToAction("Index");

            // Tính t?ng ti?n
            decimal totalAmount = carts.Sum(c => (decimal)(c.Giay.Gia * c.SoLuong));

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
            foreach (var cart in carts)
            {
                var chitiet = new DonhangChitiet
                {
                    DonhangId = donhang.Id,
                    GiayId = cart.GiayId,
                    Size = cart.Size,
                    SoLuong = cart.SoLuong,
                    DonGia = (decimal)cart.Giay.Gia
                };

                _donhangChitietSvc.Add(chitiet);
            }

            // Xóa gi? hàng
            _cartSvc.ClearCart(khachHangId);

            // Lýu OrderId vào TempData
            TempData["OrderId"] = donhang.Id;

            return RedirectToAction("Success");
        }

        // Trang thành công
        public IActionResult Success()
        {
            var orderId = TempData["OrderId"];
            if (orderId == null)
                return RedirectToAction("Index");

            return View();
        }
    }
}
```

**Gi?i thích:**
- `Index()`: Hi?n th? form thanh toán v?i gi? hàng
- `PlaceOrder()`: 
  1. L?y gi? hàng
  2. Tính t?ng ti?n
  3. T?o Donhang
  4. T?o DonhangChitiet cho m?i s?n ph?m
  5. Xóa Carts
  6. Lýu OrderId vào TempData
  7. Redirect sang Success
- `Success()`: Trang c?m õn sau ð?t hàng

### Bý?c 2: View Checkout (Checkout/Index.cshtml)

**File:** `Views/Checkout/Index.cshtml`
```html
@{
    ViewData["Title"] = "Thanh Toán";
    Layout = "~/Views/Shared/_WebLayout.cshtml";
}

<div class="container" style="padding: 40px 0;">
    <h1 class="mb-4">Thanh Toán</h1>

    <div class="row">
        <!-- Form -->
        <div class="col-md-6">
            <form method="post" action="/Checkout/PlaceOrder">
                <div class="mb-3">
                    <label class="form-label">H? Tên</label>
                    <input type="text" name="fullName" class="form-control" required>
                </div>
                <div class="mb-3">
                    <label class="form-label">Email</label>
                    <input type="email" name="email" class="form-control" required>
                </div>
                <div class="mb-3">
                    <label class="form-label">S? Ði?n Tho?i</label>
                    <input type="tel" name="phone" class="form-control" required>
                </div>
                <div class="mb-3">
                    <label class="form-label">Ð?a Ch? Giao Hàng</label>
                    <textarea name="address" class="form-control" rows="3" required></textarea>
                </div>
                <button type="submit" class="btn btn-primary btn-lg w-100">
                    <i class="bi bi-check-circle"></i> Ð?t Hàng
                </button>
            </form>
        </div>

        <!-- Ðõn hàng -->
        <div class="col-md-6">
            <div class="card">
                <div class="card-body">
                    <h5 class="card-title">Tóm T?t Ðõn Hàng</h5>
                    <hr>

                    @foreach (var item in ViewBag.CartItems)
                    {
                        <div class="row mb-2">
                            <div class="col-md-8">
                                <p>@item.Giay.Ten (Size @item.Size)<br>
                                   x@item.SoLuong</p>
                            </div>
                            <div class="col-md-4 text-end">
                                <strong>@((item.Giay.Gia * item.SoLuong).ToString("N0")) þ</strong>
                            </div>
                        </div>
                    }

                    <hr>
                    <div class="row">
                        <div class="col-md-8">
                            <h5>T?ng C?ng</h5>
                        </div>
                        <div class="col-md-4 text-end">
                            <h5>@ViewBag.TotalAmount.ToString("N0") þ</h5>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
```

---

## PH?N VIII: QU?N L? ADMIN

### Bý?c 1: Admin Authentication

**File:** `Filters/AuthenticationFilterAttribute.cs`
```csharp
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ReSole.Filters
{
    public class AuthenticationFilterAttribute : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            // Ki?m tra session admin
            var username = context.HttpContext.Session.GetString("ND_Username");
            
            if (string.IsNullOrEmpty(username))
            {
                // Redirect sang login
                context.Result = new RedirectToActionResult("Login", "Admin", null);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}
```

### Bý?c 2: Admin Controller

**File:** `Controllers/AdminController.cs`
```csharp
using Microsoft.AspNetCore.Mvc;
using ReSole.Services;
using ReSole.Filters;
using ReSole.Helpers;

namespace ReSole.Controllers
{
    public class AdminController : Controller
    {
        private readonly INguoidungSvc _nguoidungSvc;
        private readonly IMahoaHelper _mahoaHelper;

        public AdminController(INguoidungSvc nguoidungSvc, IMahoaHelper mahoaHelper)
        {
            _nguoidungSvc = nguoidungSvc;
            _mahoaHelper = mahoaHelper;
        }

        // Trang login admin
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var user = _nguoidungSvc.GetByUsername(username);

            if (user != null && user.Password == _mahoaHelper.MaHoaMD5(password))
            {
                // Lýu session
                HttpContext.Session.SetString("ND_Username", user.Username);
                HttpContext.Session.SetString("ND_HoTen", user.HoTen);

                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Tên ðãng nh?p ho?c m?t kh?u sai");
            return View();
        }

        // Dashboard Admin
        [AuthenticationFilter]
        public IActionResult Index()
        {
            return View();
        }

        // Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
```

**Gi?i thích:**
- `[AuthenticationFilter]`: Decorator b?o v? trang
- `Login()`: Validate username + password (MD5)
- `Logout()`: Clear session

### Bý?c 3: Giay Controller (Admin)

**File:** `Controllers/GiayController.cs` (ph?n Admin)
```csharp
// Danh sách s?n ph?m (Admin)
[AuthenticationFilter]
public IActionResult Index()
{
    var giays = _giaySvc.GetAll();
    return View(giays);
}

// Thêm s?n ph?m
[AuthenticationFilter]
[HttpPost]
public async Task<IActionResult> Create(Giay giay, IFormFile imageFile)
{
    if (!ModelState.IsValid)
        return View(giay);

    if (imageFile != null)
    {
        giay.Hinh = await _uploadHelper.UploadImage(imageFile, "Giay");
    }

    _giaySvc.Add(giay);
    
    // Toast notification
    TempData["success"] = "Thêm s?n ph?m thành công";
    
    return RedirectToAction("Index");
}

// S?a s?n ph?m
[AuthenticationFilter]
[HttpPost]
public async Task<IActionResult> Edit(int id, Giay giay, IFormFile imageFile)
{
    var existingGiay = _giaySvc.Get(id);
    if (existingGiay == null)
        return NotFound();

    existingGiay.Ten = giay.Ten;
    existingGiay.MoTa = giay.MoTa;
    existingGiay.Gia = giay.Gia;
    existingGiay.PhanLoai = giay.PhanLoai;
    existingGiay.TrangThai = giay.TrangThai;

    if (imageFile != null)
    {
        existingGiay.Hinh = await _uploadHelper.UploadImage(imageFile, "Giay");
    }

    _giaySvc.Update(existingGiay);
    
    TempData["success"] = "C?p nh?t s?n ph?m thành công";
    
    return RedirectToAction("Index");
}

// Xóa s?n ph?m
[AuthenticationFilter]
[HttpPost]
public IActionResult Delete(int id)
{
    var giay = _giaySvc.Get(id);
    if (giay != null)
    {
        _giaySvc.Delete(id);
        TempData["success"] = "Xóa s?n ph?m thành công";
    }

    return RedirectToAction("Index");
}
```

---

## PH?N IX: STYLING & UX

### Bý?c 1: CSS Global (site.css)

**File:** `wwwroot/css/site.css`
```css
/* Sticky Navbar */
.rs-sticky-nav {
    position: sticky;
    top: 0;
    z-index: 1000;
    background: linear-gradient(135deg, rgba(7,12,24,0.95) 0%, rgba(18,26,44,0.9) 60%, rgba(7,12,24,0.95) 100%);
    backdrop-filter: blur(10px);
    padding: 0.75rem 0;
    transition: all 0.3s ease;
    box-shadow: 0 2px 10px rgba(0,0,0,0.1);
}

.rs-sticky-nav.scrolled {
    background: linear-gradient(135deg, rgba(7,12,24,0.98) 0%, rgba(18,26,44,0.95) 60%, rgba(7,12,24,0.98) 100%);
    box-shadow: 0 4px 20px rgba(0,0,0,0.3);
    padding: 0.5rem 0;
}

/* Navigation Links */
.rs-nav-link {
    color: #ffffff !important;
    text-transform: uppercase;
    font-size: 0.95rem;
    letter-spacing: 1px;
    font-weight: 400;
    transition: all 0.3s ease;
}

.rs-nav-link:hover {
    color: #ffffff !important;
    transform: translateY(-2px);
}

/* Cards */
.card {
    border-radius: 0.75rem !important;
    box-shadow: 0 10px 30px rgba(0,0,0,0.08) !important;
    border: 1px solid #e5e7eb !important;
    background: #ffffff !important;
}

.card:hover {
    box-shadow: 0 14px 40px rgba(0,0,0,0.12) !important;
}

/* Buttons */
.btn {
    border-radius: 0.5rem;
    font-weight: 600;
    padding: 0.6rem 1.2rem;
    transition: all 0.2s ease;
}

.btn-primary {
    background: #111827;
    border-color: #111827;
    color: #ffffff !important;
}

.btn-primary:hover {
    background: #0f172a;
    border-color: #0f172a;
    box-shadow: 0 10px 30px rgba(17,24,39,0.25);
}

/* Forms */
.form-control,
.form-select {
    border-radius: 0.5rem;
    border: 1px solid #d1d5db;
    padding: 0.75rem 1rem;
}

.form-control:focus,
.form-select:focus {
    border-color: #111827;
    box-shadow: 0 0 0 0.2rem rgba(17,24,39,0.15);
}

/* Footer */
.rs-footer {
    background: linear-gradient(135deg, #1a1a1a 0%, #2d2d2d 100%);
    color: #e5e7eb;
    padding: 2rem 0;
}

/* Responsive */
@media (max-width: 768px) {
    .rs-sticky-nav {
        padding: 0.5rem 0;
    }

    .product-grid {
        grid-template-columns: repeat(auto-fill, minmax(200px, 1fr)) !important;
    }
}
```

### Bý?c 2: Toast Notifications (toasts.js)

**File:** `wwwroot/js/toasts.js`
```javascript
// Hi?n th? toast notification
function showToast(message, type = 'success') {
    const container = document.getElementById('toastContainer') || createToastContainer();
    
    const toast = document.createElement('div');
    toast.className = `alert alert-${type} alert-dismissible fade show`;
    toast.innerHTML = `
        ${message}
        <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
    `;
    
    container.appendChild(toast);
    
    // T? ð?ng ?n sau 3 giây
    setTimeout(() => {
        toast.remove();
    }, 3000);
}

function createToastContainer() {
    const container = document.createElement('div');
    container.id = 'toastContainer';
    container.style.cssText = 'position: fixed; top: 20px; right: 20px; z-index: 9999;';
    document.body.appendChild(container);
    return container;
}
```

---

## PH?N X: HELPERS

### Bý?c 1: MahoaHelper (M? hóa MD5)

**File:** `Helpers/MahoaHelper.cs`
```csharp
using System.Security.Cryptography;
using System.Text;

namespace ReSole.Helpers
{
    public interface IMahoaHelper
    {
        string MaHoaMD5(string input);
    }

    public class MahoaHelper : IMahoaHelper
    {
        public string MaHoaMD5(string input)
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
}
```

### Bý?c 2: UploadHelper (Upload H?nh ?nh)

**File:** `Helpers/UploadHelper.cs`
```csharp
namespace ReSole.Helpers
{
    public interface IUploadHelper
    {
        Task<string> UploadImage(IFormFile file, string folder);
    }

    public class UploadHelper : IUploadHelper
    {
        public async Task<string> UploadImage(IFormFile file, string folder)
        {
            // T?o tên file unique
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            
            // T?o ðý?ng d?n
            var path = Path.Combine("wwwroot/images", folder, fileName);
            
            // T?o thý m?c n?u chýa t?n t?i
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            
            // Upload file
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            
            // Tr? v? tên file
            return fileName;
        }
    }
}
```

### Bý?c 3: CartHelper (Session Extensions)

**File:** `Helpers/CartHelper.cs`
```csharp
using System.Text.Json;

namespace ReSole.Helpers
{
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
}
```

---

## ?? TÓMO T?T QUY TR?NH CHÍNH

### 1?? Mua Hàng

```
Ch?n S?n Ph?m 
  ?
Ch?n Size + S? Lý?ng 
  ?
Nh?n "Thêm Vào Gi?" 
  ?
AJAX ? /Cart/Add 
  ?
Ki?m tra Login & Duplicate 
  ?
Thêm/C?ng vào Carts table 
  ?
Update Cart Badge 
  ?
? Thành công
```

### 2?? Thanh Toán

```
Xem Gi? Hàng 
  ?
Nh?n "Checkout" 
  ?
? /Checkout/Index 
  ?
Load Gi? + Tính T?ng Ti?n 
  ?
Ði?n Thông Tin Giao Hàng 
  ?
Nh?n "Ð?t Hàng" 
  ?
T?o Donhang + DonhangChitiet 
  ?
Xóa Carts 
  ?
? /Checkout/Success 
  ?
? Thành công
```

### 3?? Qu?n L? Admin

```
? /Admin/Login 
  ?
Nh?p Username + Password 
  ?
Validate MD5 
  ?
Lýu Session 
  ?
? /Admin/Index 
  ?
[AuthenticationFilter] b?o v? 
  ?
? Truy c?p ðý?c
```

---

## ?? CHECKLIST IMPLEMENTATION

- ? T?o Models & DataContext
- ? T?o Services Layer
- ? T?o Controllers
- ? T?o Views & Layouts
- ? Implement Cart Logic
- ? Implement Checkout
- ? Implement Admin
- ? Styling & UX
- ? Toast Notifications
- ? Helpers

---

## ?? TÀI LI?U THAM KH?O

- [ASP.NET Core Docs](https://docs.microsoft.com/aspnet/core)
- [Entity Framework Core](https://docs.microsoft.com/ef/core)
- [Bootstrap 5](https://getbootstrap.com/docs/5.0)
- [jQuery](https://api.jquery.com)

---

**Phiên b?n:** 2.0  
**C?p nh?t:** 2025  
**Tr?ng thái:** ? Hoàn thành

