using System.IO;

namespace DEiXTo.Services
{
    public class FileLoader : IFileLoader
    {
        public Stream Load(string filename)
        {
            return new FileStream(filename, FileMode.Open);
        }
    }
}
