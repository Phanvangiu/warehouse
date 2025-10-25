using warehouse.RequestModels;

namespace warehouse.Services
{
  public interface IMailService
  {
    Task SendEmailAsync(MailRequest mailrequest);

    Task<bool> SendMail(MailRequest request);
  }
}