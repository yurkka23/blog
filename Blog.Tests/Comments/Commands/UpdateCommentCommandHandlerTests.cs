namespace Blog.Tests.Comments.Commands;

public class UpdateCommentCommandHandlerTests : TestCommandBase
{
    [Fact]
    public async Task CreateCommentHandler_Success()
    {
        //Arrange
        var handler = new UpdateCommentCommandHandler(Context);
        var commentMessage = "new message1234";

        //Act
        var commentId = await handler.Handle(
            new UpdateCommentCommand
            {
                Id = BlogContextFactory.CommentIdForUpdate,
                Message = commentMessage,
                ArticleId = BlogContextFactory.ArticleIdForUpdate,
                UserId = BlogContextFactory.UserBId,
            },
            CancellationToken.None);

        //Assert
        Assert.NotNull(await Context.Comments.SingleOrDefaultAsync(com =>
                            com.Id == BlogContextFactory.CommentIdForUpdate
                            && com.Message == commentMessage
                            && com.UserId == BlogContextFactory.UserBId));
    }
    [Fact]
    public async Task UpdateCommentCommandHandler_FailOnWrongId()
    {
        //Arrange
        var handler = new UpdateCommentCommandHandler(Context);

        //Act

        //Assert
        await Assert.ThrowsAsync<NotFoundException>(async () =>
             await handler.Handle(
                 new UpdateCommentCommand
                 {
                     Id = 10,//not exist
                     UserId = BlogContextFactory.UserBId,
                     ArticleId= BlogContextFactory.ArticleIdForUpdate,
                     Message = "new Message"
                 }, CancellationToken.None));
    }
    [Fact]
    public async Task UpdateCommentCommandHandler_FailOnWrongUserId()
    {
        //Arrange
        var handler = new UpdateCommentCommandHandler(Context);

        //Act

        //Assert
        await Assert.ThrowsAsync<NotRightsException>(async () =>
             await handler.Handle(
                 new UpdateCommentCommand
                 {
                     Id = BlogContextFactory.CommentIdForUpdate,
                     UserId = BlogContextFactory.UserAId,
                     ArticleId = BlogContextFactory.ArticleIdForUpdate,
                     Message = "new Message"
                 }, CancellationToken.None));
    }
}
