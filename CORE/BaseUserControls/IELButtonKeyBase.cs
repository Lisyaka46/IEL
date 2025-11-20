using IEL.CORE.Enums;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace IEL.CORE.BaseUserControls
{
    /// <summary>
    /// БАЗОВЫЙ КЛАСС для отображения кнопки IEL с возможностью манипуляции клавиатурой
    /// </summary>
    public class IELButtonKeyBase : IELButtonBase
    {

        #region UIElements
        /// <summary>
        /// Главный контейнер кнопки
        /// </summary>
        private Grid Base_HeadGridButtonKey;

        /// <summary>
        /// Правый элемент отображения направляющей
        /// </summary>
        private Border Base_BorderKey; // | |0|EL| >

        /// <summary>
        /// Левый элемент отображения направляющей
        /// </summary>
        private TextBlock Base_TextCharKey; // | 0| <> | |

        /// <summary>
        /// Главный контейнер содержимого кнопки
        /// </summary>
        protected Grid Base_GridButtonKey;
        #endregion

        #region Properties

        #region Content
        /// <summary>
        /// Данные конкретного свойства
        /// </summary>
        public static readonly new DependencyProperty ContentProperty =
            DependencyProperty.Register("Content", typeof(UIElement), typeof(IELButtonKeyBase),
                new(
                    (sender, e) =>
                    {
                        ((IELButtonKeyBase)sender).Base_GridButtonKey.Children.Insert(0, (UIElement)e.NewValue);
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

        #region CornerRadiusKey
        /// <summary>
        /// Данные конкретного свойства
        /// </summary>
        public static readonly DependencyProperty CornerRadiusKeyProperty =
            DependencyProperty.Register("CornerRadiusKey", typeof(CornerRadius), typeof(IELButtonKeyBase),
                new(new CornerRadius(0),
                    (sender, e) =>
                    {
                        ((IELButtonKeyBase)sender).Base_BorderKey.CornerRadius = (CornerRadius)e.NewValue;
                    }));

        /// <summary>
        /// Скругление границ объекта отображения управляющей клавиши
        /// </summary>
        public CornerRadius CornerRadiusKey
        {
            get => (CornerRadius)GetValue(CornerRadiusKeyProperty);
            set => SetValue(CornerRadiusKeyProperty, value);
        }
        #endregion

        #region BorderThicknessKey
        /// <summary>
        /// Данные конкретного свойства
        /// </summary>
        public static readonly DependencyProperty BorderThicknessKeyProperty =
            DependencyProperty.Register("BorderThicknessKey", typeof(Thickness), typeof(IELButtonKeyBase),
                new(new Thickness(2),
                    (sender, e) =>
                    {
                        ((IELButtonKeyBase)sender).Base_BorderKey.BorderThickness = (Thickness)e.NewValue;
                    }));

        /// <summary>
        /// Толщина границ объекта отображения управляющей клавиши
        /// </summary>
        public Thickness BorderThicknessKey
        {
            get => (Thickness)GetValue(BorderThicknessKeyProperty);
            set => SetValue(BorderThicknessKeyProperty, value);
        }
        #endregion

        #region KeyActivateButton
        /// <summary>
        /// Данные конкретного свойства
        /// </summary>
        public static readonly DependencyProperty KeyActivateButtonProperty =
            DependencyProperty.Register("KeyActivateButton", typeof(Key?), typeof(IELButtonKeyBase),
                new(null,
                    (sender, e) =>
                    {
                        ((IELButtonKeyBase)sender).Base_TextCharKey.Text = GetKeyName((Key?)e.NewValue).ToString();
                    }));

        /// <summary>
        /// Клавиша которая активирует кнопку
        /// </summary>
        public Key? KeyActivateButton
        {
            get => (Key?)GetValue(KeyActivateButtonProperty);
            set => SetValue(KeyActivateButtonProperty, value);
        }
        #endregion

        #region IsVisibleKeyActivate
        /// <summary>
        /// Данные конкретного свойства
        /// </summary>
        public static readonly DependencyProperty IsVisibleKeyActivateProperty =
            DependencyProperty.Register("IsVisibleKeyActivate", typeof(bool), typeof(IELButtonKeyBase),
                new(true,
                    (sender, e) =>
                    {
                        ((IELButtonKeyBase)sender).Base_HeadGridButtonKey.ColumnDefinitions[0].Width =
                            new(0d, (bool)e.NewValue ? GridUnitType.Auto : GridUnitType.Pixel);
                        ((IELButtonKeyBase)sender).IsVisibleKeyActivateChanged?.Invoke(sender, e);
                    }));

        /// <summary>
        /// Состояние видимости клавиши активации кнопки
        /// </summary>
        public bool IsVisibleKeyActivate
        {
            get => (bool)GetValue(IsVisibleKeyActivateProperty);
            set => SetValue(IsVisibleKeyActivateProperty, value);
        }

        /// <summary>
        /// Событие изменения состояния видимости клавиши активации кнопки
        /// </summary>
        public event DependencyPropertyChangedEventHandler? IsVisibleKeyActivateChanged;
        #endregion

        #region FontFamily
        /// <summary>
        /// Данные конкретного свойства
        /// </summary>
        public static readonly new DependencyProperty FontFamilyProperty =
            DependencyProperty.Register("FontFamily", typeof(FontFamily), typeof(IELButtonKeyBase),
                new(new FontFamily("Calibri"),
                    (sender, e) =>
                    {
                        ((IELButtonKeyBase)sender).Base_TextCharKey.FontFamily = (FontFamily)e.NewValue;
                        ((IELButtonKeyBase)sender).Base_GridButtonKey.SetValue(TextBlock.FontFamilyProperty, e.NewValue);
                    }));

        /// <summary>
        /// Шрифт используемый объектом
        /// </summary>
        public new FontFamily FontFamily
        {
            get => (FontFamily)GetValue(FontFamilyProperty);
            set => SetValue(FontFamilyProperty, value);
        }
        #endregion

        #endregion

        /// <summary>
        /// Инициализировать <b>БАЗОВОЕ ПРЕДСТАВЛЕНИЕ</b> кнопки IEL с позможностью управления клавиатурой
        /// </summary>
        public IELButtonKeyBase()
        {
            Base_GridButton.HorizontalAlignment = HorizontalAlignment.Stretch;
            Base_HeadGridButtonKey = new()
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
            };
            Base_HeadGridButtonKey.ColumnDefinitions.Add(new() { Width = new(0d, GridUnitType.Auto) });
            Base_HeadGridButtonKey.ColumnDefinitions.Add(new() { Width = new(1d, GridUnitType.Star) });

            Base_BorderKey = new()
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new(2),
                BorderThickness = new(2),
                CornerRadius = new(0),
                BorderBrush = SourceBorderBrush.InicializeConnectedSolidColorBrush(),
            };
            Base_TextCharKey = new()
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Text = "DEL",
                Padding = new(5, 0, 5, 0),
                Foreground = SourceForeground.InicializeConnectedSolidColorBrush(),
            };
            Base_BorderKey.Child = Base_TextCharKey;

            Grid.SetColumn(Base_BorderKey, 0);
            Base_HeadGridButtonKey.Children.Add(Base_BorderKey);

            Base_GridButtonKey = new();
            Grid.SetColumn(Base_GridButtonKey, 1);
            Base_HeadGridButtonKey.Children.Add(Base_GridButtonKey);


            base.SetValue(IELButtonBase.ContentProperty, Base_HeadGridButtonKey);
        }

        /// <summary>
        /// Анимация мерцания
        /// </summary>
        [MTAThread()]
        public void BlinkAnimation()
        {
            SetActiveSpecrum(StateSpectrum.Used, false);

            SetActiveSpecrum(StateSpectrum.Select, true);
        }

        /// <summary>
        /// Анимация мерцания
        /// </summary>
        [MTAThread()]
        public void UnfocusAnimation()
        {
            SetActiveSpecrum(StateSpectrum.Default, true);
        }

        private static char GetKeyName(Key? key)
        {
            return '?';
        }
    }
}
