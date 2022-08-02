using Microsoft.Azure.Management.Media.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace RadioArchive
{
    public interface IStreamingLocatorGenerator
    {
        Task<IDictionary<string, StreamingPath>> Generate(string name, Stream blob, TimeSpan? start = null, TimeSpan? end = null);
    }
}

