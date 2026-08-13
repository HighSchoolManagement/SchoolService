using Microsoft.AspNetCore.Mvc;
using SchoolService.Application.Schools.CreateSchool;
using SchoolService.Application.Schools.GetSchools;

namespace SchoolService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchoolsController : ControllerBase
    {
        private readonly GetSchoolsHandler _getSchoolsHandler;
        private readonly CreateSchoolHandler _createSchoolHandler;
        public SchoolsController(GetSchoolsHandler getSchoolsHandler, CreateSchoolHandler createSchoolHandler)
        {
            _getSchoolsHandler = getSchoolsHandler;
            _createSchoolHandler = createSchoolHandler;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var schools = await _getSchoolsHandler.HandleAsync();
            return Ok(schools);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateSchoolRequest request)
        {
            var school = await _createSchoolHandler.HandleAsync(request);
            return CreatedAtAction(nameof(Get), new { id = school.Id }, school);
        }
    }
}
