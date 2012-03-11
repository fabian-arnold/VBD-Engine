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
        
        //int ChunkOffset = 65535 / 2;
        Window window;
        int ChunkX, ChunkY;
        BasicCamera camera;
        public World(Window window)
        {
            //	-32,768 to 32,767
            chunks = new Chunk[65535,255];
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
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    if (chunks[ChunkX + x, ChunkY + y] == null)
                    {
                        chunks[ChunkX + x, ChunkY + y] = new Chunk(ref window, new Vector2((ChunkX + x) * 3, (ChunkY + y) * 3));
                        chunks[ChunkX + x, ChunkY + y].OnLoad();
                    }
                    chunks[ChunkX + x, ChunkY + y].OnRender();
                   
                }

            }

        }
        public void OnUpdate()
        {
            ChunkX = (int)(camera.pos.X / 3);
            ChunkY = ((int)camera.pos.Y / 3);
            if (ChunkY < 0)
                ChunkY = 0;
            if (ChunkX < 0)
                ChunkX = 0;

            if (ChunkX > 65535 - 3)
                ChunkX = 65535 - 3;

            if (ChunkY > 255-3)
                ChunkY = 255-3;


            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    if (chunks[ChunkX + x, ChunkY + y] != null)
                        chunks[ChunkX + x, ChunkY + y].OnUpdate();
                }
            }          
        }
        public void OnLoad()
        {
        }
        public void OnDispose()
        {
        }

    }
}
