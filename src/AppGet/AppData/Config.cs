using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppGet.AppData
{
    public interface IConfig
    {
        string LocalRepository { get; }
    }

    public class Config : IConfig
    {
        public string LocalRepository => @"C:\git\AppGet.Packages\manifests\";
    }
}
