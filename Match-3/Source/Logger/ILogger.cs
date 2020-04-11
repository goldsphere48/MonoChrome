using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match_3.Source.Logger
{
    interface ILogger
    {
         void Warn(string message);
         void Log(string message);
         void Error(string message);
    }
}
