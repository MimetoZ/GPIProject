using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Draw.src.Model
{
    public class CircleShape : Shape
    {
        #region Constructor
        public CircleShape(RectangleF rect) : base(rect) 
        { }

        public CircleShape(Shape shape) : base(shape)
        { }
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
                return false;
        }

        public override void DrawSelf(Graphics grfx)
        {
            base.DrawSelf(grfx);
            var state = grfx.Save();

            Matrix trans = grfx.Transform;
            grfx.Transform = Matrix;

            PointF pointOneLineOne = new PointF(Rectangle.X + Rectangle.Width / 2 - 65, Rectangle.Y + 37);
            PointF pointTwoLineOne = new PointF(Rectangle.X + Rectangle.Width / 2 + 65, Rectangle.Y + Rectangle.Height - 37);

            PointF pointOneLineTwo = new PointF(Rectangle.Width / 2 + 57 + Rectangle.X, Rectangle.Y + Rectangle.Width / 2 - 47);
            PointF pointTwoLineTwo = new PointF(Rectangle.X + Rectangle.Width / 2 - 59, Rectangle.Y + Rectangle.Height / 2 + 45);

            grfx.FillEllipse(new SolidBrush(FillColor), Rectangle.X, Rectangle.Y, Rectangle.Width, Rectangle.Height);
            grfx.DrawEllipse(Pens.Black, Rectangle.X, Rectangle.Y, Rectangle.Width, Rectangle.Height);
            grfx.DrawLine(Pens.Black, pointOneLineOne, pointTwoLineOne);
            grfx.DrawLine(Pens.Black, pointTwoLineTwo, pointOneLineTwo);

            grfx.Transform = trans;

        }
    }
}
