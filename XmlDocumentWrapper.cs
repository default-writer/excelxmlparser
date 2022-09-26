using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace XmlParser
{
    public class XmlDocumentWrapper
    {
        private List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
        private XmlDocument document;

        Dictionary<string, Stack<XmlElement>> elements = new Dictionary<string, Stack<XmlElement>>();

        public XmlDocumentWrapper()
        {
            this.document = new XmlDocument();
            var declaration = document.CreateXmlDeclaration("1.0", "utf-8", null);
            document.AppendChild(declaration);
        }

        private XmlElement Create(string name)
        {
            var elementName = name;
            if (name.Contains("#"))
            {
                int index = name.IndexOf("#");
                elementName = name.Substring(0, index);
            }
            var node = document.CreateElement(elementName);
            Add(name, node);
            return node;
        }

        private void Add(string name, XmlElement element) 
        {
            var list = elements.ContainsKey(name) ? elements[name] : new Stack<XmlElement>();
            list.Push(element);
            elements[name] = list;
        }

        public void Add(string name, string value)
        {
            if (name.Contains(":"))
            {
                var names = name.Split(':');
                var parent = names[0];
                var attribute = names[1];
                var attributeNode = this.document.CreateAttribute(attribute);
                if (value != null) attributeNode.Value = value;
                var parentNode = elements[parent].Peek();
                parentNode.SetAttributeNode(attributeNode);
                return;
            }
            if (name.Contains("_"))
            {
                var names = name.Split('_');
                var parent = names[0];
                var child = names[1];
                var parentNode = elements[parent].Peek();
                var childNode = Create(child);
                if (value != null) childNode.InnerText = value;
                parentNode.AppendChild(childNode);
                return;
            }
            var node = Create(name);
            document.AppendChild(node);
        }

        public void Save(string path)
        {
            this.document.Save(path);
        }

        public string ToXml()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                var settings = new XmlWriterSettings { Indent = true };
                using (XmlWriter writer = XmlWriter.Create(ms, settings))
                {
                    this.document.Save(writer);
                }
                return Encoding.UTF8.GetString(ms.ToArray());
            }
        }

        public override string ToString()
        {
            return ToXml();
        }

        public void Parse(List<KeyValuePair<string, string>> list,  XmlNodeList nodes)
        {
            foreach (var element in nodes)
            {
                var node = element as XmlElement;
                if (node != null)
                {
                    var uniqueNodeName = Unique(node, node.Name);
                    var nodeName = node.ParentNode != document ? node.ParentNode.Name + "_" + uniqueNodeName : node.Name;
                    var nodeValue = node.HasChildNodes && node.FirstChild.NodeType == XmlNodeType.Text ? node.FirstChild.Value : null;
                    list.Add(new KeyValuePair<string, string>(nodeName, nodeValue));
                    foreach (var attribute in node.Attributes)
                    {
                        var attributeNode = attribute as XmlAttribute;
                        if (attributeNode != null)
                        {
                            nodeName = uniqueNodeName + ":" + attributeNode.Name;
                            nodeValue = attributeNode.Value;
                            list.Add(new KeyValuePair<string, string>(nodeName, nodeValue));
                        }
                    }
                    Parse(list, node.ChildNodes);
                }
            }
        }

        private string Unique(XmlNode node, string name)
        {
            int counter = 0;
            var nodeName = name;
            while (list.Where(s => s.Key != node.ParentNode.Name && s.Value == nodeName).Any())
            {
                nodeName = $"{name}#{++counter}";
            }
            list.Add(new KeyValuePair<string, string>(node.ParentNode.Name, nodeName));
            return nodeName;
        }

        public List<KeyValuePair<string, string>> Load(string path)
        {
            list.Clear();
            elements.Clear();
            document.Load(path);
            var parsed = new List<KeyValuePair<string, string>>();
            Parse(parsed, document.ChildNodes);
            return parsed;
        }

        public List<KeyValuePair<string, string>> LoadXml(string xml)
        {
            list.Clear();
            elements.Clear();
            document.LoadXml(xml);
            var parsed = new List<KeyValuePair<string, string>>();
            Parse(parsed, document.ChildNodes);
            return parsed;
        }

        public void LoadFrom(List<KeyValuePair<string, string>> list)
        {
            foreach(var pair in list)
            {
                Add(pair.Key, pair.Value);
            }
        }
    }
}
