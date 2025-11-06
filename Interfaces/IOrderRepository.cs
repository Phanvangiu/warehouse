using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;
using MyWebApi.Namespace;
using warehouse.Data;
using warehouse.Models;
using warehouse.RequestModels;
using warehouse.ReturnModels;

namespace warehouse.Interfaces
{
  public interface IOrderRepository : IRepository<Order>
  {
    Task<CustomPaging> GetAll(int pageNumber, int pageSize, decimal? min, decimal? max);
    Task<CustomResult> CreateOrder(CreateOrderModel model);
  }
  public class OrderRepository(DataContext dataContext) : GenericRepository<Order>(dataContext), IOrderRepository
  {
    public async Task<CustomPaging> GetAll(int pageNumber, int pageSize, decimal? min, decimal? max)
    {

      try
      {
        pageNumber = pageNumber > 0 ? pageNumber : 1;
        pageSize = pageSize > 0 ? pageSize : 10;

        if (min is not null && min < 0)
          return new CustomPaging { Status = 400, Message = "Min value must be greater than 0" };

        if (max is not null && max < 0)
          return new CustomPaging { Status = 400, Message = "Max value must be greater than 0" };

        if (min is not null && max is not null)
        {
          if (min > max)
          {
            return new CustomPaging { Status = 400, Message = "Min must be less than Max" };
          }
        }

        var query = _context.Orders
            .AsNoTracking()
            .Include(o => o.OrderItems)
            .Include(o => o.User)
            .AsQueryable();

        if (min is not null)
          query = query.Where(o => o.Total >= min);

        if (max is not null)
          query = query.Where(o => o.Total <= max);

        var totalItems = await query.CountAsync();
        var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
        var items = await query
                          .Skip((pageNumber - 1) * pageSize)
                          .Take(pageSize)
                          .ToListAsync();

        return new CustomPaging
        {
          Status = 200,
          Message = items.Count > 0 ? "Orders retrieved successfully." : "List empty",
          CurrentPage = pageNumber,
          TotalPages = totalPages,
          PageSize = pageSize,
          TotalCount = totalItems,
          Data = items
        };
      }
      catch (Exception ex)
      {
        return new CustomPaging
        {
          Status = 500,
          Message = $"Internal Server Error: {ex.Message}",
          CurrentPage = pageNumber,
          TotalPages = 0,
          PageSize = pageSize,
          TotalCount = 0,
          Data = null
        };
      }
    }

    private async Task<(IReadOnlyCollection<Product>? Products, CustomResult? Error)> ValidateProductsAsync(IReadOnlyCollection<CreateOrderItemModel> items)
    {
      if (items is null || items.Count == 0)
      {
        return (null, new CustomResult(400, "Order must contain at least one item.", null!));
      }

      var productIds = items.Select(i => i.ProductId).Distinct().ToArray();

      var products = await _context.Products.Where(p => productIds.Contains(p.Id)).ToListAsync();

      var missingIds = productIds.Except(products.Select(p => p.Id)).ToArray();
      if (missingIds.Length > 0)
      {
        return (null, new CustomResult(404, $"Products not found: {string.Join(", ", missingIds)}", null!));
      }

      return (products.AsReadOnly(), null);
    }
    private async Task<(User? user, User? employee, Store? store, CustomResult? error)> ValidateUserAndStoreAsync(int customerId, int employeeId, int storeId)
    {
      if (customerId <= 0 || employeeId <= 0 || storeId <= 1)
      {
        return (null, null, null, new CustomResult(400, "Invalid request: customerId , employeeId must be greater than 0 and storeId must be greater than 1", null!));
      }
      var customer = await _context.Users.FindAsync(customerId);
      if (customer is null)
        return (null, null, null, new CustomResult(400, "Customer not found", null!));


      var employee = await _context.Users.FindAsync(employeeId);
      if (employee is null)
        return (null, null, null, new CustomResult(400, "Employee not found", null!));

      var store = await _context.Stores.FindAsync(storeId);
      if (store is null)
        return (null, null, null, new CustomResult(400, "Store not found", null!));


      return (customer, employee, store, null);
    }
    public async Task<CustomResult> CreateOrder(CreateOrderModel model)
    {
      await using var transaction = await _context.Database.BeginTransactionAsync();
      try
      {
        if (model is null)
          return new CustomResult(400, "Invalid request; Data model is not null!", null!);

        var (products, validationError) = await ValidateProductsAsync(model.OrderItems);
        if (validationError is not null)
          return validationError;

        var (customer, employee, store, error) = await ValidateUserAndStoreAsync(model.UserId, model.EmployeeId, model.StoreId);
        if (error != null)
          return error;

        var order = new Order
        {
          UserId = model.UserId,
          EmployeeId = model.EmployeeId,
          StoreId = model.StoreId,
          Subtotal = model.Subtotal,
          DisCount = model.Discount,
          Total = model.Total,
          Coupon = model.Coupon,
          Status = model.Status,
          PaymentMethod = model.PaymentMethod,
          CreatedAt = DateTime.UtcNow
        };

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        foreach (var item in model.OrderItems)
        {
          var product = products!.First(p => p.Id == item.ProductId);
          var stockProducts = await _context.StoreStocks
                                    .Where(s => s.StoreId == model.StoreId && s.ProductId == item.ProductId && s.Quantity > 0)
                                    .OrderBy(s => s.CreatedAt)
                                    .ToListAsync();

          int remainingQty = item.Quantity;

          foreach (var stock in stockProducts)
          {
            if (remainingQty <= 0)
              break;
            // get min
            int deduction = Math.Min(stock.Quantity, remainingQty);
            stock.Quantity -= deduction;
            remainingQty -= deduction;

            _context.StoreStocks.Update(stock);
          }

          if (remainingQty > 0)
          {
            await transaction.RollbackAsync();
            return new CustomResult(400, $"Not enough stock for product '{product.ProductName}' (Product ID: {item.ProductId})", null!);
          }

          var orderItem = new OrderItem
          {
            OrderId = order.Id,
            ProductId = product.Id,
            Quantity = item.Quantity,
            UnitPrice = item.Price,
            TotalPrice = item.Quantity * item.Price
          };
          _context.OrderItems.Add(orderItem);
        }
        await _context.SaveChangesAsync();
        await transaction.CommitAsync();
        var result = await _context.Orders
                            .Include(o => o.OrderItems)
                            .Include(o => o.User)
                            .FirstOrDefaultAsync(o => o.Id == order.Id);
        return new CustomResult(200, "Order created successfully", result!);

      }
      catch (Exception ex)
      {
        await transaction.RollbackAsync();
        return new CustomResult(200, $"An error occurred while creatting order: {ex.Message}", null!);
      }
    }
  }
}