using Draw.src.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Draw
{
	/// <summary>
	/// Класът, който ще бъде използван при управляване на диалога.
	/// </summary>
	public class DialogProcessor : DisplayProcessor
	{
		#region Constructor
		
		public DialogProcessor()
		{
		}
		
		#endregion
		
		#region Properties
		
		/// <summary>
		/// Избран елемент.
		/// </summary>
		//private Shape selection;
		//public Shape Selection {
		//	get { return selection; }
		//	set { selection = value; }
		//}

		private List<Shape> selections = new List<Shape>();

		public List<Shape> Selections
		{
			get { return selections; }
			set { selections = value; }
		}
		
		/// <summary>
		/// Дали в момента диалога е в състояние на "влачене" на избрания елемент.
		/// </summary>
		private bool isDragging;
		public bool IsDragging {
			get { return isDragging; }
			set { isDragging = value; }
		}
		
		/// <summary>
		/// Последна позиция на мишката при "влачене".
		/// Използва се за определяне на вектора на транслация.
		/// </summary>
		private PointF lastLocation;
		public PointF LastLocation {
			get { return lastLocation; }
			set { lastLocation = value; }
		}


		
		#endregion
		
		/// <summary>
		/// Добавя примитив - правоъгълник на произволно място върху клиентската област.
		/// </summary>
		public void AddRandomRectangle()
		{
			Random rnd = new Random();
			int x = rnd.Next(100,1000);
			int y = rnd.Next(100,600);
			
			RectangleShape rect = new RectangleShape(new Rectangle(x,y,100,200));
			rect.FillColor = Color.White;

			ShapeList.Add(rect);
			selections.Add(rect);
		}

        public void AddRandomEllipse()
        {
            Random rnd = new Random();
            int x = rnd.Next(100, 1000);
            int y = rnd.Next(100, 600);

            EllipseShape ellipse = new EllipseShape(new Rectangle(x, y, 100, 200));
            ellipse.FillColor = Color.White;

            ShapeList.Add(ellipse);
			selections.Add(ellipse);
        }

        public void AddRandomLine()
        {
            Random rnd = new Random();
            int x = rnd.Next(100, 1000);
            int y = rnd.Next(100, 600);

            LineShape line = new LineShape(new Rectangle(x, y, 100, 200));
            line.FillColor = Color.White;

            ShapeList.Add(line);
			selections.Add(line);
        }

        public void AddRandomStar()
        {
            Random rnd = new Random();
            int x = rnd.Next(100, 1000);
            int y = rnd.Next(100, 600);

            StarShape star = new StarShape(new Rectangle(x, y, 200, 200));
            star.FillColor = Color.White;

            ShapeList.Add(star);
			selections.Add(star);
        }

		public void AddRandomCircleShape()
		{
			Random rnd = new Random();
            int x = rnd.Next(100, 1000);
            int y = rnd.Next(100, 600);

			CircleShape circle = new CircleShape(new Rectangle(x,y, 150, 150));
			circle.FillColor = Color.White;

			ShapeList.Add(circle);
			selections.Add(circle);
        }

        /// <summary>
        /// Проверява дали дадена точка е в елемента.
        /// Обхожда в ред обратен на визуализацията с цел намиране на
        /// "най-горния" елемент т.е. този който виждаме под мишката.
        /// </summary>
        /// <param name="point">Указана точка</param>
        /// <returns>Елемента на изображението, на който принадлежи дадената точка.</returns>
        public Shape ContainsPoint(PointF point)
		{
			for(int i = ShapeList.Count - 1; i >= 0; i--){
				if (ShapeList[i].Contains(point)){
					//ShapeList[i].FillColor = Color.Red;
						
					return ShapeList[i];
				}	
			}
			return null;
		}
		
		/// <summary>
		/// Транслация на избраният елемент на вектор определен от <paramref name="p>p</paramref>
		/// </summary>
		/// <param name="p">Вектор на транслация.</param>
		public void TranslateTo(PointF p)
		{
			for (int i = 0; i < selections.Count; i++)
			{
				if (selections[i] != null)
				{
					selections[i].Location = new PointF(selections[i].Location.X + p.X - lastLocation.X, selections[i].Location.Y + p.Y - lastLocation.Y);
					lastLocation = p;
				}
			}
		}

		public void	Rotate(float angle)
		{
			for (int i = 0; i < selections.Count; i++)
			{
				if (selections[i] != null)
				{
					Matrix matrix = selections[i].Matrix;
					matrix.Rotate(angle);
                    selections[i].Matrix = matrix;

				}
			}
		}

		public void Scale(float scaleX, float scaleY)
		{
			for (int i = 0; i < selections.Count; i++)
			{
				if (selections[i] != null)
				{
					Matrix matrix = selections[i].Matrix;
					matrix.Scale(scaleX, scaleY);
					selections[i].Matrix = matrix;
				}
			}
		}


	}
}
