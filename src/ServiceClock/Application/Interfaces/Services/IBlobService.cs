using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceClock_BackEnd.Application.Interfaces.Services;
public interface IBlobService
{
    (bool Sucess, string Id, Exception? e) SaveImage(string Blob);
    void DeleteImage(string blobName);
}
