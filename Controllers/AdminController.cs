using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using warehouse.Interfaces;
using warehouse.RequestModels;
using warehouse.ReturnModels;

namespace warehouse.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class AdminController : ControllerBase
  {
    private IUnitOfWork _unitOfWork;

    public AdminController(IUnitOfWork unitOfWork)
    {
      _unitOfWork = unitOfWork;
    }
    [HttpGet]
    [Authorize(Roles = "Admin")]
    [Route("get-all-customer")]
    public async Task<IActionResult> GetAllCustomer([FromQuery] int pageNumber, [FromQuery] int pageSize, [FromQuery] string searchValue = "")
    {
      var customResult = await _unitOfWork.UserRepository.GetAllCustomer(pageNumber, pageSize, searchValue);
      return Ok(customResult);
    }
    [HttpGet]
    [Authorize(Roles = "Admin")]
    [Route("get-all-employee")]
    public async Task<IActionResult> GetAllEmployee()
    {
      var customResult = await _unitOfWork.UserRepository.GetAllEmployee();
      return Ok(customResult);
    }
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [Route("create-employee")]
    public async Task<IActionResult> CreateEmployee([FromForm] CreateEmployee employeeModel)
    {
      var customResult = await _unitOfWork.UserRepository.CreateEmployee(employeeModel);
      return Ok(customResult);
    }
    [HttpPut]
    [Authorize(Roles = "Admin")]
    [Route("active-employee/{id}")]
    public async Task<IActionResult> ActiveEmployee(int id)
    {
      var customResult = await _unitOfWork.UserRepository.ActivateEmployee(id);
      return Ok(customResult);
    }
    [HttpPut]
    [Authorize(Roles = "Admin")]
    [Route("deactive-employee/{id}")]
    public async Task<IActionResult> DeActiveEmployee(int id)
    {
      var customResult = await _unitOfWork.UserRepository.DeactivateEmployee(id);
      return Ok(customResult);
    }
  }
}