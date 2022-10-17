namespace Blog.Tests.Articles.Commands;

public class CreateArticleCommandHandlerTest : TestCommandBase
{
    [Fact]
    public async Task CreateArticleHandler_Success()
    {
        //Arrange
        var handler = new CreateArticleCommandHandler(Context);
        var articleTitle = "Article Title";
        var articleContent = "article content";

        //Act
        var articleId = await handler.Handle(
            new CreateArticleCommand
            {
                Title = articleTitle,
                Content = articleContent,
                UserId = BlogContextFactory.UserAId,
                
            },
            CancellationToken.None);

        //Assert
        Assert.NotNull(await Context.Articles.SingleOrDefaultAsync(article => 
                            article.Id == articleId 
                            && article.Title == articleTitle 
                            && article.Content == articleContent));
    }

}
