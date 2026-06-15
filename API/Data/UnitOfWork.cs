using API.Interfaces;

namespace API.Data;

public class UnitOfWork(AppDbContext context) : IUnitOfWork
{
    private IMemberRepository? _memberRepository;
    private ILikesRepository? _likesRepository;

    public IMemberRepository MemberRepository => _memberRepository ??= new MemberRepository(context);
    public ILikesRepository LikesRepository => _likesRepository ??= new LikesRepository(context);

    public async Task<bool> Complete()
    {
        try
        {
            return await context.SaveChangesAsync() > 0;
        }
        catch (Exception e)
        {
            throw new Exception("An error occured while saving changes", e);
        }
    }

    public bool HasChanges()
    {
        return context.ChangeTracker.HasChanges();
    }
}