using System;
using System.Collections.Generic;

namespace ToDoList
{
  public class TodoListController
  {
    private List<Todo> TodoRepo;
    private readonly EventHanlder es;

    public TodoListController(EventHanlder es)
    {
      this.es = es;
    }

    public void addTodo(Todo todo)
    {
      CreatedEvent newEvent = new CreatedEvent()
      {
        Data = todo,
        TimeStamp = DateTime.Now,
        Type = EventType.Created,
      };

      es.AddCreatedEvent(newEvent);
    }

    public void deleteTodo(int todoId)
    {
      DeletedEvent newEvent = new DeletedEvent()
      {
        Data = todoId,
        TimeStamp = DateTime.Now,
        Type = EventType.Deleted,
      };

      es.AddDeletedEvent(newEvent);
    }

    public void Undo()
    {
      es.UndoLast();
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