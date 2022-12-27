using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace FreedomManager.Ini
{
    [DebuggerDisplay("{Key} = {Value}")]
    public sealed class IniProperty : IniElement
    {
        public IniProperty(string key, string value)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("Property key cannot be null or empty", nameof(key));
            }

            Key = key;
            Value = value;
        }

        public IniProperty(string key, string value, IEnumerable<string> comments)
            : this(key, value)
        {
            if (comments != null)
            {
                this.comments = new List<string>(comments);
            }
        }

        public string Key { get; }

        public string Value { get; set; }
    }

    [DebuggerDisplay("Count = {Count}")]
    public sealed class IniPropertyCollection : IEnumerable<IniProperty>
    {
        private readonly LinkedList<IniProperty> backingList;
        private readonly Dictionary<string, LinkedListNode<IniProperty>> lookup;

        public IniPropertyCollection()
        {
            backingList = new LinkedList<IniProperty>();
            lookup = new Dictionary<string, LinkedListNode<IniProperty>>(StringComparer.OrdinalIgnoreCase);
        }

        public int Count => backingList.Count;

        public IniProperty this[string key]
        {
            get
            {
                if (TryGetProperty(key, out var result))
                {
                    return result;
                }
                else
                {
                    throw new KeyNotFoundException($"Property with key '{key}' was not found");
                }
            }
        }

        public bool TryGetProperty(string key, out IniProperty result)
        {
            if (lookup.TryGetValue(key, out var node))
            {
                result = node.Value;
                return true;
            }
            else
            {
                result = null;
                return false;
            }
        }

        public bool Contains(string key) => lookup.ContainsKey(key);

        public void Add(IniProperty property)
        {
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }

            if (lookup.ContainsKey(property.Key))
            {
                throw new ArgumentException($"Property with key '{property.Key}' already exists", nameof(property));
            }

            var node = backingList.AddLast(property);
            lookup.Add(property.Key, node);
        }

        public IniProperty Add(string key, string value, IEnumerable<string> comments = null)
        {
            if (lookup.ContainsKey(key))
            {
                throw new ArgumentException($"Property with key '{key}' already exists", nameof(key));
            }

            var property = new IniProperty(key, value, comments);
            Add(property);
            return property;
        }

        public void Remove(IniProperty property)
        {
            // this doubles as a "do we actually contain this property" check
            //  (we use the lookup dictionary for this since Dictionary.Remove is probably faster than LinkedList.Remove)
            if (lookup.Remove(property.Key))
            {
                backingList.Remove(property);
            }
        }

        public void Remove(string key) => Remove(lookup[key].Value);

        public void Clear()
        {
            backingList.Clear();
            lookup.Clear();
        }

        public IniProperty GetOrAddProperty(string key, string defaultValue = "")
        {
            if (!TryGetProperty(key, out var property))
            {
                property = Add(key, defaultValue);
            }
            return property;
        }

        public IEnumerator<IniProperty> GetEnumerator() => backingList.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
