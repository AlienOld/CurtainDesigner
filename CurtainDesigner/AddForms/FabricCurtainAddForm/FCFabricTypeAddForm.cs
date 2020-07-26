﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CurtainDesigner.AddForms.FabricCurtainAddForm
{
    public partial class FCFabricTypeAddForm : Form
    {
        private static string connect_str = CurtainDesigner.Classes.ConnectionString.conn;
        private SqlConnection connection;
        private bool isEditType;
        private string type_id;

        public FCFabricTypeAddForm()
        {
            InitializeComponent();
            tooltips();
        }

        public FCFabricTypeAddForm(string id, string type_name)
        {
            InitializeComponent();
            labelFormName.Text = "  Редагування";
            tooltips();
            isEditType = true;
            type_id = id;
            bunifuMaterialTextboxTypeName.Text = type_name;
        }

        public FCFabricTypeAddForm(string id, UserControl control)
        {
            InitializeComponent();
            removeType(id, control);
        }

        private void iconButtonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void tooltips()
        {
            ToolTip tip = new ToolTip();
            tip.SetToolTip(buttonDrag, "Перетягнути вікно");
        }

        private async void iconButtonOk_Click(object sender, EventArgs e)
        {
            if (checkIsEmpty())
            {
                MessageBox.Show("Одне з полів не заповнено.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                if (!isEditType)
                {
                    bool send = await Task.Run(() => sendNewType(bunifuMaterialTextboxTypeName.Text));
                    if (send)
                    {
                        MessageBox.Show("Новий тип успішно додан до системи.", "Ok", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.OK;
                    }
                    else
                        MessageBox.Show("Помилка при додаванні нового типу.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    bool send = await Task.Run(() => editType(this.type_id, bunifuMaterialTextboxTypeName.Text));
                    if (send)
                    {
                        MessageBox.Show("Тип успішно відредагован.", "Ok", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.OK;
                    }
                    else
                        MessageBox.Show("Помилка при редагуванні типу.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                this.Close();
            }
        }

        private bool checkIsEmpty()
        {
            if (string.IsNullOrWhiteSpace(bunifuMaterialTextboxTypeName.Text))
                return true;
            return false;
        }

        private async void removeType(string id, UserControl control)
        {
            bool send = await Task.Run(() => deleteType(id));
            if (send)
            {
                MessageBox.Show("Тип успішно видалений.", "Ok", MessageBoxButtons.OK, MessageBoxIcon.Information);
                (control as UserControls.UCSettingsFabricCurtain.UserControlCurt_TypeFC).load_types();
            }
            else
                MessageBox.Show("Помилка при видаленні типу.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private async Task<bool> deleteType(string id)
        {
            bool sended = true;

            if (connection == null || connection.State == ConnectionState.Closed)
            {
                connection = new SqlConnection(connect_str);
                await connection.OpenAsync();
            }

            SqlCommand sqlCommand = new SqlCommand($"Delete From [Fabric_curtains_types] Where [Fabric_curtains_types].[Type_id] = {id};", connection);

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

        private async Task<bool> sendNewType(string type_name)
        {
            bool sended = true;

            if (connection == null || connection.State == ConnectionState.Closed)
            {
                connection = new SqlConnection(connect_str);
                await connection.OpenAsync();
            }

            SqlCommand sqlCommand = new SqlCommand($"Insert Into [Fabric_curtains_types] Values (N'{type_name}');", connection);

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

        private async Task<bool> editType(string id, string type_name)
        {
            bool sended = true;

            if (connection == null || connection.State == ConnectionState.Closed)
            {
                connection = new SqlConnection(connect_str);
                await connection.OpenAsync();
            }

            SqlCommand sqlCommand = new SqlCommand($"Update [Fabric_curtains_types] Set [Fabric_curtains_types].[Type_name] = N'{type_name}' Where [Fabric_curtains_types].[Type_id] = {id};", connection);

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
    }
}