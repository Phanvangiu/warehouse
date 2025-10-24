using Microsoft.AspNetCore.Mvc;
using warehouse.Interfaces;
using warehouse.ReturnModels;

namespace warehouse.Controllers

{
  [Route("api/[controller]")]
  [ApiController]
  public class AuthController : ControllerBase
  {
    private IUnitOfWork _unitOfWork;
    public AuthController(IUnitOfWork unitOfWork)
    {
      _unitOfWork = unitOfWork;
    }
    [HttpPost]
    [Route("admin-login")]
    public async Task<IActionResult> AdminLogin([FromForm] RequestLogin account)
    {
      var customResult = await _unitOfWork.UserRepository.AdminLogin(account);

      return Ok(customResult);
    }

  }
}