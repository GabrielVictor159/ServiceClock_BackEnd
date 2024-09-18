using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceClock_BackEnd.Application.Interfaces.Services;
public interface IBlobService
{
    (bool Sucess, string Id, Exception? e) SaveBlob(string Blob, string FileName);
    void DeleteBlob(string blobName);
}
