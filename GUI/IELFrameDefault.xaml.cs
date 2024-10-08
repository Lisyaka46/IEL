using IEL.Interfaces.Core;
using IEL.Interfaces.Front;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using static IEL.Interfaces.Front.IIELFrame;

namespace IEL
{
    /// <summary>
    /// Логика взаимодействия для IELFrame.xaml
    /// </summary>
    public partial class IELFrameDefault : UserControl, IIELFrameDefault
    {
        /// <summary>
        /// Индекс смены окна страницы
        /// </summary>
        public int PanelVerschachtelung { get; private set; } = 0;

        /// <summary>
        /// Объект актуального окна страницы
        /// </summary>
        public Frame ActualFrame => PanelVerschachtelung % 2 == 0 ? ref FrameActionPanelLeft : ref FrameActionPanelRight;

        /// <summary>
        /// Объект предыдущего окна страницы
        /// </summary>
        public Frame BackFrame => !(PanelVerschachtelung % 2 == 0) ? ref FrameActionPanelLeft : ref FrameActionPanelRight;

        /// <summary>
        /// Объект анимации для управления размерами панели действий
        /// </summary>
        private static readonly DoubleAnimation DoubleAnimateObj = new(0, TimeSpan.FromMilliseconds(300d))
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
        /// Событие закрытия фрейма
        /// </summary>
        public event IELFrameEventHandler? ClosingFrame;

        /// <summary>
        /// Событие изменения активной страницы
        /// </summary>
        public event IELFrameChangedEventHandler? ChangeElementPage;

        /// <summary>
        /// Событие открытия фрейма
        /// </summary>
        public event IELFrameEventHandler? OpenFrame;

        //
        private bool Activate = false;

        public IELFrameDefault()
        {
            InitializeComponent();
            //FrameActionPanelLeft.NavigationUIVisibility = NavigationUIVisibility.Hidden;
            //FrameActionPanelRight.NavigationUIVisibility = NavigationUIVisibility.Hidden;
            FrameActionPanelLeft.Opacity = 0d;
            //FrameActionPanelRight.Opacity = 0d;
        }

        /// <summary>
        /// Перенаправить страницу
        /// </summary>
        /// <param name="Content">Новая страница</param>
        /// <param name="Orientation">Ориентация движения</param>
        public void NextPage([NotNull()] IPageDefault Content, OrientationMove Orientation = OrientationMove.Right)
        {
            try
            {
                if (ActualFrame.Content != null)
                {
                    if (((IPageDefault)ActualFrame.Content).ModulePage.ModuleName.Equals(Content.ModulePage.ModuleName))
                    {
                        if (Activate) return;
                        else
                        {
                            Activate = true;
                            DoubleAnimateObj.To = 1d;
                            ActualFrame.BeginAnimation(OpacityProperty, DoubleAnimateObj);
                            OpenFrame?.Invoke();
                            return;
                        }
                    }
                }
            }
            catch { }
            PanelVerschachtelung = (PanelVerschachtelung + 1) % 2;

            if (!Activate)
            {
                Activate = true;
                OpenFrame?.Invoke();
            }
            ActualFrame.Opacity = 0d;
            BackFrame.IsEnabled = false;
            ActualFrame.IsEnabled = true;
            ActualFrame.BeginAnimation(MarginProperty, null);
            ActualFrame.Margin = Orientation == OrientationMove.Left ? new(-20, -20, 40, -3) : new(40, -10, -20, -3);

            //Content.ModulePage.KeyboardMode = BackPage.ModulePage.KeyboardMode;
            //BackPage.ModulePage.KeyboardMode = false;
            ActualFrame.Navigate(Content);

            DoubleAnimateObj.To = 0d;
            BackFrame.BeginAnimation(OpacityProperty, DoubleAnimateObj);
            ThicknessAnimate.To = Orientation == OrientationMove.Left ? new(40, -20, -20, -3) : new(-20, -20, 40, -3);
            BackFrame.BeginAnimation(MarginProperty, ThicknessAnimate);

            DoubleAnimateObj.To = 1d;
            ActualFrame.BeginAnimation(OpacityProperty, DoubleAnimateObj);
            ThicknessAnimate.To = new(0);
            ActualFrame.BeginAnimation(MarginProperty, ThicknessAnimate);
            ChangeElementPage?.Invoke(Content.ModulePage.ModuleName);
        }

        /// <summary>
        /// Закрыть элемент фрейма
        /// </summary>
        public void CloseFrame()
        {
            DoubleAnimation Anim = DoubleAnimateObj.Clone();
            void Close(object? sender, EventArgs e)
            {
                Anim.Completed -= Close;
                //ActualFrame.Navigate(null);
            }

            Anim.To = 0d;
            Anim.FillBehavior = FillBehavior.Stop;
            Anim.Completed += Close;
            Activate = false;
            ActualFrame.BeginAnimation(OpacityProperty, Anim);
            ClosingFrame?.Invoke();
        }
    }
}
