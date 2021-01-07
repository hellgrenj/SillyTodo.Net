using System.Collections.Generic;

namespace api.Modules.TodoListModule.Domain.Models
{
    public class TodoList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<TodoListItem> Items { get; private set; } = new List<TodoListItem>();

        public void AddItem(TodoListItem item)
        {
            this.Items.Add(item);
        }
    }
}