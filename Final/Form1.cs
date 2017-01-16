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
        //initial starting values for Hero character and other stuff
        int xHero = 72;
        int yHero = 72;
        int speedHero = 5;
        int widthHero = 40;
        int heightHero = 40;
        bool legal = true;

        //sets rectangle around the obstacles
        Rectangle leftObstacle = new Rectangle(168, 168, 94, 144);
        Rectangle rightObstacle = new Rectangle(456, 168, 94, 144);
        Rectangle legalSpace = new Rectangle(111, 111, 501, 261);

        //determines whether a key is being pressed or not - DO NOT CHANGE
        Boolean leftArrowDown, downArrowDown, rightArrowDown, upArrowDown;

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
                    leftArrowDown = true;
                    break;
                case Keys.Down:
                    downArrowDown = true;
                    break;
                case Keys.Right:
                    rightArrowDown = true;
                    break;
                case Keys.Up:
                    upArrowDown = true;
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
                    break;
                case Keys.Down:
                    downArrowDown = false;
                    break;
                case Keys.Right:
                    rightArrowDown = false;
                    break;
                case Keys.Up:
                    upArrowDown = false;
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
                xHero = xHero - speedHero;

                player = new Rectangle(xHero, yHero, widthHero, heightHero);
                checkCollision(player);
              
                if (!legal)
                {
                    xHero = xHero + speedHero;
                }
            }

            if (downArrowDown == true)
            {
                yHero = yHero + speedHero;
                player = new Rectangle(xHero, yHero, widthHero, heightHero);
                checkCollision(player);

                if (!legal)
                {
                    yHero = yHero - speedHero;
                }
            }

            if (rightArrowDown == true)
            {
                xHero = xHero + speedHero;
                player = new Rectangle(xHero, yHero, widthHero, heightHero);
                checkCollision(player);

                if (!legal)
                {
                    xHero = xHero - speedHero;
                }
            }

            if (upArrowDown == true)
            {
                yHero = yHero - speedHero;
                player = new Rectangle(xHero, yHero, widthHero, heightHero);
                checkCollision(player);

                if (!legal)
                {
                    yHero = yHero + speedHero;
                }
            }

            #endregion



            //refresh the screen, which causes the Form1_Paint method to run
            Refresh();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            //draw rectangle to screen
            e.Graphics.FillRectangle(drawBrush, xHero, yHero, widthHero, heightHero);
        }
    }
}
