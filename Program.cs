using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.Windows.Markup;
using Application = System.Windows.Forms.Application;
using Button = System.Windows.Controls.Button;
using wpfUserControl = System.Windows.Controls.UserControl;
using FromUserControl = System.Windows.Forms.UserControl;
using MessageBox = System.Windows.Forms.MessageBox;

namespace Helloworld
{
    public class Program
    {
        public static mutex X = new mutex();
        [STAThread]
        public static void Main()
        {
            Application.AddMessageFilter(X);
            Application.ApplicationExit += Application_ApplicationExit;
            Application.Run(new Main());
            
        }

        static void Application_ApplicationExit(object sender, EventArgs e)
        {
           Application.RemoveMessageFilter(X);
        }
    }

    public class mutex : IMessageFilter
    {
        public bool PreFilterMessage(ref Message m)
        {
            // Trap WM_LBUTTONDOWN
            if (m.Msg == 0x201)
            {
                System.Diagnostics.Debug.WriteLine("BEEP!");
            }
            return false;
        }
    }

    public class Main : Form
    {
        const int WM_PARENTNOTIFY = 0x210;
        const int WM_LBUTTONDOWN = 0x201;
        //protected override void DefWndProc(ref Message m)
        //{
        //        base.WndProc(ref m);
        //    if (m.Msg == WM_LBUTTONDOWN)
        //        DoIt();
            

        //}

        protected override void DefWndProc(ref Message m)
        {
            
            if (m.Msg == WM_LBUTTONDOWN)
            {
                DoIt();
            }
            base.DefWndProc(ref m);
        }

        private void DoIt()
        {
           Trace.Write("click ok \n");
        }

        private readonly ElementHost _elementHost;
        private readonly wpfUserControl _from;

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _elementHost.Dispose();
            }
            base.Dispose(disposing);
        }

        public Main()
        {
            _elementHost = new ElementHost();
            var currentDirectory = Environment.CurrentDirectory;
            while (Directory.Exists(currentDirectory))
            {
                var name = new DirectoryInfo(currentDirectory).Name;
                if (name.StartsWith("Helloworld"))
                {
                    break;
                }
                currentDirectory = Directory.GetParent(currentDirectory).ToString();
            }
            var s = currentDirectory + "\\Helloworld.xaml";
            if (File.Exists(s))
            {
                using (var f= File.OpenRead(s))
                {
                    _from = (wpfUserControl)XamlReader.Load(f);
                    _elementHost.Child = _from;
                    _elementHost.Dock = DockStyle.Fill;
                }

            }
            
            Controls.Add(_elementHost);

        }

        //void WalkDownLogicalTree(object current)
        //{
        //    DoSomethingWithObjectInLogicalTree(current);

        //    // The logical tree can contain any type of object, not just 
        //    // instances of DependencyObject subclasses.  LogicalTreeHelper
        //    // only works with DependencyObject subclasses, so we must be
        //    // sure that we do not pass it an object of the wrong type.
        //    var depObj = current as DependencyObject;

        //    if (depObj != null)
        //        foreach (object logicalChild in LogicalTreeHelper.GetChildren(depObj))
        //            WalkDownLogicalTree(logicalChild);
        //}

        //private void DoSomethingWithObjectInLogicalTree(object current)
        //{
        //    var uiElement = current as Button;
        //    if (uiElement != null && uiElement.Name != string.Empty)
        //    {
        //        Wpfb.Add(uiElement.Name,uiElement);
        //    }
        //}

        protected override void OnLoad(EventArgs e)
        {
            //WalkDownLogicalTree(from);

            //if (Wpfb.Count > 0)
            //{
            //    Button x;
            //    if (Wpfb.TryGetValue("B1", out x))
            //    {
            //        x.Click += findName_Click;
            //    }
            //}

           // var findName = @from.FindName("B1");
            //MessageBox.Show(findName == null ? "error" : "OK");

            var b = _from.FindName("B1");
            if (b != null)
            {
                var button = b as Button;
                if (button != null) button.Click += findName_Click;
            }

            base.OnLoad(e);
        }

        void findName_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("hi");
        }
    }
}
