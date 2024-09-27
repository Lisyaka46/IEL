namespace IEL.Interfaces.Core
{
    public interface ILabelAction
    {
        /// <summary>
        /// Имя ярлыка
        /// </summary>
        internal string Name { get; }

        /// <summary>
        /// Описчание ярлыка
        /// </summary>
        internal string? Description { get; }

        /// <summary>
        /// Команда реализуемая ярлыком
        /// </summary>
        internal string Command { get; }
    }
}
