using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace PlanetTileMap.Utils
{
    static class YamlParser
    {
        public static int TabSize = 4;

        public static YamlNode Parse(string filePath)
        {
            using (var sr = new StreamReader(filePath))
                return Parse(sr);
        }

        public static YamlNode Parse(TextReader text)
        {
            YamlNode current = null;
            YamlNode result = new YamlNode();
            var stack = new Stack<YamlNode>();

            //read lines
            string line = null;
            while ((line = text.ReadLine()) != null)
            {
                //comment?
                if (line.StartsWith('#')) continue;
                if (line.StartsWith("---")) continue;

                //trim spaces
                var trimmed = line.TrimStart(' ');
                if (string.IsNullOrEmpty(trimmed))
                    continue;//empty line

                //calc level
                var level = (line.Length - trimmed.Length) / TabSize;
                //is block?
                var isBlock = trimmed.StartsWith('-');

                if (level > stack.Count)
                    stack.Push(current);
                while (level < stack.Count)
                    stack.Pop();

                //parse name and value
                current = new YamlNode();
                trimmed = trimmed.Trim('-', ' ');
                var parts = trimmed.Split(':');
                current.Name = parts[0].Trim();
                if (parts.Length == 2)
                    current.ValueAsString = parts[1].Trim();

                AddToParent(current);
            }

            return result;

            void AddToParent(YamlNode node)
            {
                if (stack.Count == 0)
                    result.AddChildren(node);
                else
                    stack.Peek().AddChildren(node);
            }
        }
    }

    class YamlNode
    {
        public string Name;
        public string ValueAsString;
        public bool IsBlock;

        public List<YamlNode> Children;

        public int ValueAsInt => int.Parse(ValueAsString);
        public float ValueAsFloat => float.Parse(ValueAsString, CultureInfo.InvariantCulture);

        public YamlNode this[string attrName]
        {
            get
            {
                if (Children != null)
                for (int  i = Children.Count - 1; i>=0; i--)
                    if (Children[i].Name == attrName)
                        return Children[i];

                return null;
            }
        }

        public int GetInt(string attrName, int defaultValue)
        {
            var node = this[attrName];
            if (node == null)
                return defaultValue;
            return node.ValueAsInt;
        }

        public void AddChildren(YamlNode node)
        {
            if (Children == null)
                Children = new List<YamlNode>();
            Children.Add(node);
        }

        public override string ToString()
        {
            return Name + ": " + ValueAsString;
        }
    }
}
