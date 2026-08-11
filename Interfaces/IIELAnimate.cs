using IEL.CORE.Animation;

namespace IEL.Interfaces
{
    /// <summary>
    /// Интерфейс объекта реализующего настройку менеджера анимаций OPL
    /// </summary>
    public interface IIELAnimate
    {
        /// <summary>
        /// Объект менеджера анимаций настроек OPL
        /// </summary>
        public abstract AnimationManager? ManagerAnimation { get; set; }
    }
}
