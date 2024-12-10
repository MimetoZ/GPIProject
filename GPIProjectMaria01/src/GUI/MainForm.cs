using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Windows.Forms;

namespace Draw
{
	/// <summary>
	/// Върху главната форма е поставен потребителски контрол,
	/// в който се осъществява визуализацията
	/// </summary>
	public partial class MainForm : Form
	{
		/// <summary>
		/// Агрегирания диалогов процесор във формата улеснява манипулацията на модела.
		/// </summary>
		private DialogProcessor dialogProcessor = new DialogProcessor();
		private DisplayProcessor displayProcessor = new DisplayProcessor();
		
		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}

		/// <summary>
		/// Изход от програмата. Затваря главната форма, а с това и програмата.
		/// </summary>
		void ExitToolStripMenuItemClick(object sender, EventArgs e)
		{
			Close();
		}
		
		/// <summary>
		/// Събитието, което се прихваща, за да се превизуализира при изменение на модела.
		/// </summary>
		void ViewPortPaint(object sender, PaintEventArgs e)
		{
			dialogProcessor.ReDraw(sender, e);
		}
		
		/// <summary>
		/// Бутон, който поставя на произволно място правоъгълник със зададените размери.
		/// Променя се лентата със състоянието и се инвалидира контрола, в който визуализираме.
		/// </summary>
		void DrawRectangleSpeedButtonClick(object sender, EventArgs e)
		{
			dialogProcessor.AddRandomRectangle();
			
			statusBar.Items[0].Text = "Последно действие: Рисуване на правоъгълник";
			
			viewPort.Invalidate();
		}

		/// <summary>
		/// Прихващане на координатите при натискането на бутон на мишката и проверка (в обратен ред) дали не е
		/// щракнато върху елемент. Ако е така то той се отбелязва като селектиран и започва процес на "влачене".
		/// Промяна на статуса и инвалидиране на контрола, в който визуализираме.
		/// Реализацията се диалогът с потребителя, при който се избира "най-горния" елемент от екрана.
		/// </summary>
		void ViewPortMouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (pickUpSpeedButton.Checked) {
				for (int i = 0; i < dialogProcessor.Selections.Count; i++)
				{
					dialogProcessor.Selections[i] = dialogProcessor.ContainsPoint(e.Location);
					if (dialogProcessor.Selections[i] != null)
					{
						statusBar.Items[0].Text = "Последно действие: Селекция на примитив";
						dialogProcessor.IsDragging = true;
						dialogProcessor.LastLocation = e.Location;
						viewPort.Invalidate();
					}
				}
			}
		}

		/// <summary>
		/// Прихващане на преместването на мишката.
		/// Ако сме в режм на "влачене", то избрания елемент се транслира.
		/// </summary>
		void ViewPortMouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			for (int i = 0; i < dialogProcessor.Selections.Count; i++)
			{
				if (dialogProcessor.IsDragging)
				{
					if (dialogProcessor.Selections[i] != null) statusBar.Items[0].Text = "Последно действие: Влачене";
					dialogProcessor.TranslateTo(e.Location);
					viewPort.Invalidate();
				}
			}
		}

		/// <summary>
		/// Прихващане на отпускането на бутона на мишката.
		/// Излизаме от режим "влачене".
		/// </summary>
		void ViewPortMouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			dialogProcessor.IsDragging = false;
		}

        private void drawRectangleSpeedButtonClick(object sender, EventArgs e)
        {
            dialogProcessor.AddRandomEllipse();

            statusBar.Items[0].Text = "Последно действие: Рисуване на елипса";

            viewPort.Invalidate();
        }

        private void drawLineSpeedButton_Click(object sender, EventArgs e)
        {
            dialogProcessor.AddRandomLine();

            statusBar.Items[0].Text = "Последно действие: Рисуване на черта";

            viewPort.Invalidate();
        }

        private void drawStarSpeedButton_Click(object sender, EventArgs e)
        {
            dialogProcessor.AddRandomStar();

            statusBar.Items[0].Text = "Последно действие: Рисуване на звезда";

            viewPort.Invalidate();
        }

        private void ColorPickerButton_Click(object sender, EventArgs e)
        {
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
				for (int i = 0; i < dialogProcessor.Selections.Count; i++)
				{
					if (dialogProcessor.Selections[i] != null)
					{
						dialogProcessor.Selections[i].FillColor = colorDialog.Color;
						viewPort.Invalidate();
					}
				}
            }
        }

        private void RotateButton_Click(object sender, EventArgs e)
        {
			for (int i = 0; i < dialogProcessor.Selections.Count; i++)
			{
				if (dialogProcessor.Selections[i] != null)
				{
					dialogProcessor.Rotate(45);
					viewPort.Invalidate();
				}
			}
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Binary files (*.bin)|*.bin";
            saveFileDialog.Title = "Save Model";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                using (FileStream fs = new FileStream(saveFileDialog.FileName, FileMode.Create))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    try
                    {
                        formatter.Serialize(fs, displayProcessor.ShapeList);
                        MessageBox.Show("Model saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (SerializationException ex)
                    {
                        MessageBox.Show("Failed to save model. Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Binary files (*.bin)|*.bin";
            openFileDialog.Title = "Open Model";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                using (FileStream fileStream = new FileStream(openFileDialog.FileName, FileMode.Open))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    try
                    {
                        displayProcessor.ShapeList = (List<Shape>)formatter.Deserialize(fileStream);

                        RefreshShape();

                        MessageBox.Show("Model loaded successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (SerializationException ex)
                    {
                        MessageBox.Show("Failed to load model. Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void RefreshShape()
        {
            Graphics grfx = Graphics.FromHwnd(this.Handle);

            foreach (var item in displayProcessor.ShapeList)
			{
				displayProcessor.DrawShape(grfx, item);
			}
        }

        private void ScaleButton_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dialogProcessor.Selections.Count; i++)
            {
                if (dialogProcessor.Selections[i] != null)
                {
					dialogProcessor.Scale(2, 1);
                    viewPort.Invalidate();
                }
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            dialogProcessor.AddRandomCircleShape();

            statusBar.Items[0].Text = "Последно действие: Рисуване на кръг";

            viewPort.Invalidate();
        }
    }
}
