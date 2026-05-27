# 本地部署脚本
# 使用方法: .\scripts\deploy-local.ps1 -ServerHost "your-server" -Username "admin" -Password "password"

param(
    [Parameter(Mandatory=$true)]
    [string]$ServerHost,
    
    [Parameter(Mandatory=$true)]
    [string]$Username,
    
    [Parameter(Mandatory=$true)]
    [string]$Password,
    
    [Parameter(Mandatory=$false)]
    [string]$ClientDeployPath = "C:\inetpub\wwwroot\DeliciousFood.Client",
    
    [Parameter(Mandatory=$false)]
    [string]$ManageDeployPath = "C:\inetpub\wwwroot\DeliciousFood.Manage",
    
    [Parameter(Mandatory=$false)]
    [string]$ClientSiteName = "DeliciousFood.Client",
    
    [Parameter(Mandatory=$false)]
    [string]$ManageSiteName = "DeliciousFood.Manage"
)

$ErrorActionPreference = "Stop"

Write-Host "开始部署流程..." -ForegroundColor Green

# 1. 构建项目
Write-Host "正在构建项目..." -ForegroundColor Yellow
dotnet restore AI.DeliciousFood.sln
dotnet build AI.DeliciousFood.sln --configuration Release --no-restore

# 2. 发布项目
Write-Host "正在发布 Web.Client..." -ForegroundColor Yellow
dotnet publish AI.DeliciousFood.Web.Client/AI.DeliciousFood.Web.Client.csproj `
    --configuration Release `
    --output ./publish/client `
    --no-build

Write-Host "正在发布 Web.Manage..." -ForegroundColor Yellow
dotnet publish AI.DeliciousFood.Web.Manage/AI.DeliciousFood.Web.Manage.csproj `
    --configuration Release `
    --output ./publish/manage `
    --no-build

# 3. 创建压缩包
Write-Host "正在创建部署包..." -ForegroundColor Yellow
Compress-Archive -Path ./publish/client/* -DestinationPath client-deploy.zip -Force
Compress-Archive -Path ./publish/manage/* -DestinationPath manage-deploy.zip -Force

# 4. 连接到远程服务器
Write-Host "正在连接到服务器 $ServerHost..." -ForegroundColor Yellow
$secpasswd = ConvertTo-SecureString $Password -AsPlainText -Force
$creds = New-Object System.Management.Automation.PSCredential ($Username, $secpasswd)

try {
    $session = New-PSSession -ComputerName $ServerHost -Credential $creds
    
    # 5. 部署 Client
    Write-Host "正在部署 Client 到 $ClientDeployPath..." -ForegroundColor Yellow
    Copy-Item -Path ".\client-deploy.zip" -Destination "C:\Temp\client-deploy.zip" -ToSession $session
    
    Invoke-Command -Session $session -ScriptBlock {
        param($deployPath, $siteName)
        
        # 停止站点
        Import-Module WebAdministration
        Write-Host "停止站点: $siteName"
        Stop-WebSite -Name $siteName -ErrorAction SilentlyContinue
        Start-Sleep -Seconds 5
        
        # 备份旧文件
        $backupPath = "$deployPath-backup-$(Get-Date -Format 'yyyyMMddHHmmss')"
        if (Test-Path $deployPath) {
            Write-Host "备份到: $backupPath"
            Copy-Item -Path $deployPath -Destination $backupPath -Recurse
        }
        
        # 确保目录存在
        if (-not (Test-Path $deployPath)) {
            New-Item -Path $deployPath -ItemType Directory -Force
        }
        
        # 清理旧文件
        Write-Host "清理旧文件..."
        Remove-Item -Path "$deployPath\*" -Recurse -Force -ErrorAction SilentlyContinue
        
        # 解压新文件
        Write-Host "解压新文件到: $deployPath"
        Expand-Archive -Path "C:\Temp\client-deploy.zip" -DestinationPath $deployPath -Force
        Remove-Item -Path "C:\Temp\client-deploy.zip" -Force
        
        # 启动站点
        Write-Host "启动站点: $siteName"
        Start-WebSite -Name $siteName -ErrorAction SilentlyContinue
        
        Write-Host "Client 部署完成!" -ForegroundColor Green
    } -ArgumentList $ClientDeployPath, $ClientSiteName
    
    # 6. 部署 Manage
    Write-Host "正在部署 Manage 到 $ManageDeployPath..." -ForegroundColor Yellow
    Copy-Item -Path ".\manage-deploy.zip" -Destination "C:\Temp\manage-deploy.zip" -ToSession $session
    
    Invoke-Command -Session $session -ScriptBlock {
        param($deployPath, $siteName)
        
        # 停止站点
        Import-Module WebAdministration
        Write-Host "停止站点: $siteName"
        Stop-WebSite -Name $siteName -ErrorAction SilentlyContinue
        Start-Sleep -Seconds 5
        
        # 备份旧文件
        $backupPath = "$deployPath-backup-$(Get-Date -Format 'yyyyMMddHHmmss')"
        if (Test-Path $deployPath) {
            Write-Host "备份到: $backupPath"
            Copy-Item -Path $deployPath -Destination $backupPath -Recurse
        }
        
        # 确保目录存在
        if (-not (Test-Path $deployPath)) {
            New-Item -Path $deployPath -ItemType Directory -Force
        }
        
        # 清理旧文件
        Write-Host "清理旧文件..."
        Remove-Item -Path "$deployPath\*" -Recurse -Force -ErrorAction SilentlyContinue
        
        # 解压新文件
        Write-Host "解压新文件到: $deployPath"
        Expand-Archive -Path "C:\Temp\manage-deploy.zip" -DestinationPath $deployPath -Force
        Remove-Item -Path "C:\Temp\manage-deploy.zip" -Force
        
        # 启动站点
        Write-Host "启动站点: $siteName"
        Start-WebSite -Name $siteName -ErrorAction SilentlyContinue
        
        Write-Host "Manage 部署完成!" -ForegroundColor Green
    } -ArgumentList $ManageDeployPath, $ManageSiteName
    
    Write-Host "`n✅ 部署成功完成！" -ForegroundColor Green
    
} catch {
    Write-Host "`n❌ 部署失败: $_" -ForegroundColor Red
    throw
} finally {
    # 清理
    if ($session) {
        Remove-PSSession -Session $session
    }
    
    # 清理本地文件
    Remove-Item -Path ".\client-deploy.zip" -Force -ErrorAction SilentlyContinue
    Remove-Item -Path ".\manage-deploy.zip" -Force -ErrorAction SilentlyContinue
}
