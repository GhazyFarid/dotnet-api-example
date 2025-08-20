Create and Implemented JWT authentication

Steps:
1. Install paket NuGet "Microsoft.AspNetCore.Authentication.JwtBearer"
2. Tambahkan konfigurasi JWT di appsettings.json
3. Konfigurasi JWT di Program.cs
4. Buat Service untuk Generate Token example : "JwtService.cs"
5. Daftarkan Service "JwtService" di Program.cs
6. Buat Model Login
7. Buat Controller untuk Login & Mendapatkan Token
8. Gunakan [Authorize] di Endpoint