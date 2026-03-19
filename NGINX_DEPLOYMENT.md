# Nginx 反向代理部署指南

本文档介绍如何在 Windows 服务器上使用 Nginx 为 AI.DeliciousFood 项目配置反向代理，实现通过域名直接访问而无需输入端口号。

## 项目信息

- **Client 应用域名**: `app.abc.Cloud` (HTTPS)
- **Client 后端端口**: `5178`
- **Manage 应用域名**: `manage.abc.Cloud` (HTTPS)
- **Manage 后端端口**: `5222`

## 一、安装 Nginx

### 1.1 下载 Nginx
1. 访问 [Nginx 官方网站](http://nginx.org/en/download.html)
2. 下载最新的 Windows 稳定版本（建议使用 1.24.x 或更高版本）
3. 解压到 `C:\nginx` 目录

### 1.2 验证安装
```powershell
cd C:\nginx
.\nginx.exe -v
```

## 二、SSL 证书准备

### 2.1 获取 SSL 证书
您需要为两个域名分别申请 SSL 证书：
- `app.abc.Cloud`
- `manage.abc.Cloud`

证书文件通常包括：
- `.crt` 或 `.pem` 文件（证书文件）
- `.key` 文件（私钥文件）

### 2.2 证书存放位置
建议在 Nginx 目录下创建证书文件夹：
```powershell
New-Item -ItemType Directory -Path "C:\nginx\ssl" -Force
```

将证书文件复制到此目录（证书文件可以是 .pem 或 .crt 格式）：
- `C:\nginx\ssl\app.abc.cloud.pem` 或 `.crt`（证书文件）
- `C:\nginx\ssl\app.abc.cloud.key`（私钥文件）
- `C:\nginx\ssl\manage.abc.cloud.pem` 或 `.crt`（证书文件）
- `C:\nginx\ssl\manage.abc.cloud.key`（私钥文件）

## 三、Nginx 配置

### 3.1 完整配置文件

编辑 `C:\nginx\conf\nginx.conf`，使用以下配置：

```nginx
worker_processes  1;

events {
    worker_connections  1024;
}

http {
    include       mime.types;
    default_type  application/octet-stream;
    
    sendfile        on;
    keepalive_timeout  65;
    
    # Client 应用上游服务器配置
    upstream deliciousfood_client {
        server localhost:5178;
    }
    
    # Manage 应用上游服务器配置
    upstream deliciousfood_manage {
        server localhost:5222;
    }
    
    # Client 应用 - HTTP 重定向到 HTTPS
    server {
        listen       80;
        server_name  app.abc.cloud;
        
        # 重定向所有 HTTP 请求到 HTTPS
        return 301 https://$server_name$request_uri;
    }
    
    # Client 应用 - HTTPS 配置
    server {
        listen       443 ssl;
        server_name  app.abc.cloud;
        
        # SSL 证书配置（注意：Windows 路径使用正斜杠）
        ssl_certificate      C:/nginx/ssl/app.abc.cloud.pem;
        ssl_certificate_key  C:/nginx/ssl/app.abc.cloud.key;
        
        # SSL 安全配置
        ssl_protocols TLSv1.2 TLSv1.3;
        ssl_ciphers HIGH:!aNULL:!MD5;
        ssl_prefer_server_ciphers on;
        ssl_session_cache shared:SSL:10m;
        ssl_session_timeout 10m;
        
        # 客户端最大上传大小
        client_max_body_size 50M;
        
        location / {
            proxy_pass http://deliciousfood_client;
            
            # 代理头设置
            proxy_set_header Host $host;
            proxy_set_header X-Real-IP $remote_addr;
            proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
            proxy_set_header X-Forwarded-Proto $scheme;
            proxy_set_header X-Forwarded-Host $server_name;
            
            # WebSocket 支持
            proxy_http_version 1.1;
            proxy_set_header Upgrade $http_upgrade;
            proxy_set_header Connection "upgrade";
            
            # 超时设置
            proxy_connect_timeout 60s;
            proxy_send_timeout 60s;
            proxy_read_timeout 60s;
        }
        
        # 静态文件缓存优化
        location ~* \.(js|css|png|jpg|jpeg|gif|ico|svg|woff|woff2|ttf|eot)$ {
            proxy_pass http://deliciousfood_client;
            proxy_cache_valid 200 1d;
            expires 1d;
            add_header Cache-Control "public, immutable";
        }
    }
    
    # Manage 应用 - HTTP 重定向到 HTTPS
    server {
        listen       80;
        server_name  manage.abc.cloud;
        
        # 重定向所有 HTTP 请求到 HTTPS
        return 301 https://$server_name$request_uri;
    }
    
    # Manage 应用 - HTTPS 配置
    server {
        listen       443 ssl;
        server_name  manage.abc.cloud;
        
        # SSL 证书配置（注意：Windows 路径使用正斜杠）
        ssl_certificate      C:/nginx/ssl/manage.abc.cloud.pem;
        ssl_certificate_key  C:/nginx/ssl/manage.abc.cloud.key;
        
        # SSL 安全配置
        ssl_protocols TLSv1.2 TLSv1.3;
        ssl_ciphers HIGH:!aNULL:!MD5;
        ssl_prefer_server_ciphers on;
        ssl_session_cache shared:SSL:10m;
        ssl_session_timeout 10m;
        
        # 客户端最大上传大小（管理后台可能需要上传图片等）
        client_max_body_size 100M;
        
        location / {
            proxy_pass http://deliciousfood_manage;
            
            # 代理头设置
            proxy_set_header Host $host;
            proxy_set_header X-Real-IP $remote_addr;
            proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
            proxy_set_header X-Forwarded-Proto $scheme;
            proxy_set_header X-Forwarded-Host $server_name;
            
            # 超时设置
            proxy_connect_timeout 60s;
            proxy_send_timeout 60s;
            proxy_read_timeout 60s;
        }
        
        # 静态文件缓存优化
        location ~* \.(js|css|png|jpg|jpeg|gif|ico|svg|woff|woff2|ttf|eot)$ {
            proxy_pass http://deliciousfood_manage;
            proxy_cache_valid 200 1d;
            expires 1d;
            add_header Cache-Control "public, immutable";
        }
    }
}
```

### 3.2 测试配置文件
在应用配置之前，务必测试配置文件的语法：

```powershell
cd C:\nginx
.\nginx.exe -t
```

如果看到 `syntax is ok` 和 `test is successful`，说明配置正确。

## 四、启动和管理 Nginx

### 4.1 启动 Nginx
```powershell
cd C:\nginx
start nginx
```

### 4.2 检查 Nginx 是否运行
```powershell
Get-Process nginx
```

### 4.3 重新加载配置（修改配置后）
```powershell
cd C:\nginx
.\nginx.exe -s reload
```

### 4.4 停止 Nginx
```powershell
cd C:\nginx
.\nginx.exe -s stop
```

## 五、将 Nginx 安装为 Windows 服务

为了让 Nginx 开机自动启动,建议将其安装为 Windows 服务。

### 5.1 使用 WinSW(推荐)

1. **下载 WinSW**
   - 访问 [https://github.com/winsw/winsw/releases](https://github.com/winsw/winsw/releases)
   - 下载最新的 `WinSW-x64.exe` 文件
   - 将文件重命名为 `NginxService.exe` 并放到 `C:\nginx` 目录

2. **创建配置文件**
   
   在 `C:\nginx` 目录下创建 `NginxService.xml` 文件:
   ```xml
   <service>
     <id>NginxService</id>
     <name>Nginx Reverse Proxy</name>
     <description>Nginx reverse proxy for DeliciousFood applications</description>
     <executable>C:\nginx\nginx.exe</executable>
     <startmode>Automatic</startmode>
     <logpath>C:\nginx\logs</logpath>
     <log mode="roll-by-size">
       <sizeThreshold>10240</sizeThreshold>
       <keepFiles>8</keepFiles>
     </log>
     <onfailure action="restart" delay="10 sec"/>
     <onfailure action="restart" delay="20 sec"/>
     <resetfailure>1 hour</resetfailure>
     <stopexecutable>C:\nginx\nginx.exe</stopexecutable>
     <stopargument>-s</stopargument>
     <stopargument>quit</stopargument>
   </service>
   ```

3. **安装服务**
   ```powershell
   cd C:\nginx
   .\NginxService.exe install
   ```

4. **启动服务并验证**
   ```powershell
   # 启动服务
   Start-Service NginxService
   
   # 查看服务状态
   Get-Service NginxService
   ```

### 5.2 管理 Nginx 服务

- **启动服务**: `Start-Service NginxService`
- **停止服务**: `Stop-Service NginxService`
- **重启服务**: `Restart-Service NginxService`
- **卸载服务**: `C:\nginx\NginxService.exe uninstall`

## 六、IIS 和 ASP.NET Core 应用配置

### 6.1 确保应用监听正确端口

确认您的应用在 IIS 或直接运行时监听以下端口：
- Client 应用: `http://localhost:5178`
- Manage 应用: `http://localhost:5222`

### 6.2 应用配置更新建议

如果您的应用需要知道它在 HTTPS 后面运行，请在 `Program.cs` 中添加转发头支持：

```csharp
// 在 var app = builder.Build(); 之前添加
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
});

// 在 var app = builder.Build(); 之后添加
app.UseForwardedHeaders();
```

## 七、防火墙配置

确保 Windows 防火墙允许 80 和 443 端口的入站连接：

```powershell
# 允许 HTTP (端口 80)
New-NetFirewallRule -DisplayName "Nginx HTTP" -Direction Inbound -LocalPort 80 -Protocol TCP -Action Allow

# 允许 HTTPS (端口 443)
New-NetFirewallRule -DisplayName "Nginx HTTPS" -Direction Inbound -LocalPort 443 -Protocol TCP -Action Allow
```

## 八、DNS 配置

确保您的域名 DNS 记录正确指向服务器 IP：

| 域名 | 类型 | 值 |
|------|------|-----|
| app.abc.cloud | A | 您的服务器IP地址 |
| manage.abc.cloud | A | 您的服务器IP地址 |

## 九、测试和验证

### 9.1 本地测试
在部署服务器上修改 `C:\Windows\System32\drivers\etc\hosts` 文件测试：
```
127.0.0.1 app.abc.cloud
127.0.0.1 manage.abc.cloud
```

### 9.2 浏览器测试
- 访问 `https://app.abc.cloud` 应该显示 Client 应用
- 访问 `https://manage.abc.cloud` 应该显示 Manage 应用
- 访问 `http://app.abc.cloud` 应该自动重定向到 HTTPS

### 9.3 SSL 测试
使用 [SSL Labs](https://www.ssllabs.com/ssltest/) 测试您的 SSL 配置安全性。

## 十、常见问题排查

### 10.1 502 Bad Gateway
- 检查后端应用是否正在运行
- 验证端口号是否正确（5178 和 5222）
- 查看 Nginx 错误日志：`C:\nginx\logs\error.log`

### 10.2 SSL 证书错误
- 确认证书文件路径正确
- 检查证书是否过期
- 验证证书和域名是否匹配

### 10.3 证书替换后仍提示过期（重点）

在 Windows 环境下，手动替换证书后建议严格按以下顺序执行：

1. **仅保留一套 Nginx 实例（避免多实例冲突）**
```powershell
taskkill /F /IM nginx.exe
```

2. **清理旧 PID 文件（避免 reload 指向旧进程）**
```powershell
Remove-Item C:\nginx\logs\nginx.pid -Force -ErrorAction SilentlyContinue
```

3. **在 `nginx.conf` 主级别显式配置 pid**
将以下配置放在 `worker_processes` 同级（不在 `http {}` 内）：
```nginx
pid C:/nginx/logs/nginx.pid;
```

4. **检查配置语法**
```powershell
cd C:\nginx
.\nginx.exe -t
```

5. **按固定 prefix 启动 Nginx（保证 PID 路径一致）**
```powershell
Start-Process -FilePath C:\nginx\nginx.exe -ArgumentList "-p C:\nginx -c conf\nginx.conf"
```

6. **确认进程 PID 与 pid 文件一致**
```powershell
Get-Process nginx
Get-Content C:\nginx\logs\nginx.pid
```

7. **再执行 reload**
```powershell
.\nginx.exe -s reload
```

如果出现 `OpenEvent("Global\ngx_reload_xxx") failed`，通常是 `nginx.pid` 与实际运行进程不一致，重复上述 1~7 步即可恢复。

8. **确认线上返回的是新证书**
```powershell
# PowerShell 方式（无需 < NUL 重定向）
$tcp = New-Object Net.Sockets.TcpClient("app.abc.cloud",443)
$ssl = New-Object Net.Security.SslStream($tcp.GetStream(),$false,({$true}))
$ssl.AuthenticateAsClient("app.abc.cloud")
$cert = New-Object System.Security.Cryptography.X509Certificates.X509Certificate2($ssl.RemoteCertificate)
$cert | Select-Object Subject,Issuer,NotBefore,NotAfter,Thumbprint
$ssl.Dispose(); $tcp.Close()
```

9. **如浏览器仍报旧证书，继续排查**
- 清理本机 DNS 缓存：`ipconfig /flushdns`
- 使用无痕窗口测试
- 检查域名解析是否命中当前服务器
- 若使用 CDN（如 Cloudflare），确认边缘证书也已更新

### 10.4 连接超时
- 检查防火墙设置
- 验证后端应用是否响应
- 增加 Nginx 超时设置

### 10.5 查看日志
```powershell
# 访问日志
Get-Content C:\nginx\logs\access.log -Tail 50

# 错误日志
Get-Content C:\nginx\logs\error.log -Tail 50
```

## 十一、性能优化建议

### 11.1 启用 Gzip 压缩
在 `http` 块中添加：
```nginx
gzip on;
gzip_vary on;
gzip_min_length 1024;
gzip_types text/plain text/css text/xml text/javascript application/x-javascript application/xml+rss application/json;
```

### 11.2 调整 Worker 进程数
根据 CPU 核心数调整：
```nginx
worker_processes  auto;
```

## 十二、安全建议

1. **定期更新 Nginx** 到最新稳定版本
2. **使用强密码** 保护管理后台
3. **限制访问速率** 防止 DDoS 攻击
4. **配置 HSTS** 头强制 HTTPS
5. **定期更新 SSL 证书**
6. **配置 WAF** Web 应用防火墙

## 维护检查清单

- [ ] 每月检查 Nginx 版本并更新
- [ ] 每月检查 SSL 证书有效期
- [ ] 每周检查日志文件大小并清理
- [ ] 每季度进行安全扫描
- [ ] 定期备份 Nginx 配置文件

---

**文档版本**: 1.1  
**最后更新**: 2026年3月19日  
**适用版本**: AI.DeliciousFood Project
