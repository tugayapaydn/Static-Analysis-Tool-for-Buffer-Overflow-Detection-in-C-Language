using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.IO;

namespace CodeAnalysis
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //var gridView = new GridView();
            //this.AnalysisList.View = gridView;
        }

        private void MainFile_Click(object sender, RoutedEventArgs e)
        {

            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog()
            {
                Filter = "C Files (*.c) | *.c",
                DefaultExt = ".c"
            };

            Nullable<bool> res = dlg.ShowDialog();
            

            if (res == true)
            {
                try
                {
                    AnalysisList.Items.Clear();

                    string filename = dlg.FileName;
                    //string filename = "C:\\Users\\tugay\\source\\repos\\CodeAnalysis\\CodeAnalysis\\Test\\test3.c";
                    
                    AST ast = new AST();
                    ast.CreateSyntaxTree(filename);
                    ast.PrintTree();

                    FilePresentation.Text = File.ReadAllText(filename);
                    OverflowBeast ob = new OverflowBeast(ast);
                    List<ListViewObject> err = ob.ValueFlowAnalysis();
                    List<ListViewObject> uqErr = UniqueErrors(err);

                    foreach (ListViewObject s in uqErr)
                    {
                        s.File = filename;
                        AnalysisList.Items.Add(s);
                    }
                }
                catch (Exception exc)
                {
                    Console.WriteLine("Failure: " + exc.ToString());
                }

            }
        }

        private List<ListViewObject> UniqueErrors(List<ListViewObject> err)
        {
            List<ListViewObject> uqErr = new List<ListViewObject>();

            foreach(ListViewObject obj in err)
            {
                bool add = true;
                foreach(ListViewObject uqobj in uqErr)
                {
                    if (uqobj.MyEquals(obj))
                    {
                        add = false;
                    }
                }
                if (add)
                {
                    uqErr.Add(obj);
                }
            }
            return uqErr;
        }
    }
}
