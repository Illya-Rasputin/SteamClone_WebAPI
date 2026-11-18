using API.Extensions;
using BLL.DTO.Developer;
using BLL.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/developer")]
    public class DeveloperController : ControllerBase
    {
        private readonly DeveloperService _developerService;

        public DeveloperController(DeveloperService developerService)
        {
            _developerService = developerService;
        }

        // api/developer
        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            var response = await _developerService.GetAllAsync();
            return this.GetResult(response);
        }

        // api/developer/5
        [HttpGet("by-id/{id}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
        {
            var response = await _developerService.GetByIdAsync(id);
            return this.GetResult(response);
        }

        [HttpGet("by-name/{name}")]
        public async Task<IActionResult> GetByNameAsync([FromRoute] string name)
        {
            var response = await _developerService.GetByNameAsync(name);
            return this.GetResult(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromForm] CreateDeveloperDTO dto)
        {
            var response = await _developerService.CreateAsync(dto);
            return this.GetResult(response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromForm] UpdateDeveloperDTO dto)
        {
            var response = await _developerService.UpdateAsync(dto);
            return this.GetResult(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id)
        {
            var response = await _developerService.DeleteAsync(id);
            return this.GetResult(response);
        }
    }
}
