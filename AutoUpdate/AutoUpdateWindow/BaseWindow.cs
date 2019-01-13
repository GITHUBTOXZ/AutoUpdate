using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoUpdateWindow
{
    /// <summary>
    /// window的公共处理的逻辑
    /// </summary>
   public class BaseWindow: Form
    {
        public BaseWindow()
        {
            //初始化配置
            AutoUpdate.Update.Init();
        }

        /// <summary>  
        /// 给DataGridView添加全选  
        /// reference:http://blog.csdn.net/netgyc/article/details/5578628，
        /// </summary>  
        public class AddCheckBoxToDataGridView
        {
            public static System.Windows.Forms.DataGridView dgv;
            public static void AddFullSelect()
            {
                System.Windows.Forms.CheckBox ckBox = new System.Windows.Forms.CheckBox();
                ckBox.Text = "";
                ckBox.Checked = false;
                System.Drawing.Rectangle rect = dgv.GetCellDisplayRectangle(0, -1, true);
                ckBox.Size = new System.Drawing.Size(13, 13);
                ckBox.Location = new Point(rect.Location.X + dgv.Columns[0].Width / 2 - 13 / 2 - 1, rect.Location.Y + 3);
                //ckBox.Location.Offset(-40, rect.Location.Y);
                ckBox.CheckedChanged += new EventHandler(ckBox_CheckedChanged);
                dgv.Controls.Add(ckBox);
            }
            static void ckBox_CheckedChanged(object sender, EventArgs e)
            {
                for (int i = 0; i < dgv.Rows.Count; i++)
                {
                    dgv.Rows[i].Cells[0].Value = ((System.Windows.Forms.CheckBox)sender).Checked;
                }
                dgv.EndEdit();
            }

        }

        #region datagrid添加进度条
        public class DataGridViewProgressBarCell : DataGridViewCell
        {
            public DataGridViewProgressBarCell()
            {

            }

            //设置进度条的背景色；
            public DataGridViewProgressBarCell(Color progressBarColor)
            : base()
            {
                ProgressBarColor = progressBarColor;
            }

            protected Color ProgressBarColor = Color.Green; //进度条的默认背景颜色,绿色；

            protected override void Paint(Graphics graphics, Rectangle clipBounds,
            Rectangle cellBounds, int rowIndex,
            DataGridViewElementStates cellState,
            object value, object formattedValue,
            string errorText, DataGridViewCellStyle cellStyle,
            DataGridViewAdvancedBorderStyle advancedBorderStyle,
            DataGridViewPaintParts paintParts)
            {
                using (SolidBrush backBrush = new SolidBrush(cellStyle.BackColor))
                {
                    graphics.FillRectangle(backBrush, cellBounds);
                }
                base.PaintBorder(graphics, clipBounds, cellBounds, cellStyle, advancedBorderStyle);

                using (SolidBrush brush = new SolidBrush(ProgressBarColor))
                {
                    if (value != null)
                    {
                        if (!string.IsNullOrEmpty(value.ToString()))
                        {
                            int num = Convert.ToInt32(value);
                            float percent = num / 100F;

                            graphics.FillRectangle(brush, cellBounds.X, cellBounds.Y + 1, cellBounds.Width * percent, cellBounds.Height - 3);

                            string text = string.Format("{0}%", num);
                            SizeF rf = graphics.MeasureString(text, cellStyle.Font);
                            float x = cellBounds.X + (cellBounds.Width - rf.Width) / 2f;
                            float y = cellBounds.Y + (cellBounds.Height - rf.Height) / 2f;
                            graphics.DrawString(text, cellStyle.Font, new SolidBrush(cellStyle.ForeColor), x, y);
                        }
                    }
                }
            }
        }

        public class DataGridViewProgressBarColumn : DataGridViewColumn
        {
            public DataGridViewProgressBarColumn()
                : base(new DataGridViewProgressBarCell())
            {

            }
        }

        #endregion
    }
}
