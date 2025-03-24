using System.Reflection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WaterProject.API.Data;

namespace WaterProject.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class WaterController : ControllerBase
    {
        private WaterDbContext _waterContext;
        
        public WaterController(WaterDbContext temp) => _waterContext = temp;

        [HttpGet("AllProjects")]
        public IActionResult GetProjects(int pageSize = 10, int pageNum =1, [FromQuery] List<String>?  projectTypes = null)
        {
            string? favProjType = Request.Cookies["FavoriteProjectType"];
            Console.WriteLine("~~~~~~~~~~COOKIE~~~~~~~~~~\n" + favProjType);
            
            HttpContext.Response.Cookies.Append("FavoriteProjectType", "Protected Spring", new CookieOptions
            {
                HttpOnly = true, // only accessable to the browser
                Secure = true, // only tranmitted over https
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.Now.AddMinutes(1),
            });
            
            var query = _waterContext.Projects.AsQueryable();
            
            if (projectTypes != null && projectTypes.Any())
            {
                query = query.Where(p => projectTypes.Contains(p.ProjectType));
            }
            
            var totalNumProjects = query.Count();
            
             
            var something = query
                .Skip((pageNum - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var someObject = new
            {
                Projects = something,
                TotalNumProjects = totalNumProjects
            };
            
            return Ok(someObject);
        }

        [HttpGet("GetProjectTypes")]
        public IActionResult GetProjectTypes()
        {
            var projectTypes = _waterContext.Projects
                .Select(p => p.ProjectType)
                .Distinct()
                .ToList();
            
            return Ok(projectTypes);
        }
        
    }
}
