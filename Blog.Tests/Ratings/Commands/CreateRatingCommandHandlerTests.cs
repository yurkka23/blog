namespace Blog.Tests.Ratings.Commands;

public class CreateRatingCommandHandlerTests : TestCommandBase
{
    [Fact]
    public async Task CreateRatingCommandhandler()
    {
        //Arrange
        var handler = new CreateRatingCommandHandler(Context);
        byte ratingScore = 4;
        //Act
        var ratingId = await handler.Handle(new CreateRatingCommand
        {
            Score = ratingScore,
            ArticleId = BlogContextFactory.ArticleIdForDelete,
            UserId = BlogContextFactory.UserAId
        },CancellationToken.None);
        //Assert
        Assert.NotNull(await Context.Ratings
            .SingleOrDefaultAsync(r => r.Id == ratingId 
                        && r.Score == ratingScore 
                        && r.UserId == BlogContextFactory.UserAId 
                        && r.ArticleId == BlogContextFactory.ArticleIdForDelete));
    }
}
