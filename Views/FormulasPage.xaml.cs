using AlgebraCalculatorApp.ViewModels;
using System;
using Windows.UI.Xaml.Controls;

namespace AlgebraCalculatorApp.Views
{
    public sealed partial class FormulasPage : Page
    {
        public FormulasViewModel ViewModel { get; } = new FormulasViewModel();

        public FormulasPage()
        {
            InitializeComponent();
            database.createDict();
        }

        //output to FormulasOutput

        private void FormulaEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                SearchFormulas();
            }
            catch
            {
                FormulasOutput.Text = "";
            }
        }

        public void SearchFormulas()
        {
            FormulasOutput.Text = "";

            void checkLine()
            {
                if (FormulasOutput.Text != "")
                {
                    FormulasOutput.Text += Environment.NewLine;
                }
            }

            string searchword = FormulasEntry.Text.ToLower();

            foreach (string key in database.dict.Keys)
            {
                bool goodsearch = true;
                for (int i = 0; i < searchword.Length; i++)
                {
                    if (searchword[i].ToString() != key[i].ToString().ToLower())
                    {
                        goodsearch = false;
                        break;
                    }
                }
                if (goodsearch == true)
                {
                    checkLine();
                    FormulasOutput.Text += $"{key}: {database.dict[key]}";
                }
            }

            if (FormulasOutput.Text == "")
            {
                FormulasOutput.Text += "No results match your search";
            }
        }
    }
}
