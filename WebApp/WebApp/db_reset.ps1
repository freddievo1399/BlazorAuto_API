# 1. Xoá thư mục Migration
Remove-Item -Recurse -Force ../../BlazorAuto_API.AbstractServer/Migrations


Write-Host "✅ Đã xóa toàn bộ migration."
Write-Host "Đang khởi tạo"


# 2. Rollback database về trạng thái ban đầu
dotnet ef migrations add "Init" `
  --project ../../BlazorAuto_API.AbstractServer/BlazorAuto_API.AbstractServer.csproj `
  --startup-project .

Read-Host "Nhấn Enter để thoát"
