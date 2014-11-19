using DEiXTo.Models;
using System.IO;

namespace DEiXTo.Services
{
    public interface IDeixtoWrapperRepository
    {
        DeixtoWrapper Load(Stream stream);
        void Save(DeixtoWrapper wrapper, Stream stream);
    }
}
