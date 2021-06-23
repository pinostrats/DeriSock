namespace DeriSock.Gen.Model
{
  using HtmlAgilityPack;

  public class ResponseProperty
  {
    public string Name { get; set; }
    public HtmlNode NameNode { get; set; }

    public string Type { get; set; }
    public HtmlNode TypeNode { get; set; }

    public string Description { get; set; }
    public HtmlNode DescriptionNode { get; set; }
  }
}
