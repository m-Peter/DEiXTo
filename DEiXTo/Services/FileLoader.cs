using System.IO;

namespace DEiXTo.Services
{
    public class FileLoader : IFileLoader
    {
        public Stream Load(string filename, FileMode mode)
        {
            return new FileStream(filename, mode);
        }
    }
}
