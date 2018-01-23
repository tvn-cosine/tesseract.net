using System.Collections.Generic;
using System.Collections;

namespace Tesseract
{
    public class PageIteratorEnumerator : IEnumerator<PageIterator>
    {
        private readonly PageIteratorLevel pageIteratorLevel;
        private readonly PageIterator pageIterator;

        public PageIteratorEnumerator(PageIterator pageIterator, PageIteratorLevel pageIteratorLevel)
        {
            this.pageIteratorLevel = pageIteratorLevel;
            this.pageIterator = pageIterator;
        }

        public PageIterator Current
        {
            get
            {
                return pageIterator;
            }
        }

        object IEnumerator.Current
        {
            get
            {
                return Current;
            }
        }

        public void Dispose()
        {
            pageIterator.Dispose();
        }

        public bool MoveNext()
        {
            return pageIterator.Next(pageIteratorLevel);
        }

        public void Reset()
        {
            pageIterator.Begin();
        }
    }
}
