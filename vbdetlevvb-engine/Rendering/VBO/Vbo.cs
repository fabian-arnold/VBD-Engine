using System;
using System.Collections.Generic;
using System.Text;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using GL = OpenTK.Graphics.OpenGL.GL;
namespace vbdetlevvb_engine.Rendering.VBO
{
    class Vbo {
        public int vertexBufferID, textureCoordBufferID, normalBufferID, colorBufferID, NumElements;
        protected int verticesNumber;
        protected BeginMode drawMode = BeginMode.Triangles;

        public void Draw()
        {
            if (vertexBufferID > 0)
            {
                GL.EnableClientState(ArrayCap.VertexArray);
                GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferID);
                GL.VertexPointer(3, VertexPointerType.Float, 0, 0);
                //glVertexPointer(3, GL_FLOAT, 0, 0);

                if (colorBufferID > 0)
                {
                    GL.EnableClientState(ArrayCap.ColorArray);
                    GL.BindBuffer(BufferTarget.ArrayBuffer, colorBufferID);
                    GL.ColorPointer(4, ColorPointerType.Float, 0, 0);
                    //glColorPointer(4, GL_FLOAT, 0, 0);
                }

                if (textureCoordBufferID > 0)
                {
                    GL.EnableClientState(ArrayCap.TextureCoordArray);
                    GL.BindBuffer(BufferTarget.ArrayBuffer, textureCoordBufferID);
                    //glColorPointer(2, GL_FLOAT, 0, 0);
                    GL.TexCoordPointer(2, TexCoordPointerType.Float, 0, 0);
                    
                }

                if (normalBufferID > 0)
                {
                    GL.EnableClientState(ArrayCap.NormalArray);
                    GL.BindBuffer(BufferTarget.ArrayBuffer, normalBufferID);
                    GL.NormalPointer(NormalPointerType.Float, 0, 0);
                    //glNormalPointer(GL_FLOAT, 0, 0);
                }

                GL.DrawArrays(drawMode, 0, verticesNumber);

                //glDrawRangeElements(drawMode, 0, verticesNumber, verticesNumber, GL_UNSIGNED_INT, 0);

                GL.DisableClientState(ArrayCap.VertexArray);
                GL.DisableClientState(ArrayCap.ColorArray);
                GL.DisableClientState(ArrayCap.TextureCoordArray);
                GL.DisableClientState(ArrayCap.NormalArray);
            }
        }

        public void BufferVertices<TVertex>(ref TVertex[] vertices) where TVertex : struct
        {
            int size;
            GL.GenBuffers(1, out vertexBufferID);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferID);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertices.Length * BlittableValueType.StrideOf(vertices)), vertices, BufferUsageHint.StreamDraw);
            GL.GetBufferParameter(BufferTarget.ArrayBuffer, BufferParameterName.BufferSize, out size);
            if (vertices.Length * BlittableValueType.StrideOf(vertices) != size)
                throw new ApplicationException("Vertex data not uploaded correctly");
            verticesNumber = vertices.Length;
        }

        public void BufferColor<TColors>(ref TColors[] colors) where TColors : struct
        {
            int size;
            GL.GenBuffers(1, out colorBufferID);
            GL.BindBuffer(BufferTarget.ArrayBuffer, colorBufferID);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(colors.Length * BlittableValueType.StrideOf(colors)), colors, BufferUsageHint.StreamDraw);
            GL.GetBufferParameter(BufferTarget.ArrayBuffer, BufferParameterName.BufferSize, out size);
            if (colors.Length * BlittableValueType.StrideOf(colors) != size)
                throw new ApplicationException("Vertex data not uploaded correctly");
        }
        
        public void BufferTexCoords<TTexCoords>(ref TTexCoords[] vertices) where TTexCoords : struct
        {
            int size;
            GL.GenBuffers(1, out textureCoordBufferID);
            GL.BindBuffer(BufferTarget.ArrayBuffer, textureCoordBufferID);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertices.Length * BlittableValueType.StrideOf(vertices)), vertices, BufferUsageHint.StreamDraw);
            GL.GetBufferParameter(BufferTarget.ArrayBuffer, BufferParameterName.BufferSize, out size);
            if (vertices.Length * BlittableValueType.StrideOf(vertices) != size)
                throw new ApplicationException("Vertex data not uploaded correctly");
        }
       
        public void BufferNormals<TNormals>(ref TNormals[] vertices) where TNormals : struct
        {
            int size;
            GL.GenBuffers(1, out normalBufferID);
            GL.BindBuffer(BufferTarget.ArrayBuffer, normalBufferID);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertices.Length * BlittableValueType.StrideOf(vertices)), vertices, BufferUsageHint.StreamDraw);
            GL.GetBufferParameter(BufferTarget.ArrayBuffer, BufferParameterName.BufferSize, out size);
            if (vertices.Length * BlittableValueType.StrideOf(vertices) != size)
                throw new ApplicationException("Vertex data not uploaded correctly");
        }



        public void Dispose()
        {
            if (vertexBufferID > 0) 
                GL.DeleteBuffers(1, ref vertexBufferID);

            if (colorBufferID > 0)
                GL.DeleteBuffers(1, ref colorBufferID);

            if (textureCoordBufferID > 0)
                GL.DeleteBuffers(1, ref textureCoordBufferID);

           

        }
    
    }
}
