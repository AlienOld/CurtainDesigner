﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CurtainDesigner.Models.Interfaces;

namespace CurtainDesigner.Models.Classes
{
    class DNCManage<L> : CurtainDesigner.Models.Interfaces.IObjectManage<L>
    {
        private static string connect_str = CurtainDesigner.Classes.ConnectionString.conn;
        private CurtainDesigner.Classes.DNC dnc;
        private SqlConnection connection;

        public Task<bool> editObject()
        {
            throw new NotImplementedException();
        }

        public void closeConnection() => connection.Close();

        async public Task<SqlDataReader> getDataFromDB(string command)
        {
            if (command == null)
                throw new NullReferenceException();

            if (connection == null || connection.State == ConnectionState.Closed)
            {
                connection = new SqlConnection(connect_str);
                await connection.OpenAsync();
            }

            SqlDataReader reader = null;
            SqlCommand sqlCommand = new SqlCommand(command, connection);
            try
            {
                reader = await sqlCommand.ExecuteReaderAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return reader;
        }

        public L readObjects(L list)
        {
            if (list == null)
                throw new NullReferenceException();
            else
                (list as List<CurtainDesigner.Classes.DNC2>).Clear();

            if (connection == null || connection.State == ConnectionState.Closed)
            {
                connection = new SqlConnection(connect_str);
                connection.Open();
            }

            SqlCommand command_get_dn_curtain = new SqlCommand("Select * From [DayNight_curtains] order by Curtain_id desc;", connection);


            SqlDataReader reader_dn_curtain = null;
            SqlDataReader reader_type = null;
            SqlDataReader reader_subtype = null;
            SqlDataReader reader_fabric_category = null;
            SqlDataReader reader_other = null;
            CurtainDesigner.Classes.DNC2 obj = null;

            try
            {
                reader_dn_curtain = command_get_dn_curtain.ExecuteReader();
                while (reader_dn_curtain.Read())
                {
                    obj = new CurtainDesigner.Classes.DNC2();

                    //get id
                    obj.fb_id = reader_dn_curtain["Curtain_id"].ToString();

                    //Get start informations
                    obj.fabric_id = reader_dn_curtain["Fabric_id"].ToString();
                    obj.subtype_id = reader_dn_curtain["Subtype_id"].ToString();
                    obj.type_id = reader_dn_curtain["Type_id"].ToString();
                    obj.fabric_category_id = reader_dn_curtain["Category_id"].ToString();
                    obj.system_color_id = reader_dn_curtain["Color_id"].ToString();
                    obj.side_id = reader_dn_curtain["Control_id"].ToString();
                    obj.equipment_id = reader_dn_curtain["Equipment_id"].ToString();
                    obj.installation_id = reader_dn_curtain["Installation_id"].ToString();
                    obj.customer_id = reader_dn_curtain["Customer_id"].ToString();
                    //Get dates, image, price
                    obj.start_order_time = Convert.ToDateTime(reader_dn_curtain["Order_data"]);
                    obj.end_order_time = Convert.ToDateTime(reader_dn_curtain["End_date"]);
                    obj.picture = (reader_dn_curtain["Drawing"]).ToString();
                    obj.price = (float)Convert.ToDouble(reader_dn_curtain["Price"]);
                    //Get sizes and count
                    obj.width = (float)Convert.ToDouble(reader_dn_curtain["Width"]);
                    obj.height = (float)Convert.ToDouble(reader_dn_curtain["Height"]);
                    obj.yardage = (float)Convert.ToDouble(reader_dn_curtain["Yardage"]);
                    obj.count = Convert.ToInt32(reader_dn_curtain["Count"]);

                    //get type
                    obj.type = get_type(obj.type_id.ToString(), reader_type);

                    //get subtype
                    obj.subtype = get_subtype(obj.subtype_id.ToString(), reader_subtype);

                    //get fabric
                    obj.fabric_name = get_fabrc(obj.fabric_id.ToString(), reader_other);

                    //Get Fabric Category
                    string[] category = get_category(obj.fabric_category_id, reader_fabric_category);
                    obj.fabric_category_name = category[0];
                    obj.fabric_category_price = (float)Convert.ToDouble(category[1]);

                    //Get system_color
                    obj.system_color_name = get_system_color(obj.system_color_id, reader_other);

                    //Get control_side
                    obj.side_name = get_side(obj.side_id, reader_other);

                    //Get equipment
                    obj.equipment_price = (float)Convert.ToDouble(get_equipment(obj.equipment_id, reader_other));

                    //Get Installation
                    obj.installation_price = (float)Convert.ToDouble(get_installation(obj.installation_id, reader_other));


                    //add to list
                    (list as List<CurtainDesigner.Classes.DNC2>).Add(obj);
                }

                if (reader_dn_curtain != null && !reader_dn_curtain.IsClosed)
                    reader_dn_curtain.Close();
            }
            catch (Exception exeption)
            {
                MessageBox.Show(exeption.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                connection.Close();
            }
            return list;
        }

        async public Task<bool> writeObject(object obj)
        {
            if (obj != null)
                dnc = (CurtainDesigner.Classes.DNC)obj;
            else
            {
                throw new NullReferenceException();
            }

            if (connection == null || connection.State == ConnectionState.Closed)
            {
                connection = new SqlConnection(connect_str);
                await connection.OpenAsync();
            }

            try
            {

                SqlCommand command_addFB = new SqlCommand($"Insert Into [DayNight_curtains] Values ({dnc.fabric_id}, " +
                                                                                                    $"{dnc.width}, " +
                                                                                                    $"{dnc.height}, " +
                                                                                                    $"{dnc.count}, " +
                                                                                                    $"{dnc.yardage}, " +
                                                                                                    $"{dnc.side_id}, " +
                                                                                                    $"{dnc.equipment_id}, " +
                                                                                                    $"{dnc.customer_id}, " +
                                                                                                    $"{dnc.system_color_id}, " +
                                                                                                    $"@start_date, " +
                                                                                                    $"@end_date, " +
                                                                                                    $"{dnc.type_id}, " +
                                                                                                    $"@image, " +
                                                                                                    $"{dnc.price}, " +
                                                                                                    $"{dnc.installation_id}, " +
                                                                                                    $"{dnc.subtype_id}, " +
                                                                                                    $"{dnc.category_id});", connection);

                try
                {
                    command_addFB.Parameters.Add("@image", SqlDbType.NVarChar).Value = dnc.picture;
                    command_addFB.Parameters.Add("@start_date", SqlDbType.DateTime2).Value = dnc.start_order_time;
                    command_addFB.Parameters.Add("@end_date", SqlDbType.DateTime2).Value = dnc.end_order_time;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Помилка при конвертаціії дати або чертежа замовлення, подрбиці: \n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                await command_addFB.ExecuteNonQueryAsync();
            }
            catch (Exception exeption)
            {
                MessageBox.Show(exeption.Message, "!!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            finally
            {
                connection.Close();
            }
            return true;
        }

        private string get_type(string type_id, SqlDataReader reader)
        {
            string res = "";

            SqlCommand command = new SqlCommand($"Select * From [DNC_types] Where Type_id = {type_id};", new SqlConnection(connect_str));

            if (command.Connection.State != ConnectionState.Open && command.Connection != null)
                command.Connection.Open();

            try
            {
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    res = reader["Type_name"].ToString();
                }
                if (reader != null && !reader.IsClosed)
                    reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\nПомилка при читанні данних з БД.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (command.Connection.State != ConnectionState.Closed && command.Connection != null)
                    command.Connection.Close();
            }

            return res;
        }

        private string get_subtype(string subtype_id, SqlDataReader reader)
        {
            string res = "";

            SqlCommand command = new SqlCommand($"Select * From [DNC_subtypes] Where Subtype_id = {subtype_id};", new SqlConnection(connect_str));

            if (command.Connection.State != ConnectionState.Open && command.Connection != null)
                command.Connection.Open();

            try
            {
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    res = reader["Subtype_name"].ToString();
                }
                if (reader != null && !reader.IsClosed)
                    reader.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Помилка при читанні данних з БД.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (command.Connection.State != ConnectionState.Closed && command.Connection != null)
                    command.Connection.Close();
            }

            return res;
        }

        private string get_fabrc(string fabric_id, SqlDataReader reader)
        {
            string res = "";

            SqlCommand command = new SqlCommand($"Select * From [DNC_Fabric] Where Fabric_id = {fabric_id};", new SqlConnection(connect_str));

            if (command.Connection.State != ConnectionState.Open && command.Connection != null)
                command.Connection.Open();

            try
            {
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    res = reader["Name"].ToString();
                }
                if (reader != null && !reader.IsClosed)
                    reader.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Помилка при читанні данних з БД.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (command.Connection.State != ConnectionState.Closed && command.Connection != null)
                    command.Connection.Close();
            }

            return res;
        }

        private string[] get_category(string category_id, SqlDataReader reader)
        {
            string[] res = new string[2];

            SqlCommand command = new SqlCommand($"Select * From [DNC_category] Where Category_id = {category_id};", new SqlConnection(connect_str));

            if (command.Connection.State != ConnectionState.Open && command.Connection != null)
                command.Connection.Open();

            try
            {
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    res[0] = reader["Category"].ToString();
                    res[1] = reader["Price"].ToString();
                }
                if (reader != null && !reader.IsClosed)
                    reader.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Помилка при читанні данних з БД.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (command.Connection.State != ConnectionState.Closed && command.Connection != null)
                    command.Connection.Close();
            }

            return res;
        }

        private string get_system_color(string color_id, SqlDataReader reader)
        {
            string res = "";

            SqlCommand command = new SqlCommand($"Select * From [System_color] Where Color_id = {color_id};", new SqlConnection(connect_str));

            if (command.Connection.State != ConnectionState.Open && command.Connection != null)
                command.Connection.Open();

            try
            {
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    res = reader["Name"].ToString();
                }
                if (reader != null && !reader.IsClosed)
                    reader.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Помилка при читанні данних з БД.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (command.Connection.State != ConnectionState.Closed && command.Connection != null)
                    command.Connection.Close();
            }

            return res;
        }

        private string get_side(string side_id, SqlDataReader reader)
        {
            string res = "";

            SqlCommand command = new SqlCommand($"Select * From [Control] Where Control_id = {side_id};", new SqlConnection(connect_str));

            if (command.Connection.State != ConnectionState.Open && command.Connection != null)
                command.Connection.Open();

            try
            {
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    res = reader["Control_side"].ToString();
                }
                if (reader != null && !reader.IsClosed)
                    reader.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Помилка при читанні данних з БД.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (command.Connection.State != ConnectionState.Closed && command.Connection != null)
                    command.Connection.Close();
            }

            return res;
        }

        private string get_equipment(string equipment_id, SqlDataReader reader)
        {
            string res = "";

            SqlCommand command = new SqlCommand($"Select * From [DNC_Additional_equipment] Where Equipment_id = {equipment_id};", new SqlConnection(connect_str));

            if (command.Connection.State != ConnectionState.Open && command.Connection != null)
                command.Connection.Open();

            try
            {
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    res = reader["Price"].ToString();
                }
                if (reader != null && !reader.IsClosed)
                    reader.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Помилка при читанні данних з БД.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (command.Connection.State != ConnectionState.Closed && command.Connection != null)
                    command.Connection.Close();
            }

            return res;
        }

        private string get_installation(string installation_id, SqlDataReader reader)
        {
            string res = "";

            SqlCommand command = new SqlCommand($"Select * From [Installation] Where Installation_id = {installation_id};", new SqlConnection(connect_str));

            if (command.Connection.State != ConnectionState.Open && command.Connection != null)
                command.Connection.Open();

            try
            {
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    res = reader["Price"].ToString();
                }
                if (reader != null && !reader.IsClosed)
                    reader.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Помилка при читанні данних з БД.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (command.Connection.State != ConnectionState.Closed && command.Connection != null)
                    command.Connection.Close();
            }

            return res;
        }
    }
}
