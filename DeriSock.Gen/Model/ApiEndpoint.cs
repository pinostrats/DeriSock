namespace DeriSock.Gen.Model
{
  using System;
  using System.Web;
  using HtmlAgilityPack;

  public class ApiEndpoint
  {
        public string Method;
        public RequestTable Parameters;
        public ResponseTable Response;

        public ApiEndpoint(HtmlNode parametersNode)
        {
            var pNode = parametersNode.PreviousSibling;
            while (pNode.NodeType != HtmlNodeType.Element || !pNode.Name.Equals("p", StringComparison.OrdinalIgnoreCase))
            {
                pNode = pNode.PreviousSibling;
            }

            var apiConsoleEndPoint = new Uri(pNode.FirstChild.Attributes["href"].Value);
            var query = apiConsoleEndPoint.Query;
            var queryDict = HttpUtility.ParseQueryString(query);

            Method = queryDict["method"];

            var tableNode = parametersNode.NextSibling;
            while (tableNode.Name == "#text")
            {
                tableNode = tableNode.NextSibling;
            }

            if (tableNode.Name == "p" && tableNode.FirstChild.Name == "em")
            {
                Parameters = null;
                if (Method.StartsWith("/private/"))
                {
                    Parameters = new RequestTable(true, null);
                }
            }
            else if (tableNode.Name == "table")
            {
                Parameters = new RequestTable(Method.StartsWith("/private/"), tableNode);
            }

            tableNode = tableNode.NextSibling;
            while (tableNode.Name == "#text")
            {
                tableNode = tableNode.NextSibling;
            }
            if (tableNode.Name == "h3" && tableNode.Attributes["id"].Value.StartsWith("response"))
            {
                tableNode = tableNode.NextSibling;
                while (tableNode.Name == "#text")
                {
                    tableNode = tableNode.NextSibling;
                }
                if (tableNode.Name == "p" && tableNode.FirstChild.Name == "em")
                {
                    Response = null;
                }
                else if (tableNode.Name == "table")
                {
                    Response = new ResponseTable(Method, tableNode);
                }
            }
        }
    }
}
