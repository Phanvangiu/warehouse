using Microsoft.AspNetCore.Mvc;
using warehouse.Interfaces;

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

  }
}