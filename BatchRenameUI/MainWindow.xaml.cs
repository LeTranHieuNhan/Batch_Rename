
//Quan Lap Van - 2159014
//Nguyen Do Hai Duy - 2159020
//Le Tran Hieu Nhan - 2159023
// Improvement: 4, 6, 8, 10, 11


using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Contract;
using System.Windows.Forms;
using MessageBox = System.Windows.Forms.MessageBox;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

namespace BatchRenameUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //PROPERTIES
        //all rules loaded
        List<IRule> allRuleList = null;

        //all rules are chosen
        ObservableCollection<IRule> ruleList = new ObservableCollection<IRule>();
        //all files'names added to ListView
        ObservableCollection<FileSystemInfo> _filenames = new ObservableCollection<FileSystemInfo>();
        //some values use for the entire app
        static string baseDir = AppDomain.CurrentDomain.BaseDirectory;
        string currentWorkFile = "Files/Progress.txt";
        string ruleFile = "Files/CVRules.txt";
        Converter converter;

        public delegate void AddFileHandler();
        public event AddFileHandler ResetCountEvent;


        //MAIN
        public MainWindow()
        {
            InitializeComponent();
        }


        //Read, Write file
        //load all classes in *.dll files
        void LoadDLLFiles()
        {
            //string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            DirectoryInfo dirInfo = new DirectoryInfo(baseDir);
            allRuleList = new List<IRule>();

            var dllFiles = dirInfo.GetFiles("*.dll");
            Debug.WriteLine(dllFiles.Length);
            //process each *.dll file
            foreach (var dllFile in dllFiles)
            {
                var assembly = Assembly.LoadFrom(dllFile.FullName);
                var types = assembly.GetTypes();

                //load all classes in each *.dll file
                foreach (var type in types)
                {
                    if (type.IsClass && typeof(IRule).IsAssignableFrom(type))
                    {
                        Debug.WriteLine(type.Name);
                        IRule rule = (IRule)Activator.CreateInstance(type);
                        ResetCountEvent += rule.ResetCount;

                        allRuleList.Add(rule);
                    }
                }
            }
        }

        //load saved work
        void LoadSavedWork()
        {
            
            FileInfo currentWorkFileInfo = new FileInfo(currentWorkFile);

            if (!currentWorkFileInfo.Exists)
            {
                currentWorkFileInfo.Create();
            }
            else
            {
                string[] lines = File.ReadAllLines(currentWorkFile);

                ruleFile = lines[0].Replace("Preset file: ", "");
                ReadFile(ruleFile);

                List<FileInfo> list = new List<FileInfo>();
                ResetCountEvent?.Invoke();
                //get all saved files'path
                for (int i = 2; i < lines.Length; i++)
                {
                    list.Add(new FileInfo(lines[i]));
                }

                //re-allocate _filenames property
                _filenames = new ObservableCollection<FileSystemInfo>(list);
                
                //unregister name all the children in stack panel and clear stack panel
                foreach (object child in s.Children)
                {
                    string childname = null;
                    if (child is FrameworkElement)
                    {
                        childname = (child as FrameworkElement).Name;
                    }

                    if (childname != null)
                    {
                        UnregisterName(childname);
                    }
                }
                s.Children.Clear();

                //checked all the textbox related to the rules in the CVRules.txt file
                foreach (var rule in allRuleList)
                {
                    System.Windows.Controls.CheckBox checkbox;
                    if (ruleList.Contains(rule))
                    {
                        checkbox = new System.Windows.Controls.CheckBox()
                        {
                            Content = rule.Name,
                            IsChecked = true,
                            Name = rule.Name
                        };
                    }
                    else
                    {
                        checkbox = new System.Windows.Controls.CheckBox()
                        {
                            Content = rule.Name,
                            IsChecked = false,
                            Name = rule.Name
                        };
                    }
                    
                    RegisterName(rule.Name, checkbox);
                    checkbox.Checked += Checkbox_Checked;
                    checkbox.Unchecked += Checkbox_Unchecked;

                    s.Children.Add(checkbox);
                }

                //invoke the PropertyChanged event
                AddedFiles_ListView.ItemsSource = _filenames;
                PreviewChanges_ListView.ItemsSource = _filenames;
            }
        }

        //save the current work
        void SaveCurrentWork()
        {
            FileInfo ruleFileInfo = new FileInfo(ruleFile);

            //save rules'infos
            using (StreamWriter sw = ruleFileInfo.CreateText())
            {
                try
                {
                    foreach (var rule in ruleList)
                    {
                        PropertyInfo[] propertiesInfo = rule.GetType().GetProperties();
                        int i;
                        for (i = 0; i < propertiesInfo.Length - 1; i++)
                        {
                            sw.Write($"{propertiesInfo[i].GetValue(rule).ToString()} ");
                        }
                        sw.Write($"{propertiesInfo[i].GetValue(rule).ToString()}");
                        sw.Write("\n");
                    }
                }
                catch (NullReferenceException)
                {
                    MessageBox.Show("Parameters of one of the rules is NULL \n => CANNOT store to a file");
                    sw.Close();
                    ruleFileInfo.CreateText();
                }
            }

            //save added files
            FileInfo currentWorkFileInfo = new FileInfo(currentWorkFile);
            using (StreamWriter sw = currentWorkFileInfo.CreateText())
            {
                sw.WriteLine($"Preset file: {ruleFile}");
                sw.WriteLine($"File: {_filenames.Count}");
                
                foreach(var filenames in _filenames)
                {
                    sw.WriteLine($"{filenames.FullName}");
                }
            }
        }


        //read the file which has chosen rules (CVRules.txt)
        void ReadFile(string filename)
        {
            List<string> lines = File.ReadAllLines(filename).ToList();
            IRule rule = null;
            ruleList.Clear();
            
            //get rule from file
            foreach (var line in lines)
            {
                rule = RuleFactory.Parse(line);
                ruleList.Add(rule);
            }

            
            if (converter != null)
            {
                converter.RuleList = ruleList.ToList();
            }
        }

        //rename the file
        string Rename(string filename)
        {
            string result = filename;
            try 
            {
                //apply all chosen rules to the filename
                foreach (var rule in ruleList)
                {
                    result = rule.Rename(result);
                }
            }
            catch(ArgumentNullException)
            {
                MessageBox.Show("properties are null!");
            }

            return result;
        }


        //Load all needed things to ready
        //window loaded
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //add all existing rules from *.dll files into a list
            LoadDLLFiles();
            RuleFactory.Rules = allRuleList;

            //add UI
            foreach (var rule in allRuleList)
            {
                var checkbox = new System.Windows.Controls.CheckBox()
                {
                    Content = rule.Name,
                    IsChecked = false,
                    Name = rule.Name
                };
                RegisterName(rule.Name, checkbox);
                checkbox.Checked += Checkbox_Checked;
                checkbox.Unchecked += Checkbox_Unchecked;
                s.Children.Add(checkbox);
            }

            //find the converter in *.xaml file
            converter = FindResource("converter") as Converter;

            //load all saved works after loading all the rules
            AddedFiles_ListView.ItemsSource = _filenames;
            PreviewChanges_ListView.ItemsSource = _filenames;
        }

        //close window
        private void Window_Closed(object sender, EventArgs e)
        {
            //Save all the things before the window is closed
            SaveCurrentWork();
        }

       

        //UI event handlers
        //LoadRules button handler
        private void LoadRules_Click(object sender, RoutedEventArgs e)
        {
            ruleFile = "CVRules.txt";

            PreviewChanges_ListView.ItemsSource = null;
            PreviewChanges_ListView.ItemsSource = _filenames;
        }


        List<string> specialChar = new List<string>
        {
            "!", "@", "#", "$", "%", "^", "&", "(", ")", "`", "~", "|", "/", "\\", ":", "?", "*", "<", ">", "\""
        };
        private bool isContainSpecialCharacters(string filename)
        {
            bool result = false;

            foreach(var c in specialChar)
            {
                if (filename.Contains(c))
                {
                    result = true;
                }    
            }

            return result;
        }

        private bool isOver255(string filename)
        {
            bool result = false;

            if (filename.Length > 255)
            {
                result = true;
            }

            return result;
        }

        //SelectFile button handler
        private void AddedFiles_Click(object sender, RoutedEventArgs e)
        {
            var screen = new OpenFileDialog();

            if (screen.ShowDialog() == true)
            {
                FileInfo info = new FileInfo(screen.FileName);

                if (isContainSpecialCharacters(info.Name))
                {
                    MessageBox.Show("File contains invalid character(s) !!!");
                }
                else if (isOver255(info.Name))
                {
                    MessageBox.Show("File length is over 255 characters !!!");
                }
                else  //Add if file NAME is not added before.
                {
                    if (!_filenames.Any(x => x.FullName == info.FullName) || _filenames.Count < 1)
                    {
                        _filenames.Add(info);
                    }
                    else
                    {
                        MessageBox.Show("File Name is already addded.");
                    }
                }
            }
        }

        //ApplyChanges button handler
        private void CreateCopy_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            //reset counter
            ResetCountEvent?.Invoke();

            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                for (int i = 0; i < _filenames.Count; i++)
                {
                    string fileName = _filenames[i].Name;
                    string newFileName = _filenames[i].Name.Replace(fileName, Rename(fileName));

                    if (isOver255(System.IO.Path.GetFileNameWithoutExtension(newFileName)))
                    {
                        newFileName = newFileName.Substring(0, 255) + System.IO.Path.GetExtension(newFileName);
                    }
                    
                    string newFilePath = System.IO.Path.Combine(fbd.SelectedPath, newFileName);
                    string newFileNameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(newFilePath);
                    string fileExtension = System.IO.Path.GetExtension(newFilePath);

                    if (File.Exists(newFilePath))
                    {
                        int count = 1;
                        while (true)
                        {
                            string duplicatedFileName = fbd.SelectedPath + "/" + newFileNameWithoutExtension + "(" + count + ")" + fileExtension;

                            if (File.Exists(duplicatedFileName))
                            {
                                count++;
                            }
                            else
                            {
                                File.Copy(_filenames[i].FullName, duplicatedFileName);
                                break;
                            }
                        }
                    }
                    else
                    {
                        if (System.IO.Path.GetExtension(fileName) == "")
                        {
                            Directory.CreateDirectory(System.IO.Path.Combine(fbd.SelectedPath, newFileName));
                        }
                        else
                        {
                            File.Copy(_filenames[i].FullName, newFilePath);
                        }
                    }
                }
                
                MessageBox.Show("Apply changes successfully!");
            }

        }

        //Save button handler
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            SaveCurrentWork();
        }

        //LoadSavedWork button handler
        private void LoadSavedWork_Click(object sender, RoutedEventArgs e)
        {
            LoadSavedWork();
        }

        //Checkbox handler
        //Check
        private void Checkbox_Checked(object sender, RoutedEventArgs e)
        {
            PropertyInfo info = sender.GetType().GetProperty("Name");
            IRule rule = RuleFactory.Parse(info.GetValue(sender).ToString());
            
            //reset counter
            ResetCountEvent?.Invoke();

            string temp = "";

            if (rule.GetType().GetProperties().Count() > 1)
            {
                var screen = new EditParametersWindow(rule);
                if (screen.ShowDialog() == true)
                {

                }
            }

            ruleList.Add(rule);
            foreach(var r in ruleList)
            {
                temp += r.Name + "\n";
            }

            Debug.WriteLine(temp);
            //inovoke PropertyChanged evnt of Observable Collection
            converter.RuleList = ruleList.ToList();
            PreviewChanges_ListView.ItemsSource = null;
            PreviewChanges_ListView.ItemsSource = _filenames;
        }

        //Uncheck
        private void Checkbox_Unchecked(object sender, RoutedEventArgs e)
        {
            PropertyInfo info = sender.GetType().GetProperty("Name");
            IRule rule = RuleFactory.Parse(info.GetValue(sender).ToString());
            string temp = "";

            //reset counter
            ResetCountEvent?.Invoke();

            IRule del = ruleList.Single(x => x.Name == rule.Name);
            ruleList.Remove(del);

            if (ruleList.Count == 0)
            {
                Debug.WriteLine("empty");
            }
            foreach (var r in ruleList)
            {
                temp += r.Name + "\n";
            }

            Debug.WriteLine(temp);
            converter.RuleList = ruleList.ToList();
            PreviewChanges_ListView.ItemsSource = null;
            PreviewChanges_ListView.ItemsSource = _filenames;
        }

        private void AddedFolders_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog OF = new FolderBrowserDialog();

            if (OF.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                FileInfo info = new FileInfo(OF.SelectedPath);

                if (isContainSpecialCharacters(info.Name))
                {
                    MessageBox.Show("Folder contains invalid character(s) !!!");
                }
                else if (isOver255(info.Name))
                {
                    MessageBox.Show("Folder length is over 255 characters !!!");
                }
                else
                {
                    if (!_filenames.Any(x => x.FullName == info.FullName) || _filenames.Count < 1)
                    {
                        _filenames.Add(info);
                    }
                    else
                    {
                        MessageBox.Show("Folder name is already addded.");
                    }
                }
            }
        }

        private void ChangeDirectly_Click(object sender, RoutedEventArgs e)
        {
            ResetCountEvent?.Invoke();

            for (int i = 0; i < _filenames.Count; i++)
            {
                string fileName = _filenames[i].Name;
                string renamedFileName = Rename(_filenames[i].Name);

                if (isOver255(System.IO.Path.GetFileNameWithoutExtension(renamedFileName)))
                {
                    renamedFileName = System.IO.Path.GetFileNameWithoutExtension(renamedFileName).Substring(0, 255) + System.IO.Path.GetExtension(renamedFileName);
                }

                string newFileFullName = _filenames[i].FullName.Replace(fileName, renamedFileName);

                if (System.IO.Path.GetExtension(fileName) == "")
                {
                    Directory.Move(_filenames[i].FullName, newFileFullName);
                }
                else
                {
                    File.Move(_filenames[i].FullName, newFileFullName);
                }
            }
            _filenames.Clear();
            MessageBox.Show("Apply changes successfully!");
        }
    }
}
