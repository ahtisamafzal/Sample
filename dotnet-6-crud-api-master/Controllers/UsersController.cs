namespace WebApi.Controllers;

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebApi.Entities;
using WebApi.Models.Users;
using WebApi.Services;

[ApiController]
[Route("[controller]")]
public class UsersController : BaseCURDController<User>
{
    private IUserService<User> _userService;
    private IMapper _mapper;

    public UsersController(
        IUserService<User> userService,
        IMapper mapper) : base(userService, mapper)
    {
        _userService = userService;
        _mapper = mapper;
    } 
    
    
}