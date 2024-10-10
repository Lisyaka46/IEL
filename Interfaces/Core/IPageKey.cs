using IEL.Classes;
using IEL.Interfaces.Front;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;

namespace IEL.Interfaces.Core
{
    public interface IPageKey : IPage
    {
        /// <summary>
        /// Перечисление вариаций активации кнопки по клавише
        /// </summary>
        public enum OrientationActivate
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
        public enum ActionButton
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

        /// <summary>
        /// Объект состояния режима клавиатуры <b>БЕЗ СОБЫТИЯ ИЗМЕНЕНИЯ</b>
        /// </summary>
        public bool KeyboardMode { get; set; }

        /// <summary>
        /// Делегат события изменения состояния режима клавиатуры
        /// </summary>
        /// <param name="ModeChanged">Новое значение Alt режима</param>
        public delegate void Delegate_KeyboardModeChanged(bool ModeChanged);

        /// <summary>
        /// Объект события режима клавиатуры
        /// </summary>
        public Delegate_KeyboardModeChanged? KeyboardModeChanged { get; }

        /// <summary>
        /// Найти элемент поддерживающий клавишу в странице
        /// </summary>
        /// <typeparam name="T">Тип элемента поддерживающего подключение по клавише</typeparam>
        /// <param name="VisualObject">Ссылка на объект поиска</param>
        /// <param name="key">Ключ клавиши</param>
        private static T? SearchButton<T>(Visual VisualObject, Key key) where T : IIELButtonKey
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(VisualObject); i++)
            {
                Visual ChildVisualElement = (Visual)VisualTreeHelper.GetChild(VisualObject, i);
                try
                {
                    T ObjectButton = (T)(IIELButtonKey)ChildVisualElement;
                    if (ObjectButton.CharKeyKeyboard == key && ObjectButton.IsEnabled) return ObjectButton;
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
        /// <param name="SearchPage">Объект интерфейса в котором находится элемент</param>
        /// <param name="key">Клавиша которую нажали</param>
        /// <param name="ElementAction">Событие которое нужно совершить над объектом</param>
        /// <param name="Orientation">Ориентация нажатия на кнопку</param>
        /// <remarks>Производится поиск и реализация действия над объектом</remarks>
        /// <returns>Выполнилось удачно или нет</returns>
        internal virtual bool ActivateElementKey<T>(Page SearchPage, Key key, ActionButton ElementAction,
            OrientationActivate Orientation = OrientationActivate.LeftButton) where T : IIELButtonKey
        {
            T? Button = SearchButton<T>((Visual)SearchPage.Content, key);
            if (Button == null) return false;
            switch (ElementAction)
            {
                case ActionButton.ActionActivate:
                    if (Orientation == OrientationActivate.LeftButton) Button.OnActivateMouseLeft?.Invoke(true);
                    else if (Orientation == OrientationActivate.RightButton) Button.OnActivateMouseRight?.Invoke(true);
                    break;
                case ActionButton.BlinkActivate:
                    Button.BlinkAnimation();
                    break;
            }
            return true;
        }
    }
}
