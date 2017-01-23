using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Final
{
    public partial class Form1 : Form
    {
        #region image declarations and stuff
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
        int speedHero = 4;
        int widthHero = 40;
        int heightHero = 40;

        #region initial starting values and other variables for Darknuts
        //universal
        int speedDN = 3;
        int widthDN = 48;
        int heightDN = 48;
        int timerDN;

        //top left (1)
        int xDN = 72;
        int yDN = 72;
        int stepCountDN = 0;
        string directionDN = "down";
        int directionDNHold;
        int distDNHold;
        bool legalDN = true;
        bool DN180 = false;

        //top right (2)
        int xDN2 = 599;
        int yDN2 = 72;
        int stepCountDN2 = 0;
        string directionDN2 = "down";
        int directionDN2Hold;
        int distDN2Hold;
        bool legalDN2 = true;
        bool DN2180 = false;

        //bottom left (3)
        int xDN3 = 72;
        int yDN3 = 359;
        int stepCountDN3 = 0;
        string directionDN3 = "down";
        int directionDN3Hold;
        int distDN3Hold;
        bool legalDN3 = true;
        bool DN3180 = false;
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

        public void checkDNCollision(Rectangle darknut, int DN)
        {
            if(DN == 1)
            {
                if (darknut.IntersectsWith(nWall)
                || darknut.IntersectsWith(eWall)
                || darknut.IntersectsWith(sWall)
                || darknut.IntersectsWith(wWall)
                || darknut.IntersectsWith(leftObstacle)
                || darknut.IntersectsWith(rightObstacle))

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
                || darknut.IntersectsWith(rightObstacle))

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
                || darknut.IntersectsWith(rightObstacle))

                {
                    legalDN3 = false;
                }
                else
                {
                    legalDN3 = true;
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
        }

        private void gameTimer_Tick(object sender, EventArgs e)
        {
            //sets player hitbox
            Rectangle player;
            Rectangle darknut;
            Rectangle darknut2;
            Rectangle darknut3;

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

            #region attacks
            if (spaceDown)
            {
                attacking = true;
            }


            if (attackTimer >= 10)
            {
                attacking = false;
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

            #region darknut 1 stuff
            if(directionDN == "up")
            {
                yDN = yDN - speedDN;
                darknut = new Rectangle(xDN, yDN, widthDN, heightDN);
                checkDNCollision(darknut, 1);

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
                checkDNCollision(darknut, 1);

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
                checkDNCollision(darknut, 1);

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
                checkDNCollision(darknut, 1);

                if (!legalDN)
                {
                    yDN = yDN - speedDN;
                    genDNPath(1);
                }
                darknut = new Rectangle(xDN, yDN, widthDN, heightDN);
            }

            stepCountDN = stepCountDN + speedDN;

            if(stepCountDN == distDNHold * 48)
            {
                genDNPath(1);
            }
            #endregion

            #region darknut 2 stuff
            if (directionDN2 == "up")
            {
                yDN2 = yDN2 - speedDN;
                darknut2 = new Rectangle(xDN2, yDN2, widthDN, heightDN);
                checkDNCollision(darknut2, 2);

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
                checkDNCollision(darknut2, 2);

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
                checkDNCollision(darknut2, 2);

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
                checkDNCollision(darknut2, 2);

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

            #region darknut 3 stuff
            if (directionDN3 == "up")
            {
                yDN3 = yDN3 - speedDN;
                darknut3 = new Rectangle(xDN3, yDN3, widthDN, heightDN);
                checkDNCollision(darknut3, 3);

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
                checkDNCollision(darknut3, 3);

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
                checkDNCollision(darknut3, 3);

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
                checkDNCollision(darknut3, 3);

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

            //refresh the screen, which causes the Form1_Paint method to run
            Refresh();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            //draw sprit1e
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
            timerDN++;
        }
    }
}
