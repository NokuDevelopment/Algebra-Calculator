using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml.Controls;

namespace AlgebraCalculatorApp.Views
{
    public sealed partial class ConvertPage : Page
    {
        public ConvertPage()
        {
            this.InitializeComponent();
        }

        private void InputBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ConversionGuard();
        }

        private void OutputBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ConversionGuard();
        }

        //called as a preprocessing method to the conversion code
        private void ConversionGuard()
        {
            if(InputUnitBox == null || OutputUnitBox == null || InputBox == null || StatusBlock == null)
            {
                return;
            }

            string unitIn = InputUnitBox.SelectedValue.ToString();
            string targetUnit = OutputUnitBox.SelectedValue.ToString();
            float inputValue = 0;

            //guard clause for non float values
            try
            {
                inputValue = float.Parse(InputBox.Text);
            }
            catch
            {
                StatusBlock.Text = "Input value must be a number";
                return;
            }

            //get and compare the conversion type
            string inputType = GetType(unitIn);
            string outType = GetType(targetUnit);

            if(inputType == "N/A" || outType == "N/A")
            {
                StatusBlock.Text = "Invalid unit";
                return;
            }

            if(inputType != outType)
            {
                StatusBlock.Text = "Units must measure the same property";
                return;
            }

            StatusBlock.Text = "";

            Convert(outType, unitIn, targetUnit, inputValue);
        }

        private void Convert(string type, string inputUnit, string outputUnit, float value)
        {
            //Convert the input to a default type
            float genericValue = ConvertToStandard(inputUnit, value);

            //Convert to output unit
            float output = ConvertToTarget(outputUnit, genericValue);

            bool isRounded = false;
            if(output < 0.0001 && output > -0.0001)
            {
                isRounded = true;
            }

            //convert to string
            string processingString = Math.Round(output, 4).ToString();
            string result = "";

            //return without rounding if PlaneAngle (deg/rad)
            if(type == "PlaneAngle") { OutputBox.Text = output.ToString(); return; }

            //loop to round off only irrational decimals
            for(int i = processingString.Length - 1; i >= 0; i--)
            {
                if(processingString[i] == '.')
                {
                    if(i > 5)
                    {
                        i = 5;
                        isRounded = true;
                    }
                }

                result = processingString[i] + result;
            }

            OutputBox.Text = result;
        }

        //Find the property that a unit measures
        private string GetType(string unit)
        {
            switch (unit)
            {
                case "Kilometer": case "Meter": case "Centimeter": case "Millimeter": case "Mile": case "Yard": case "Foot": case "Inch":
                    return "Length";
                case "Kilogram": case "Gram": case "Milligram": case "Pound": case "Ounce": case "Metric Ton": case "US Ton":
                    return "Weight";
                case "Liter": case "Milliliter": case "Gallon": case "Fluid Ounce": case "Pint": case "Quart": case "Cup":
                    return "Volume";
                case "Fahrenheit": case "Celsius": case "Kelvin":
                    return "Temperature";
                case "Degrees": case "Radians":
                    return "PlaneAngle";
                default:
                    return "N/A";
            }
        }

        private static float ConvertToStandard(string unit, float value)
        {
            switch (unit)
            {
                //length standard: meter
                case "Kilometer":
                    return value * 1000f;
                case "Meter":
                    return value;
                case "Centimeter":
                    return value / 100f;
                case "Millimeter":
                    return value / 1000f;
                case "Nanometer":
                    return value / 1000000000f; //1 billion
                case "Mile":
                    return value * 1609.34f;
                case "Yard":
                    return value / 1.094f;
                case "Foot":
                    return value / 3.281f;
                case "Inch":
                    return value / 39.37f;

                //mass standard: gram
                case "Metric Ton":
                    return value * 1000000f; //1 million
                case "Kilogram":
                    return value * 1000f;
                case "Gram":
                    return value;
                case "Milligram":
                    return value / 1000f;
                case "US Ton":
                    return value * 907185f; //1.016 million
                case "Pound":
                    return value * 453.592f;
                case "Ounce":
                    return value * 28.3495f;

                //volume standard: liter
                case "Liter":
                    return value;
                case "Milliliter":
                    return value / 1000f;
                case "Gallon":
                    return value * 3.78541f;
                case "Quart":
                    return value * 0.946353f;
                case "Pint":
                    return value / 2.113f;
                case "Cup":
                    return value * 0.24f;
                case "Fluid Ounce":
                    return value * 0.0295735f;

                //temperature standard: celcius
                case "Celcius":
                    return value;
                case "Fahrenheit":
                    return ((value - 32f) * (5f / 9f));
                case "Kelvin":
                    return value - 273.15f;

                //PlaneAngle standard: degrees
                case "Degrees":
                    return value;
                case "Radians":
                    return ((value * 180f) / (355f/113f));

                default:
                    return value;
            }
        }

        private static float ConvertToTarget(string unit, float value)
        {
            switch (unit)
            {
                //length standard: meter
                case "Kilometer":
                    return value / 1000f;
                case "Meter":
                    return value;
                case "Centimeter":
                    return value * 100f;
                case "Millimeter":
                    return value * 1000f;
                case "Nanometer":
                    return value * 1000000000f; //1 billion
                case "Mile":
                    return value / 1609.34f;
                case "Yard":
                    return value * 1.094f;
                case "Foot":
                    return value * 3.281f;
                case "Inch":
                    return value * 39.37f;

                //mass standard: gram
                case "Metric Ton":
                    return value / 1000000f; //1 million
                case "Kilogram":
                    return value / 1000f;
                case "Gram":
                    return value;
                case "Milligram":
                    return value * 1000f;
                case "US Ton":
                    return value / 907185f; //1.016 million
                case "Pound":
                    return value / 453.592f;
                case "Ounce":
                    return value / 28.3495f;

                //volume standard: liter
                case "Liter":
                    return value;
                case "Milliliter":
                    return value * 1000f;
                case "Gallon":
                    return value / 3.78541f;
                case "Quart":
                    return value / 0.946353f;
                case "Pint":
                    return value * 2.113f;
                case "Cup":
                    return value / 0.24f;
                case "Fluid Ounce":
                    return value / 0.0295735f;

                //temperature standard: celcius
                case "Celcius":
                    return value;
                case "Fahrenheit":
                    return ((value - 32f) / (5f / 9f));
                case "Kelvin":
                    return value + 273.15f;

                //PlaneAngle standard: degrees
                case "Degrees":
                    return value;
                case "Radians":
                    return ((value * (355f / 113f)) / 180f);

                default:
                    return value;
            }
        }

        private void InputUnitBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ConversionGuard();
        }
    }
}
