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

namespace SuperSaiyan
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        #region Game Variables
        // Game Variables
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        // Making Goku Object
        Goku goku;

        // Sound effect
        SoundEffect GokuShoot;
        SoundEffect GokuGetHit;
        SoundEffect SupermanShoot;
        SoundEffect SupermanGetHit;
        SoundEffect explosion;
        SoundEffect GokuDead;

        //font
        SpriteFont font;

        // string displayments
        string scoreString;
        string healthString;

        // make the current score;
        int score = 0;

        /// <summary>
        ///Using static to make sure that every objects can have a weapon (either our character or the enemies)
        /// </summary>

        // Goku Weapon Variables
        static Texture2D GokuWeaponSprite;

        // Superfat Weapon Variables
        static Texture2D SupermanWeaponSprite;

        // Create lists of Enemies
        List<Superman> supermen = new List<Superman>();

        // Make a list of weapons
        static List<Weapon> weapons = new List<Weapon>();

        // Make a explosion sprite
        static Texture2D explosionSprite;
        List<Explosion> explosions = new List<Explosion>();

        // Check if lose or not
        static bool IsLost = false;

        #endregion

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            //Making the screen resolution to 800x600
            graphics.PreferredBackBufferHeight = GameVariables.SCREEN_HEIGHT;
            graphics.PreferredBackBufferWidth = GameVariables.SCREEN_WIDTH;
        }


        /// <summary>
        /// Get current Status of the game
        /// </summary>
        public static bool IsLosing
        {
            get { return IsLost; }
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            // Load Music Content
            GokuShoot = Content.Load<SoundEffect>(@"sound\GokuAttack");
            GokuGetHit = Content.Load<SoundEffect>(@"sound\GokuGetHit");
            GokuDead = Content.Load<SoundEffect>(@"sound\GokuDeath");
            explosion = Content.Load<SoundEffect>(@"sound\Explosion");
            SupermanShoot = Content.Load<SoundEffect>(@"sound\SuperfatShot");
            SupermanGetHit = Content.Load<SoundEffect>(@"sound\SuperfatBounce");

            // Load Sprite font
            font = Content.Load<SpriteFont>("Arial20");

            // Load content for Goku
            goku = new Goku(Content, "Goku", graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferWidth - graphics.PreferredBackBufferWidth * 2/3, GokuShoot, GokuGetHit);

            // Load contents for Weapons
            GokuWeaponSprite = Content.Load<Texture2D>("gokuweapon");
            SupermanWeaponSprite = Content.Load<Texture2D>("supermanweapon");


            // Load contents for Explosion animation
            explosionSprite = Content.Load<Texture2D>("explosion"); 

            // Create a new Superman (Spawns)
            for(int i = 0;  i < GameVariables.MAX_NUM_SUPERMEN; i++)
            {
                SpawnSuperman();
            }
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            // Allows user to exit the game by using Escape Button
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                this.Exit();
            }

            // TODO: Add your update logic here

            // Making "GOKU" character following the mouse state
            MouseState mouse = Mouse.GetState();
            goku.Update(gameTime, mouse);

            // Update every objects in the game every milisecond
            foreach (Superman superfat in supermen)
            {
                superfat.Update(gameTime);
            }
            foreach (Weapon weapon in weapons)
            {
                weapon.Update(gameTime);
            }
            foreach (Explosion explosion in explosions)
            {
                explosion.Update(gameTime);
            }

            // Check and Update the collisions between enemies
            for(int i = 0; i < supermen.Count; i++)
            {
                for(int j = i + 1; j < supermen.Count; j++)
                {
                    if(supermen[i].Alive && supermen[j].Alive)
                    {
                        EnemiesCollisionInfor collis = CollisionUtils.CheckCollision(
                                                                        gameTime.ElapsedGameTime.Milliseconds,
                                                                        GameVariables.SCREEN_WIDTH,
                                                                        GameVariables.SCREEN_WIDTH,
                                                                        supermen[i].sVelocity,
                                                                        supermen[i].sRectangle,
                                                                        supermen[j].sVelocity,
                                                                        supermen[j].sRectangle);
                        if( collis!= null)
                        {
                            if (collis.firstOutBounds)
                            {
                                supermen[i].Alive = false;
                            }
                            else
                            {
                                supermen[i].sVelocity = collis.getFirstVelocity;
                                supermen[i].sRectangle = collis.getFirstRectangle;
                            }

                            if (collis.firstOutBounds)
                            {
                                supermen[j].Alive = false;
                            }
                            else
                            {
                                supermen[j].sVelocity = collis.getSecondVelocity;
                                supermen[j].sRectangle = collis.getSecondRectangle;
                            }
                        }
                    }
                }
            }

            // Check and Update what happen if the Goku's fire hits a superman
            foreach (Superman superfat in supermen)
            {
                foreach(Weapon weapon in weapons)
                {
                    if (weapon.Type == WeaponType.GokuWeapon &&
                       weapon.Active && superfat.Alive && superfat.getRectangle.Intersects(weapon.wRectangle))
                    {
                        weapon.Active = false;
                        superfat.Alive = false;
                        score += GameVariables.KILLED_SUPERMAN_AWARD;
                        // Make a new Explosion when the superfat is not Alive
                        Explosion newExplode = new Explosion(explosionSprite, superfat.currentLocation.X,
                                                                              superfat.currentLocation.Y);

                        newExplode.Sound = explosion;
                        // Add the Explosion to the Array List
                        explosions.Add(newExplode);

                        //play sound
                        if (!Game1.IsLosing)
                            newExplode.Sound.Play();
                    }
                }
            }

            // Check if Goku gets hit by touching the superman
            foreach(Superman superfat in supermen)
            {
                if(superfat.Alive && goku.getRectangle.Intersects(superfat.sRectangle))
                {
                    goku.health -= GameVariables.SUPERMAN_DAMAGES_HIMSELF;
                    superfat.Alive = false;
                    score += GameVariables.KILLED_SUPERMAN_AWARD;
                    explosions.Add(new Explosion(explosionSprite, superfat.currentLocation.X, superfat.currentLocation.Y));
                    if (!Game1.IsLosing) {
                        GokuGetHit.Play();
                        explosion.Play();
                    }
                }
            }

            // Check if Goku gets hit by touch the superman's fires
            foreach(Weapon weapon in weapons)
            {
                if(weapon.Type == WeaponType.SupermanWeapon && weapon.wRectangle.Intersects(goku.getRectangle))
                {
                    weapon.Active = false;
                    goku.health -= GameVariables.SUPERMAN_SHOOTING_DAMAGES;
                    if (!Game1.IsLosing)
                        GokuGetHit.Play();
                }
            }

            // Clean out the superman objects which has the Aclive status is false
            for(int i = supermen.Count - 1; i>= 0; i--) // remove from the back of the list
            {
                if (!supermen[i].Alive)
                {
                    supermen.RemoveAt(i);
                }
            }

            while(supermen.Count < GameVariables.MAX_NUM_SUPERMEN)
            {
                SpawnSuperman();
            }

            // Clean out the weapon objects which has the Aclive status is false
            for (int i = weapons.Count - 1; i >= 0; i--) // remove from the back of the list
            {
                if (!weapons[i].Active)
                {
                    weapons.RemoveAt(i);
                }
            }

            // Clean out the explosion when it finishes
            for (int i = explosions.Count - 1; i >= 0; i--) // remove from the back of the list
            {
                if (explosions[i].Finish)
                {
                    explosions.RemoveAt(i);
                }
            }

            // display current score and health
            healthString = GameVariables.HEALTH + goku.health;
            scoreString = GameVariables.SCORE + score;

            // Always check if we are losing or not
            IsLostAlready();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            spriteBatch.Begin();
            // Draw Goku on the Screen
            goku.Draw(spriteBatch);

            // Draw List of Supermen
            foreach(Superman superfat in supermen)
            {
                superfat.Draw(spriteBatch);
            }
            foreach(Weapon weapon in weapons)
            {
                weapon.Draw(spriteBatch);
            }
            foreach(Explosion explode in explosions)
            {
                explode.Draw(spriteBatch);
            }

            spriteBatch.DrawString(font, scoreString, GameVariables.SCORE_DISPLAY_LOCATION, Color.White);
            spriteBatch.DrawString(font, healthString, GameVariables.HEALTH_DISPLAY_LOCATION, Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// Region of Game's actions
        /// </summary>
        /// 
        #region Game Static Functions
        // Random a location for Superman in order to randomly displaying
        private int GetRandomLocation(int min, int max)
        {
            return min + RandomGenerator.Next(max);
        }

        // Random a velocity for Superman
        private float GetRandomSpeed(float maxSpeed)
        {
            return GameVariables.SUPERMAN_MIN_SPEED + RandomGenerator.NextFloat(GameVariables.SUPERMAN_MAX_SPEED);
        }

        // Spawn a new Superman enemy at a random location
        private void SpawnSuperman()
        {
            /*
                Renerate random Location
            */

            // for X location: Spawn in between 100 left to 100 right -> Domain is from 100 to 700 Horizontally
            int x = GetRandomLocation(GameVariables.SPAWN_LOCATION_MIN, graphics.PreferredBackBufferWidth - 2 * GameVariables.SPAWN_LOCATION_MIN);
            // for Y location: Spawn in between 100 top to 100 down -> Domain is from 100 to 500 Vertically
            int y = GetRandomLocation(GameVariables.SPAWN_LOCATION_MIN, graphics.PreferredBackBufferHeight - 2 * GameVariables.SPAWN_LOCATION_MIN);

            /*
                Renerate random Speed for Superman
            */
            float speed = GetRandomSpeed(GameVariables.SUPERMAN_MAX_SPEED);
            float angle = RandomGenerator.NextFloat((float)Math.PI * 2);

            Vector2 velocity = new Vector2((float)(speed * Math.Cos(angle)), (float)(speed*Math.Sin(angle)));

            /*
                Make a new Alive Man of Steel
            */
            Console.WriteLine(x + y);
            Superman newSuperMan = new Superman(Content, "superfat", x, y, velocity, SupermanShoot, SupermanGetHit);

            //make the superman does not spawn right at the collistion
            List<Rectangle> collideRectangles = getListCollideRectangles();
            while (!CollisionUtils.IsCollisionFree(newSuperMan.sRectangle, collideRectangles))
            {
                // for X location: Spawn in between 100 left to 100 right -> Domain is from 100 to 700 Horizontally
                newSuperMan.x = GetRandomLocation(GameVariables.SPAWN_LOCATION_MIN, graphics.PreferredBackBufferWidth - 2 * GameVariables.SPAWN_LOCATION_MIN);
                // for Y location: Spawn in between 100 top to 100 down -> Domain is from 100 to 500 Vertically
                newSuperMan.y = GetRandomLocation(GameVariables.SPAWN_LOCATION_MIN, graphics.PreferredBackBufferHeight - 2 * GameVariables.SPAWN_LOCATION_MIN);
            }

            /*
                Add New Enemies
            */
            supermen.Add(newSuperMan);

        }

        /// <summary>
        /// Chosing what type of weapon to return
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Texture2D GetWeapon(WeaponType type)
        {
            // get the correct weapon for each character
            if(type == WeaponType.GokuWeapon)
            {
                return GokuWeaponSprite;
            } else
            {
                return SupermanWeaponSprite;
            }
        }

        /// <summary>
        /// Add an active weapon to the game
        /// </summary>
        public static void AddWeapon(Weapon currentWeapon)
        {
            weapons.Add(currentWeapon);
        }

        /*
            Get the list of Rectangles of every characters at the current game time
        */
        private List<Rectangle> getListRectangle()
        {   
            // Declaring a new Array list
            List<Rectangle> listRectangles = new List<Rectangle>();

            // Add Goku (The main character) to the list
            listRectangles.Add(goku.getRectangle);

            // Add Enemies Characters to the list (Super fat men)
            foreach(Superman superfat in supermen)
            {
                listRectangles.Add(superfat.getRectangle);
            }

            //return the list
            return listRectangles;
        }

        /// <summary>
        /// list all of the unavailable areas into a list
        /// </summary>
        /// <returns></returns>
        private List<Rectangle> getListCollideRectangles()
        {
            List<Rectangle> collideRectangles = new List<Rectangle>();
            collideRectangles.Add(goku.getRectangle);
            foreach(Superman superfat in supermen)
            {
                collideRectangles.Add(superfat.getRectangle);
            }
            foreach(Weapon weapon in weapons)
            {
                collideRectangles.Add(weapon.wRectangle);
            }
            foreach(Explosion explode in explosions)
            {
                collideRectangles.Add(explode.getRectangle);
            }

            return collideRectangles;
        }

        private void IsLostAlready()
        {
            if(goku.health == 0 && !IsLost)
            {
                IsLost = true;
                GokuDead.Play();
            }
        }
        #endregion
    }
}
