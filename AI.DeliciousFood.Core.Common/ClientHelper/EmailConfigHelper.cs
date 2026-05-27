using Microsoft.Extensions.Configuration;

namespace AI.DeliciousFood.Core.Common.ClientHelper;

public class EmailConfigHelper
{
    [ConfigurationKeyName("From")]
    public required string SmtpFrom { get; set; }

    [ConfigurationKeyName("Host")]
    public required string SmtpHost { get; set; }

    [ConfigurationKeyName("Port")]
    public int SmtpPort { get; set; }

    [ConfigurationKeyName("UserName")]
    public required string SmtpUserName { get; set; }

    [ConfigurationKeyName("Password")]
    public required string FromPassword { get; set; }

    [ConfigurationKeyName("EnableSsl")]
    public bool FromEnableSsl { get; set; }
}
