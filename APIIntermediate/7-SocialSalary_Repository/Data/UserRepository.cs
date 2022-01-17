using AutoMapper;
using SocialSalary.Models;
using Microsoft.EntityFrameworkCore;

namespace SocialSalary.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContextEF _entityFramework;
        
        public UserRepository(DataContextEF context)
        {
            _entityFramework = context;
        }
        public void Add<T>(T entity) where T : class
        {
            _entityFramework.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _entityFramework.Remove(entity);
        }

        public async Task<bool> SaveAll()
        {
            return await _entityFramework.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<Users>> GetUsers()
        {
            return await _entityFramework.Users.ToListAsync();
        }
        public async Task<Users> GetUser(int id)
        {
            var user = await _entityFramework.Users.FirstOrDefaultAsync(u => u.UserId == id);
            return user;
        }

    }
}