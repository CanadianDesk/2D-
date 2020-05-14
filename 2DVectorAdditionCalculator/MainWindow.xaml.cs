using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Policy;
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

namespace _2DVectorAdditionCalculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public int[] NumVectorChoices { get; set; }
        public int NumVectors { get; set; }
        public string[] VectorSettingChoices { get; set; }
        public int VectorASetting { get; set; }
        public int VectorBSetting { get; set; }
        public int VectorCSetting { get; set; }
        public int VectorDSetting { get; set; }
        public bool AIsEnabled { get; set; }
        public bool BIsEnabled { get; set; }
        public bool CIsEnabled { get; set; }
        public bool EIsEnabled { get; set; }
        public bool DIsEnabled { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            NumVectorChoices = new int[] { 1, 2, 3, 4, 5 };
            VectorSettingChoices = new string[] { "Mag. and Dir.", "X & Y Components" };
            DataContext = this;

            //disables all rows on start
            DisableRow(1);
            DisableRow(2);
            DisableRow(3);
            DisableRow(4);
            DisableRow(5);

        }

        private void NumVectorsChanged(object sender, SelectionChangedEventArgs e)
        {
            NumVectors = CBNumVectors.SelectedIndex + 1;
            for (int i = 5; i > NumVectors; i--)
            {
                DisableRow(i);
            }
            for(int i = 1; i <= NumVectors; i++)
            {
                EnableRow(i);
            }
        }

        private void CalculateSum(object sender, RoutedEventArgs e)
        {
            if (CheckVectorSettings())
            {
                if (CheckParsable())
                {
                    Calculate();
                }
                else
                {
                    Output("Error. Input(s) not readable as a number");
                }
            }
            else
            {
                Output("Error. Inputs for vector(s) has not been specified as \"Mag. and Dir.\" or as \"X & Y Components\"");
            }
        }
        bool CheckParsable() // checks if every user input is parsable as a double
        {
            if (!double.TryParse(VectorAMag.Text, out _) && VectorAMag.IsEnabled) { return false; }
            if (!double.TryParse(VectorADir.Text, out _) && VectorADir.IsEnabled) { return false; }
            if (!double.TryParse(VectorAX.Text, out _) && VectorAX.IsEnabled) { return false; }
            if (!double.TryParse(VectorAY.Text, out _ ) && VectorAY.IsEnabled) { return false; }

            if (!double.TryParse(VectorBMag.Text, out _) && VectorBMag.IsEnabled) { return false; }
            if (!double.TryParse(VectorBDir.Text, out _) && VectorBDir.IsEnabled) { return false; }
            if (!double.TryParse(VectorBX.Text, out _) && VectorBX.IsEnabled) { return false; }
            if (!double.TryParse(VectorBY.Text, out _) && VectorBY.IsEnabled) { return false; }

            if (!double.TryParse(VectorCMag.Text, out _) && VectorCMag.IsEnabled) { return false; }
            if (!double.TryParse(VectorCDir.Text, out _) && VectorCDir.IsEnabled) { return false; }
            if (!double.TryParse(VectorCX.Text, out _) && VectorCX.IsEnabled) { return false; }
            if (!double.TryParse(VectorCY.Text, out _) && VectorCY.IsEnabled) { return false; }

            if (!double.TryParse(VectorDMag.Text, out _) && VectorDMag.IsEnabled) { return false; }
            if (!double.TryParse(VectorDDir.Text, out _) && VectorDDir.IsEnabled) { return false; }
            if (!double.TryParse(VectorDX.Text, out _) && VectorDX.IsEnabled) { return false; }
            if (!double.TryParse(VectorDY.Text, out _) && VectorDY.IsEnabled) { return false; }

            if (!double.TryParse(VectorEMag.Text, out _) && VectorEMag.IsEnabled) { return false; }
            if (!double.TryParse(VectorEDir.Text, out _) && VectorEDir.IsEnabled) { return false; }
            if (!double.TryParse(VectorEX.Text, out _) && VectorEX.IsEnabled) { return false; }
            if (!double.TryParse(VectorEY.Text, out _) && VectorEY.IsEnabled) { return false; }

            return true;
        }
        bool CheckVectorSettings() // checks if every vector has a setting selected
        {
            if ((string)CBVectorA.SelectedItem != "Mag. and Dir." && (string)CBVectorA.SelectedItem != "X & Y Components" && CBVectorA.IsEnabled) { return false; }
            if ((string)CBVectorB.SelectedItem != "Mag. and Dir." && (string)CBVectorB.SelectedItem != "X & Y Components" && CBVectorB.IsEnabled) { return false; }
            if ((string)CBVectorC.SelectedItem != "Mag. and Dir." && (string)CBVectorC.SelectedItem != "X & Y Components" && CBVectorC.IsEnabled) { return false; }
            if ((string)CBVectorD.SelectedItem != "Mag. and Dir." && (string)CBVectorD.SelectedItem != "X & Y Components" && CBVectorD.IsEnabled) { return false; }
            if ((string)CBVectorE.SelectedItem != "Mag. and Dir." && (string)CBVectorE.SelectedItem != "X & Y Components" && CBVectorE.IsEnabled) { return false; }
            return true;
        }
        void Output(string str) // outputs text to error message box
        {
            ErrorText.Text = str;
        }
        double DegreesToRadians(double num) => num * Math.PI / 180;
        double RadiansToDegrees(double num) => num * 180 / Math.PI;

        void Calculate()
        {
            double xSum = 0;
            double ySum = 0;
            double rMag;
            double rDir = 0;
            List<double> xComponents = new List<double>();
            List<double> yComponents = new List<double>();

            #region Add x and y components to list
            if (NumVectors >= 1)
            {
                if ((string)CBVectorA.SelectedItem == "Mag. and Dir.")
                {
                    xComponents.Add(Math.Cos(DegreesToRadians(double.Parse(VectorADir.Text))) * double.Parse(VectorAMag.Text));
                    yComponents.Add(Math.Sin(DegreesToRadians(double.Parse(VectorADir.Text))) * double.Parse(VectorAMag.Text));
                }
                else
                {
                    xComponents.Add(double.Parse(VectorAX.Text));
                    yComponents.Add(double.Parse(VectorAY.Text));
                }
                VectorAX.Text = xComponents[0].ToString();
                VectorAY.Text = yComponents[0].ToString();
            }
            if (NumVectors >= 2)
            {
                if ((string)CBVectorB.SelectedItem == "Mag. and Dir.")
                {
                    xComponents.Add(Math.Cos(DegreesToRadians(double.Parse(VectorBDir.Text))) * double.Parse(VectorBMag.Text));
                    yComponents.Add(Math.Sin(DegreesToRadians(double.Parse(VectorBDir.Text))) * double.Parse(VectorBMag.Text));
                }
                else
                {
                    xComponents.Add(double.Parse(VectorBX.Text));
                    yComponents.Add(double.Parse(VectorBY.Text));
                }
                VectorBX.Text = xComponents[1].ToString();
                VectorBY.Text = yComponents[1].ToString();
            }
            if (NumVectors >= 3)
            {
                if ((string)CBVectorC.SelectedItem == "Mag. and Dir.")
                {
                    xComponents.Add(Math.Cos(DegreesToRadians(double.Parse(VectorCDir.Text))) * double.Parse(VectorCMag.Text));
                    yComponents.Add(Math.Sin(DegreesToRadians(double.Parse(VectorCDir.Text))) * double.Parse(VectorCMag.Text));
                }
                else
                {
                    xComponents.Add(double.Parse(VectorCX.Text));
                    yComponents.Add(double.Parse(VectorCY.Text));
                }
                VectorCX.Text = xComponents[2].ToString();
                VectorCY.Text = yComponents[2].ToString();
            }
            if (NumVectors >= 4)
            {
                if ((string)CBVectorD.SelectedItem == "Mag. and Dir.")
                {
                    xComponents.Add(Math.Cos(DegreesToRadians(double.Parse(VectorDDir.Text))) * double.Parse(VectorDMag.Text));
                    yComponents.Add(Math.Sin(DegreesToRadians(double.Parse(VectorDDir.Text))) * double.Parse(VectorDMag.Text));
                }
                else
                {
                    xComponents.Add(double.Parse(VectorDX.Text));
                    yComponents.Add(double.Parse(VectorDY.Text));
                }
                VectorDX.Text = xComponents[3].ToString();
                VectorDY.Text = yComponents[3].ToString();
            }
            if (NumVectors >= 5)
            {
                if ((string)CBVectorE.SelectedItem == "Mag. and Dir.")
                {
                    xComponents.Add(Math.Cos(DegreesToRadians(double.Parse(VectorEDir.Text))) * double.Parse(VectorEMag.Text));
                    yComponents.Add(Math.Sin(DegreesToRadians(double.Parse(VectorEDir.Text))) * double.Parse(VectorEMag.Text));
                }
                else
                {
                    xComponents.Add(double.Parse(VectorEX.Text));
                    yComponents.Add(double.Parse(VectorEY.Text));
                }
                VectorEX.Text = xComponents[4].ToString();
                VectorEY.Text = yComponents[4].ToString();
            }
            #endregion

            foreach (double num in xComponents)
            {
                xSum += num;
            }
            foreach (double num in yComponents)
            {
                ySum += num;
            }

            rMag = Math.Sqrt(ySum * ySum + xSum * xSum);

            #region Remove Close to Zero

            if (rDir < 0.0000001 && rDir > 0) { rDir = 0; }
            if (rMag < 0.0000001 && rMag > 0) { rMag = 0; }
            if (xSum < 0.0000001 && xSum > 0) { xSum = 0; }
            if (ySum < 0.0000001 && ySum > 0) { ySum = 0; }

            #endregion

            #region Find priciple angle

            if (ySum == 0) 
            {
                if (xSum >= 0)
                {
                    rDir = 0;
                }
                else
                {
                    rDir = 180;
                }
            }
            else if (xSum == 0) 
            {
                if (ySum >= 0)
                {
                    rDir = 90;
                }
                else
                {
                    rDir = 270;
                }
            }
            else
            {
                if (ySum >= 0 && xSum >= 0) //quadrant I
                {
                    rDir = Math.Abs(RadiansToDegrees(Math.Atan(ySum / xSum)));
                }
                else if (ySum >= 0 && xSum <= 0) // quadrant II
                {
                    rDir = 180 - Math.Abs(RadiansToDegrees(Math.Atan(ySum / xSum)));
                }
                else if (ySum <= 0 && xSum <= 0) // quadrant III
                {
                    rDir = 180 + Math.Abs(RadiansToDegrees(Math.Atan(ySum / xSum)));
                }
                else if (ySum <= 0 && xSum >= 0) // quadrant IV
                {
                    rDir = 360 - Math.Abs(RadiansToDegrees(Math.Atan(ySum / xSum)));
                }
            }
            #endregion

            TBResultantDirection.Text = rDir.ToString();
            TBResultantMagnitude.Text = rMag.ToString();
            TBResultantX.Text = xSum.ToString();
            TBResultantY.Text = ySum.ToString();
            Output(" ");
        } 
        void DisableRow(int number)
        {
            switch (number)
            {
                case 1:
                    CBVectorA.IsEnabled = false;
                    VectorAMag.IsEnabled = false;
                    VectorADir.IsEnabled = false;
                    VectorAX.IsEnabled = false;
                    VectorAY.IsEnabled = false;
                    break;
                case 2:
                    CBVectorB.IsEnabled = false;
                    VectorBMag.IsEnabled = false;
                    VectorBDir.IsEnabled = false;
                    VectorBX.IsEnabled = false;
                    VectorBY.IsEnabled = false;
                    break;
                case 3:
                    CBVectorC.IsEnabled = false;
                    VectorCMag.IsEnabled = false;
                    VectorCDir.IsEnabled = false;
                    VectorCX.IsEnabled = false;
                    VectorCY.IsEnabled = false;
                    break;
                case 4:
                    CBVectorD.IsEnabled = false;
                    VectorDMag.IsEnabled = false;
                    VectorDDir.IsEnabled = false;
                    VectorDX.IsEnabled = false;
                    VectorDY.IsEnabled = false;
                    break;
                case 5:
                    CBVectorE.IsEnabled = false;
                    VectorEMag.IsEnabled = false;
                    VectorEDir.IsEnabled = false;
                    VectorEX.IsEnabled = false;
                    VectorEY.IsEnabled = false;
                    break;

            }
        }
       
        void EnableRow(int number)
        {
            switch (number)
            {
                case 1:
                    if ((string)CBVectorA.SelectedItem == "Mag. and Dir.")
                    {
                        CBVectorA.IsEnabled = true;
                        VectorAMag.IsEnabled = true;
                        VectorADir.IsEnabled = true;
                    }
                    else if ((string)CBVectorA.SelectedItem == "X & Y Components")
                    {
                        CBVectorA.IsEnabled = true;
                        VectorAX.IsEnabled = true;
                        VectorAY.IsEnabled = true;
                    }
                    else
                    {
                        CBVectorA.IsEnabled = true;
                        VectorAMag.IsEnabled = true;
                        VectorADir.IsEnabled = true;
                        VectorAX.IsEnabled = true;
                        VectorAY.IsEnabled = true;
                    }
                    break;

                case 2:
                    if ((string)CBVectorB.SelectedItem == "Mag. and Dir.")
                    {
                        CBVectorB.IsEnabled = true;
                        VectorBMag.IsEnabled = true;
                        VectorBDir.IsEnabled = true;
                    }
                    else if ((string)CBVectorB.SelectedItem == "X & Y Components")
                    {
                        CBVectorB.IsEnabled = true;
                        VectorBX.IsEnabled = true;
                        VectorBY.IsEnabled = true;
                    }
                    else
                    {
                        CBVectorB.IsEnabled = true;
                        VectorBMag.IsEnabled = true;
                        VectorBDir.IsEnabled = true;
                        VectorBX.IsEnabled = true;
                        VectorBY.IsEnabled = true;
                    }
                    break;

                case 3:
                    if ((string)CBVectorC.SelectedItem == "Mag. and Dir.")
                    {
                        CBVectorC.IsEnabled = true;
                        VectorCMag.IsEnabled = true;
                        VectorCDir.IsEnabled = true;
                    }
                    else if ((string)CBVectorC.SelectedItem == "X & Y Components")
                    {
                        CBVectorC.IsEnabled = true;
                        VectorCX.IsEnabled = true;
                        VectorCY.IsEnabled = true;
                    }
                    else
                    {
                        CBVectorC.IsEnabled = true;
                        VectorCMag.IsEnabled = true;
                        VectorCDir.IsEnabled = true;
                        VectorCX.IsEnabled = true;
                        VectorCY.IsEnabled = true;
                    }
                    break;

                case 4:
                    if ((string)CBVectorD.SelectedItem == "Mag. and Dir.")
                    {
                        CBVectorD.IsEnabled = true;
                        VectorDMag.IsEnabled = true;
                        VectorDDir.IsEnabled = true;
                    }
                    else if ((string)CBVectorD.SelectedItem == "X & Y Components")
                    {
                        CBVectorD.IsEnabled = true;
                        VectorDX.IsEnabled = true;
                        VectorDY.IsEnabled = true;
                    }
                    else
                    {
                        CBVectorD.IsEnabled = true;
                        VectorDMag.IsEnabled = true;
                        VectorDDir.IsEnabled = true;
                        VectorDX.IsEnabled = true;
                        VectorDY.IsEnabled = true;
                    }
                    break;

                case 5:
                    if ((string)CBVectorE.SelectedItem == "Mag. and Dir.")
                    {
                        CBVectorE.IsEnabled = true;
                        VectorEMag.IsEnabled = true;
                        VectorEDir.IsEnabled = true;
                    }
                    else if ((string)CBVectorE.SelectedItem == "X & Y Components")
                    {
                        CBVectorE.IsEnabled = true;
                        VectorEX.IsEnabled = true;
                        VectorEY.IsEnabled = true;
                    }
                    else
                    {
                        CBVectorE.IsEnabled = true;
                        VectorEMag.IsEnabled = true;
                        VectorEDir.IsEnabled = true;
                        VectorEX.IsEnabled = true;
                        VectorEY.IsEnabled = true;
                    }
                    break;

            }
        }

        #region Vector Settings Changed
        private void VectorASettingChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((string)CBVectorA.SelectedItem == "Mag. and Dir.")
            {
                VectorAX.IsEnabled = false;
                VectorAY.IsEnabled = false;
                VectorADir.IsEnabled = true;
                VectorAMag.IsEnabled = true;
            }
            else if ((string)CBVectorA.SelectedItem == "X & Y Components")
            {
                VectorAX.IsEnabled = true;
                VectorAY.IsEnabled = true;
                VectorADir.IsEnabled = false;
                VectorAMag.IsEnabled = false;
            }
        }

        private void VectorBSettingChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((string)CBVectorB.SelectedItem == "Mag. and Dir.")
            {
                VectorBX.IsEnabled = false;
                VectorBY.IsEnabled = false;
                VectorBDir.IsEnabled = true;
                VectorBMag.IsEnabled = true;
            }
            else if ((string)CBVectorB.SelectedItem == "X & Y Components")
            {
                VectorBX.IsEnabled = true;
                VectorBY.IsEnabled = true;
                VectorBDir.IsEnabled = false;
                VectorBMag.IsEnabled = false;
            }
        }

        private void VectorCSettingChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((string)CBVectorC.SelectedItem == "Mag. and Dir.")
            {
                VectorCX.IsEnabled = false;
                VectorCY.IsEnabled = false;
                VectorCDir.IsEnabled = true;
                VectorCMag.IsEnabled = true;
            }
            else if ((string)CBVectorC.SelectedItem == "X & Y Components")
            {
                VectorCX.IsEnabled = true;
                VectorCY.IsEnabled = true;
                VectorCDir.IsEnabled = false;
                VectorCMag.IsEnabled = false;
            }
        }

        private void VectorDSettingChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((string)CBVectorD.SelectedItem == "Mag. and Dir.")
            {
                VectorDX.IsEnabled = false;
                VectorDY.IsEnabled = false;
                VectorDDir.IsEnabled = true;
                VectorDMag.IsEnabled = true;
            }
            else if ((string)CBVectorD.SelectedItem == "X & Y Components")
            {
                VectorDX.IsEnabled = true;
                VectorDY.IsEnabled = true;
                VectorDDir.IsEnabled = false;
                VectorDMag.IsEnabled = false;
            }
        }

        private void VectorESettingChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((string)CBVectorE.SelectedItem == "Mag. and Dir.")
            {
                VectorEX.IsEnabled = false;
                VectorEY.IsEnabled = false;
                VectorEDir.IsEnabled = true;
                VectorEMag.IsEnabled = true;
            }
            else if ((string)CBVectorE.SelectedItem == "X & Y Components")
            {
                VectorEX.IsEnabled = true;
                VectorEY.IsEnabled = true;
                VectorEDir.IsEnabled = false;
                VectorEMag.IsEnabled = false;
            }
        }
        #endregion
    }
}
