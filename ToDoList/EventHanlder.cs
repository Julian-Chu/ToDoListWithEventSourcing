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
      else if (e is UndoEvent)
      {
           Undo(e);
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

    private void AddDeletedEvent(DeletedEvent e)
    {
      //var todo = (Todo)e.Data;
      var todoId =(int)e.Data;
      var target = todoRepo.SingleOrDefault(t => t.Id == todoId);
      e.Data = target;
      todoRepo.Remove(target);
    }

    private void Undo(IEvent undoEvent)
    {
      var baseEvent = undoEvent.Data as BaseEvent;
      Todo todo;
      switch (baseEvent.Type)
      {
        case EventType.Created:
          var createdEvent = baseEvent as CreatedEvent;
          todo = baseEvent.Data as Todo;
          todoRepo.Remove(todo);
          break;
        case EventType.Deleted:
          var deletedEvent = baseEvent as DeletedEvent;
          todo = deletedEvent.Data as Todo;
          todoRepo.Insert(todo.Id, todo);
          break;
        default:
          break;
      }
    }
  }
}