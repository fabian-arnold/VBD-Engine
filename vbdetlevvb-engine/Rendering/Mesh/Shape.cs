using System;
using System.Collections.Generic;
using System.Text;
using vbdetlevvb_engine.Rendering.VBO;

namespace vbdetlevvb_engine.Rendering.Mesh
{
    class Shape
    {

        VertexPosition[] vertices = null;
        short[] elements;
        Vbo vbo = new Vbo();

        public void Load(VertexPosition[] vertices, short[] elements)
        {
            this.vertices = vertices;
            this.elements = elements;
            throw new ApplicationException("not created class");
            //vbo = VBOHelper.LoadStaticVBO(vertices, elements);
        }

        public void Draw()
        {
            //VBOHelper.Draw(vbo, ref vertices);
        }

        public void Dispose()
        {
        }
    }
}
