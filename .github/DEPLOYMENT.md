# GitHub Actions 自动部署到 IIS 服务器配置指南

本文档说明如何配置 GitHub Actions 自动将代码部署到 IIS 服务器。

## 📋 前置要求

### 1. 设置 Self-hosted Runner ⚠️ **重要**

由于 IIS 服务器通常在内网环境,GitHub Actions 的云端 Runner 无法直接访问,因此**必须设置 Self-hosted Runner**。

**设置步骤**:
1. 进入 GitHub 仓库 `Settings` → `Actions` → `Runners` → `New self-hosted runner`
2. 选择 **Windows** 系统
3. 在 **IIS 服务器**上以管理员身份打开 PowerShell
4. **复制** GitHub 页面上显示的命令并执行(包含下载、配置和启动 Runner)
5. 返回 GitHub 确认 Runner 状态显示为 **Idle** (绿色) ✅

💡 **提示**: GitHub 会为你生成带有临时 token 的命令,直接复制执行即可,非常简单!

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
- ✅ 自动停止/启动 IIS 站点和应用程序池
- ✅ 自动备份旧版本(保留最近5个)
- ✅ 本地直接部署,无需网络配置
- ✅ 自动排除 appsettings.json 文件(保留服务器上的配置)
- ✅ 自动清理旧备份

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
