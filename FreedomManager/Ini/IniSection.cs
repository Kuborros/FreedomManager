using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace FreedomManager.Ini
{
    [DebuggerDisplay("Name = {Name}, Properties.Count = {Properties.Count}")]
    public sealed class IniSection : IniElement
    {
        public IniSection(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("Section name cannot be null or empty", nameof(name));
            }

            Name = name;
        }

        public IniSection(string name, IEnumerable<string> comments)
            : this(name)
        {
            if (comments != null)
            {
                this.comments = new List<string>(comments);
            }
        }

        public string Name { get; }

        public IniPropertyCollection Properties { get; } = new IniPropertyCollection();

        public string this[string key]
        {
            get => Properties[key].Value;
            set => Properties[key].Value = value;
        }
    }

    [DebuggerDisplay("Count = {Count}")]
    public sealed class IniSectionCollection : IEnumerable<IniSection>
    {
        private readonly LinkedList<IniSection> backingList;
        private readonly Dictionary<string, LinkedListNode<IniSection>> lookup;

        public IniSectionCollection()
        {
            backingList = new LinkedList<IniSection>();
            lookup = new Dictionary<string, LinkedListNode<IniSection>>(StringComparer.OrdinalIgnoreCase);
        }

        public int Count => backingList.Count;

        public IniSection this[string name] => lookup[name].Value;

        public bool TryGetSection(string name, out IniSection result)
        {
            if (lookup.TryGetValue(name, out var node))
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

        public bool Contains(string name) => lookup.ContainsKey(name);

        public void Add(IniSection section)
        {
            if (section == null)
            {
                throw new ArgumentNullException(nameof(section));
            }

            if (lookup.ContainsKey(section.Name))
            {
                throw new ArgumentException($"Section with name '{section.Name}' already exists", nameof(section));
            }

            var node = backingList.AddLast(section);
            lookup.Add(section.Name, node);
        }

        public IniSection Add(string name, IEnumerable<string> comments = null)
        {
            if (lookup.ContainsKey(name))
            {
                throw new ArgumentException($"Section with name '{name}' already exists", nameof(name));
            }

            var section = new IniSection(name, comments);
            Add(section);
            return section;
        }

        public void Remove(IniSection section)
        {
            // this doubles as a "do we actually contain this section" check
            //  (we use the lookup dictionary for this since Dictionary.Remove is probably faster than LinkedList.Remove)
            if (lookup.Remove(section.Name))
            {
                backingList.Remove(section);
            }
        }

        public void Remove(string name) => Remove(lookup[name].Value);

        public void Clear()
        {
            backingList.Clear();
            lookup.Clear();
        }

        public IniSection GetOrAddSection(string name)
        {
            if (!TryGetSection(name, out var section))
            {
                section = Add(name);
            }
            return section;
        }

        public IEnumerator<IniSection> GetEnumerator() => backingList.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
