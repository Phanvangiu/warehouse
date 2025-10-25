using arts_core.Service;
using Microsoft.Extensions.Options;
using warehouse.Setting;

namespace warehouse.Services
{
  public class MailService : IMailService
  {
    private readonly MailSetting _mailSetting;
    private readonly IConfiguration _configuration;
    private readonly ILogger<MailService> _logger;
    public MailService(IOptions<MailSetting> mailSetting, IConfiguration configuration, ILogger<MailService> logger)
    {
      _mailSetting = mailSetting.Value;
      _configuration = configuration;
      _logger = logger;
    }
    public class MailRequestNhan
    {
      public string To { get; set; } = string.Empty;
      public string Subject { get; set; } = string.Empty;
      public string Body { get; set; } = string.Empty;
      public MailRequestNhan(string to, string subject, string body)
      {
        To = to;
        Subject = subject;
        Body = body;
      }
    }

  }
}