using System.Collections.Generic;
using System.Linq;

namespace LDVELH
{
    public class PageGraphVertex
    {
        public int PageNumber { get; }
        public IReadOnlyCollection<int> Links { get; }
        public string Notes { get; }

        public bool IsFirst { get; }
        public bool IsLast { get; }
        public bool IsDeadEnd { get; }

        public PageGraphVertex(PageVM pageVM)
        {
            PageNumber = pageVM.PageNumber;
            Links = pageVM.Links.ToArray();
            Notes = pageVM.Notes;

            IsFirst = pageVM.PageNumber == 1 || pageVM.Notes == "debut";
            IsLast = pageVM.Notes == "fin";
            IsDeadEnd = !IsFirst && !IsLast && Links?.Count == 0;
        }

        public override string ToString()
        {
            return $"{PageNumber}";
        }
    }
}
