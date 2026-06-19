using Microsoft.Playwright;

namespace Playwright.Framework.Pages
{
    public class TodoPage(IPage page, string? baseUrl)
    {
        private readonly string _baseUrl = string.IsNullOrWhiteSpace(baseUrl) ? "/" : baseUrl;

        public async Task NavigateAsync()
        {
            await page.GotoAsync(_baseUrl);
            await page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        }

        public async Task AddTodoAsync(string text)
        {
            await page.FillAsync(".new-todo", text);
            await page.PressAsync(".new-todo", "Enter");
            await page.WaitForSelectorAsync($".todo-list li:has-text(\"{text}\")");
        }

        public async Task<int> GetTodoCountAsync()
        {
            var items = await page.QuerySelectorAllAsync(".todo-list li");
            return items?.Count ?? 0;
        }

        public async Task<string?> GetTodoTextAtAsync(int index)
        {
            var el = await page.QuerySelectorAsync($".todo-list li:nth-child({index + 1}) .view label");
            if (el == null)
                return null;
            return await el.InnerTextAsync();
        }

        public async Task ToggleTodoAsync(int index)
        {
            var checkbox = await page.QuerySelectorAsync($".todo-list li:nth-child({index + 1}) .toggle");
            if (checkbox != null)
                await checkbox.ClickAsync();
        }
    }
}
