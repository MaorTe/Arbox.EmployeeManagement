using Application.Interfaces;
using Core.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;
public class DashboardService : IDashboardService
{
    private readonly AppDbContext _context;
    public DashboardService(AppDbContext context) => _context = context;

    public async Task<int> GetTotalEmployeeCountAsync() =>
        await _context.Employees.CountAsync();

    public async Task<Dictionary<int, int>> GetEmployeeCountByDepartmentAsync() =>
        await _context.Employees
            .GroupBy(e => e.DepartmentId)
            .ToDictionaryAsync(g => g.Key, g => g.Count());

    public async Task<IEnumerable<Employee>> GetRecentHiresAsync() =>
        await _context.Employees
            .Where(e => e.HireDate >= DateTime.UtcNow.AddDays(-30))
            .ToListAsync();
}