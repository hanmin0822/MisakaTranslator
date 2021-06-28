using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;

namespace MisakaTranslator_WPF.SettingsPages.DictionaryPages
{
    /// <summary>
    /// Interaction logic for MecabDictPage.xaml
    /// </summary>
    public partial class MecabDictPage : Page
    {
        public MecabDictPage()
        {
            InitializeComponent();
        }

        private void ChoosePathBtn_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog dialog = new();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                PathBox.Text = dialog.SelectedPath;
                Common.appSettings.Mecab_dicPath = PathBox.Text;
            }
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            PathBox.Text = Common.appSettings.Mecab_dicPath;
        }

        private void ClearPathBtn_Click(object sender, RoutedEventArgs e)
        {
            PathBox.Text = string.Empty;
            Common.appSettings.Mecab_dicPath = PathBox.Text;
        }
    }
}
