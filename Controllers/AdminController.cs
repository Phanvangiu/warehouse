using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using warehouse.Interfaces;
using warehouse.RequestModels;
using warehouse.ReturnModels;

namespace warehouse.Controllers
{

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
    public async Task<IActionResult> GetAllCustomer()
    {
      var customResult = await _unitOfWork.UserRepository.GetAllCustomer();
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
    public async Task<IActionResult> CreateEmployee([FromBody] CreateEmployee employeeModel)
    {
      var customResult = await _unitOfWork.UserRepository.CreateEmployee(employeeModel);
      return Ok(customResult);
    }
    [HttpPut]
    [Authorize(Roles = "Admin")]
    [Route("active-employee")]
    public async Task<IActionResult> ActiveEmployee([FromForm] int employeeId)
    {
      var customResult = await _unitOfWork.UserRepository.ActivateEmployee(employeeId);
      return Ok(customResult);
    }
    [HttpPut]
    [Authorize(Roles = "Admin")]
    [Route("deactive-employee")]
    public async Task<IActionResult> DeActiveEmployee([FromForm] int employeeId)
    {
      var customResult = await _unitOfWork.UserRepository.DeactivateEmployee(employeeId);
      return Ok(customResult);
    }
  }
}