using Microsoft.EntityFrameworkCore;
using sakeny.DbContexts;
using sakeny.Entities;
using System.Linq;

namespace sakeny.Services
{
    public class UserInfoRepositorycs : IUserInfoRepository
    {
        private readonly HOUSE_RENT_DBContext _context;

        public UserInfoRepositorycs(HOUSE_RENT_DBContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }




        public async Task AddPostFeedbackForUserAsync(int userId, PostFeedbackTbl postFeedbackTbl)
        {
            var user =await GetUserAsync(userId, false);
            if(user !=null)
            {
                user.PostFeedbackTbls.Add(postFeedbackTbl);
            }

        }




        public async Task AddUserAsync(UsersTbl user)
        {
            if(user != null)
            {
                await _context.UsersTbls.AddAsync(user);
            }
        }




        public void DeletePostFeedback(PostFeedbackTbl postFeedbackTbl)
        {
            _context.PostFeedbackTbls.Remove(postFeedbackTbl);
        }




        public Task DeleteUserAsync(UsersTbl user)
        {
            throw new NotImplementedException();
        }



        public async Task<PostFeedbackTbl?> GetPostFeedbackForUserAsync(int userId, int postFeedbackId)
        {
            return await _context.PostFeedbackTbls
                .Where(p => p.UserId == userId && p.PostFeedId == postFeedbackId).FirstOrDefaultAsync();
        }



        public async Task<IEnumerable<PostFeedbackTbl>> GetPostFeedbacksForUserAsync(int userId)
        {
            return await _context.PostFeedbackTbls
                .Where(p => p.UserId == userId).ToListAsync();
        }



        public async Task<UsersTbl?> GetUserAsync(int userId , bool includePostFeedbacks = false)
        {
            if(includePostFeedbacks)
            {
                return await _context.UsersTbls.Include(u => u.PostFeedbackTbls)
                    .Where(u => u.UserId == userId).FirstOrDefaultAsync();
            }
            return await _context.UsersTbls.Where(u => u.UserId == userId).FirstOrDefaultAsync();
        }



        public async Task<IEnumerable<UsersTbl>> GetUsersAsync()
        {
            return await _context.UsersTbls.OrderBy(u => u.UserName).ToListAsync();
        }



        public async Task<(IEnumerable<UsersTbl>,PaginationMetadata)> GetUsersAsync(string? name , string? SearchQuery
            ,int pageNumber , int pageSize)
        {
           
            var collection = _context.UsersTbls as IQueryable<UsersTbl>;
            if(!string.IsNullOrWhiteSpace(name))
            {
               name = name.Trim();
                collection = collection.Where(u => u.UserName ==name);
            }
            if(!string.IsNullOrWhiteSpace(SearchQuery))
            {
                SearchQuery = SearchQuery.Trim();
                collection = collection.Where(u => u.UserName.Contains(SearchQuery) ||
                u.UserEmail != null && u.UserEmail.Contains(SearchQuery));
            }
            var totalItemCount = await collection.CountAsync();

            var paginationMetadata = new PaginationMetadata(totalItemCount, pageSize,pageNumber);

            var collectionToReturn = await collection.OrderBy(u => u.UserName)
                .Skip(pageSize * (pageNumber - 1)).Take(pageSize)
                .ToListAsync();

            return (collectionToReturn,paginationMetadata);
        }


        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }

        public Task UpdateUserAsync(UsersTbl user)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UserExistsAsync(int userId)
        {
            return await _context.UsersTbls.AnyAsync(u => u.UserId == userId);
        }
    }
}
