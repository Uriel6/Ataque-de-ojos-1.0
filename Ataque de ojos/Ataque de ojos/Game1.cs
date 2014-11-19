/* Creditos
 * Diseno y Programacion: Uriel Emiliano
 * Sprites: Terraria http://terraria.org/
 * Tema musical: Soul Showdown by Maelstromrealm http://www.newgrounds.com/audio/listen/588099
 * Sonidos: Terraria http://terraria.org/
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
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
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //Declaracion de los menu
        menuComponent menu_principal;
        menuComponent menu_dificultad;
        leerComponent menu_tiempos;
        leerComponent menu_nombre;

        //Declaracion variables
        bool perder, ganar, facil, medio, dificil, sup, punt, diff;
        bool der = false, izq = false, mus = true;
        bool teclap, teclae;
        int timer, vidas, s, ms, ss, mss, cont = 0, scrollX=0;
        string tiempo = "Tiempo";
        //Declaracion texturas
        Texture2D tPj, tOjo, tFondo, tFondo2, tVida, tPerder, tGanar, tZombie, tMenu;
        //Declaracion posiciones
        Vector2 posPj, posOjo1, posOjo2, posOjo3, posText, posZombie;
        Vector2[] posVida = new Vector2[3];
        //Declaracion colores variables
        Microsoft.Xna.Framework.Color cPj;
        Microsoft.Xna.Framework.Color[] cVida = new Microsoft.Xna.Framework.Color[3];
        //Declaracion colision
        BoundingBox bPj, bZombie;
        BoundingSphere bOjo1, bOjo2, bOjo3;
        //Declaracion sonidos
        SoundEffect sKill, sHit, sAbrir, sCerrar, sTick;
        Song sMusic;
        //Declaracion del texto
        SpriteFont txtJuego, txtMenu;
        //Declaracion del random
        Random rnd = new Random((int)DateTime.Now.Ticks);
        //Personajes animados
        PersonajeAnimado AnimOjo1, AnimOjo2, AnimOjo3, AnimPj, AnimZombie;
        //Variables para escribir
        KeyboardState oldKeyState;
        StringBuilder texto = new StringBuilder();
        StringBuilder textof = new StringBuilder();
        //Lado de las texturas
        SpriteEffects iZombie, iPj = SpriteEffects.FlipHorizontally;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        //Modulos en general
        //Modulo de ordenar el Puntajes.txt
        public void OrdenarP()
        {
            int i, j, auxs, auxms;
            string auxn;
            var reader = new StreamReader(File.OpenRead("Puntajes.txt"));
            List<int> seg = new List<int>();
            List<int> mseg = new List<int>();
            List<string> nom = new List<string>();
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(',');

                seg.Add(int.Parse(values[0]));
                mseg.Add(int.Parse(values[1]));
                nom.Add(values[2]);
            }
            for (j = 0; j < nom.Count; j++)
            {
                for (i = 0; i < nom.Count - 1; i++)
                {
                    if ((seg[i] < seg[i + 1]) || ((seg[i] == seg[i + 1]) && (mseg[i] < mseg[i + 1])))
                    {
                        auxs = seg[i];
                        auxms = mseg[i];
                        auxn = nom[i];

                        seg[i] = seg[i + 1];
                        mseg[i] = mseg[i + 1];
                        nom[i] = nom[i + 1];

                        seg[i + 1] = auxs;
                        mseg[i + 1] = auxms;
                        nom[i + 1] = auxn;
                    }
                }
            }
            reader.Close();
            StreamWriter writer = new StreamWriter("Puntajes.txt");
            for (i = 0; i < nom.Count; i++)
            {
                writer.WriteLine("{0},{1},{2}", seg[i], mseg[i], nom[i]);
            }
            writer.Close();
        }

        private void UpdateKeys()
        {
            KeyboardState KeyState = Keyboard.GetState();

            Keys[] teclasAnteriores = KeyState.GetPressedKeys();
            Keys[] teclasAntiguas = oldKeyState.GetPressedKeys();
            Boolean encontrado = false;

            for (int i = 0; i < teclasAnteriores.Length; i++)
            {
                encontrado = false;

                // recorre todas las teclas de ambos estados para hacer que se muestre
                // una sola vez
                for (int y = 0; y < teclasAntiguas.Length; y++)
                {
                    if (teclasAnteriores[i] == teclasAntiguas[y])
                    {
                        encontrado = true;
                        break;
                    }
                }
                if (!encontrado)
                    if ((teclasAnteriores[i] != Keys.Escape) && (teclasAnteriores[i] != Keys.Enter))
                        if ((teclasAnteriores[i] != Keys.Right) && (teclasAnteriores[i] != Keys.Left))
                            if ((teclasAnteriores[i] != Keys.Up) && (teclasAnteriores[i] != Keys.Down))
                                if ((teclasAnteriores[i] != Keys.LeftShift) && (teclasAnteriores[i] != Keys.RightShift))
                                    PressKeys(teclasAnteriores[i]);
            }

            oldKeyState = KeyState;

        }

        private void PressKeys(Keys tecla)
        {
            // verificar cada tecla, algunas cambian con los teclados
            if (tecla == Keys.Back)
            {
                // si hay texto, se remueve el último carácter
                if (texto.Length > 0)
                {
                    texto = texto.Remove(texto.Length - 1, 1);
                }
            }
            else if (tecla == Keys.Enter)
            {
                // se agrega una nueva línea
                texto.Append(Environment.NewLine);
            }
            else if (tecla == Keys.Decimal)
            {
                texto.Append(".");
            }
            else
            {
                // las teclas de números y letras no cambian en los teclados
                texto.Append(((char)tecla).ToString().ToLower());
            }
        }

        public void TeclaMenu(KeyboardState keys)
        {
            //Nuevo juego
            if (keys.IsKeyDown(Keys.Enter) && menu_principal.Element_active() == 1 && teclap)
            {
                diff = true;
                sAbrir.Play();
                teclap = false;
                sTick.Play();
            }
            else if (keys.IsKeyUp(Keys.Enter)) teclap = true;
            //Supervivencia
            if (keys.IsKeyDown(Keys.Enter) && menu_principal.Element_active() == 2 && teclap)
            {
                sup = true;
                sAbrir.Play();
                teclap = false;
                sTick.Play();
            }
            else if (keys.IsKeyUp(Keys.Enter)) teclap = true;
            //Tabla de tiempos
            if (keys.IsKeyDown(Keys.Enter) && menu_principal.Element_active() == 3 && teclap)
            {
                punt = true;
                sAbrir.Play();
                teclap = false;
                sTick.Play();
            }
            else if (keys.IsKeyUp(Keys.Enter)) teclap = true;
            //Musica: ON/OFF
            if (keys.IsKeyDown(Keys.Enter) && menu_principal.Element_active() == 4 && teclap)
            {
                if (mus == true)
                {
                    MediaPlayer.Stop();
                    menu_principal.AddElement(3, "Musica: OFF");
                    mus = false;
                }
                else if (mus == false)
                {
                    MediaPlayer.Play(sMusic);
                    MediaPlayer.Volume = 0.4f;
                    MediaPlayer.IsRepeating = true;
                    menu_principal.AddElement(3, "Musica: ON");
                    mus = true;
                }
                teclap = false;
                sTick.Play();
            }
            else if (keys.IsKeyUp(Keys.Enter)) teclap = true;
            //Salir
            if (keys.IsKeyDown(Keys.Enter) && menu_principal.Element_active() == 5) this.Exit();
        }

        public void TeclaSubMenu(KeyboardState keys)
        {
            //Facil
            if (keys.IsKeyDown(Keys.Enter) && menu_dificultad.Element_active() == 1 && teclap)
            {
                facil = true;
                diff = false;
                sAbrir.Play();
                teclap = false;
                sTick.Play();
            }
            else if (keys.IsKeyUp(Keys.Enter)) teclap = true;
            //Medio
            if (keys.IsKeyDown(Keys.Enter) && menu_dificultad.Element_active() == 2 && teclap)
            {
                medio = true;
                diff = false;
                sAbrir.Play();
                teclap = false;
                sTick.Play();
            }
            else if (keys.IsKeyUp(Keys.Enter)) teclap = true;
            //Dificil
            if (keys.IsKeyDown(Keys.Enter) && menu_dificultad.Element_active() == 3 && teclap)
            {
                dificil = true;
                diff = false;
                sAbrir.Play();
                teclap = false;
                sTick.Play();
            }
            else if (keys.IsKeyUp(Keys.Enter)) teclap = true;

        }

        public void TeclaReset(KeyboardState keys)
        {
            if ((keys.IsKeyDown(Keys.Escape)) && teclae)
            {
                teclae = false;
                //Escribe el tiempo en el archivo
                if (sup == true)
                {
                    StreamWriter writer = new StreamWriter("Puntajes.txt", true);
                    writer.WriteLine("{0},{1},{2}", ss, mss, texto.ToString());
                    writer.Close();
                }
                sCerrar.Play();
                OrdenarP();
                //Reinicia las variables
                Initialize();
            }
            else if (keys.IsKeyUp(Keys.Escape)) teclae = true;
        }

        protected override void Initialize()
        {
            //Archivo puntajes
            StreamWriter writer = new StreamWriter("Puntajes.txt", true);
            writer.Close();

            //Menu opciones
            menu_principal = new menuComponent(this, 5, Color.Yellow, Color.Red, Content.Load<SpriteFont>("txtMenu"), 40);
            menu_principal.AddElement(0, "Nuevo Juego");
            menu_principal.AddElement(1, "Supervivencia");
            menu_principal.AddElement(2, "Tiempos: Supervivencia");
            menu_principal.AddElement(3, "Musica: ON");
            menu_principal.AddElement(4, "Salir");

            //Sub Menu dificultades
            menu_dificultad = new menuComponent(this, 3, Color.Yellow, Color.Red, Content.Load<SpriteFont>("txtMenu"), 40);
            menu_dificultad.AddElement(0, "Facil");
            menu_dificultad.AddElement(1, "Medio");
            menu_dificultad.AddElement(2, "Dificil");

            //Sub Menu tiempos
            OrdenarP();
            menu_tiempos = new leerComponent(this, 6, Color.Red, Content.Load<SpriteFont>("txtMenu"), 40);
            while (cont < 6)
            {
                menu_tiempos.AddElement(cont, "");
                cont++;
            }
            cont = 0;
            //Cargar mejores tiempos
            var reader = new StreamReader(File.OpenRead("Puntajes.txt"));
            while ((!reader.EndOfStream) && (cont < 6))
            {
                var line = reader.ReadLine();
                var values = line.Split(',');
                menu_tiempos.AddElement(cont, values[0] + ":" + values[1] + " - " + values[2]);
                cont++;
            }
            reader.Close();

            //Sub Menu nombre
            menu_nombre = new leerComponent(this, 1, Color.Red, Content.Load<SpriteFont>("txtMenu"), 40);
            menu_nombre.AddElement(0, "Ingrese su nombre:");

            //Variables Inicializacion
            facil = medio = dificil = perder = ganar = sup = diff = punt = false;
            timer = 0;
            vidas = 2;
            s = 60;
            ms = 0;
            ss = 0;
            mss = 0;
            bOjo1 = bOjo2 = bOjo3 = new BoundingSphere();

            //Declaracion y pasaje de variables(Posicion,cantidad de fotogramas)
            AnimOjo1 = new PersonajeAnimado(posOjo1, 6, 10);
            AnimOjo2 = new PersonajeAnimado(posOjo2, 6, 10);
            AnimOjo3 = new PersonajeAnimado(posOjo3, 6, 10);
            AnimPj = new PersonajeAnimado(posPj, 16, 30);
            AnimZombie = new PersonajeAnimado(posZombie, 3, 5);

            //Posiciones(X,Y)
            posPj = new Vector2(200, 425);
            posOjo1 = new Vector2(0, 800);
            posOjo2 = new Vector2(0, 800);
            posOjo3 = new Vector2(0, 800);
            posText = new Vector2(600, 0);
            posVida[0] = new Vector2(0, 0);
            posVida[1] = new Vector2(25, 0);
            posVida[2] = new Vector2(50, 0);
            posZombie = new Vector2(900, 432);
            //Colores
            cVida[0] = Color.White;
            cVida[1] = Color.White;
            cVida[2] = Color.White;
            cPj = Color.White;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            //Carga de texturas
            tPj = Content.Load<Texture2D>("NPC_17");
            tOjo = Content.Load<Texture2D>("NPC_4");
            tZombie = Content.Load<Texture2D>("NPC_3");
            tFondo = Content.Load<Texture2D>("Background_175");
            tFondo2 = Content.Load<Texture2D>("Background_14");
            tMenu = Content.Load<Texture2D>("menu");
            tPerder = Content.Load<Texture2D>("Perdiste");
            tGanar = Content.Load<Texture2D>("Ganaste");
            tVida = Content.Load<Texture2D>("Heart");
            //Carga de sonido
            sKill = Content.Load<SoundEffect>("Player_Killed");
            sHit = Content.Load<SoundEffect>("Player_Hit_0");
            sAbrir = Content.Load<SoundEffect>("Menu_Open");
            sCerrar = Content.Load<SoundEffect>("Menu_Close");
            sTick = Content.Load<SoundEffect>("Menu_Tick");
            sMusic = Content.Load<Song>("588099_Soul-Showdown");
            //Carga de Texto
            txtJuego = this.Content.Load<SpriteFont>("txtJuego");
            txtMenu = this.Content.Load<SpriteFont>("txtMenu");
            //Carga de texturas para las animaciones
            AnimOjo1.Load(Content, "NPC_4");
            AnimOjo2.Load(Content, "NPC_4");
            AnimOjo3.Load(Content, "NPC_4");
            AnimPj.Load(Content, "NPC_17");
            AnimZombie.Load(Content, "NPC_3");
            //Reproducir musica
            if (mus == false)
            {
                MediaPlayer.Stop();
                menu_principal.AddElement(3, "Musica: OFF");
            }
            else if (mus == true)
            {
                MediaPlayer.Play(sMusic);
                MediaPlayer.Volume = 0.4f;
                MediaPlayer.IsRepeating = true;
                menu_principal.AddElement(3, "Musica: ON");
            }
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            //Pasar el estado del teclado a los menues, movimiento vertical
            menu_principal.Press_keys(Keyboard.GetState(), sTick);
            menu_dificultad.Press_keys(Keyboard.GetState(), sTick);

            if (Keyboard.GetState().IsKeyDown(Keys.OemPlus)) MediaPlayer.Volume += 0.1f;
            if (Keyboard.GetState().IsKeyDown(Keys.OemMinus)) MediaPlayer.Volume -= 0.1f;

            //Menu
            if ((facil == false) && (medio == false) && (dificil == false) && (sup == false) && (diff == false))
            {
                TeclaMenu(Keyboard.GetState());
            }
            //Sub Menu
            if (diff == true)
            {
                TeclaSubMenu(Keyboard.GetState());
            }
            //Reseteo
            TeclaReset(Keyboard.GetState());
            
            //Sub menu tiempos
            if (punt == true)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Escape)) punt = false;
            }
            //Perder en sup
            if ((perder == true)&&(sup == true))
            {
                UpdateKeys();
                if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                {
                    //Escribe el tiempo en el archivo
                    StreamWriter writer = new StreamWriter("Puntajes.txt", true);
                    writer.WriteLine("{0},{1},{2}", ss, mss, texto.ToString());
                    writer.Close();
                    sCerrar.Play();
                    //Reinicia las variables
                    Initialize();
                }
            }

            //Entrada al juego despues de elejir opcion en el menu
            if (((facil == true) || (medio == true) || (dificil == true) || (sup == true)) && (perder == false) && (ganar == false))
            {
                
                //Aparicion y movimiento random del ojo
                posOjo1.Y += 8;
                if (posOjo1.Y >= 500)
                {
                    posOjo1.X = rnd.Next(1, 700);
                    posOjo1.Y = -200;
                }
                if ((medio == true) || (dificil == true))
                {
                    posOjo2.Y += 7;
                    if (posOjo2.Y >= 500)
                    {
                        posOjo2.X = rnd.Next(1, 700);
                        posOjo2.Y = -200;
                    }
                    if (dificil == true)
                    {
                        posOjo3.Y += 6;
                        if (posOjo3.Y >= 500)
                        {
                            posOjo3.X = rnd.Next(1, 700);
                            posOjo3.Y = -200;
                        }
                    }
                }

                //Movimiento del personaje
                izq = false;
                der = false;
                if ((Keyboard.GetState().IsKeyDown(Keys.Left))&&(Keyboard.GetState().IsKeyUp(Keys.Right)))
                {
                    posPj.X -= 5;
                    izq = true;
                    scrollX --;
                    iPj = SpriteEffects.None;
                }
                if (posPj.X < 0) posPj.X += 765;
                if ((Keyboard.GetState().IsKeyDown(Keys.Right)) && (Keyboard.GetState().IsKeyUp(Keys.Left)))
                {
                    posPj.X += 5;
                    der = true;
                    scrollX ++;
                    iPj = SpriteEffects.FlipHorizontally;
                }
                if (posPj.X >= 765) posPj.X -= 765;

                //Colision
                bOjo1 = new BoundingSphere(new Vector3(new Vector2(tOjo.Width / 2, tOjo.Height / 8) + posOjo1, 0), tOjo.Width / 2);
                if ((medio == true) || (dificil == true))
                {
                    bOjo2 = new BoundingSphere(new Vector3(new Vector2(tOjo.Width / 2, tOjo.Height / 8) + posOjo2, 0), tOjo.Width / 2);
                    if (dificil == true)
                    {
                        bOjo3 = new BoundingSphere(new Vector3(new Vector2(tOjo.Width / 2, tOjo.Height / 8) + posOjo3, 0), tOjo.Width / 2);
                    }
                }
                bPj = new BoundingBox(new Vector3(posPj, 0), new Vector3(posPj.X + tPj.Width, posPj.Y + tPj.Height / 16, 0));
                bZombie = new BoundingBox(new Vector3(posZombie, 0), new Vector3(posZombie.X + tZombie.Width, posZombie.Y + tZombie.Height / 3, 0));

                //Interseccion y movimiento Zombie
                if (bPj.Intersects(bZombie))
                {
                    if (iPj == SpriteEffects.FlipHorizontally) posPj.X -= 6;
                    if (iPj == SpriteEffects.None) posPj.X += 6;   
                }
                if (posZombie.X < posPj.X)
                {
                    posZombie.X += 1;
                    iZombie = SpriteEffects.FlipHorizontally;
                }
                if (posZombie.X > posPj.X)
                {
                    posZombie.X -= 1;
                    iZombie = SpriteEffects.None;
                }

                //Invulnerabilidad
                if (timer > -1) timer--;
                cPj = Color.White;
                if ((timer > 0) && ((int)gameTime.TotalGameTime.Milliseconds % 2 == 0)) cPj = Color.Transparent;
                    
                //Interseccion de los ojos y vidas
                if (((bPj.Intersects(bOjo1))||(bPj.Intersects(bOjo2))||(bPj.Intersects(bOjo3))) && (timer < 0))
                {
                    if (vidas == 2)
                    {
                        cVida[2] = Color.Black;
                        sHit.Play();
                    }
                    if (vidas == 1)
                    {
                        cVida[1] = Color.Black;
                        sHit.Play();
                    }
                    if (vidas == 0)
                    {
                        cVida[0] = Color.Black;
                        sKill.Play();
                        perder = true;
                    }
                    vidas--;
                    timer = 50;
                }

                //Timer
                if (sup == false)
                {
                    TimerN();
                }
                else if (sup == true)
                {
                    TimerS();
                }
                if ((sup == true) && (ss == 30)) medio = true;
                if ((sup == true) && (ss == 60)) dificil = true;

                //Actualizacion de la animacion
                float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
                AnimOjo1.UpdateFrame(elapsed);
                AnimOjo2.UpdateFrame(elapsed);
                AnimOjo3.UpdateFrame(elapsed);
                AnimPj.UpdateFrame(elapsed);
                AnimZombie.UpdateFrame(elapsed);
            }

            base.Update(gameTime);
        }

        public void TimerN()
        {
            if ((ms == 0) && (s > 0))
            {
                ms = 60;
                s--;
            }
            ms--;
            if ((ms == 0) && (s == 0)) ganar = true;
            tiempo = String.Concat("Tiempo: ", s, ":", ms);
        }

        public void TimerS()
        {
            mss++;
            if (mss == 60)
            {
                ss += 1;
                mss = 0;
            }
            tiempo = String.Concat("Tiempo:   ", ss, ":", mss);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Gray);

            //Descomentar para usar modo Reach
            spriteBatch.Begin();
            //Descomentar para usar modo HiDef
            //spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.LinearWrap, null, null);

            //Fondo
            spriteBatch.Draw(tFondo2, Vector2.Zero, new Rectangle(scrollX, 0, tFondo2.Width, tFondo2.Height), Color.White);
            spriteBatch.Draw(tFondo, Vector2.Zero, new Rectangle(scrollX/4, 0, tFondo.Width, tFondo.Height) , Color.White);
            //Zombie
            AnimZombie.DrawFrame(spriteBatch, posZombie, Color.White, iZombie);
            //Ojos
            AnimOjo1.DrawFrame(spriteBatch, posOjo1, Color.White, 0);
            if ((medio == true) || (dificil == true))
            {
                AnimOjo2.DrawFrame(spriteBatch, posOjo2, Color.White, 0);
                if (dificil == true)
                {
                    AnimOjo3.DrawFrame(spriteBatch, posOjo3, Color.White, 0);
                }
            }
            //Dibujando el pj
            if(der == true)
                AnimPj.DrawFrame(spriteBatch, posPj, cPj, iPj);
            if(izq == true)
                AnimPj.DrawFrame(spriteBatch, posPj, cPj, iPj);
            //spriteBatch.Draw(texture, Position, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.FlipVertically, 0);
            if ((izq == false) && (der == false)) spriteBatch.Draw(tPj, posPj, null, cPj, 0, Vector2.Zero, 1, iPj, 0);
            //Vidas
            spriteBatch.Draw(tVida, posVida[0], cVida[0]);
            spriteBatch.Draw(tVida, posVida[1], cVida[1]);
            spriteBatch.Draw(tVida, posVida[2], cVida[2]);
            //Tiempo
            spriteBatch.DrawString(txtJuego, tiempo, posText, Color.White);
            //Pantallas de resultado
            if (ganar == true) spriteBatch.Draw(tGanar, Vector2.Zero, Color.White);
            if ((perder == true) && (sup == false)) spriteBatch.Draw(tPerder, Vector2.Zero, Color.White);
            if ((perder == true) && (sup == true))
            {
                //spriteBatch.Draw(tMenu, Vector2.Zero, Color.White);
                spriteBatch.DrawString(txtMenu, texto.ToString(), new Vector2(400, 300), Color.White);
            }
            spriteBatch.End();
            //Menu
            if ((perder == true) && (sup == true)) menu_nombre.Draw(spriteBatch, tMenu,texto);
            if ((facil == false) && (medio == false) && (dificil == false) && (sup == false))
            {
                graphics.GraphicsDevice.Clear(Color.Black);   
                menu_principal.Draw(spriteBatch,tMenu);
                
                if (diff == true) menu_dificultad.Draw(spriteBatch, tMenu);
                if (punt == true) menu_tiempos.Draw(spriteBatch, tMenu, textof);
            }
            base.Draw(gameTime);
        }
    }
}
