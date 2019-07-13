using JazugNight.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;

namespace JazugNight.Server
{
    public class EmployeesService
    {
        [FunctionName("GetEmployees")]
        public IActionResult GetEmployees(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "employees")]HttpRequest req,
            [CosmosDB("db", "emps", ConnectionStringSetting = "CosmosDB", SqlQuery = "SELECT * FROM emps e WHERE e.partitionkey = '0'")]IEnumerable<Employee> emps)
        {
            return new OkObjectResult(emps);
        }

        [FunctionName("GetEmployeeById")]
        public IActionResult GetEmployeeById(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "employees/{id}")]HttpRequest req,
            [CosmosDB("db", "emps", ConnectionStringSetting = "CosmosDB", Id = "{id}", PartitionKey = "0", CreateIfNotExists = true)]Employee emp)
        {
            if (emp == null)
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(emp);
        }

        [FunctionName("CreateEmployee")]
        public async Task<IActionResult> CreateEmployee(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "employees")]Employee req,
            [CosmosDB("db", "emps", ConnectionStringSetting = "CosmosDB", PartitionKey = "/partitionkey", CreateIfNotExists = true)]IAsyncCollector<Employee> emps)
        {
            if (string.IsNullOrEmpty(req?.Name))
            {
                return new BadRequestResult();
            }

            await emps.AddAsync(req);
            await emps.FlushAsync();
            return new CreatedResult("", null);
        }
    }
}
