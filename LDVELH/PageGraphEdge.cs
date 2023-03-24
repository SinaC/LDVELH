using QuickGraph;

namespace LDVELH
{
    public class PageGraphEdge : Edge<PageGraphVertex>
    {
        public PageGraphEdge(PageGraphVertex source, PageGraphVertex target) : base(source, target)
        {
        }

        public override string ToString()
        {
            return $"{Source.PageNumber} -> {Target.PageNumber}";
        }
    }
}
