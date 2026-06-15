using API.Entities;
using API.Helpers;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class LikesRepository(AppDbContext context) : ILikesRepository
{
    public async Task<MemberLike?> GetMemberLike(string sourcememberId, string targetmemberId)
    {
        return await context.Likes.FindAsync(sourcememberId, targetmemberId);
    }

    public async Task<IReadOnlyList<Member>> GetMemberLikes(string predicate, string memberId)
    {
        var query = context.Likes.AsQueryable();
        switch (predicate)
        {
            case "liked":
                return await query
                    .Where(x => x.SourceMemberId == memberId)
                    .Select(x => x.TargetMember)
                    .ToListAsync();
            case "likedBy":
                return await query
                    .Where(x => x.TargetMemberId == memberId)
                    .Select(x => x.SourceMember)
                    .ToListAsync();
            default:
                var likeIds = await GetCurrentMemberLikeIds(memberId);

                return await query
                    .Where(x => x.TargetMemberId == memberId && likeIds.Contains(x.SourceMemberId))
                    .Select(x => x.SourceMember)
                    .ToListAsync();
        }
    } 
    public async Task<PaginatedResult<Member>> GetMemberLikes(LikesParam likesParam)
    {
        var query = context.Likes.AsQueryable();
        IQueryable<Member> result;
        
        switch (likesParam.Predicate)
        {
            case "liked":
                result = query
                    .Where(x => x.SourceMemberId == likesParam.UserId)
                    .Select(x => x.TargetMember);
                break;
            case "likedBy":
                result = query
                    .Where(x => x.TargetMemberId == likesParam.UserId)
                    .Select(x => x.SourceMember);
break;
            default:
                var likeIds = await GetCurrentMemberLikeIds(likesParam.UserId);
                result = query
                    .Where(x => x.TargetMemberId == likesParam.UserId && likeIds.Contains(x.SourceMemberId))
                    .Select(x => x.SourceMember);
        break;
            
        }
        return await PaginationHelper.CreateAsync(result, likesParam.PageNumber, likesParam.PageSize);
    }

    public async Task<IReadOnlyList<string>> GetCurrentMemberLikeIds(string memberId)
    {
        return await context.Likes
            .Where(x => x.SourceMemberId == memberId)
            .Select(x => x.TargetMemberId)
            .ToListAsync();
    }

    public void AddLike(MemberLike like)
    {
        context.Likes.Add(like);
    }

    public void DeleteLike(MemberLike like)
    {
        context.Likes.Remove(like);
    }
    
}