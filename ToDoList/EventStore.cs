using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ToDoList
{
  public class EventStore
  {
    private readonly List<IEvent> events = new List<IEvent>();
    private readonly List<Todo> todoRepo;

    public EventStore(List<Todo> todoRepo)
    {
      this.todoRepo = todoRepo;
    }

    public void AddCreatedEvent(CreatedEvent e)
    {
      var todo = e.Data as Todo;
      todo.Id = todoRepo.Count;
      todoRepo.Add(todo);

      e.Id = events.Count;
      events.Add(e);
    }

    public void AddDeletedEvent(DeletedEvent e)
    {
      var todoId = (int)e.Data;
      var target = todoRepo.SingleOrDefault(t => t.Id == todoId);
      e.Data = target;
      todoRepo.Remove(target);

      e.Id = events.Count;
      events.Add(e);
    }

    public IEnumerable<IEvent> GetEvents()
    {
      return events;
    }

    public void UndoLast()
    {
      var e = events.LastOrDefault(o => o.Type != EventType.Undo);
      var data = e.Data as Todo;
      switch (e.Type)
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
      events.Add(new UndoEvent()
      {
        Id = events.Count,
        Type = EventType.Undo,
        Data = e,
        TimeStamp = DateTime.Now
      });
    }
  }
}