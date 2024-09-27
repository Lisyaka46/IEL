using System.Windows.Controls;
using System.Windows.Input;
using IEL.Interfaces.Front;

namespace IEL.Interfaces.Core
{
    public interface IModulePageKey : IModulePage
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
        internal Delegate_KeyboardModeChanged? KeyboardModeChanged { get; }

        /// <summary>
        /// Активировать кнопку в данном элементе типа "IIELButtonKey" с помощью клавиши
        /// </summary>
        /// <param name="VisualObject">Объект интерфейса в котором находится элемент</param>
        /// <param name="key">Клавиша которую нажали</param>
        /// <param name="ElementAction">Событие которое нужно совершить над объектом</param>
        /// <param name="Orientation">Ориентация нажатия на кнопку</param>
        /// <remarks>Производится поиск и реализация действия над объектом</remarks>
        /// <returns>Выполнилось удачно или нет</returns>
        internal bool ActivateElementKey<T>(Grid VisualObject, Key key, ActionButton ElementAction,
            OrientationActivate Orientation = OrientationActivate.LeftButton) where T : IIELButtonKey;
    }
}
