﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FontAwesome.Sharp;

namespace CurtainDesigner
{
    public partial class Form1 : Form
    {
        //Fields
        private bool subMenuIsOpen;
        private IconButton currentButton;
        private IconButton currentButtonSubMenu;
        private bool orderButtonIsActivate;
        private object currentClickButton;

        public Form1()
        {
            InitializeComponent();
            panelSubMenu.Height = panelSubMenu.MinimumSize.Height;
            subMenuIsOpen = false;
            this.Size = this.MinimumSize;
            startprogramm_SetSelectedButton();
        }

        //Animation SubMenu
        private void timerOpenSubMenu_Tick(object sender, EventArgs e)
        {
            //Check, whick button has been clicked
            if((currentClickButton as Button).Name.Equals("iconButtonNewOrder"))
            {
                if (!this.subMenuIsOpen)
                {
                    if(panelSubMenu.Height != panelSubMenu.MaximumSize.Height)
                    {
                        panelSubMenu.Height = panelSubMenu.Height + 10;
                    }
                    else
                    {
                        this.subMenuIsOpen = true;
                        timerOpenSubMenu.Stop();
                    }
                }
                else
                {
                    if (panelSubMenu.Height != panelSubMenu.MinimumSize.Height)
                    {
                        panelSubMenu.Height = panelSubMenu.Height - 10;
                    }
                    else
                    {
                        this.subMenuIsOpen = false;
                        timerOpenSubMenu.Stop();
                    }
                }
            }
            else
            {
                if (panelSubMenu.Height != panelSubMenu.MinimumSize.Height)
                {
                    panelSubMenu.Height = panelSubMenu.Height - 10;
                }
                else
                {
                    this.subMenuIsOpen = false;
                    timerOpenSubMenu.Stop();
                }
            }
        }

        //Timer for SubMenu animation
        private void iconButtonNewOrder_Click_1(object sender, EventArgs e)
        {
            ActivateButton(sender, Colors.color3);
            currentClickButton = sender;
            timerOpenSubMenu.Start();
        }

        //Color struct
        private struct Colors
        {
            public static Color color1 = Color.FromArgb(172,126,241);
            public static Color color2 = Color.FromArgb(249,118,176);
            public static Color color3 = Color.FromArgb(253,138,114);
            public static Color color4 = Color.FromArgb(95,77,221);
            public static Color color5 = Color.FromArgb(249,88,155);
            public static Color color6 = Color.FromArgb(24,161,251);
            public static Color color7 = Color.Silver;
        }

        #region [Design][ActivateMainMenuButton]
        private void ActivateButton(object SenderBtn, Color color)
        {
            if(SenderBtn != null)
            {
                //Special Customize for OrderButton
                if((SenderBtn as Button).Name.Equals("iconButtonNewOrder") && orderButtonIsActivate)
                {
                    DeactivateButton();
                    orderButtonIsActivate = false;
                    return;
                }
                else if((SenderBtn as Button).Name.Equals("iconButtonNewOrder") && !orderButtonIsActivate)
                {
                    DeactivateButton();
                    currentButton = (IconButton)SenderBtn;
                    currentButton.BackColor = Color.FromArgb(31, 53, 97);
                    currentButton.ForeColor = color;
                    currentButton.IconColor = color;
                    currentButton.IconSize = currentButton.IconSize + 2;
                    currentButton.Font = new System.Drawing.Font("Century Gothic", 10.50F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
                    orderButtonIsActivate = true;
                    return;
                }

                DeactivateButton();
                //Customize selected button
                currentButton = (IconButton)SenderBtn;
                currentButton.BackColor = Color.FromArgb(31, 53, 97);
                currentButton.ForeColor = color;
                currentButton.IconColor = color;
                currentButton.IconSize = currentButton.IconSize + 2;
                currentButton.Font = new System.Drawing.Font("Century Gothic", 10.50F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            }
        }

        private void DeactivateButton()
        {
            if (currentButton != null)
            {
                //Return previous button to normal state
                currentButton.BackColor = Color.FromArgb(18, 30, 54);
                currentButton.ForeColor = Color.Gainsboro;
                currentButton.IconColor = Color.Gainsboro;
                currentButton.IconSize = 32;
                currentButton.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            }
        }
        #endregion

        #region [Design][ActivateSubMainMenuButton]
        private void ActivateSubMenuButton(object SenderBtn, Color color)
        {
            if(SenderBtn != null)
            {
                DeactivateSubMenuButton();
                currentButtonSubMenu = (IconButton)SenderBtn;
                currentButtonSubMenu.BackColor = Color.FromArgb(31, 53, 97);
                currentButtonSubMenu.ForeColor = color;
                currentButtonSubMenu.IconColor = color;
                currentButtonSubMenu.IconSize = currentButton.IconSize + 1;
                currentButtonSubMenu.Font = new System.Drawing.Font("Century Gothic", 10.00F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            }
        }

        private void DeactivateSubMenuButton()
        {
            if (currentButtonSubMenu != null)
            {
                //Return previous button to normal state
                currentButtonSubMenu.BackColor = Color.FromArgb(98, 117, 155);
                currentButtonSubMenu.ForeColor = Color.Gainsboro;
                currentButtonSubMenu.IconColor = Color.Gainsboro;
                currentButtonSubMenu.IconSize = 30;
                currentButtonSubMenu.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            }
        }
        #endregion

        private void startprogramm_SetSelectedButton()
        {
            ActivateButton(iconButtonAllOrders, Colors.color2);
        }

        private void iconButtonAllOrders_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, Colors.color6);
            orderButtonIsActivate = false;
            currentClickButton = sender;
            timerOpenSubMenu.Start();
        }

        private void iconButtonSettings_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, Colors.color2);
            orderButtonIsActivate = false;
            currentClickButton = sender;
            timerOpenSubMenu.Start();
        }

        private void iconButtonInfo_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, Colors.color4);
            orderButtonIsActivate = false;
            currentClickButton = sender;
            timerOpenSubMenu.Start();
        }

        private void iconButtonFabricCurtains_MouseEnter(object sender, EventArgs e)
        {
            ActivateSubMenuButton(sender, Colors.color7);
        }

        private void iconButtonFabricCurtains_MouseLeave(object sender, EventArgs e)
        {
            DeactivateSubMenuButton();
        }

        private void iconButton2_MouseEnter(object sender, EventArgs e)
        {
            ActivateSubMenuButton(sender, Colors.color7);
        }

        private void iconButton2_MouseLeave(object sender, EventArgs e)
        {
            DeactivateSubMenuButton();
        }

        private void iconButton3_MouseEnter(object sender, EventArgs e)
        {
            ActivateSubMenuButton(sender, Colors.color7);
        }

        private void iconButton3_MouseLeave(object sender, EventArgs e)
        {
            DeactivateSubMenuButton();
        }

        private void iconButton4_MouseEnter(object sender, EventArgs e)
        {
            ActivateSubMenuButton(sender, Colors.color7);
        }

        private void iconButton4_MouseLeave(object sender, EventArgs e)
        {
            DeactivateSubMenuButton();
        }

        private void iconButton5_MouseEnter(object sender, EventArgs e)
        {
            ActivateSubMenuButton(sender, Colors.color7);
        }

        private void iconButton5_MouseLeave(object sender, EventArgs e)
        {
            DeactivateSubMenuButton();
        }

        private void iconButton6_MouseEnter(object sender, EventArgs e)
        {
            ActivateSubMenuButton(sender, Colors.color7);
        }

        private void iconButton6_MouseLeave(object sender, EventArgs e)
        {
            DeactivateSubMenuButton();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
