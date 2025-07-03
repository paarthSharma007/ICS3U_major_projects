//Author :Paarth Sharma
//File Name: Game1.cs
//Project Name: MP2
//creation Date: 6-11-24
//Modification Date: 11-11-24
//Description: dodging asteroids and collecting a crate for your ship
using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using GameUtility;
using System.Data;
using System.CodeDom;
using System.Linq.Expressions;
using System.Xml.Schema;
using System.Diagnostics.Eventing.Reader;

namespace MP2
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        static Random random = new Random();

        const int MENU = 0;
        const int PREGAME = 1;
        const int GAME = 2;
        const int ENDGAME = 3;

        //directions 
        const int POS = 1;
        const int NEG = -1;
        const int STOP = 0;

        //max speeds 
        Vector2 maxSpeed = new Vector2(0, 0);

        //time passed
        float timePassed;

        //speed 
        Vector2 speed = new Vector2(0,0);

        //Store and set the initial game state, typically MENU to start
        int gameState = MENU;

        //changing the graphics adaptation 
        int screenWidth;
        int screenHeight;
        int targetFPS = 60;

        //reandom numbers for crate 
        int randomYCrate;
        int randomXCrate;

        //random numbers for asteroid
        int randomXAsteroid1;
        int randomXAsteroid2;
        int randomXAsteroid3;
        int randomXAsteroid4;
        int randomXAsteroid5;

        int randomYAsteroid1;
        int randomYAsteroid2;
        int randomYAsteroid3;
        int randomYAsteroid4;
        int randomYAsteroid5;

        //random size factor for asteroid 
        int RSFAsteroid1;
        int RSFAsteroid2;
        int RSFAsteroid3;
        int RSFAsteroid4;
        int RSFAsteroid5;

        //spritefont creation
        SpriteFont HUDFont;
        SpriteFont titleFont;
        SpriteFont menuFont;

        //Adding object textures
        Texture2D asteroidImg;
        Texture2D playerImg;
        Texture2D crateImg;
        Texture2D backgroundImg;

        //Creating positions for texts in menu state
        Vector2 menuTitleLoc;
        Vector2 menuPlayLoc;
        Vector2 menuExitLoc;

        //creating position for individual text in pregame state 
        Vector2 pregameTextLoc;

        //creating postitions for unique texts in endgame state 
        Vector2 pbTextLoc;
        Vector2 finalScoreTextLoc;
        Vector2 endgameOptionLoc;

        //creating loactions 
        Vector2 backgroundLoc1;
        Vector2 backgroundLoc2;

        //creating backgrounds
        Rectangle backgroundRec1;
        Rectangle backgroundRec2;

        //crearting vector 2 for player location
        Vector2 playerPos;
        Vector2 playerLocInitial;

        //Timer for score
        Timer scoreTimer = new Timer(Timer.INFINITE_TIMER, false);

        //Timer for final score
        Timer finalScoreTimer = new Timer(Timer.INFINITE_TIMER, false);

        //Timer for collision 
        Timer collisionFreeze = new Timer(1500, false);

        //Values for different HUD information 
        int bestScore;
        Vector2 bestScoreLoc;
        int level;
        Vector2 levelLoc;
        int score = 0;
        Vector2 scoreLoc ;
        int finalScore;
        Vector2 finalScoreLoc;

        //creating strings for all the scores
        String finalScoreString = "00000000";
        String bestScoreString = "00000000";
        String scoreString = "00000000";

        //Creating the sprites for the levels 
        Rectangle playerRec;

        Rectangle asteroidRec1 ;
        Rectangle asteroidRec2 ;
        Rectangle asteroidRec3 ;
        Rectangle asteroidRec4 ;
        Rectangle asteroidRec5 ;

        Rectangle crateRec ;

        //Vector positions for asteroids
        Vector2 asteroid1Pos;

        Vector2 asteroid2Pos;

        Vector2 asteroid3Pos;

        Vector2 asteroid4Pos;

        Vector2 asteroid5Pos;

        Vector2 cratePos;

        //setting up keyboard and previous keyboard
        KeyboardState kb ;
        KeyboardState kbPrev ;

        //Adding audio 
        Song backgroundAudio;
        SoundEffect explosion;
        SoundEffect bounce;
        SoundEffect UISelection;
        SoundEffect crateWin;

        //Acceleration 
        const float ACCEL = 0.02f;

        //Friction 
        const float DECCEL = ACCEL * 0.5f;

        //tollarance
        const float TOLLARANCE = DECCEL * 0.9f;

        //scrool speed 
        float scroolSpeed;
        float scroolSpeedMax = 5 * 60f;

        //Animation 
        Vector2 explosionAnimPos;
        Texture2D explosionImg;
        Animation explosionAnim;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            IsMouseVisible = true;

            graphics.PreferredBackBufferWidth = 400;
            graphics.PreferredBackBufferHeight = 700;

            graphics.SynchronizeWithVerticalRetrace = false;
            IsFixedTimeStep = true;
            TargetElapsedTime = TimeSpan.FromMilliseconds(1000.0f / targetFPS);

            graphics.ApplyChanges();
            
            screenWidth = graphics.GraphicsDevice.Viewport.Width;
            screenHeight = graphics.GraphicsDevice.Viewport.Height;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            //Loading Audio
            backgroundAudio = Content.Load<Song>("GameMedia/Audio/Music/Tetris");
            explosion = Content.Load<SoundEffect>("GameMedia/Audio/Sounds/Explosion");
            bounce = Content.Load<SoundEffect>("GameMedia/Audio/Sounds/Bounce");
            UISelection = Content.Load<SoundEffect>("GameMedia/Audio/Sounds/UISelection");
            crateWin = Content.Load<SoundEffect>("GameMedia/Audio/Sounds/CrateWin");

            //Loading fonts
            HUDFont = Content.Load<SpriteFont>("GameMedia/Fonts/HUDFont");
            menuFont = Content.Load<SpriteFont>("GameMedia/Fonts/MenuFont");
            titleFont = Content.Load<SpriteFont>("GameMedia/Fonts/TitleFont");

            //Loading textures for sprites
            backgroundImg = Content.Load<Texture2D>("GameMedia/Images/Backgrounds/space");
            asteroidImg = Content.Load<Texture2D>("GameMedia/Images/Sprites/asteroid");
            crateImg = Content.Load<Texture2D>("GameMedia/Images/Sprites/crate");
            playerImg = Content.Load<Texture2D>("GameMedia/Images/Sprites/ship");
            explosionImg = Content.Load<Texture2D>("GameMedia/Images/Sprites/explode");

            //max speed
            maxSpeed.X = 20f;
            maxSpeed.Y = 20f;

            //Locations for menu elecments 
            menuTitleLoc = new Vector2(20,80);
            menuExitLoc = new Vector2((screenWidth / 2) - 40, 180);
            menuPlayLoc = new Vector2((screenWidth / 2) - 40, 150);
            backgroundLoc1 = new Vector2(0, 0);
            backgroundLoc2 = new Vector2(0, 0);

            //Locations for end game elements 
            pbTextLoc = new Vector2((screenWidth/2) - 150, 300);
            finalScoreTextLoc = new Vector2((screenWidth/2) - 60, 400); 
            finalScoreLoc = new Vector2((screenWidth/2) - 60, 450);
            endgameOptionLoc = new Vector2((screenWidth / 2) - 150, 550);

            //location for pregame text
            pregameTextLoc = new Vector2((screenWidth / 2) - 110, screenHeight - 250);

            //Background boundary
            backgroundRec1 = new Rectangle(0,0, screenWidth, screenHeight);
            backgroundRec2 = new Rectangle(0,-screenHeight, screenWidth, screenHeight);

            //loactions for HUD elements 
            bestScoreLoc = new Vector2((screenWidth / 2) -70 , 0);
            scoreLoc = new Vector2(screenWidth - 90, 0 );

            //level location
            levelLoc = new Vector2((screenWidth / 2) - 45, screenHeight - 300);

            //Sprites 
            playerRec = new Rectangle(0,0, 50 , 50);
            asteroidRec1 = new Rectangle(1, 1, 1, 1);
            asteroidRec2 = new Rectangle(1, 1, 1, 1);
            asteroidRec3 = new Rectangle(1, 1, 1, 1);
            asteroidRec4 = new Rectangle(1, 1, 1, 1);
            asteroidRec5 = new Rectangle(1, 1, 1, 1);
            crateRec = new Rectangle(1, 1, 40, 40);

            //Playing song
            MediaPlayer.Volume = 0.1f;
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(backgroundAudio);

            //playing sound effects
            SoundEffect.MasterVolume = 0.6f;

            //Animation
            explosionAnimPos = new Vector2(0, 0);
            explosionAnim = new Animation(explosionImg, 4, 4, 24, 0, 0, 0, 1500, explosionAnimPos, false);

        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            
            kbPrev = kb;
            kb = Keyboard.GetState();

            timePassed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            switch (gameState)
            {
                case MENU:
                    //Get and implement menu interactions, e.g. when the user clicks a Play button set gameState = GAMEPLAY
                    
                    if (kbPrev.IsKeyDown(Keys.P))
                    {
                        UISelection.CreateInstance().Play();
                        gameState = PREGAME;
                        level = 1;
                    }
                    if (kbPrev.IsKeyDown(Keys.X))
                    {
                        UISelection.CreateInstance().Play();
                        Exit();
                    }

                    break;
                case PREGAME:
                    //Get and apply changes to game settings
                    if (kbPrev.IsKeyDown(Keys.Space))
                    {
                        UISelection.CreateInstance().Play();
                        gameState = GAME;
                        scoreTimer.Activate();
                        
                        //resetting HUD elements position
                        levelLoc = new Vector2(0, 0);

                        LevelSetup();
                    }

                    break;
                case GAME:
                    //Updating the timers
                    scoreTimer.Update(gameTime);
                    finalScoreTimer.Update(gameTime);
                    collisionFreeze.Update(gameTime);
                    explosionAnim.Update(gameTime);

                    score = scoreTimer.GetTimePassedInt();
                    scoreString = Convert.ToString(score);

                    PlayerControl();
                    ResetPlayer();

                    if(level == 1)
                    {
                        finalScoreTimer.Activate();
                    }

                    //code for resetting to PreGame if level is <5 or going to EndGame if level==5
                    if (crateRec.Intersects(playerRec) && level < 5 )
                    {
                        crateWin.CreateInstance().Play();
                        gameState = PREGAME;
                        level += 1 ;
                        scoreTimer.Deactivate();
                        SpeedReset();
                    }
                    else if (crateRec.Intersects(playerRec) && level == 5 )
                    {
                        SpeedReset();
                        crateWin.CreateInstance().Play();
                        finalScoreTimer.Deactivate();
                        gameState = ENDGAME;
                    }

                    
                    break;
                case ENDGAME:
                    //set final score = to time in total for 5 levels 

                    finalScore = finalScoreTimer.GetTimePassedInt();
                    finalScoreString = Convert.ToString(finalScore);

                    if (finalScore > bestScore)
                    {
                        bestScore = finalScore;
                        bestScoreString = Convert.ToString(bestScore);
                    }
                    if (bestScore > finalScore)
                    {
                        bestScore = finalScore;
                        bestScoreString = Convert.ToString(bestScore);
                    }

                    if (kbPrev.IsKeyDown(Keys.Escape)) 
                    {
                        UISelection.CreateInstance().Play();
                        gameState = MENU;
                    }

                    break;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            ScrollingScreen((float)gameTime.ElapsedGameTime.TotalSeconds);

            switch (gameState)
            {
                case MENU:
                    //Draw the possible menu options

                    spriteBatch.Draw(backgroundImg, backgroundLoc1,backgroundRec1,Color.White);
                    spriteBatch.Draw(backgroundImg, backgroundLoc2,backgroundRec2,Color.White);
                    spriteBatch.DrawString(titleFont, "SPACE GEOMETRY!",menuTitleLoc, Color.CornflowerBlue);
                    spriteBatch.DrawString(menuFont, "Play(P)", menuPlayLoc, Color.Yellow);
                    spriteBatch.DrawString(menuFont, "Exit(X)", menuExitLoc, Color.Yellow);     
                    break;
                case PREGAME:
                    

                    //drawing the background
                    spriteBatch.Draw(backgroundImg, backgroundLoc1, backgroundRec1, Color.White);
                    spriteBatch.Draw(backgroundImg, backgroundLoc2, backgroundRec2, Color.White);
                     
                    //HUD elements 
                    if (bestScore == 0)
                    {
                        spriteBatch.DrawString(HUDFont, "Best:--" , bestScoreLoc, Color.Yellow);
                    }
                    else 
                    {
                        spriteBatch.DrawString(HUDFont, "Best: " + bestScoreString.PadLeft(8,'0'), bestScoreLoc, Color.Yellow);
                    }

                    spriteBatch.DrawString(HUDFont, "00000000", scoreLoc, Color.White);
                    spriteBatch.DrawString(HUDFont, "LEVEL "+ level,levelLoc, Color.Red );
                    spriteBatch.DrawString(menuFont, "Space to begin level" , pregameTextLoc, Color.Yellow);

                    break;
                case GAME:

                    //drawing the background
                    spriteBatch.Draw(backgroundImg, backgroundLoc1, backgroundRec1, Color.White);
                    spriteBatch.Draw(backgroundImg, backgroundLoc2, backgroundRec2, Color.White);

                    //Displaying best score
                    if (bestScore == 0)
                    {
                        spriteBatch.DrawString(HUDFont, "Best:--", bestScoreLoc, Color.Yellow);
                    }
                    else
                    {
                        spriteBatch.DrawString(HUDFont, "Best: " + bestScoreString.PadLeft(8,'0') , bestScoreLoc, Color.Yellow);
                    }

                    //HUD elements 
                    spriteBatch.DrawString(HUDFont, scoreString.PadLeft(8,'0'), scoreLoc, Color.White);
                    spriteBatch.DrawString(HUDFont, "LEVEL " + level, levelLoc, Color.Red);

                    LevelDisp(); 

                    break;
                case ENDGAME:

                    //drawing the background
                    spriteBatch.Draw(backgroundImg, backgroundLoc1, backgroundRec1, Color.White);
                    spriteBatch.Draw(backgroundImg, backgroundLoc2, backgroundRec2, Color.White);

                    //Loading HUD elements for end game screen

                    spriteBatch.DrawString(titleFont, "PERSONAL BEST!", pbTextLoc, Color.CornflowerBlue);
                    spriteBatch.DrawString(menuFont, "Final Score", finalScoreTextLoc, Color.Green);
                    spriteBatch.DrawString(menuFont, finalScoreString.PadLeft(8,'0') , finalScoreLoc, Color.Green );
                    spriteBatch.DrawString(menuFont, "Press Esc to return to menu", endgameOptionLoc, Color.Yellow);

                    break;
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
     //Define Subprogrammes here

        //setting up level sprites
        private void LevelSetup()
        {
            playerPos.X = (screenWidth / 2) - (playerImg.Width / 2);
            playerPos.Y = screenHeight - 80;

            playerLocInitial.X = playerPos.X; 
            playerLocInitial.Y = playerPos.Y;

            playerRec.X = (int)playerPos.X;
            playerRec.Y = (int)playerPos.Y;

            //random postions for crate 
            randomYCrate = random.Next(20, 101 - crateRec.Height);
            randomXCrate = random.Next(0, 400 - crateRec.Width);

            cratePos.X = randomXCrate;
            cratePos.Y = randomYCrate;

            crateRec.X = (int)cratePos.X;
            crateRec.Y = (int)cratePos.Y;

            RSFAsteroid1 = random.Next(30, 101);
            RSFAsteroid2 = random.Next(30, 101);
            RSFAsteroid3 = random.Next(30, 101);
            RSFAsteroid4 = random.Next(30, 101);
            RSFAsteroid5 = random.Next(30, 101);

            //randomizing asteroid width with a factor
            asteroidRec1.Width = RSFAsteroid1;
            asteroidRec2.Width = RSFAsteroid2;
            asteroidRec3.Width = RSFAsteroid3;
            asteroidRec4.Width = RSFAsteroid4;
            asteroidRec5.Width = RSFAsteroid5;

            //randomizing asteroid height with the same factor
            asteroidRec1.Height = RSFAsteroid1;
            asteroidRec2.Height = RSFAsteroid2;
            asteroidRec3.Height = RSFAsteroid3;
            asteroidRec4.Height = RSFAsteroid4;
            asteroidRec5.Height = RSFAsteroid5;

            //randomizing asteroid X
            randomXAsteroid1 = random.Next(0, screenWidth - asteroidRec1.Width);
            randomXAsteroid2 = random.Next(0, screenWidth - asteroidRec2.Width);
            randomXAsteroid3 = random.Next(0, screenWidth - asteroidRec3.Width);
            randomXAsteroid4 = random.Next(0, screenWidth - asteroidRec4.Width);
            randomXAsteroid5 = random.Next(0, screenWidth - asteroidRec5.Width);

            //randomizing asteroid Y
            randomYAsteroid1 = random.Next(100, screenHeight - asteroidRec1.Height - 99);
            randomYAsteroid2 = random.Next(100, screenHeight - asteroidRec2.Height - 99);
            randomYAsteroid3 = random.Next(100, screenHeight - asteroidRec3.Height - 99);
            randomYAsteroid4 = random.Next(100, screenHeight - asteroidRec4.Height - 99);
            randomYAsteroid5 = random.Next(100, screenHeight - asteroidRec5.Height - 99);

            //assigning these random numbers to positions 
            asteroid1Pos.X = randomXAsteroid1;
            asteroid2Pos.X = randomXAsteroid2;
            asteroid3Pos.X = randomXAsteroid3;
            asteroid4Pos.X = randomXAsteroid4;
            asteroid5Pos.X = randomXAsteroid5;

            asteroid1Pos.Y = randomYAsteroid1;
            asteroid2Pos.Y = randomYAsteroid2;
            asteroid3Pos.Y = randomYAsteroid3;
            asteroid4Pos.Y = randomYAsteroid4;
            asteroid5Pos.Y = randomYAsteroid5;

            //assigning these position to the rectangle 
            asteroidRec1.X = (int)asteroid1Pos.X;
            asteroidRec2.X = (int)asteroid2Pos.X;
            asteroidRec3.X = (int)asteroid3Pos.X;
            asteroidRec4.X = (int)asteroid4Pos.X;
            asteroidRec5.X = (int)asteroid5Pos.X;
                            
            asteroidRec1.Y = (int)asteroid1Pos.Y;
            asteroidRec2.Y = (int)asteroid2Pos.Y;
            asteroidRec3.Y = (int)asteroid3Pos.Y;
            asteroidRec4.Y = (int)asteroid4Pos.Y;
            asteroidRec5.Y = (int)asteroid5Pos.Y;
        }

        //displaying sprites 
        private void LevelDisp()
        {
            if (collisionFreeze.IsInactive())
            {
                spriteBatch.Draw(playerImg, playerRec, Color.White);
            }
            else
            {
                spriteBatch.Draw(playerImg, playerRec, Color.OrangeRed);
            }

            spriteBatch.Draw(asteroidImg, asteroidRec1, Color.White);
            spriteBatch.Draw(asteroidImg, asteroidRec2, Color.White);
            spriteBatch.Draw(asteroidImg, asteroidRec3, Color.White);
            spriteBatch.Draw(asteroidImg, asteroidRec4, Color.White);
            spriteBatch.Draw(asteroidImg, asteroidRec5, Color.White);
            spriteBatch.Draw(crateImg, crateRec, Color.White);
            if (collisionFreeze.IsActive())
            {
                explosionAnim.Draw(spriteBatch, Color.White, Animation.FLIP_NONE);
            }
        }
        
        //updating the player location
        private void ResetPlayer()
        {
            //for boundries 
            if (WallCollision())
            {
                bounce.CreateInstance().Play();
            }

            if (AccurateCollision(playerRec, asteroidRec1) || AccurateCollision(playerRec, asteroidRec2) || AccurateCollision(playerRec, asteroidRec3) || AccurateCollision(playerRec, asteroidRec4) || AccurateCollision(playerRec, asteroidRec5)) 
            {
                collisionFreeze.Activate();
                explosionAnim.TranslateTo(playerRec.X, playerRec.Y);
                explosionAnim.Activate(true);
                explosion.CreateInstance().Play();
                SpeedReset();
                playerPos.X = (int)playerLocInitial.X;
                playerPos.Y = (int)playerLocInitial.Y;
                playerRec.X = (int)playerLocInitial.X;
                playerRec.Y = (int)playerLocInitial.Y;
            }
            if (collisionFreeze.IsFinished())
            {
                collisionFreeze.Deactivate();
                explosionAnim.Deactivate();
            }
        }

        //giving control ability to the user 
        private void PlayerControl()
        {       
            if (collisionFreeze.IsInactive())
            {

                if (kb.IsKeyDown(Keys.W))
                {
                    speed.Y -= ACCEL;
                    speed.Y = MathHelper.Clamp(speed.Y, -maxSpeed.Y , maxSpeed.Y );
                }
                else if (kb.IsKeyDown(Keys.S))
                {
                    speed.Y += ACCEL;
                    speed.Y = MathHelper.Clamp(speed.Y, -maxSpeed.Y, maxSpeed.Y);
                }
                else if (kb.IsKeyDown(Keys.D))
                {
                    speed.X += ACCEL;
                    speed.X = MathHelper.Clamp(speed.X, -maxSpeed.X, maxSpeed.X);
                }
                else if (kb.IsKeyDown(Keys.A))
                {
                    speed.X -= ACCEL;
                    speed.X = MathHelper.Clamp(speed.X, -maxSpeed.X, maxSpeed.X);
                }
                else
                {
                    //speed.X += -Math.Sign(speed.X) * DECCEL;
                    //speed.Y += -Math.Sign(speed.Y) * DECCEL;

                    if(Math.Abs(speed.X) <= TOLLARANCE)
                    {
                        speed.X = 0;
                    }
                    if (Math.Abs(speed.Y) <= TOLLARANCE)
                    {
                        speed.Y = 0;
                    }
                }
                playerPos.X += speed.X;
                playerPos.Y += speed.Y;

                playerRec.X = (int)playerPos.X;
                playerRec.Y = (int)playerPos.Y;
            }
        }

        //speed reset for player if it collides anywhere except crate 
        private void SpeedReset()
        {
            speed.X = 0;
            speed.Y = 0;
        }

        //using accurate collision
        private bool AccurateCollision(Rectangle playerRecInput , Rectangle asteroidRec)
        {
            bool collision = false;
            
            double playerRadius = (playerRecInput.Width)/2;
            double asteroidRadius = (asteroidRec.Width) / 2;
            
            Vector2 playerCenter = new Vector2((int)(playerRecInput.X + playerRadius), (int)(playerRecInput.Y + playerRadius));
            Vector2 asteroidCenter = new Vector2((int)(asteroidRec.X + asteroidRadius), (int)(asteroidRec.Y + asteroidRadius));

            double centerDistance = Vector2.Distance(playerCenter, asteroidCenter);

            if(centerDistance <=  (asteroidRadius + playerRadius))
            {
                collision = true;
            }
            return collision;
        }

        //wall Rebounding and collision
        private bool WallCollision()
        {
            bool collision = false;
            if(playerPos.X <= 0 || playerPos.X >= (screenWidth - playerRec.Width))
            {
                collision = true;
                speed.X = -(speed.X);
            }
            else if(playerPos.Y <= 0 || playerPos.Y >= (screenHeight - playerRec.Height))
            {
                collision = true;
                speed.Y = -(speed.Y);
            }
            return collision;
        }

        //scrolling screens 
        private void ScrollingScreen(float deltaTime)
        {
            scroolSpeed = scroolSpeedMax * deltaTime;
            backgroundLoc1.Y += scroolSpeed;
            backgroundLoc2.Y += scroolSpeed;
            
            if(backgroundLoc1.Y >= screenHeight)
            {
                backgroundLoc1.Y = 0;
            }
            else if (backgroundLoc2.Y >= 0)
            {
                backgroundLoc2.Y = -screenHeight;
            }
            backgroundRec1.Y = (int)backgroundLoc1.Y;
            backgroundRec2.Y = (int)backgroundLoc2.Y;
        }

    }
}
