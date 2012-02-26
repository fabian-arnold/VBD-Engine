using System;
using System.Collections.Generic;
using System.Text;
using OpenTK;

namespace vbdetlevvb_engine.Rendering.Camera
{
    public abstract class Camera
    {

        public abstract void DoResize(int Height, int Width);

        public abstract void Update();

        public abstract void OnLoad(int Height, int Width);

        public abstract Matrix4 perpectiveMatrix();

        public abstract Matrix4 lookatMatrix();

    }
}
