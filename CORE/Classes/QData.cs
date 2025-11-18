using IEL.CORE.Enums;
using System;
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
        /// Делегат события изменения данных спектра
        /// </summary>
        /// <param name="Spectrum">Изменяемый спектр</param>
        internal delegate void ChangeDataFromSpectrum(EnumDataSpectrum Spectrum);

        /// <summary>
        /// Событие изменения данных конкретного спектра
        /// </summary>
        internal event ChangeDataFromSpectrum? ChangedData;

        /// <summary>
        /// Массив данных цвета
        /// </summary>
        private byte[][] Data;

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
        /// Получить цвет по определённому спектру
        /// </summary>
        /// <param name="Spectrum">Спектр которому присваивается значение</param>
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
            if (ByteColorData == null || ByteColorData.Length != 4 || ByteColorData.Any(i => i == null || i.Length != 4))
                throw new Exception("Не хватает данных, массив не имеет размеры 4/4");
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

        /// <summary>
        /// Скопировать данные расположения цветов
        /// </summary>
        /// <returns>Склонированный элемент</returns>
        public QData Clone() => new(Data);
    }
}
