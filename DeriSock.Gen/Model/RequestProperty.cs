namespace DeriSock.Gen.Model
{
  using HtmlAgilityPack;

  public class RequestProperty
  {
    public string Name { get; set; }
    public HtmlNode NameNode { get; set; }

    public bool Required { get; set; }
    public HtmlNode RequiredNode { get; set; }

    public string Type { get; set; }
    public HtmlNode TypeNode { get; set; }

    public string Enum { get; set; }
    public string[] EnumEntries { get; set; }
    public HtmlNode EnumNode { get; set; }

    public string Description { get; set; }
    public HtmlNode DescriptionNode { get; set; }
  }
}
