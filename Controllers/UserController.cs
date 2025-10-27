using Microsoft.AspNetCore.Mvc;
using warehouse.Interfaces;

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

  }
}