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
    public class LineShape : Shape
    {
        #region Constructor
        public LineShape()
        {
        }

        public LineShape(RectangleF rect) : base(rect)
        {
        }

        public LineShape(Shape shape) : base(shape)
        {
        }

        #endregion

        public override bool Contains(PointF point)
        {
            if (base.Contains(point))
                // Проверка дали е в обекта само, ако точката е в обхващащия правоъгълник.
                // В случая на правоъгълник - директно връщаме true
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

            grfx.DrawLine(Pens.Black, new PointF(Location.X, Location.Y),
                new PointF(Location.X + Width, Location.Y + Height));

            grfx.Transform = trans;

            

        }
    }
}
