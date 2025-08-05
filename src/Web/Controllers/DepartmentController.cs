using Application.Interfaces;
using Core.Entities;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace Web.Controllers;
public class DepartmentController : Controller
{
    private readonly IDepartmentService _departmentService;

    public DepartmentController(IDepartmentService departmentService) {
        _departmentService = departmentService;
    }
    public async Task<IActionResult> Index() {
        var department = await _departmentService.GetAllAsync();
        return View(department);
    }

    [HttpGet]
    public IActionResult Create() {
        // Just show an empty form
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Department department) {
        if (!ModelState.IsValid) {
            return View(department);
        }

        await _departmentService.CreateAsync(department);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id) {
        var department = await _departmentService.GetByIdAsync(id);
        if (department == null)
            return NotFound();

        return View(department);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Department department) {
        if (!ModelState.IsValid) {
            return View(department);
        }
        await _departmentService.UpdateAsync(department);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id) {
        var department = await _departmentService.GetByIdAsync(id);
        if (department == null)
            return NotFound();
        
        // 2. See if it has any employees
        if (await _departmentService.HasEmployeesAsync(id)) {
            // Block deletion and show an error message
            TempData["Error"] =
               "Cannot delete this department while it has employees. " +
               "Please reassign or remove its employees first.";
            return RedirectToAction(nameof(Index));
        }

        await _departmentService.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
