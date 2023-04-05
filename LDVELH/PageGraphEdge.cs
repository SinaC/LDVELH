using QuickGraph;
using System.Windows.Media;

namespace LDVELH
{
    public enum PageGraphEdgeStatus
    {
        Normal,
        Reverse,
        Calculated
    }

    public class PageGraphEdge : Edge<PageGraphVertex>
    {
        public PageGraphEdgeStatus Status { get; }

        public Brush EdgeBrush
        {
            get
            {
                switch (Status)
                {
                    case PageGraphEdgeStatus.Normal:
                        return Brushes.Black;
                    case PageGraphEdgeStatus.Calculated:
                        return Brushes.Blue;
                    case PageGraphEdgeStatus.Reverse:
                        return Brushes.Green;
                    default:
                        return Brushes.Red;
                }
            }
        }

        public PageGraphEdge(PageGraphVertex source, PageGraphVertex target, PageGraphEdgeStatus status) : base(source, target)
        {
            Status = status;
        }

        public override string ToString()
        {
            switch (Status)
            {
                case PageGraphEdgeStatus.Normal:
                    return $"{Source.PageNumber} ===> {Target.PageNumber}";
                case PageGraphEdgeStatus.Calculated:
                    return $"{Source.PageNumber} ---> {Target.PageNumber}";
                case PageGraphEdgeStatus.Reverse:
                    return $"{Source.PageNumber} OOO> {Target.PageNumber}";
                default:
                    return $"{Source.PageNumber} ???> {Target.PageNumber}";
            }
        }
    }
}
