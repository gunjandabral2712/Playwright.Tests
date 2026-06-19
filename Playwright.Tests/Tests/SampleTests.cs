using Playwright.Framework.Pages;

namespace Playwright.Tests.Tests
{
    [TestFixture]
    public class SampleTests : PlaywrightTestBase
    {
        [Test]
        public async Task CanAddNewTodo()
        {
            var page = Page ?? throw new InvalidOperationException("Page is null");
            var config = Config ?? throw new InvalidOperationException("Config is null");
            var todo = new TodoPage(page, config.BaseUrl);
            await todo.NavigateAsync();
            await todo.AddTodoAsync("Buy milk");
            var count = await todo.GetTodoCountAsync();
            Assert.That(count, Is.EqualTo(1));
            var text = await todo.GetTodoTextAtAsync(0) ?? string.Empty; // guard potential null
            Assert.That(text, Does.Contain("Buy milk"));
        }

        [Test]
        public async Task CanCompleteTodo()
        {
            var page = Page ?? throw new InvalidOperationException("Page is null");
            var config = Config ?? throw new InvalidOperationException("Config is null");
            var todo = new TodoPage(page, config.BaseUrl);
            await todo.NavigateAsync();
            await todo.AddTodoAsync("Task 1");
            await todo.ToggleTodoAsync(0);
            var completed = (await page.QuerySelectorAllAsync(".todo-list li.completed")) ?? [];
            Assert.That(completed, Has.Count.EqualTo(1));
        }
    }
}
