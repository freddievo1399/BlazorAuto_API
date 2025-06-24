#cập nhật tên cho powershell
dotnet ef migrations add "$(Get-Date -Format yyyyMMddHHmmss)" `
  --project ../../BlazorAuto_API.Infrastructure/BlazorAuto_API.Infrastructure.csproj `
  --startup-project . 
Read-Host "Press Enter to exit"