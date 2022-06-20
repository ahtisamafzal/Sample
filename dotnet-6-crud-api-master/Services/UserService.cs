namespace WebApi.Services;

using AutoMapper;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using WebApi.Entities;
using WebApi.Entities.Base;
using WebApi.Helpers;
using WebApi.Models.Users;

public class UserService<T> : IUserService<T> where T : User
{
    private DataContext _context;
    private readonly IMapper _mapper;
    protected DbSet<T> _dbSet { get; set; }

    public UserService(
        DataContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
        _dbSet = _context.Set<T>();
    }
        
    // public async Task<IEnumerable<T>> GetAllAsync()
    // {
    //     return await _dbSet.Where(x => x.IsActive == true).ToListAsync();
    // }

    public virtual async Task<IEnumerable<T>> GetAllAsync(int count = -1, int skip = -1, string searchTerm = null, string sortBy = null)
    {
        if (!String.IsNullOrEmpty(searchTerm))
        {
            throw new Exception("Cannot search in base controller.");
        }

        if (!String.IsNullOrEmpty(sortBy))
        {
            throw new Exception("Cannot sort in base controller.");
        }

        if (count != -1 && skip != -1)
        {
            return await _dbSet.Where(x => x.IsActive == true).Skip(skip).Take(count).ToListAsync();
        }

        return await _dbSet.Where(x => x.IsActive == true).ToListAsync();
    }

    
    public async Task<T> GetByIdAsync(int id)
    {           
        return await getUser(id);
    }    

    public async Task<IEnumerable<T>> GetAllDeletedAsync()
    {
        return await _dbSet.Where(x => x.IsActive == false).ToListAsync();
    }  
    
    public async Task CreateAsync(CreateRequest model)
    {
        // validate
        if (_context.Users.Any(x => x.Email == model.Email))
            throw new AppException("User with the email '" + model.Email + "' already exists");

        // map model to new user object
        var user = _mapper.Map<User>(model);

        // hash password
        user.PasswordHash = BCrypt.HashPassword(model.Password);

        // save user        
        _dbSet.Add((T)user);
       await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(int id, UpdateRequest model)
    {           
        var user = await _dbSet.Where(e => e.Id == id).FirstOrDefaultAsync();
        // validate
        if (model.Email != user.Email && _context.Users.Any(x => x.Email == model.Email))
            throw new AppException("User with the email '" + model.Email + "' already exists");
        
        // hash password if it was entered
        if (!string.IsNullOrEmpty(model.Password))
            user.PasswordHash = BCrypt.HashPassword(model.Password);

        _mapper.Map(model, user);        
        _dbSet.Update(user);
        await _context.SaveChangesAsync();        
    }    


    public async Task DeleteAsync(int id)
    {           
        var entity = await getUser(id);
        entity.IsActive = false;
        entity.ModifiedDate = DateTime.UtcNow;
        _dbSet.Update(entity);
        await _context.SaveChangesAsync();    
    }    

    // helper methods
    private async Task<T> getUser(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) throw new KeyNotFoundException("User not found");
        return (T)(object)user;
    }
    
}