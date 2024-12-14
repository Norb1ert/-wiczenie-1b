using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Diagnostics;

namespace Ćwiczenie_1b
{
   
    public partial class MainWindow : Window
    {
        private int totalPoints = 0;
        private int pointsInsideCircle = 0;
        private int pointsInsideSquare = 0;
        private int numberOfPoints = 10000;
        private Stopwatch stopwatch = new Stopwatch();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void StartSimulationButton_Click(object sender, RoutedEventArgs e)
        {
            // Reset punktów przed symulacją
            totalPoints = 0;
            pointsInsideCircle = 0;
            pointsInsideSquare = 0;

            // Pobieranie liczby punktów z pola tekstowego
            if (int.TryParse(NumberOfPointsTextBox.Text, out int points))
            {
                numberOfPoints = points;
            }
            else
            {
                MessageBox.Show("Wprowadź poprawną liczbę punktów.");
                return;
            }

            stopwatch.Start();

            // Uruchomienie symulacji w osobnym wątku
            Dispatcher.BeginInvoke(new Action(() => SimulateMonteCarlo()), DispatcherPriority.Background);
        }

        private void SimulateMonteCarlo()
        {
            Random random = new Random();
            double r = 1; // Promień koła

            for (int i = 0; i < numberOfPoints; i++)
            {
                double x = (random.NextDouble() * 2) - 1; // Losowanie współrzędnej x w zakresie [-1, 1]
                double y = (random.NextDouble() * 2) - 1; // Losowanie współrzędnej y w zakresie [-1, 1]

                // Sprawdzanie, czy punkt jest wewnątrz koła
                if (x * x + y * y <= r * r)
                {
                    pointsInsideCircle++;
                }

                // Liczymy punkty wewnątrz kwadratu (każdy punkt należy do kwadratu)
                pointsInsideSquare++;

                // Zwiększamy liczbę wszystkich punktów
                totalPoints++;

                // Szacowanie Pi co 100 punktów
                if (i % 100 == 0)
                {
                    double piEstimate = 4.0 * pointsInsideCircle / totalPoints;
                    PiValueLabel.Content = $"Szacowane Pi: {piEstimate}";
                }
            }

            // Zatrzymanie zegara
            stopwatch.Stop();
            TimeLabel.Content = $"Czas obliczeń: {stopwatch.ElapsedMilliseconds} ms";

            // Obliczamy ostateczną wartość Pi
            double finalPi = 4.0 * pointsInsideCircle / totalPoints;
            PiValueLabel.Content = $"Szacowane Pi: {finalPi}";
        }
    }
}