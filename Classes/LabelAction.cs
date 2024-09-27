using IEL.Interfaces.Core;

namespace IEL.Classes
{
    public class LabelAction(string name, string description, string command) : ILabelAction
    {
        /// <summary>
        /// Имя ярлыка
        /// </summary>
        public string Name { set; get; } = name;

        /// <summary>
        /// Описчание ярлыка
        /// </summary>
        public string? Description { set; get; } = description;

        /// <summary>
        /// Команда реализуемая ярлыком
        /// </summary>
        public string Command { get; set; } = command;
    }
}
