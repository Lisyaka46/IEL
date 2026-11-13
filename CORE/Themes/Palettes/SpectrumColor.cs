using LibraryIEL.CORE.Themes.Data;
namespace LibraryIEL.CORE.Themes.Palettes
{
    /// <summary>
    /// Перечисление спектров цвета
    /// </summary>
    public enum SpectrumColor : byte
    {
        /// <summary>
        /// Спектр цвета который не контролируется типом <see cref="QData"/>
        /// </summary>
        /// <remarks>
        /// <c>Такое состояние устанавливается в случае изменения цвета на собственный, 
        /// даже если он совпадает с одним из спектров <see cref="QData"/></c>
        /// </remarks>
        Custom = 0,

        /// <summary>
        /// Спектр обычного состояния
        /// </summary>
        Default = 1,

        /// <summary>
        /// Спектр выделенного состояния
        /// </summary>
        Select = 2,

        /// <summary>
        /// Спектр использованного цвета
        /// </summary>
        Used = 3,

        /// <summary>
        /// Спектр отключённого цвета
        /// </summary>
        NotEnabled = 4,
    }
}
