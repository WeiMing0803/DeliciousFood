# GitHub Actions 自动部署到 IIS 服务器配置指南

本文档说明如何配置 GitHub Actions 自动将代码部署到 IIS 服务器。

## 📋 前置要求

### 1. 设置 Self-hosted Runner ⚠️ **重要**

由于 IIS 服务器通常在内网环境,GitHub Actions 的云端 Runner 无法直接访问,因此**必须设置 Self-hosted Runner**。

**设置步骤**:
1. 进入 GitHub 仓库 `Settings` → `Actions` → `Runners` → `New self-hosted runner`
2. 选择 **Windows** 系统
3. 在 **IIS 服务器**上以管理员身份打开 PowerShell
4. **复制** GitHub 页面上显示的命令并执行配置:
   ```powershell
   # 创建目录并下载 Runner
   mkdir actions-runner; cd actions-runner
   # 下载 Runner (GitHub 会提供具体命令)
   # 配置 Runner
   ./config.cmd --url https://github.com/WeiMing0803/DeliciousFood --token XXXXXX
   ```
5. **将 Runner 安装为 Windows 服务** (推荐):
   
   以**管理员身份**运行 PowerShell（右键 PowerShell → "以管理员身份运行"），然后进入 Runner 目录执行:
   
   ```powershell
   # 进入 Runner 目录
   cd C:\Users\Administrator\actions-runner  # 修改为你的实际路径
   
   # 安装为 Windows 服务
   .\run.cmd install
   
   # 启动服务
   .\run.cmd start
   ```
   
   **如果上述命令不工作**，使用以下 PowerShell 脚本创建服务：
   
   ```powershell
   # 以管理员身份运行 PowerShell
   
   # 设置 Runner 路径（修改为你的实际路径）
   $runnerPath = "C:\Users\Administrator\actions-runner"
   $serviceName = "GitHubActionsRunner"
   
   # 创建 Windows 服务
   New-Service -Name $serviceName `
       -BinaryPathName "$runnerPath\bin\RunnerService.exe" `
       -DisplayName "GitHub Actions Runner (DeliciousFood)" `
       -Description "GitHub Actions self-hosted runner for DeliciousFood" `
       -StartupType Automatic
   
   # 启动服务
   Start-Service -Name $serviceName
   
   # 验证服务状态
   Get-Service -Name $serviceName
   ```
   
   安装成功后，你会在 Windows 服务管理器（`services.msc`）中看到 `GitHub Actions Runner` 服务。

6. 返回 GitHub 确认 Runner 状态显示为 **Idle** (绿色) ✅

💡 **提示**: 
- GitHub 会为你生成带有临时 token 的命令，直接复制执行即可，非常简单！
- **强烈建议**将 Runner 安装为 Windows 服务，这样可以：
  - ✅ 后台持续运行，无需保持 PowerShell 窗口打开
  - ✅ 系统重启后自动启动
  - ✅ 更加稳定可靠
  - ✅ 可以在 Windows 服务管理器中管理

**Windows 服务管理命令**:

```powershell
# 方法 1：使用 PowerShell 管理服务
Get-Service -Name "GitHubActionsRunner"           # 查看服务状态
Stop-Service -Name "GitHubActionsRunner"          # 停止服务
Start-Service -Name "GitHubActionsRunner"         # 启动服务
Remove-Service -Name "GitHubActionsRunner"        # 删除服务（PowerShell 6.0+）

# 方法 2：使用 sc 命令管理服务
sc query GitHubActionsRunner                      # 查看服务状态
sc stop GitHubActionsRunner                       # 停止服务
sc start GitHubActionsRunner                      # 启动服务
sc delete GitHubActionsRunner                     # 删除服务

# 方法 3：使用服务管理器（图形界面）
services.msc                                      # 打开服务管理器
```

**运行方式对比**:

| 方法 | 优点 | 缺点 | 适用场景 |
|-----|------|------|---------|
| **Windows 服务** (推荐) | ✅ 后台运行<br>✅ 自动启动<br>✅ 稳定可靠 | ⚠️ 需要管理员权限 | 生产环境、长期使用 |
| **手动运行** `.\run.cmd` | ✅ 无需管理员权限<br>✅ 方便调试 | ❌ 需要保持窗口打开<br>❌ 关闭窗口后停止 | 临时测试、开发调试 |

### 2. IIS 服务器配置要求

在你的 IIS 服务器上需要完成以下配置:

#### A. 安装 .NET Runtime
- 安装 .NET 10.0 Runtime (或更高版本)
- 下载地址: https://dotnet.microsoft.com/download

#### B. 配置 IIS 站点
创建两个 IIS 站点:
1. **DeliciousFood.Client** - 客户端站点
2. **DeliciousFood.Manage** - 管理后台站点

确保应用程序池配置:
- .NET CLR 版本: 无托管代码
- 托管管道模式: 集成
- 启用 32 位应用程序: False

## 🔐 GitHub Secrets 配置

在 GitHub 仓库中配置以下 Secrets:

进入仓库 `Settings` → `Secrets and variables` → `Actions` → `New repository secret`

添加以下 Secrets:

| Secret 名称 | 说明 | 示例值 |
|------------|------|--------|
| `CLIENT_DEPLOY_PATH` | Client 部署路径 | `C:\inetpub\wwwroot\DeliciousFood.Client` |
| `MANAGE_DEPLOY_PATH` | Manage 部署路径 | `C:\inetpub\wwwroot\DeliciousFood.Manage` |
| `CLIENT_SITE_NAME` | Client IIS 站点名称 | `DeliciousFood.Client` |
| `MANAGE_SITE_NAME` | Manage IIS 站点名称 | `DeliciousFood.Manage` |

**注意**: 部署时会自动排除 `appsettings.json` 和 `appsettings.Development.json` 文件,请在服务器上手动配置这些文件。

## 📁 已创建的文件

### 1. GitHub Actions 工作流文件

#### `.github/workflows/deploy-to-iis-local.yml`
本地部署工作流。特点:
- ✅ **智能变更检测**: 自动识别哪些项目被修改，只构建和部署有变更的项目
- ✅ 自动停止/启动 IIS 站点和应用程序池
- ✅ 自动备份旧版本(保留最近5个)
- ✅ 本地直接部署,无需网络配置
- ✅ 自动排除 appsettings.json 文件(保留服务器上的配置)
- ✅ 自动清理旧备份

**智能部署说明**:
- 只修改了 `AI.DeliciousFood.Web.Client` 或 `AI.DeliciousFood.Core.Server` → 只部署 Client 站点
- 只修改了 `AI.DeliciousFood.Web.Manage` 或 `AI.DeliciousFood.Core.ManageServer` → 只部署 Manage 站点
- 修改了 `AI.DeliciousFood.Core.Common`、`AI.DeliciousFood.Core.Model` 或 `AI.DeliciousFood.Core.Data` → 同时部署两个站点（因为是共享库）
- 修改了 `.sln` 文件 → 同时部署两个站点

这样可以:
- 🚀 **节省时间**: 避免不必要的构建和部署
- 💰 **节省资源**: 减少 GitHub Actions 运行时间
- 🎯 **降低风险**: 只更新有变化的部分，减少潜在问题

### 2. 本地部署脚本

#### `.github/scripts/deploy-local.ps1`
用于本地手动部署的 PowerShell 脚本。

使用方法:
```powershell
.\.github\scripts\deploy-local.ps1 `
    -ServerHost "192.168.1.100" `
    -Username "Administrator" `
    -Password "YourPassword" `
    -ClientDeployPath "C:\inetpub\wwwroot\DeliciousFood.Client" `
    -ManageDeployPath "C:\inetpub\wwwroot\DeliciousFood.Manage" `
    -ClientSiteName "DeliciousFood.Client" `
    -ManageSiteName "DeliciousFood.Manage"
```

## 🚀 部署流程

### 自动部署
1. 提交代码到 `feature/Iframe` 分支
2. GitHub Actions 自动触发
3. 自动构建 → 发布 → 部署到 IIS

### 监控部署状态
- 访问仓库的 `Actions` 标签页
- 查看工作流运行状态
- 查看详细日志

## 🔍 故障排查

### 常见问题

#### 1. 权限不足
```
错误: Access Denied
```

解决方案:
- 确保使用的账户有管理员权限
- 检查 IIS 站点目录的写入权限
- 确认账户可以停止/启动 IIS 站点

#### 3. 站点无法访问
部署成功但站点无法访问:
- 检查 IIS 站点是否已启动
- 检查应用程序池是否正在运行
- 查看 IIS 日志和事件查看器
- 检查 `appsettings.json` 中的数据库连接字符串

#### 4. .NET Runtime 版本不匹配
```
错误: You must install or update .NET to run this application
```

解决方案:
- 在服务器上安装 .NET 10.0 Runtime
- 或修改项目 `TargetFramework` 为服务器已有版本

## 💰 GitHub Actions 免费配额说明

对于公共仓库:
- ✅ **完全免费,无限制使用**
- ✅ 无分钟数限制
- ✅ 无并发任务限制

对于私有仓库(免费账户):
- ⏱️ 每月 2,000 分钟免费额度
- 💻 Windows runner: 1 分钟消耗 2 倍额度(即 1 分钟实际消耗 2 分钟配额)
- 🐧 Linux runner: 1 分钟消耗 1 倍额度
- 🍎 macOS runner: 1 分钟消耗 10 倍额度

**你的情况**:
- 仓库类型: 公共仓库 ✅
- 费用: 完全免费
- 使用限制: 无
- Runner 类型: Windows (因为需要部署到 IIS)

**估算单次部署时间**: 约 3-5 分钟(包括构建、发布、部署)

了解更多: https://docs.github.com/en/billing/managing-billing-for-github-actions/about-billing-for-github-actions

## 🔒 安全建议

1. **Runner 服务账户权限**: Self-hosted Runner 服务应使用具有 IIS 管理权限的账户运行
2. **保护 GitHub Secrets**: 定期检查和更新 Secrets 中的配置
3. **备份管理**: 定期检查备份文件夹,确保有足够的磁盘空间
4. **监控日志**: 定期检查部署日志和 IIS 日志
5. **Runner 安全**: 保持 Runner 机器的操作系统和软件更新

## 📞 测试部署

提交一个小改动到 `feature/Iframe` 分支来测试部署:

```bash
# 创建测试提交
git checkout feature/Iframe
echo "# Test deployment" >> test.txt
git add test.txt
git commit -m "test: GitHub Actions 部署测试"
git push origin feature/Iframe
```

然后在 GitHub Actions 页面观察部署过程。

## 🎯 下一步

部署成功后,你可能还需要:
1. 配置数据库连接字符串
2. 设置 HTTPS/SSL 证书
3. 配置域名绑定
4. 设置应用程序日志
5. 配置健康检查和监控

---

如有问题,请检查 GitHub Actions 日志或服务器事件查看器获取详细错误信息。
