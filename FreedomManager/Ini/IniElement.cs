using System.Collections.Generic;
using System.Linq;

namespace FreedomManager.Ini
{
    public abstract class IniElement
    {
        protected List<string> comments;

        public IniElement() { }

        public IniElement(IEnumerable<string> comments)
        {
            if (comments != null)
            {
                this.comments = new List<string>(comments);
            }
        }

        public List<string> Comments
        {
            get
            {
                if (comments == null)
                {
                    comments = new List<string>();
                }
                return comments;
            }
        }

        public bool HasComments
        {
            get
            {
                if (comments == null)
                {
                    return false;
                }
                else
                {
                    return comments.Count == 0;
                }
            }
        }

        public int CommentCount
        {
            get
            {
                if (comments == null)
                {
                    return 0;
                }
                else
                {
                    return comments.Count;
                }
            }
        }

        public void AddComment(string line)
        {
            if (line == null)
            {
                line = "";
            }
            Comments.Add(line);
        }

        public void AddBlankLine() => AddComment("");

        public void AddComments(IEnumerable<string> lines)
        {
            if (comments == null)
            {
                comments = new List<string>(lines);
            }
            else
            {
                comments.AddRange(lines);
            }
        }

        public void AddComments(params string[] lines) => AddComments(lines.AsEnumerable());

        public void ClearComments() => comments?.Clear();
    }
}
