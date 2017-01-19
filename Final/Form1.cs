﻿using System;
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
        //image declarations and stuff
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

        //initial starting values for Hero character and other stuff
        int xHero = 336;
        int yHero = 216;
        int speedHero = 4;
        int widthHero = 40;
        int heightHero = 40;

        //initial starting values for Darknut
        int xDN = 599;
        int yDN = 359;
        int widthDN = 45;
        int heightDN = 45;
        int timerDN;
        string directionDN = "down";

        //various variables for varying variable things
        bool moving = false;
        bool legal = true;
        bool movementPaused = false;
        bool attacking = false;
        string direction = "down";
        int timer;
        int attackTimer;

        //sets rectangle around the obstacles
        Rectangle leftObstacle = new Rectangle(168, 168, 94, 144);
        Rectangle rightObstacle = new Rectangle(456, 168, 94, 144);
        Rectangle legalSpace = new Rectangle(111, 111, 501, 261);
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
            if (!(player.IntersectsWith(legalSpace))
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

        private void gameTimer_Tick(object sender, EventArgs e)
        {
            //sets player hitbox
            Rectangle player;

            //

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
                        directionDN = "left";
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
                        directionDN = "down";
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
                        directionDN = "right";
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
                        directionDN = "up";
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

            #region darknut stuff

            #endregion

            //refresh the screen, which causes the Form1_Paint method to run
            Refresh();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            //draw sprite
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

            #region draw darknut
            if (directionDN == "up")
            {
                if (timerDN <= 5)
                {
                    e.Graphics.DrawImage(upDN, xDN, yDN - 2, 45, 48);
                }
                else if (timerDN <= 9)
                {
                    e.Graphics.DrawImage(up2DN, xDN, yDN - 2, 45, 48);
                }
                else
                {
                    e.Graphics.DrawImage(up2DN, xDN, yDN - 2, 45, 48);
                    timerDN = 0;
                }
            }
            else if (directionDN == "left")
            {
                if (timerDN <= 5)
                {
                    e.Graphics.DrawImage(leftDN, xDN - 2, yDN - 2, 48, 48);
                }
                else if (timerDN <= 9)
                {
                    e.Graphics.DrawImage(left2DN, xDN - 2, yDN, 48, 45);
                }
                else
                {
                    e.Graphics.DrawImage(left2DN, xDN - 2, yDN, 48, 45);
                    timerDN = 0;
                }
            }
            else if (directionDN == "right")
            {
                if (timerDN <= 5)
                {
                    e.Graphics.DrawImage(rightDN, xDN - 2, yDN - 2, 48, 48);
                }
                else if (timerDN <= 9)
                {
                    e.Graphics.DrawImage(right2DN, xDN - 2, yDN, 48, 45);
                }
                else
                {
                    e.Graphics.DrawImage(right2DN, xDN - 2, yDN, 48, 45);
                    timerDN = 0;
                }
            }
            else
            {
                if (timerDN <= 5)
                {
                    e.Graphics.DrawImage(downDN, xDN, yDN - 2, 45, 48);
                }
                else if (timerDN <= 9)
                {
                    e.Graphics.DrawImage(down2DN, xDN, yDN - 2, 45, 48);
                }
                else
                {
                    e.Graphics.DrawImage(down2DN, xDN, yDN - 2, 45, 48);
                    timerDN = 0;
                }
            }
            timerDN++;
            #endregion
        }
    }
}
