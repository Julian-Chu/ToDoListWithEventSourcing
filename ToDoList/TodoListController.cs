using System;
using System.Collections.Generic;
using System.Linq;

namespace ToDoList
{
  public class TodoListController
  {
    private readonly EventHanlder hanlder;
    private readonly List<IEvent> eventStore;

    public TodoListController(EventHanlder hanlder, List<IEvent> eventStore)
    {
      this.hanlder = hanlder;
      this.eventStore = eventStore;
    }

    public void AddTodo(Todo todo)
    {
      CreatedEvent newEvent = new CreatedEvent()
      {
        Data = todo,
        TimeStamp = DateTime.Now,
        Type = EventType.Created,
      };

      SaveToEventStore(newEvent);
      hanlder.Handle(newEvent);
    }

    public void DeleteTodo(int todoId)
    {
      DeletedEvent newEvent = new DeletedEvent()
      {
        Data = todoId,
        TimeStamp = DateTime.Now,
        Type = EventType.Deleted,
      };
      SaveToEventStore(newEvent);

      hanlder.Handle(newEvent);
    }

    public void Undo()
    {
      var undoEvent = eventStore.LastOrDefault(o => o.Type != EventType.Undo);

      eventStore.Add(new UndoEvent()
      {
              Id = eventStore.Count,
              Type = EventType.Undo,
              Data = undoEvent,
              TimeStamp = DateTime.Now
      });

      hanlder.Undo(undoEvent);
    }

    private void SaveToEventStore(IEvent newEvent)
    {
      newEvent.Id = eventStore.Count;
      eventStore.Add(newEvent);
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