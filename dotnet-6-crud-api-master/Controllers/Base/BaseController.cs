namespace WebApi.Controllers;

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Entities;
using WebApi.Models.Users;
using WebApi.Services;

[ApiController]
[Route("[controller]")]
    public class BaseCURDController<TEntity> : ControllerBase where TEntity : User
    {
        private IUserService<TEntity> _userService;
        private IMapper _mapper;          

        public BaseCURDController(IUserService<TEntity> userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;             
        }

        // [HttpGet]
        // public async Task<IEnumerable<TEntity>> GetAllAsync()
        // {
        //     var users = _userService.GetAllAsync();
        //     return await users;
        // }  
        
        [HttpGet]
        public async Task<IEnumerable<TEntity>> GetAllAsync(int count = -1, int skip = -1, string searchTerm = null, string orderBy = null)
        {
            var users = _userService.GetAllAsync(count, skip, searchTerm);
            return await users;

            // if (!String.IsNullOrEmpty(searchTerm))
            // {
            //     return await users.Where(d => d.Email.Contains(searchTerm)).AsQueryable().ToListAsync();
            // }

            // if (!String.IsNullOrEmpty(orderBy))
            // {
            //     return await users.OrderBy(orderBy).AsQueryable().ToListAsync();
            // }

            //return await GetAllAsync(count, skip, searchTerm);
        }
        
        [HttpGet]
        [Route("Inactive")]
        public async Task<IEnumerable<TEntity>> GetAllDeletedAsync()
        {
            var users = _userService.GetAllDeletedAsync();
            return await users;
        }

        [HttpGet("{id}")]        
        public async Task<TEntity> GetByIdAsync(int id)
        {
            var user = _userService.GetByIdAsync(id);
            return await user;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateRequest model)
        {
            await _userService.CreateAsync(model);
            return Ok(new { message = "Created" });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, UpdateRequest model)
        {
            await _userService.UpdateAsync(id, model);
            return Ok(new { message = "Updated" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            await _userService.DeleteAsync(id);
            return Ok(new { message = "Deleted" });
        }
  
    }

