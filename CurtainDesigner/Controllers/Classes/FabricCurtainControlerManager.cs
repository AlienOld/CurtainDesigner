﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CurtainDesigner.Controllers.Classes
{
    class FabricCurtainControlerManager<O, L, F, T> : CurtainDesigner.Controllers.IControlerManage<O, L, F, T>
    {
        public void packing(O obj, L list, F form)
        {
            CurtainDesigner.Views.Interfaces.IViewManage<O, L, F, T> view = new CurtainDesigner.Views.Classes.FabricCurtainViewManage<O, L, F, T>();
            CurtainDesigner.Models.Interfaces.IObjectManage<L> model = new CurtainDesigner.Models.Classes.FabricCurtainManage<L>();
            model.writeObject(view.readObject(form, obj));
        }

        public void unpacking(L list, T table)
        {
            CurtainDesigner.Views.Interfaces.IViewManage<O, L, F, T> view = new CurtainDesigner.Views.Classes.FabricCurtainViewManage<O, L, F, T>();
            CurtainDesigner.Models.Interfaces.IObjectManage<L> model = new CurtainDesigner.Models.Classes.FabricCurtainManage<L>();
            view.writeObjects(model.readObjects(list), table);
        }

        public async void load_installations(F form)
        {
            CurtainDesigner.Models.Classes.FabricCurtainManage<L> model = new CurtainDesigner.Models.Classes.FabricCurtainManage<L>();
            CurtainDesigner.Views.Classes.FabricCurtainViewManage<O, L, F, T> view = new CurtainDesigner.Views.Classes.FabricCurtainViewManage<O, L, F, T>();
            SqlDataReader reader = null;

            reader = await model.getDataFromDB($"Select * From [Installation];");
            view.loadDataFromDB_comboboxes((form as CurtainDesigner.FormFabricCurtainOrder).comboBoxInstallation, reader, new System.ComponentModel.BindingList<KeyValuePair<string, int>>(), "Price", "Installation_id");

            model.closeConnection();
        }

        public async void load_data_once_comboboxes(ComboBox comboBox, string query, string key, string value, int selected_value)
        {
            CurtainDesigner.Models.Classes.FabricCurtainManage<L> model = new CurtainDesigner.Models.Classes.FabricCurtainManage<L>();
            CurtainDesigner.Views.Classes.FabricCurtainViewManage<O, L, F, T> view = new CurtainDesigner.Views.Classes.FabricCurtainViewManage<O, L, F, T>();
            SqlDataReader reader = null;

            reader = await model.getDataFromDB(query);
            view.loadDataFromDB_comboboxes(comboBox, reader, new System.ComponentModel.BindingList<KeyValuePair<string, int>>(), key, value, selected_value);
        }

        public async void load_data_once_label(Label label_price, Label label_id, string query, string key, string value)
        {
            CurtainDesigner.Models.Classes.FabricCurtainManage<L> model = new CurtainDesigner.Models.Classes.FabricCurtainManage<L>();
            CurtainDesigner.Views.Classes.FabricCurtainViewManage<O, L, F, T> view = new CurtainDesigner.Views.Classes.FabricCurtainViewManage<O, L, F, T>();
            SqlDataReader reader = null;

            reader = await model.getDataFromDB(query);
            view.loadDataFromDB_label(label_price, label_id, reader, value, key);
        }

        public async void load_data(F form)
        {
            CurtainDesigner.Models.Classes.FabricCurtainManage<L> model = new CurtainDesigner.Models.Classes.FabricCurtainManage<L>();
            CurtainDesigner.Views.Classes.FabricCurtainViewManage<O, L, F, T> view = new CurtainDesigner.Views.Classes.FabricCurtainViewManage<O, L, F, T>();
            SqlDataReader reader = null;

            reader = await model.getDataFromDB("Select * From [Fabric_curtains_types];");
            view.loadDataFromDB_comboboxes((form as CurtainDesigner.FormFabricCurtainOrder).comboBoxCurtainType, reader, new System.ComponentModel.BindingList<KeyValuePair<string, int>>(), "Type_name", "Type_id");

            reader = await model.getDataFromDB("Select * From [Control];");
            view.loadDataFromDB_comboboxes((form as CurtainDesigner.FormFabricCurtainOrder).comboBoxSide, reader, new System.ComponentModel.BindingList<KeyValuePair<string, int>>(), "Control_side", "Control_id");

            reader = await model.getDataFromDB("Select * From [System_color];");
            view.loadDataFromDB_comboboxes((form as CurtainDesigner.FormFabricCurtainOrder).comboBoxSystemColor, reader, new System.ComponentModel.BindingList<KeyValuePair<string, int>>(), "Name", "Color_id");

            reader = await model.getDataFromDB($"Select * From [Installation];");
            view.loadDataFromDB_comboboxes((form as CurtainDesigner.FormFabricCurtainOrder).comboBoxInstallation, reader, new System.ComponentModel.BindingList<KeyValuePair<string, int>>(), "Price", "Installation_id");

            model.closeConnection();
        }

        public async void load_subtypeAndAdditionall(F form, string type_id)
        {
            CurtainDesigner.Models.Classes.FabricCurtainManage<L> model = new CurtainDesigner.Models.Classes.FabricCurtainManage<L>();
            CurtainDesigner.Views.Classes.FabricCurtainViewManage<O, L, F, T> view = new CurtainDesigner.Views.Classes.FabricCurtainViewManage<O, L, F, T>();
            SqlDataReader reader = null;

            reader = await model.getDataFromDB($"Select * From [Fabric_curtains_subtypes] fcs Where [fcs].[Type_id] = {type_id};");
            view.loadDataFromDB_comboboxes((form as CurtainDesigner.FormFabricCurtainOrder).comboBoxCurtainSubtype, reader, new System.ComponentModel.BindingList<KeyValuePair<string, int>>(), "Subtype_name", "Subtype_id");

            reader = await model.getDataFromDB($"Select * From [Additional_equipment] fcs Where [fcs].[Type_id] = {type_id};");
            view.loadDataFromDB_comboboxes((form as CurtainDesigner.FormFabricCurtainOrder).comboBoxEquipment, reader, new System.ComponentModel.BindingList<KeyValuePair<string, int>>(), "Equipment", "Equipment_id");
        }

        public async void load_data(F form, string type_id, string subtype_id)
        {
            CurtainDesigner.Models.Classes.FabricCurtainManage<L> model = new CurtainDesigner.Models.Classes.FabricCurtainManage<L>();
            CurtainDesigner.Views.Classes.FabricCurtainViewManage<O, L, F, T> view = new CurtainDesigner.Views.Classes.FabricCurtainViewManage<O, L, F, T>();
            SqlDataReader reader = null;

            reader = await model.getDataFromDB($"Select * From [Fabric] f Where [f].[Type_id] = {type_id} and [f].[Subtype_id] = {subtype_id};");
            view.loadDataFromDB_comboboxes((form as CurtainDesigner.FormFabricCurtainOrder).comboBoxFabric, reader, new System.ComponentModel.BindingList<KeyValuePair<string, int>>(), "Name", "Fabric_id");

            model.closeConnection();
        }

        public async void load_FabricCategorydata(F form, string type_id, string fabric_id, string subtype_id)
        {
            CurtainDesigner.Models.Classes.FabricCurtainManage<L> model = new CurtainDesigner.Models.Classes.FabricCurtainManage<L>();
            CurtainDesigner.Views.Classes.FabricCurtainViewManage<O, L, F, T> view = new CurtainDesigner.Views.Classes.FabricCurtainViewManage<O, L, F, T>();
            SqlDataReader reader = null;

            reader = await model.getDataFromDB($"Select * From [Fabric_curtains_category] fcc Where [fcc].[Category_id] IN (Select Category_id From [Fabric] ffc Where [ffc].[Fabric_id] = {fabric_id}) and [fcc].[Type_id] = {type_id} and [fcc].[Subtype_id] = {subtype_id};");
            view.loadDataFromDB_label((form as CurtainDesigner.FormFabricCurtainOrder).labelFabricCategory, (form as CurtainDesigner.FormFabricCurtainOrder).labelFabricCategoryId, reader, "Price", "Category_id");

            model.closeConnection();
        }
    }
}
