using System.Text.Json.Serialization;

namespace api.Modules.TodoListModule.Domain.Models
{
    public class TodoListItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Done { get; set; }

        // TodolistModule´s Domain Model now "cares about JSON", but thats slightly better then switching to Newtonsoft and use the reference loop support
        // in a more complex app we would not return the domain model entity but rather map the domain model to a view model or similiar for the API and then leave out this navigation property
        [JsonIgnore]
        public TodoList TodoList { get; set; }
    }
}