namespace DeriSock.Gen.Model
{
  using System.Diagnostics;
  using HtmlAgilityPack;

  [DebuggerDisplay("{" + nameof(ToDebuggerDisplay) + "(),nq}")]
  public class ResponseTableEntry
  {
    public string Description;
    public string Name;
    public string Type;

    public ResponseTableEntry(HtmlNode rowNode)
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
            Type = colNode.InnerText;
            break;
          case 2:
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
      return $"Name: {Name}, Type: {Type}";
    }
  }
}
