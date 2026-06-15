using System.Collections.Generic;
using System.Threading.Tasks;
using API.Entities;
using API.Helpers;

namespace API.Interfaces;

public interface IMemberRepository
{
    void Update(Member member);
    Task<PaginatedResult<Member>> GetAllMembersAsync(MemberParams memberParams);
    Task<Member?> GetMemberByIdAsync(string id);
    Task<IReadOnlyList<Photo>> GetPhotsForMemberAsync(string memberId);
    Task<Member?> GetMemberForUpdatesAsync(string memberId);
}