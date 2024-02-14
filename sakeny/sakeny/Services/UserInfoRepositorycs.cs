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


        // defination for the post feedbacks of the user

        public async Task AddPostFeedbackForUserAsync(int userId,int postId , PostFeedbackTbl postFeedbackTbl)
        {
            var post = await _context.PostsTbls.Where(p => p.PostId == postId).FirstOrDefaultAsync();
            if (post != null)
            {
                postFeedbackTbl.Post = post;
                postFeedbackTbl.User = await GetUserAsync(userId);
                post.PostFeedbackTbls.Add(postFeedbackTbl);
            }

        }


        public void DeletePostFeedback(PostFeedbackTbl postFeedbackTbl)
        {
            _context.PostFeedbackTbls.Remove(postFeedbackTbl);
        }

        public async Task<bool> PostFeedbackExistsAsync(int userId, int postId , DateTime? datetime)
        {
            return await _context.PostFeedbackTbls
                .AnyAsync(u => u.UserId == userId && u.PostId == postId && u.PostFeedDate == datetime);
        }

        public async Task<PostFeedbackTbl?> GetPostFeedbackForUserAsync(int userId, int postId, int postFeedbackId)
        {
            return await _context.PostFeedbackTbls
                .Where(p => p.UserId == userId && p.PostFeedId == postFeedbackId && p.PostId == postId
                ).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<PostFeedbackTbl>> GetPostFeedbacksForUserAsync( int postId)
        {
            //return await _context.PostFeedbackTbls
            //    .Where(p => p.UserId == userId && p.PostId == postId).ToListAsync();
            var postFeedbacks = await _context.PostsTbls
                   .Where(p => p.PostId == postId)
                   .SelectMany(p => p.PostFeedbackTbls) // Flatten the nested collections
                   .ToListAsync();

            return postFeedbacks;
        }




        // defination for the user


        public async Task<bool> checkEmailNotRepated(UsersTbl user)
        {
            if (await _context.UsersTbls.AnyAsync(u => u.UserEmail == user.UserEmail))
            {
                return true;
            }
            return false;
        }

        public async Task AddUserAsync(UsersTbl user)
        {
            if (user != null)
            {
                await _context.UsersTbls.AddAsync(user);
            }
        }



        public void DeleteUserAsync(UsersTbl user)
        {
            _context.UsersTbls.Remove(user);
        }

        public async Task<UsersTbl?> GetUserAsync(int userId, bool includePostFeedbacks = false)
        {
            if (includePostFeedbacks)
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



        public async Task<(IEnumerable<UsersTbl>, PaginationMetadata)> GetUsersAsync(string? name, string? SearchQuery
            , int pageNumber, int pageSize)
        {

            var collection = _context.UsersTbls as IQueryable<UsersTbl>;
            if (!string.IsNullOrWhiteSpace(name))
            {
                name = name.Trim();
                collection = collection.Where(u => u.UserName == name);
            }
            if (!string.IsNullOrWhiteSpace(SearchQuery))
            {
                SearchQuery = SearchQuery.Trim();
                collection = collection.Where(u => u.UserName.Contains(SearchQuery) ||
                u.UserEmail != null && u.UserEmail.Contains(SearchQuery));
            }
            var totalItemCount = await collection.CountAsync();

            var paginationMetadata = new PaginationMetadata(totalItemCount, pageSize, pageNumber);

            var collectionToReturn = await collection.OrderBy(u => u.UserName)
                .Skip(pageSize * (pageNumber - 1)).Take(pageSize)
                .ToListAsync();

            return (collectionToReturn, paginationMetadata);
        }


        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }

        public async Task UpdateUserAsync(UsersTbl user)
        {

        }

        public async Task<bool> UserExistsAsync(int userId)
        {
            return await _context.UsersTbls.AnyAsync(u => u.UserId == userId);
        }


       // defination for the posts of the user

        public async Task<(IEnumerable<PostsTbl>,PaginationMetadata)> GetPostsForUserAsync(int userId 
            , string? name,
            string? searchQuery , int pageNumber , int PageSize)
        {
            var collection = _context.PostsTbls as IQueryable<PostsTbl>;

            if(! string.IsNullOrWhiteSpace(name))
            {
                name = name.Trim();
                collection = collection.Where(p => p.PostUserId == userId && p.PostStatue == true
                && p.PostTitle == name);
            }
            if(! string.IsNullOrWhiteSpace(searchQuery))
            {
                searchQuery = searchQuery.Trim();
                collection = collection.Where(p => p.PostUserId == userId && p.PostStatue == true);
                collection = collection.Where(p =>p.PostTitle != null 
                && p.PostTitle.Contains(searchQuery) || 
                p.PostAddress != null && p.PostAddress.Contains(searchQuery));
            }

            var totalItemCount = await collection.CountAsync();
            var paginationMetadata = new PaginationMetadata(totalItemCount, PageSize, pageNumber);

            var collectionToReturn = await collection.OrderBy(p => p.PostDate).
                Skip(PageSize * (pageNumber - 1)).Take(PageSize).ToListAsync();

            return (collectionToReturn, paginationMetadata);
        }
        public async Task<bool> PostExistsAsync(int userId, int postId)
        {
            return await _context.PostsTbls.AnyAsync(u => u.PostUserId == userId && u.PostId == postId);
        }

        public async Task<bool> PostExistsAsync( int postId)
        {
            return await _context.PostsTbls.AnyAsync(u =>  u.PostId == postId);
        }


        public async Task<PostsTbl?> GetPostForUserAsync(int userId, int postId )
        {
            var post = await _context.PostsTbls.Where(p => p.PostUserId == userId && p.PostId == postId)
                .FirstOrDefaultAsync();
            

            if (post is not null && post.PostStatue == true )
            {
                return post;
            }
            return null;
        }
        public async Task<PostsTbl?> GetPostForUserAsyncForUpdate(int userId, int postId)
        {
            var post = await _context.PostsTbls.Where(p => p.PostUserId == userId && p.PostId == postId)
                .FirstOrDefaultAsync();

            if (post is not null && post.PostStatue == false)
            {
                post.PostStatue = true;
                return post;
            }
            else
            {
                post.PostStatue = false; 
                return post;
            }
        }


        public async Task AddPostForUserAsync(int userId, PostsTbl postTbl)
        {
            var user = GetUserAsync(userId, false);
            if (user != null)
            {
                await _context.PostsTbls.AddAsync(postTbl);

            }
        }

        public void DeletePost(PostsTbl postTbl)
        {
            _context.PostsTbls.Remove(postTbl);
        }


        // defination for the features of the post

        public async Task<bool> FeaturesExistsAsync(int postId, int featuresId)
        {
            return await _context.PostFeaturesTbls.AnyAsync(u => u.PostId == postId && u.FeaturesId == featuresId);
        }

        public async Task<IEnumerable<FeaturesTbl>> GetFeaturesForPostAsync(int postId
            )
        {
            

            return await _context.PostFeaturesTbls.Where(p => p.PostId == postId)
                .Select(f => f.Features).ToListAsync();
        }

        public async Task<FeaturesTbl?> GetFeatureForPostAsync(int postId, int featuresId)
        {
            return await _context.PostFeaturesTbls.Where(p => p.PostId == postId && p.FeaturesId == featuresId)
                .Select(f => f.Features).FirstOrDefaultAsync();
        }

        public async Task AddFeatureForPostAsync(int postId, FeaturesTbl featureTbl)
        {
           var post = _context.PostsTbls.Where(p => p.PostId == postId).FirstOrDefault();
            if (post != null)
            {
                var postFeature = new PostFeaturesTbl
                {
                    FeaturesId = featureTbl.FeaturesId,
                    PostId = post.PostId,
                    Features = featureTbl,
                    Post = post
                };
                await _context.PostFeaturesTbls.AddAsync(postFeature);
            }
        }

        public void DeleteFeature(FeaturesTbl featureTbl)
        {
            _context.FeaturesTbls.Remove(featureTbl);
        }
    }
}
