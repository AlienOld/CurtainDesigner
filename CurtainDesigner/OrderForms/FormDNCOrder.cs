﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ImageMagick;
using ImageMagick.Configuration;
using ImageMagick.Defines;
using ImageMagick.ImageOptimizers;

namespace CurtainDesigner.OrderForms
{
    public partial class FormDNCOrder : Form
    {
        private static string connect_str = CurtainDesigner.Classes.ConnectionString.conn;
        private SqlConnection connection;
        internal ToolTip tip;
        private bool processing;
        private bool img_processing;

        public FormDNCOrder()
        {
            InitializeComponent();

            processing = true;
            img_processing = false;
            this.tip = new ToolTip();
            tip.UseAnimation = true;
            tip.UseFading = true;

            tip.SetToolTip(bunifuButtonCustomInstallPrice, "Зберегти ціну в БД");

            LoadDataFromDb();
            comboBoxCurtainType.SelectionChangeCommitted += new EventHandler(loadSubtypesAndAdditional);
            comboBoxCurtainSubtype.SelectionChangeCommitted += new EventHandler(loadNextDataFromDB);
            comboBoxFabric.SelectionChangeCommitted += new EventHandler(loadFabricCategoryFromBD);

            comboBoxSide.SelectionChangeCommitted += (sender, e) => { setToolTip((Control)sender, comboBoxSide.SelectedItem.ToString().Split(new char[] { '[', ',', ']' }, StringSplitOptions.None)[1]); update_draw(numericUpDownWidth.Value.ToString() + "м.", numericUpDownHeight.Value.ToString() + "м.", labelYardage.Text + "м."); };
            comboBoxSystemColor.SelectionChangeCommitted += (sender, e) => { setToolTip((Control)sender, comboBoxSystemColor.SelectedItem.ToString().Split(new char[] { '[', ',', ']' }, StringSplitOptions.None)[1]); };
            comboBoxEquipment.SelectionChangeCommitted += (sender, e) => { setToolTip((Control)sender, comboBoxEquipment.SelectedItem.ToString().Split(new char[] { '[', ',', ']' }, StringSplitOptions.None)[1]); };

            bunifuImageButtonSeeClientDetail.Click += (sender, e) => { if (!string.IsNullOrEmpty(labelCustomer.Text)) MessageBox.Show(tip.GetToolTip(labelCustomer), "Info", MessageBoxButtons.OK, MessageBoxIcon.Information); };

            numericUpDownWidth.ValueChanged += (sender, e) => { labelYardage.Text = string.Join(",", Convert.ToString((float)Math.Round(Convert.ToDouble(numericUpDownWidth.Value * numericUpDownHeight.Value), 2, MidpointRounding.AwayFromZero)).Split(',')); setTooltipsyardage(); update_draw(numericUpDownWidth.Value.ToString() + "м.", numericUpDownHeight.Value.ToString() + "м.", labelYardage.Text + "м."); };
            numericUpDownHeight.ValueChanged += (sender, e) => { labelYardage.Text = string.Join(",", Convert.ToString((float)Math.Round(Convert.ToDouble(numericUpDownWidth.Value * numericUpDownHeight.Value), 2, MidpointRounding.AwayFromZero)).Split(',')); setTooltipsyardage(); update_draw(numericUpDownWidth.Value.ToString() + "м.", numericUpDownHeight.Value.ToString() + "м.", labelYardage.Text + "м."); };

            setTooltipsyardage();

            labelFabricCategory.TextChanged += (sender, e) => { updatePrice(); };
            comboBoxEquipment.SelectedValueChanged += (sender, e) => { updatePrice(); };
            labelYardage.TextChanged += (sender, e) => { updatePrice(); };
            comboBoxInstallation.SelectedValueChanged += (sender, e) => { updatePrice(); };
            numericUpDownCount.ValueChanged += (sender, e) => { updatePrice(); };

            processing = false;

            update_draw(numericUpDownWidth.Value.ToString() + "м.", numericUpDownHeight.Value.ToString() + "м.", labelYardage.Text + "м.");
        }

        private void create_img_id()
        {
            int random = 0;
            string path = Classes.PathCombiner.join_combine("\\");
            Random num = new Random();
            random = num.Next();
            path = string.Join("", path, "draw_images\\dnc\\print\\", random.ToString(), ".png");
            while (File.Exists(path))
            {
                random = num.Next();
                path = string.Join("", path, "draw_images\\dnc\\print\\", random.ToString(), ".png");
            }
            if (File.Exists(Classes.PathCombiner.join_combine("\\draw_images\\dnc\\draw.png")))
            {
                File.Copy(Classes.PathCombiner.join_combine("\\draw_images\\dnc\\draw.png"), path, true);
                label_img_id.Text = Classes.PathCombiner.combine(path);
            }
        }

        private void update_draw(string width, string height, string yardage)
        {
            if (img_processing)
            {
                MessageBox.Show("Зачекайте, будь ласка, програма створює креслення.", "Please, wait..", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            img_processing = true;

            try
            {
                string selected_side = "";

                if (comboBoxSide.DataSource == null || comboBoxSide.Items.Count == 0 || comboBoxSide.SelectedValue == null)
                    selected_side = "";
                else
                    selected_side = (comboBoxSide.DataSource as BindingList<KeyValuePair<string, int>>).Where(x => x.Value == Convert.ToInt32(comboBoxSide.SelectedValue)).Select(x => x.Key).Single();

                Thread set_img = new Thread(new ParameterizedThreadStart(get_and_set_img));
                set_img.Start(new string[] { width, yardage, height, selected_side });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка при спробі обробки та завантаження малюнку: \n\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void get_and_set_img(object values)
        {
            string[] vls = (string[])values;
            if (vls == null || vls.Length < 4)
                return;

            MagickColor font_color = new MagickColor(MagickColors.Black);
            MagickColor back_color = new MagickColor(MagickColors.Transparent);

            Classes.MagicImage.MagicLabels labels = new Classes.MagicImage.MagicLabels();
            labels.add_label(font_color, back_color, "Arial", 120, 70, $"label:{vls[0]}");
            labels.add_label(font_color, back_color, "Arial", 120, 70, $"label:{vls[1]}");
            labels.add_label(font_color, back_color, "Arial", 120, 70, $"label:{vls[2]}");

            List<int> coordinates = new List<int>()
            {
                600,
                50,
                575,
                525,
                90,
                600
            };

            try
            {
                Bitmap bitmap = null;

                if ((vls[3].Contains("лів") || vls[3].Contains("Лів")) && (vls[3].Contains("прав") || vls[3].Contains("Прав")))
                    bitmap = Classes.MagicImage.ClassMagicImage.create_img(labels.getList, coordinates, Classes.PathCombiner.join_combine("\\draw_images\\dnc\\dnc_left_and_right_side.png"), -45, -90, "dnc");
                else if (vls[3].Contains("прав") || vls[3].Contains("Прав"))
                    bitmap = Classes.MagicImage.ClassMagicImage.create_img(labels.getList, coordinates, Classes.PathCombiner.join_combine("\\draw_images\\dnc\\dnc_right_side.png"), -45, -90, "dnc");
                else if (vls[3].Contains("лів") || vls[3].Contains("Лів"))
                    bitmap = Classes.MagicImage.ClassMagicImage.create_img(labels.getList, coordinates, Classes.PathCombiner.join_combine("\\draw_images\\dnc\\dnc_left_side.png"), -45, -90, "dnc");
                else
                    bitmap = Classes.MagicImage.ClassMagicImage.create_img(labels.getList, coordinates, Classes.PathCombiner.join_combine("\\draw_images\\dnc\\dnc_right_side.png"), -45, -90, "dnc");

                pictureBoxMainPicture.Invoke((MethodInvoker)delegate {
                    pictureBoxMainPicture.Image = bitmap;
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка при спробі обробки та завантаження малюнку: \n\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                img_processing = false;
            }
        }

        private async void updatePrice()
        {
            if (processing)
                return;
            processing = true;

            if (labelFabricCategory.Text != "0$" &&
                !string.IsNullOrEmpty(labelFabricCategory.Text) &&
                comboBoxEquipment.DataSource != null &&
                comboBoxEquipment.SelectedValue != null &&
                labelYardage.Text != "0" &&
                !string.IsNullOrEmpty(labelYardage.Text) &&
                numericUpDownCount.Value != 0 &&
                comboBoxInstallation.DataSource != null &&
                comboBoxInstallation.SelectedValue != null &&
                comboBoxFabric.DataSource != null &&
                comboBoxFabric.SelectedValue != null)
            {
                double yardage = 0;
                double categoryPrice = 0;
                double installation = 0;
                double equipment_price = 0;
                int good_count = 0;

                try
                {
                    equipment_price = await getEquipmentPrice(comboBoxEquipment.SelectedValue.ToString());
                    installation = Convert.ToDouble((comboBoxInstallation.DataSource as BindingList<KeyValuePair<string, int>>).Where(x => x.Value == Convert.ToInt32(comboBoxInstallation.SelectedValue)).Select(x => x.Key).Single());
                    yardage = Convert.ToDouble(labelYardage.Text);
                    categoryPrice = Convert.ToDouble(labelFabricCategory.Text.Split('$')[0]);
                    good_count = Convert.ToInt32(numericUpDownCount.Value);

                    double allPrice = Convert.ToDouble((yardage * categoryPrice + installation + equipment_price) * good_count);

                    labelPrice.Text = string.Join(" ", "[", (Math.Round(allPrice, 2, MidpointRounding.AwayFromZero)).ToString(), "$", "]");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Помилка при спробі розрахунку ціни, подробиці: \n\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    labelPrice.Text = "[ 0 $ ]";
                }
                finally
                {
                    processing = false;
                }
            }
            else
            {
                labelPrice.Text = "[ 0 $ ]";
            }

            processing = false;
        }

        private async Task<double> getEquipmentPrice(string Equipment_id)
        {
            double price = 0;

            if (connection == null || connection.State == ConnectionState.Closed)
            {
                connection = new SqlConnection(connect_str);
                await connection.OpenAsync();
            }

            SqlCommand sqlCommand = new SqlCommand($"Select Price From [DNC_Additional_equipment] Where Equipment_id = {Equipment_id};", connection);
            SqlDataReader reader = null;

            try
            {
                reader = await sqlCommand.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    price = Convert.ToDouble(reader["Price"]);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (!reader.IsClosed && reader != null)
                    reader.Close();
                connection.Close();
            }

            return price;
        }

        private void setTooltipsyardage()
        {
            tip.SetToolTip(numericUpDownWidth, string.Join(" ", numericUpDownWidth.Value.ToString(), "м"));
            tip.SetToolTip(numericUpDownHeight, string.Join(" ", numericUpDownHeight.Value.ToString(), "м"));
            tip.SetToolTip(labelYardage, string.Join(" ", labelYardage.Text, "м2"));
        }

        private void iconButtonNewOrder_Click(object sender, EventArgs e)
        {
            #region [Check information before create order]
            if (!File.Exists(Classes.PathCombiner.join_combine("\\draw_images\\dnc\\draw.png")))
            {
                MessageBox.Show("Не вдалось знайти креслення, спробуйте ще раз.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (comboBoxCurtainType.DataSource == null || comboBoxCurtainType.Items.Count == 0 || comboBoxCurtainType.SelectedValue == null)
            {
                MessageBox.Show("Поле \"Тип системи\" не заповнено!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (comboBoxCurtainSubtype.DataSource == null || comboBoxCurtainSubtype.Items.Count == 0 || comboBoxCurtainSubtype.SelectedValue == null)
            {
                MessageBox.Show("Поле \"Підтип системи\" не заповнено!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (comboBoxSide.DataSource == null || comboBoxSide.Items.Count == 0 || comboBoxSide.SelectedValue == null)
            {
                MessageBox.Show("Поле \"Керування\" не заповнено!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (comboBoxFabric.DataSource == null || comboBoxFabric.Items.Count == 0 || comboBoxFabric.SelectedValue == null)
            {
                MessageBox.Show("Поле \"Тканина\" не заповнено!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(labelFabricCategory.Text) || labelFabricCategory.Text == "0$" || string.IsNullOrEmpty(labelFabricCategoryId.Text))
            {
                MessageBox.Show("Поле \"Цінова категорія тканини\" не заповнено!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (comboBoxSystemColor.DataSource == null || comboBoxSystemColor.Items.Count == 0 || comboBoxSystemColor.SelectedValue == null)
            {
                MessageBox.Show("Поле \"Колір системи\" не заповнено!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (comboBoxEquipment.DataSource == null || comboBoxEquipment.Items.Count == 0 || comboBoxEquipment.SelectedValue == null)
            {
                MessageBox.Show("Поле \"Додаткова комплектація\" не заповнено!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(labelYardage.Text) || labelYardage.Text == "0")
            {
                MessageBox.Show("Поле \"Квадратура(площа)\" не заповнено!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (comboBoxInstallation.DataSource == null || comboBoxInstallation.Items.Count == 0 || comboBoxInstallation.SelectedValue == null)
            {
                MessageBox.Show("Поле \"Ціна встановлення\" не заповнено!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (numericUpDownCount.Value == 0)
            {
                MessageBox.Show("Поле \"Кількість\" не заповнено!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(labelCustomer.Text) || string.IsNullOrWhiteSpace(labelCustomer.Text))
            {
                MessageBox.Show("Поле \"Замовник\" не заповнено!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(labelPrice.Text) || labelPrice.Text == "[ 0 $ ]")
            {
                MessageBox.Show("Поле \"Ціна\" не заповнено!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            #endregion

            create_img_id();
            CurtainDesigner.Controllers.IControlerManage<Classes.DNC, List<Classes.DNC2>, FormDNCOrder, DataGridView> controler = new CurtainDesigner.Controllers.Classes.DNCControlerManager<Classes.DNC, List<Classes.DNC2>, FormDNCOrder, DataGridView>();
            controler.packing(new Classes.DNC(), new List<Classes.DNC2>(), this);
            MessageBox.Show("Замовлення успішно створено.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private async void LoadDataFromDb()
        {
            CurtainDesigner.Controllers.Classes.DNCControlerManager<Classes.DNC, List<Classes.DNC2>, FormDNCOrder, DataGridView> controler = new CurtainDesigner.Controllers.Classes.DNCControlerManager<Classes.DNC, List<Classes.DNC2>, FormDNCOrder, DataGridView>();
            await Task.Run(() => controler.load_data(this));
        }

        private async void loadSubtypesAndAdditional(object sender, EventArgs e)
        {
            comboBoxCurtainSubtype.DataSource = null;
            comboBoxCurtainSubtype.Invalidate();
            comboBoxEquipment.DataSource = null;
            comboBoxEquipment.Invalidate();
            comboBoxFabric.DataSource = null;
            comboBoxFabric.Invalidate();
            labelFabricCategory.Text = "0$";
            labelFabricCategoryId.Text = "0";
            setToolTip((Control)sender, comboBoxCurtainType.SelectedItem.ToString().Split(new char[] { '[', ',', ']' }, StringSplitOptions.None)[1]);
            string boxCurtainType = "";
            if (comboBoxCurtainType.InvokeRequired)
                comboBoxCurtainType.Invoke((MethodInvoker)delegate
                {
                    boxCurtainType = comboBoxCurtainType.SelectedValue.ToString();
                });
            else
                boxCurtainType = comboBoxCurtainType.SelectedValue.ToString();

            CurtainDesigner.Controllers.Classes.DNCControlerManager<Classes.DNC, List<Classes.DNC2>, FormDNCOrder, DataGridView> controler = new CurtainDesigner.Controllers.Classes.DNCControlerManager<Classes.DNC, List<Classes.DNC2>, FormDNCOrder, DataGridView>();
            Task t1 = Task.Run(() => controler.load_subtypeAndAdditionall(this, boxCurtainType));
            comboBoxCurtainSubtype.Enabled = true;
            comboBoxEquipment.Enabled = true;
            await Task.WhenAll(t1);
            //update_status(sender, e);
            //update_status(comboBoxEquipment, e);
        }

        private async void loadNextDataFromDB(object sender, EventArgs e)
        {
            comboBoxFabric.DataSource = null;
            comboBoxFabric.Invalidate();
            labelFabricCategory.Text = "0$";
            labelFabricCategoryId.Text = "0";
            setToolTip((Control)sender, comboBoxCurtainSubtype.SelectedItem.ToString().Split(new char[] { '[', ',', ']' }, StringSplitOptions.None)[1]);
            string boxCurtainType = "";
            if (comboBoxCurtainType.InvokeRequired)
                comboBoxCurtainType.Invoke((MethodInvoker)delegate
                {
                    boxCurtainType = comboBoxCurtainType.SelectedValue.ToString();
                });
            else
                boxCurtainType = comboBoxCurtainType.SelectedValue.ToString();

            string boxCurtainSubType = "";
            if (comboBoxCurtainSubtype.InvokeRequired)
                comboBoxCurtainSubtype.Invoke((MethodInvoker)delegate
                {
                    boxCurtainSubType = comboBoxCurtainSubtype.SelectedValue.ToString();
                });
            else
                boxCurtainSubType = comboBoxCurtainSubtype.SelectedValue.ToString();

            CurtainDesigner.Controllers.Classes.DNCControlerManager<Classes.DNC, List<Classes.DNC2>, FormDNCOrder, DataGridView> controler = new CurtainDesigner.Controllers.Classes.DNCControlerManager<Classes.DNC, List<Classes.DNC2>, FormDNCOrder, DataGridView>();
            Task t1 = Task.Run(() => controler.load_data(this, boxCurtainType, boxCurtainSubType));
            comboBoxFabric.Enabled = true;
            await Task.WhenAll(t1);
            //update_status(sender, e);
            //update_status(comboBoxFabric, e);
        }

        private async void loadFabricCategoryFromBD(object sender, EventArgs e)
        {
            setToolTip((Control)sender, comboBoxFabric.SelectedItem.ToString().Split(new char[] { '[', ',', ']' }, StringSplitOptions.None)[1]);
            string boxCurtainType = "";
            if (comboBoxCurtainType.InvokeRequired)
                comboBoxCurtainType.Invoke((MethodInvoker)delegate
                {
                    boxCurtainType = comboBoxCurtainType.SelectedValue.ToString();
                });
            else
                boxCurtainType = comboBoxCurtainType.SelectedValue.ToString();

            string boxCurtainSubType = "";
            if (comboBoxCurtainSubtype.InvokeRequired)
                comboBoxCurtainSubtype.Invoke((MethodInvoker)delegate
                {
                    boxCurtainSubType = comboBoxCurtainSubtype.SelectedValue.ToString();
                });
            else
                boxCurtainSubType = comboBoxCurtainSubtype.SelectedValue.ToString();

            string boxCurtainFabric = "";
            if (comboBoxFabric.InvokeRequired)
                comboBoxFabric.Invoke((MethodInvoker)delegate
                {
                    boxCurtainFabric = comboBoxFabric.SelectedValue.ToString();
                });
            else
                boxCurtainFabric = comboBoxFabric.SelectedValue.ToString();

            CurtainDesigner.Controllers.Classes.DNCControlerManager<Classes.DNC, List<Classes.DNC2>, FormDNCOrder, DataGridView> controler = new CurtainDesigner.Controllers.Classes.DNCControlerManager<Classes.DNC, List<Classes.DNC2>, FormDNCOrder, DataGridView>();
            Task t1 = Task.Run(() => controler.load_FabricCategorydata(this, boxCurtainType, boxCurtainFabric, boxCurtainSubType));
            await Task.WhenAll(t1);
            //update_status(sender, e);
        }

        private void bunifuCheckboxCustomInstallation_OnChange(object sender, EventArgs e)
        {
            if (bunifuCheckboxCustomInstallation.Checked == true)
            {
                numericUpDownCustomInstallationPrice.Enabled = true;
                comboBoxInstallation.Enabled = false;
                bunifuButtonCustomInstallPrice.Enabled = true;
            }
            else
            {
                numericUpDownCustomInstallationPrice.Enabled = false;
                comboBoxInstallation.Enabled = true;
                bunifuButtonCustomInstallPrice.Enabled = false;
            }
        }

        private void setToolTip(Control control, string tooltip)
        {
            if (control.GetType().Name == "ComboBox")
            {
                tip.SetToolTip(control, tooltip);
            }
        }

        private async void bunifuButtonCustomInstallPrice_Click(object sender, EventArgs e)
        {
            comboBoxInstallation.DataSource = null;
            decimal numericPrice = 0;

            if (numericUpDownCustomInstallationPrice.InvokeRequired)
                numericUpDownCustomInstallationPrice.Invoke((MethodInvoker)delegate
                {
                    numericPrice = numericUpDownCustomInstallationPrice.Value;
                });
            else
                numericPrice = numericUpDownCustomInstallationPrice.Value;

            bool send = await Task.Run(() => sendNewInstallation("Особиста ціна", numericPrice));
            if (!send)
            {
                MessageBox.Show("Помилка при додаванні нового встановлення.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            load_installations();

            numericUpDownCustomInstallationPrice.Value = 0;
            numericUpDownCustomInstallationPrice.Enabled = false;
            bunifuButtonCustomInstallPrice.Enabled = false;
            comboBoxInstallation.Enabled = true;
            bunifuCheckboxCustomInstallation.Checked = false;

            //update_status(comboBoxInstallation, e);
        }

        private async Task<bool> sendNewInstallation(string name, decimal price)
        {
            bool sended = true;

            if (connection == null || connection.State == ConnectionState.Closed)
            {
                connection = new SqlConnection(connect_str);
                await connection.OpenAsync();
            }

            SqlCommand sqlCommand = new SqlCommand($"Insert Into [Installation] Values (N'{name}', {string.Join(".", Convert.ToString((float)Math.Round(Convert.ToDouble(price), 2, MidpointRounding.AwayFromZero)).Split(','))});", connection);

            try
            {
                await sqlCommand.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                sended = false;
            }
            finally
            {
                connection.Close();
            }

            return sended;
        }

        private async void load_installations()
        {
            CurtainDesigner.Controllers.Classes.DNCControlerManager<Classes.DNC, List<Classes.DNC2>, FormDNCOrder, DataGridView> controler = new CurtainDesigner.Controllers.Classes.DNCControlerManager<Classes.DNC, List<Classes.DNC2>, FormDNCOrder, DataGridView>();
            await Task.Run(() => controler.load_installations(this));
        }

        private void iconButtonSelectClient_Click(object sender, EventArgs e)
        {
            OrderForms.OrderFormSelectClient.FormSelectClient formSelectClient = new OrderForms.OrderFormSelectClient.FormSelectClient(this.labelCustomer, this.tip);
            formSelectClient.ShowDialog();
        }

        private void iconButtonCreateNewClientAndSelect_Click(object sender, EventArgs e)
        {
            AddForms.FormAddNewClient newClient = new AddForms.FormAddNewClient();
            newClient.DialogResult = DialogResult.None;
            newClient.ShowDialog();
            if (newClient.DialogResult == DialogResult.OK)
                load_Newclient();
        }

        internal async void load_Newclient()
        {
            await Task.Run(() => load_data());
        }

        private async void load_data()
        {
            if (connection == null || connection.State == ConnectionState.Closed)
            {
                connection = new SqlConnection(connect_str);
                await connection.OpenAsync();
            }

            SqlCommand command_loadclient = new SqlCommand("Select Customer_id, Name, Surname From [Clients] Where Customer_id = (Select Max(Customer_id) From [Clients]);", connection);

            SqlDataReader reader = null;

            try
            {
                reader = await command_loadclient.ExecuteReaderAsync();
                if (labelCustomer.InvokeRequired)
                {
                    labelCustomer.Invoke((MethodInvoker)async delegate
                    {
                        while (await reader.ReadAsync())
                        {
                            labelCustomer.Text = reader["Customer_id"].ToString();
                            tip.SetToolTip(labelCustomer, string.Join(" ", reader["Surname"].ToString(), reader["Name"].ToString(), reader["Customer_id"].ToString()));
                        }
                    });

                }
                else
                {
                    while (await reader.ReadAsync())
                    {
                        labelCustomer.Text = reader["Customer_id"].ToString();
                        tip.SetToolTip(labelCustomer, string.Join(" ", reader["Surname"].ToString(), reader["Name"].ToString(), reader["Customer_id"].ToString()));
                    }
                }
            }
            catch (Exception exeption)
            {
                MessageBox.Show(exeption.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (reader != null && !reader.IsClosed)
                    reader.Close();
                connection.Close();
            }
        }
    }
}
