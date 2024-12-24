using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ticket2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void bt_convert_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Проверяем, выбраны ли единицы в обоих комбобоксах
                if (cb_units1.SelectedItem == null || cb_units2.SelectedItem == null)
                {
                    MessageBox.Show("Выберите единицы измерения для конвертации.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Получаем выбранные единицы
                string unitFrom = cb_units1.SelectedItem.ToString();
                string unitTo = cb_units2.SelectedItem.ToString();

                // Парсим вводимое значение
                if (!double.TryParse(tb_input.Text, out double inputValue))
                {
                    MessageBox.Show("Введите корректное числовое значение.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Получаем коэффициент конверсии
                double conversionFactor = GetConversionFactor(unitFrom, unitTo);

                // Выполняем преобразование
                double result = inputValue * conversionFactor;

                // Форматируем результат и выводим в tb_output
                tb_output.Text = result.ToString("F6"); // Формат до 6 знаков после запятой
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private double GetConversionFactor(string unitFrom, string unitTo)
        {
            // Словари для коэффициентов преобразования относительно базовой единицы
            var distanceFactors = new Dictionary<string, double>
            {
                { "Километр", 1000 }, // 1 км = 1000 м
                { "Метр", 1 },        // 1 м = 1 м
                { "Сантиметр", 0.01 } // 1 см = 0.01 м
            };

            var volumeFactors = new Dictionary<string, double>
            {
                { "Кубический метр", 1000 }, // 1 куб.м = 1000 л
                { "Литр", 1 },               // 1 л = 1 л
                { "Миллилитр", 0.001 }       // 1 мл = 0.001 л
            };

            var weightFactors = new Dictionary<string, double>
            {
                { "Кг", 1000 },   // 1 кг = 1000 г
                { "Грамм", 1 },   // 1 г = 1 г
                { "Миллиграм", 0.001 } // 1 мг = 0.001 г
            };

            var currencyFactors = new Dictionary<string, double>
            {
                { "Рубль", 1 },    // базовая единица
                { "Доллар", 99.8729 },  // курс: 1 доллар = 75 рублей
                { "Евро", 104.2310 }     // курс: 1 евро = 85 рублей
            };

            // Выбираем соответствующий словарь
            Dictionary<string, double> factors = null;

            if (distanceFactors.ContainsKey(unitFrom) && distanceFactors.ContainsKey(unitTo))
            {
                factors = distanceFactors;
            }
            else if (volumeFactors.ContainsKey(unitFrom) && volumeFactors.ContainsKey(unitTo))
            {
                factors = volumeFactors;
            }
            else if (weightFactors.ContainsKey(unitFrom) && weightFactors.ContainsKey(unitTo))
            {
                factors = weightFactors;
            }
            else if (currencyFactors.ContainsKey(unitFrom) && currencyFactors.ContainsKey(unitTo))
            {
                factors = currencyFactors;
            }

            if (factors == null)
                throw new InvalidOperationException("Невозможно выполнить преобразование между выбранными единицами.");

            // Вычисляем коэффициент преобразования
            double factorFrom = factors[unitFrom]; // Коэффициент для исходной единицы
            double factorTo = factors[unitTo];     // Коэффициент для целевой единицы

            return factorFrom / factorTo;
        }




        private void rb_checked(object sender, RoutedEventArgs e)
        {
            RadioButton rb = e.OriginalSource as RadioButton;

            switch (rb.Name)
            {
                case "rb_distance":

                    var units_distance = new List<string>()
                    {
                        "Километр",
                        "Метр",
                        "Сантиметр"
                    };
                    cb_units1.ItemsSource = units_distance;
                    cb_units2.ItemsSource = units_distance;

                    break;

                case "rb_volume":

                    var units_volume = new List<string>
                    {
                        "Кубический метр",
                        "Литр",
                        "Миллилитр",
                    };

                    cb_units1.ItemsSource = units_volume;
                    cb_units2.ItemsSource = units_volume;

                    break;

                case "rb_weight":

                    var units_weight = new List<string>
                    {
                        "Кг",
                        "Грамм",
                        "Миллиграм",
                    };

                    cb_units1.ItemsSource = units_weight;
                    cb_units2.ItemsSource = units_weight;

                    break;

                case "rb_currency":

                    var units_currency = new List<string>
                    {
                        "Рубль",
                        "Доллар",
                        "Евро",
                    };

                    cb_units1.ItemsSource = units_currency;
                    cb_units2.ItemsSource = units_currency;

                    break;
            }
        }
    }
}
