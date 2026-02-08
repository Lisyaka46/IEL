using IEL.CORE.Enums;
using IEL.UserElementsControl.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace IEL.UserElementsControl
{
    /// <summary>
    /// Логика взаимодействия для IELScrollViewer.xaml
    /// </summary>
    public partial class IELScrollViewer : IELScrollViewerBase
    {
        /// <summary>
        /// Инициализировать объект визуализации прокрутки
        /// </summary>
        public IELScrollViewer()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Объект реализующий прокрутку
        /// </summary>
        public ScrollViewer SourceViewer => MainScrollViewer;
    }
}
