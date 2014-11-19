using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Ataque_de_ojos
{
    public class menuComponent : Microsoft.Xna.Framework.GameComponent
    {
        int number_elements;
        int element_active = 0;
        string[] elements;
        Color selected_color;
        Color unselected_color;
        Vector2 free_position = new Vector2(0, 0);
        SpriteFont font;
        int separation;
        private Boolean key_Up_press;
        private Boolean key_Down_press;

        public menuComponent(Game game, int Num_elements, Color selected_c, Color unselected_c, SpriteFont _font, int _separation)
            : base(game)
        {
            number_elements = Num_elements;
            element_active = 0;
            elements = new string[number_elements];
            selected_color = selected_c;
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

        private void next_item()
        {
            if (element_active < number_elements - 1)
            {
                element_active++;
            }
            else
            {
                element_active = 0;
            }
        }

       private void previus_item()
        {
            if (element_active > 0)
            {
                element_active--;
            }
            else
            {
                element_active = number_elements - 1;
            }
        }

        public void Press_keys(KeyboardState keys, SoundEffect sTick)
        {
            if (keys.IsKeyDown(Keys.Up) && key_Up_press)
            {
                this.previus_item();
                key_Up_press = false;
                sTick.Play();
            }
            else
            {
                if (keys.IsKeyUp(Keys.Up))
                {
                    key_Up_press = true;
                    
                }
            }

            if (keys.IsKeyDown(Keys.Down) && key_Down_press)
            {
                this.next_item();
                key_Down_press = false;
                sTick.Play();
            }
            else
            {
                if (keys.IsKeyUp(Keys.Down))
                {
                    key_Down_press = true;
                }
            }
        }

        public int Element_active()
        {
            return element_active + 1;
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


        public void Draw(SpriteBatch spriteBatch, Texture2D tMenu)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(tMenu, Vector2.Zero, Color.White);
            spriteBatch.DrawString(font, "Uriel Emiliano", new Vector2(630, 450), Color.WhiteSmoke);
            for (int i = 0; i < number_elements; i++)
            {
                if (element_active == i)
                {
                    spriteBatch.DrawString(font, elements[i].ToString(), new Vector2(350, 150 + (separation * i)), selected_color);
                }
                else
                {
                    spriteBatch.DrawString(font, elements[i].ToString(), new Vector2(300, 150 + (separation * i)), unselected_color);
                }
            }

            spriteBatch.End();
        }


    }
}
