using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Ataque_de_ojos
{
    public class leerComponent : Microsoft.Xna.Framework.GameComponent
    {
        int number_elements;
        string[] elements;
        Color unselected_color;
        Vector2 free_position = new Vector2(0, 0);
        SpriteFont font;
        int separation;

        public leerComponent(Game game, int Num_elements, Color unselected_c, SpriteFont _font, int _separation)
            : base(game)
        {
            number_elements = Num_elements;
            elements = new string[number_elements];
            unselected_color = unselected_c;
            font = _font;
            separation = _separation;
        }

        public void AddElement(int element_number, string element_name)
        {
            if ((element_number > -1) && (element_number < number_elements))
            {
                elements[element_number] = element_name;
            }
        }

        private Vector2 size_menu()
        {
            float width = 0, high = 0;

            for (int i = 0; i < number_elements; i++)
            {
                if (font.MeasureString(elements[ i ].ToString()).X > width) width = font.MeasureString(elements[ i ]).X;
                if (font.MeasureString(elements[ i ].ToString()).Y > high) high = font.MeasureString(elements[ i ]).Y;
            }

            high = high * (number_elements / 2);

            return new Vector2(width, high);
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D tMenu, StringBuilder texto)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(tMenu, Vector2.Zero, Color.White);
            spriteBatch.DrawString(font, "Uriel Emiliano", new Vector2(630, 450), Color.WhiteSmoke);
            spriteBatch.DrawString(font, texto.ToString(), new Vector2(350, 200), Color.WhiteSmoke);
            for (int i = 0; i < number_elements; i++)
            {
                spriteBatch.DrawString(font, elements[i].ToString(), new Vector2(300, 150 + (separation * i)), unselected_color);
            }

            spriteBatch.End();
        }


    }
}
