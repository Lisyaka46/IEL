using IEL.CORE.Classes;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace IEL.CORE.Themes
{
    /// <summary>
    /// Главный класс управления темами
    /// </summary>
    public static class Theme
    {
        /// <summary>
        /// Расширение файлов тем
        /// </summary>
        public static readonly string ExtensionThemeFile = ".qd";

        /// <summary>
        /// Числовой тип, который используется для перечисления спектров палитры <code>UINT</code>
        /// </summary>
        public static readonly Type EnumUnderlyingTypePalette = typeof(uint);

        /// <summary>
        /// Узнать тип перечисления для спектров палитры
        /// </summary>
        /// <param name="NameType">Имя поискового типа</param>
        /// <param name="SourceAssembly">Сборка в которой хранится тип</param>
        public static Type? GetEnumSpectrumType(Assembly SourceAssembly, string NameType)
        {
            Type[] AllTypesCallAssembly = SourceAssembly.GetTypes();
            Type? SourceType = AllTypesCallAssembly.FirstOrDefault((i) => i.Name.Equals(NameType));
            if (SourceType == null) return null;
            else if (SourceType.IsEnum && Enum.GetUnderlyingType(SourceType) == EnumUnderlyingTypePalette)
                    return SourceType;
            else return null;
        }
    }
}
