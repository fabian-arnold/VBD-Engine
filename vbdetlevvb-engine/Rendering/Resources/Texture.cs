using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Text;
using OpenTK.Graphics.OpenGL;

namespace vbdetlevvb_engine.Rendering.Resources
{
    class Texture
    {
        private int id;
        Bitmap bitmap;

        public Texture(string Name)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string directoryName = Path.GetDirectoryName(assembly.Location);
            string logFileName =  directoryName + "/Data/Textures/" + Name;
            
            
            bitmap = new Bitmap(new FileStream(logFileName, FileMode.Open));
            BitmapData data = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height),
                ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GenerateTexture(data);

           
        }

        public void bind()
        {
            
            GL.BindTexture(TextureTarget.Texture2D, id);
        }

        private void GenerateTexture(BitmapData data)
        {
            GL.GenTextures(1, out id);
            GL.BindTexture(TextureTarget.Texture2D, id);



            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

            bitmap.UnlockBits(data);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
        }
    }
}
