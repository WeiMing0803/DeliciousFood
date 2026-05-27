# 🍽️ DeliciousFood

一个基于 ASP.NET Core 的美食菜谱管理系统，提供用户端和管理端双平台，支持菜谱浏览、分类管理、会员订阅、支付宝支付等功能。

## 📋 项目简介

DeliciousFood 是一个全栈美食菜谱平台，采用前后端分离架构，包含以下核心功能：

- 🔐 用户注册、登录、身份认证
- 📖 菜谱浏览、搜索、分类
- 👨‍💼 后台管理系统
- 💳 会员订阅与支付宝支付集成
- 📧 邮件服务
- 📊 菜谱推荐系统

## 🏗️ 项目架构

项目采用多层架构设计，分为以下模块：

```
AI.DeliciousFood/
├── AI.DeliciousFood.Web.Client        # 用户端 Web 应用
├── AI.DeliciousFood.Web.Manage        # 管理端 Web 应用
├── AI.DeliciousFood.Core.Server       # 用户端业务逻辑层
├── AI.DeliciousFood.Core.ManageServer # 管理端业务逻辑层
├── AI.DeliciousFood.Core.Model        # 数据模型层
└── AI.DeliciousFood.Core.Common       # 公共工具类库
```

## 🛠️ 技术栈

### 后端技术

- **框架**: ASP.NET Core 10.0
- **数据库**: SQL Server
- **ORM**: Entity Framework Core 10.0
- **身份认证**: ASP.NET Core Identity
- **认证方式**: Cookie Authentication (支持 JWT Bearer)
- **日志**: Serilog
- **支付**: 支付宝 SDK (AlipaySDKNet.Standard)

### 核心 NuGet 包

- `Microsoft.AspNetCore.Identity.EntityFrameworkCore 10.0.0`
- `Microsoft.EntityFrameworkCore.SqlServer 10.0.0`
- `Microsoft.AspNetCore.Authentication.JwtBearer 10.0.0`
- `Serilog.AspNetCore 9.0.0`
- `AlipaySDKNet.Standard 4.9.874`

### 前端技术

- ASP.NET Core MVC
- Razor Views
- Bootstrap (通过 libman 管理)

## 📦 数据模型

主要数据实体包括：

- **FoodUser**: 用户信息
- **Recipe**: 菜谱
- **RecipeStatus**: 菜谱状态
- **RecipeCategories**: 菜谱分类
- **BaseCategory**: 基础分类
- **BaseCategoryItem**: 分类项
- **MemberPrice**: 会员价格
- **Recommend**: 推荐信息

## 🚀 快速开始

### 前置要求

- .NET 10.0 SDK
- SQL Server
- Visual Studio 2022 或 Visual Studio Code

### 安装步骤

1. **克隆项目**
   ```bash
   git clone https://github.com/WeiMing0803/DeliciousFood.git
   cd DeliciousFood
   ```

2. **配置数据库连接**
   
   编辑 `AI.DeliciousFood.Web.Client/appsettings.json` 和 `AI.DeliciousFood.Web.Manage/appsettings.json`，配置 SQL Server 连接字符串：
   ```json
   {
     "SqlConnectionStrings": "Server=your_server;Database=DeliciousFood;..."
   }
   ```

3. **数据库迁移**
   ```bash
   cd AI.DeliciousFood.Core.Model
   dotnet ef database update
   ```

4. **运行项目**
   
   用户端：
   ```bash
   cd AI.DeliciousFood.Web.Client
   dotnet run
   ```
   
   管理端：
   ```bash
   cd AI.DeliciousFood.Web.Manage
   dotnet run
   ```

## 📁 项目结构说明

### AI.DeliciousFood.Web.Client
用户端 Web 应用，提供菜谱浏览、搜索、用户注册登录、会员订阅等功能。

### AI.DeliciousFood.Web.Manage
管理端 Web 应用，提供后台管理功能，包括菜谱管理、用户管理、分类管理等。

### AI.DeliciousFood.Core.Model
数据模型层，包含所有实体类、数据库上下文、迁移文件和验证器。

### AI.DeliciousFood.Core.Common
公共类库，包含通用工具类、扩展方法、分页模型等。

## 🔑 核心功能

- ✅ 用户身份认证与授权（支持 Cookie 和 JWT）
- ✅ 菜谱 CRUD 操作
- ✅ 菜谱分类管理
- ✅ 搜索与筛选
- ✅ 会员订阅系统
- ✅ 支付宝支付集成
- ✅ 邮件通知
- ✅ 日志记录
- ✅ 数据分页

## 📝 开发说明

### 密码策略

系统配置了宽松的密码策略（可根据需要调整）：
- 最小长度：6 位
- 不要求数字、小写、大写、特殊字符

### 日志配置

使用 Serilog 记录日志，日志文件存储在 `Logs/` 目录下。

## 🤝 贡献

欢迎提交 Issue 和 Pull Request！

## 📄 许可证

本项目遵循 MIT 许可证。

## 👤 作者

WeiMing0803

## 📞 联系方式

如有问题或建议，请通过 GitHub Issues 联系。
