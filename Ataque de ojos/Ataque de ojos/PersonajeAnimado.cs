#region sentencias using
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.GamerServices;
#endregion

namespace Ataque_de_ojos
{
    public class PersonajeAnimado
    {
        private int framecount;
        private Texture2D myTexture;
        private float timePerFrame;
        private int frame;
        private float totalElapsed;

        public PersonajeAnimado(Vector2 pos, int frameCount, int framesPerSec)
        {
            framecount = frameCount;
            timePerFrame = 1.0f/ framesPerSec;
            frame = 0;
            totalElapsed = 0;
        }

        public void Load(ContentManager content, string textName)
        {
            myTexture = content.Load<Texture2D>(textName);
        }

        public void UpdateFrame(float elapsed)
        {
            totalElapsed += elapsed;
            if (totalElapsed > timePerFrame)
            {
                frame++;
                //Los frames son circulares, al llegar al último volvemos al primero.
                frame = frame % framecount;
                totalElapsed -= timePerFrame;
            }
        }

        public void DrawFrame(SpriteBatch batch, Vector2 pos, Microsoft.Xna.Framework.Color color, SpriteEffects inv)
        {
            DrawFrame(batch, frame, pos, color, inv);
        }

        public void DrawFrame(SpriteBatch batch, int frame, Vector2 pos, Microsoft.Xna.Framework.Color color, SpriteEffects inv)
        {
            int FrameHeight = myTexture.Height / framecount;

            //Recorte de la imagen, Variables: Alto, Ancho, X, Y
            Rectangle sourcerect = new Rectangle(0, frame * FrameHeight,myTexture.Width , FrameHeight);

            batch.Draw(myTexture, pos, sourcerect, color,0, new Vector2(0,0), 1, inv, 1);
        }


    }
}
