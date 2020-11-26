using Microsoft.EntityFrameworkCore;
using Reboost.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reboost.DataAccess.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetByIdAsync(string id);
        Task<User> GetByUsernameAsync(string username);
        Task<User> GetByEmailAync(string email);
        Task<List<User>> GetAllAsync();
        Task<User> UpdateScoreAsync(string userId, List<UserScores> score);
        Task<User> UpdateAsync(User user);
    }

    public class UserRepository : IUserRepository
    {
        ReboostDbContext _context;
        public UserRepository(ReboostDbContext context)
        {
            _context = context;
        }

        public async Task<User> UpdateScoreAsync(string userId, List<UserScores> scores)
        {
            var listScore = _context.UserScores.Where(sc => sc.UserId == userId);
            _context.UserScores.RemoveRange(listScore);
            await _context.SaveChangesAsync();

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            user.UserScores = scores;
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _context.Users.Include("UserScores").AsNoTracking().ToListAsync();
        }

        public async Task<User> GetByEmailAync(string email)
        {
            return await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> GetByIdAsync(string id)
        {
            return await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            return await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<User> UpdateAsync(User user)
        {
            _context.Set<User>().Attach(user);
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return user;
        }
    }
}
