namespace Blog.Tests.Articles.Queries;

[Collection("QueryCollection")]
public class GetArticleContentHandlerTests
{
    private readonly BlogDbContext Context;
    private readonly IMapper Mapper;

    public GetArticleContentHandlerTests(QueryTestFixture fixture)
    {
        Context = fixture.Context;
        Mapper = fixture.Mapper;
    }

    

    [Fact]
    public async void GetArticleContentQueryHandler_FailOnWrongId()
    {
        //Arrange
        var handler = new GetArticleDetailsQueryHandler(Context, Mapper);

        //Act
       
        //Asset
        await Assert.ThrowsAsync<NotFoundException>(async () =>
              await handler.Handle(new GetArticleContentQuery
              {
                  Id = Guid.Empty
              }, CancellationToken.None));
    }
}
