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
                          Description = "test init",
                          CreatedAt = DateTime.Now
                  }
          });
      var eventStore = new EventStore(todoDb);
      var controller = new TodoListController(eventStore);

      controller.addTodo(new Todo()
      {
        Description = "test"
      });

      Task.Delay(2000).Wait();
      controller.addTodo(new Todo()
      {
        Description = "test2"
      });
      controller.deleteTodo(0);

      Console.WriteLine("show events");
      foreach (var e in eventStore.GetEvents())
      {
        object data;
        if (e.data is Todo)
        {
          data = (e.data as Todo).Description;
        }
        else
        {
          data = e.data;
        }
        Console.WriteLine($"Event Id: {e.Id}, Event Type:{e.Type}, Created at {e.TimeStamp}, data: {data}");
      }
      Console.WriteLine();
      Console.WriteLine("show todos");
      foreach (var todo in todoDb)
      {
        Console.WriteLine($"Todo Id: {todo.Id}, Description:{todo.Description}, Created at: {todo.CreatedAt}");
      }

      Console.ReadKey();
    }
  }
}