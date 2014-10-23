using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEiXTo.Services
{
    public class ReadTargetUrls
    {
        public string[] Read(string filename)
        {
            return File.ReadAllLines(filename);
        }
    }
}
