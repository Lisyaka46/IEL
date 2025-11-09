namespace IEL.CORE.Enums
{
    /// <summary>
    /// Перечисление спектров цвета
    /// </summary>
    public enum StateSpectrum
    {
        /// <summary>
        /// Спектр цвета который не контролируется типом QData
        /// </summary>
        /// <remarks>
        /// <c>Такое состояние устанавливается в случае изменения цвета на собственный, даже если он совпадает с одним из спектров QData</c>
        /// </remarks>
        Custom = -1,

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
