# FIX CART PERSISTENCE ISSUE

## Problem
When you add items to the cart and restart the web application, the cart items are not persisted and disappear.

## Root Cause
The `Carts` table was defined in the `DataContext.cs` but was not created in the database through Entity Framework migrations.

## Changes Made

### 1. Updated Models/DataContext.cs
- Added relationship configuration for the `Cart` entity
- Configured foreign keys to `KhachHang` and `Giay` tables

### 2. Updated Services/CartSvc.cs
- Added `Include` statements to load navigation properties (`Giay` and `KhachHang`) when fetching cart data
- This ensures related data is loaded with cart items

### 3. Updated Controllers/CartController.cs
- Fixed type conversion from `float` to `decimal` for the `Gia` property
- Refactored to properly load `Giay` details when creating `CartItem` objects

### 4. Updated Views/Cart/Index.cshtml
- Changed to calculate total from model data instead of session
- This ensures the total is calculated from database data

### 5. Created Migration Files
- `Migrations/20251203000000_AddCartsTable.cs` - Migration to create Carts table
- `Migrations/20251203000000_AddCartsTable.Designer.cs` - Designer file for the migration

## How to Apply the Fix

### Step 1: Stop the Application
Stop the running web application in Visual Studio

### Step 2: Apply the Migration
Run the following command in the Package Manager Console or Terminal:

```bash
dotnet ef database update
```

Or in Package Manager Console in Visual Studio:

```powershell
Update-Database
```

### Step 3: Verify Database
Check that the `Carts` table was created in your database with the following structure:
- Id (int, primary key, identity)
- KhachHangId (int, foreign key to KhachHangs)
- GiayId (int, foreign key to MonAns)
- Size (nvarchar(10))
- SoLuong (int)
- CreatedAt (datetime2)

### Step 4: Restart the Application
Start the application again

## Testing
1. Log in as a customer
2. Add items to the cart
3. Stop the application
4. Start the application again
5. Log in with the same customer account
6. Check that the cart items are still there

## Note
The cart data is now persisted in the database, so it will survive application restarts. Each customer's cart is tied to their `KhachHangId`.
