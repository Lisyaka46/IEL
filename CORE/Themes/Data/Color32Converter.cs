using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text;
using System.Windows.Media;

namespace LibraryIEL.CORE.Themes.Data
{
    /// <summary>
    /// Конвертер значение к типу <see cref="Color32"/>
    /// </summary>
    public class Color32Converter : TypeConverter
    {
        /// <summary>
        /// Узнать, можно ли конвертировать значение в ожидаемый тип
        /// </summary>
        public override bool CanConvertFrom(ITypeDescriptorContext? Context, Type SourceType)
            => SourceType == typeof(string) || base.CanConvertFrom(Context, SourceType);

        /// <summary>
        /// Конвертировать значение в ожидаемый тип
        /// </summary>
        public override object? ConvertFrom(ITypeDescriptorContext? Context, CultureInfo? Culture, object Value)
        {
            if (Value is string Str)
                return (Color32)(Color)ColorConverter.ConvertFromString(Str);
            return base.ConvertFrom(Context, Culture, Value);
        }
    }
}
