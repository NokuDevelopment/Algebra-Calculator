using AlgebraCalculatorApp.ViewModels;
using System;
using Windows.UI.Xaml.Controls;

namespace AlgebraCalculatorApp.Views
{
    public sealed partial class FunctionCompPage : Page
    {
        public FunctionCompViewModel ViewModel { get; } = new FunctionCompViewModel();

        public FunctionCompPage()
        {
            InitializeComponent();
        }

        private void FxEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            FnCompOutput.Text = FnCompLauncher(FxEntry.Text, GxEntry.Text, FCXvalEntry.Text, FCMode.SelectedItem.ToString());
        }

        private void GxEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            //gx currently calls fxentry event handler
        }

        private void FCMode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FnCompOutput.Text = FnCompLauncher(FxEntry.Text, GxEntry.Text, FCXvalEntry.Text, FCMode.SelectedItem.ToString());
        }

        public static string FnCompLauncher(string fx, string gx, string sxval, string mode)
        {
            try
            {
                if (mode == "f(g(x))")
                {
                    return $"f(g({sxval})) = " + DetectPage.SolveItem(fx, DetectPage.SolveItem(gx, sxval));
                }
                else if (mode == "g(f(x))")
                {
                    return $"g(f({sxval})) = " + DetectPage.SolveItem(gx, DetectPage.SolveItem(fx, sxval));
                }
            }
            catch
            {
                return "";
            }

            return "Invalid mode selected";
        }
    }
}
