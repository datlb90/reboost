using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Reboost.DataAccess.Entities;

namespace Reboost.DataAccess.Repositories
{
    public interface IRaterRepository: IRepository<Rater>
    {
        //Task<List<Rater>> GetAllAsync();
        //Task<Rater> GetByIdAsync(int id);
        //Task<Rater> CreateAsync(Rater rater);
        //Task<Rater> UpdateAsync(Rater rater);
        //Task<Rater> DeleteAsync(int id);


    }
    public class RaterRepository : Repository<Rater>, IRaterRepository
    {
        public RaterRepository(ReboostDbContext context): base(context)
        { }

    //    public async Task<Rater> CreateAsync(Rater rater)
    //    {
    //        Context.Raters.Add(rater);
    //        await Context.SaveChangesAsync();
    //        return rater;
    //    }

    //    public async Task<Rater> DeleteAsync(int id)
    //    {
    //        Rater _rater = await Context.Raters.SingleOrDefaultAsync(r => r.Id == id);
    //        Context.Raters.Remove(_rater);
    //        await Context.SaveChangesAsync();
    //        return _rater;
    //    }

    //    public async Task<List<Rater>> GetAllAsync()
    //    {
    //        return await Context.Raters.ToListAsync();
    //    }

    //    public async Task<Rater> GetByIdAsync(int id)
    //    {
    //        return await Context.Raters.Where(r => r.Id == id).FirstAsync();
    //    }

    //    public async Task<Rater> UpdateAsync(Rater rater)
    //    {
    //        Rater _rater = await Context.Raters.SingleOrDefaultAsync(r => r.Id == rater.Id);
    //        _rater.FirstName = rater.FirstName;
    //        _rater.LastName = rater.LastName;
    //        _rater.Gender = rater.Gender;
    //        _rater.Occupation = rater.Occupation;
    //        _rater.FirstLanguage = rater.FirstLanguage;
    //        _rater.idPhoto = rater.idPhoto;
    //        _rater.Biography = rater.Biography;
    //        _rater.ApplicationDate = rater.ApplicationDate;
    //        _rater.Status = rater.Status;
    //        _rater.ScoreReport = rater.ScoreReport;
    //        _rater.AppliedDate = rater.AppliedDate;
    //        _rater.Note = rater.Note;
    //        _rater.LastActivityDate = rater.LastActivityDate;
    //        await Context.SaveChangesAsync();
    //        return _rater;
    //}
    }
}
