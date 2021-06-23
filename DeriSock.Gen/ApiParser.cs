namespace DeriSock.Gen
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using DeriSock.Gen.Model;
  using HtmlAgilityPack;

  public class ApiParser
  {
    private readonly string _apiDocUrl;
    private readonly HtmlWeb _htmlWeb = new HtmlWeb();
    private readonly List<MethodInfo> _methods = new List<MethodInfo>();
    private HtmlDocument _htmlDocument;

    public IEnumerable<MethodInfo> Methods
    {
      get
      {
        if (_methods.Count <= 0)
        {
          ParseMethods();
        }

        return _methods;
      }
    }

    public ApiParser(string apiDocUrl)
    {
      _apiDocUrl = apiDocUrl;
    }

    private void EnsureHtmlDocument()
    {
      if (_htmlDocument != null)
      {
        return;
      }

      _htmlDocument = _htmlWeb.LoadFromWebAsync(_apiDocUrl).GetAwaiter().GetResult();
    }

    private void ParseMethods()
    {
      EnsureHtmlDocument();
      _methods.Clear();

      foreach (var methodTitleNode in FindAllMethodTitles(_htmlDocument))
      {
        var methodInfo = ParseMethod(methodTitleNode);
        _methods.Add(methodInfo);
      }
    }

    private void ParseSubscriptions()
    {
      EnsureHtmlDocument();
      //TODO: Add Logic to parse the subscriptions
    }

    private static MethodInfo ParseMethod(HtmlNode titleNode)
    {
      // <h1 id="methods">
      // ...
      // ...
      // ...
      // <h2 id="public-method">/public/method</h2>
      // <pre> elements with classes: highlight <language> tab-<language>
      // also (maybe) a blockquote element
      // [0..n] <p> elements with method description
      // [0..1] <aside> When it's a private method
      // <h3 id="parameters[-n]">
      // <table> | <p> > <em>This method takes no parameters</em>
      // <h3 id="response[-n]>
      // <table>
      // ...
      // ... repeat from h2
      // ...
      var methodName = titleNode.InnerText.Substring(1);
      var result = new MethodInfo {Name = methodName, Parameters = new List<RequestProperty>(), Response = new List<ResponseProperty>()};

      var descNode = titleNode.NextSibling;
      while (descNode.Name != "p")
      {
        descNode = descNode.NextSibling;
      }

      while (descNode.Name == "p")
      {
        result.Description += descNode.InnerText + "\n";
        descNode = descNode.NextSibling;
      }

      result.Description = result.Description.TrimEnd();

      var hasParameters = true;
      var hasResponse = true;
      var paramsTableBody = titleNode.NextSibling;
      while (paramsTableBody.Name != "table")
      {
        if (paramsTableBody.Name == "p" && paramsTableBody.InnerText == "This method takes no parameters")
        {
          hasParameters = false;
          break;
        }

        paramsTableBody = paramsTableBody.NextSibling;
      }

      var responseTableBody = paramsTableBody.NextSibling;
      while (responseTableBody.Name != "table")
      {
        if (responseTableBody.Name == "p" && responseTableBody.InnerText == "This method has no response body")
        {
          hasResponse = false;
          break;
        }

        responseTableBody = responseTableBody.NextSibling;
      }

      /*
       * Now we know the Parameters and Response table nodes
       */

      if (hasParameters)
      {
        paramsTableBody = paramsTableBody.FirstChild.NextSibling;
        var paramsTableRows = paramsTableBody.ChildNodes.Where(c => c.Name == "tr");
        foreach (var row in paramsTableRows)
        {
          var rowColumns = row.ChildNodes.Where(c => c.Name == "td").ToArray();
          var prop = new RequestProperty
          {
            Name = rowColumns[0].InnerText.RemoveHtmlChars(),
            NameNode = rowColumns[0],
            Required = rowColumns[1].InnerText == "true",
            RequiredNode = rowColumns[1],
            Type = rowColumns[2].InnerText,
            TypeNode = rowColumns[2],
            Enum = rowColumns[3].InnerText,
            EnumEntries = rowColumns[3].InnerHtml.Split("<br>"),
            EnumNode = rowColumns[3],
            Description = rowColumns[4].InnerText,
            DescriptionNode = rowColumns[4]
          };
          for (var i = 0; i < prop.EnumEntries.Length; i++)
          {
            prop.EnumEntries[i] = prop.EnumEntries[i].Replace("<code>", string.Empty).Replace("</code>", string.Empty);
          }

          result.Parameters.Add(prop);
        }
      }

      if (hasResponse)
      {
        responseTableBody = responseTableBody.FirstChild.NextSibling;
        var responseTableRows = responseTableBody.ChildNodes.Where(c => c.Name == "tr");
        foreach (var row in responseTableRows)
        {
          var rowColumns = row.ChildNodes.Where(c => c.Name == "td").ToArray();
          var prop = new ResponseProperty
          {
            Name = rowColumns[0].InnerText.Replace("&rsaquo;", ">").RemoveHtmlChars(),
            NameNode = rowColumns[0],
            Type = rowColumns[1].InnerText,
            TypeNode = rowColumns[1],
            Description = rowColumns[2].InnerText,
            DescriptionNode = rowColumns[2]
          };

          if (string.IsNullOrWhiteSpace(prop.Name))
          {
            continue;
          }

          if (prop.Name == "id" || prop.Name == "jsonrpc")
          {
            continue;
          }

          if (prop.Name == "result" && (prop.Type == "" || prop.Type == "object"))
          {
            continue;
          }

          if (prop.Name == "object" && prop.Type == "")
          {
            continue;
          }

          //Fix Documentation Error in private/get_user_trades_by_order
          if (prop.Name == "array of")
          {
            prop.Name = prop.Type;
            prop.Type = prop.Description;
            prop.Description = "Trade amount. For perpetual and futures - in USD units, for options it is amount of corresponding cryptocurrency contracts, e.g., BTC or ETH.";
          }

          result.Response.Add(prop);
        }
      }

      return result;
    }

    private static void ParseSubscription(HtmlNode titleNode)
    {
      // <h1 id="subscriptions">
      // ...
      // ...
      // ...
      // <h2 id="subscription-name">subscription-name-with-variables-in-curly-braces</h2>
      // <blockquote> telling subscriptions only with websockets
      // <pre> elements with classes: highlight <language> tab-<language>
      // also (maybe) a blockquote element
      // [0..n] <p> elements with method description
      // <h3 id="channel-parameters[-n]">
      // <table> | <p> > <em>This channel takes no parameters</em>
      // <h3 id="response[-n]>
      // <table>
      //foreach (var subscriptionTitleNode in FindAllSubscriptionTitles(htmlDoc))
      //{
      //  ParseSubscription(subscriptionTitleNode);
      //}

      var name = titleNode.InnerText;
      Console.WriteLine(name);
    }

    private static IEnumerable<HtmlNode> FindAllMethodTitles(HtmlDocument htmlDoc)
    {
      var methodsTitleNode = htmlDoc.GetElementbyId("methods");
      if (methodsTitleNode == null)
      {
        yield break;
      }

      var curNode = methodsTitleNode;
      while (true)
      {
        curNode = curNode.NextSibling;
        if (curNode == null)
        {
          break;
        }

        if (curNode.Id == "subscriptions")
        {
          yield break;
        }

        if (curNode.Name != "h2")
        {
          continue;
        }

        yield return curNode;
      }
    }

    private static IEnumerable<HtmlNode> FindAllSubscriptionTitles(HtmlDocument htmlDoc)
    {
      var subscriptionsTitleNode = htmlDoc.GetElementbyId("subscriptions");
      if (subscriptionsTitleNode == null)
      {
        yield break;
      }

      var curNode = subscriptionsTitleNode;
      while (true)
      {
        curNode = curNode.NextSibling;
        if (curNode == null)
        {
          break;
        }

        if (curNode.Id == "rpc-error-codes")
        {
          yield break;
        }

        if (curNode.Name != "h2")
        {
          continue;
        }

        yield return curNode;
      }
    }
  }
}
