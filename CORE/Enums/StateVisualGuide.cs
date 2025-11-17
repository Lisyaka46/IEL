namespace IEL.CORE.Enums
{
    /// <summary>
    /// Перечисление состояний отображения
    /// </summary>
    public enum StateVisualGuide : byte
    {
        /// <summary>
        /// Обычное отображение
        /// </summary>
        Default = 0,

        /// <summary>
        /// Отображение с левосторонней стрелкой
        /// </summary>
        LeftArrow = 1,

        /// <summary>
        /// Отображение с правосторонней стрелкой
        /// </summary>
        RightArrow = 2,

        /// <summary>
        /// Отображение с двумя стрелками
        /// </summary>
        DuoArrow = 3,
    }
}
