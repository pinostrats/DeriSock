namespace DeriSock.Gen
{
  using System.Text;
  using DeriSock.Gen.Model;

  public class CSharpCodeProvider
  {
    public string CreateRequestClass(string className, MethodInfo method)
    {
      var output = new StringBuilder();
      output.AppendLine("namespace DeriSock.Gen.ModelTest");
      output.AppendLine("{");
      output.AppendLine("  using Newtonsoft.Json;");
      output.AppendLine("");
      output.AppendLine($"  public class {className}");
      output.AppendLine("  {");
      foreach (var parameter in method.Parameters)
      {
        if (!string.IsNullOrWhiteSpace(parameter.Description))
        {
          output.AppendLine("    /// <summary>");
          output.AppendLine($"    /// {parameter.Description}");
          output.AppendLine("    /// </summary>");
        }

        output.AppendLine($"    [JsonProperty(\"{parameter.Name}\")]");
        output.AppendLine($"    public {parameter.Type.GetCSharpTypeName()} {parameter.Name.GetCSharpPropertyName()} {{ get; set; }}");
      }

      //if (IsPrivate)
      //{
      //  output.AppendLine("\t/// <summary>");
      //  output.AppendLine("\t/// Access Token for the WebSocket Connection");
      //  output.AppendLine("\t/// </summary>");
      //  output.AppendLine("\tpublic string access_token;");
      //}

      output.AppendLine("  }");
      output.AppendLine("}");
      return output.ToString();
    }

    public string CreateResponseClass(string className, MethodInfo method)
    {
      var output = new StringBuilder();
      output.AppendLine("namespace DeriSock.Gen.ModelTest");
      output.AppendLine("{");
      output.AppendLine("  using Newtonsoft.Json;");
      output.AppendLine("");
      output.AppendLine($"  public class {className}");
      output.AppendLine("  {");
      foreach (var parameter in method.Response)
      {
        if (!string.IsNullOrWhiteSpace(parameter.Description))
        {
          output.AppendLine("    /// <summary>");
          output.AppendLine($"    /// {parameter.Description}");
          output.AppendLine("    /// </summary>");
        }

        output.AppendLine($"    [JsonProperty(\"{parameter.Name}\")]");
        output.AppendLine($"    public {parameter.Type.GetCSharpTypeName()} {parameter.Name.GetCSharpPropertyName()} {{ get; set; }}");
      }

      output.AppendLine("  }");
      output.AppendLine("}");
      return output.ToString();
    }
  }
}
