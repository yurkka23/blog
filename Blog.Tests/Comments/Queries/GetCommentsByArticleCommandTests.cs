//namespace Blog.Tests.Comments.Queries;

//[Collection("QueryCollection")]
//public class GetCommentsByArticleCommandTests
//{
//    private readonly BlogDbContext Context;
//    private readonly IMapper Mapper;

//    public GetCommentsByArticleCommandTests(QueryTestFixture fixture)
//    {
//        Context = fixture.Context;
//        Mapper = fixture.Mapper;
//    }
    
//    [Fact]
//    public async void GetCommentsByArticleQueryHandler_FailOnEmptyArticleId()
//    {
//        //Arrange 
//        var handler = new GetCommentsByArticleQueryHandler(Context, Mapper);
//        //Act
//        //Assert
//       await Assert.ThrowsAsync<NotFoundException>(async() =>
//                     await handler.Handle(new GetCommentsByArticleQuery
//                    {
//                        ArticleId = Guid.Empty
//                    }, CancellationToken.None));
//    }
//}
