using CrudMySql.DTOs;
using CrudMySql.Models;
using Mapster;

namespace CrudMySql.Helpers;

public sealed class MapsterProfile : TypeAdapterConfig
{
    public MapsterProfile()
    {
        EmployeeMapping();
    }

    private void EmployeeMapping()
    {
        TypeAdapterConfig<Employee, GetEmployeeDto>.NewConfig()
            .Map(x => x.empId, map => map.Id)
            .Map(x => x.empName, map => map.Name)
            .Map(x => x.emailAddress, map => map.Email)
            .Map(x => x.salary, map => map.Salary)
            .Map(x => x.created, map => map.CreatedDate)
            .Map(x => x.depId, map => map.DepartmentId)
            .IgnoreNullValues(true);

        TypeAdapterConfig<CreateEmployeeDto, Employee>.NewConfig()
            .Map(x => x.Name, map => map.Name)
            .Map(x => x.Email, map => map.Email)
            .Map(x => x.Salary, map => map.Salary)
            .Map(x => x.DepartmentId, map => map.DepartmentId)
           .IgnoreNullValues(true);
    }
}
