using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Draw.src.Model
{
    [Serializable]
    public class EllipseShape : Shape
    {
        #region Constructor

        public EllipseShape(RectangleF rect) : base(rect)
        {
        }

        public EllipseShape(EllipseShape rectangle) : base(rectangle)
        {
        }

        #endregion

        public override bool Contains(PointF point)
        {
            var pointx = point.X;
            var x = Rectangle.X + (Rectangle.Width / 2);
            var radx = Rectangle.Width / 2;
            var pointy = point.Y;
            var y = Rectangle.Y + Rectangle.Height / 2;
            var rady = Rectangle.Height / 2;

            if (((Math.Pow(pointx - x, 2) / Math.Pow(radx, 2)) + (Math.Pow(pointy - y, 2) / Math.Pow(rady, 2))) <= 1)
                return true;
            else
                // Ако не е в обхващащия правоъгълник, то неможе да е в обекта и => false
                return false;
        }

        public override void DrawSelf(Graphics grfx)
        {
            base.DrawSelf(grfx);

            var state = grfx.Save();

            Matrix trans = grfx.Transform;
            grfx.Transform = Matrix;

            grfx.FillEllipse(new SolidBrush(FillColor), Rectangle.X, Rectangle.Y, Rectangle.Width, Rectangle.Height);
            grfx.DrawEllipse(Pens.Black, Rectangle.X, Rectangle.Y, Rectangle.Width, Rectangle.Height);

            grfx.Transform = trans;
        }
    }
}
