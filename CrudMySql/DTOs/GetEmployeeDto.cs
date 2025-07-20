namespace CrudMySql.DTOs;

public readonly record struct GetEmployeeDto(string empId, string empName, string emailAddress, decimal salary, string created, int depId);

