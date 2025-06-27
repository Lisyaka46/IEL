namespace IEL.CORE.Enums
{
    /// <summary>
    /// Перечисление спектров цвета
    /// </summary>
    public enum StateSpectrum
    {
        /// <summary>
        /// Спектр обычного состояния
        /// </summary>
        Default = 0,

        /// <summary>
        /// Спектр выделенного состояния
        /// </summary>
        Select = 1,

        /// <summary>
        /// Спектр использованного цвета
        /// </summary>
        Used = 2,

        /// <summary>
        /// Спектр отключённого цвета
        /// </summary>
        NotEnabled = 3,
    }
}
