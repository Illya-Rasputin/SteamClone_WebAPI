using API.Extensions;
using BLL.DTO.Game;
using BLL.Services;
using DAL.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BLL.DTO.Genre;

namespace API.Controllers
{
    [ApiController]
    [Route("api/genre")]
    public class GenreController : ControllerBase
    {
        private readonly GenreService _genreService;

        public GenreController(GenreService genreService)
        {
            _genreService = genreService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            var response = await _genreService.GetAllAsync();
            return this.GetResult(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var response = await _genreService.GetByIdAsync(id);
            return this.GetResult(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromForm] CreateGenreDTO dto)
        {
            var response = await _genreService.CreateAsync(dto);
            return this.GetResult(response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromForm] UpdateGenreDTO dto)
        {
            var response = await _genreService.UpdateAsync(dto);
            return this.GetResult(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id)
        {
            var response = await _genreService.DeleteAsync(id);
            return this.GetResult(response);
        }

    }
}
