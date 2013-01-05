using System;
using System.Collections.Generic;
using System.Windows;

namespace minIDE
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Controller _controller;

        public MainWindow()
        {
            //InitializeComponent();
            //var firstDocumentPane = dockingManager.Layout.;

            //Init Controller
            _controller = new Controller();
        }

        private void compileButton_Click(object sender, RoutedEventArgs e)
        {
            String codeText = CodeTextBox1.Text;
            int language = _controller.LanguageToIdMap[LanguagesList.SelectedItem.ToString()];
            OutputTextBox1.Text = _controller.SendSubmission(codeText, "", 55);
        }

        private void LanguagesList_OnLoaded(object sender, RoutedEventArgs e)
        {
            List<String> langList = _controller.GetLanguagesList();
            foreach (string lang in langList)
            {
                LanguagesList.Items.Add(lang);
            }
            LanguagesList.SelectedIndex = 0;
        }
    }
}