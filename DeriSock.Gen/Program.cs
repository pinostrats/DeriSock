namespace DeriSock.Gen
{
  using System;
  using System.IO;
  using System.Linq;

  internal class Program
  {
    public static void Main(string[] args)
    {
      var p = new ApiParser("https://docs.deribit.com");

      //TODO: Find all sub-types in p.Response - Create property groups

      foreach (var methodInfo in p.Methods)
      {
        Console.WriteLine(new String('-', 80));
        Console.WriteLine($"---- {methodInfo.Name}");
        Console.WriteLine($"---- {methodInfo.Description}");
        //Console.WriteLine(new String('-', 80));
        //Console.WriteLine("---- Parameters");
        //Console.WriteLine(new String('-', 80));
        //foreach (var prop in methodInfo.Parameters)
        //{
        //  Console.WriteLine($"{prop.Name}\t{prop.Description}");
        //}
        Console.WriteLine(new String('-', 80));
        Console.WriteLine("---- Response");
        Console.WriteLine(new String('-', 80));
        foreach (var prop in methodInfo.Response)
        {
          Console.WriteLine($"{prop.Name}\t{prop.Description}");
        }
        Console.WriteLine(new String('-', 80));
        Console.WriteLine();
      }

      //foreach (var prop in p.Methods.GetDistinctEnumProperties())
      //{
      //  Console.WriteLine(new String('-', 80));
      //  Console.WriteLine($"---- {prop.Name}");
      //  Console.WriteLine(new String('-', 80));
      //  Console.WriteLine(String.Join(',', prop.EnumEntries));
      //  Console.WriteLine(new String('-', 80));
      //  Console.WriteLine();
      //}

      //////////////////////////////////////////////////////////////////////////

      //var codeProvider = new CSharpCodeProvider();
      //var targetDirectory = @"C:\Users\psoll\source\repos\DeriSock\DeriSock.Gen\ModelTest";
      //foreach (var method in p.Methods)
      //{
      //  var pathParts = method.Name.Split(new[] { '/', '_' }, StringSplitOptions.RemoveEmptyEntries);
      //  var baseClassName = pathParts.Select(pathPart => pathPart.ToLower())
      //    .Aggregate("", (current, lowerPathPart) => current + (char.ToUpper(lowerPathPart[0]) + lowerPathPart.Substring(1)));

      //  var requestClassName = baseClassName + "Request";
      //  var responseClassName = baseClassName + "Response";

      //  var requestFilePath = Path.Combine(targetDirectory, requestClassName + ".cs");
      //  var responseFilePath = Path.Combine(targetDirectory, responseClassName + ".cs");

      //  //Create Class for Request
      //  if (method.Parameters.Count > 0)
      //  {
      //    var textToWrite = codeProvider.CreateRequestClass(requestClassName, method);
      //    File.WriteAllText(requestFilePath, textToWrite);
      //  }

      //  //Create Class for Response
      //  if (method.Response.Count > 0)
      //  {
      //    var textToWrite = codeProvider.CreateResponseClass(responseClassName, method);
      //    File.WriteAllText(responseFilePath, textToWrite);
      //  }
      //}
    }
  }
}
