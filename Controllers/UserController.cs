using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using warehouse.Interfaces;
using warehouse.ReturnModels;

namespace warehouse.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class UserController : ControllerBase
  {
    private IUnitOfWork _unitOfWork;
    public UserController(IUnitOfWork unitOfWork)
    {
      _unitOfWork = unitOfWork;
    }
    [HttpGet]
    [Route("get-user")]
    public async Task<IActionResult> GetUser()
    {
      var email = User.FindFirst(ClaimTypes.Email)?.Value;

      if (string.IsNullOrEmpty(email))
      {
        return Ok(new CustomResult(401, "Invalid token or email not found in claims", null));
      }
      var customResult = await _unitOfWork.UserRepository.GetUser(email);
      return Ok(customResult);
    }
  }
}