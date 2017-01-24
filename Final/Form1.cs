/// created by : Michael Peterman
/// date       : January 24, 2017
/// description: A simple game which is created as if it were a room in the NES game The Legend of Zelda.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Media;

namespace Final
{
    public partial class Form1 : Form
    {
        #region image declarations and stuff
        Image fullHeart = Properties.Resources.full_heart;
        Image emptyHeart = Properties.Resources.empty_heart;
        Image celebration = Properties.Resources.celebration;

        Image upStand = Properties.Resources.up_stand;
        Image rightStand = Properties.Resources.right_stand;
        Image leftStand = Properties.Resources.left_stand;
        Image downStand = Properties.Resources.down_stand;

        Image upStep = Properties.Resources.up;
        Image rightStep = Properties.Resources.right;
        Image leftStep = Properties.Resources.left;
        Image downStep = Properties.Resources.down;

        Image upAttack = Properties.Resources.up_attack;
        Image rightAttack = Properties.Resources.right_attack;
        Image leftAttack = Properties.Resources.left_attack;
        Image downAttack = Properties.Resources.down_attack;

        Image upDN = Properties.Resources.DN_up_stand;
        Image up2DN = Properties.Resources.DN_up_step;
        Image rightDN = Properties.Resources.DN_right_stand;
        Image right2DN = Properties.Resources.DN_right_step;
        Image leftDN = Properties.Resources.DN_left_stand;
        Image left2DN = Properties.Resources.DN_left_step;
        Image downDN = Properties.Resources.DN_down_stand;
        Image down2DN = Properties.Resources.DN_down_step;
        #endregion

        //initial starting values for Hero character and other stuff
        int xHero = 336;
        int yHero = 216;
        int speedHero = 5;
        int widthHero = 40;
        int heightHero = 40;
        int lifeHero = 3;
        int damageTimer;
        bool heroHurt = false;
        int opacityVal = 0;

        SoundPlayer labyrinth = new SoundPlayer(Properties.Resources._04_labyrinth);
        SoundPlayer win = new SoundPlayer(Properties.Resources._06_triforce);
        SoundPlayer lose = new SoundPlayer(Properties.Resources._07_game_over);

        #region initial starting values and other variables for Darknuts
        //universal
        int speedDN = 3;
        int widthDN = 48;
        int heightDN = 48;
        int timerDN;
        int countDN = 4;

        //top left (1)
        int xDN = 72;
        int yDN = 72;
        int stepCountDN = 0;
        string directionDN = "down";
        int directionDNHold;
        int distDNHold;
        bool legalDN = true;
        bool DN180 = false;
        bool DN = true;

        //top right (2)
        int xDN2 = 599;
        int yDN2 = 72;
        int stepCountDN2 = 0;
        string directionDN2 = "down";
        int directionDN2Hold;
        int distDN2Hold;
        bool legalDN2 = true;
        bool DN2180 = false;
        bool DN2 = true;

        //bottom left (3)
        int xDN3 = 72;
        int yDN3 = 359;
        int stepCountDN3 = 0;
        string directionDN3 = "down";
        int directionDN3Hold;
        int distDN3Hold;
        bool legalDN3 = true;
        bool DN3180 = false;
        bool DN3 = true;

        //bottom right (4)
        int xDN4 = 599;
        int yDN4 = 359;
        int stepCountDN4 = 0;
        string directionDN4 = "down";
        int directionDN4Hold;
        int distDN4Hold;
        bool legalDN4 = true;
        bool DN4180 = false;
        bool DN4 = true;
        #endregion

        #region various variables for varying variable things
        bool moving = false;
        bool legal = true;
        bool movementPaused = false;
        bool attacking = false;
        string direction = "down";
        int timer;
        int attackTimer;
        Random rnd = new Random();
        bool soundPlayed = false;
        #endregion

        //sets rectangle around the obstacles
        Rectangle leftObstacle = new Rectangle(168, 168, 96, 144);
        Rectangle rightObstacle = new Rectangle(456, 168, 96, 144);
        Rectangle nWall = new Rectangle(0, 40, 720, 31);
        Rectangle eWall = new Rectangle(648, 0, 31, 480);
        Rectangle sWall = new Rectangle(0, 408, 720, 31);
        Rectangle wWall = new Rectangle(40, 0, 31, 480);
        Rectangle sword;

        //determines whether a key is being pressed or not - DO NOT CHANGE
        Boolean leftArrowDown, downArrowDown, rightArrowDown, upArrowDown, spaceDown;

        //create graphic objects
        SolidBrush drawBrush = new SolidBrush(Color.Black);

        public Form1()
        {
            InitializeComponent();

            //start the timer when the program starts
            gameTimer.Enabled = true;
            gameTimer.Start();
            genDNPath(1);
            genDNPath(2);
            genDNPath(3);
            genDNPath(4);

            labyrinth.PlayLooping();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            //check to see if a key is pressed and set is KeyDown value to true if it has
            switch (e.KeyCode)
            {
                case Keys.Left:
                    if (!attacking)
                    {
                        leftArrowDown = true;
                        moving = true;
                        rightArrowDown = false;
                        downArrowDown = false;
                        upArrowDown = false;
                    }
                    break;
                case Keys.Down:
                    if (!attacking)
                    {
                        downArrowDown = true;
                        moving = true;
                        rightArrowDown = false;
                        upArrowDown = false;
                        leftArrowDown = false;
                    }
                    break;
                case Keys.Right:
                    if (!attacking)
                    {
                        rightArrowDown = true;
                        moving = true;
                        downArrowDown = false;
                        upArrowDown = false;
                        leftArrowDown = false;
                    }
                    break;
                case Keys.Up:
                    if (!attacking)
                    {
                        upArrowDown = true;
                        moving = true;
                        rightArrowDown = false;
                        downArrowDown = false;
                        leftArrowDown = false;
                    }
                    break;
                case Keys.Space:
                    spaceDown = true;
                    if (moving)
                    {
                        moving = false;
                        movementPaused = true;
                    }
                    break;
                default:
                    break;
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            //check to see if a key has been released and set its KeyDown value to false if it has
            switch (e.KeyCode)
            {
                case Keys.Left:
                    leftArrowDown = false;
                    if (!leftArrowDown &&
                        !rightArrowDown &&
                        !upArrowDown &&
                        !downArrowDown)
                    {
                        moving = false;
                        timer = 0;
                    }
                    break;
                case Keys.Down:
                    downArrowDown = false;
                    if (!leftArrowDown &&
                        !rightArrowDown &&
                        !upArrowDown &&
                        !downArrowDown)
                    {
                        moving = false;
                        timer = 0;
                    }
                    break;
                case Keys.Right:
                    rightArrowDown = false;
                    if (!leftArrowDown &&
                        !rightArrowDown &&
                        !upArrowDown &&
                        !downArrowDown)
                    {
                        moving = false;
                        timer = 0;
                    }
                    break;
                case Keys.Up:
                    upArrowDown = false;
                    if (!leftArrowDown &&
                        !rightArrowDown &&
                        !upArrowDown &&
                        !downArrowDown)
                    {
                        moving = false;
                        timer = 0;
                    }
                    break;
                case Keys.Space:
                    spaceDown = false;
                    break;
                default:
                    break;
            }
        }

        public void checkCollision(Rectangle player)
        {
            if (player.IntersectsWith(nWall)
                || player.IntersectsWith(eWall)
                || player.IntersectsWith(sWall)
                || player.IntersectsWith(wWall)
                || player.IntersectsWith(leftObstacle)
                || player.IntersectsWith(rightObstacle))
                
            {
                legal = false;
            }
            else
            {
                legal = true;
            }


        }

        public void checkDNCollision(Rectangle darknut, Rectangle darknut2, Rectangle darknut3, Rectangle darknut4, int DN)
        {
            if(DN == 1)
            {
                if (darknut.IntersectsWith(nWall)
                || darknut.IntersectsWith(eWall)
                || darknut.IntersectsWith(sWall)
                || darknut.IntersectsWith(wWall)
                || darknut.IntersectsWith(leftObstacle)
                || darknut.IntersectsWith(rightObstacle)
                || darknut.IntersectsWith(darknut2)
                || darknut.IntersectsWith(darknut3)
                || darknut.IntersectsWith(darknut4))

                {
                    legalDN = false;
                }
                else
                {
                    legalDN = true;
                }
            }
            else if (DN == 2)
            {
                if (darknut.IntersectsWith(nWall)
                || darknut.IntersectsWith(eWall)
                || darknut.IntersectsWith(sWall)
                || darknut.IntersectsWith(wWall)
                || darknut.IntersectsWith(leftObstacle)
                || darknut.IntersectsWith(rightObstacle)
                || darknut.IntersectsWith(darknut2)
                || darknut.IntersectsWith(darknut3)
                || darknut.IntersectsWith(darknut4))

                {
                    legalDN2 = false;
                }
                else
                {
                    legalDN2 = true;
                }
            }
            else if (DN == 3)
            {
                if (darknut.IntersectsWith(nWall)
                || darknut.IntersectsWith(eWall)
                || darknut.IntersectsWith(sWall)
                || darknut.IntersectsWith(wWall)
                || darknut.IntersectsWith(leftObstacle)
                || darknut.IntersectsWith(rightObstacle)
                || darknut.IntersectsWith(darknut2)
                || darknut.IntersectsWith(darknut3)
                || darknut.IntersectsWith(darknut4))

                {
                    legalDN3 = false;
                }
                else
                {
                    legalDN3 = true;
                }
            }
            else if (DN == 4)
            {
                if (darknut.IntersectsWith(nWall)
                || darknut.IntersectsWith(eWall)
                || darknut.IntersectsWith(sWall)
                || darknut.IntersectsWith(wWall)
                || darknut.IntersectsWith(leftObstacle)
                || darknut.IntersectsWith(rightObstacle)
                || darknut.IntersectsWith(darknut2)
                || darknut.IntersectsWith(darknut3)
                || darknut.IntersectsWith(darknut4))

                {
                    legalDN4 = false;
                }
                else
                {
                    legalDN4 = true;
                }
            }
        }

        public void genDNPath(int DN)
        {
            //get direction
            #region darknut 1
            if(DN == 1)
            {
                if (DN180)
                {
                    directionDNHold = rnd.Next(1, 3);
                }
                else
                {
                    directionDNHold = rnd.Next(1, 4);
                }

                if (directionDNHold == 3)
                {
                    DN180 = true;
                }
                else
                {
                    DN180 = false;
                }

                if (directionDN == "up")
                {
                    switch (directionDNHold)
                    {
                        case 1:
                            directionDN = "right";
                            break;
                        case 2:
                            directionDN = "left";
                            break;
                        case 3:
                            directionDN = "down";
                            break;
                    }
                }
                else if (directionDN == "left")
                {
                    switch (directionDNHold)
                    {
                        case 1:
                            directionDN = "down";
                            break;
                        case 2:
                            directionDN = "up";
                            break;
                        case 3:
                            directionDN = "right";
                            break;
                    }
                }
                else if (directionDN == "right")
                {
                    switch (directionDNHold)
                    {
                        case 1:
                            directionDN = "down";
                            break;
                        case 2:
                            directionDN = "up";
                            break;
                        case 3:
                            directionDN = "left";
                            break;
                    }
                }
                else if (directionDN == "down")
                {
                    switch (directionDNHold)
                    {
                        case 1:
                            directionDN = "right";
                            break;
                        case 2:
                            directionDN = "left";
                            break;
                        case 3:
                            directionDN = "up";
                            break;
                    }
                }
            }
            #endregion
            #region darknut 2
            else if (DN == 2)
            {
                if (DN2180)
                {
                    directionDN2Hold = rnd.Next(1, 3);
                }
                else
                {
                    directionDN2Hold = rnd.Next(1, 4);
                }

                if (directionDN2Hold == 3)
                {
                    DN2180 = true;
                }
                else
                {
                    DN2180 = false;
                }

                if (directionDN2 == "up")
                {
                    switch (directionDN2Hold)
                    {
                        case 1:
                            directionDN2 = "right";
                            break;
                        case 2:
                            directionDN2 = "left";
                            break;
                        case 3:
                            directionDN2 = "down";
                            break;
                    }
                }
                else if (directionDN2 == "left")
                {
                    switch (directionDN2Hold)
                    {
                        case 1:
                            directionDN2 = "down";
                            break;
                        case 2:
                            directionDN2 = "up";
                            break;
                        case 3:
                            directionDN2 = "right";
                            break;
                    }
                }
                else if (directionDN2 == "right")
                {
                    switch (directionDN2Hold)
                    {
                        case 1:
                            directionDN2 = "down";
                            break;
                        case 2:
                            directionDN2 = "up";
                            break;
                        case 3:
                            directionDN2 = "left";
                            break;
                    }
                }
                else if (directionDN2 == "down")
                {
                    switch (directionDN2Hold)
                    {
                        case 1:
                            directionDN2 = "right";
                            break;
                        case 2:
                            directionDN2 = "left";
                            break;
                        case 3:
                            directionDN2 = "up";
                            break;
                    }
                }
            }
            #endregion
            #region darknut 3
            else if (DN == 3)
            {
                if (DN3180)
                {
                    directionDN3Hold = rnd.Next(1, 3);
                }
                else
                {
                    directionDN3Hold = rnd.Next(1, 4);
                }

                if (directionDN3Hold == 3)
                {
                    DN3180 = true;
                }
                else
                {
                    DN3180 = false;
                }

                if (directionDN3 == "up")
                {
                    switch (directionDN3Hold)
                    {
                        case 1:
                            directionDN3 = "right";
                            break;
                        case 2:
                            directionDN3 = "left";
                            break;
                        case 3:
                            directionDN3 = "down";
                            break;
                    }
                }
                else if (directionDN3 == "left")
                {
                    switch (directionDN3Hold)
                    {
                        case 1:
                            directionDN3 = "down";
                            break;
                        case 2:
                            directionDN3 = "up";
                            break;
                        case 3:
                            directionDN3 = "right";
                            break;
                    }
                }
                else if (directionDN3 == "right")
                {
                    switch (directionDN3Hold)
                    {
                        case 1:
                            directionDN3 = "down";
                            break;
                        case 2:
                            directionDN3 = "up";
                            break;
                        case 3:
                            directionDN3 = "left";
                            break;
                    }
                }
                else if (directionDN3 == "down")
                {
                    switch (directionDN3Hold)
                    {
                        case 1:
                            directionDN3 = "right";
                            break;
                        case 2:
                            directionDN3 = "left";
                            break;
                        case 3:
                            directionDN3 = "up";
                            break;
                    }
                }
            }
            #endregion
            #region darknut 4
            if (DN == 4)
            {
                if (DN4180)
                {
                    directionDN4Hold = rnd.Next(1, 3);
                }
                else
                {
                    directionDN4Hold = rnd.Next(1, 4);
                }

                if (directionDN4Hold == 3)
                {
                    DN4180 = true;
                }
                else
                {
                    DN4180 = false;
                }

                if (directionDN4 == "up")
                {
                    switch (directionDN4Hold)
                    {
                        case 1:
                            directionDN4 = "right";
                            break;
                        case 2:
                            directionDN4 = "left";
                            break;
                        case 3:
                            directionDN4 = "down";
                            break;
                    }
                }
                else if (directionDN4 == "left")
                {
                    switch (directionDN4Hold)
                    {
                        case 1:
                            directionDN4 = "down";
                            break;
                        case 2:
                            directionDN4 = "up";
                            break;
                        case 3:
                            directionDN4 = "right";
                            break;
                    }
                }
                else if (directionDN4 == "right")
                {
                    switch (directionDN4Hold)
                    {
                        case 1:
                            directionDN4 = "down";
                            break;
                        case 2:
                            directionDN4 = "up";
                            break;
                        case 3:
                            directionDN4 = "left";
                            break;
                    }
                }
                else if (directionDN4 == "down")
                {
                    switch (directionDN4Hold)
                    {
                        case 1:
                            directionDN4 = "right";
                            break;
                        case 2:
                            directionDN4 = "left";
                            break;
                        case 3:
                            directionDN4 = "up";
                            break;
                    }
                }
            }
            #endregion

            //get distance
            if (DN == 1)
            {
                distDNHold = rnd.Next(2, 6);
                stepCountDN = 0;
            }
            else if (DN == 2)
            {
                distDN2Hold = rnd.Next(2, 6);
                stepCountDN2 = 0;
            }
            else if (DN == 3)
            {
                distDN3Hold = rnd.Next(2, 6);
                stepCountDN3 = 0;
            }
            else if (DN == 4)
            {
                distDN4Hold = rnd.Next(2, 6);
                stepCountDN4 = 0;
            }
        }

        public void checkPlayerDNCollision(Rectangle player, Rectangle darknut, Rectangle darknut2, Rectangle darknut3, Rectangle darknut4)
        {
            if (!heroHurt)
            {
                if (player.IntersectsWith(darknut)
                || player.IntersectsWith(darknut2)
                || player.IntersectsWith(darknut3)
                || player.IntersectsWith(darknut4))
                {
                    heroHurt = true;
                    lifeHero--;
                }
            }
        }

        public void checkSwordCollision(Rectangle sword, Rectangle darknut, Rectangle darknut2, Rectangle darknut3, Rectangle darknut4)
        {
            if (sword.IntersectsWith(darknut))
            {
                DN = false;
                countDN--;
            }

            if (sword.IntersectsWith(darknut2))
            {
                DN2 = false;
                countDN--;
            }

            if (sword.IntersectsWith(darknut3))
            {
                DN3 = false;
                countDN--;
            }

            if (sword.IntersectsWith(darknut4))
            {
                DN4 = false;
                countDN--;
            }
        }

        private void gameTimer_Tick(object sender, EventArgs e)
        {
            if (lifeHero > 0 && countDN > 0)
            {
                switch (countDN)
                {
                    case 3:
                        speedDN = 4;
                        break;
                    case 2:
                        speedDN = 5;
                        break;
                    case 1:
                        speedDN = 6;
                        break;
                }

                //sets player and enemy hitboxes
                Rectangle player;
                Rectangle darknut = new Rectangle(xDN, yDN, widthDN, heightDN);
                Rectangle darknut2 = new Rectangle(xDN2, yDN2, widthDN, heightDN);
                Rectangle darknut3 = new Rectangle(xDN3, yDN3, widthDN, heightDN);
                Rectangle darknut4 = new Rectangle(xDN4, yDN4, widthDN, heightDN);

                #region move character based on key presses

                if (leftArrowDown == true)
                {
                    if (!attacking)
                    {
                        if (moving)
                        {
                            xHero = xHero - speedHero;

                            player = new Rectangle(xHero, yHero, widthHero, heightHero);
                            checkCollision(player);
                        }

                        if (!legal)
                        {
                            xHero = xHero + speedHero;
                        }
                        player = new Rectangle(xHero, yHero, widthHero, heightHero);
                        if (!movementPaused)
                        {
                            direction = "left";
                        }
                    }
                }

                if (downArrowDown == true)
                {
                    if (!attacking)
                    {
                        if (moving)
                        {
                            yHero = yHero + speedHero;
                            player = new Rectangle(xHero, yHero, widthHero, heightHero);
                            checkCollision(player);
                        }

                        if (!legal)
                        {
                            yHero = yHero - speedHero;
                        }
                        player = new Rectangle(xHero, yHero, widthHero, heightHero);
                        if (!movementPaused)
                        {
                            direction = "down";
                        }
                    }
                }

                if (rightArrowDown == true)
                {
                    if (!attacking)
                    {
                        if (moving)
                        {
                            xHero = xHero + speedHero;
                            player = new Rectangle(xHero, yHero, widthHero, heightHero);
                            checkCollision(player);
                        }

                        if (!legal)
                        {
                            xHero = xHero - speedHero;
                        }
                        player = new Rectangle(xHero, yHero, widthHero, heightHero);
                        if (!movementPaused)
                        {
                            direction = "right";
                        }
                    }
                }

                if (upArrowDown == true)
                {
                    if (!attacking)
                    {
                        if (moving)
                        {
                            yHero = yHero - speedHero;
                            player = new Rectangle(xHero, yHero, widthHero, heightHero);
                            checkCollision(player);
                        }

                        if (!legal)
                        {
                            yHero = yHero + speedHero;
                        }
                        player = new Rectangle(xHero, yHero, widthHero, heightHero);
                        if (!movementPaused)
                        {
                            direction = "up";
                        }
                    }
                }
                #endregion
                player = new Rectangle(xHero, yHero, widthHero, heightHero);
                #region attacks
                if (spaceDown)
                {
                    attacking = true;
                }


                if (attackTimer >= 10)
                {
                    attacking = false;
                    sword = new Rectangle(0, 0, 0, 0);
                    if (movementPaused)
                    {
                        movementPaused = false;
                        if (upArrowDown ||
                            downArrowDown ||
                            rightArrowDown ||
                            leftArrowDown)
                        {
                            moving = true;
                        }
                    }
                    attackTimer = 0;
                }

                if (attacking)
                {
                    attackTimer++;
                }
                #endregion

                #region darknut stuff
                if (DN)
                {
                    #region darknut 1 stuff
                    if (directionDN == "up")
                    {
                        yDN = yDN - speedDN;
                        darknut = new Rectangle(xDN, yDN, widthDN, heightDN);
                        checkDNCollision(darknut, darknut2, darknut3, darknut4, 1);

                        if (!legalDN)
                        {
                            yDN = yDN + speedDN;
                            genDNPath(1);
                        }
                        darknut = new Rectangle(xDN, yDN, widthDN, heightDN);
                    }
                    else if (directionDN == "left")
                    {
                        xDN = xDN - speedDN;
                        darknut = new Rectangle(xDN, yDN, widthDN, heightDN);
                        checkDNCollision(darknut, darknut2, darknut3, darknut4, 1);

                        if (!legalDN)
                        {
                            xDN = xDN + speedDN;
                            genDNPath(1);
                        }
                        darknut = new Rectangle(xDN, yDN, widthDN, heightDN);
                    }
                    else if (directionDN == "right")
                    {
                        xDN = xDN + speedDN;
                        darknut = new Rectangle(xDN, yDN, widthDN, heightDN);
                        checkDNCollision(darknut, darknut2, darknut3, darknut4, 1);

                        if (!legalDN)
                        {
                            xDN = xDN - speedDN;
                            genDNPath(1);
                        }
                        darknut = new Rectangle(xDN, yDN, widthDN, heightDN);
                    }
                    else if (directionDN == "down")
                    {
                        yDN = yDN + speedDN;
                        darknut = new Rectangle(xDN, yDN, widthDN, heightDN);
                        checkDNCollision(darknut, darknut2, darknut3, darknut4, 1);

                        if (!legalDN)
                        {
                            yDN = yDN - speedDN;
                            genDNPath(1);
                        }
                        darknut = new Rectangle(xDN, yDN, widthDN, heightDN);
                    }

                    stepCountDN = stepCountDN + speedDN;

                    if (stepCountDN == distDNHold * 48)
                    {
                        genDNPath(1);
                    }
                    #endregion
                }
                else
                {
                    xDN = 1000;
                    yDN = 1000;
                }
                darknut = new Rectangle(xDN, yDN, widthDN, heightDN);

                if (DN2)
                {
                    #region darknut 2 stuff
                    if (directionDN2 == "up")
                    {
                        yDN2 = yDN2 - speedDN;
                        darknut2 = new Rectangle(xDN2, yDN2, widthDN, heightDN);
                        checkDNCollision(darknut2, darknut, darknut3, darknut4, 2);

                        if (!legalDN2)
                        {
                            yDN2 = yDN2 + speedDN;
                            genDNPath(2);
                        }
                        darknut2 = new Rectangle(xDN2, yDN2, widthDN, heightDN);
                    }
                    else if (directionDN2 == "left")
                    {
                        xDN2 = xDN2 - speedDN;
                        darknut2 = new Rectangle(xDN2, yDN2, widthDN, heightDN);
                        checkDNCollision(darknut2, darknut, darknut3, darknut4, 2);

                        if (!legalDN2)
                        {
                            xDN2 = xDN2 + speedDN;
                            genDNPath(2);
                        }
                        darknut2 = new Rectangle(xDN, yDN, widthDN, heightDN);
                    }
                    else if (directionDN2 == "right")
                    {
                        xDN2 = xDN2 + speedDN;
                        darknut2 = new Rectangle(xDN2, yDN2, widthDN, heightDN);
                        checkDNCollision(darknut2, darknut, darknut3, darknut4, 2);

                        if (!legalDN2)
                        {
                            xDN2 = xDN2 - speedDN;
                            genDNPath(2);
                        }
                        darknut2 = new Rectangle(xDN, yDN, widthDN, heightDN);
                    }
                    else if (directionDN2 == "down")
                    {
                        yDN2 = yDN2 + speedDN;
                        darknut2 = new Rectangle(xDN2, yDN2, widthDN, heightDN);
                        checkDNCollision(darknut2, darknut, darknut3, darknut4, 2);

                        if (!legalDN2)
                        {
                            yDN2 = yDN2 - speedDN;
                            genDNPath(2);
                        }
                        darknut2 = new Rectangle(xDN, yDN, widthDN, heightDN);
                    }

                    stepCountDN2 = stepCountDN2 + speedDN;

                    if (stepCountDN2 == distDN2Hold * 48)
                    {
                        genDNPath(2);
                    }
                    #endregion
                }
                else
                {
                    xDN2 = 2000;
                    yDN2 = 2000;
                }
                darknut2 = new Rectangle(xDN2, yDN2, widthDN, heightDN);

                if (DN3)
                {
                    #region darknut 3 stuff
                    if (directionDN3 == "up")
                    {
                        yDN3 = yDN3 - speedDN;
                        darknut3 = new Rectangle(xDN3, yDN3, widthDN, heightDN);
                        checkDNCollision(darknut3, darknut, darknut2, darknut4, 3);

                        if (!legalDN3)
                        {
                            yDN3 = yDN3 + speedDN;
                            genDNPath(3);
                        }
                        darknut3 = new Rectangle(xDN3, yDN3, widthDN, heightDN);
                    }
                    else if (directionDN3 == "left")
                    {
                        xDN3 = xDN3 - speedDN;
                        darknut3 = new Rectangle(xDN3, yDN3, widthDN, heightDN);
                        checkDNCollision(darknut3, darknut, darknut2, darknut4, 3);

                        if (!legalDN3)
                        {
                            xDN3 = xDN3 + speedDN;
                            genDNPath(3);
                        }
                        darknut3 = new Rectangle(xDN3, yDN3, widthDN, heightDN);
                    }
                    else if (directionDN3 == "right")
                    {
                        xDN3 = xDN3 + speedDN;
                        darknut3 = new Rectangle(xDN3, yDN3, widthDN, heightDN);
                        checkDNCollision(darknut3, darknut, darknut2, darknut4, 3);

                        if (!legalDN3)
                        {
                            xDN3 = xDN3 - speedDN;
                            genDNPath(3);
                        }
                        darknut3 = new Rectangle(xDN3, yDN3, widthDN, heightDN);
                    }
                    else if (directionDN3 == "down")
                    {
                        yDN3 = yDN3 + speedDN;
                        darknut3 = new Rectangle(xDN3, yDN3, widthDN, heightDN);
                        checkDNCollision(darknut3, darknut, darknut2, darknut4, 3);

                        if (!legalDN3)
                        {
                            yDN3 = yDN3 - speedDN;
                            genDNPath(3);
                        }
                        darknut3 = new Rectangle(xDN3, yDN3, widthDN, heightDN);
                    }

                    stepCountDN3 = stepCountDN3 + speedDN;

                    if (stepCountDN3 == distDN3Hold * 48)
                    {
                        genDNPath(3);
                    }
                    #endregion
                }
                else
                {
                    xDN3 = 3000;
                    yDN3 = 3000;
                }
                darknut3 = new Rectangle(xDN3, yDN3, widthDN, heightDN);

                if (DN4)
                {
                    #region darknut 4 stuff
                    if (directionDN4 == "up")
                    {
                        yDN4 = yDN4 - speedDN;
                        darknut4 = new Rectangle(xDN4, yDN4, widthDN, heightDN);
                        checkDNCollision(darknut4, darknut, darknut2, darknut3, 4);

                        if (!legalDN4)
                        {
                            yDN4 = yDN4 + speedDN;
                            genDNPath(4);
                        }
                        darknut4 = new Rectangle(xDN4, yDN4, widthDN, heightDN);
                    }
                    else if (directionDN4 == "left")
                    {
                        xDN4 = xDN4 - speedDN;
                        darknut4 = new Rectangle(xDN4, yDN4, widthDN, heightDN);
                        checkDNCollision(darknut4, darknut, darknut2, darknut3, 4);

                        if (!legalDN4)
                        {
                            xDN4 = xDN4 + speedDN;
                            genDNPath(4);
                        }
                        darknut4 = new Rectangle(xDN4, yDN4, widthDN, heightDN);
                    }
                    else if (directionDN4 == "right")
                    {
                        xDN4 = xDN4 + speedDN;
                        darknut4 = new Rectangle(xDN4, yDN4, widthDN, heightDN);
                        checkDNCollision(darknut4, darknut, darknut2, darknut3, 4);

                        if (!legalDN4)
                        {
                            xDN4 = xDN4 - speedDN;
                            genDNPath(4);
                        }
                        darknut4 = new Rectangle(xDN4, yDN4, widthDN, heightDN);
                    }
                    else if (directionDN4 == "down")
                    {
                        yDN4 = yDN4 + speedDN;
                        darknut4 = new Rectangle(xDN4, yDN4, widthDN, heightDN);
                        checkDNCollision(darknut4, darknut, darknut2, darknut3, 4);

                        if (!legalDN4)
                        {
                            yDN4 = yDN4 - speedDN;
                            genDNPath(4);
                        }
                        darknut4 = new Rectangle(xDN4, yDN4, widthDN, heightDN);
                    }

                    stepCountDN4 = stepCountDN4 + speedDN;

                    if (stepCountDN4 == distDN4Hold * 48)
                    {
                        genDNPath(4);
                    }
                    #endregion
                }
                else
                {
                    xDN4 = 4000;
                    yDN4 = 4000;
                }
                darknut4 = new Rectangle(xDN4, yDN4, widthDN, heightDN);
                #endregion

                checkPlayerDNCollision(player, darknut, darknut2, darknut3, darknut4);
                if (heroHurt)
                {
                    damageTimer++;
                }
                if (damageTimer >= 50)
                {
                    heroHurt = false;
                    damageTimer = 0;
                }

                //refresh the screen, which causes the Form1_Paint method to run
                Refresh();

                checkSwordCollision(sword, darknut, darknut2, darknut3, darknut4);
            }
            else
            {
                Refresh();
            }
        }
        
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            //draw life hearts
            if(lifeHero == 3)
            {
                e.Graphics.DrawImage(fullHeart, 741, 120, 42, 48);
                e.Graphics.DrawImage(fullHeart, 741, 216, 42, 48);
                e.Graphics.DrawImage(fullHeart, 741, 312, 42, 48);
            }
            else if(lifeHero == 2)
            {
                e.Graphics.DrawImage(fullHeart, 741, 120, 42, 48);
                e.Graphics.DrawImage(fullHeart, 741, 216, 42, 48);
                e.Graphics.DrawImage(emptyHeart, 741, 312, 42, 48);
            }
            else if(lifeHero == 1)
            {
                e.Graphics.DrawImage(fullHeart, 741, 120, 42, 48);
                e.Graphics.DrawImage(emptyHeart, 741, 216, 42, 48);
                e.Graphics.DrawImage(emptyHeart, 741, 312, 42, 48);
            }
            else
            {
                e.Graphics.DrawImage(emptyHeart, 741, 120, 42, 48);
                e.Graphics.DrawImage(emptyHeart, 741, 216, 42, 48);
                e.Graphics.DrawImage(emptyHeart, 741, 312, 42, 48);
            }

            //draw sprite
            if (lifeHero > 0 && countDN > 0)
            {
                #region move character
                if (direction == "up")
                {
                    if (attacking)
                    {
                        e.Graphics.DrawImage(upAttack, xHero - 4, yHero - 40, 48, 84);
                        sword = new Rectangle(xHero + 12, yHero - 40, 8, 32);
                    }
                    else
                    {
                        if (timer <= 5)
                        {
                            e.Graphics.DrawImage(upStand, xHero + 2, yHero - 4, 36, 48);
                        }
                        else if (timer <= 9)
                        {
                            e.Graphics.DrawImage(upStep, xHero + 2, yHero - 4, 36, 48);
                        }
                        else
                        {
                            e.Graphics.DrawImage(upStep, xHero + 2, yHero - 4, 36, 48);
                            timer = 0;
                        }
                    }
                }
                else if (direction == "right")
                {
                    if (attacking)
                    {
                        e.Graphics.DrawImage(rightAttack, xHero + 2, yHero - 8, 86, 48);
                        sword = new Rectangle(xHero + 54, yHero + 15, 34, 9);
                    }
                    else
                    {
                        if (timer <= 5)
                        {
                            e.Graphics.DrawImage(rightStand, xHero - 4, yHero - 8, 48, 48);
                        }
                        else if (timer <= 9)
                        {
                            e.Graphics.DrawImage(rightStep, xHero - 4, yHero - 8, 48, 48);
                        }
                        else
                        {
                            e.Graphics.DrawImage(rightStep, xHero - 4, yHero - 8, 48, 48);
                            timer = 0;
                        }
                    }
                }
                else if (direction == "left")
                {
                    if (attacking)
                    {
                        e.Graphics.DrawImage(leftAttack, xHero - 47, yHero - 8, 86, 48);
                        sword = new Rectangle(xHero - 47, yHero + 15, 34, 9);
                    }
                    else
                    {
                        if (timer <= 5)
                        {
                            e.Graphics.DrawImage(leftStand, xHero - 4, yHero - 8, 48, 48);
                        }
                        else if (timer <= 9)
                        {
                            e.Graphics.DrawImage(leftStep, xHero - 4, yHero - 8, 48, 48);
                        }
                        else
                        {
                            e.Graphics.DrawImage(leftStep, xHero - 4, yHero - 8, 48, 48);
                            timer = 0;
                        }
                    }
                }
                else
                {
                    if (attacking)
                    {
                        e.Graphics.DrawImage(downAttack, xHero - 4, yHero - 4, 48, 81);
                        sword = new Rectangle(xHero + 18, yHero + 45, 8, 32);
                    }
                    else
                    {
                        if (timer <= 5)
                        {
                            e.Graphics.DrawImage(downStand, xHero - 2, yHero - 4, 45, 48);
                        }
                        else if (timer <= 9)
                        {
                            e.Graphics.DrawImage(downStep, xHero - 1, yHero - 4, 42, 48);
                        }
                        else
                        {
                            e.Graphics.DrawImage(downStep, xHero - 1, yHero - 4, 42, 48);
                            timer = 0;
                        }
                    }
                }
                if (moving)
                {
                    timer++;
                }
                #endregion
            }
            else
            {
                drawBrush.Color = Color.FromArgb(opacityVal, 0, 0, 0);
                e.Graphics.FillRectangle(drawBrush, 0, 0, 720, 480);

                if (opacityVal <= 255)
                {
                    opacityVal = opacityVal + 2;
                }
                if(opacityVal > 255)
                {
                    opacityVal = 255;
                }

                if (opacityVal == 255)
                {
                    Font drawFont = new Font("The Legend of Zelda NES", 32, FontStyle.Bold);
                    SolidBrush drawBrush = new SolidBrush(Color.White);
                    if(lifeHero > 0)
                    {
                        e.Graphics.DrawImage(celebration, xHero + 5, yHero - 4, 39, 48);
                        e.Graphics.DrawString("You win!", drawFont, drawBrush, 50, 250);
                        if (!soundPlayed)
                        {
                            win.Play();
                            soundPlayed = true;
                        }
                    }
                    else
                    {
                        e.Graphics.DrawString("You lose!", drawFont, drawBrush, 50, 250);
                        if (!soundPlayed)
                        {
                            lose.Play();
                            soundPlayed = true;
                        }
                    }
                }
            }

            if (countDN > 0)
            {
                #region draw darknut 1
                if (directionDN == "up")
                {
                    if (timerDN <= 7)
                    {
                        e.Graphics.DrawImage(upDN, xDN + 2, yDN, 45, 48);
                    }
                    else if (timerDN <= 13)
                    {
                        e.Graphics.DrawImage(up2DN, xDN + 2, yDN, 45, 48);
                    }
                    else
                    {
                        e.Graphics.DrawImage(up2DN, xDN + 2, yDN, 45, 48);
                        timerDN = 0;
                    }
                }
                else if (directionDN == "left")
                {
                    if (timerDN <= 7)
                    {
                        e.Graphics.DrawImage(leftDN, xDN, yDN, 48, 48);
                    }
                    else if (timerDN <= 13)
                    {
                        e.Graphics.DrawImage(left2DN, xDN, yDN + 2, 48, 45);
                    }
                    else
                    {
                        e.Graphics.DrawImage(left2DN, xDN, yDN + 2, 48, 45);
                        timerDN = 0;
                    }
                }
                else if (directionDN == "right")
                {
                    if (timerDN <= 7)
                    {
                        e.Graphics.DrawImage(rightDN, xDN, yDN, 48, 48);
                    }
                    else if (timerDN <= 13)
                    {
                        e.Graphics.DrawImage(right2DN, xDN, yDN + 2, 48, 45);
                    }
                    else
                    {
                        e.Graphics.DrawImage(right2DN, xDN, yDN + 2, 48, 45);
                        timerDN = 0;
                    }
                }
                else
                {
                    if (timerDN <= 7)
                    {
                        e.Graphics.DrawImage(downDN, xDN + 2, yDN, 45, 48);
                    }
                    else if (timerDN <= 13)
                    {
                        e.Graphics.DrawImage(down2DN, xDN + 2, yDN, 45, 48);
                    }
                    else
                    {
                        e.Graphics.DrawImage(down2DN, xDN + 2, yDN, 45, 48);
                        timerDN = 0;
                    }
                }
                #endregion
                #region draw darknut 2
                if (directionDN2 == "up")
                {
                    if (timerDN <= 7)
                    {
                        e.Graphics.DrawImage(upDN, xDN2 + 2, yDN2, 45, 48);
                    }
                    else if (timerDN <= 13)
                    {
                        e.Graphics.DrawImage(up2DN, xDN2 + 2, yDN2, 45, 48);
                    }
                    else
                    {
                        e.Graphics.DrawImage(up2DN, xDN2 + 2, yDN2, 45, 48);
                    }
                }
                else if (directionDN2 == "left")
                {
                    if (timerDN <= 7)
                    {
                        e.Graphics.DrawImage(leftDN, xDN2, yDN2, 48, 48);
                    }
                    else if (timerDN <= 13)
                    {
                        e.Graphics.DrawImage(left2DN, xDN2, yDN2 + 2, 48, 45);
                    }
                    else
                    {
                        e.Graphics.DrawImage(left2DN, xDN2, yDN2 + 2, 48, 45);
                    }
                }
                else if (directionDN2 == "right")
                {
                    if (timerDN <= 7)
                    {
                        e.Graphics.DrawImage(rightDN, xDN2, yDN2, 48, 48);
                    }
                    else if (timerDN <= 13)
                    {
                        e.Graphics.DrawImage(right2DN, xDN2, yDN2 + 2, 48, 45);
                    }
                    else
                    {
                        e.Graphics.DrawImage(right2DN, xDN2, yDN2 + 2, 48, 45);
                    }
                }
                else
                {
                    if (timerDN <= 7)
                    {
                        e.Graphics.DrawImage(downDN, xDN2 + 2, yDN2, 45, 48);
                    }
                    else if (timerDN <= 13)
                    {
                        e.Graphics.DrawImage(down2DN, xDN2 + 2, yDN2, 45, 48);
                    }
                    else
                    {
                        e.Graphics.DrawImage(down2DN, xDN2 + 2, yDN2, 45, 48);
                    }
                }
                #endregion
                #region draw darknut 3
                if (directionDN3 == "up")
                {
                    if (timerDN <= 7)
                    {
                        e.Graphics.DrawImage(upDN, xDN3 + 2, yDN3, 45, 48);
                    }
                    else if (timerDN <= 13)
                    {
                        e.Graphics.DrawImage(up2DN, xDN3 + 2, yDN3, 45, 48);
                    }
                    else
                    {
                        e.Graphics.DrawImage(up2DN, xDN3 + 2, yDN3, 45, 48);
                    }
                }
                else if (directionDN3 == "left")
                {
                    if (timerDN <= 7)
                    {
                        e.Graphics.DrawImage(leftDN, xDN3, yDN3, 48, 48);
                    }
                    else if (timerDN <= 13)
                    {
                        e.Graphics.DrawImage(left2DN, xDN3, yDN3 + 2, 48, 45);
                    }
                    else
                    {
                        e.Graphics.DrawImage(left2DN, xDN3, yDN3 + 2, 48, 45);
                    }
                }
                else if (directionDN3 == "right")
                {
                    if (timerDN <= 7)
                    {
                        e.Graphics.DrawImage(rightDN, xDN3, yDN3, 48, 48);
                    }
                    else if (timerDN <= 13)
                    {
                        e.Graphics.DrawImage(right2DN, xDN3, yDN3 + 2, 48, 45);
                    }
                    else
                    {
                        e.Graphics.DrawImage(right2DN, xDN3, yDN3 + 2, 48, 45);
                    }
                }
                else
                {
                    if (timerDN <= 7)
                    {
                        e.Graphics.DrawImage(downDN, xDN3 + 2, yDN3, 45, 48);
                    }
                    else if (timerDN <= 13)
                    {
                        e.Graphics.DrawImage(down2DN, xDN3 + 2, yDN3, 45, 48);
                    }
                    else
                    {
                        e.Graphics.DrawImage(down2DN, xDN3 + 2, yDN3, 45, 48);
                    }
                }
                #endregion
                #region draw darknut 4
                if (directionDN4 == "up")
                {
                    if (timerDN <= 7)
                    {
                        e.Graphics.DrawImage(upDN, xDN4 + 2, yDN4, 45, 48);
                    }
                    else if (timerDN <= 13)
                    {
                        e.Graphics.DrawImage(up2DN, xDN4 + 2, yDN4, 45, 48);
                    }
                    else
                    {
                        e.Graphics.DrawImage(up2DN, xDN4 + 2, yDN4, 45, 48);
                    }
                }
                else if (directionDN4 == "left")
                {
                    if (timerDN <= 7)
                    {
                        e.Graphics.DrawImage(leftDN, xDN4, yDN4, 48, 48);
                    }
                    else if (timerDN <= 13)
                    {
                        e.Graphics.DrawImage(left2DN, xDN4, yDN4 + 2, 48, 45);
                    }
                    else
                    {
                        e.Graphics.DrawImage(left2DN, xDN4, yDN4 + 2, 48, 45);
                    }
                }
                else if (directionDN4 == "right")
                {
                    if (timerDN <= 7)
                    {
                        e.Graphics.DrawImage(rightDN, xDN4, yDN4, 48, 48);
                    }
                    else if (timerDN <= 13)
                    {
                        e.Graphics.DrawImage(right2DN, xDN4, yDN4 + 2, 48, 45);
                    }
                    else
                    {
                        e.Graphics.DrawImage(right2DN, xDN4, yDN4 + 2, 48, 45);
                    }
                }
                else
                {
                    if (timerDN <= 7)
                    {
                        e.Graphics.DrawImage(downDN, xDN4 + 2, yDN4, 45, 48);
                    }
                    else if (timerDN <= 13)
                    {
                        e.Graphics.DrawImage(down2DN, xDN4 + 2, yDN4, 45, 48);
                    }
                    else
                    {
                        e.Graphics.DrawImage(down2DN, xDN4 + 2, yDN4, 45, 48);
                    }
                }
                #endregion
                timerDN++;
            }
        }
    }
}
