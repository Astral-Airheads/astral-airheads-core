using System;
using System.IO;
using System.Text;

namespace Matt;

public class KeyValues
{
    public string Name { get; private set; }
    public string? Value { get; private set; }
    public KVType DataType { get; private set; } = KVType.None;

    public KeyValues? Parent { get; private set; }
    public KeyValues? FirstChild { get; private set; }
    public KeyValues? NextSibling { get; private set; }

    // Constructors
    public KeyValues(string name)
    {
        Name = name;
    }

    public KeyValues(string name, string value)
    {
        Name = name;
        SetString(value);
    }

    public bool LoadFromFile(string filePath)
    {
        if (!File.Exists(filePath))
            throw new FileNotFoundException($"Could not find KeyValues file: \"{filePath}\"!");

        var content = File.ReadAllText(filePath);
        if (string.IsNullOrEmpty(content))
            throw new FormatException("File's content is empty.");

        return Parse(content) != null;
    }

    // Setters
    public void SetString(string value)
    {
        Value = value;
        DataType = KVType.String;
    }

    public void SetInt(int value)
    {
        Value = value.ToString();
        DataType = KVType.Int;
    }

    public void SetFloat(float value)
    {
        Value = value.ToString(System.Globalization.CultureInfo.InvariantCulture);
        DataType = KVType.Float;
    }

    // Tree manipulation
    public void AddSubKey(KeyValues subkey)
    {
        if (subkey == null) return;
        subkey.Parent = this;
        if (FirstChild == null)
        {
            FirstChild = subkey;
        }
        else
        {
            KeyValues last = FirstChild;
            while (last.NextSibling != null)
                last = last.NextSibling;
            last.NextSibling = subkey;
        }
    }

    public void RemoveSubKey(KeyValues subkey)
    {
        if (subkey == null || FirstChild == null) return;
        if (FirstChild == subkey)
        {
            FirstChild = subkey.NextSibling;
        }
        else
        {
            KeyValues? prev = FirstChild;
            while (prev != null && prev.NextSibling != subkey)
                prev = prev.NextSibling;
            if (prev != null)
                prev.NextSibling = subkey.NextSibling;
        }
        subkey.Parent = null;
        subkey.NextSibling = null;
    }

    public KeyValues? GetFirstSubKey() => FirstChild;
    public KeyValues? GetNextKey() => NextSibling;

    public KeyValues? FindKey(string keyName, bool create = false)
    {
        for (var sub = FirstChild; sub != null; sub = sub.NextSibling)
        {
            if (string.Equals(sub.Name, keyName, StringComparison.OrdinalIgnoreCase))
                return sub;
        }
        if (create)
        {
            var newKey = new KeyValues(keyName);
            AddSubKey(newKey);
            return newKey;
        }
        return null;
    }

    public static KeyValues Parse(string text)
    {
        using var reader = new StringReader(text);
        return ParseBlock(reader);
    }

    private static KeyValues ParseBlock(StringReader reader)
    {
        KeyValues? root = null;
        KeyValues? current = null;
        string? token;
        while ((token = ReadToken(reader)) != null)
        {
            if (token == "}")
                break;
            if (token == "{")
            {
                if (current == null)
                    throw new FormatException("Unexpected '{' with no key name");
                var child = ParseBlock(reader);
                current.AddSubKey(child);
                continue;
            }
            string? valueOrBlock = ReadToken(reader);
            if (valueOrBlock == null)
                throw new FormatException("Unexpected end of input");
            if (valueOrBlock == "{")
            {
                var block = new KeyValues(token);
                block.AddSubKey(ParseBlock(reader));
                if (root == null) root = block;
                else root.AddSubKey(block);
                current = block;
            }
            else
            {
                var kv = new KeyValues(token, valueOrBlock);
                if (root == null) root = kv;
                else root.AddSubKey(kv);
                current = kv;
            }
        }
        if (root == null)
            throw new FormatException("No root KeyValues found");
        return root;
    }

    private static string? ReadToken(StringReader reader)
    {
        StringBuilder sb = new();
        bool inQuote = false;
        while (true)
        {
            int c = reader.Peek();
            if (c == -1) break;
            char ch = (char)c;
            if (!inQuote && char.IsWhiteSpace(ch))
            {
                reader.Read();
                if (sb.Length > 0) break;
                continue;
            }
            if (ch == '"')
            {
                reader.Read();
                inQuote = !inQuote;
                continue;
            }
            if (!inQuote && (ch == '{' || ch == '}'))
            {
                reader.Read();
                if (sb.Length == 0)
                    return ch.ToString();
                break;
            }
            sb.Append(ch);
            reader.Read();
        }
        if (sb.Length == 0) return null;
        return sb.ToString();
    }

    public override string ToString() => ToString(0);

    public string ToString(int indent)
    {
        var sb = new StringBuilder();
        string ind = new string(' ', indent * 2);
        sb.Append(ind).Append('"').Append(Name).Append('"');
        if (FirstChild != null)
        {
            sb.AppendLine().Append(ind).AppendLine("{");
            for (var sub = FirstChild; sub != null; sub = sub.NextSibling)
                sb.Append(sub.ToString(indent + 1));
            sb.Append(ind).AppendLine("}");
        }
        else if (Value != null)
        {
            sb.Append(' ').Append('"').Append(Value).Append('"').AppendLine();
        }
        else
        {
            sb.AppendLine();
        }
        return sb.ToString();
    }

    public string GetString(string keyName, string defaultValue = "")
    {
        var sub = FindKey(keyName);
        return sub?.Value ?? defaultValue;
    }

    public int GetInt(string keyName, int defaultValue = 0)
    {
        var sub = FindKey(keyName);
        if (sub?.Value == null) return defaultValue;
        if (int.TryParse(sub.Value, out int result))
            return result;
        if (float.TryParse(sub.Value, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out float f))
            return (int)f;
        return defaultValue;
    }

    public float GetFloat(string keyName, float defaultValue = 0f)
    {
        var sub = FindKey(keyName);
        if (sub?.Value == null) return defaultValue;
        if (float.TryParse(sub.Value, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out float result))
            return result;
        if (int.TryParse(sub.Value, out int i))
            return i;
        return defaultValue;
    }

    public bool GetBool(string keyName, bool defaultValue = false)
    {
        var sub = FindKey(keyName);
        if (sub?.Value == null) return defaultValue;
        if (bool.TryParse(sub.Value, out bool b))
            return b;
        if (int.TryParse(sub.Value, out int i))
            return i != 0;
        if (sub.Value.Equals("yes", StringComparison.OrdinalIgnoreCase) || sub.Value.Equals("true", StringComparison.OrdinalIgnoreCase) || sub.Value == "1")
            return true;
        if (sub.Value.Equals("no", StringComparison.OrdinalIgnoreCase) || sub.Value.Equals("false", StringComparison.OrdinalIgnoreCase) || sub.Value == "0")
            return false;
        return defaultValue;
    }
}
