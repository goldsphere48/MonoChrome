using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoChrome.SceneSystem
{
    public interface IScene
    {
        Type SceneType { get; }
        void Setup();
        void OnEnable();
        void OnDisable();
    }
}
