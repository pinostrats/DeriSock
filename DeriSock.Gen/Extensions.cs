namespace DeriSock.Gen
{
  using System;
  using System.Collections.Generic;
  using System.Globalization;
  using System.Linq;
  using DeriSock.Gen.Model;

  public static class Extensions
  {
    public static string GetCSharpTypeName(this string typeString)
    {
      return typeString switch
      {
        "number" => "decimal",
        "float" => "double",
        "integer" => "int",
        "boolean" => "bool",
        "array" => "object[]",
        "array of string" => "string[]",
        "array of number" => "decimal[]",
        "array of integer" => "int[]",
        "array of object" => "object[]",
        "array of objects" => "object[]",
        "array of [timestamp, value]" => "object[]",
        "array of [price, amount]" => "object[]",
        "string" => "string",
        "object" => "object",
        _ => throw new ArgumentOutOfRangeException($"Unknown type: '{typeString}'")
      };
    }

    public static string GetCSharpPropertyName(this string propertyName)
    {
      var txtInfo = new CultureInfo("en-us", false).TextInfo;
      return txtInfo.ToTitleCase(propertyName).Replace("_", string.Empty).Replace(">", string.Empty);
    }

    public static string RemoveHtmlChars(this string value)
    {
      return value.Replace("&nbsp;", string.Empty).Replace("&rsaquo;", string.Empty);
    }

    public static IEnumerable<RequestProperty> GetDistinctEnumProperties(this List<MethodInfo> methods)
    {
      var enumDict = new Dictionary<string, RequestProperty>();
      var enumProps = methods.SelectMany(m => m.Parameters).Where(p => !string.IsNullOrEmpty(p.Enum));
      foreach (var prop in enumProps)
      {
        var key = $"{prop.Name}{prop.Enum}";
        if (enumDict.ContainsKey(key))
        {
          continue;
        }
        enumDict.Add(key, prop);
        yield return prop;
      }
    }
  }
}
