using Microsoft.AspNetCore.Http;
using Reboost.DataAccess;
using Reboost.DataAccess.Entities;
using Reboost.DataAccess.Models;
using Reboost.Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reboost.Service.Services
{
    public interface IDiscussionService
    {
        Task<IEnumerable<Discussion>> GetAllAsync();
        Task<Discussion> GetByIdAsync(int id);
        Task<Discussion> CreateAsync(Discussion discussion);
        Task<Discussion> UpdateAsync(Discussion update);
        Task<Discussion> DeleteAsync(int id);
        Task<List<Tags>> GetAllTagsAsync();
        Task<List<DiscussionModel>> GetComments(int id);
        Task<IEnumerable<Discussion>> GetAllByQuestionIdAsync(int id);
        Task<Discussion> IncreaseViewAsync(int id);
        Task<Discussion> UpVoteAsync(VoteModel voteModel);
        Task<Discussion> DownVoteAsync(VoteModel voteModel);
    }
    public class DiscussionService : BaseService, IDiscussionService
    {
        public DiscussionService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
        public async Task<IEnumerable<Discussion>> GetAllAsync()
        {
            var discuss = await _unitOfWork.Discussion.GetAllAsync();
            foreach (var item in discuss)
            {
                var user = await _unitOfWork.Users.GetByIdAsync(item.UserId);
                var tags = await _unitOfWork.Discussion.GetAllTagsByDiscussionId(item.Id);
                item.Tags = tags;
                item.User = user;
            }
            return discuss;
        }
        public async Task<IEnumerable<Discussion>> GetAllByQuestionIdAsync(int id)
        {
            var discuss = await _unitOfWork.Discussion.GetAllDiscussionByQuestionId(id);

            discuss = this.HotFilter(discuss);

            foreach (var item in discuss)
            {
                var user = await _unitOfWork.Users.GetByIdAsync(item.UserId);
                var tags = await _unitOfWork.Discussion.GetAllTagsByDiscussionId(item.Id);
                item.Tags = tags;
                item.User = user;
            }
            return discuss;
        }
        public async Task<Discussion> GetByIdAsync(int id)
        {
            var dis = await _unitOfWork.Discussion.GetByIdAsync(id);
            var user = await _unitOfWork.Users.GetByIdAsync(dis.UserId);
            var tags = await _unitOfWork.Discussion.GetAllTagsByDiscussionId(dis.Id);
            var votes = await _unitOfWork.Discussion.GetDiscussionVoteByDiscussionId(dis.Id);
            dis.User = user;
            dis.Tags = tags;
            dis.DiscussionVote = votes;
            return dis;
        }
        public async Task<Discussion> CreateAsync(Discussion discussion)
        {
            discussion.CreatedDate = DateTime.Now;
            discussion.UpdatedDate = DateTime.Now;
            List<string> listTagName = new List<string>();
            foreach (var item in discussion.Tags)
            {
                listTagName.Add(item.Name);
            }
            discussion.Tags = null;

            var rs = await _unitOfWork.Discussion.Create(discussion);

            foreach (var item in listTagName)
            {
                Tags tag = new Tags { DiscussionId = rs.Id, Name = item };
                await _unitOfWork.Discussion.AddTag(tag);
            }
            return rs;
        }
        public async Task<Discussion> DeleteAsync(int id)
        {
            return await _unitOfWork.Discussion.Delete(id);
        }
        public async Task<Discussion> UpdateAsync(Discussion update)
        {
            var listTag = await _unitOfWork.Discussion.GetAllTagsByDiscussionId(update.Id);

            foreach (var item in listTag)
            {
                await _unitOfWork.Discussion.DeleteTag(item);
            }
            if (update.Tags != null)
            {
            foreach (var item in update.Tags)
                {
                    Tags tag = new Tags { DiscussionId = update.Id, Name = item.Name };
                    await _unitOfWork.Discussion.AddTag(tag);
                }
            }
            var discussionVote = update.DiscussionVote.LastOrDefault();
            if (discussionVote != null)
            {
                await _unitOfWork.Discussion.CreateDiscussionVote(discussionVote);
            }

            update.Tags = null;
            update.DiscussionVote = null;
            update.UpdatedDate = DateTime.Now;
            return await _unitOfWork.Discussion.Update(update);
        }
        public async Task<List<Tags>> GetAllTagsAsync()
        {
            return await _unitOfWork.Discussion.GetAllTags();
        }

        public async Task<List<DiscussionModel>> GetComments(int id)
        {
            return await _unitOfWork.Discussion.GetComments(id);
        }
        public async Task<Discussion> IncreaseViewAsync(int id)
        {
            return await _unitOfWork.Discussion.IncreaseView(id);
        }
        public List<Discussion> HotFilter(List<Discussion> discussions)
        {
            for (int i= 0; i < discussions.Count; i++)
            {
                for (int j = 0; j < discussions.Count; j++)
                {
                    if (this.AvgView(discussions[i].Views, discussions[i].CreatedDate) > this.AvgView(discussions[j].Views, discussions[j].CreatedDate))
                    {
                        this.Swap(discussions,i, j);
                    }
                }
            }
            return discussions;
        }
        public void Swap(List<Discussion> list, int indexA, int indexB)
        {
            var tmp = list[indexA];
            list[indexA] = list[indexB];
            list[indexB] = tmp;
        }
        public double AvgView(int views,DateTime create){
            var now = DateTime.Now;
            TimeSpan diff = now - create;
            double hours = diff.TotalHours;
            double rs = views / hours;
            return rs;
        }

        public async Task<Discussion> UpVoteAsync(VoteModel voteModel)
        {
            return await _unitOfWork.Discussion.UpVoteAsync(voteModel);
        }

        public async Task<Discussion> DownVoteAsync(VoteModel voteModel)
        {
            return await _unitOfWork.Discussion.DownVoteAsync(voteModel);
        }
    }
}
