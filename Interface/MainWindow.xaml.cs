using FirstFloor.ModernUI.Windows.Controls;
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

namespace Interface
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ModernWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        public DB.DBOffline dbase { get; set; }
        private void ModernWindow_Loaded(object sender, RoutedEventArgs e)
        {
          this.dbase = DB.DBOffline.GetDBOffline();
            this.Linhas.ItemsSource = this.dbase.tabela.linhas;
        }

        private void set_linha_selecionada(object sender, SelectionChangedEventArgs e)
        {
            this.Celulas.ItemsSource = null;
            if(this.Linhas.SelectedItem is DB.Linha)
            {
                var sel = this.Linhas.SelectedItem as DB.Linha;
                this.Celulas.ItemsSource = sel.celulas;
            }
        }
    }
}
