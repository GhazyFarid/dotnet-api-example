Create and Implemented JWT authentication V2

Steps:

1. Install paket NuGet "Microsoft.AspNetCore.Authentication.JwtBearer"
2. Tambahkan konfigurasi JWT di appsettings.json
3. Buat model "User.cs"
4. Buat Service untuk Generate Token example : "JwtService.cs"
5. Buat Model Login
6. Buat Controller untuk Login & Mendapatkan Token
7. Buat "CustomJwtBearerHandler.cs" di folder function
   -> Handler custom untuk memvalidasi token saat request masuk.
8. Konfigurasi JWT Authentication di Program.cs
   1. Tambahkan AddAuthentication() -> pakai scheme CustomJwtBearer.
   2. Tambahkan AddScoped<JwtService>().
9. Gunakan [Authorize] di Endpoint
