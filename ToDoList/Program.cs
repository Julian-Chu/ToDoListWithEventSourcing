using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ToDoList
{
  internal class Program
  {
    private static void Main(string[] args)
    {
      var todoDb = new List<Todo>(new List<Todo>()
          {
                  new Todo()
                  {
                          Id = 0,
                          Description = "test0",
                          CreatedAt = DateTime.Now
                  }
          });
      var eventHandler = new EventHanlder(todoDb);
      var eventStore = new List<IEvent>();
      var controller = new TodoListController(eventHandler, eventStore);

      Console.WriteLine("initial todo list");
      foreach (var todo in todoDb)
      {
        Console.WriteLine($"Todo Id: {todo.Id}, Description:{todo.Description}, Created at: {todo.CreatedAt}");
      }

      controller.AddTodo(new Todo()
      {
        Description = "test1"
      });

      Task.Delay(1000).Wait();
      controller.AddTodo(new Todo()
      {
        Description = "test2"
      });

      Task.Delay(1000).Wait();
      controller.DeleteTodo(1);


      Console.WriteLine("\nAfter Add 2 todos, and delete todo[1], Show todos");
      foreach (var todo in todoDb)
      {
        Console.WriteLine($"Todo Id: {todo.Id}, Description:{todo.Description}, Created at: {todo.CreatedAt}");
      }
      Console.WriteLine("\nAfter undo, add , undo, Show todos");
      controller.Undo();
      controller.AddTodo(new  Todo()
      {
              Description = "test3"
      });
      controller.Undo();
      

      foreach (var todo in todoDb)
      {
        Console.WriteLine($"Todo Id: {todo.Id}, Description:{todo.Description}, Created at: {todo.CreatedAt}");
      }

      Console.WriteLine("\nShow events in Event Store");
      foreach (var e in eventStore)
      {
        object data;
        if (e.Data is Todo)
        {
          data = (e.Data as Todo).Description;
        }
        else
        {
          data = e.Data;
        }
        Console.WriteLine($"Event Id: {e.Id}, Event Type:{e.Type}, Created at {e.TimeStamp}, data: {data}");
      }
      Console.ReadKey();
    }
  }
}