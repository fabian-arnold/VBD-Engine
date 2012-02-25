using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;

namespace vbdetlevvb_engine.Rendering.Mesh
{
    
    struct VertexPositionColor
    {
        public OpenTK.Vector3 Position;
        public uint Color;

        public VertexPositionColor(float x, float y, float z, Color color)
        {
            Position = new OpenTK.Vector3(x, y, z);
            Color = ToRgba(color);
        }
        public VertexPositionColor(OpenTK.Vector3 pos, Color color)
        {
            Position = pos;
            Color = ToRgba(color);
        }

        public VertexPositionColor(OpenTK.Vector2 pos, System.Drawing.Color color)
        {
            Position = new OpenTK.Vector3(pos.X, pos.Y, 0);
            Color = ToRgba(color);
        }

        static uint ToRgba(Color color)
        {
            return (uint)color.A << 24 | (uint)color.B << 16 | (uint)color.G << 8 | (uint)color.R;
        }
    }

    struct VertexPosition
    {
        public OpenTK.Vector3 Position;

        public VertexPosition(float x, float y, float z)
        {
            Position = new OpenTK.Vector3(x, y, z);
        }
        public VertexPosition(OpenTK.Vector3 pos)
        {
            Position = pos;
        }

        public VertexPosition(OpenTK.Vector2 pos)
        {
           // Console.WriteLine(pos);
            Position = new OpenTK.Vector3(pos.X, pos.Y, 0);

        }
    }
    struct TextureCoords
    {
        public OpenTK.Vector2 Position;

        public TextureCoords(float x, float y)
        {
            
            Position = new OpenTK.Vector2(x, y);
        }
        public TextureCoords(OpenTK.Vector2 pos)
        {
            
            Position = pos;
        }

       
    }
}
