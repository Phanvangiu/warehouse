using warehouse.Models;
using warehouse.RequestModels;

namespace arts_core.Service
{
  public interface IMailService
  {
    Task SendEmailAsync(MailRequest mailrequest);

    Task<bool> SendMail(MailRequestNhan request);
  }
}