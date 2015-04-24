using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace RenderOrderClient
{
    public partial class Form1 : Form
    {

        public class overRidePanel : Panel
        {
            protected override void OnPaintBackground(PaintEventArgs pevent) { }
        }
        #region GlobalParams
        Bitmap bitmap;
        BufferedGraphicsContext currentContext;
        BufferedGraphics myBuffer;
        PointF viewPortCenter;
        float Zoom = 1.0f;
        Rectangle drawRect;

        Bitmap bitmap2;
        BufferedGraphicsContext currentContext2;
        BufferedGraphics myBuffer2;
        PointF viewPortCenter2;
        float Zoom2 = 1.0f;
        Point lastMouse2;

        bool draging = false;
        Point lastMouse;
        private Rectangle dragBoxFromMouseDown;
        private int rowIndexFromMouseDown;
        private int rowIndexOfItemUnderMouseToDrop;
        #endregion

        public Form1()
        {
            InitializeComponent();
            currentContext = BufferedGraphicsManager.Current;
            currentContext2 = BufferedGraphicsManager.Current;
            setup(false);
            setup2(false);
        }

        private void setup(bool resetViewport)
        {
            if (myBuffer != null)
                myBuffer.Dispose();
            myBuffer = currentContext.Allocate(this.panel1.CreateGraphics(), this.panel1.DisplayRectangle);
            if (bitmap != null)
            {
                if (resetViewport)
                    SetViewPort(new RectangleF(0, 0, bitmap.Width, bitmap.Height));
            }
            // this.panel1.Focus();
            this.panel1.Invalidate();
        }
        private void setup2(bool resetViewport)
        {
            if (myBuffer2 != null)
                myBuffer2.Dispose();
            myBuffer2 = currentContext2.Allocate(this.panel2.CreateGraphics(), this.panel2.DisplayRectangle);
            if (bitmap2 != null)
            {
                if (resetViewport)
                    SetViewPort2(new RectangleF(0, 0, bitmap2.Width, bitmap2.Height));
            }
            // this.panel1.Focus();
            this.panel2.Invalidate();
        }

        private void SetViewPort(RectangleF worldCords)
        {
            //Find smallest screen extent and zoom to that
            if (worldCords.Height > worldCords.Width)
            {
                //Use With as limiting factor
                this.Zoom = worldCords.Width / bitmap.Width;
            }
            else
                this.Zoom = worldCords.Height / bitmap.Height;

            viewPortCenter = new PointF(worldCords.X + (worldCords.Width / 2.0f), worldCords.Y + (worldCords.Height / 2.0f));
            // this.toolStripStatusLabel1.Text = "Zoom: " + ((int)(this.Zoom * 100)).ToString() + "%";

        }

        private void SetViewPort(Rectangle screenCords)
        {
        }

        private void SetViewPort2(RectangleF worldCords)
        {
            //Find smallest screen extent and zoom to that
            if (worldCords.Height > worldCords.Width)
            {
                //Use With as limiting factor
                this.Zoom2 = worldCords.Width / bitmap2.Width;
            }
            else
                this.Zoom2 = worldCords.Height / bitmap2.Height;

            viewPortCenter2 = new PointF(worldCords.X + (worldCords.Width / 2.0f), worldCords.Y + (worldCords.Height / 2.0f));
            // this.toolStripStatusLabel1.Text = "Zoom: " + ((int)(this.Zoom * 100)).ToString() + "%";

        }

        private void SetViewPort2(Rectangle screenCords)
        {
        }

        private void PaintImage2()
        {
            if (bitmap2 != null)
            {
                float widthZoomed = panel2.Width / Zoom2;
                float heigthZoomed = panel2.Height / Zoom2;
                //if (Zoom > 3)
                //{
                //    MessageBox.Show("Test");
                //    return;
                //}
                ////Do checks the reason 30,000 is used is because much over this will cause DrawImage to crash
                //if (widthZoomed > 30000.0f)
                //{
                //    Zoom = panel1.Width / 30000.0f;
                //    widthZoomed = 30000.0f;
                //}                
                //if (heigthZoomed > 30000.0f)
                //{
                //    Zoom = panel1.Height / 30000.0f;
                //    heigthZoomed = 30000.0f;
                //}

                ////we stop at 2 because at this point you have almost zoomed into a single pixel
                //if (widthZoomed < 2.0f)
                //{
                //    Zoom = panel1.Width / 2.0f;
                //    widthZoomed = 2.0f;
                //}
                //if (heigthZoomed < 2.0f)
                //{
                //    Zoom = panel1.Height / 2.0f;
                //    heigthZoomed = 2.0f;
                //}

                float wz2 = widthZoomed / 2.0f;
                float hz2 = heigthZoomed / 2.0f;
                drawRect = new Rectangle(
                    (int)(viewPortCenter2.X - wz2),
                    (int)(viewPortCenter2.Y - hz2),
                    (int)(widthZoomed),
                    (int)(heigthZoomed));

                //this.toolStripStatusLabel1.Text = "DrawRect = " + drawRect.ToString();

                myBuffer2.Graphics.Clear(Color.White); //Clear the Back buffer

                //Draw the image, Write image to back buffer, and render back buffer
                myBuffer2.Graphics.DrawImage(bitmap2, panel2.DisplayRectangle, drawRect, GraphicsUnit.Pixel);

                myBuffer2.Render(panel2.CreateGraphics());
                //   this.toolStripStatusLabel1.Text += "     Zoom: " + ((int)(this.Zoom * 100)).ToString() + "%";










            }
        }

        private void PaintImage()
        {
            if (bitmap != null)
            {
                float widthZoomed = panel1.Width / Zoom;
                float heigthZoomed = panel1.Height / Zoom;
                //if (Zoom > 3)
                //{
                //    MessageBox.Show("Test");
                //    return;
                //}
                ////Do checks the reason 30,000 is used is because much over this will cause DrawImage to crash
                //if (widthZoomed > 30000.0f)
                //{
                //    Zoom = panel1.Width / 30000.0f;
                //    widthZoomed = 30000.0f;
                //}                
                //if (heigthZoomed > 30000.0f)
                //{
                //    Zoom = panel1.Height / 30000.0f;
                //    heigthZoomed = 30000.0f;
                //}

                ////we stop at 2 because at this point you have almost zoomed into a single pixel
                //if (widthZoomed < 2.0f)
                //{
                //    Zoom = panel1.Width / 2.0f;
                //    widthZoomed = 2.0f;
                //}
                //if (heigthZoomed < 2.0f)
                //{
                //    Zoom = panel1.Height / 2.0f;
                //    heigthZoomed = 2.0f;
                //}

                float wz2 = widthZoomed / 2.0f;
                float hz2 = heigthZoomed / 2.0f;
                drawRect = new Rectangle(
                    (int)(viewPortCenter.X - wz2),
                    (int)(viewPortCenter.Y - hz2),
                    (int)(widthZoomed),
                    (int)(heigthZoomed));

                //this.toolStripStatusLabel1.Text = "DrawRect = " + drawRect.ToString();
                myBuffer.Graphics.Clear(Color.White); //Clear the Back buffer

                //Draw the image, Write image to back buffer, and render back buffer
                myBuffer.Graphics.DrawImage(bitmap, panel1.DisplayRectangle, drawRect, GraphicsUnit.Pixel);

                myBuffer.Render(panel1.CreateGraphics());
                //   this.toolStripStatusLabel1.Text += "     Zoom: " + ((int)(this.Zoom * 100)).ToString() + "%";










            }
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            setup(false);
            setup2(false);
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            PaintImage();
        }

        private void panel1_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            Zoom += Zoom * (e.Delta / 1200.0f); //the 1200.0f is any-number it just seem to work well so i use it.
            //if (e.Delta > 0) //I prefer to use the targe zoom when zooming in only, remove "if" to have it apply to zoom in and out
            //    viewPortCenter = new PointF(viewPortCenter.X + ((e.X - (panel1.Width / 2)) /(2* Zoom)), viewPortCenter.Y + ((e.Y - (panel1.Height/2)) / (2*Zoom)));                            
            this.panel1.Invalidate();
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                draging = true;
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (draging)
            {
                viewPortCenter = new PointF(viewPortCenter.X + ((lastMouse.X - e.X) / Zoom), viewPortCenter.Y + ((lastMouse.Y - e.Y) / Zoom));
                panel1.Invalidate();
            }
            lastMouse = e.Location;
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                draging = false;
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // myBuffer.Graphics.DrawImage(bit, 1000, 600);
            //  bitmap2.Save("c:\\files\\f\\d.jpg");

        }

        private void Form1_Load(object sender, EventArgs e)
        {

            //textBox2.MaxLength = int.Parse(ConfigurationSettings.AppSettings["StillsTitleLenght"].ToString().Trim()) * 2;
            //textBox1.MaxLength = int.Parse(ConfigurationSettings.AppSettings["FwnTitleLenght"].ToString().Trim());
            // richTextBox1.MaxLength = int.Parse(ConfigurationSettings.AppSettings["FwnNewsLenght"].ToString().Trim());

            for (int i = 0; i < 3; i++)
            {
                DataGridViewRow Rw = new DataGridViewRow();
                FwnDgv.ClearSelection();
                FwnDgv.Rows.Add(Rw);
                FwnDgv.ClearSelection();
                FwnDgv.Rows[i].Cells[0].Value = i + 1;
                FwnDgv.Rows[i].Cells[1].Value = dateTimePicker1.Value.ToLongDateString();
                FwnDgv.Rows[i].Cells[2].Value = "---" + i.ToString();
                FwnDgv.Rows[i].Cells[3].Value = "---" + i.ToString();
                FwnDgv.Rows[i].Cells[4].Value = RenderOrderClient.Properties.Resources.FWN2;
                FwnDgv.Rows[i].Cells[6].Value = "0";
            }

            for (int i = 0; i < 7; i++)
            {
                DataGridViewRow Rw2 = new DataGridViewRow();
                dataGridView1.ClearSelection();
                dataGridView1.Rows.Add(Rw2);
                dataGridView1.ClearSelection();
                dataGridView1.Rows[i].Cells[0].Value = (i + 1).ToString();
                dataGridView1.Rows[i].Cells[1].Value = RenderOrderClient.Properties.Resources.STILLS;
                dataGridView1.Rows[i].Cells[2].Value = "0";
            }

        }

        private void FwnDgv_SelectionChanged(object sender, EventArgs e)
        {
            pictureBox1.Visible = true;
            panel1.Visible = false;
            if (FwnDgv.Rows.Count == 3)
            {
                if (FwnDgv.SelectedRows.Count == 1)
                {
                    textBox1.Text = FwnDgv.SelectedRows[0].Cells[2].Value.ToString();
                    richTextBox1.Text = FwnDgv.SelectedRows[0].Cells[3].Value.ToString();
                    dateTimePicker1.Value = DateTime.Parse(FwnDgv.SelectedRows[0].Cells[1].Value.ToString());
                    if (FwnDgv.SelectedRows[0].Cells[5].Value != null)
                    {
                        //bitmap = (Bitmap)FwnDgv.SelectedRows[0].Cells[4].Value;
                        if (FwnDgv.SelectedRows[0].Cells[6].Value.ToString() == "1")
                        {
                            panel1.Visible = false;
                            pictureBox1.Visible = true;
                            pictureBox1.Image = (Image)FwnDgv.SelectedRows[0].Cells[4].Value;
                            // pictureBox1.Image = (Image)Image.FromFile(FwnDgv.SelectedRows[0].Cells[5].Value.ToString());
                        }
                        else
                        {
                            bitmap = (Bitmap)Image.FromFile(FwnDgv.SelectedRows[0].Cells[5].Value.ToString());
                            panel1.Visible = true;
                            pictureBox1.Visible = false;
                            setup(true);
                        }
                    }
                    else
                    {
                        openFileDialog1.ShowDialog();
                    }

                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (FwnDgv.Rows.Count == 3)
            {
                if (FwnDgv.SelectedRows.Count == 1)
                {
                    string Title = textBox1.Text.Trim();
                    int TitleLenght = 15;
                    if (Title.Length <= TitleLenght)
                    {
                        string TitleDesc = richTextBox1.Text.Trim();
                        int TitleLenghtDesc = 350;
                        if (TitleDesc.Length <= TitleLenghtDesc)
                        {
                            FwnDgv.SelectedRows[0].Cells[2].Value = textBox1.Text.Trim();
                            FwnDgv.SelectedRows[0].Cells[3].Value = richTextBox1.Text.Trim();
                            FwnDgv.SelectedRows[0].Cells[1].Value = dateTimePicker1.Value.ToLongDateString();
                            if (FwnDgv.SelectedRows[0].Cells[6].Value.ToString() == "0")
                            {
                                Rectangle cropRect = new Rectangle();
                                cropRect.Width = 859;
                                cropRect.Height = 688;
                                Bitmap src = (Bitmap)FwnDgv.SelectedRows[0].Cells[4].Value;
                                Bitmap target = new Bitmap(cropRect.Width, cropRect.Height);

                                using (Graphics g = Graphics.FromImage(target))
                                {
                                    g.DrawImage(src, new Rectangle(0, 0, 859, 688),
                                                     drawRect,
                                                     GraphicsUnit.Pixel);
                                    FwnDgv.SelectedRows[0].Cells[4].Value = target;
                                }
                                FwnDgv.SelectedRows[0].Cells[6].Value = "1";
                                FwnDgv_SelectionChanged(new object(), new EventArgs());
                            }
                        }
                        else
                        {
                            MessageBox.Show("DESCRIPTION: Current length is: " + TitleDesc.Length.ToString() + " Max allow is:" + (TitleLenghtDesc).ToString());
                        }

                    }
                    else
                    {
                        MessageBox.Show("TITLE: Current length is: " + Title.Length.ToString() + " Max allow is:" + (TitleLenght).ToString());
                    }
                }
            }
        }

        private void panel1_Click(object sender, EventArgs e)
        {
            panel1.Focus();
        }

        private void panel1_MouseLeave(object sender, EventArgs e)
        {
            //FwnDgv.Focus();
        }

        private void FwnDgv_DoubleClick(object sender, EventArgs e)
        {
            if (FwnDgv.Rows.Count == 3)
            {
                if (FwnDgv.SelectedRows.Count == 1)
                {
                    openFileDialog1.ShowDialog();
                }
            }
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            if (FwnDgv.Rows.Count == 3)
            {
                if (FwnDgv.SelectedRows.Count == 1)
                {
                    FwnDgv.SelectedRows[0].Cells[5].Value = openFileDialog1.FileName;
                    FwnDgv.SelectedRows[0].Cells[4].Value = Image.FromFile(openFileDialog1.FileName);
                    bitmap = (Bitmap)Image.FromFile(openFileDialog1.FileName);
                    FwnDgv.SelectedRows[0].Cells[6].Value = "0";
                    panel1.Visible = true;
                    pictureBox1.Visible = false;
                    setup(true);
                }
            }
        }

        private void FwnDgv_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                // If the mouse moves outside the rectangle, start the drag.
                if (dragBoxFromMouseDown != Rectangle.Empty &&
                    !dragBoxFromMouseDown.Contains(e.X, e.Y))
                {

                    // Proceed with the drag and drop, passing in the list item.                    
                    DragDropEffects dropEffect = FwnDgv.DoDragDrop(
                    FwnDgv.Rows[rowIndexFromMouseDown],
                    DragDropEffects.Move);
                }
            }
        }

        private void FwnDgv_MouseDown(object sender, MouseEventArgs e)
        {
            // Get the index of the item the mouse is below.
            rowIndexFromMouseDown = FwnDgv.HitTest(e.X, e.Y).RowIndex;
            if (rowIndexFromMouseDown != -1)
            {
                // Remember the point where the mouse down occurred. 
                // The DragSize indicates the size that the mouse can move 
                // before a drag event should be started.                
                Size dragSize = SystemInformation.DragSize;

                // Create a rectangle using the DragSize, with the mouse position being
                // at the center of the rectangle.
                dragBoxFromMouseDown = new Rectangle(new Point(e.X - (dragSize.Width / 2),
                                                               e.Y - (dragSize.Height / 2)),
                                    dragSize);
            }
            else
                // Reset the rectangle if the mouse is not over an item in the ListBox.
                dragBoxFromMouseDown = Rectangle.Empty;
        }

        private void FwnDgv_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void FwnDgv_DragDrop(object sender, DragEventArgs e)
        {
            // The mouse locations are relative to the screen, so they must be 
            // converted to client coordinates.
            Point clientPoint = FwnDgv.PointToClient(new Point(e.X, e.Y));

            // Get the row index of the item the mouse is below. 
            rowIndexOfItemUnderMouseToDrop =
                FwnDgv.HitTest(clientPoint.X, clientPoint.Y).RowIndex;

            // If the drag operation was a move then remove and insert the row.
            if (e.Effect == DragDropEffects.Move)
            {
                DataGridViewRow rowToMove = e.Data.GetData(
                    typeof(DataGridViewRow)) as DataGridViewRow;
                FwnDgv.Rows.RemoveAt(rowIndexFromMouseDown);
                FwnDgv.Rows.Insert(rowIndexOfItemUnderMouseToDrop, rowToMove);

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                int TitleFontSize = 60;

                int NewsFontSize = 50;

                bool AddToQueue = true;
                string[] DirQeue = Directory.GetDirectories("\\\\192.168.20.48\\input$\\FWN\\");
                if (DirQeue.Length > 0)
                {
                    string DirQeueList = "";
                    for (int i = 0; i < DirQeue.Length; i++)
                    {
                        DirQeueList += "\r\n" + i.ToString() + "-" + DirQeue[i].ToString();
                    }
                    DialogResult DlgRes = MessageBox.Show("There are ( " + DirQeue.Length + " ) " + DirQeueList + "\r\n" + "item in the  queue,Are you sure to add this one to render queue?", "Render Queue", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (DlgRes == System.Windows.Forms.DialogResult.Yes)
                    {
                        AddToQueue = true;
                    }
                    else
                    {
                        AddToQueue = false;
                    }
                }

                if (AddToQueue)
                {
                    try
                    {
                        bool AllInserted = true;
                        for (int i = 0; i < FwnDgv.Rows.Count; i++)
                        {
                            AllInserted = true;
                            if (FwnDgv.Rows[i].Cells[6].Value.ToString() != "1")
                            {
                                AllInserted = false;
                                MessageBox.Show("Please select a photo for row:# " + (i + 1).ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                        if (AllInserted)
                        {
                            string Folder = "\\" + string.Format("{0:0000}", DateTime.Now.Year) + "-" + string.Format("{0:00}", DateTime.Now.Month)
                           + "-" + string.Format("{0:00}", DateTime.Now.Day) + "-" + string.Format("{0:00}", DateTime.Now.Hour)
                           + "-" + string.Format("{0:00}", DateTime.Now.Minute) + "-" + string.Format("{0:00}", DateTime.Now.Second);
                            Directory.CreateDirectory("\\\\192.168.20.48\\input$\\FWN" + "\\" + Folder + "\\IMAGES\\");
                            Directory.CreateDirectory("\\\\192.168.20.48\\input$\\FWN" + "\\" + Folder + "\\TITLES\\");
                            for (int i = 0; i < FwnDgv.Rows.Count; i++)
                            {

                                Bitmap Img = new Bitmap((Bitmap)FwnDgv.Rows[i].Cells[4].Value);

                                string SSSS = "\\\\192.168.20.48\\input$\\FWN" +
                                     "\\" + Folder + "\\IMAGES\\" + (i + 1) + ".jpg";
                                Img.Save("\\\\192.168.20.48\\input$\\FWN" +
                                    "\\" + Folder + "\\IMAGES\\" + (i + 1) + ".jpg"
                                    , System.Drawing.Imaging.ImageFormat.Jpeg);
                                Img.Dispose();

                                //NEWS
                                GenerateImage(FwnDgv.Rows[i].Cells[3].Value.ToString().ToUpper(),
                                   "\\\\192.168.20.48\\input$\\FWN" + "\\" + Folder + "\\TITLES\\News " +
                                        string.Format("{0:00}", i + 1) + ".png",
                                    1260, 792, NewsFontSize, "Context Reprise SSi Medium", "#d3d1d9", 39, 35, FontStyle.Bold, true);

                                //CITY
                                GenerateImage(FwnDgv.Rows[i].Cells[2].Value.ToString().ToUpper(),
                                   "\\\\192.168.20.48\\input$\\FWN" + "\\" + Folder + "\\TITLES\\City " +
                                        string.Format("{0:00}", i + 1) + ".png",
                                   737, 135, TitleFontSize, "Context Reprise ExtraBlack SSi", "#000000", 1, 27, FontStyle.Bold, true);

                                //Date
                                DateTime DtRw = DateTime.Parse(FwnDgv.Rows[i].Cells[1].Value.ToString());
                                string DDDD = DtRw.DayOfWeek.ToString() + ", " + DtRw.ToString("m");
                                GenerateImage(DDDD.ToUpper(),
                                   "\\\\192.168.20.48\\input$\\FWN" + "\\" + Folder + "\\TITLES\\Date " +
                                        string.Format("{0:00}", i + 1) + ".png",
                                   870, 135, TitleFontSize, "Context Reprise ExtraBlack SSi", "#000000", 1, 27, FontStyle.Bold, true);
                            }

                            //Check Files Here:
                            try
                            {
                                if (Directory.GetFiles("\\\\192.168.20.48\\input$" + "\\" + Folder + "\\TITLES\\", "*.png").Length == FwnDgv.Rows.Count * 3)
                                {
                                    MessageBox.Show("Render's order added to FWN queue");
                                }
                                else
                                {
                                    Directory.Delete("\\\\192.168.20.48\\input$" + "\\" + Folder, true);
                                    MessageBox.Show("please add to queue again!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                            catch
                            {

                                Directory.Delete("\\\\192.168.20.48\\input$" + "\\" + Folder, true);
                                MessageBox.Show("please add to queue again!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            
                            //MessageBox.Show("Render order added to FWN queue");
                        }
                    }
                    catch (Exception Exp)
                    {
                        MessageBox.Show(Exp.Message);
                    }
                }
            }
            catch (Exception Err)
            {
                MessageBox.Show(Err.Message);
            }
        }

        protected Bitmap GenerateImage(string Text, string FileName, int Width, int Height, int FontSize, string FontName, string ColorCode, int IndentLeft, int IndentTop, FontStyle FntStyle, bool Save)
        {
            float fontSize = FontSize;

            //some test image for this demo
            Bitmap bmp = new Bitmap(Width, Height);
            Graphics g = Graphics.FromImage(bmp);


            //this will center align our text at the bottom of the image
            StringFormat sf = new StringFormat();
            //sf.Alignment = StringAlignment.Near;
            //sf.LineAlignment = StringAlignment.Far;

            //define a font to use.
            //   Font f = new Font("Context Reprise SSi", fontSize, FontStyle.Bold, GraphicsUnit.Pixel);
            Font f = new Font(FontName, fontSize, FntStyle, GraphicsUnit.Pixel);
            //if (FontName == "Context Reprise ExtraBlack SSi")
            //{
            //    f = new Font(FontName, fontSize, GraphicsUnit.Pixel);
            //}

            //pen for outline - set width parameter
            //Pen p = new Pen(ColorTranslator.FromHtml("#000000"), 3);
            Pen p = new Pen(ColorTranslator.FromHtml(ColorCode), 0);
            p.LineJoin = LineJoin.Round; //prevent "spikes" at the path

            //this makes the gradient repeat for each text line
            //Rectangle fr = new Rectangle(0, bmp.Height - f.Height, bmp.Width, f.Height);
            Rectangle fr = new Rectangle(0, 0, bmp.Width, f.Height);
            LinearGradientBrush b = new LinearGradientBrush(fr,
                                                            ColorTranslator.FromHtml(ColorCode),
                                                            ColorTranslator.FromHtml(ColorCode),
                                                            90);

            //this will be the rectangle used to draw and auto-wrap the text.
            //basically = image size
            Rectangle r = new Rectangle(IndentLeft, IndentTop, bmp.Width, bmp.Height);
            if (FileName.ToLower().Contains("news"))
            {
                r = new Rectangle(IndentLeft, IndentTop, bmp.Width - 170, bmp.Height);
            }


            GraphicsPath gp = new GraphicsPath();

            //look mom! no pre-wrapping!
            gp.AddString(Text,
                         f.FontFamily, (int)f.Style, fontSize, r, sf);

            //these affect lines such as those in paths. Textrenderhint doesn't affect
            //text in a path as it is converted to ..well, a path.    
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;


            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit; // <-- important!
            g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            g.TextContrast = 0;
            //TODO: shadow -> g.translate, fillpath once, remove translate


            //var matrix = new Matrix();
            //matrix.Translate(10, 10);
            //g.Transform=matrix;
            g.DrawPath(p, gp);
            g.FillPath(b, gp);

            //cleanup
            gp.Dispose();
            b.Dispose();
            b.Dispose();
            f.Dispose();
            sf.Dispose();
            g.Dispose();
            if (Save)
            {
                bmp.Save(FileName, System.Drawing.Imaging.ImageFormat.Png);
                bmp.Dispose();
            }

            return bmp;


        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {

            pictureBox2.Visible = true;
            panel2.Visible = false;
            if (dataGridView1.Rows.Count == 7)
            {
                if (dataGridView1.SelectedRows.Count == 1)
                {
                    textBox2.Text = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                    if (dataGridView1.SelectedRows[0].Cells[3].Value != null)
                    {
                        //bitmap = (Bitmap)FwnDgv.SelectedRows[0].Cells[4].Value;
                        if (dataGridView1.SelectedRows[0].Cells[2].Value.ToString() == "1")
                        {
                            panel2.Visible = false;
                            pictureBox2.Visible = true;
                            pictureBox2.Image = (Image)dataGridView1.SelectedRows[0].Cells[1].Value;
                            // pictureBox1.Image = (Image)Image.FromFile(FwnDgv.SelectedRows[0].Cells[5].Value.ToString());




                            string OutLine1 = "";
                            string OutLine2 = "";
                            int TitleLenght = 53;
                            int FontSize = 38;
                            string Title = dataGridView1.SelectedRows[0].Cells[0].Value.ToString().Trim();

                            if (Title.Length <= TitleLenght * 2)
                            {
                                char[] delimiters = new char[] { ' ' };
                                string[] PrgNameList = Title.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                                int CutIndex = 0;
                                foreach (string item in PrgNameList)
                                {
                                    if (CutIndex + item.Length + 1 <= TitleLenght)
                                    {
                                        CutIndex += item.Length + 1;
                                        OutLine1 += item + " ";
                                    }
                                    else
                                    {
                                        if (CutIndex + item.Length + 1 <= TitleLenght * 2)
                                        {
                                            CutIndex += item.Length + 1;
                                            OutLine2 += item + " ";
                                        }
                                    }
                                }

                            }
                            pictureBox3.Image = (Image)GenerateImage(OutLine1.ToUpper(), "FILENAME",
                             1920, 57, FontSize, "Context Reprise SSi Black", "#ffffff", 100, 10, FontStyle.Bold, false);

                            pictureBox4.Image = GenerateImage(OutLine2.ToUpper(), "FILENAME",
                                  1920, 57, FontSize, "Context Reprise SSi Black", "#ffffff", 100, 10, FontStyle.Bold, false);
















                        }
                        else
                        {
                            bitmap2 = (Bitmap)Image.FromFile(dataGridView1.SelectedRows[0].Cells[3].Value.ToString());
                            panel2.Visible = true;
                            pictureBox2.Visible = false;
                            setup2(true);
                        }
                    }
                    else
                    {
                        openFileDialog2.ShowDialog();
                    }

                }
            }
        }

        private void openFileDialog2_FileOk(object sender, CancelEventArgs e)
        {
            if (dataGridView1.Rows.Count == 7)
            {
                if (dataGridView1.SelectedRows.Count == 1)
                {
                    dataGridView1.SelectedRows[0].Cells[3].Value = openFileDialog2.FileName;
                    dataGridView1.SelectedRows[0].Cells[1].Value = Image.FromFile(openFileDialog2.FileName);
                    bitmap2 = (Bitmap)Image.FromFile(openFileDialog2.FileName);
                    dataGridView1.SelectedRows[0].Cells[2].Value = "0";
                    panel2.Visible = true;
                    pictureBox2.Visible = false;
                    setup2(true);
                }
            }
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == 7)
            {
                if (dataGridView1.SelectedRows.Count == 1)
                {
                    openFileDialog2.ShowDialog();
                }
            }
        }

        private void dataGridView1_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                // If the mouse moves outside the rectangle, start the drag.
                if (dragBoxFromMouseDown != Rectangle.Empty &&
                    !dragBoxFromMouseDown.Contains(e.X, e.Y))
                {

                    // Proceed with the drag and drop, passing in the list item.                    
                    DragDropEffects dropEffect = dataGridView1.DoDragDrop(
                    dataGridView1.Rows[rowIndexFromMouseDown],
                    DragDropEffects.Move);
                }
            }
        }

        private void dataGridView1_MouseDown(object sender, MouseEventArgs e)
        {
            // Get the index of the item the mouse is below.
            rowIndexFromMouseDown = dataGridView1.HitTest(e.X, e.Y).RowIndex;
            if (rowIndexFromMouseDown != -1)
            {
                // Remember the point where the mouse down occurred. 
                // The DragSize indicates the size that the mouse can move 
                // before a drag event should be started.                
                Size dragSize = SystemInformation.DragSize;

                // Create a rectangle using the DragSize, with the mouse position being
                // at the center of the rectangle.
                dragBoxFromMouseDown = new Rectangle(new Point(e.X - (dragSize.Width / 2),
                                                               e.Y - (dragSize.Height / 2)),
                                    dragSize);
            }
            else
                // Reset the rectangle if the mouse is not over an item in the ListBox.
                dragBoxFromMouseDown = Rectangle.Empty;
        }

        private void dataGridView1_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void dataGridView1_DragDrop(object sender, DragEventArgs e)
        {
            // The mouse locations are relative to the screen, so they must be 
            // converted to client coordinates.
            Point clientPoint = dataGridView1.PointToClient(new Point(e.X, e.Y));

            // Get the row index of the item the mouse is below. 
            rowIndexOfItemUnderMouseToDrop =
                dataGridView1.HitTest(clientPoint.X, clientPoint.Y).RowIndex;

            // If the drag operation was a move then remove and insert the row.
            if (e.Effect == DragDropEffects.Move)
            {
                DataGridViewRow rowToMove = e.Data.GetData(
                    typeof(DataGridViewRow)) as DataGridViewRow;
                dataGridView1.Rows.RemoveAt(rowIndexFromMouseDown);
                dataGridView1.Rows.Insert(rowIndexOfItemUnderMouseToDrop, rowToMove);

            }
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            PaintImage2();
        }

        private void panel2_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            Zoom2 += Zoom2 * (e.Delta / 1200.0f); //the 1200.0f is any-number it just seem to work well so i use it.
            //if (e.Delta > 0) //I prefer to use the targe zoom when zooming in only, remove "if" to have it apply to zoom in and out
            //    viewPortCenter = new PointF(viewPortCenter.X + ((e.X - (panel1.Width / 2)) /(2* Zoom)), viewPortCenter.Y + ((e.Y - (panel1.Height/2)) / (2*Zoom)));                            
            this.panel2.Invalidate();
        }

        private void panel2_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                draging = true;
        }

        private void panel2_MouseMove(object sender, MouseEventArgs e)
        {
            if (draging)
            {
                viewPortCenter2 = new PointF(viewPortCenter2.X + ((lastMouse2.X - e.X) / Zoom2), viewPortCenter2.Y + ((lastMouse2.Y - e.Y) / Zoom2));
                panel2.Invalidate();
            }
            lastMouse2 = e.Location;
        }

        private void panel2_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                draging = false;
        }

        private void panel2_Click(object sender, EventArgs e)
        {
            panel2.Focus();
        }

        private void panel2_MouseLeave(object sender, EventArgs e)
        {

            // dataGridView1.Focus();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == 7)
            {
                if (dataGridView1.SelectedRows.Count == 1)
                {
                    string Title = textBox2.Text.Trim().Replace("\n", "").Replace("\r", "");
                    int TitleLenght = 53;
                    if (Title.Length <= TitleLenght * 2)
                    {


                        dataGridView1.SelectedRows[0].Cells[0].Value = Title;
                        if (dataGridView1.SelectedRows[0].Cells[2].Value.ToString() == "0")
                        {
                            Rectangle cropRect = new Rectangle();
                            cropRect.Width = 1000;
                            cropRect.Height = 600;
                            Bitmap src = (Bitmap)dataGridView1.SelectedRows[0].Cells[1].Value;
                            Bitmap target = new Bitmap(cropRect.Width, cropRect.Height);

                            using (Graphics g = Graphics.FromImage(target))
                            {
                                g.DrawImage(src, new Rectangle(0, 0, 1000, 600),
                                                 drawRect,
                                                 GraphicsUnit.Pixel);
                                dataGridView1.SelectedRows[0].Cells[1].Value = target;
                            }
                            dataGridView1.SelectedRows[0].Cells[2].Value = "1";
                            dataGridView1_SelectionChanged(new object(), new EventArgs());

                        }
                    }
                    else
                    {
                        MessageBox.Show("Current length is: " + Title.Length.ToString() + " Max allow is:" + (TitleLenght * 2).ToString());
                    }
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //textBox2.MaxLength = int.Parse(ConfigurationSettings.AppSettings["StillsTitleLenght"].ToString().Trim()) * 2;
            //textBox1.MaxLength = int.Parse(ConfigurationSettings.AppSettings["FwnTitleLenght"].ToString().Trim());
            // richTextBox1.MaxLength = int.Parse(ConfigurationSettings.AppSettings["FwnNewsLenght"].ToString().Trim());
            try
            {
                bool AllInserted = true;
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    AllInserted = true;
                    if (dataGridView1.Rows[i].Cells[2].Value.ToString() != "1")
                    {
                        AllInserted = false;
                        MessageBox.Show("Please select a photo for row:# " + (i + 1).ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                if (AllInserted)
                {
                    string Folder = "";
                    if (radioButton1.Checked)
                    {
                        Folder = "POLITIC";
                    }
                    if (radioButton2.Checked)
                    {
                        Folder = "SPORT";
                    }
                    Directory.CreateDirectory("\\\\192.168.20.48\\input$" + "\\" + Folder);
                    bool AddToQueue = true;
                    string[] DirQeue = Directory.GetDirectories("\\\\192.168.20.48\\input$" + "\\" + Folder);
                    if (DirQeue.Length > 0)
                    {
                        string DirQeueList = "";
                        for (int i = 0; i < DirQeue.Length; i++)
                        {
                            DirQeueList += "\r\n" + i.ToString() + "-" + DirQeue[i].ToString();
                        }
                        DialogResult DlgRes = MessageBox.Show("There are ( " + DirQeue.Length + " ) " + DirQeueList + "\r\n" + "item in the  queue,Are you sure to add this one to render queue?", "Render Queue", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (DlgRes == System.Windows.Forms.DialogResult.Yes)
                        {
                            AddToQueue = true;
                        }
                        else
                        {
                            AddToQueue = false;
                        }
                    }

                    if (AddToQueue)
                    {
                        Folder += "\\" + string.Format("{0:0000}", DateTime.Now.Year) + "-" + string.Format("{0:00}", DateTime.Now.Month)
                            + "-" + string.Format("{0:00}", DateTime.Now.Day) + "-" + string.Format("{0:00}", DateTime.Now.Hour)
                            + "-" + string.Format("{0:00}", DateTime.Now.Minute) + "-" + string.Format("{0:00}", DateTime.Now.Second);
                        Directory.CreateDirectory("\\\\192.168.20.48\\input$" + "\\" + Folder + "\\IMAGES\\");
                        Directory.CreateDirectory("\\\\192.168.20.48\\input$" + "\\" + Folder + "\\TITLES\\");
                        int TitleLenght = 53;
                        int FontSize = 38;
                        for (int i = 0; i < dataGridView1.Rows.Count; i++)
                        {
                            Bitmap Img = new Bitmap((Bitmap)dataGridView1.Rows[i].Cells[1].Value);
                            // string tttt = ConfigurationSettings.AppSettings["StillsImagePathTitle"].ToString().Trim() + "\\" + Folder + "\\IMAGES\\" + i+1 + ".png";
                            Img.Save("\\\\192.168.20.48\\input$" + "\\" + Folder + "\\IMAGES\\" + (i + 1) + ".jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                            Img.Dispose();

                            string OutLine1 = "";
                            string OutLine2 = "";
                            string Title = dataGridView1.Rows[i].Cells[0].Value.ToString().Trim();

                            if (Title.Length <= TitleLenght * 2)
                            {
                                char[] delimiters = new char[] { ' ' };
                                string[] PrgNameList = Title.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                                int CutIndex = 0;
                                foreach (string item in PrgNameList)
                                {
                                    if (CutIndex + item.Length + 1 <= TitleLenght)
                                    {
                                        CutIndex += item.Length + 1;
                                        OutLine1 += item + " ";
                                    }
                                    else
                                    {
                                        if (CutIndex + item.Length + 1 <= TitleLenght * 2)
                                        {
                                            CutIndex += item.Length + 1;
                                            OutLine2 += item + " ";
                                        }
                                    }
                                }

                            }
                            GenerateImage(OutLine1.ToUpper(),
                             "\\\\192.168.20.48\\input$" + "\\" + Folder + "\\TITLES\\" +
                             string.Format("{0:00}", i + 1) + ".png",
                             1920, 57, FontSize, "Context Reprise SSi Black", "#ffffff", 100, 10, FontStyle.Bold, true);

                            GenerateImage(OutLine2.ToUpper(),
                               "\\\\192.168.20.48\\input$" + "\\" + Folder + "\\TITLES\\" +
                                string.Format("{0:00}", i + 1) + "-2.png",
                                1920, 57, FontSize, "Context Reprise SSi Black", "#ffffff", 100, 10, FontStyle.Bold, true);
                        }
                        //Check Files Here:
                        try
                        {
                            if (Directory.GetFiles("\\\\192.168.20.48\\input$" + "\\" + Folder + "\\TITLES\\", "*.png").Length == dataGridView1.Rows.Count * 2
                           && Directory.GetFiles("\\\\192.168.20.48\\input$" + "\\" + Folder + "\\IMAGES\\", "*.jpg").Length == dataGridView1.Rows.Count)
                            {
                                MessageBox.Show("Render's order added to Stills queue");
                            }
                            else
                            {
                                Directory.Delete("\\\\192.168.20.48\\input$" + "\\" + Folder, true);
                                MessageBox.Show("please add to queue again!", "Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                            }
                        }
                        catch 
                        {


                            Directory.Delete("\\\\192.168.20.48\\input$" + "\\" + Folder, true);
                            MessageBox.Show("please add to queue again!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                       
                    }
                }
            }
            catch (Exception EXP)
            {
                MessageBox.Show(EXP.Message);
            }

        }
    }
}
