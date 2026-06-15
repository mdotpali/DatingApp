namespace API.Helpers;

public class LikesParam: PagingParams
{
    public string Predicate { get; set; } = "liked";
    public string UserId { get; set; } = "";
}