using System.IO;

namespace DEiXTo.Services
{
    public interface IFileLoader
    {
        Stream Load(string filename);
    }
}
