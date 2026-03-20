using System.Collections.Generic;

namespace PGC.ModelEvent.Data
{
    public class LineClearedEvent
    {
        public List<int> lines;

        public LineClearedEvent(List<int> lines)
        {
            this.lines = lines;
        }
    }
}