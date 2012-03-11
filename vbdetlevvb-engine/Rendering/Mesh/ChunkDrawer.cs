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
    class ChunkDrawer: Interfaces.RenderAbleObject
    {

        Logging.Logger log;

        public ChunkDrawer(ref Logging.Logger log)
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


            if( ConcaveTriangulator.Triangulate( value, new int[] { value.Length }, ref log ) )
            {

                this.vertices = vertices;
                this.indices = ConcaveTriangulator.Indices.ToArray();
            }
        }

        protected int texture;
        public virtual void OnUpdate() { }
        public virtual void OnDraw() 
        {
            GL.Begin(BeginMode.Triangles);
            GL.BindTexture(TextureTarget.Texture2D, texture);
            foreach (int index in indices)
            {
                GL.TexCoord2(((double)vertices[index].Position.X), ((double)vertices[index].Position.Y));
                GL.Vertex3(new float[] { vertices[index].Position.X, vertices[index].Position.Y, vertices[index].Position.Z });
                //log.Log("X",vertices[index].Position.X +" Y:"+ vertices[index].Position.Y);
               
            }
            GL.End();
            
        }
        public virtual void OnDispose() { }
        public virtual void OnRender() { }
        public virtual void OnLoad() { }


    }
}
