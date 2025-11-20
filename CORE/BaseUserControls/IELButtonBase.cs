using IEL.CORE.Classes.ObjectSettings;
using IEL.CORE.Enums;
using IEL.GUI;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace IEL.CORE.BaseUserControls
{
    /// <summary>
    /// БАЗОВЫЙ КЛАСС для отображения кнопки IEL
    /// </summary>
    public class IELButtonBase : IELContainerBase
    {
        /// <summary>
        /// Делегат события изменения поля значения
        /// </summary>
        /// <typeparam name="T_Value">Входной тип изменяемого значения</typeparam>
        /// <param name="NewValue">Новое значение</param>
        public delegate void IELSettingValueChangedHandler<T_Value>(T_Value NewValue);

        #region UIElements
        /// <summary>
        /// Главный контейнер кнопки
        /// </summary>
        private Grid Base_HeadGridButton;

        /// <summary>
        /// Левый элемент отображения направляющей
        /// </summary>
        private Border Base_LeftBorderGuideButton; // < |=|

        /// <summary>
        /// Правый элемент отображения направляющей
        /// </summary>
        private Border Base_RightBorderGuideButton; // |=| >

        /// <summary>
        /// Главный контейнер содержимого кнопки
        /// </summary>
        protected Grid Base_GridButton;
        #endregion

        #region OnActivateMouse
        /// <summary>
        /// Делегат события активации
        /// </summary>
        public delegate void ActivateHandler(object Source, MouseButtonEventArgs eventArgs, bool KeyActivate = false);

        /// <summary>
        /// Объект события активации левым щелчком мыши
        /// </summary>
        public ActivateHandler? OnActivateMouseLeft { get; set; }

        /// <summary>
        /// Объект события активации правым щелчком мыши
        /// </summary>
        public ActivateHandler? OnActivateMouseRight { get; set; }
        #endregion

        #region Properties

        #region Content
        /// <summary>
        /// Данные конкретного свойства
        /// </summary>
        public static readonly new DependencyProperty ContentProperty =
            DependencyProperty.Register("Content", typeof(UIElement), typeof(IELButtonBase),
                new(
                    (sender, e) =>
                    {
                        ((IELButtonBase)sender).Base_GridButton.Children.Insert(0, (UIElement)e.NewValue);
                    }));

        /// <summary>
        /// Внутренний элемент объекта
        /// </summary>
        public new UIElement Content
        {
            get => (UIElement)GetValue(ContentProperty);
            set => SetValue(ContentProperty, value);
        }
        #endregion

        #region CornerRadiusGuides
        /// <summary>
        /// Данные конкретного свойства
        /// </summary>
        public static readonly DependencyProperty CornerRadiusGuidesProperty =
            DependencyProperty.Register("CornerRadiusGuides", typeof(CornerRadius), typeof(IELButtonBase),
                new(new CornerRadius(0),
                    (sender, e) =>
                    {
                        ((IELButtonBase)sender).Base_LeftBorderGuideButton.CornerRadius = (CornerRadius)e.NewValue;
                        ((IELButtonBase)sender).Base_RightBorderGuideButton.CornerRadius = (CornerRadius)e.NewValue;
                    }));

        /// <summary>
        /// Скругление границ объектов направляющих кнопки
        /// </summary>
        public CornerRadius CornerRadiusGuides
        {
            get => (CornerRadius)GetValue(CornerRadiusGuidesProperty);
            set => SetValue(CornerRadiusGuidesProperty, value);
        }
        #endregion

        #region BorderThicknessGuides
        /// <summary>
        /// Данные конкретного свойства
        /// </summary>
        public static readonly DependencyProperty BorderThicknessGuidesProperty =
            DependencyProperty.Register("BorderThicknessGuides", typeof(Thickness), typeof(IELButtonBase),
                new(new Thickness(2),
                    (sender, e) =>
                    {
                        ((IELButtonBase)sender).Base_LeftBorderGuideButton.BorderThickness = (Thickness)e.NewValue;
                        ((IELButtonBase)sender).Base_RightBorderGuideButton.BorderThickness = (Thickness)e.NewValue;
                    }));

        /// <summary>
        /// Толщина границ объектов направляющей кнопки
        /// </summary>
        public Thickness BorderThicknessGuides
        {
            get => (Thickness)GetValue(BorderThicknessGuidesProperty);
            set => SetValue(BorderThicknessGuidesProperty, value);
        }
        #endregion

        #region VisualGuide
        /// <summary>
        /// Данные конкретного свойства
        /// </summary>
        public static readonly DependencyProperty VisualGuideProperty =
            DependencyProperty.Register("VisualGuide", typeof(StateVisualGuide), typeof(IELButtonBase),
                new(StateVisualGuide.Default,
                    (sender, e) =>
                    {
                        var NV = (StateVisualGuide)e.NewValue;
                        ((IELButtonBase)sender).Base_HeadGridButton.ColumnDefinitions[0].Width = new(0d,
                            NV == StateVisualGuide.LeftArrow || NV == StateVisualGuide.DuoArrow ? GridUnitType.Auto :
                            (GridUnitType.Pixel));
                        ((IELButtonBase)sender).Base_HeadGridButton.ColumnDefinitions[2].Width = new(0d,
                            NV == StateVisualGuide.RightArrow || NV == StateVisualGuide.DuoArrow ? GridUnitType.Auto : GridUnitType.Pixel);

                        ((IELButtonBase)sender).VisualGuideChanged?.Invoke(NV);
                    }));

        /// <summary>
        /// Состояние отображения направляющих кнопки
        /// </summary>
        public StateVisualGuide VisualGuide
        {
            get => (StateVisualGuide)GetValue(VisualGuideProperty);
            set => SetValue(VisualGuideProperty, value);
        }

        /// <summary>
        /// Событие изменения состояния отображения направляющих кнопки
        /// </summary>
        protected event IELSettingValueChangedHandler<StateVisualGuide>? VisualGuideChanged;
        #endregion

        #endregion

        /// <summary>
        /// Инициализация базового класса визуализации кнопки IEL
        /// </summary>
        protected IELButtonBase()
        {
            Base_HeadGridButton = new()
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
            };
            Base_HeadGridButton.ColumnDefinitions.Add(new() { Width = new(0d, GridUnitType.Pixel) });
            Base_HeadGridButton.ColumnDefinitions.Add(new() { Width = new(1d, GridUnitType.Star) });
            Base_HeadGridButton.ColumnDefinitions.Add(new() { Width = new(0d, GridUnitType.Pixel) });

            Base_LeftBorderGuideButton = new() // < |=|
            {
                Width = 20d,
                Height = 20d,
                Margin = new(2),
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Center,
                CornerRadius = new(0),
                BorderThickness = new(2),
                BorderBrush = SourceBorderBrush.InicializeConnectedSolidColorBrush(),
                Child = new TextBlock()
                {
                    Padding = new(0d, 0.7d, 1d, 0d),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Foreground = SourceForeground.InicializeConnectedSolidColorBrush(),
                    Text = "<",
                    FontFamily = new("Arial Black"),
                }
            };
            Base_RightBorderGuideButton = new() // |=| >
            {
                Width = 20d,
                Height = 20d,
                Margin = new(2),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
                CornerRadius = new(0),
                BorderThickness = new(2),
                BorderBrush = SourceBorderBrush.InicializeConnectedSolidColorBrush(),
                Child = new TextBlock()
                {
                    Padding = new(1d, 0.7d, 0d, 0d),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Foreground = SourceForeground.InicializeConnectedSolidColorBrush(),
                    Text = ">",
                    FontFamily = new("Arial Black"),
                }
            };

            Base_GridButton = new()
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
            };
            Grid.SetColumn(Base_LeftBorderGuideButton, 0);
            Grid.SetColumn(Base_GridButton, 1);
            Grid.SetColumn(Base_RightBorderGuideButton, 2);
            Base_HeadGridButton.Children.Add(Base_LeftBorderGuideButton);
            Base_HeadGridButton.Children.Add(Base_GridButton);
            Base_HeadGridButton.Children.Add(Base_RightBorderGuideButton);


            Base_BorderContainer.MouseDown += (sender, e) =>
            {
                if (IsEnabled)
                {
                    if (
                    (e.LeftButton == MouseButtonState.Pressed && OnActivateMouseLeft != null) ||
                    (e.RightButton == MouseButtonState.Pressed && OnActivateMouseRight != null))
                    {
                        SetActiveSpecrum(StateSpectrum.Used, false);
                        SourceTimer.Stop();
                    }
                }
            };

            Base_BorderContainer.MouseLeftButtonUp += (sender, e) =>
            {
                if (IsEnabled && OnActivateMouseLeft != null)
                {
                    SetActiveSpecrum(StateSpectrum.Select, true);
                    OnActivateMouseLeft.Invoke(this, e);
                }
            };

            Base_BorderContainer.MouseRightButtonUp += (sender, e) =>
            {
                if (IsEnabled && OnActivateMouseRight != null)
                {
                    SetActiveSpecrum(StateSpectrum.Select, true);
                    OnActivateMouseRight.Invoke(this, e);
                }
            };

            base.SetValue(IELContainerBase.ContentProperty, Base_HeadGridButton);
        }

        /// <summary>
        /// Узнать тип доступных нажатий на элемент
        /// </summary>
        /// <returns>Объект пересичления возможных нажатий</returns>
        private EventsMouse GetSourceEventMouse()
        {
            if (OnActivateMouseLeft != null)
            {
                if (OnActivateMouseRight != null) return EventsMouse.Full;
                return EventsMouse.Left;
            }
            else if (OnActivateMouseRight != null) return EventsMouse.Right;
            return EventsMouse.Not;
        }
    }
}
