using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Fixie.AutoRun.Converters
{
   public class Bool2ColorConverter : IValueConverter
   {
      public Brush FalseBrush { get; set; }
      public Brush TrueBrush { get; set; }

      public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
      {
         return (bool)value ? TrueBrush : FalseBrush;
      }

      public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
      {
         throw new NotImplementedException();
      }
   }
}