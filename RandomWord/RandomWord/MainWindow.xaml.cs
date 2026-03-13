using System.ComponentModel.DataAnnotations;
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

namespace RandomWord
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Random rnd = new Random();
            bool click = false;

            Task.Run(() =>
            {
                char[] alfabeto = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

                while (true)
                {
                    int num = rnd.Next(0, 26);
                    char lettera = alfabeto[num];

                    this.Dispatcher.Invoke(() =>
                    {
                        CicloNumeri.Content = lettera.ToString();

                    });

                    Thread.Sleep(100);
                }
            });
        }

        private string parolaCorrente = "";

        private async void EstrazioneLettera(object sender, RoutedEventArgs e)
        {

            string carattereEstratto = CicloNumeri.Content.ToString();
            parolaCorrente += carattereEstratto;
            await Task.Delay(50);

            //if (!int.TryParse(max.Text, out int maxLength)) return;

            if (max.Text == "")
            {
                if (ListaParole.Items.Count == 0)
                {
                    ListaParole.Items.Add(parolaCorrente);
                }
                else
                {

                    ListaParole.Items[0] = parolaCorrente;
                }
            }
            else
            {
                if (int.TryParse(max.Text, out int maxLength))
                {
                    if (parolaCorrente.Length == 1)
                    {
                        ListaParole.Items.Add(parolaCorrente);
                    }
                    else
                    {
                        int ultimoIndice = ListaParole.Items.Count - 1;
                        ListaParole.Items[ultimoIndice] = parolaCorrente;
                    }

                    if (parolaCorrente.Length >= maxLength)
                    {
                        parolaCorrente = "";
                    }
                }
                else
                {

                    MessageBox.Show("Per favore, inserisci un numero intero valido.", "Errore di input", MessageBoxButton.OK, MessageBoxImage.Warning);
                    max.Clear();
                    max.Focus();
                }
            }
        }

    }
}