using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces;
public interface IDashboardService
{
    Task<int> GetTotalEmployeeCountAsync();
    Task<Dictionary<int, int>> GetEmployeeCountByDepartmentAsync();
    Task<IEnumerable<Employee>> GetRecentHiresAsync();
}
