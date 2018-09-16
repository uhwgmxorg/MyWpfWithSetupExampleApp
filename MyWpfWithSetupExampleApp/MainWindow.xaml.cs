using System;
using System.IO;
using System.Windows;

namespace MyWpfWithSetupExampleApp
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string _revision = "0";

        /// <summary>
        /// Constructor
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            string machine = "";
            if (IntPtr.Size == 8)
                machine = " x64";
            else
                if (IntPtr.Size == 4)
                machine = " x32";
#if DEBUG
            Title += "    Debug Version " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version + " Revision " + _revision + machine;
#else
            Title += "    Release Version " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version + " Revision " + _revision + machine;
#endif
        }

        /******************************/
        /*       Button Events        */
        /******************************/
        #region Button Events

        /// <summary>
        /// Button_CheckForUpdate_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_CheckForUpdate_Click(object sender, RoutedEventArgs e)
        {
            if(CheckIfUpdateIsAvailable())
                Console.Beep();
            else
            {
                Console.Beep();
                Console.Beep();
            }

        }

        /// <summary>
        /// Button_Download_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Download_Click(object sender, RoutedEventArgs e)
        {
            UpdateTheApplication();
        }

        /// <summary>
        /// Button_Close_Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        #endregion
        /******************************/
        /*      Menu Events          */
        /******************************/
        #region Menu Events

        #endregion
        /******************************/
        /*      Other Events          */
        /******************************/
        #region Other Events

        #endregion
        /******************************/
        /*      Other Functions       */
        /******************************/
        #region Other Functions

        /// <summary>
        /// CheckIfUpdateIsAvailable
        /// </summary>
        /// <returns></returns>
        private bool CheckIfUpdateIsAvailable()
        {
            try
            {
                string URL = Properties.Settings.Default.UpdateURL;
                System.Net.HttpWebRequest myRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(URL);
                myRequest.Method = "GET";
                System.Net.WebResponse myResponse = myRequest.GetResponse();
                StreamReader sr = new StreamReader(myResponse.GetResponseStream(), System.Text.Encoding.UTF8);
                string result = sr.ReadToEnd();
                System.Diagnostics.Debug.WriteLine(result);
                result = result.Replace('\n', ' ');
                sr.Close();
                myResponse.Close();

                System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
                xmlDoc.LoadXml(result);
                System.Xml.XmlNodeList parentNode = xmlDoc.GetElementsByTagName("Version");
                string remoteVersion = parentNode[0].InnerXml.ToString();
                string remoteVersionAddition = "";
                int countOfDots = System.Text.RegularExpressions.Regex.Matches(remoteVersion, "[.]").Count;
                if (countOfDots == 1) remoteVersionAddition = ".0.0"; else remoteVersionAddition = ".0";
                Version sVersionToDownload = new Version(parentNode[0].InnerXml.ToString() + remoteVersionAddition);
                Version sCurrentVersion = new Version(System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString());
                var vResult = sVersionToDownload.CompareTo(sCurrentVersion);
                if (vResult > 0)
                    return true;
                else
                    if (vResult < 0)
                    return false;
                else
                        if (vResult == 0)
                    return false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                return false;
            }
            return true;
        }

        /// <summary>
        /// UpdateTheApplication
        /// </summary>
        private void UpdateTheApplication()
        {
            try
            {
                string mypath = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
                string filename = Path.Combine(mypath, "GUP.exe");
                var proc = System.Diagnostics.Process.Start(filename, "");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }
        #endregion
    }
}
