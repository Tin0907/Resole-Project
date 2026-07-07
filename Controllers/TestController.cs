using ASM.Helpers;
using ASM.Models;
using ASM.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASM.Controllers
{
    public class TestController : Controller
    {
        private readonly IMahoaHelper _mahoaHelper;
        private readonly DataContext _context;
        private readonly INguoidungSvc _nguoidungSvc;
        private readonly IKhachhangSvc _khachhangSvc;

        public TestController(IMahoaHelper mahoaHelper, DataContext context, INguoidungSvc nguoidungSvc, IKhachhangSvc khachhangSvc)
        {
            _mahoaHelper = mahoaHelper;
            _context = context;
            _nguoidungSvc = nguoidungSvc;
            _khachhangSvc = khachhangSvc;
        }

        // Test 1: /Test/HashPassword?password=123456
        public IActionResult HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                return Content("Vui l?ng cung c?p password. Ví d?: /Test/HashPassword?password=123456");
            }

            string hashed = _mahoaHelper.Mahoa(password);
            
            return Content($@"
                <h2>K?t qu? m? hóa MD5</h2>
                <p><strong>M?t kh?u g?c:</strong> {password}</p>
                <p><strong>M?t kh?u ð? m? hóa:</strong> {hashed}</p>
                <p><strong>Ð? dài:</strong> {hashed.Length} k? t?</p>
                <hr>
                <p>Copy ðo?n SQL sau ð? thêm vào database:</p>
                <textarea style='width:100%; height:150px;'>
UPDATE Nguoidungs 
SET Password = '{hashed}'
WHERE Email = 'a@gmail.com';
                </textarea>
            ", "text/html");
        }

        // Test 2: /Test/CheckDatabase
        public async Task<IActionResult> CheckDatabase()
        {
            try
            {
                var users = await _context.Nguoidungs.ToListAsync();
                
                var html = "<h2>Danh sách User trong Database</h2>";
                html += "<table border='1' cellpadding='10' style='border-collapse:collapse; width:100%;'>";
                html += "<tr style='background:#f0f0f0;'><th>ID</th><th>User</th><th>HoTen</th><th>Email</th><th>Admin</th><th>Locked</th><th>Password</th><th>Pass Length</th></tr>";
                
                foreach (var user in users)
                {
                    var lockedStatus = user.Locked ? "? Kích ho?t" : "? B? khóa";
                    var adminStatus = user.Admin ? "? Admin" : "? User";
                    html += $"<tr>";
                    html += $"<td>{user.NguoiDungID}</td>";
                    html += $"<td>{user.User}</td>";
                    html += $"<td>{user.HoTen}</td>";
                    html += $"<td>{user.Email}</td>";
                    html += $"<td>{adminStatus}</td>";
                    html += $"<td>{lockedStatus}</td>";
                    html += $"<td>{user.Password}</td>";
                    html += $"<td>{user.Password?.Length ?? 0}</td>";
                    html += $"</tr>";
                }
                
                html += "</table>";
                html += $"<p><strong>T?ng s? users:</strong> {users.Count}</p>";
                
                if (!users.Any())
                {
                    html += "<hr><p style='color:red;'><strong>?? KHÔNG CÓ USER NÀO TRONG DATABASE!</strong></p>";
                    html += "<p>Ch?y script SQL sau ð? t?o user test:</p>";
                    html += @"<textarea style='width:100%; height:200px;'
INSERT INTO Nguoidungs ([User], HoTen, Email, ChucDanh, NgaySinh, Admin, Locked, Password)
VALUES 
('admin', 'Admin Test', 'a@gmail.com', 'Administrator', '1990-01-01', 1, 1, 'E10ADC3949BA59ABBE56E057F20F883E');
                    </textarea>";
                }
                
                return Content(html, "text/html");
            }
            catch (Exception ex)
            {
                return Content($@"
                    <h2 style='color:red;'>L?i k?t n?i Database!</h2>
                    <p><strong>L?i:</strong> {ex.Message}</p>
                    <p><strong>Inner Exception:</strong> {ex.InnerException?.Message}</p>
                    <hr>
                    <p>Ki?m tra:</p>
                    <ol>
                        <li>SQL Server có ðang ch?y không?</li>
                        <li>Connection string trong appsettings.json ðúng không?</li>
                        <li>Database 'QuanLyMonAn' ð? ðý?c t?o chýa?</li>
                        <li>Ð? ch?y migrations chýa? (Update-Database)</li>
                    </ol>
                ", "text/html");
            }
        }

        // Test 3: /Test/TestLogin?email=a@gmail.com&password=123456
        public async Task<IActionResult> TestLogin(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                return Content("Vui l?ng cung c?p email và password. Ví d?: /Test/TestLogin?email=a@gmail.com&password=123456");
            }

            try
            {
                var html = "<h2>Test Login Debug</h2>";
                
                // Step 1: Hash password
                string hashedPassword = _mahoaHelper.Mahoa(password);
                html += $"<p><strong>1. M?t kh?u g?c:</strong> {password}</p>";
                html += $"<p><strong>2. M?t kh?u ð? m? hóa:</strong> {hashedPassword}</p>";
                
                // Step 2: Find user by email
                var userByEmail = await _context.Nguoidungs
                    .Where(p => p.Email.ToLower() == email.ToLower())
                    .FirstOrDefaultAsync();
                
                html += "<hr>";
                if (userByEmail != null)
                {
                    html += $"<p style='color:green;'>? <strong>3. T?m th?y user v?i email:</strong> {email}</p>";
                    html += $"<p><strong>Username:</strong> {userByEmail.User}</p>";
                    html += $"<p><strong>H? tên:</strong> {userByEmail.HoTen}</p>";
                    html += $"<p><strong>Password trong DB:</strong> {userByEmail.Password}</p>";
                    html += $"<p><strong>Admin:</strong> {(userByEmail.Admin ? "? Có" : "? Không")}</p>";
                    html += $"<p><strong>Locked (Kích ho?t):</strong> {(userByEmail.Locked ? "? Ðang ho?t ð?ng" : "? B? khóa")}</p>";
                    
                    // Step 3: Compare passwords
                    html += "<hr>";
                    bool passwordMatch = userByEmail.Password == hashedPassword;
                    if (passwordMatch)
                    {
                        html += "<p style='color:green; font-size:20px;'>? <strong>4. M?T KH?U KH?P!</strong></p>";
                        
                        if (!userByEmail.Locked)
                        {
                            html += "<p style='color:red;'>?? NHÝNG TÀI KHO?N B? KHÓA (Locked = false)!</p>";
                            html += "<p>Ch?y SQL sau ð? m? khóa:</p>";
                            html += $"<textarea style='width:100%; height:80px;'>UPDATE Nguoidungs SET Locked = 1 WHERE Email = '{email}';</textarea>";
                        }
                        else
                        {
                            html += "<p style='color:green; font-size:18px;'>?? <strong>T?T C? Ð?U ÐÚNG! LOGIN NÊN THÀNH CÔNG!</strong></p>";
                        }
                    }
                    else
                    {
                        html += "<p style='color:red; font-size:20px;'>? <strong>4. M?T KH?U KHÔNG KH?P!</strong></p>";
                        html += "<p>Ch?y SQL sau ð? c?p nh?t m?t kh?u:</p>";
                        html += $"<textarea style='width:100%; height:80px;'>UPDATE Nguoidungs SET Password = '{hashedPassword}' WHERE Email = '{email}';</textarea>";
                    }
                }
                else
                {
                    html += $"<p style='color:red;'>? <strong>3. KHÔNG T?M TH?Y USER v?i email:</strong> {email}</p>";
                    html += "<p>Ch?y SQL sau ð? t?o user:</p>";
                    html += $@"<textarea style='width:100%; height:150px;'
INSERT INTO Nguoidungs ([User], HoTen, Email, ChucDanh, NgaySinh, Admin, Locked, Password)
VALUES ('admin', 'Admin Test', '{email}', 'Administrator', '1990-01-01', 1, 1, '{hashedPassword}');
                    </textarea>";
                }
                
                // Step 4: Test using service
                html += "<hr><h3>Test qua Service Layer:</h3>";
                var viewLogin = new ViewLogin { Email = email, Password = password };
                var result = _nguoidungSvc.Login(viewLogin);
                
                if (result != null)
                {
                    html += $"<p style='color:green;'>? <strong>Service Login thành công!</strong></p>";
                    html += $"<p>Username: {result.User}, H? tên: {result.HoTen}</p>";
                }
                else
                {
                    html += $"<p style='color:red;'>? <strong>Service Login th?t b?i!</strong></p>";
                }
                
                return Content(html, "text/html");
            }
            catch (Exception ex)
            {
                return Content($@"
                    <h2 style='color:red;'>L?i!</h2>
                    <p><strong>Message:</strong> {ex.Message}</p>
                    <p><strong>Stack Trace:</strong></p>
                    <pre>{ex.StackTrace}</pre>
                ", "text/html");
            }
        }

        // Test 4: /Test/CreateTestUser
        public async Task<IActionResult> CreateTestUser()
        {
            try
            {
                // Check if user exists
                var existingUser = await _context.Nguoidungs
                    .FirstOrDefaultAsync(u => u.Email == "a@gmail.com");
                
                if (existingUser != null)
                {
                    return Content($@"
                        <h2>User ð? t?n t?i!</h2>
                        <p><strong>Email:</strong> {existingUser.Email}</p>
                        <p><strong>Username:</strong> {existingUser.User}</p>
                        <p><strong>Password Hash:</strong> {existingUser.Password}</p>
                        <p><strong>Locked:</strong> {existingUser.Locked}</p>
                        <hr>
                        <p>N?u mu?n t?o l?i, xóa user này trý?c b?ng SQL:</p>
                        <textarea style='width:100%; height:60px;'>DELETE FROM Nguoidungs WHERE Email = 'a@gmail.com';</textarea>
                    ", "text/html");
                }
                
                // Create new user
                var newUser = new NguoiDung
                {
                    User = "admin",
                    HoTen = "Admin Test",
                    Email = "a@gmail.com",
                    ChucDanh = "Administrator",
                    NgaySinh = new DateTime(1990, 1, 1),
                    Admin = true,
                    Locked = true,
                    Password = _mahoaHelper.Mahoa("123456") // Hash password "123456"
                };
                
                _context.Nguoidungs.Add(newUser);
                await _context.SaveChangesAsync();
                
                return Content($@"
                    <h2 style='color:green;'>? T?o user thành công!</h2>
                    <p><strong>Email:</strong> a@gmail.com</p>
                    <p><strong>Password:</strong> 123456</p>
                    <p><strong>Username:</strong> admin</p>
                    <p><strong>Password Hash:</strong> {newUser.Password}</p>
                    <hr>
                    <p><a href='/Admin/Login'>Ði ð?n trang ðãng nh?p</a></p>
                ", "text/html");
            }
            catch (Exception ex)
            {
                return Content($@"
                    <h2 style='color:red;'>L?i khi t?o user!</h2>
                    <p><strong>Message:</strong> {ex.Message}</p>
                    <p><strong>Inner Exception:</strong> {ex.InnerException?.Message}</p>
                ", "text/html");
            }
        }

        // Test 5: /Test/Index - Dashboard
        public IActionResult Index()
        {
            return Content(@"
                <h1>Test Dashboard</h1>
                <h3>Các test có s?n:</h3>
                <ol style='font-size:16px; line-height:2;'>
                    <li><a href='/Test/CheckDatabase'>Check Database</a> - Xem t?t c? users trong database</li>
                    <li><a href='/Test/HashPassword?password=123456'>Hash Password</a> - M? hóa m?t kh?u</li>
                    <li><a href='/Test/TestLogin?email=a@gmail.com&password=123456'>Test Login</a> - Test ðãng nh?p chi ti?t</li>
                    <li><a href='/Test/CreateTestUser'>Create Test User</a> - T?o user test t? ð?ng</li>
                    <li><a href='/Test/TestRegisterCustomer'>Test Register Customer</a> - Test ðãng k? khách hàng</li>
                    <li><a href='/Test/CheckCustomers'>Check Customers</a> - Xem danh sách khách hàng</li>
                </ol>
                <hr>
                <p><strong>Hý?ng d?n:</strong></p>
                <ol>
                    <li>Ch?y <strong>Check Database</strong> ð? xem có users không</li>
                    <li>N?u không có, ch?y <strong>Create Test User</strong></li>
                    <li>Ch?y <strong>Test Login</strong> ð? xem chi ti?t l?i</li>
                    <li>Sau ðó th? ðãng nh?p t?i <a href='/Admin/Login'>/Admin/Login</a></li>
                </ol>
            ", "text/html");
        }

        // Test 6: /Test/TestRegisterCustomer
        public async Task<IActionResult> TestRegisterCustomer()
        {
            try
            {
                var testCustomer = new KhachHang
                {
                    Fullname = "Test Customer",
                    Email = "test@customer.com",
                    PhoneNumber = "0123456789",
                    NgaySinh = new DateTime(1995, 5, 15),
                    Password = "123456",
                    ConfirmPassword = "123456"
                };

                var html = "<h2>Test Ðãng K? Khách Hàng</h2>";
                
                // Test validation
                html += "<h3>1. Ki?m tra d? li?u:</h3>";
                html += $"<p>Fullname: {testCustomer.Fullname}</p>";
                html += $"<p>Email: {testCustomer.Email}</p>";
                html += $"<p>PhoneNumber: {testCustomer.PhoneNumber}</p>";
                html += $"<p>Password: {testCustomer.Password}</p>";
                html += $"<p>ConfirmPassword: {testCustomer.ConfirmPassword}</p>";
                
                // Check if email exists
                var existingCustomer = _context.KhachHangs.FirstOrDefault(k => k.Email == testCustomer.Email);
                if (existingCustomer != null)
                {
                    html += "<p style='color:red;'>?? Email ð? t?n t?i! Xóa trý?c:</p>";
                    html += $"<form method='post' action='/Test/DeleteCustomer?email={testCustomer.Email}'>";
                    html += "<button type='submit' class='btn btn-danger'>Xóa customer c?</button>";
                    html += "</form>";
                }
                else
                {
                    html += "<p style='color:green;'>? Email chýa t?n t?i</p>";
                    
                    // Try to add
                    html += "<h3>2. Th? thêm vào database:</h3>";
                    int result = _khachhangSvc.AddKhachhang(testCustomer);
                    
                    if (result > 0)
                    {
                        html += $"<p style='color:green; font-size:20px;'>? <strong>ÐÃNG K? THÀNH CÔNG!</strong></p>";
                        html += $"<p>Customer ID: {result}</p>";
                        html += "<p>Bây gi? có th? ðãng nh?p t?i:</p>";
                        html += "<a href='/Khachhang/Login' class='btn btn-success'>Ðãng nh?p ngay</a>";
                    }
                    else
                    {
                        html += "<p style='color:red; font-size:20px;'>? <strong>ÐÃNG K? TH?T B?I!</strong></p>";
                        html += "<p>Ki?m tra log trong Output window ð? xem l?i chi ti?t</p>";
                    }
                }
                
                return Content(html, "text/html");
            }
            catch (Exception ex)
            {
                return Content($@"
                    <h2 style='color:red;'>L?i!</h2>
                    <p><strong>Message:</strong> {ex.Message}</p>
                    <p><strong>Inner Exception:</strong> {ex.InnerException?.Message}</p>
                    <p><strong>Stack Trace:</strong></p>
                    <pre>{ex.StackTrace}</pre>
                ", "text/html");
            }
        }

        // Test 7: /Test/CheckCustomers
        public async Task<IActionResult> CheckCustomers()
        {
            try
            {
                var customers = await _context.KhachHangs.ToListAsync();
                
                var html = "<h2>Danh sách Khách Hàng</h2>";
                html += "<table border='1' cellpadding='10' style='border-collapse:collapse; width:100%;'>";
                html += "<tr style='background:#f0f0f0;'><th>ID</th><th>H? tên</th><th>Email</th><th>SÐT</th><th>Ngày sinh</th><th>Password</th><th>Pass Length</th></tr>";
                
                foreach (var kh in customers)
                {
                    html += $"<tr>";
                    html += $"<td>{kh.Id}</td>";
                    html += $"<td>{kh.Fullname}</td>";
                    html += $"<td>{kh.Email}</td>";
                    html += $"<td>{kh.PhoneNumber}</td>";
                    html += $"<td>{kh.NgaySinh?.ToString("dd/MM/yyyy")}</td>";
                    html += $"<td>{kh.Password}</td>";
                    html += $"<td>{kh.Password?.Length ?? 0}</td>";
                    html += $"</tr>";
                }
                
                html += "</table>";
                html += $"<p><strong>T?ng s? khách hàng:</strong> {customers.Count}</p>";
                
                if (!customers.Any())
                {
                    html += "<hr><p style='color:red;'><strong>?? KHÔNG CÓ KHÁCH HÀNG NÀO!</strong></p>";
                    html += "<p><a href='/Test/TestRegisterCustomer'>T?o khách hàng test</a></p>";
                }
                
                return Content(html, "text/html");
            }
            catch (Exception ex)
            {
                return Content($@"
                    <h2 style='color:red;'>L?i k?t n?i Database!</h2>
                    <p><strong>L?i:</strong> {ex.Message}</p>
                    <p><strong>Inner Exception:</strong> {ex.InnerException?.Message}</p>
                ", "text/html");
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCustomer(string email)
        {
            var customer = await _context.KhachHangs.FirstOrDefaultAsync(k => k.Email == email);
            if (customer != null)
            {
                _context.KhachHangs.Remove(customer);
                await _context.SaveChangesAsync();
                return Content($"<h2>Ð? xóa customer: {email}</h2><p><a href='/Test/TestRegisterCustomer'>Th? ðãng k? l?i</a></p>", "text/html");
            }
            return Content($"<h2>Không t?m th?y customer: {email}</h2>", "text/html");
        }
    }
}
