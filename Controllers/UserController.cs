using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using warehouse.Interfaces;
using warehouse.RequestModels;
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
        return Ok(new CustomResult(401, "Invalid token or email not found in claims", null!));
      }
      var customResult = await _unitOfWork.UserRepository.GetUser(email);
      return Ok(customResult);
    }
    [HttpPut]
    [Route("change-password")]
    [Authorize]
    public async Task<IActionResult> ChangePassword([FromForm] ChangePasswordModel changePasswordModel)
    {
      var idClaim = User.Claims.FirstOrDefault(c => c.Type == "Id");

      if (idClaim == null)
      {
        return Ok(new CustomResult(401, "Invalid token: missing Id claim", null!));
      }

      if (!int.TryParse(idClaim.Value, out int userId))
      {
        return Ok(new CustomResult(401, "Invalid token: Id claim not valid", null!));
      }

      var customResult = await _unitOfWork.UserRepository.ChangePassword(userId, changePasswordModel);

      return Ok(customResult);
    }

    [HttpPut("change-avatar")]
    [Authorize]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> ChangeUserAvatar([FromForm] ChangeAvatarModel request)
    {
      var email = User.FindFirst(ClaimTypes.Email)?.Value;

      if (string.IsNullOrEmpty(email))
      {
        return Ok(new CustomResult(404, "Please login before updating avatar", null!));
      }

      var customResult = await _unitOfWork.UserRepository.ChangeUserImage(email, request.Image);
      return Ok(customResult);
    }
    [HttpGet]
    [Route("get-customers-paging")]
    public async Task<IActionResult> GetAllCustomer([FromQuery] int pageNumber, [FromQuery] int pageSize, [FromQuery] string? searchValue)
    {
      var customPaging = await _unitOfWork.UserRepository.GetAllCustomer(pageNumber, pageSize, searchValue);
      return Ok(customPaging);
    }
    [HttpPut]
    [Route("active-user")]
    public async Task<IActionResult> ActivateEmployee([FromBody] int id)
    {
      var customResult = await _unitOfWork.UserRepository.ActivateEmployee(id);
      return Ok(customResult);
    }
    [HttpPut]
    [Route("deactive-user")]
    public async Task<IActionResult> DeActivateEmployee([FromBody] int id)
    {
      var customResult = await _unitOfWork.UserRepository.DeactivateEmployee(id);
      return Ok(customResult);
    }
  }
}