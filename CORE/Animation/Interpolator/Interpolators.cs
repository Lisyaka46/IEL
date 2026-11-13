using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace LibraryIEL.CORE.Animation.Interpolator
{
    /// <summary>
    /// Преобразователь <see cref="double"/> значения
    /// </summary>
    internal sealed class DoubleInterpolator : InterpolatorValue<double>
    {
        /// <summary>
        /// Типизированное преобразование
        /// </summary>
        public override double InterpolateTyped(double FromValue, double ToValue)
            => FromValue + (ToValue - FromValue) * ProgressAnimate;
    }

    /// <summary>
    /// Преобразователь <see cref="Thickness"/> значения
    /// </summary>
    internal sealed class ThicknessInterpolator : InterpolatorValue<Thickness>
    {
        /// <summary>
        /// Типизированное преобразование
        /// </summary>
        public override Thickness InterpolateTyped(Thickness FromValue, Thickness ToValue)
            => new(
                FromValue.Left + (ToValue.Left - FromValue.Left) * ProgressAnimate,
                FromValue.Top + (ToValue.Top - FromValue.Top) * ProgressAnimate,
                FromValue.Right + (ToValue.Right - FromValue.Right) * ProgressAnimate,
                FromValue.Bottom + (ToValue.Bottom - FromValue.Bottom) * ProgressAnimate);
    }

    /// <summary>
    /// Преобразователь <see cref="CornerRadius"/> значения
    /// </summary>
    internal sealed class CornerRadiusInterpolator : InterpolatorValue<CornerRadius>
    {
        /// <summary>
        /// Типизированное преобразование
        /// </summary>
        public override CornerRadius InterpolateTyped(CornerRadius FromValue, CornerRadius ToValue)
            => new(
                FromValue.TopLeft + (ToValue.TopLeft - FromValue.TopLeft) * ProgressAnimate,
                FromValue.TopRight + (ToValue.TopRight - FromValue.TopRight) * ProgressAnimate,
                FromValue.BottomRight + (ToValue.BottomRight - FromValue.BottomRight) * ProgressAnimate,
                FromValue.BottomLeft + (ToValue.BottomLeft - FromValue.BottomLeft) * ProgressAnimate);
    }

    /// <summary>
    /// Преобразователь <see cref="Color"/> значения
    /// </summary>
    internal sealed class ColorInterpolator : InterpolatorValue<Color>
    {
        private int p;
        private unsafe byte* ptr;

        /// <summary>
        /// Типизированное преобразование
        /// </summary>
        public override unsafe Color InterpolateTyped(Color From, Color To)
        {
            p = (int)(ProgressAnimate * 255.0d + 0.5d);

            Color result = default;
            ptr = (byte*)&result;

            ptr[0] = (byte)(From.A + ((To.A - From.A) * p) / 0xFF); // A
            ptr[1] = (byte)(From.R + ((To.R - From.R) * p) / 0xFF); // R
            ptr[2] = (byte)(From.G + ((To.G - From.G) * p) / 0xFF); // G
            ptr[3] = (byte)(From.B + ((To.B - From.B) * p) / 0xFF); // B

            return result;
        }
    }

    /// <summary>
    /// Преобразователь <see cref="Rect"/> значения
    /// </summary>
    internal sealed class RectInterpolator : InterpolatorValue<Rect>
    {
        /// <summary>
        /// Типизированное преобразование
        /// </summary>
        public override Rect InterpolateTyped(Rect FromValue, Rect ToValue)
            => new(
                FromValue.X + (ToValue.Y - FromValue.Y) * ProgressAnimate,
                FromValue.Y + (ToValue.Y - FromValue.Y) * ProgressAnimate,
                FromValue.Width + (ToValue.Width - FromValue.Width) * ProgressAnimate,
                FromValue.Height + (ToValue.Height - FromValue.Height) * ProgressAnimate);
    }
}
