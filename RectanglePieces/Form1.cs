using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RectanglePieces
{
    public partial class Form1 : Form
    {
        List<RectangleDetail> rectangles = new List<RectangleDetail>();
        public Form1()
        {
            InitializeComponent();
        }

        //private void btnAdd_Click(object sender, EventArgs e)
        //{
        //    if(nWidth.Value>0 && nHeight.Value>0)
        //    {
        //        //if(nDetailHeight.Value>0 && nDetailWidth.Value>0 && nDetailSize.Value>0)
        //        //{
        //            RestangleDetail restangle = new RestangleDetail
        //            {
        //                Width = nDetailWidth.Value,
        //                Height = nDetailHeight.Value,
        //                Size = nDetailSize.Value
        //            };
        //           var oldRestangle= restangles.Any(x => x.Width == restangle.Width && x.Height == restangle.Height);
        //            if (oldRestangle)
        //            {
        //                restangles.Where(x => x.Width == restangle.Width && x.Height == restangle.Height).ToList().ForEach(s => s.Size += restangle.Size);
        //            }
        //            else
        //            {
        //                restangles.Add(restangle);
        //            }
        //            var source = new BindingSource();
        //            source.DataSource = restangles;
        //            restangleDataGridView.DataSource = source;
        //           // this.restangleDataGridView.Rows.Add(restangle);
        //        }
        //        else
        //        {
        //            MessageBox.Show("All fields required!");
        //        }
        //    //}
        //    //else
        //    //{
        //    //    MessageBox.Show("Please,indicate width and Height main Restangle");
        //    //}
        //}

        private void btnCalculate_Click(object sender, EventArgs e)
        {
            if (nWidth.Value>0 && nHeight.Value>0)
            {
                var mainWidth =nWidth.Value;
                var mainHeight = nHeight.Value;
            if (rectangles.Count > 0)
            {
                    RectangleResult resultForm=null;
                    try
                    {
                        var outOfMainRec = rectangles.Any(x => (x.Height > mainHeight || x.Width > mainWidth) && x.Size!=0);
                        if (outOfMainRec)
                        {
                            throw new Exception("Width or Height doesn't suit!");
                        }
                        resultForm = new RectangleResult(mainWidth, mainHeight, rectangles.OrderByDescending(x => x.Area).ToList());
                        resultForm.ShowDialog();
                    }
                    catch(Exception ex)
                    {
                       // resultForm.Close();
                        MessageBox.Show(ex.Message);
                    }
                        
            }
            else
            {
                MessageBox.Show("Please,add rentagle details to grid.");
            }
            }
            else
            {
                MessageBox.Show("Please,indicate width and Height main Restangle");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var source = new BindingSource();
            source.DataSource = rectangles;
            restangleDataGridView.DataSource = source;
        }

        private void restangleDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
