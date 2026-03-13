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

            // Lancio un thread separato per far girare l'alfabeto a manetta
            Task.Run(() =>
            {
                char[] alfabeto = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

                while (true)
                {
                    // Pesco un indice casuale tra 0 e 25 
                    int num = rnd.Next(0, 26);
                    char lettera = alfabeto[num];

                    // Devo usare il Dispatcher altrimenti WPF crasha se tocco la UI da un altro thread
                    this.Dispatcher.Invoke(() =>
                    {
                        CicloNumeri.Content = lettera.ToString();

                    });

                    // Aspetto 100ms per non far sembrare il cambio lettera un glitch impazzito
                    Thread.Sleep(100);
                }
            });
        }

        private string parolaCorrente = "";

        // Questo avviene quando l'utente preme il tasto per "fermare" la lettera
        private async void EstrazioneLettera(object sender, RoutedEventArgs e)
        {

            string carattereEstratto = CicloNumeri.Content.ToString();
            parolaCorrente += carattereEstratto;
            // Un piccolo delay per evitare doppi click involontari o problemi di sync
            await Task.Delay(50);

            // Se il campo 'max' è vuoto, lavoriamo solo sulla prima riga della lista
            if (max.Text == "")
            {
                if (ListaParole.Items.Count == 0)
                {
                    ListaParole.Items.Add(parolaCorrente);
                }
                else
                {
                    // Sovrascrivo sempre il primo elemento se non c'è un limite di lunghezza
                    ListaParole.Items[0] = parolaCorrente;
                }
            }
            else
            {
                // Qui controllo se l'utente ha inserito effettivamente un numero
                if (int.TryParse(max.Text, out int maxLength))
                {
                    if (ListaParole.Items.Count == 0 || parolaCorrente.Length == 1)
                    {
                        ListaParole.Items.Add(parolaCorrente);
                    }
                    else
                    {
                        // Aggiorno l'ultima parola che stiamo componendo nella lista
                        int ultimoIndice = ListaParole.Items.Count - 1;
                        ListaParole.Items[ultimoIndice] = parolaCorrente;
                    }

                    // Se abbiamo raggiunto la lunghezza massima, resetto la stringa per la prossima parola
                    if (parolaCorrente.Length >= maxLength)
                    {
                        parolaCorrente = "";
                    }
                }
                else
                {
                    // Se scrive lettere nel box del numero, lo avviso e resetto il campo
                    MessageBox.Show("Per favore, inserisci un numero intero valido.", "Errore di input", MessageBoxButton.OK, MessageBoxImage.Warning);
                    max.Clear();
                    max.Focus();
                }
            }
        }

    }
}