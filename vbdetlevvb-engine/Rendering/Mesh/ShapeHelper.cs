using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace vbdetlevvb_engine.Rendering.Mesh
{
    class ShapeHelper
    {

        public static Shape Quad(float Height, float Width)
        {
            VertexPosition[] QuadVertices = new VertexPosition[]
            {
                    new VertexPosition(-Width, -Height,  0.0f),
                    new VertexPosition( Width, -Height,  0.0f),
                    new VertexPosition( Width,  Height,  0.0f),
                    new VertexPosition(-Width,  Height,  0.0f)
            };

             short[] QuadElements = new short[]
            {
                0, 1, 2, 2, 3, 0, // front face
           
            };
            Shape q = new Shape();
            
            q.Load(QuadVertices, QuadElements);
            return q;
        }

    }
}
