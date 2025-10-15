using System.Windows.Threading;

namespace IEL.CORE.Classes.ObjectSettings
{
    /// <summary>
    /// Класс настроек всех управляющих объектов интерфейса
    /// </summary>
    public class IELUsingObjectSetting : IELObjectSetting
    {
        #region MouseHover
        /// <summary>
        /// Длительность задержки в миллисекундах
        /// </summary>
        public double IntervalHover
        {
            get => TimerBorderInfo.Interval.TotalMilliseconds;
            set => TimerBorderInfo.Interval = TimeSpan.FromMilliseconds(value);
        }

        /// <summary>
        /// Таймер события MouseHover
        /// </summary>
        private readonly DispatcherTimer TimerBorderInfo = new()
        {
            Interval = TimeSpan.FromMilliseconds(1300d),
        };

        /// <summary>
        /// Запустить таймер позиции мыши
        /// </summary>
        public void StartHover() => TimerBorderInfo.Start();

        /// <summary>
        /// Остановить таймер позиции мыши
        /// </summary>
        public void StopHover() => TimerBorderInfo.Stop();

        /// <summary>
        /// Событие задержки курсора на элементе
        /// </summary>
        public event EventHandler? MouseHover;
        #endregion

        /// <summary>
        /// Инициализировать настройку всех управляющих объектов интерфейса по умолчанию
        /// </summary>
        public IELUsingObjectSetting()
        {
            TimerBorderInfo.Tick += (sender, e) =>
            {
                MouseHover?.Invoke(this, e);
                TimerBorderInfo.Stop();
            };
            AnimationMillisecond = 200d;
        }
    }
}
