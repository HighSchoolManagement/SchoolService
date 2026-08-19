using Microsoft.AspNetCore.Mvc;
using SchoolService.Application.Schools.CreateSchool;
using SchoolService.Application.Schools.DeleteSchool;
using SchoolService.Application.Schools.GetSchoolById;
using SchoolService.Application.Schools.GetSchools;
using SchoolService.Application.Schools.UpdateSchool;

namespace SchoolService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchoolsController : ControllerBase
    {
        private readonly GetSchoolsHandler _getSchoolsHandler;
        private readonly CreateSchoolHandler _createSchoolHandler;
        private readonly GetSchoolByIdHandler _getSchoolByIdHandler;
        private readonly UpdateSchoolHandler _updateSchoolHandler;
        private readonly DeleteSchoolHandler _deleteSchoolHandler;
        public SchoolsController(GetSchoolsHandler getSchoolsHandler, CreateSchoolHandler createSchoolHandler, GetSchoolByIdHandler getSchoolByIdHandler, UpdateSchoolHandler updateSchoolHandler, DeleteSchoolHandler deleteSchoolHandler)
        {
            _getSchoolsHandler = getSchoolsHandler;
            _createSchoolHandler = createSchoolHandler;
            _getSchoolByIdHandler = getSchoolByIdHandler;
            _updateSchoolHandler = updateSchoolHandler;
            _deleteSchoolHandler = deleteSchoolHandler;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            var result = await _getSchoolsHandler.HandleAsync(page, pageSize);
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var school = await _getSchoolByIdHandler.HandleAsync(id);
            return Ok(school);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateSchoolRequest request)
        {
            var school = await _createSchoolHandler.HandleAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = school.Id }, school);
        }

        [HttpPatch("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateSchoolRequest request)
        {
            await _updateSchoolHandler.HandleAsync(id, request);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            await _deleteSchoolHandler.HandleAsync(id);
            return NoContent();
        }
    }
}
