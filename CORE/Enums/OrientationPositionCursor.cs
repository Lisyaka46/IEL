namespace IEL.CORE.Enums
{
    /// <summary>
    /// Перечисление вариаций позиционирования курсора от объекта панели действий
    /// </summary>
    public enum OrientationPositionCursor : sbyte
    {
        /// <summary>
        /// Автоматическое определение оптимальной позиции элемента
        /// </summary>
        Auto = -10,

        /// <summary>
        /// Пустое отображение
        /// </summary>
        Empty = -01,

        /// <summary>
        /// Слева сверху
        /// </summary>
        LeftUp = 00,

        /// <summary>
        /// Слева по середине
        /// </summary>
        LeftCenter = 01,

        /// <summary>
        /// Слева снизу
        /// </summary>
        LeftDown = 02,

        /// <summary>
        /// Справа сверху
        /// </summary>
        RightUp = 10,

        /// <summary>
        /// Справа по середине
        /// </summary>
        RightCenter = 11,

        /// <summary>
        /// Справа снизу
        /// </summary>
        RightDown = 12,
    }
}
