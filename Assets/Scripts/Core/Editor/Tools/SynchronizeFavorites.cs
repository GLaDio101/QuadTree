using System.IO;
using System.Xml;
using Core.Editor.Code;
using Newtonsoft.Json.Utilities;
using UnityEditor;
using UnityEngine;
using System.Linq;

namespace Core.Editor.Tools
{
  public class SynchronizeFavorites : UnityEditor.Editor
  {
    private static readonly string _favoritesPath = Application.dataPath + "/../Docs/Favorites.xml";

    private static readonly string ideafavoritesPath =
      Application.dataPath + "/../.idea/.idea.sanalika-friends/.idea/workspace.xml";

    [MenuItem("Tools/Syncronize Favorites")]
    public static void OnSynchronizeFavorites()
    {
      XmlDocument ideafavoritesXml = new XmlDocument();
      ideafavoritesXml.LoadXml(CodeUtilities.LoadScript(ideafavoritesPath));

      XmlDocument favoritesXml = new XmlDocument();

      var loadFavoritesXml = CodeUtilities.LoadScript(_favoritesPath);

      if (string.IsNullOrEmpty(loadFavoritesXml))
      {
        CodeUtilities.SaveFile(GetXMLAsString(ideafavoritesXml), _favoritesPath);
        return;
      }

      favoritesXml.LoadXml(loadFavoritesXml);
      var favoriteNode = favoritesXml.ChildNodes[1];
      var xnList = ideafavoritesXml.ChildNodes[1];

      var findNodeByAttribute = FindNodeByAttribute("name", "FavoritesManager", favoriteNode.ChildNodes);
      if (!IsContainsNode("name", "FavoritesManager", xnList))
      {
        var newElement = ideafavoritesXml.CreateElement("component");
        newElement.SetAttribute("name", "FavoritesManager");

        xnList.AppendChild(newElement);
      }

      var nodeByAttribute = FindNodeByAttribute("name", "FavoritesManager", xnList.ChildNodes);

      CheckNotes(findNodeByAttribute, nodeByAttribute);
      CheckNotes(nodeByAttribute, findNodeByAttribute);
//          var xnList = ideafavoritesXml.ChildNodes[1].Attributes["version"].Value;

      XmlTextWriter writer = new XmlTextWriter(ideafavoritesPath, null);
      XmlTextWriter writer2 = new XmlTextWriter(_favoritesPath, null);

      ideafavoritesXml.Save(writer);
      favoritesXml.Save(writer2);


      Debug.Log("Finish Sync");
      AssetDatabase.Refresh();
    }

    private static void CheckNotes(XmlNode firstNode, XmlNode secondNode)
    {
      for (var index = 0; index < firstNode.ChildNodes.Count; index++)
      {
        XmlNode child = firstNode.ChildNodes[index];
        var value = child.Attributes["name"].Value;
        if (!IsContainsNode("name", value, secondNode))
        {
          var newElement = secondNode.OwnerDocument.CreateElement("favorites_list");
          foreach (XmlAttribute attribute in child.Attributes)
          {
            newElement.SetAttribute(attribute.Name, attribute.Value);
          }

          secondNode.AppendChild(newElement);
        }
        else
        {
          for (var index2 = 0; index2 < child.ChildNodes.Count; index2++)
          {
            XmlNode child2 = child.ChildNodes[index2];
            var value2 = child2.Attributes["url"].Value;
            var findNodeByAttribute = FindNodeByAttribute("name", value, secondNode.ChildNodes);
            if (!IsContainsNode("url", value2, findNodeByAttribute))
            {
              var newElement = findNodeByAttribute.OwnerDocument.CreateElement("favorite_root");
              foreach (XmlAttribute attribute in child2.Attributes)
              {
                newElement.SetAttribute(attribute.Name, attribute.Value);
              }

              findNodeByAttribute.AppendChild(newElement);
            }
          }
        }
      }
    }

    private static XmlNode FindNodeByAttribute(string attribute, string value, XmlNodeList list)
    {
      foreach (XmlNode node in list)
      {
        var key = node.Attributes[attribute].Value;
        if (key == value)
        {
          return node;
        }
      }

      return null;
    }

    private static bool IsContainsNode(string attribute, string value, XmlNode node)
    {
      bool isContain = false;

      foreach (XmlNode childNode in node.ChildNodes)
      {
        if (childNode.Attributes[attribute].Value == value)
        {
          isContain = true;
        }
      }

      return isContain;
    }

    public static string GetXMLAsString(XmlNode myxml)
    {
      StringWriter sw = new StringWriter();
      XmlTextWriter tx = new XmlTextWriter(sw);
      myxml.WriteTo(tx);

      string str = sw.ToString(); //
      return str;
    }
  }
}