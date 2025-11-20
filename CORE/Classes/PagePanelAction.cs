using IEL.CORE.BaseUserControls;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace IEL.CORE.Classes
{
    #region Enum
    /// <summary>
    /// Перечисление вариаций активации кнопки по клавише
    /// </summary>
    internal enum OrientationActivate
    {
        /// <summary>
        /// Левая активация кнопки
        /// </summary>
        LeftButton = 0,

        /// <summary>
        /// Правая активация кнопки
        /// </summary>
        RightButton = 1,
    }

    /// <summary>
    /// Какое действие совершить
    /// </summary>
    internal enum ActionButton
    {
        /// <summary>
        /// Активация действия
        /// </summary>
        ActionActivate = 0,

        /// <summary>
        /// Активация мерцания
        /// </summary>
        BlinkActivate = 1,
    }
    #endregion

    /// <summary>
    /// Инициализатор класса управления страницей
    /// </summary>
    /// <param name="DefaultSource">Страница данных</param>
    public class PagePanelAction(Page DefaultSource)
    {
        #region ElementChangedHandler
        /// <summary>
        /// Делегат события изменения параметра панели действий
        /// </summary>
        /// <typeparam name="T_Value">Тип изменяемого параметра</typeparam>
        /// <param name="Source"></param>
        /// <param name="NewValue">Новое значение параметра</param>
        public delegate void ElementChangedHandler<T_Value>(Page Source, T_Value NewValue);

        /// <summary>
        /// Событие изменения состояния управления с помощью клавиатуры
        /// </summary>
        public event ElementChangedHandler<bool>? IsKeyboardModeChanged;
        #endregion

        #region IsKeyboardMode
        private bool _IsKeyboardMode = false;
        /// <summary>
        /// Состояние управления с помощью клавиатуры
        /// </summary>
        public bool IsKeyboardMode
        {
            get => _IsKeyboardMode;
            set
            {
                IsKeyboardModeChanged?.Invoke(ObjectPage, value);
                _IsKeyboardMode = value;
            }
        }
        #endregion

        /// <summary>
        /// Страница управления
        /// </summary>
        public Page ObjectPage { get; } = DefaultSource;

        /// <summary>
        /// Найти элемент поддерживающий клавишу в странице
        /// </summary>
        /// <typeparam name="T">Тип элемента поддерживающего подключение по клавише</typeparam>
        /// <param name="VisualObject">Ссылка на объект поиска</param>
        /// <param name="key">Ключ клавиши</param>
        private static T? SearchButton<T>(Visual VisualObject, Key key) where T : IELButtonKeyBase
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(VisualObject); i++)
            {
                Visual ChildVisualElement = (Visual)VisualTreeHelper.GetChild(VisualObject, i);
                try
                {
                    T ObjectButton = (T)ChildVisualElement;
                    //if (ObjectButton.CharKeyKeyboard == key) return ObjectButton; // && ObjectButton.IsEnabled
                }
                catch
                {
                    if (ChildVisualElement.GetType().Name.Equals("Grid"))
                    {
                        T? Returing = SearchButton<T>((Grid)ChildVisualElement, key);
                        if (Returing != null) return Returing;
                        else continue;
                    }
                }
            }
            return default;
        }

        /// <summary>
        /// Активировать кнопку в данном элементе типа "IIELButtonKey" с помощью клавиши
        /// </summary>
        /// <param name="key">Клавиша которую нажали</param>
        /// <param name="ElementAction">Событие которое нужно совершить над объектом</param>
        /// <param name="Orientation">Ориентация нажатия на кнопку</param>
        /// <remarks>Производится поиск и реализация действия над объектом</remarks>
        /// <returns>Выполнилось удачно или нет</returns>
        internal bool ActivateElementKey<T>(Key key, ActionButton ElementAction,
            OrientationActivate Orientation = OrientationActivate.LeftButton) where T : IELButtonKeyBase
        {
            T? Button = SearchButton<T>((Visual)ObjectPage.Content, key);
            if (Button == null) return false;
            switch (ElementAction)
            {
                case ActionButton.ActionActivate:
                    Button.UnfocusAnimation();
                    if (Orientation == OrientationActivate.LeftButton)
                        Button.OnActivateMouseLeft?.Invoke(Button, new MouseButtonEventArgs(Mouse.PrimaryDevice, 0, MouseButton.Left), true);
                    else if (Orientation == OrientationActivate.RightButton)
                        Button.OnActivateMouseRight?.Invoke(Button, new MouseButtonEventArgs(Mouse.PrimaryDevice, 0, MouseButton.Right), true);
                    break;
                case ActionButton.BlinkActivate:
                    Button.BlinkAnimation();
                    break;
            }
            return true;
        }
    }
}
