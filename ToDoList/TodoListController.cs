using System;
using System.Collections.Generic;
using System.Text;

namespace ToDoList
{
  public class TodoListController
  {
    private List<Todo> TodoRepo;
    private readonly EventStore es;

    public TodoListController(EventStore es)
    {
      this.es = es;
    }

    public void addTodo(Todo todo)
    {
      TodoCreatedEvent newEvent = new TodoCreatedEvent()
      {
        data = todo,
        TimeStamp = DateTime.Now,
        Type = EventType.TodoCreated,
      };

      es.AddCreatedEvent(newEvent);
    }

    public void deleteTodo(int todoId)
    {
      TodoDeletedEvent newEvent = new TodoDeletedEvent()
      {
        data = todoId,
        TimeStamp = DateTime.Now,
        Type = EventType.TodoDeleted,
        
      };

      es.AddDeletedEvent(newEvent);
    }

  }


  public class Todo
  {
    public DateTime CreatedAt { get; set; }
    public string Description { get; set; }
    public int Id { get; set; }

    public Todo()
    {
      CreatedAt = DateTime.Now;
      
    }
  }
}
