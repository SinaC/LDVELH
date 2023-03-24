using QuickGraph;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LDVELH
{
    //https://sachabarbs.wordpress.com/2010/08/31/pretty-cool-graphs-in-wpf/
    public class PageGraph : BidirectionalGraph<PageGraphVertex, PageGraphEdge>
    {
        public PageGraph(IEnumerable<PageVM> pages, bool createUnknownVertex)
        {
            // create vertices
            var vertices = pages.Select(x => new PageGraphVertex(x)).ToList();
            AddVertexRange(vertices);
            // create edges excluding reverse link
            var edges = new List<PageGraphEdge>();
            var newVertices = new List<PageGraphVertex>();
            foreach (var vertex in vertices)
            {
                var source = vertex;
                foreach (var link in source.Links.Where(x => x != -1)) // -1 is reverse
                {
                    var target = vertices.Concat(newVertices).SingleOrDefault(x => x.PageNumber == link);
                    if (target == null)
                    {
                        if (createUnknownVertex)
                        {
                            target = new PageGraphVertex(new PageVM(link));
                            newVertices.Add(target);
                            AddVertex(target);
                        }
                        else
                            throw new Exception($"Page with number {link} not found");
                    }
                    var edge = new PageGraphEdge(source, target);
                    edges.Add(edge);
                }
            }
            vertices.AddRange(newVertices);
            // create reverse
            foreach (var vertex in vertices.Where(x => x.Links.Contains(-1)))
            {
                var source = vertex;
                // search target: vertices with source as target
                var targets = vertices.Where(x => x.Links.Contains(source.PageNumber));
                foreach (var target in targets)
                {
                    var edge = new PageGraphEdge(source, target);
                    edges.Add(edge);
                }
            }
            AddEdgeRange(edges);
        }
    }
}
