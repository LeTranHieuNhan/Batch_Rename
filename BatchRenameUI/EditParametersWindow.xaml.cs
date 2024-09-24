using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Contract;

namespace BatchRenameUI
{
    /// <summary>
    /// Interaction logic for EditParametersWindow.xaml
    /// </summary>
    public partial class EditParametersWindow : Window
    {
        public IRule ReturnValue { get; set; }
        List<PropertyInfo> propertiesInfos;

        public EditParametersWindow(IRule rule)
        {
            ReturnValue = rule;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Type typeInfo = ReturnValue.GetType();
            propertiesInfos = new List<PropertyInfo>(typeInfo.GetProperties());

            //create textboxes
            for (int i = 1; i < propertiesInfos.Count; i++)
            {
                s.Children.Add(new Label()
                {
                    Content = propertiesInfos[i].Name,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    FontFamily = new FontFamily("Blackadder ITC"),
                    FontSize = 25
                });

                if (propertiesInfos[i].Name == "ExtensionTypes")
                {
                    List<string> extension = new List<string>
                    {
                        ".txt", ".pdf", ".docx"
                    };


                    ComboBox cbb = new ComboBox()
                    {
                        Name = propertiesInfos[i].Name,
                        ItemsSource = extension
                    };


                    RegisterName(propertiesInfos[i].Name, cbb);
                    s.Children.Add(cbb);
                }
                else
                {
                    TextBox txb = new TextBox
                    {
                        Name = propertiesInfos[i].Name,
                        //Text = (string)propertiesInfos[i].GetValue(ReturnValue),
                        Width = 150,
                        Height = 20,
                    };
                    txb.LayoutTransform = new ScaleTransform(1.5, 1.5);
                    Debug.WriteLine($"dest window: {(string)propertiesInfos[i].GetValue(ReturnValue)}");
                    RegisterName(propertiesInfos[i].Name, txb);
                    s.Children.Add(txb);
                }
            }
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            List<string> items = new List<string>();
            for (int i = 1; i < propertiesInfos.Count; i++)
            {
                if (propertiesInfos[i].Name == "ExtensionTypes")
                {
                    var item = s.FindName(propertiesInfos[i].Name) as ComboBox;

                    string a = (string)item.SelectedValue;
                    items.Add(a);
                    propertiesInfos[i].SetValue(ReturnValue, a);
                    //Debug.WriteLine(a);
                }
                else
                {
                    var item = s.FindName(propertiesInfos[i].Name) as TextBox;

                    string a = item.Text;
                    if (a.Length >= 1)
                    {
                        a += "";
                    }else
                    {
                        a = null;
                    }
                    items.Add(a);
                    propertiesInfos[i].SetValue(ReturnValue, a);
                    //Debug.WriteLine(a);
                }
            }

            DialogResult = true;
            //MessageBox.Show($"{ReturnValue.GetType().GetProperty("Prefix").Attributes}");
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            for (int i = 1; i < propertiesInfos.Count; i++)
            {
                if (propertiesInfos[i].Name == "ExtensionTypes")
                {
                    var item = s.FindName(propertiesInfos[i].Name) as ComboBox;

                    string a = (string)item.SelectedValue;
                    //items.Add(a);
                    propertiesInfos[i].SetValue(ReturnValue, a);
                    //Debug.WriteLine(a);
                }
                else
                {
                    var item = s.FindName(propertiesInfos[i].Name) as TextBox;

                    string a = item.Text;
                    if (a.Length >= 1)
                    {
                        a += "";
                    }
                    else
                    {
                        a = null;
                    }
                    //items.Add(a);
                    propertiesInfos[i].SetValue(ReturnValue, a);
                    //Debug.WriteLine(a);
                }
            }

            //DialogResult = true;
            //MessageBox.Show($"{ReturnValu
        }
    }
}
