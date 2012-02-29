using System;
using System.Collections.Generic;
using System.Text;

namespace vbdetlevvb_engine.Interfaces
{
    interface RederAbleObject
    {
        void OnRender();
        void OnUpdate();
        void OnLoad();
        void OnDispose();

    }
}
