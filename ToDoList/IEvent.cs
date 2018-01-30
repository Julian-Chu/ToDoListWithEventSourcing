using System;

namespace ToDoList
{
  public interface IEvent
  {
    int Id { get; set; }
    EventType Type { get; set; }
    DateTime TimeStamp { get; set; }
    object Data { get; set; }
  };

  public enum EventType
  {
    Created,
    Deleted,
    Undo
  };

  public class BaseEvent : IEvent
  {
    public int Id { get; set; }
    public EventType Type { get; set; }
    public DateTime TimeStamp { get; set; }
    public object Data { get; set; }

    protected BaseEvent()
    {
      TimeStamp = DateTime.Now;
    }
  }

  public class CreatedEvent : BaseEvent
  { }

  public class DeletedEvent : BaseEvent
  { }

  public class UndoEvent : BaseEvent
  { }
}