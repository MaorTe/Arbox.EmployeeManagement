using Application.Interfaces;
using Core.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;
public class DepartmentRepository : IDepartmentRepository
{
    private readonly AppDbContext _context;

    public DepartmentRepository(AppDbContext context) {
        _context = context;
    }

    public async Task<IEnumerable<Department>> GetAllAsync() {
        return await _context.Departments.ToListAsync();
    }

    public async Task<Department?> GetByIdAsync(int id) {
        return await _context.Departments.FindAsync(id);
    }

    public async Task<Department> CreateAsync(Department department) {
        _context.Departments.Add(department);
        await _context.SaveChangesAsync();
        return department;
    }

    public async Task UpdateAsync(Department department) {
        var existing = await _context.Departments.FindAsync(department.Id);
        if (existing == null) return;

        existing.Name = department.Name;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id) {
        var existing = await _context.Departments.FindAsync(id);
        if (existing == null) return;

        _context.Departments.Remove(existing);
        await _context.SaveChangesAsync();
    }
}