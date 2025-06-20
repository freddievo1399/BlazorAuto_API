#cập nhật tên cho powershell
dotnet ef migrations add "$(Get-Date -Format yyyyMMddHHmmss)" `
  --project ../../BlazorAuto_API.AbstractServer/BlazorAuto_API.AbstractServer.csproj `
  --startup-project . 
Read-Host "Press Enter to exit"