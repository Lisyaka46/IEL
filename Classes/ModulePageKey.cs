using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using IEL.Interfaces.Core;
using IEL.Interfaces.Front;

namespace IEL.Classes
{
    public class ModulePageKey(string Name) : IModulePageKey
    {
        /// <summary>
        /// Объект данных режима клавиатуры
        /// </summary>
        private bool _KeyboardMode;

        /// <summary>
        /// Имя страницы
        /// </summary>
        public string ModuleName { get; } = Name;

        /// <summary>
        /// Режим клавиатуры
        /// </summary>
        public bool KeyboardMode
        {
            get => _KeyboardMode;
            set
            {
                _KeyboardMode = value;
                KeyboardModeChanged?.Invoke(value);
            }
        }

        /// <summary>
        /// Объект события изменения состояния Alt режима
        /// </summary>
        public IModulePageKey.Delegate_KeyboardModeChanged? KeyboardModeChanged { get; set; }

        /// <summary>
        /// Найти элемент поддерживающий клавишу в странице
        /// </summary>
        /// <typeparam name="T">Тип элемента поддерживающего подключение по клавише</typeparam>
        /// <param name="VisualObject">Ссылка на объект поиска</param>
        /// <param name="key">Ключ клавиши</param>
        private static T? SearchButton<T>(Grid VisualObject, Key key) where T : IIELButtonKey
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
        /// Активировать элемент поддерживающий клавишу в странице
        /// </summary>
        /// <param name="VisualObject">Ссылка на объект поиска</param>
        /// <param name="key">Ключ клавиши</param>
        /// <param name="Orientation">Ориентация нажатия</param>
        /// <typeparam name="T">Тип элемента поддерживающего подключение по клавише</typeparam>
        /// <returns>Выполнилась удачно или нет функция</returns>
        public bool ActivateElementKey<T>(Grid VisualObject, Key key, IModulePageKey.ActionButton ElementAction,
            IModulePageKey.OrientationActivate Orientation = IModulePageKey.OrientationActivate.LeftButton) where T : IIELButtonKey
        {
            T? Button = SearchButton<T>(VisualObject, key);
            if (Button == null) return false;
            switch (ElementAction)
            {
                case IModulePageKey.ActionButton.ActionActivate:
                    if (Orientation == IModulePageKey.OrientationActivate.LeftButton) Button.OnActivateMouseLeft?.Invoke(true);
                    else if (Orientation == IModulePageKey.OrientationActivate.RightButton) Button.OnActivateMouseRight?.Invoke(true);
                    break;
                case IModulePageKey.ActionButton.BlinkActivate:
                    Button.BlinkAnimation();
                    break;
            }
            return true;
        }
    }
}
