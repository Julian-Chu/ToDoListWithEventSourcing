using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ToDoList
{
  public class EventHanlder
  {
    private readonly List<IEvent> eventStore = new List<IEvent>();
    private readonly List<Todo> todoRepo;

    public EventHanlder(List<Todo> todoRepo)
    {
      this.todoRepo = todoRepo;
    }

    public void AddCreatedEvent(CreatedEvent e)
    {
      var todo = e.Data as Todo;
      todo.Id = todoRepo.Count;
      todoRepo.Add(todo);

      e.Id = eventStore.Count;
      eventStore.Add(e);
    }

    public void AddDeletedEvent(DeletedEvent e)
    {
      var todoId = (int)e.Data;
      var target = todoRepo.SingleOrDefault(t => t.Id == todoId);
      e.Data = target;
      todoRepo.Remove(target);

      e.Id = eventStore.Count;
      eventStore.Add(e);
    }

    public IEnumerable<IEvent> GetEvents()
    {
      return eventStore;
    }

    public void UndoLast()
    {
      var e = eventStore.LastOrDefault(o => o.Type != EventType.Undo);
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
      eventStore.Add(new UndoEvent()
      {
        Id = eventStore.Count,
        Type = EventType.Undo,
        Data = e,
        TimeStamp = DateTime.Now
      });
    }
  }
}