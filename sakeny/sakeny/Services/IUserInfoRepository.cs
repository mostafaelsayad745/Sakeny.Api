using sakeny.Entities;

namespace sakeny.Services
{
    public interface IUserInfoRepository
    {
        Task<bool> UserExistsAsync(int userId);

        Task<IEnumerable<UsersTbl>> GetUsersAsync();

        Task<(IEnumerable<UsersTbl>, PaginationMetadata)> GetUsersAsync
            (string? name, string? SearchQuery, int pageNumber, int pageSize);

        Task<UsersTbl?> GetUserAsync(int userId, bool includePostFeedbacks);

        Task AddUserAsync(UsersTbl user);
        Task UpdateUserAsync(UsersTbl user);
        void DeleteUserAsync(UsersTbl user);
        Task<bool> SaveChangesAsync();


        Task<bool> checkEmailNotRepated(UsersTbl user);


        // defination for the post feedbacks of the user

        Task<bool> PostFeedbackExistsAsync(int userId, int postId, DateTime? datetime);
        Task<IEnumerable<PostFeedbackTbl>> GetPostFeedbacksForUserAsync( int postId);
        Task<PostFeedbackTbl?> GetPostFeedbackForUserAsync(int userId,int postId, int postFeedbackId);

        Task AddPostFeedbackForUserAsync(int userId,int postId, PostFeedbackTbl postFeedbackTbl);
        void DeletePostFeedback(PostFeedbackTbl postFeedbackTbl);


        // defination for the posts of the user
        Task<bool> PostExistsAsync(int userId, int postId);
        Task<bool> PostExistsAsync(int postId);
        Task<(IEnumerable<PostsTbl>, PaginationMetadata)> GetPostsForUserAsync(int userId , string? name , 
            string? seacrchQuery , int pageNumber , int pageSize);
        Task<PostsTbl?> GetPostForUserAsync(int userId, int postId );
        Task<PostsTbl?> GetPostForUserAsyncForUpdate(int userId, int postId);
        Task AddPostForUserAsync(int userId, PostsTbl postTbl);
        void DeletePost(PostsTbl postTbl);


        // defination for the features of the post
        Task<bool> FeaturesExistsAsync(int postId, int featuresId);
        Task<IEnumerable<FeaturesTbl>> GetFeaturesForPostAsync(int postId);
        Task<FeaturesTbl?> GetFeatureForPostAsync(int postId, int featuresId);
        Task AddFeatureForPostAsync(int postId, FeaturesTbl featureTbl);
        void DeleteFeature(FeaturesTbl featureTbl);




    }
}
