# Resole - ASP.NET Core Shoe E-Commerce Platform

A robust, full-featured e-commerce web application engineered with ASP.NET Core MVC, focused on high-performance server-side rendering, secure architecture, and clean user data management.

## 🚀 Tech Stack & Core Technologies
* **Backend Framework:** ASP.NET Core MVC & C# (.NET 8.0)
* **Database & ORM:** SQL Server & Entity Framework Core (Code-First Migrations)
* **Frontend Technologies:** Razor Views, HTML5, CSS3, JavaScript, Bootstrap 5
* **State & Data Management:** Native Session handling, ViewData, ViewBag, Strongly-Typed ViewModels
* **Security & Helpers:** Custom Hashing Utilities (`MahoaHelper`), File Upload Management (`UploadHelper`)

## 🌟 Core Architecture & Engineering Highlights
* **Comprehensive E-Commerce Logic:** Built full-scale business workflows including dynamic shopping carts (`CartSvc`), persistent order management (`DonhangSVC`), and strict stock/inventory controls.
* **Database Layer & Migrations:** Formulated efficient relational schemas for multi-table operations (Customers, Users, Shoes, Reviews, Dynamic Carts, and Details) with seamless EF Core structural rollouts.
* **Custom Security Architecture:** Developed an explicit `AuthenticationFilterAttribute` to robustly manage multi-role routing and backend access controls between regular Customers and Administrators.
* **Asynchronous Web Interactions:** Leveraged modern native JavaScript Fetch API/AJAX for asynchronous data updates (like live review logging and interactive components) to achieve non-blocking UI updates.
* **Clean Separation of Concerns:** Rigidly followed the MVC structural boundaries, isolating heavy domain processes inside independent Service classes away from the presentation Controllers.

## 🛠️ Local Development Setup
1. **Prerequisites:** Ensure .NET 8.0 SDK and Microsoft SQL Server are installed.
2. **Database Migration:** Adjust connection strings in `appsettings.json` to target your local server, then execute:
   ```bash
   dotnet ef database update
