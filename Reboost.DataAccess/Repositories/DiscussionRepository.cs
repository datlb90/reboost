using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Reboost.DataAccess.Entities;
using Reboost.DataAccess.Models;

namespace Reboost.DataAccess.Repositories
{
    public interface IDiscussionRepository : IRepository<Discussion>
    {
        Task<Discussion> GetDetailByDiscussionId(int id);
        Task<List<Discussion>> GetAllDiscussionByQuestionId(int id);
        Task<List<Discussion>> GetByUserId(string id);
        Task<Discussion> GetByDiscussionId(int id);
        Task<List<Tags>> GetAllTags();
        Task<Tags> AddTag(Tags tag);
        Task DeleteTag(Tags tag);
        Task<List<DiscussionModel>> GetComments(int id);
        Task<List<Tags>> GetAllTagsByDiscussionId(int id);
        Task<DiscussionVote> CreateDiscussionVote(DiscussionVote discussionVote);
        Task<List<DiscussionVote>> GetDiscussionVoteByDiscussionId(int id);
        Task<Discussion> IncreaseView(int id);
        Task<Discussion> UpVoteAsync(VoteModel voteModel);
        Task<Discussion> DownVoteAsync(VoteModel voteModel);
    }
    public class DiscussionRepository: BaseRepository<Discussion>, IDiscussionRepository
    {
        ReboostDbContext _context;
        private ReboostDbContext ReboostDbContext => context as ReboostDbContext;
        public DiscussionRepository(ReboostDbContext context) : base(context)
        {
            _context = context;
        }
        

        public async Task<Discussion> GetDetailByDiscussionId(int id)
        {
            var discussion = await (from q in ReboostDbContext.Discussion
                                    where q.Id == id
                                    select q).FirstOrDefaultAsync();

            return discussion;
        }
        public async Task<List<Discussion>> GetAllDiscussionByQuestionId(int id)
        {
            var discusstions = await (from q in ReboostDbContext.Discussion
                                      where q.QuestionId == id && q.ParentId == null
                                      select q).ToListAsync();
            return discusstions;
        }
        public async Task<List<Discussion>> GetByUserId(string id)
        {
            var discussions = await (from q in ReboostDbContext.Discussion
                                     where q.UserId == id
                                     select q).ToListAsync();
            return discussions;
        }
        public async Task<Discussion> GetByDiscussionId(int id)
        {
            return await ReboostDbContext.Discussion
               .Include(r => r.Tags)
               .AsNoTracking().FirstOrDefaultAsync(r => r.Id == id);
        }
        public async Task<List<Tags>> GetAllTags()
        {
            var tags = await (from q in ReboostDbContext.Tags
                                     select q).ToListAsync();
            List<Tags> rs = new List<Tags>();
            foreach(var item in tags)
            {
                var t = rs.Find(x => x.Name == item.Name);
                if (t == null)
                {
                    rs.Add(item);
                }
            }
            return rs;
        }
        public async Task<List<Tags>> GetAllTagsByDiscussionId(int id)
        {
            var tags = await (from q in ReboostDbContext.Tags
                              where q.DiscussionId == id
                              select q).ToListAsync();
            return tags;
        }

        public async Task<List<DiscussionModel>> GetComments(int id)
        {
            List<DiscussionModel> result = new List<DiscussionModel>();
            var _result = await ReboostDbContext.Discussion.Where(r => r.ParentId == id).Include(r => r.User).Include(r => r.DiscussionVote).ToListAsync();

            for (int i = 0; i < _result.Count; i++)
            {
                List<Discussion> dis = new List<Discussion>();
                DiscussionModel discussion = new DiscussionModel();
                dis = await ReboostDbContext.Discussion.Where(r => r.ParentId == _result[i].Id).Include(r => r.DiscussionVote).Include(r => r.User).ToListAsync();
                discussion.Id = _result[i].Id;
                discussion.Title = _result[i].Title;
                discussion.Content = _result[i].Content;
                discussion.QuestionId = _result[i].QuestionId;
                discussion.UserId = _result[i].UserId;
                discussion.Views = _result[i].Views;
                discussion.Votes = _result[i].Votes;
                discussion.CreatedDate = _result[i].CreatedDate;
                discussion.UpdatedDate = _result[i].UpdatedDate;
                discussion.ParentId = _result[i].ParentId;
                discussion.Level = _result[i].Level;
                discussion.User = _result[i].User;
                discussion.DiscussionVote = _result[i].DiscussionVote.ToList();
                discussion.Discussions = dis;
                result.Add(discussion);
            }
            return result;
        }
        public async Task<Tags> AddTag(Tags tag)
        {
            var t = ReboostDbContext.Tags.Add(tag);
            ReboostDbContext.SaveChanges();
            return tag;
        }
        public async Task DeleteTag(Tags tag)
        {
            ReboostDbContext.Tags.Remove(tag);
            await ReboostDbContext.SaveChangesAsync();
            return;
        }

        public async Task<Discussion> IncreaseView(int id)
        {
            var discussion = await (from q in ReboostDbContext.Discussion
                                    where q.Id == id
                                    select q).FirstOrDefaultAsync();
            discussion.Views += 1;
            ReboostDbContext.SaveChanges();
            return discussion;

        }
        public async Task<List<DiscussionVote>> GetDiscussionVoteByDiscussionId(int id)
        {
            return await ReboostDbContext.DiscussionVote.Where(r => r.DiscussionId == id).Include(u => u.User).ToListAsync();
        }

        public async Task<DiscussionVote> CreateDiscussionVote(DiscussionVote discussionVote)
        {
            var vote = await ReboostDbContext.DiscussionVote.Where(rs => rs.UserId == discussionVote.UserId && rs.DiscussionId == discussionVote.DiscussionId).FirstOrDefaultAsync();
            if(vote != null)
            {
                vote.Status = discussionVote.Status;
                ReboostDbContext.DiscussionVote.Update(vote);
                await ReboostDbContext.SaveChangesAsync();
                return discussionVote;
            }
            ReboostDbContext.DiscussionVote.Add(discussionVote);
            await ReboostDbContext.SaveChangesAsync();
            return discussionVote;
        }

        public async Task<Discussion> UpVoteAsync(VoteModel voteModel)
        {
            var discussion = await (from q in ReboostDbContext.Discussion
                                    where q.Id == voteModel.DiscussionId
                                    select q).FirstOrDefaultAsync();
            var discussionVote = await (from q in ReboostDbContext.DiscussionVote
                                where q.DiscussionId == voteModel.DiscussionId && q.UserId == voteModel.UserId
                                select q).FirstOrDefaultAsync();
            if(discussionVote == null || discussionVote.Status == 0)
            {
                discussion.Votes += 1;
            }
            else if (discussionVote.Status == -1 && voteModel.Status == 1)
            {
                discussion.Votes += 2;
            }
            else if (discussionVote.Status == 1 && voteModel.Status == 0)
            {
                discussion.Votes -= 1;
            }
            DiscussionVote _discussionVote = new DiscussionVote();
            _discussionVote.DiscussionId = voteModel.DiscussionId;
            _discussionVote.UserId = voteModel.UserId;
            _discussionVote.Status = voteModel.Status;
            await CreateDiscussionVote(_discussionVote);
            ReboostDbContext.SaveChanges();
            return discussion;
        }

        public async Task<Discussion> DownVoteAsync(VoteModel voteModel)
        {
            var discussion = await(from q in ReboostDbContext.Discussion
                                   where q.Id == voteModel.DiscussionId
                                   select q).FirstOrDefaultAsync();
            var discussionVote = await (from q in ReboostDbContext.DiscussionVote
                                        where q.DiscussionId == voteModel.DiscussionId && q.UserId == voteModel.UserId
                                        select q).FirstOrDefaultAsync();
            if (discussionVote == null || discussionVote.Status == 0)
            {
                discussion.Votes -= 1;
            }
            else if (discussionVote.Status == 1 && voteModel.Status == -1)
            {
                discussion.Votes -= 2;
            }
            else if (discussionVote.Status == -1 && voteModel.Status == 0)
            {
                discussion.Votes += 1;
            }
            DiscussionVote _discussionVote = new DiscussionVote();
            _discussionVote.DiscussionId = voteModel.DiscussionId;
            _discussionVote.UserId = voteModel.UserId;
            _discussionVote.Status = voteModel.Status;
            await CreateDiscussionVote(_discussionVote);
            ReboostDbContext.SaveChanges();
            return discussion;
        }
    }
}
