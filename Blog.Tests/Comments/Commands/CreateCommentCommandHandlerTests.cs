//namespace Blog.Tests.Comments.Commands;

//public class CreateCommentCommandHandlerTests : TestCommandBase
//{
//    [Fact]
//    public async Task CreateCommentHandler_Success()
//    {
//        //Arrange
//        var handler = new CreateCommentCommandHandler(Context);
//        var commentMessage = "new message";

//        //Act
//        var commentId = await handler.Handle(
//            new CreateCommentCommand
//            {
//                Message = commentMessage,
//                ArticleId = BlogContextFactory.ArticleIdForDelete,
//                UserId = BlogContextFactory.UserAId,

//            },
//            CancellationToken.None);

//        //Assert
//        Assert.NotNull(await Context.Comments.SingleOrDefaultAsync(com =>
//                            com.Id == commentId
//                            && com.Message == commentMessage
//                            && com.UserId == BlogContextFactory.UserAId));
//    }
//}
