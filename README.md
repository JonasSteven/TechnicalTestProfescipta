# Technical Test Profescipta

Pembuatan Web Apps Sederhana dengan ASP.NET Core Web App (Razor Page)

## Dependencies

Pastikan untuk menginstal paket berikut agar proyek dapat berjalan dengan baik:

1. **EPPlus** (versi 8.0.2)
   - Digunakan untuk memanipulasi file Excel di dalam aplikasi.
   - Instal menggunakan NuGet:
     ```bash
     Install-Package EPPlus -Version 8.0.2
     ```

2. **Microsoft.EntityFrameworkCore** (versi 9.0.4)
   - Paket utama untuk Entity Framework Core.
   - Instal menggunakan NuGet:
     ```bash
     Install-Package Microsoft.EntityFrameworkCore -Version 9.0.4
     ```

3. **Microsoft.EntityFrameworkCore.SqlServer** (versi 9.0.4)
   - Menyediakan dukungan untuk SQL Server dalam Entity Framework Core.
   - Instal menggunakan NuGet:
     ```bash
     Install-Package Microsoft.EntityFrameworkCore.SqlServer -Version 9.0.4
     ```

4. **System.Data.SqlClient** (versi 4.9.0)
   - Menyediakan akses SQL Server untuk aplikasi.
   - Instal menggunakan NuGet:
     ```bash
     Install-Package System.Data.SqlClient -Version 4.9.0
     ```

### Pengaturan yang Harus Disesuaikan di `appsettings.json`

Setelah meng-clone proyek ini, Anda perlu menyesuaikan beberapa pengaturan di file `appsettings.json` agar aplikasi dapat berjalan dengan benar di lingkungan lokal Anda. Berikut adalah langkah-langkah yang perlu dilakukan:

1. **String Koneksi Database**
Buka file `appsettings.json` dan sesuaikan string koneksi database sesuai dengan pengaturan di mesin Anda. Sebagai contoh, file `appsettings.json` di proyek ini berisi string koneksi seperti ini:

```json
"ConnectionStrings": {
  "DefaultConnection": "Data Source=LAPTOP-PK6DOAKC\\SQLEXPRESS;Initial Catalog=TechnicalTestProfescipta;Integrated Security=True;Trust Server Certificate=True"
}
