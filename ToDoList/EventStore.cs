using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Linq.Expressions;

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
      public void AddCreatedEvent(TodoCreatedEvent e)
      {
        var todo = e.data as Todo;
        todo.Id = todoRepo.Count;
        todoRepo.Add(todo);

        events.Add(e);
      }

      public void AddDeletedEvent(TodoDeletedEvent e)
      {
        var todoId = (int) e.data;
        var target = todoRepo.SingleOrDefault(t => t.Id == todoId);
        todoRepo.Remove(target);
        events.Add(e);
      }

      public IEnumerable<IEvent> GetEvents()
      {
        return events;
      }
    }
}
