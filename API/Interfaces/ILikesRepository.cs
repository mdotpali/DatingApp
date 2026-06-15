using API.Entities;
using API.Helpers;

namespace API.Interfaces;

public interface ILikesRepository
{
    Task<MemberLike?> GetMemberLike(string sourcememberId, string targetmemberId);
    Task<PaginatedResult<Member>> GetMemberLikes(LikesParam likesParam);
    // Task<IReadOnlyList<Member>> GetMemberLikes(string predicate, string memberId);
    Task<IReadOnlyList<string>> GetCurrentMemberLikeIds(string memberId);
    void AddLike(MemberLike like);
    void DeleteLike(MemberLike like);
}