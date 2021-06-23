namespace DeriSock.Gen.Model
{
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;
  using HtmlAgilityPack;

  //TODO: Enhance on:
  // /private/create_subaccount
  // /private/get_subaccounts
  // /private/buy
  // /private/sell
  // /private/edit
  // /private/close_position

  public class ResponseTable
  {
    public List<ResponseTableEntry> Parameters = new List<ResponseTableEntry>();

    public ResponseTable(string path, HtmlNode tableNode)
    {
      if (path == "/private/create_subaccount"
          || path == "/private/get_subaccounts"
          || path == "/private/buy"
          || path == "/private/sell"
          || path == "/private/edit"
          || path == "/private/close_position")
      {
        return;
      }

      var tbody = tableNode.FirstChild.NextSibling;
      var rowCount = tbody.SelectNodes(".//tr").Count;

      var rowIdx = 0;
      foreach (var rowNode in tbody.SelectNodes(".//tr"))
      {
        if (rowIdx >= 3 || (rowIdx == 2 && rowCount == 3))
        {
          if (string.IsNullOrEmpty(rowNode.SelectNodes(".//td").First().InnerText))
          {
            continue;
          }

          //TODO: &rsaquo; markiert eine Einrückung nach innen. Eine kann ignoriert werden, eine Zweite oder dritte aber nicht ...
          Parameters.Add(new ResponseTableEntry(rowNode));
        }

        rowIdx++;
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

      output.AppendLine("  }");
      output.AppendLine("}");
      return output.ToString();
    }
  }
}
