using IEL.Interfaces.Core;

namespace IEL.Classes
{
    public struct LabelAction(string name, string description, string command) : ILabelAction
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

        /// <summary>
        /// Пустой объект ярлыка
        /// </summary>
        public static LabelAction Empty { get; } = new LabelAction(string.Empty, string.Empty, string.Empty);

        /// <summary>
        /// Сравнение двух объектов ярлыка
        /// </summary>
        /// <param name="A">Левый объект ярлыка</param>
        /// <param name="B">Правый объект ярлыка</param>
        public static bool operator !=(LabelAction A, LabelAction B) => !(A.Name.Equals(B.Name) && A.Command.Equals(B.Command));

        /// <summary>
        /// Сравнение двух объектов ярлыка
        /// </summary>
        /// <param name="A">Левый объект ярлыка</param>
        /// <param name="B">Правый объект ярлыка</param>
        public static bool operator ==(LabelAction A, LabelAction B) => A.Name.Equals(B.Name) && A.Command.Equals(B.Command);

        /// <summary>
        /// Сравнить объект ярлыка между другим объектом
        /// </summary>
        /// <param name="Value">Сравниваемый объект</param>
        public readonly override bool Equals(object? Value) => base.Equals(Value);

        /// <summary>
        /// Узнать текущий код объекта ярлыка
        /// </summary>
        public readonly override int GetHashCode() => base.GetHashCode();
    }
}
