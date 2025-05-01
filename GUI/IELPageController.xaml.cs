using IEL.Interfaces.Core;
using IEL.Interfaces.Front;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace IEL
{
    /// <summary>
    /// Логика взаимодействия для IELPageController.xaml
    /// </summary>
    public partial class IELPageController : UserControl, IIELObject
    {
        /// <summary>
        /// Объект анимации для управления размерами панели действий
        /// </summary>
        private static readonly DoubleAnimation DoubleAnimate = new(0, TimeSpan.FromMilliseconds(300d))
        {
            DecelerationRatio = 0.6d,
            EasingFunction = new CubicEase() { EasingMode = EasingMode.EaseOut }
        };

        /// <summary>
        /// Объект анимации для управления позицией
        /// </summary>
        private static readonly ThicknessAnimation ThicknessAnimate = new(new Thickness(0), TimeSpan.FromMilliseconds(300d))
        {
            DecelerationRatio = 0.6d,
            EasingFunction = new PowerEase() { EasingMode = EasingMode.EaseOut }
        };

        /// <summary>
        /// Индекс смены окна страницы
        /// </summary>
        private int PanelVerschachtelung = 0;

        /// <summary>
        /// Объект актуального окна страницы
        /// </summary>
        internal Frame ActualFrame => PanelVerschachtelung % 2 == 0 ? ref FrameActionPanelLeft : ref FrameActionPanelRight;

        /// <summary>
        /// Объект предыдущего окна страницы
        /// </summary>
        internal Frame BackFrame => !(PanelVerschachtelung % 2 == 0) ? ref FrameActionPanelLeft : ref FrameActionPanelRight;

        /// <summary>
        /// Объект актуальной страницы
        /// </summary>
        internal object ActualPage => ActualFrame.Content;

        /// <summary>
        /// Объект предыдущей страницы
        /// </summary>
        internal object BackPage => BackFrame.Content;

        /// <summary>
        /// Левая анимация переключателя
        /// </summary>
        /// <remarks>
        /// Используется позиция настройки при левом переключении страниц
        /// </remarks>
        public Thickness LeftAnimateSwitch { get; set; }

        /// <summary>
        /// Правая анимация переключателя
        /// </summary>
        /// <remarks>
        /// Используется позиция настройки при правом переключении страниц
        /// </remarks>
        public Thickness RightAnimateSwitch { get; set; }

        public new HorizontalAlignment HorizontalAlignment
        {
            get
            {
                return base.HorizontalAlignment;
            }
            set
            {
                base.HorizontalAlignment = value;
                FrameActionPanelLeft.HorizontalAlignment = value;
                FrameActionPanelRight.HorizontalAlignment = value;
            }
        }

        public new VerticalAlignment VerticalAlignment
        {
            get
            {
                return base.VerticalAlignment;
            }
            set
            {
                base.VerticalAlignment = value;
                FrameActionPanelLeft.VerticalAlignment = value;
                FrameActionPanelRight.VerticalAlignment = value;
            }
        }

        public IELPageController()
        {
            InitializeComponent();
            LeftAnimateSwitch = new(0);
            RightAnimateSwitch = new(0);
        }

        /// <summary>
        /// Перенаправить страницу
        /// </summary>
        /// <param name="Content">Новая страница</param>
        /// <param name="RightAlign">Правая ориентация движения</param>
        public void NextPage([NotNull()] Page Content, bool RightAlign = true)
        {
            DoubleAnimation animation_double = DoubleAnimate.Clone();
            ThicknessAnimation animation_thickness = ThicknessAnimate.Clone();
            PanelVerschachtelung = (PanelVerschachtelung + 1) % 2;

            ActualFrame.Opacity = 0d;
            Canvas.SetZIndex(BackFrame, 0);
            Canvas.SetZIndex(ActualFrame, 1);
            BackFrame.IsEnabled = false;
            ActualFrame.IsEnabled = true;
            ActualFrame.BeginAnimation(MarginProperty, null);
            ActualFrame.Margin = !RightAlign ? LeftAnimateSwitch : RightAnimateSwitch;
            //Content.KeyboardMode = BackPage.KeyboardMode;
            //BackPage.KeyboardMode = false;
            Content.HorizontalAlignment = HorizontalAlignment;
            Content.VerticalAlignment = VerticalAlignment;
            ActualFrame.Navigate(Content);

            animation_thickness.To = !RightAlign ? RightAnimateSwitch : LeftAnimateSwitch;
            BackFrame.BeginAnimation(MarginProperty, animation_thickness);
            animation_thickness.To = new(0);
            ActualFrame.BeginAnimation(MarginProperty, animation_thickness);

            animation_double.To = 0d;
            BackFrame.BeginAnimation(OpacityProperty, animation_double);
            animation_double.To = 1d;
            ActualFrame.BeginAnimation(OpacityProperty, animation_double);
        }

        /// <summary>
        /// Переместить текущую страницу к определённой точке
        /// </summary>
        /// <param name="MoveTo">Точка перемещения</param>
        /// <param name="Millisecond">Время анимации в миллисекундах</param>
        public void MoveActualPage(Thickness MoveTo, uint Millisecond = 100u)
        {
            ThicknessAnimation animation_thickness = ThicknessAnimate.Clone();
            animation_thickness.To = MoveTo;
            animation_thickness.Duration = TimeSpan.FromMilliseconds(Millisecond);
            ActualFrame.BeginAnimation(MarginProperty, animation_thickness);
        }

        /// <summary>
        /// Закрыть элемент фрейма
        /// </summary>
        public void ClosePage()
        {
            BackFrame.Navigate(null);
            BackFrame.Source = null;
            ActualFrame.Navigate(null);
            ActualFrame.Source = null;
            //Activate = false;
            //ClosingFrame?.Invoke();
        }
    }
}
