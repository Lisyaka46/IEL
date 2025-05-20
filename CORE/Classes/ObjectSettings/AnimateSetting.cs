using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace IEL.CORE.Classes.ObjectSettings
{
    /// <summary>
    /// Класс полей анимации
    /// </summary>
    internal class AnimateSetting
    {
        #region animateObjects
        #region ColorAnimation
        private readonly ColorAnimation _AnimationColor;
        public ColorAnimation GetAnimationColor() => _AnimationColor.Clone();
        public ColorAnimation GetAnimationColor(Color To)
        {
            ColorAnimation animation = _AnimationColor.Clone();
            animation.To = To;
            return animation;
        }
        public ColorAnimation GetAnimationColor(Color To, TimeSpan Duration)
        {
            ColorAnimation animation = _AnimationColor.Clone();
            animation.To = To;
            animation.Duration = Duration;
            return animation;
        }
        public ColorAnimation GetAnimationColor(Color From, Color To)
        {
            ColorAnimation animation = _AnimationColor.Clone();
            animation.From = From;
            animation.To = To;
            return animation;
        }
        public ColorAnimation GetAnimationColor(Color From, Color To, TimeSpan Duration)
        {
            ColorAnimation animation = _AnimationColor.Clone();
            animation.From = From;
            animation.To = To;
            animation.Duration = Duration;
            return animation;
        }
        #endregion

        #region ThicknessAnimation
        private readonly ThicknessAnimation _AnimationThickness;
        public ThicknessAnimation GetAnimationThickness() => _AnimationThickness.Clone();
        public ThicknessAnimation GetAnimationThickness(Thickness To)
        {
            ThicknessAnimation animation = _AnimationThickness.Clone();
            animation.To = To;
            return animation;
        }
        public ThicknessAnimation GetAnimationThickness(Thickness To, TimeSpan Duration)
        {
            ThicknessAnimation animation = _AnimationThickness.Clone();
            animation.To = To;
            animation.Duration = Duration;
            return animation;
        }
        public ThicknessAnimation GetAnimationThickness(Thickness From, Thickness To)
        {
            ThicknessAnimation animation = _AnimationThickness.Clone();
            animation.From = From;
            animation.To = To;
            return animation;
        }
        public ThicknessAnimation GetAnimationThickness(Thickness From, Thickness To, TimeSpan Duration)
        {
            ThicknessAnimation animation = _AnimationThickness.Clone();
            animation.From = From;
            animation.To = To;
            animation.Duration = Duration;
            return animation;
        }
        #endregion

        #region DoubleAnimation
        private readonly DoubleAnimation _AnimationDouble;
        /// <summary>
        /// Анимация double значения
        /// </summary>
        public DoubleAnimation GetAnimationDouble() => _AnimationDouble.Clone();
        public DoubleAnimation GetAnimationDouble(double To)
        {
            DoubleAnimation animation = _AnimationDouble.Clone();
            animation.To = To;
            return animation;
        }
        public DoubleAnimation GetAnimationDouble(double To, TimeSpan Duration)
        {
            DoubleAnimation animation = _AnimationDouble.Clone();
            animation.To = To;
            animation.Duration = Duration;
            return animation;
        }
        public DoubleAnimation GetAnimationDouble(double From, double To)
        {
            DoubleAnimation animation = _AnimationDouble.Clone();
            animation.From = From;
            animation.To = To;
            return animation;
        }
        public DoubleAnimation GetAnimationDouble(double From, double To, TimeSpan Duration)
        {
            DoubleAnimation animation = _AnimationDouble.Clone();
            animation.From = From;
            animation.To = To;
            animation.Duration = Duration;
            return animation;
        }
        #endregion
        #endregion

        #region AnimationMillisecond
        private double _AnimationMillisecond;
        /// <summary>
        /// Длительность анимации объекта в миллисекундах
        /// </summary>
        public double AnimationMillisecond
        {
            get => _AnimationMillisecond;
            set
            {
                TimeSpan time = TimeSpan.FromMilliseconds(value);
                _AnimationColor.Duration = time;
                _AnimationThickness.Duration = time;
                _AnimationDouble.Duration = time;
                _AnimationMillisecond = value;

            }
        }
        #endregion

        /// <summary>
        /// Инициализировать поля анимации по умолчанию
        /// </summary>
        public AnimateSetting()
        {
            double Deceleration = 0.2d;
            IEasingFunction DefaultEasingFunction = new QuinticEase() { EasingMode = EasingMode.EaseOut };
            _AnimationColor = new()
            {
                DecelerationRatio = Deceleration,
                EasingFunction = DefaultEasingFunction
            };
            _AnimationThickness = new()
            {
                DecelerationRatio = Deceleration,
                EasingFunction = DefaultEasingFunction
            };
            _AnimationDouble = new()
            {
                DecelerationRatio = Deceleration,
                EasingFunction = DefaultEasingFunction
            };
            AnimationMillisecond = 160d;
        }
    }
}
