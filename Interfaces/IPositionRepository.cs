using Microsoft.EntityFrameworkCore;
using warehouse.Data;
using warehouse.Models;
using warehouse.RequestModels;
using warehouse.ReturnModels;

namespace warehouse.Interfaces
{
  public interface IPositionRepository : IRepository<Position>
  {
    Task<CustomResult> GetPositions();
    Task<CustomResult> CreatePosition(CreatePositionModel createPositionModel);
    Task<CustomResult> GetPosition(int id);
  }
  public class PositionRepository : GenericRepository<Position>, IPositionRepository
  {
    public PositionRepository(DataContext dataContext) : base(dataContext)
    {
    }
    public async Task<CustomResult> GetPositions()
    {
      try
      {
        var positions = await _context.Positions.ToListAsync();
        if (positions.Count == 0)
        {
          return new CustomResult(200, "List empty", null!);
        }
        return new CustomResult(200, "Positions retrieved successfully", positions);
      }
      catch (Exception ex)
      {
        return new CustomResult(500, $"An error occurred while retrieving positons: {ex.Message}", null!);
      }
    }
    public async Task<CustomResult> CreatePosition(CreatePositionModel createPositionModel)
    {
      try
      {
        if (createPositionModel is null)
        {
          return new CustomResult(400, "Invalid request: position data cannot be null.", null!);
        }
        if (string.IsNullOrWhiteSpace(createPositionModel.Title))
        {
          return new CustomResult(400, "Title is required", null!);
        }
        var positionNew = new Position
        {
          Title = createPositionModel.Title,
          Description = createPositionModel.Description
        };
        _context.Positions.Add(positionNew);
        await _context.SaveChangesAsync();
        return new CustomResult(200, "Position created successfully", positionNew);
      }
      catch (Exception ex)
      {
        return new CustomResult(500, $"An error occurred while creating position: {ex.Message}", null!);
      }
    }
    public async Task<CustomResult> GetPosition(int id)
    {
      try
      {
        if (id <= 0)
          return new CustomResult(400, "Invalid ID. ID must be greater than 0.", null!);

        var position = await _context.Positions.FindAsync(id);
        if (position is null)
          return new CustomResult(200, "Position not found", null!);

        return new CustomResult(200, "Position retrieved successfully", position);
      }
      catch (Exception ex)
      {
        return new CustomResult(500, $"An error occurred while retrieving position: {ex.Message}", null!);
      }
    }

  }
}