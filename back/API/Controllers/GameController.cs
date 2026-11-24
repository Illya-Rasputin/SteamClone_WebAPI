using API.Extensions;
using BLL.DTO.Developer;
using BLL.Services;
using DAL;
using DAL.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BLL.DTO.Game;

namespace API.Controllers
{
    [ApiController]
    [Route("api/game")]
    public class GameController : ControllerBase
    {
        private readonly GameService _gameService;

        public GameController(GameService gameService)
        {
            _gameService = gameService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            var response = await _gameService.GetAllAsync();
            return this.GetResult(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _gameService.GetByIdAsync(id);
            return this.GetResult(response);
        }
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
        public async Task<IActionResult> CreateAsync([FromForm] CreateGameDTO dto)
        {
            var response = await _gameService.CreateAsync(dto);
            return this.GetResult(response);
        }
        [HttpPut]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
        public async Task<IActionResult> UpdateAsync([FromForm] UpdateGameDTO dto)
        {
            var response = await _gameService.UpdateAsync(dto);
            return this.GetResult(response);
        }
        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id)
        {
            var response = await _gameService.DeleteAsync(id);
            return this.GetResult(response);
        }
    }
}
