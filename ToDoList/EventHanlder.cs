using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ToDoList
{
  public class EventHanlder
  {
    private readonly List<Todo> todoRepo;

    public EventHanlder(List<Todo> todoRepo)
    {
      this.todoRepo = todoRepo;
    }

    public void Handle(IEvent e)
    {
      if (e is CreatedEvent)
      {
          AddCreatedEvent((CreatedEvent) e);
      }
      else if (e is DeletedEvent)
      {
          AddDeletedEvent((DeletedEvent) e);
      }
      else
      {
        Console.WriteLine("unknow event");
      }
    }
    private void AddCreatedEvent(IEvent e)
    {
      var todo = e.Data as Todo;
      todo.Id = todoRepo.Count;
      todoRepo.Add(todo);
    }

    private void AddDeletedEvent(IEvent e)
    {
      var todoId = (int)e.Data;
      var target = todoRepo.SingleOrDefault(t => t.Id == todoId);
      e.Data = target;
      todoRepo.Remove(target);
    }

    public void Undo(IEvent undoEvent)
    {
      var data = undoEvent.Data as Todo;
      switch (undoEvent.Type)
      {
        case EventType.Created:
          todoRepo.Remove(data);
          break;
        case EventType.Deleted:
          todoRepo.Insert(data.Id, data);
          break;
        default:
          break;
      }
    }
  }
}