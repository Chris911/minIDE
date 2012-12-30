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
using AvalonDock.Layout;
using AvalonDock.Layout.Serialization;

namespace minIDE
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Controller controller;

        public MainWindow()
        {
            //InitializeComponent();
            //var firstDocumentPane = dockingManager.Layout.;
            
            //Init Controller
            controller = new Controller(); 
        }

        private void compileButton_Click(object sender, RoutedEventArgs e)
        {
            String codeText = codeTextBox1.Text;
            String outputText = controller.sendCodeToServer(codeText, 55);
            outputTextBox1.Text = outputText;
        }



    }
}
