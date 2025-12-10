using IEL.CORE.Enums;
using System.Collections.ObjectModel;
using System.Windows.Media;

namespace IEL.CORE.Classes
{
    internal static class QDataView
    {
        public static Color ToColor(this byte[] rgb) => Color.FromArgb(rgb[0], rgb[1], rgb[2], rgb[3]);
    }

    /// <summary>
    /// Класс управляемых данных цветовой палитры отображения объекта
    /// </summary>
    public class QData
    {
        /// <summary>
        /// Объект перечисления спектров данных цвета
        /// </summary>
        public enum EnumDataSpectrum : byte
        {
            /// <summary>
            /// Спектр значения по обычному состоянию
            /// </summary>
            Default = 0,

            /// <summary>
            /// Спектр выделенного состояния
            /// </summary>
            Select = 1,

            /// <summary>
            /// Спектр используемого состояния
            /// </summary>
            Used = 2,

            /// <summary>
            /// Спектр отключённого состояния
            /// </summary>
            NotEnabled = 3
        }

        /// <summary>
        /// Константа размера хранимых байтов одного цвета
        /// </summary>
        public static int CountBytesFromColor => 4;

        /// <summary>
        /// Константа количества состояний цветов
        /// </summary>
        public static int CountSpectrumColor => 4;

        /// <summary>
        /// Массив данных цвета
        /// </summary>
        internal protected byte[][] Data;

        /// <summary>
        /// Исключение неверного размера <b>одномерного</b> массива
        /// </summary>
        internal static readonly Exception ExceptionArrayNotLengthBytes =
            new($"Массив не имеет размер {CountSpectrumColor * CountBytesFromColor} байт");

        /// <summary>
        /// Фиксированные значения по умолчанию объекта QData
        /// </summary>
        public static readonly ReadOnlyCollection<byte> DefaultBytesValues = new byte[]
            {
                255, 0, 0, 0,
                255, 128, 128, 128,
                255, 200, 200, 200,
                255, 255, 70, 70,
            }.AsReadOnly();

        /// <summary>
        /// Делегат события изменения данных спектра
        /// </summary>
        /// <param name="Spectrum">Изменяемый спектр</param>
        internal delegate void ChangeDataFromSpectrum(EnumDataSpectrum Spectrum);

        /// <summary>
        /// Событие изменения данных конкретного спектра
        /// </summary>
        internal ChangeDataFromSpectrum? ChangedData;

        #region QDataManipulate
        /// <summary>
        /// Спектр обычного сотояния цвета
        /// </summary>
        public Color Default
        {
            get => GetFromSpectrumColor(EnumDataSpectrum.Default);
            set => SetFromSpectrumColor(EnumDataSpectrum.Default, value);
        }

        /// <summary>
        /// Спектр выделенного состояния цвета
        /// </summary>
        public Color Select
        {
            get => GetFromSpectrumColor(EnumDataSpectrum.Select);
            set => SetFromSpectrumColor(EnumDataSpectrum.Select, value);
        }

        /// <summary>
        /// Спектр используемого цвета
        /// </summary>
        public Color Used
        {
            get => GetFromSpectrumColor(EnumDataSpectrum.Used);
            set => SetFromSpectrumColor(EnumDataSpectrum.Used, value);
        }

        /// <summary>
        /// Спектр отключённого цвета
        /// </summary>
        public Color NotEnabled
        {
            get => GetFromSpectrumColor(EnumDataSpectrum.NotEnabled);
            set => SetFromSpectrumColor(EnumDataSpectrum.NotEnabled, value);
        }
        #endregion

        /// <summary>
        /// Получить объект байтов текущего объекта QData
        /// <br/>Массив представляет собой размер <b>"CountSpectrumColor * CountBytesFromColor"</b>
        /// <br/>Имеет определение всех спектров цвета текущего объекта в байтовом представлении
        /// </summary>
        /// <returns>[<b>"CountSpectrumColor * CountBytesFromColor"</b> байт]</returns>
        public byte[] GetSourceBytes() => [
                Data[0][0], Data[0][1], Data[0][2], Data[0][3],
                Data[1][0], Data[1][1], Data[1][2], Data[1][3],
                Data[2][0], Data[2][1], Data[2][2], Data[2][3],
                Data[3][0], Data[3][1], Data[3][2], Data[3][3]
                ];

        /// <summary>
        /// Получить цвет определённого спектра
        /// </summary>
        /// <param name="Spectrum">Спектр из которого берётся значение</param>
        public Color GetFromSpectrumColor(EnumDataSpectrum Spectrum) => Data[(int)Spectrum].ToColor();

        /// <summary>
        /// Установить цвет по определённому спектру
        /// </summary>
        /// <param name="Spectrum">Спектр которому присваивается значение</param>
        /// <param name="Value">Значение цвета</param>
        public void SetFromSpectrumColor(EnumDataSpectrum Spectrum, Color Value)
        {
            Data[(int)Spectrum] = [Value.A, Value.R, Value.G, Value.B];
            ChangedData?.Invoke(Spectrum);
        }

        /// <summary>
        /// Изменить данные цветов по экземпляру массива байтов
        /// </summary>
        /// <remarks>
        /// Массив должен представлять собой размер <b>"CountSpectrumColor * CountBytesFromColor"</b>
        /// <br/>Определяется <b>CountSpectrumColor</b> количество спектров по <b>CountBytesFromColor</b> значениям цвета
        /// </remarks>
        /// <param name="NewObj">Опорный экземпляр значений</param>
        public void ChangeSourceQData(QData NewObj)
        {
            Data = NewObj.Data;
            if (ChangedData != null)
            {
                ChangedData.Invoke(EnumDataSpectrum.Default);
                ChangedData.Invoke(EnumDataSpectrum.Select);
                ChangedData.Invoke(EnumDataSpectrum.Used);
                ChangedData.Invoke(EnumDataSpectrum.NotEnabled);
            }
        }

        /// <summary>
        /// Инициализировать управляемый объект данных спектра цвета со значениями по умолчанию
        /// </summary>
        public QData()
        {
            Data =
            [
                [255, 0, 0, 0],
                [255, 128, 128, 128],
                [255, 200, 200, 200],
                [255, 255, 70, 70],
            ];
        }

        /// <summary>
        /// Инициализировать управляемый объект данных цветовой палитры отображения объекта<br/>
        /// с константным значением цвета
        /// </summary>
        /// <remarks>
        /// <b>
        /// | A  R  G  B |<br/>
        /// <br/>
        /// | 0  0  0  0 | - Default<br/>
        /// | 0  0  0  0 | - Select<br/>
        /// | 0  0  0  0 | - Used<br/>
        /// | 0  0  0  0 | - NotEnabled<br/>
        /// </b>
        /// </remarks>
        /// <param name="ByteColorData">Массив байтовых значений цвета</param>
        public QData(byte[][] ByteColorData)
        {
            if (ByteColorData == null || ByteColorData.Length != CountSpectrumColor || ByteColorData.Any(i => i == null || i.Length != CountBytesFromColor))
                throw new Exception($"Не хватает данных, массив не имеет размеры {CountSpectrumColor}/{CountBytesFromColor}");
            Data = ByteColorData;
        }

        /// <summary>
        /// Инициализировать управляемый объект данных цветовой палитры отображения объекта<br/>
        /// с одним константным значением цвета
        /// </summary>
        /// <remarks>
        /// <b>
        /// | A  R  G  B |<br/>
        /// <br/>
        /// | 0  0  0  0 | - Default / Select / Used / NotEnabled<br/>
        /// </b>
        /// </remarks>
        /// <param name="ByteColorData">Массив байтовых значений цвета</param>
        public QData(byte[] ByteColorData)
        {
            if (ByteColorData.Length == 4)
            {
                Data =
                    [
                    ByteColorData,
                    ByteColorData,
                    ByteColorData,
                    ByteColorData,
                    ];
            }
            else throw new Exception("Не хватает данных, массив не имеет размера 4");
        }
    }
}
