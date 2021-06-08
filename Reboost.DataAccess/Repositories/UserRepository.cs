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
        Task<List<UserScores>> GetUserScores(string userId);
        Task<bool> HasSubmissionOnTaskOf(string userId, int questionId);
        Task<UserRanks> GetUserRate(string userId);
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

        public async Task<List<UserScores>> GetUserScores(string userId)
        {
            return await _context.UserScores.Where(u => u.UserId == userId).ToListAsync();
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

        public async Task<bool> HasSubmissionOnTaskOf(string userId, int questionId)
        {
            var userSubmissionTasks = from s in _context.Submissions
                                      join q in _context.Questions on s.QuestionId equals q.Id
                                      join t in _context.Tasks on q.TaskId equals t.Id
                                      where s.UserId == userId
                                      select t;

            var questionTask = await (from q in _context.Questions
                                join t in _context.Tasks on q.TaskId equals t.Id
                                where q.Id == questionId
                                select t).FirstOrDefaultAsync();

            if (questionTask == null)
                return false;

            return userSubmissionTasks.Any(t => t.Id == questionTask.Id);
        }

        public async Task<UserRanks> GetUserRate(string userId) {
            return await _context.UserRanks.Where(r => r.UserId == userId).FirstOrDefaultAsync();
        }
    }
}
