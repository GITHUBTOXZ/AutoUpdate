using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoUpdateWindow
{
    public partial class UpdaetManage : BaseWindow
    {
        public UpdaetManage()
        {
            InitializeComponent();

        }

        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdaetManage_Load(object sender, EventArgs e)
        {
            //初始化更新列表
            InitupGrid();
            //AddCheckBoxToDataGridView.dgv = upGrid;
            //AddCheckBoxToDataGridView.AddFullSelect();
        }




        #region 更新列表的操作
        /// <summary>
        /// 添加站点到更新列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addBtnSite_Click(object sender, EventArgs e)
        {
            SiteWindow frm = new SiteWindow();
            frm.getSiteHandler += getValue;
            frm.ShowDialog();
        }

        /// <summary>
        /// 获取站点列表数据
        /// </summary>
        /// <param name="dic"></param>
        public void getValue(Dictionary<string, string> dic)
        {
            foreach (var item in dic)
            {
                upGridAddNewRow(item.Key, item.Key, item.Value, "站点", "等待下载");
            }
        }

        /// <summary>
        /// 添加一行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addBtn_Click(object sender, EventArgs e)
        {
            upGridAddNewRow("", "", "", "其他", "等待下载");
        }

        /// <summary>
        /// 更新列表增加新行
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fileName"></param>
        /// <param name="path"></param>
        /// <param name="typeName"></param>
        /// <param name="statusName"></param>
        /// <param name="progress"></param>
        private void upGridAddNewRow(string name, string fileName, string path, string typeName, string statusName)
        {
            DataGridViewRow newRow = new DataGridViewRow();
            upGrid.Rows.Insert(0, newRow);
            this.upGrid.Rows[0].Cells[1].Value = name;
            this.upGrid.Rows[0].Cells[2].Value = fileName+".rar";
            this.upGrid.Rows[0].Cells[3].Value = path;
            this.upGrid.Rows[0].Cells[4].Value = typeName;
            this.upGrid.Rows[0].Cells[5].Value = statusName;
            this.upGrid.Rows[0].Cells[6].Value = "";
            //this.upGrid.Rows[0].Cells[6].Value = 0;
        }

        /// <summary>
        /// 删除一行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void delBtn_Click(object sender, EventArgs e)
        {
            if (this.upGrid.CurrentRow == null)
            {
                MessageBox.Show("没有可删除行!");
                return;
            }

            var rowIndex = this.upGrid.CurrentRow.Index;
            var row = this.upGrid.Rows[rowIndex];
            DialogResult RSS = MessageBox.Show(this, "确定要删除第" + (rowIndex + 1) + "行数据码？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            switch (RSS)
            {
                case DialogResult.Yes:
                    upGrid.Rows.Remove(row);
                    break;
                case DialogResult.No:
                    break;
            }
        }
        public delegate string MethodCaller(string name, string fileName, string path, string typeName);//定义个代理 

        /// <summary>
        /// 下载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void downBtn_Click(object sender, EventArgs e)
        {
            var rows = upGrid.Rows;
            if (rows.Count == 0)
            {
                MessageBox.Show("没有要下载的任务");
                return;
            }

            List<Task> taskList = new List<Task>();
            foreach (DataGridViewRow row in rows)
            {
                var name= row.Cells[1].Value.ToString();
                var fileName = row.Cells[2].Value.ToString();
                var filePath = row.Cells[3].Value.ToString();
                var typeName = row.Cells[4].Value.ToString();
                var task = Task.Factory.StartNew(() =>
                {
                    MethodCaller mc = new MethodCaller(DownTask);
                    IAsyncResult result= mc.BeginInvoke(name,fileName, filePath, typeName,null, null);
                    string upStatus = mc.EndInvoke(result);//用于接收返回值
                    this.upGrid.Rows[row.Index].Cells[5].Value = "下载完成";
                    this.upGrid.Rows[row.Index].Cells[6].Value = upStatus;
                    Thread.Sleep(500);
                });
                taskList.Add(task);
                this.upGrid.Rows[row.Index].Cells[5].Value = "正在下载";
            }
            Task.WaitAll(taskList.ToArray());//等待所有线程都执行完毕
        }



        /// <summary>
        /// 执行任务
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="fileName">文件名</param>
        /// <param name="path">需要更新文件的路径</param>
        /// <param name="typeName">类型名</param>
        /// <param name="backMsg">返回消息</param>
        /// <param name="updateResult">返回结果</param>
        /// <returns></returns>
        private string DownTask(string name,string fileName, string path,string typeName)
        {
            var msg=AutoUpdate.Update.ExecuteUpdate(new AutoUpdate.ReceiveUpdateInfo() {Name=name,DownFileName=fileName,LocalFilePath=path,FileType=(AutoUpdate.MyEnum.UpdateFileTypeEnum)Enum.Parse(typeof(AutoUpdate.MyEnum.UpdateFileTypeEnum),typeName) });    
            return Enum.GetName(typeof(AutoUpdate.MyEnum.UpdateResultEnum), msg.UpdateResult);
        }


    #endregion


    #region 更新列表

    /// <summary>
    /// 初始化更新列表
    /// </summary>
    private void InitupGrid()
    {

        //upGrid.Columns.Add(new DataGridViewCheckBoxColumn()
        //{
        //    HeaderText = "全选",
        //    Name = "cb_check",
        //    TrueValue = true,
        //    FalseValue = false,
        //    Width = 80,
        //    DataPropertyName = "CheckBox"
        //});

        upGrid.Columns.Add(new DataGridViewTextBoxColumn()
        {
            DataPropertyName = "bh",
            Name = "序号",
            Width = 60
        }
     );

        upGrid.Columns.Add(new DataGridViewTextBoxColumn()
        {
            DataPropertyName = "Name",
            Name = "名称",
            Width = 100
        }
       );
        upGrid.Columns.Add(new DataGridViewTextBoxColumn()
        {
            DataPropertyName = "FileName",
            Name = "下载的文件",
            Width = 100
        }
        );

        upGrid.Columns.Add(new DataGridViewTextBoxColumn()
        {
            DataPropertyName = "Path",
            Name = "物理路径",
            Width = 160
        });
        upGrid.Columns.Add(new DataGridViewComboBoxColumn()
        {
            DataPropertyName = "TypeName",
            Name = "类型",
            Width = 100,
            DataSource = new string[] { "站点", "其他" }
        });

        upGrid.Columns.Add(new DataGridViewTextBoxColumn()
        {
            DataPropertyName = "StatusName",
            Name = "状态",
            ReadOnly = true,
            Width = 100
        });
            //upGrid.Columns.Add(new DataGridViewProgressBarColumn()
            //{
            //    DataPropertyName = "Progress",
            //    Name = "下载进度",
            //    ReadOnly = true,
            //    Width = 100
            //});

            upGrid.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "UpdateResult",
                Name = "更新结果",
                ReadOnly = true,
                Width = 100
            });

        }

    /// <summary>
    /// 显示更新列表序号
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void upGrid_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
    {
        //自动编号，与数据无关
        Rectangle rectangle = new Rectangle(e.RowBounds.Location.X,
           e.RowBounds.Location.Y,
           upGrid.RowHeadersWidth - 4,
           e.RowBounds.Height);
        TextRenderer.DrawText(e.Graphics,
              (e.RowIndex + 1).ToString(),
               upGrid.RowHeadersDefaultCellStyle.Font,
               rectangle,
               upGrid.RowHeadersDefaultCellStyle.ForeColor,
               TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
    }

    /// <summary>
    /// 增加新行时出现的错误
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void upGrid_DataError(object sender, DataGridViewDataErrorEventArgs e)
    {
        if (upGrid.Rows[e.RowIndex].IsNewRow) return;
    }
    #endregion


}
}
