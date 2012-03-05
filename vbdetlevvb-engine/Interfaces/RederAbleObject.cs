using System;
using System.Collections.Generic;
using System.Text;

namespace vbdetlevvb_engine.Interfaces
{
    interface RenderAbleObject
    {
        void OnRender();
        void OnUpdate();
        void OnLoad();
        void OnDispose();

    }
}
