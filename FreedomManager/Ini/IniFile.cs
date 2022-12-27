using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace FreedomManager.Ini
{
    [DebuggerDisplay("Sections.Count = {Sections.Count}")]
    public sealed class IniFile
    {
        private List<string> trailingComments;

        public IniSection DefaultSection { get; } = new IniSection("<default>");

        public IniSectionCollection Sections { get; } = new IniSectionCollection();

        public IniSection this[string name] => Sections[name];

        public List<string> LeadingComments => DefaultSection.Comments;

        public bool HasLeadingComments => DefaultSection.HasComments;

        public int LeadingCommentCount => DefaultSection.CommentCount;

        public void AddLeadingComment(string line) => DefaultSection.AddComment(line);

        public void AddLeadingBlankLine() => DefaultSection.AddBlankLine();

        public void AddLeadingComments(IEnumerable<string> lines) => DefaultSection.AddComments(lines);

        public void AddLeadingComments(params string[] lines) => DefaultSection.AddComments(lines);

        public List<string> TrailingComments
        {
            get
            {
                if (trailingComments == null)
                {
                    trailingComments = new List<string>();
                }
                return trailingComments;
            }
        }

        public bool HasTrailingComments
        {
            get
            {
                if (trailingComments == null)
                {
                    return false;
                }
                else
                {
                    return trailingComments.Count == 0;
                }
            }
        }

        public int TrailingCommentCount
        {
            get
            {
                if (trailingComments == null)
                {
                    return 0;
                }
                else
                {
                    return trailingComments.Count;
                }
            }
        }

        public void AddTrailingComment(string line)
        {
            if (line == null)
            {
                line = "";
            }
            TrailingComments.Add(line);
        }

        public void AddTrailingBlankLine() => AddTrailingComment("");

        public void AddTrailingComments(IEnumerable<string> lines)
        {
            if (trailingComments == null)
            {
                trailingComments = new List<string>(lines);
            }
            else
            {
                trailingComments.AddRange(lines);
            }
        }

        public void AddTrailingComments(params string[] lines) => AddTrailingComments(lines.AsEnumerable());

        public void ClearTrailingComments() => trailingComments?.Clear();
    }
}
