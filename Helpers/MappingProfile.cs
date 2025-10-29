using AutoMapper;
using warehouse.Models;
using warehouse.ReturnModels;

namespace warehouse.Helpers
{
  public class MappingProfile : Profile
  {
    public MappingProfile()
    {
      CreateMap<User, UserResult>();
    }
  }
}