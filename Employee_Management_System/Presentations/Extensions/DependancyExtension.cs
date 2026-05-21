using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Employee_Management_System.Applications.Repositories;
using Employee_Management_System.Infrastructures.Adapters;
using Employee_Management_System.Infrastructures.Context;
using Employee_Management_System.Infrastructures.Repositories;
using Employee_Management_System.Presentations.ViewModels.Adapters;
using Microsoft.EntityFrameworkCore;

namespace Employee_Management_System.Presentations.Extensions;
public static class DependancyExtension
{
    public static void SettingDependencyInjection(
        this IServiceCollection services, IConfiguration configuration)
    {
        SettingEntityFrameworkCore(configuration, services);
        SettingInfrastructures(services);
        SettingApplications(services);
        SettingPresentations(services);
    }
    
    private static void SettingEntityFrameworkCore(IConfiguration configuration, IServiceCollection services)
    {
        var connectionString = configuration.GetConnectionString("PostgreSqlConnection");
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(connectionString));
    }

    private static void SettingInfrastructures(IServiceCollection services)
    {
        services.AddScoped<DepartmentEntityAdapter>();
        services.AddScoped<EmployeeEntityAdapter>();
        services.AddScoped<AdminEntityAdapter>();
        services.AddScoped<IDepartmentRepository, DepartmentRepository>();
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        services.AddScoped<IAdminRepository, AdminRepository>();
    }

    private static void SettingApplications(IServiceCollection services)
    {
        // services.AddScoped<IDepartmentService, DepartmentService>();
        // services.AddScoped<IEmployeeService, EmployeeService>();
        // services.AddScoped<IAdminService, AdminService>();
    }

    private static void SettingPresentations(IServiceCollection services)
    {
        services.AddScoped<DepartmentViewModelAdapter>();
        services.AddScoped<EmployeeViewModelAdapter>();
        services.AddScoped<EmployeeViewModelAdapter>();
    }       
}