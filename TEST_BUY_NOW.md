# Test Tính Nãng Buy Now

## Các bý?c test:

### 1. Test Basic - Ki?m tra Buy Now có g?i ðúng API không

1. M? trang chi ti?t s?n ph?m b?t k?
2. M? Developer Tools (F12) ? Tab "Network"
3. Ch?n size và s? lý?ng
4. B?m "Buy Now"
5. **Ki?m tra Network:**
   - Ph?i có request POST ð?n `/Checkout/BuyNow`
   - Response ph?i là: `{"success": true}`

### 2. Test Redirect - Ki?m tra có chuy?n ð?n Checkout không

1. Sau khi b?m "Buy Now" và nh?n response success
2. **URL trên tr?nh duy?t ph?i là:** `https://localhost:xxxx/Checkout`
3. **KHÔNG ðý?c là:** `https://localhost:xxxx/Cart`

### 3. Test Display - Ki?m tra hi?n th? ðúng

1. Khi ð? vào trang Checkout
2. **Ph?i th?y:**
   - Tiêu ð? "Payment Information"
   - Form nh?p ð?a ch? (Delivery Address)
   - **Ch? 1 s?n ph?m** trong Product list (s?n ph?m v?a ch?n)
   - Ðúng size và s? lý?ng ð? ch?n

### 4. Test Order - Ki?m tra ð?t hàng

1. Ði?n ð?y ð? thông tin:
   - Street Address
   - City
   - Phone Number
   - Email
2. B?m "Place Order"
3. **K?t qu?:**
   - Chuy?n ð?n trang Success
   - Ðõn hàng ðý?c t?o trong database
   - **Gi? hàng KHÔNG b? xóa**

### 5. Test v?i Gi? Hàng - So sánh

**Add to Cart:**
1. Thêm s?n ph?m vào gi? hàng
2. Vào trang Cart
3. B?m "Checkout"
4. **K?t qu?:** Hi?n th? T?T C? s?n ph?m trong gi?

**Buy Now:**
1. Ch?n s?n ph?m và b?m "Buy Now"
2. Chuy?n th?ng ð?n Checkout
3. **K?t qu?:** Ch? hi?n th? 1 s?n ph?m v?a ch?n

## Ki?m tra Session

### Cách 1: Developer Tools
1. F12 ? Tab "Application"
2. Storage ? Cookies
3. T?m cookie `.AspNetCore.Session`
4. S? th?y m?t chu?i Session ID

### Cách 2: Code
Thêm logging vào CheckoutController.Index:

```csharp
[HttpGet]
public IActionResult Index()
{
    var khId = HttpContext.Session.GetInt32(SessionKey.KhachHang.KH_Id);
    var buyNowJson = HttpContext.Session.GetString("BuyNowItem");
    
    Console.WriteLine($"Checkout.Index called");
    Console.WriteLine($"KhachHangId: {khId}");
    Console.WriteLine($"BuyNowItem: {buyNowJson ?? "NULL"}");
    
    // ...existing code...
}
```

## Troubleshooting

### V?n ð? 1: B?m Buy Now nhýng không chuy?n trang

**Ki?m tra:**
1. Console log có l?i không?
2. Network tab có request POST ð?n `/Checkout/BuyNow` không?
3. Response có success = true không?

**Nguyên nhân:**
- JavaScript l?i
- API tr? v? error
- URL không ðúng

**Gi?i pháp:**
- M? Console và xem l?i
- Ki?m tra URL trong fetch: `/Checkout/BuyNow`

### V?n ð? 2: Chuy?n ð?n Cart thay v? Checkout

**Ki?m tra:**
- JavaScript trong Details.cshtml có ðúng không:
```javascript
window.location.href = '/Checkout';  // Ph?i là /Checkout, KHÔNG ph?i /Cart
```

### V?n ð? 3: Checkout hi?n th? gi? hàng thay v? 1 s?n ph?m

**Ki?m tra:**
1. Session có BuyNowItem không:
```csharp
var buyNowJson = HttpContext.Session.GetString("BuyNowItem");
Console.WriteLine($"BuyNowItem in session: {buyNowJson}");
```

2. ViewBag.IsBuyNow có = true không:
```csharp
Console.WriteLine($"IsBuyNow: {ViewBag.IsBuyNow}");
```

**Nguyên nhân:**
- Session b? m?t
- CheckoutController.Index không ð?c ðý?c Session
- Logic if/else sai

**Gi?i pháp:**
- Ki?m tra Session timeout
- Ð?m b?o `app.UseSession()` ðý?c g?i trong Program.cs
- Thêm logging ð? debug

### V?n ð? 4: Ð?t hàng b? l?i

**Ki?m tra:**
- PlaceOrder có nh?n ðúng isBuyNow = true không?
- buyNowData có giá tr? không?

**Debug:**
```csharp
Console.WriteLine($"PlaceOrder - isBuyNow: {isBuyNow}");
Console.WriteLine($"PlaceOrder - buyNowData: {buyNowData ?? "NULL"}");
```

## Expected Results

### Buy Now Flow
```
1. Details Page ? Ch?n s?n ph?m, size, s? lý?ng
2. B?m "Buy Now" ? POST /Checkout/BuyNow
3. Response: {"success": true}
4. Redirect ? /Checkout
5. Checkout Page ? Hi?n th? 1 s?n ph?m + form nh?p ð?a ch?
6. Ði?n thông tin ? B?m "Place Order"
7. POST /Checkout/PlaceOrder v?i isBuyNow=true
8. T?o ðõn hàng, KHÔNG xóa gi? hàng
9. Redirect ? /Checkout/Success
```

### Add to Cart Flow
```
1. Details Page ? Ch?n s?n ph?m, size, s? lý?ng
2. B?m "Add to Cart" ? POST /Cart/Add
3. Hi?n th? notification thành công
4. Badge gi? hàng c?p nh?t
5. User t? vào /Cart
6. B?m "Checkout" ? /Checkout
7. Hi?n th? T?T C? s?n ph?m trong gi? + form
8. POST /Checkout/PlaceOrder v?i isBuyNow=false
9. T?o ðõn hàng, XÓA gi? hàng
10. Redirect ? /Checkout/Success
```

## Quick Test Script

Copy và paste vào Console (F12) khi ðang ? trang chi ti?t s?n ph?m:

```javascript
// Test Buy Now
fetch('/Checkout/BuyNow', {
    method: 'POST',
    headers: {
        'Content-Type': 'application/json',
    },
    body: JSON.stringify({
        giayId: 1,  // Thay b?ng ID s?n ph?m hi?n t?i
        size: 'M',
        soLuong: 1
    })
})
.then(response => response.json())
.then(data => {
    console.log('Response:', data);
    if (data.success) {
        console.log('? Buy Now API works!');
        window.location.href = '/Checkout';
    } else {
        console.error('? Buy Now failed:', data.message);
    }
})
.catch(error => {
    console.error('? Error:', error);
});
```

---
**Lýu ?:** N?u sau khi làm theo t?t c? các bý?c trên mà v?n không ðý?c, h?y:
1. Clear cache và cookies tr?nh duy?t
2. Restart application
3. Ki?m tra l?i t?t c? các file ð? s?a
