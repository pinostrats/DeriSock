namespace DeriSock.Gen.Model
{
  using System.Collections.Generic;
  using System.Text;
  using DeriSock.Console.Model;
  using HtmlAgilityPack;

  public class RequestTable
  {
    public readonly bool IsPrivate;
    public readonly List<RequestTableEntry> Parameters = new List<RequestTableEntry>();

    public RequestTable(bool isPrivate, HtmlNode tableNode)
    {
      IsPrivate = isPrivate;
      if (tableNode == null)
      {
        return;
      }

      var tbody = tableNode.FirstChild.NextSibling;
      foreach (var rowNode in tbody.SelectNodes(".//tr"))
      {
        Parameters.Add(new RequestTableEntry(rowNode));
      }
    }

    public string ToCSharpCode(string className)
    {
      var output = new StringBuilder();
      output.AppendLine("namespace DeriSock.Gen.ModelTest");
      output.AppendLine("{");
      output.AppendLine("  using Newtonsoft.Json;");
      output.AppendLine("");
      output.AppendLine($"  public class {className}");
      output.AppendLine("  {");
      foreach (var parameter in Parameters)
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
  }
}
