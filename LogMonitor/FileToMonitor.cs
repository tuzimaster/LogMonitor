using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LogMonitor
{
    public class FileToMonitor
    {
        public string path;
        public DateTime FileEditedTimestamp;

        public void setPath(string paramPath)
        {
            path = paramPath;
        }

        public string getPath()
        {
            return path;
        }

        public void setDateTime(DateTime dtsmp)
        {
            FileEditedTimestamp = dtsmp;
        }

        public DateTime getTimeStamp()
        {
            return FileEditedTimestamp;
        }
    }
}
