namespace DeriSock.Gen.Model
{
  using System.Collections.Generic;

  public class MethodInfo
  {
    public string Name { get; set; }
    public string Description { get; set; }
    public List<RequestProperty> Parameters { get; set; }
    public IList<ResponseProperty> Response { get; set; }
  }
}
