# C?P NH?T TRANG ADMIN - HÝ?NG D?N

## Các Thay Ð?i

### 1. Header/Navbar Admin M?i

#### Màu S?c M?i
- **Gradient ð?p m?t:** T? xanh ð?m (#1a1a2e) ? xanh navy (#16213e) ? xanh dýõng (#0f3460)
- **Border ð?:** Vi?n dý?i màu ð? (#e94560) ð? n?i b?t
- **Text tr?ng:** D? ð?c, týõng ph?n t?t

#### Logo
- Ð?i t? icon c?c cà phê ? icon speedometer (b?ng ði?u khi?n)
- Text: "ReSole Admin" ð? phân bi?t v?i trang user

#### Hover Effect
- Khi hover vào menu ? màu ð? (#e94560)
- Smooth transition

### 2. Cards Dashboard

#### Ð?i Tên & Icon

**Trý?c:**
- ? Qu?n l? Món Ãn ? Link: MonAn/Index

**Sau:**
- ?? Qu?n l? S?n Ph?m ? Link: **Giay/Index**
- Icon: `bi-basket3` (gi? hàng)
- Mô t?: "Qu?n l? danh sách giày"

#### Màu S?c Cards

1. **Card S?n Ph?m** (Ðen - Dark)
   - Border trái: #1a1a2e
   - Button: btn-dark
   - Icon màu ðen

2. **Card Khách Hàng** (Xanh dýõng - Cyan)
   - Border trái: #17a2b8
   - Button: btn-info
   - Icon màu cyan

3. **Card Ðõn Hàng** (Vàng - Yellow)
   - Border trái: #ffc107
   - Button: btn-warning
   - Icon màu vàng

4. **Card Qu?n Tr? Viên** (Ð? - Red)
   - Border trái: #dc3545
   - Button: btn-danger
   - Icon màu ð?

#### Hover Effect cho Cards
- Khi hover ? Card nâng lên 5px
- Box shadow ð?p m?t
- Smooth transition

### 3. Ch?c Nãng

#### Button "Xem chi ti?t" - Card S?n Ph?m
**Trý?c:** ? `/MonAn/Index`  
**Sau:** ? `/Giay/Index`

Khi click vào "Xem chi ti?t" ? card S?n Ph?m:
- Chuy?n ð?n trang **Giay/Index**
- Hi?n th? danh sách s?n ph?m giày
- Có nút **"Thêm s?n ph?m m?i"** ð? thêm giày

## Files Ð? S?a

1. ? `Views/Shared/_Layout.cshtml` - Header admin m?i
2. ? `Views/Admin/Index.cshtml` - Dashboard admin
3. ? `Views/Home/Index.cshtml` - Dashboard chính (admin)
4. ? `wwwroot/css/site.css` - CSS m?i cho admin

## CSS Classes M?i

### Navbar
```css
.admin-navbar {
  background: gradient xanh ð?m ? navy ? xanh dýõng
  border-bottom: 3px ð?
}
```

### Cards
```css
.admin-card-dark   /* Card ðen cho S?n ph?m */
.admin-card-cyan   /* Card xanh cho Khách hàng */
.admin-card-yellow /* Card vàng cho Ðõn hàng */
.admin-card-red    /* Card ð? cho Qu?n tr? viên */
```

M?i card có:
- Border trái 4px màu týõng ?ng
- Hover effect: nâng lên + box shadow
- Transition mý?t mà

## K?t Qu?

### ? Header
- Màu gradient xanh ð?m ð?p m?t
- Logo "ReSole Admin" v?i icon speedometer
- Border ð? n?i b?t
- Hover effect màu ð?

### ? Dashboard Cards
- Card S?n Ph?m: Màu ðen, icon gi? hàng
- Link ð?n Giay/Index ð? qu?n l? s?n ph?m
- 4 cards v?i 4 màu khác nhau
- Hover effect ð?p

### ? Responsive
- Ho?t ð?ng t?t trên m?i kích thý?c màn h?nh
- Cards t? ð?ng s?p x?p

## Test

1. **Ch?y l?i ?ng d?ng** (F5)
2. **Ðãng nh?p admin**
3. Xem **header m?i** ? Gradient xanh ð?p!
4. Xem **cards** ? 4 màu khác nhau
5. **Hover vào card** ? Nâng lên!
6. Click **"Xem chi ti?t"** ? card S?n Ph?m
7. ? Chuy?n ð?n trang **Giay/Index** ?

## So Sánh

### ? Trý?c
- Header: Màu nâu cà phê
- Card: "Qu?n l? Món Ãn" ? MonAn
- Icon: C?c cà phê
- Cards không có border màu

### ? Sau  
- Header: Gradient xanh ð?m ? navy ? xanh dýõng + vi?n ð?
- Card: "Qu?n l? S?n Ph?m" ? Giay
- Icon: Gi? hàng
- Cards có border trái 4 màu khác nhau
- Hover effects ð?p

## Tùy Ch?nh

### Ð?i màu header
```css
.admin-navbar {
  background: linear-gradient(...) !important;
}
```

### Ð?i màu card
```css
.admin-card-dark {
  border-left: 4px solid #YOUR_COLOR !important;
}
```

### Thêm card m?i
```html
<div class="col-md-6 col-lg-3">
    <div class="card admin-card-YOURCOLOR">
        <!-- Card content -->
    </div>
</div>
```

---

**HOÀN THÀNH!** ??

Trang admin c?a b?n gi? ð?p hõn nhi?u v?i:
- ? Header gradient xanh ð?m sang tr?ng
- ? Cards màu s?c r? ràng
- ? "Qu?n l? S?n Ph?m" thay v? "Qu?n l? Món Ãn"
- ? Link ð?n Giay/Index ð? qu?n l? giày
- ? Hover effects mý?t mà
