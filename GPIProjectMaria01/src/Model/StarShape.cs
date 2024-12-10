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
    public class StarShape : Shape
    {
        #region Constructor
        public StarShape()
        {
        }

        public StarShape(RectangleF rect) : base(rect)
        {
        }

        public StarShape(Shape shape) : base(shape)
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

        /// <summary>
        /// Частта, визуализираща конкретния примитив.
        /// </summary>
        public override void DrawSelf(Graphics grfx)
        {
            base.DrawSelf(grfx);

            PointF[] points = new PointF[10];

            points[0] = new PointF(Location.X+Width/2,Location.Y);
            points[2] = new PointF(Location.X, Location.Y+Height*2/5);
            points[4] = new PointF(Location.X + Width/5,Location.Y+Height);
            points[6] = new PointF(Location.X + Width * 4 / 5, Location.Y+Height);
            points[8] = new PointF(Location.X + Width, Location.Y+Height*2/5);

            PointF center=new PointF(
                (points[0].X+ points[2].X + points[4].X + points[6].X + points[8].X) /5,
                (points[0].Y + points[2].Y + points[4].Y + points[6].Y + points[8].Y) / 5 );

            points[1] = new PointF(
                (points[0].X + points[2].X+center.X) / 3,
                (points[0].Y + points[2].Y + center.Y) / 3);
            points[3] = new PointF(
                (points[4].X + points[2].X + center.X) / 3,
                (points[4].Y + points[2].Y + center.Y) / 3 );
            points[5] = new PointF(
                (points[4].X + points[6].X + center.X) / 3,
                (points[4].Y + points[6].Y + center.Y) / 3);
            points[7] = new PointF(
                (points[6].X + points[8].X + center.X) / 3,
                (points[6].Y + points[8].Y + center.Y) / 3);
            points[9] = new PointF(
                (points[0].X + points[8].X + center.X) / 3,
                (points[0].Y + points[8].Y + center.Y) / 3);

            var state = grfx.Save();

            Matrix trans = grfx.Transform;
            grfx.Transform = Matrix;

            grfx.FillPolygon(new SolidBrush(FillColor), points);
            grfx.DrawPolygon(Pens.Black, points);
            grfx.Transform = trans;

        }
    }
}
