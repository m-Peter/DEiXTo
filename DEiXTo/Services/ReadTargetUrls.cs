using System.IO;

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
