using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EmployeeDataLayer;

namespace EmployeeAPI.Controllers
{
    public class EmployeeController : ApiController
    {
        public IEnumerable<Employee> Get()
        {
            EmployeeDBEntities entities = new EmployeeDBEntities();
            return entities.Employees.ToList();
        }

        public HttpResponseMessage Get(int id) { 
            var entities = new EmployeeDBEntities();
            var employee = entities.Employees.FirstOrDefault(e => e.ID == id);
           
                if (employee != null)
                { 
                    var message = Request.CreateResponse(HttpStatusCode.OK, employee);
                    return message; 
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Employee not found" + id.ToString());
                }
        }
        public HttpResponseMessage Post([FromBody] Employee employee) {
            try
            {
                using (EmployeeDBEntities enitites = new EmployeeDBEntities())
                {
                    enitites.Employees.Add(employee);
                    enitites.SaveChanges();

                    var message = Request.CreateResponse(HttpStatusCode.Created, employee);
                    message.Headers.Location = new Uri(Request.RequestUri + employee.ID.ToString());
                    return message;
                }
            }
            catch(Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e);
            }
        }

        public HttpResponseMessage Delete(int id) {
            try
            {
                using (EmployeeDBEntities entities = new EmployeeDBEntities())
                {
                    var deletedEmployee = entities.Employees.FirstOrDefault(e => e.ID == id);
                    if (deletedEmployee == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Employee to be deleted not found");
                    }
                    entities.Employees.Remove(deletedEmployee);
                    entities.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK, "Employee with id = " + deletedEmployee.ID.ToString() + " Deleted successfully");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        public HttpResponseMessage Put (int id,[FromBody]Employee employee)
        {
            try
            {
                using (EmployeeDBEntities entities = new EmployeeDBEntities())
                {
                    var existingEmployee = entities.Employees.FirstOrDefault(e => e.ID == id);
                    if (existingEmployee == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No such employee exists");
                    }
                    existingEmployee.FirstName = employee.FirstName;
                    existingEmployee.LastName = employee.LastName;
                    existingEmployee.Gender = employee.Gender;
                    existingEmployee.Salary = employee.Salary;

                    entities.SaveChanges();

                    return Request.CreateResponse(HttpStatusCode.OK, "Employee details updated succesfully");
                }
            }
            catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }

        }
    }
}
