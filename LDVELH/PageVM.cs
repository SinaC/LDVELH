using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LDVELH
{
    public class PageVM : BindableBase
    {
        public PageVM(int pageNumber)
        {
            PageNumber = pageNumber;
        }

        public PageVM(int pageNumber, string rawLinks, string notes)
        {
            PageNumber = pageNumber;
            RawLinks = rawLinks;
            Notes = notes;
        }

        public int PageNumber { get; }

        private string _rawLinks;
        public string RawLinks
        {
            get => _rawLinks;
            set => SetProperty(ref _rawLinks, value);
        }

        private string _notes;
        public string Notes
        {
            get => _notes;
            set => SetProperty(ref _notes, value);
        }

        public IEnumerable<int> Links
        {
            get
            {
                if (string.IsNullOrWhiteSpace(RawLinks) || RawLinks == "/")
                    return Enumerable.Empty<int>();
                return RawLinks.Split(',', ';', '.', '|').Select(x => Convert.ToInt32(x));
            }
        }
    }
}
