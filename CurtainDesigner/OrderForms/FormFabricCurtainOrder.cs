﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CurtainDesigner
{
    public partial class FormFabricCurtainOrder : Form
    {
        //private readonly BindingList<KeyValuePair<string, int>> types = new BindingList<KeyValuePair<string, int>>();

        public FormFabricCurtainOrder()
        {
            InitializeComponent();
            LoadDataFromDb();
        }

        private void iconButtonNewOrder_Click(object sender, EventArgs e)
        {
            CurtainDesigner.Controllers.IControlerManage<Classes.FabricCurtain, List<Classes.FabricCurtain2>, FormFabricCurtainOrder, DataGridView> controler = new CurtainDesigner.Controllers.Classes.FabricCurtainControlerManager<Classes.FabricCurtain, List<Classes.FabricCurtain2>, FormFabricCurtainOrder, DataGridView>();
            controler.packing(new Classes.FabricCurtain(), new List<Classes.FabricCurtain2>(), this);
        }

        private async void LoadDataFromDb()
        {
            CurtainDesigner.Controllers.Classes.FabricCurtainControlerManager<Classes.FabricCurtain, List<Classes.FabricCurtain2>, FormFabricCurtainOrder, DataGridView> controler = new CurtainDesigner.Controllers.Classes.FabricCurtainControlerManager<Classes.FabricCurtain, List<Classes.FabricCurtain2>, FormFabricCurtainOrder, DataGridView>();
            await Task.Run(() => controler.load_data(this));
        }
    }
}
