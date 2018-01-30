using System;
using System.Collections.Generic;
using System.Text;

namespace ToDoList
{
  public interface IEvent
  {
    int Id { get; set; }
    EventType Type { get; set; }
    DateTime TimeStamp { get; set; }
    object data { get; set; }
  };


  public enum EventType{
      TodoCreated,
      TodoDeleted
  };


  public class TodoCreatedEvent : IEvent
  {
    public int Id { get; set; }
    public EventType Type { get; set; }
    public DateTime TimeStamp { get; set; }
    public object data { get; set; }
  }





  public class TodoDeletedEvent : IEvent
  {
    public int Id { get; set; }
    public EventType Type { get; set; }
    public DateTime TimeStamp { get; set; }
    public object data { get; set; }
  }


}
