﻿using FontAwesome.Sharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CurtainDesigner.SettingForm
{
    public partial class FormSettings : Form
    {
        private Bunifu.Framework.UI.BunifuFlatButton currentButton = null;
        private Bunifu.Framework.UI.BunifuFlatButton currentButtonMainMenu = null;
        private bool iconIsActive = false;
        private bool ButtonOthersIsActive = false;
        private UserControl activeControl;
        private Panel currentOpenPanel = null;

        private UserControls.UserControlClientDataBase control_clients;
        private UserControls.UserControlColorDataBase control_colors;
        private UserControls.UserControlSideControllDataBase control_sidecontrol;
        private UserControls.UserControlInstallationDataBase control_installation;

        private UserControls.UCSettingsFabricCurtain.UserControlCurt_categoryFB control_fc_categories;
        private UserControls.UCSettingsFabricCurtain.UserControlCurt_TypeFC control_fc_types;
        private UserControls.UCSettingsFabricCurtain.UserControlCurt_SubtypeFC control_fc_subtypes;
        private UserControls.UCSettingsFabricCurtain.UserControlCurt_AdditionalEquipmentFC control_fc_additionalEquipment;
        private UserControls.UCSettingsFabricCurtain.UserControlCurt_fabricFC control_fc_fabric;

        private UserControls.UCSettingsDayNightCurtain.UserControlCurt_AdditionalEquipmentDNC control_dnc_additionalEquipment;
        private UserControls.UCSettingsDayNightCurtain.UserControlCurt_TypeDNC control_dnc_types;
        private UserControls.UCSettingsDayNightCurtain.UserControlCurt_SubtypeDNC control_dnc_subtypes;
        private UserControls.UCSettingsDayNightCurtain.UserControlCurt_categoryDNC control_dnc_categories;
        private UserControls.UCSettingsDayNightCurtain.UserControlCurt_fabricDNC control_dnc_fabric;

        private UserControls.UCSettingsProtectiveCurtain.UserControlCurt_AdditionalEquipmentPC control_pc_additionalEquipment;
        private UserControls.UCSettingsProtectiveCurtain.UserControlCurt_categoryPC control_pc_categories;

        private UserControls.UCSettingsRomanCurtain.UserControlCurt_AdditionalEquipmentRC control_rc_additionalEquipment;
        private UserControls.UCSettingsRomanCurtain.UserControlCurt_categoryRC control_rc_categories;
        private UserControls.UCSettingsRomanCurtain.UserControlCurt_fabricRC control_rc_fabric;

        private UserControls.UCSettingsMosquitoCurtain.UserControlCurt_AdditionalEquipmentMC control_mc_additionalEquipment;
        private UserControls.UCSettingsMosquitoCurtain.UserControlCurt_TypeMC control_mc_types;

        private UserControls.UCSettingsHorizontalCurtain.UserControlCurt_AdditionalEquipmentHC control_hc_additionalEquipment;
        private UserControls.UCSettingsHorizontalCurtain.UserControlCurt_TypeHC control_hc_types;

        private UserControls.UCSettingsVerticalCurtain.UserControlCurt_AdditionalEquipmentVC control_vc_additionalEquipment;
        private UserControls.UCSettingsVerticalCurtain.UserControlSideControllDataBaseVC control_vc_sidecontrol;
        private UserControls.UCSettingsVerticalCurtain.UserControlCurt_TypeVC control_vc_types;
        private UserControls.UCSettingsVerticalCurtain.UserControlCurt_fabricVC control_vc_fabrics;
        //private UserControls.UCSettingsDayNightCurtain.UserControlCurt_fabricDNC control_dnc_fabric;

        private Panel mainMenuSidePanel = null;

        public FormSettings()
        {
            InitializeComponent();
            addUserControlls();
            
            panelSidePanelFabricCurtain.Hide();

            init_MainbuttonSelectedPanel();
        }

        //private void setActiveButtonOnStartForm()
        //{
        //    AvtivateMainMenuButton(bunifuFlatButtonMainSettingFabricCurtains);
        //    CloseChildControl();
        //    openSidePanel(panelSidePanelFabricCurtain);
        //}

        //Color struct
        private struct Colors
        {
            public static Color colorMainMenuButtonInactive = Color.FromArgb(7, 5, 33);
            public static Color colorMainMenuTextInactive = Color.FromArgb(62, 72, 93);
            public static Color colorMainMenuButtonActive = Color.FromArgb(18, 16, 51);
            public static Color colorMainMenuTextActive = Color.FromArgb(112, 123, 140);

            public static Color colorSubMenuButtonInactive = Color.FromArgb(232, 238, 245);
            public static Color colorSubMenuTextInactive = Color.FromArgb(112, 123, 140);
            public static Color colorSubMenuButtonActive = Color.FromArgb(223, 232, 242);
            public static Color colorSubMenuTextActive = Color.FromArgb(62, 72, 93);

            public static Color color5 = Color.FromArgb(249, 88, 155);
            public static Color color6 = Color.FromArgb(24, 161, 251);
            public static Color color7 = Color.Silver;
        }

        private void init_MainbuttonSelectedPanel()
        {
            mainMenuSidePanel = new Panel { Size = new Size(8, 41), Dock = DockStyle.Right, BackColor = Color.FromArgb(255, 229, 127), Visible = true };
        }

        private void addUserControlls()
        {
            this.panelContainer.Controls.Add(control_installation = new UserControls.UserControlInstallationDataBase()); control_installation.Hide();
            this.panelContainer.Controls.Add(control_sidecontrol = new UserControls.UserControlSideControllDataBase()); control_sidecontrol.Hide();
            this.panelContainer.Controls.Add(control_colors = new UserControls.UserControlColorDataBase()); control_colors.Hide();
            this.panelContainer.Controls.Add(control_clients = new UserControls.UserControlClientDataBase()); control_clients.Hide();

            this.panelContainer.Controls.Add(control_fc_categories = new UserControls.UCSettingsFabricCurtain.UserControlCurt_categoryFB()); control_fc_categories.Hide();
            this.panelContainer.Controls.Add(control_fc_types = new UserControls.UCSettingsFabricCurtain.UserControlCurt_TypeFC()); control_fc_types.Hide();
            this.panelContainer.Controls.Add(control_fc_subtypes = new UserControls.UCSettingsFabricCurtain.UserControlCurt_SubtypeFC()); control_fc_subtypes.Hide();
            this.panelContainer.Controls.Add(control_fc_additionalEquipment = new UserControls.UCSettingsFabricCurtain.UserControlCurt_AdditionalEquipmentFC()); control_fc_additionalEquipment.Hide();
            this.panelContainer.Controls.Add(control_fc_fabric = new UserControls.UCSettingsFabricCurtain.UserControlCurt_fabricFC()); control_fc_fabric.Hide();

            this.panelContainer.Controls.Add(control_dnc_additionalEquipment = new UserControls.UCSettingsDayNightCurtain.UserControlCurt_AdditionalEquipmentDNC()); control_dnc_additionalEquipment.Hide();
            this.panelContainer.Controls.Add(control_dnc_categories = new UserControls.UCSettingsDayNightCurtain.UserControlCurt_categoryDNC()); control_dnc_categories.Hide();
            this.panelContainer.Controls.Add(control_dnc_fabric = new UserControls.UCSettingsDayNightCurtain.UserControlCurt_fabricDNC()); control_dnc_fabric.Hide();
            this.panelContainer.Controls.Add(control_dnc_subtypes = new UserControls.UCSettingsDayNightCurtain.UserControlCurt_SubtypeDNC()); control_dnc_subtypes.Hide();
            this.panelContainer.Controls.Add(control_dnc_types = new UserControls.UCSettingsDayNightCurtain.UserControlCurt_TypeDNC()); control_dnc_types.Hide();

            this.panelContainer.Controls.Add(control_pc_additionalEquipment = new UserControls.UCSettingsProtectiveCurtain.UserControlCurt_AdditionalEquipmentPC()); control_pc_additionalEquipment.Hide();
            this.panelContainer.Controls.Add(control_pc_categories = new UserControls.UCSettingsProtectiveCurtain.UserControlCurt_categoryPC()); control_pc_categories.Hide();

            this.panelContainer.Controls.Add(control_rc_additionalEquipment = new UserControls.UCSettingsRomanCurtain.UserControlCurt_AdditionalEquipmentRC()); control_rc_additionalEquipment.Hide();
            this.panelContainer.Controls.Add(control_rc_categories = new UserControls.UCSettingsRomanCurtain.UserControlCurt_categoryRC()); control_rc_categories.Hide();
            this.panelContainer.Controls.Add(control_rc_fabric = new UserControls.UCSettingsRomanCurtain.UserControlCurt_fabricRC()); control_rc_fabric.Hide();

            this.panelContainer.Controls.Add(control_mc_additionalEquipment = new UserControls.UCSettingsMosquitoCurtain.UserControlCurt_AdditionalEquipmentMC()); control_mc_additionalEquipment.Hide();
            this.panelContainer.Controls.Add(control_mc_types = new UserControls.UCSettingsMosquitoCurtain.UserControlCurt_TypeMC()); control_mc_types.Hide();

            this.panelContainer.Controls.Add(control_hc_additionalEquipment = new UserControls.UCSettingsHorizontalCurtain.UserControlCurt_AdditionalEquipmentHC()); control_hc_additionalEquipment.Hide();
            this.panelContainer.Controls.Add(control_hc_types = new UserControls.UCSettingsHorizontalCurtain.UserControlCurt_TypeHC()); control_hc_types.Hide();

            this.panelContainer.Controls.Add(control_vc_additionalEquipment = new UserControls.UCSettingsVerticalCurtain.UserControlCurt_AdditionalEquipmentVC()); control_vc_additionalEquipment.Hide();
            this.panelContainer.Controls.Add(control_vc_sidecontrol = new UserControls.UCSettingsVerticalCurtain.UserControlSideControllDataBaseVC()); control_vc_sidecontrol.Hide();
            this.panelContainer.Controls.Add(control_vc_types = new UserControls.UCSettingsVerticalCurtain.UserControlCurt_TypeVC()); control_vc_types.Hide();
            this.panelContainer.Controls.Add(control_vc_fabrics = new UserControls.UCSettingsVerticalCurtain.UserControlCurt_fabricVC()); control_vc_fabrics.Hide();
            //this.panelContainer.Controls.Add(control_dnc_types = new UserControls.UCSettingsDayNightCurtain.UserControlCurt_TypeDNC()); control_dnc_types.Hide();
        }

        private void openSidePanel(Panel sender)
        {
            if (currentOpenPanel != null)
                currentOpenPanel.Hide();
            currentOpenPanel = sender;
            sender.BringToFront();
            sender.Show();
        }

        private void OpenChildControl(UserControl childControl, object sender)
        {
            if (activeControl != null)
                activeControl.Hide();
            try
            {
                activeControl = childControl ?? throw new NullReferenceException();
                childControl.Dock = DockStyle.Fill;
                this.panelContainer.Tag = childControl;
                childControl.BringToFront();
                childControl.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CloseChildControl()
        {
            if (activeControl != null)
                activeControl.Hide();
        }

        private void AvtivateMainMenuButton(object sender)
        {
            DeactivateMainMenuButton();
            //Customize selected button
            currentButtonMainMenu = (Bunifu.Framework.UI.BunifuFlatButton)sender;
            currentButtonMainMenu.Normalcolor = Colors.colorMainMenuButtonActive;
            currentButtonMainMenu.Textcolor = Colors.colorMainMenuTextActive;
            currentButtonMainMenu.IconZoom = 60;
            pictureBoxView.BackgroundImage = currentButtonMainMenu.Iconimage;
            labelView.Text = currentButtonMainMenu.Text + " :";
            currentButtonMainMenu.Controls.Add(mainMenuSidePanel);
            mainMenuSidePanel.BringToFront();
            mainMenuSidePanel.Show();
            //ButtonOthersIsActive = true;
        }

        private void DeactivateMainMenuButton()
        {
            if (currentButtonMainMenu != null)
            {
                //Return previous button to normal state
                currentButtonMainMenu.Normalcolor = Colors.colorMainMenuButtonInactive;
                currentButtonMainMenu.Textcolor = Colors.colorMainMenuTextInactive;
                currentButtonMainMenu.IconZoom = 55;
                currentButtonMainMenu.Controls.Remove(mainMenuSidePanel);
            }
        }

        private void ActivateButton(object SenderBtn)
        {
            if ((SenderBtn as Bunifu.Framework.UI.BunifuFlatButton).Text == "Інше")
            {
                if(!ButtonOthersIsActive)
                {
                    DeactivateButton();
                    //Customize selected button
                    currentButton = (Bunifu.Framework.UI.BunifuFlatButton)SenderBtn;
                    currentButton.Normalcolor = Colors.colorSubMenuButtonActive;
                    currentButton.Textcolor = Colors.colorSubMenuTextActive;
                    currentButton.IconZoom = 60;
                    pictureBoxView2.BackgroundImage = currentButton.Iconimage;
                    labelView2.Text = currentButton.Text;
                    ButtonOthersIsActive = true;
                }
                else
                {
                    DeactivateButton();
                    ButtonOthersIsActive = false;
                }
            }
            else
            {
                DeactivateButton();
                //Customize selected button
                currentButton = (Bunifu.Framework.UI.BunifuFlatButton)SenderBtn;
                currentButton.Normalcolor = Colors.colorSubMenuButtonActive;
                currentButton.Textcolor = Colors.colorSubMenuTextActive;
                currentButton.IconZoom = 60;
                pictureBoxView2.BackgroundImage = currentButton.Iconimage;
                labelView2.Text = currentButton.Text;
            }
        }

        private void DeactivateButton()
        {
            if (currentButton != null)
            {
                //Return previous button to normal state
                currentButton.Normalcolor = Colors.colorSubMenuButtonInactive;
                currentButton.Textcolor = Colors.colorSubMenuTextInactive;
                currentButton.IconZoom = 55;
            }
        }

        private void bunifuMainSettingsButton_Click(object sender, EventArgs e)
        {
            ActivateButton(sender);
            //do_visiblePanelOthers(sender);
        }

        private void bunifuClientDataBaseButton_Click(object sender, EventArgs e)
        {
            ActivateButton(sender);
            //do_visiblePanelOthers(sender);
            OpenChildControl(control_clients, sender);
            control_clients.load_clients();
        }

        private void bunifuColorDataBaseButton_Click(object sender, EventArgs e)
        {
            ActivateButton(sender);
            //do_visiblePanelOthers(sender);
        }

        private void bunifuFabricDataBaseButton_Click(object sender, EventArgs e)
        {
            ActivateButton(sender);
            //do_visiblePanelOthers(sender); control_fc_fabric
            OpenChildControl(control_fc_fabric, sender);
            control_fc_fabric.load_fabrics();
        }

        private void bunifuSystemTypesButton_Click(object sender, EventArgs e)
        {
            ActivateButton(sender);
            //do_visiblePanelOthers(sender);
            OpenChildControl(control_fc_types, sender);
            control_fc_types.load_types();
        }

        private void bunifuAdditionalEquipmentButton_Click(object sender, EventArgs e)
        {
            ActivateButton(sender);
            //do_visiblePanelOthers(sender);
            OpenChildControl(control_fc_additionalEquipment, sender);
            control_fc_additionalEquipment.load_equipments();
        }

        private void bunifuOthersButton_Click(object sender, EventArgs e)
        {
            ActivateButton(sender);
            //do_visiblePanelOthers(sender);
        }

        private void bunifuConnectDataBaseButton_Click(object sender, EventArgs e)
        {
            AvtivateMainMenuButton(sender);
        }

        //private void do_visiblePanelOthers(object button)
        //{
        //    if ((button as Bunifu.Framework.UI.BunifuFlatButton).Text == "Інше")
        //    {
        //        if (!panelOthers.Visible)
        //        {
        //            panelOthers.Visible = true;
        //            panelOthers.BringToFront();
        //            panelOthers.Show();
        //        }
        //        else
        //        {
        //            panelOthers.Visible = false;
        //            panelOthers.Hide();
        //        }
        //    }
        //    else
        //        if(panelOthers.Visible)
        //        panelOthers.Visible = false;
        //}

        private void timerForIconSettingsAndLabel_Tick(object sender, EventArgs e)
        {
            if(!iconIsActive)
            {
                if(iconPictureBoxSettings.IconSize < 60)
                {
                    iconPictureBoxSettings.IconSize += 2;
                }
                else
                {
                    iconIsActive = true;
                    timerForIconSettingsAndLabel.Stop();
                }
            }
            else
            {
                if (iconPictureBoxSettings.IconSize > 50)
                {
                    iconPictureBoxSettings.IconSize -= 2;
                }
                else
                {               
                    iconIsActive = false;
                    timerForIconSettingsAndLabel.Stop();
                }
            }
        }

        private void iconPictureBoxSettings_MouseEnter(object sender, EventArgs e)
        {
            timerForIconSettingsAndLabel.Start();
        }

        private void iconPictureBoxSettings_MouseLeave(object sender, EventArgs e)
        {
            timerForIconSettingsAndLabel.Start();
        }

        private void bunifuFlatButtonMainSettingFabricCurtains_Click(object sender, EventArgs e)
        {
            AvtivateMainMenuButton(sender);
            CloseChildControl();
            openSidePanel(panelSidePanelFabricCurtain);
        }

        private void bunifuFlatButtonMainSettingDay_NightCurtains_Click(object sender, EventArgs e)
        {
            AvtivateMainMenuButton(sender);
            CloseChildControl();
            openSidePanel(panelSidePanelDNC);
        }

        private void bunifuFlatButtonMainSettingProtectiveCurtains_Click(object sender, EventArgs e)
        {
            AvtivateMainMenuButton(sender);
            CloseChildControl();
            openSidePanel(panelSidePanelPC);
        }

        private void bunifuFlatButtonMainSettingRomanCurtains_Click(object sender, EventArgs e)
        {
            AvtivateMainMenuButton(sender);
            CloseChildControl();
            openSidePanel(panelSidePanelRC);
        }

        private void bunifuFlatButtonMainSettingHorisontallJalousie_Click(object sender, EventArgs e)
        {
            AvtivateMainMenuButton(sender);
            CloseChildControl();
            openSidePanel(panelSidePanelHC);
        }

        private void bunifuFlatButtonMainSettingVerticalJalousie_Click(object sender, EventArgs e)
        {
            AvtivateMainMenuButton(sender);
            CloseChildControl();
            openSidePanel(panelSidePanelVC);
        }

        private void bunifuFlatButtonMainSettingMosquitoNets_Click(object sender, EventArgs e)
        {
            AvtivateMainMenuButton(sender);
            CloseChildControl();
            openSidePanel(panelSidePanelMC);
        }

        private void bunifuFlatButtonMainSettingPliseCurtain_Click(object sender, EventArgs e)
        {
            AvtivateMainMenuButton(sender);
            CloseChildControl();
        }

        private void bunifuSubtypesSystemButtonFC_Click(object sender, EventArgs e)
        {
            ActivateButton(sender);
            //do_visiblePanelOthers(sender);
            OpenChildControl(control_fc_subtypes, sender);
            control_fc_subtypes.load_subtypes();
        }

        private void bunifuFlatFabricCategiryButtonFC_Click(object sender, EventArgs e)
        {
            ActivateButton(sender);
            //do_visiblePanelOthers(sender);
            OpenChildControl(control_fc_categories, sender);
            control_fc_categories.load_categories();
        }

        private void bunifuColorDataBaseButton_Click_1(object sender, EventArgs e)
        {
            ActivateButton(sender);
            //do_visiblePanelOthers(sender);
            OpenChildControl(control_colors, sender);
            control_colors.load_colors();
        }

        private void bunifuSideManageButton_Click(object sender, EventArgs e)
        {
            ActivateButton(sender);
            //do_visiblePanelOthers(sender);
            OpenChildControl(control_sidecontrol, sender);
            control_sidecontrol.load_sides();
        }

        private void bunifuInstallationButton_Click(object sender, EventArgs e)
        {
            ActivateButton(sender);
            //do_visiblePanelOthers(sender);
            OpenChildControl(control_installation, sender);
            control_installation.load_installations();
        }

        private void bunifuAdditionalEquipmentButtonDNC_Click(object sender, EventArgs e)
        {
            ActivateButton(sender);
            OpenChildControl(control_dnc_additionalEquipment, sender);
            control_dnc_additionalEquipment.load_equipments();
        }

        private void bunifuFlatFabricCategiryButtonDNC_Click(object sender, EventArgs e)
        {
            ActivateButton(sender);
            OpenChildControl(control_dnc_categories, sender);
            control_dnc_categories.load_categories();
        }

        private void bunifuDNCFabricDataBaseButton_Click(object sender, EventArgs e)
        {
            ActivateButton(sender);
            OpenChildControl(control_dnc_fabric, sender);
            control_dnc_fabric.load_fabrics();
        }

        private void bunifuSubtypesSystemButtonDNC_Click(object sender, EventArgs e)
        {
            ActivateButton(sender);
            OpenChildControl(control_dnc_subtypes, sender);
            control_dnc_subtypes.load_subtypes();
        }

        private void bunifuSystemTypesButtonDNC_Click(object sender, EventArgs e)
        {
            ActivateButton(sender);
            OpenChildControl(control_dnc_types, sender);
            control_dnc_types.load_types();
        }

        private void bunifuAdditionalEquipmentButtonPC_Click(object sender, EventArgs e)
        {
            ActivateButton(sender);
            OpenChildControl(control_pc_additionalEquipment, sender);
            control_pc_additionalEquipment.load_equipments();
        }

        private void bunifuFlatFabricCategiryButtonPC_Click(object sender, EventArgs e)
        {
            ActivateButton(sender);
            OpenChildControl(control_pc_categories, sender);
            control_pc_categories.load_categories();
        }

        private void bunifuAdditionalEquipmentButtonRC_Click(object sender, EventArgs e)
        {
            ActivateButton(sender);
            OpenChildControl(control_rc_additionalEquipment, sender);
            control_rc_additionalEquipment.load_equipments();
        }

        private void bunifuFlatFabricCategiryButtonRC_Click(object sender, EventArgs e)
        {
            ActivateButton(sender);
            OpenChildControl(control_rc_categories, sender);
            control_rc_categories.load_categories();
        }

        private void bunifuRCFabricDataBaseButton_Click(object sender, EventArgs e)
        {
            ActivateButton(sender);
            OpenChildControl(control_rc_fabric, sender);
            control_rc_fabric.load_fabrics();
        }

        private void bunifuAdditionalEquipmentButtonMC_Click(object sender, EventArgs e)
        {
            ActivateButton(sender);
            OpenChildControl(control_mc_additionalEquipment, sender);
            control_mc_additionalEquipment.load_equipments();
        }

        private void bunifuSystemTypesButtonMC_Click(object sender, EventArgs e)
        {
            ActivateButton(sender);
            OpenChildControl(control_mc_types, sender);
            control_mc_types.load_types();
        }

        private void bunifuSystemTypesButtonHC_Click(object sender, EventArgs e)
        {
            ActivateButton(sender);
            OpenChildControl(control_hc_types, sender);
            control_hc_types.load_types();
        }

        private void bunifuAdditionalEquipmentButtonHC_Click(object sender, EventArgs e)
        {
            ActivateButton(sender);
            OpenChildControl(control_hc_additionalEquipment, sender);
            control_hc_additionalEquipment.load_equipments();
        }

        private void bunifuAdditionalEquipmentButtonVC_Click(object sender, EventArgs e)
        {
            ActivateButton(sender);
            OpenChildControl(control_vc_additionalEquipment, sender);
            control_vc_additionalEquipment.load_equipments();
        }

        private void bunifuInstallationButtonVC_Click(object sender, EventArgs e)
        {
            ActivateButton(sender);
            OpenChildControl(control_vc_sidecontrol, sender);
            control_vc_sidecontrol.load_sides();
        }

        private void bunifuFlatButton2_Click(object sender, EventArgs e)
        {
            ActivateButton(sender);
            OpenChildControl(control_vc_types, sender);
            control_vc_types.load_types();
        }

        private void bunifuVCFabricDataBaseButton_Click(object sender, EventArgs e)
        {
            ActivateButton(sender);
            OpenChildControl(control_vc_fabrics, sender);
            control_vc_fabrics.load_fabrics();
        }
    }
}
