namespace IEL.CORE.Enums
{
    /// <summary>
    /// Перечисление вариаций позиционирования
    /// </summary>
    public enum OrientationBorderPosition
    {
        /// <summary>
        /// Слева снизу
        /// </summary>
        LeftDown = 0,

        /// <summary>
        /// Слева сверху
        /// </summary>
        LeftUp = 1,

        /// <summary>
        /// Справа сверху
        /// </summary>
        RightUp = 2,

        /// <summary>
        /// Справа снизу
        /// </summary>
        RightDown = 3,

        /// <summary>
        /// Автоматическое определение оптимальной позиции элемента
        /// </summary>
        Auto = 4,
    }

    /// <summary>
    /// Перечисление вариаций позиционирования панели дейтсвий относительно объекта
    /// </summary>
    public readonly struct OrientationPanelActionPosition(IntPtr lr, IntPtr ud)
    {
        /// <summary>
        /// l 00 r 01
        /// </summary>
        internal readonly IntPtr Code_lr = lr;
        /// <summary>
        /// d 00 c 01 u 02
        /// </summary>
        internal readonly IntPtr Code_ud = ud;

        /// <summary>
        /// Слева снизу
        /// </summary>
        public static readonly OrientationPanelActionPosition LeftDown = new(0x00, 0x00);

        /// <summary>
        /// Слева по центру
        /// </summary>
        public static readonly OrientationPanelActionPosition LeftCenter = new(0x00, 0x01);

        /// <summary>
        /// Слева сверху
        /// </summary>
        public static readonly OrientationPanelActionPosition LeftUp = new(0x00, 0x02);

        /// <summary>
        /// Справа снизу
        /// </summary>
        public static readonly OrientationPanelActionPosition RightDown = new(0x01, 0x00);

        /// <summary>
        /// Справа по центру
        /// </summary>
        public static readonly OrientationPanelActionPosition RightCenter = new(0x01, 0x01);

        /// <summary>
        /// Справа сверху
        /// </summary>
        public static readonly OrientationPanelActionPosition RightUp = new(0x01, 0x02);

        /// <summary>
        /// Автоматическое определение оптимальной позиции элемента
        /// </summary>
        public static readonly OrientationPanelActionPosition Auto = new(0xf0, 0xf0);

        /// <summary>
        /// Cравнительный элемент истенного типа
        /// </summary>
        /// <param name="o">Сравнительный элемент</param>
        /// <returns>Сравнение кодов текуцих элементов</returns>
        public override bool Equals(object? o)
        {
            try { return this == (OrientationPanelActionPosition?)o; }
            catch { return base.Equals(o); }
        }

        /// <summary>
        /// Представление ссылки на объект как 32 битное число
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() => base.GetHashCode();

        /// <summary>
        /// Перегруженный сравнительный элемент истенного типа
        /// </summary>
        /// <param name="A">Сравниваемый элемент</param>
        /// <param name="B">Сравнительный элемент</param>
        /// <returns>Сравнение кодов текуцих элементов</returns>
        public static bool operator ==(OrientationPanelActionPosition A, OrientationPanelActionPosition B) => A.Code_lr == B.Code_lr & A.Code_ud == B.Code_ud;

        /// <summary>
        /// Перегруженный сравнительный элемент отрицательного типа
        /// </summary>
        /// <param name="A">Сравниваемый элемент</param>
        /// <param name="B">Сравнительный элемент</param>
        /// <returns>Сравнение кодов текуцих элементов</returns>
        public static bool operator !=(OrientationPanelActionPosition A, OrientationPanelActionPosition B) => A.Code_lr != B.Code_lr | A.Code_ud != B.Code_ud;
    }
}
