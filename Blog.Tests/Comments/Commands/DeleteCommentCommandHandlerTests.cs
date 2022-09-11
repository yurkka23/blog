namespace Blog.Tests.Comments.Commands;

public class DeleteCommentCommandHandlerTests : TestCommandBase
{
    [Fact]
    public async Task DeleteCommentHandler_Success()
    {
        //Arrange
        var handler = new DeleteCommentCommandHandler(Context);
        //Act
        await handler.Handle(new DeleteCommentCommand
        {
            Id = BlogContextFactory.CommentIdForDelete,
            UserId = BlogContextFactory.UserAId,
            Role = Role.User
        }, CancellationToken.None);
        //Assert
        Assert.Null(await Context.Comments.SingleOrDefaultAsync(c => c.Id == BlogContextFactory.CommentIdForDelete));
    }
    [Fact]
    public async Task DeleteCommentHandler_FailOnWrongId()
    {
        //Arrange
        var handler = new DeleteCommentCommandHandler(Context);
        //Act
        //Assert
        await Assert.ThrowsAsync<NotFoundException>(async () =>
                await handler.Handle(new DeleteCommentCommand
                {
                    Id = 10,
                    UserId = BlogContextFactory.UserAId,
                    Role = Role.User
                }, CancellationToken.None));
    }
    [Fact]
    public async Task DeleteCommentHandler_FailOnWrongUserId()
    {
        //Arrange
        var handler = new DeleteCommentCommandHandler(Context);
        //Act
        //Assert
        await Assert.ThrowsAsync<NotRightsException>(async () =>
                await handler.Handle(new DeleteCommentCommand
                {
                    Id=BlogContextFactory.CommentIdForDelete,
                    UserId = BlogContextFactory.UserBId,
                    Role = Role.User
                }, CancellationToken.None));
    }
}
