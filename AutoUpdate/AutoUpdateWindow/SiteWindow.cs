using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoUpdateWindow
{

    /// <summary>
    /// 站点window
    /// </summary>
    public partial class SiteWindow : BaseWindow
    {

        public delegate void GetSiteHandler(Dictionary<string,string> dic);//声明委托
        public GetSiteHandler getSiteHandler;//委托对象

        public SiteWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        ///datagrid初始化数据源
        /// </summary>
        private void InitDtSource()
        {
            //加载列和初始数据        
            dataGrid.Columns.Add(new DataGridViewCheckBoxColumn()
            {
                HeaderText = "全选",
                Name = "cb_check",
                TrueValue = true,
                FalseValue = false,
                Width = 80,
                DataPropertyName = "CheckBox"
            });

            dataGrid.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Name",
                Name = "名称",
                Width = 150
            });

            dataGrid.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "IP",
                Name = "IP",
                Width = 130
            });


            dataGrid.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Path",
                Name = "物理路径",
                Width = 160
            });

            dataGrid.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "PoolName",
                Name = "应用程序池",
                Width = 130
            });

            //绑定数据
            var iisList = AutoUpdate.Utils.IISHelper.GetLocalIISStationsList();
            dataGrid.DataSource = iisList;
        }

        /// <summary>
        /// 默认不选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGrid_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dataGrid.ClearSelection();
        }

        /// <summary>
        /// 选中一列的单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)//如果单击列表头,不管
            {
                int index = dataGrid.CurrentRow.Index;
                this.dataGrid.Rows[e.RowIndex].Selected = true;
                if (Convert.ToBoolean(dataGrid.Rows[index].Cells[0].Value))
                {
                    dataGrid.Rows[index].Cells[0].Value = false;
                }
                else
                {
                    dataGrid.Rows[index].Cells[0].Value = true;
                }
            }
        }

        /// <summary>
        /// 窗体加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SiteWindow_Load(object sender, EventArgs e)
        {
            InitDtSource();
            AddCheckBoxToDataGridView.dgv = dataGrid;
            AddCheckBoxToDataGridView.AddFullSelect();
        }

        #region 站点列表确定和取消事件
        private void sureBtn_Click(object sender, EventArgs e)
        {       
            if (getSiteHandler != null)
            {
                var dic = new Dictionary<string, string>();
                for (int i = this.dataGrid.RowCount - 1; i >= 0; i--)
                {
                    var ck = this.dataGrid.Rows[i].Cells[0].Value;
                    if (ck != null && (bool)ck == true)
                    {
                        var name = dataGrid.Rows[i].Cells[2].Value.ToString();
                        var path = dataGrid.Rows[i].Cells[3].Value.ToString();
                        dic.Add(name, path);
                    }
                }
                getSiteHandler(dic);
                this.Close();
            }
        }

        /// <summary>
        /// 关闭站点列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cancelBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion



       
    }
}
