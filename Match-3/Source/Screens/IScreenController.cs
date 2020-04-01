using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match_3.Source.Screens
{
    interface IScreenController
    {
        bool IsInitialized { get; }
        bool IsDisposed { get; }
        bool IsEnabled { get; }

        void Enable();

        void Disable();

        void Initialize();

        void CleanUp(bool clean);
    }
}
