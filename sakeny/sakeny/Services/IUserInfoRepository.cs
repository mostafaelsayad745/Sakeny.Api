using sakeny.Entities;

namespace sakeny.Services
{
    public interface IUserInfoRepository
    {
        Task<bool> UserExistsAsync(int userId);

        Task<IEnumerable<UsersTbl>> GetUsersAsync();

        Task<(IEnumerable<UsersTbl>, PaginationMetadata)> GetUsersAsync
            (string? name , string? SearchQuery,int pageNumber, int pageSize) ;

        Task<UsersTbl?> GetUserAsync(int userId , bool includePostFeedbacks);

        Task AddUserAsync(UsersTbl user);
        Task UpdateUserAsync(UsersTbl user);
        Task DeleteUserAsync(UsersTbl user);
        Task <bool> SaveChangesAsync();

        Task<IEnumerable<PostFeedbackTbl>> GetPostFeedbacksForUserAsync(int userId);
        Task<PostFeedbackTbl?> GetPostFeedbackForUserAsync(int userId, int postFeedbackId);

        Task AddPostFeedbackForUserAsync(int userId, PostFeedbackTbl postFeedbackTbl);
        void DeletePostFeedback(PostFeedbackTbl postFeedbackTbl);
    }
}
