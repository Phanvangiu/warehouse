using Microsoft.AspNetCore.Mvc;
using warehouse.Interfaces;
using warehouse.RequestModels;
using warehouse.ReturnModels;
using warehouse.Services;

namespace warehouse.Controllers

{
  [Route("api/[controller]")]
  [ApiController]
  public class AuthController : ControllerBase
  {
    private IUnitOfWork _unitOfWork;
    private IMailService _mailService;
    public AuthController(IUnitOfWork unitOfWork, IMailService mailService)
    {
      _unitOfWork = unitOfWork;
      _mailService = mailService;
    }
    [HttpPost]
    [Route("admin-login")]
    public async Task<IActionResult> AdminLogin([FromForm] RequestLogin account)
    {
      var customResult = await _unitOfWork.UserRepository.AdminLogin(account);
      return Ok(customResult);
    }


    [HttpPost("send")]
    public async Task<IActionResult> SendTestMail([FromBody] MailRequest request)
    {
      if (string.IsNullOrEmpty(request.ToEmail))
        return BadRequest("Email is required");

      request.Subject = "Test Mail";
      request.Body = "This is a test email from MailKit!";

      await _mailService.SendEmailAsync(request);
      return Ok("Mail sent successfully!");
    }

  }
}