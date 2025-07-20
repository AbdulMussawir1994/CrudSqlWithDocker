using CrudMySql.DTOs;
using CrudMySql.Helpers;

namespace CrudMySql.Repositories;

public interface IEmployeeLayer
{
    Task<MobileResponse<IEnumerable<GetEmployeeDto>>> GetEmployeesListAsync(CancellationToken ctx);
    Task<MobileResponse<GetEmployeeDto>> CreateEmployeeAsync(CreateEmployeeDto model, CancellationToken ctx);
    Task<MobileResponse<GetEmployeeDto>> UpdateEmployeeAsync(string id, CreateEmployeeDto model, CancellationToken ctx);
    Task<MobileResponse<GetEmployeeDto>> GetEmployeeByIdAsync(string id, CancellationToken ctx);
    Task<MobileResponse<bool>> DeleteEmployeeAsync(string id, CancellationToken ctx);
}
