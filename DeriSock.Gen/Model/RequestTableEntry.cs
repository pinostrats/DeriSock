namespace DeriSock.Console.Model
{
  using System.Diagnostics;
  using DeriSock.Gen;
  using HtmlAgilityPack;

  [DebuggerDisplay("{" + nameof(ToDebuggerDisplay) + "(),nq}")]
  public class RequestTableEntry
  {
    public string Description;
    public string Enum;
    public string Name;
    public bool Required;
    public string Type;

    public RequestTableEntry(HtmlNode rowNode)
    {
      var colIdx = 0;
      foreach (var colNode in rowNode.SelectNodes(".//td"))
      {
        switch (colIdx)
        {
          case 0:
            Name = colNode.InnerText.RemoveHtmlChars();
            break;
          case 1:
            Required = colNode.InnerText == "true";
            break;
          case 2:
            Type = colNode.InnerText;
            break;
          case 3:
            //TODO: Parse InnerHtml to keep <br> tags as newlines
            Enum = colNode.InnerText;
            break;
          case 4:
            Description = colNode.InnerText;
            break;
          default:
            colIdx++;
            continue;
        }

        colIdx++;
      }
    }

    public string ToDebuggerDisplay()
    {
      return $"Name: {Name}, Required: {Required}, Type: {Type}";
    }
  }
}
