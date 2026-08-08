/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件如若商用，请务必官网购买！

daily assets update for try.

U should buy the asset from home store if u use it in your project!
*/

namespace UnityEditor.FacebookEditor
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;

    public class PListDict : Dictionary<string, object>
    {
        public PListDict()
        {
        }

        public PListDict(PListDict dict) : base(dict)
        {
        }

        public PListDict(XElement dict)
        {
            this.Load(dict);
        }

        public void Load(XElement dict)
        {
            var dictElements = dict.Elements();
            this.ParseDictForLoad(this, dictElements);
        }

        public void Save(string fileName, XDeclaration declaration, XDocumentType docType)
        {
            XElement plistNode = new XElement("plist", this.ParseDictForSave(this));
            plistNode.SetAttributeValue("version", "1.0");
            XDocument file = new XDocument(declaration, docType);
            file.Add(plistNode);
            file.Save(fileName);
        }

        public XElement ParseValueForSave(object node)
        {
            if (node is string)
            {
                return new XElement("string", node);
            }
            else if (node is bool)
            {
                return new XElement(node.ToString().ToLower());
            }
            else if (node is int)
            {
                return new XElement("integer", node);
            }
            else if (node is float)
            {
                return new XElement("real", node);
            }
            else if (node is IList<object>)
            {
                return this.ParseArrayForSave(node);
            }
            else if (node is PListDict)
            {
                return this.ParseDictForSave((PListDict)node);
            }
            else if (node == null)
            {
                return null;
            }

            throw new NotSupportedException("Unexpected type: " + node.GetType().FullName);
        }

        private void ParseDictForLoad(PListDict dict, IEnumerable<XElement> elements)
        {
            for (int i = 0; i < elements.Count(); i += 2)
            {
                XElement key = elements.ElementAt(i);
                XElement val = elements.ElementAt(i + 1);
                dict[key.Value] = this.ParseValueForLoad(val);
            }
        }

        private IList<object> ParseArrayForLoad(IEnumerable<XElement> elements)
        {
            var list = new List<object>();
            foreach (XElement e in elements)
            {
                object one = this.ParseValueForLoad(e);
                list.Add(one);
            }

            return list;
        }

        private object ParseValueForLoad(XElement val)
        {
            switch (val.Name.ToString())
            {
                case "string":
                    return val.Value;
                case "integer":
                    return int.Parse(val.Value);
                case "real":
                    return float.Parse(val.Value);
                case "true":
                    return true;
                case "false":
                    return false;
                case "dict":
                    PListDict plist = new PListDict();
                    this.ParseDictForLoad(plist, val.Elements());
                    return plist;
                case "array":
                    return this.ParseArrayForLoad(val.Elements());
                default:
                    throw new ArgumentException("Format unsupported, Parser update needed");
            }
        }

        private XElement ParseDictForSave(PListDict dict)
        {
            XElement dictNode = new XElement("dict");
            foreach (string key in dict.Keys)
            {
                dictNode.Add(new XElement("key", key));
                dictNode.Add(this.ParseValueForSave(dict[key]));
            }

            return dictNode;
        }

        private XElement ParseArrayForSave(object node)
        {
            XElement arrayNode = new XElement("array");
            var array = (IList<object>)node;
            for (int i = 0; i < array.Count; i++)
            {
                arrayNode.Add(this.ParseValueForSave(array[i]));
            }

            return arrayNode;
        }
    }
}
