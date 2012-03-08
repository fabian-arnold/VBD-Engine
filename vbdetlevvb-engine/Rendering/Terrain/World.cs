using System;
using System.Collections.Generic;
using System.Text;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using vbdetlevvb_engine.Rendering.Camera;
namespace vbdetlevvb_engine.Rendering.Terrain
{
    public class World: Interfaces.RenderAbleObject
    {
        Logging.Logger log;
        Chunk[,] chunks;
        
        int ChunkOffset = 65535 / 2;
        Window window;
        BasicCamera camera;
        public World(Window window)
        {
            //	-32,768 to 32,767
            chunks = new Chunk[255, 65535];
            log = window.logger;
            this.window = window;
            this.camera = (BasicCamera)window.camera;
            /*
             * 
             * Chunkgröße = 2*2
             * byte 256;
             * Höhe 256 CHunks
             */

        }
        public void OnRender()
        {
        }
        public void OnUpdate()
        {

            Chunk activechunk = chunks[((int)camera.pos.Y * 3), (int)(camera.pos.X + ChunkOffset)];

        }
        public void OnLoad()
        {
        }
        public void OnDispose()
        {
        }

    }
}
