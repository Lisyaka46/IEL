namespace LibraryIEL.CORE.Enums
{
    /// <summary>
    /// Перечисление вариаций позиционирования объекта сообщения от при магниченного объекта
    /// </summary>
    public enum EnumPositionMessage : byte
    {
        /// <summary>
        /// Слева сверху
        /// </summary>
        LeftUp = 0x00,

        /// <summary>
        /// По середине сверху
        /// </summary>
        CenterUp = 0x01,

        /// <summary>
        /// Справа сверху
        /// </summary>
        RightUp = 0x02,

        /// <summary>
        /// По середине справа
        /// </summary>
        CenterRight = 0x03,

        /// <summary>
        /// Справа снизу
        /// </summary>
        RightDown = 0x04,

        /// <summary>
        /// По середине снизу
        /// </summary>
        CenterDown = 0x05,

        /// <summary>
        /// Слева снизу
        /// </summary>
        LeftDown = 0x06,

        /// <summary>
        /// По середине слева
        /// </summary>
        CenterLeft = 0x07,
    }
}
