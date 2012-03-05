using System;
using System.Collections.Generic;
using System.Xml;
using System.Runtime.InteropServices;
using System.Diagnostics;
using OpenTK;
using GL = OpenTK.Graphics.OpenGL.GL;
using OpenTK.Graphics.OpenGL;
namespace vbdetlevvb_engine.Rendering.Mesh
{
    class Polygon: Interfaces.RenderAbleObject
    {

        Logging.Logger log;

        public Polygon(ref Logging.Logger log)
        {
            this.log = log;
           
        }

        int[] indices;
        VertexPosition[] vertices;
        public void BufferVertices(ref VertexPosition[] vertices)
        {
            Vector2[] value = new Vector2[vertices.Length];
            for(int i = 0; i<value.Length;i++)
            {
                value[i] = new Vector2(vertices[i].Position.X,vertices[i].Position.Y); 
            }
            

            log.Log("Triangluation", ""+ConcaveTriangulator.Triangulate(value, new int[] { value.Length }));
           
            this.vertices = vertices;
            this.indices = ConcaveTriangulator.Indices.ToArray();
        }


        public virtual void OnUpdate() { }
        public virtual void OnDraw() 
        {
            GL.Begin(BeginMode.Triangles);
            foreach (int index in indices)
            {
                GL.Vertex3(new float[] { vertices[index].Position.X, vertices[index].Position.Y, vertices[index].Position.Z });
            }
            GL.End();
        }
        public virtual void OnDispose() { }
        public virtual void OnRender() { }
        public virtual void OnLoad() { }


    }
}
