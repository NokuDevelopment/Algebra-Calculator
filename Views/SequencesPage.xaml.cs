using AlgebraCalculatorApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml.Controls;

namespace AlgebraCalculatorApp.Views
{
    public sealed partial class SequencesPage : Page
    {
        public SequencesViewModel ViewModel { get; } = new SequencesViewModel();

        public SequencesPage()
        {
            InitializeComponent();
        }

        //update SequencesOutput, SequencesModeLabel

        private void SequencesDataInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            //triggered by SequencesDataInput
            if (SequencesDataInput.Text == "")
            {
                SequencesOutput.Text = "";
                SequencesModeLabel.Text = "Mode: Waiting for input";
            }
            else
            {
                try
                {
                    SequencesOutput.Text = findRule(SequencesDataInput.Text);
                    SequencesModeLabel.Text = "Mode: Find arithmetic or geometric rule";
                }
                catch
                {
                    SequencesOutput.Text = "";
                    SequencesModeLabel.Text = "Mode: Find rule (invalid syntax)";
                }
            }

        }

        private void RecursiveEqInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            //triggered by RecursiveEqInput, RecursiveInitialValue, RecursiveSolveValue
            if (RecursiveEqInput.Text == "" || RecursiveInitialValue.Text == "" || RecursiveSolveValue.Text == "")
            {
                SequencesOutput.Text = "";
                SequencesModeLabel.Text = "Recursion (waiting for input)";
                if (RecursiveEqInput.Text == "" && RecursiveInitialValue.Text == "" && RecursiveSolveValue.Text == "")
                {
                    SequencesModeLabel.Text = "Waiting for input";
                }
            }
            else
            {
                try
                {
                    SequencesOutput.Text = solveRecursion(1, Int32.Parse(RecursiveInitialValue.Text), Int32.Parse(RecursiveSolveValue.Text), RecursiveEqInput.Text);
                    SequencesModeLabel.Text = "Recursion";
                }
                catch
                {
                    SequencesOutput.Text = "";
                    SequencesModeLabel.Text = "Recursion (syntax error)";
                }
            }
        }

        private void SigmaEqInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            //triggered by SigmaEqInput, SigmaInitialValue, SigmaMaxValue
            if (SigmaEqInput.Text == "" || SigmaInitialValue.Text == "" || SigmaMaxValue.Text == "")
            {
                SequencesOutput.Text = "";
                SequencesModeLabel.Text = "Sigma (waiting for input)";
                if (SigmaEqInput.Text == "" && SigmaInitialValue.Text == "" && SigmaMaxValue.Text == "")
                {
                    SequencesModeLabel.Text = "Waiting for input";
                }
            }
            else
            {
                try
                {
                    SequencesOutput.Text = solveSigma(Int32.Parse(SigmaInitialValue.Text), Int32.Parse(SigmaMaxValue.Text), SigmaEqInput.Text);
                    SequencesModeLabel.Text = "Sigma";
                }
                catch
                {
                    SequencesOutput.Text = "";
                    SequencesModeLabel.Text = "Sigma (syntax error)";
                }
            }
        }

        public static string findRule(string data)
        {
            string[] dataArr = data.Split(new string[] { ", " }, StringSplitOptions.None);
            List<double> dataList = new List<double>();

            foreach (string c in dataArr)
            {
                try
                {
                    double tempint = double.Parse(c);
                    dataList.Add(tempint);
                }
                catch (FormatException)
                {
                    return "Invalid character";
                }
            }

            string checkForAddition(List<double> inputData)
            {
                double lastnum = 0;
                double checking = 0;
                for (int i = 0; i < inputData.Count(); i++)
                {
                    if (i == 0)
                    {
                        lastnum = inputData[i];
                        continue;
                    }

                    if (i == 1)
                    {
                        checking = inputData[i] - lastnum;
                    }

                    if (checking < 0)
                    {
                        return "false";
                    }

                    if (inputData[i] - checking == lastnum)
                    {
                        lastnum = inputData[i];
                        continue;
                    }
                    else
                    {
                        return "false";
                    }
                }

                return checking.ToString();
            }

            string checkForSubtraction(List<double> inputData)
            {
                double lastnum = 0;
                double checking = 0;
                for (int i = 0; i < inputData.Count(); i++)
                {
                    if (i == 0)
                    {
                        lastnum = inputData[i];
                        continue;
                    }

                    if (i == 1)
                    {
                        checking = lastnum - inputData[i];
                    }

                    if (checking < 0)
                    {
                        return "false";
                    }

                    if (inputData[i] + checking == lastnum)
                    {
                        lastnum = inputData[i];
                        continue;
                    }
                    else
                    {
                        return "false";
                    }
                }

                return checking.ToString();
            }

            string checkForMultiplication(List<double> inputData)
            {
                double lastnum = 0;
                double coefficient = 0;
                for (int i = 0; i < inputData.Count(); i++)
                {
                    if (i == 0)
                    {
                        lastnum = inputData[i];
                        continue;
                    }
                    if (i == 1)
                    {
                        coefficient = (double)((decimal)inputData[i] / (decimal)lastnum);
                    }
                    if (coefficient < 1 && coefficient > 0)
                    {
                        return "false";
                    }
                    if (((decimal)inputData[i] / (decimal)lastnum) == (decimal)coefficient)
                    {
                        lastnum = inputData[i];
                        continue;
                    }
                    return "false";
                }
                return coefficient.ToString();
            }

            string checkForDivision(List<double> inputData)
            {
                double lastnum = 0;
                double coefficient = 0;
                for (int i = 0; i < inputData.Count(); i++)
                {
                    if (i == 0)
                    {
                        lastnum = inputData[i];
                        continue;
                    }
                    if (i == 1)
                    {
                        coefficient = (double)((decimal)lastnum / (decimal)inputData[i]);
                    }
                    if (coefficient < 1 && coefficient > 0)
                    {
                        return "false";
                    }
                    if (((decimal)lastnum / (decimal)inputData[i]) == (decimal)coefficient)
                    {
                        lastnum = inputData[i];
                        continue;
                    }
                    return "false";
                }
                return coefficient.ToString();
            }

            string checkForExponent(List<double> inputData)
            {
                double lastnum = 0;
                double degree = 1;
                for (int i = 0; i < inputData.Count(); i++)
                {
                    if (i == 0)
                    {
                        lastnum = inputData[i];
                        continue;
                    }
                    if (i == 1)
                    {
                        for (int m = 2; m < 10; m++)
                        {
                            if ((double)Math.Pow(lastnum, m) == inputData[i])
                            {
                                degree = m;
                                break;
                            }
                        }
                    }
                    if (degree == 1)
                    {
                        return "false";
                    }
                    if (Math.Pow(lastnum, degree) == inputData[i])
                    {
                        lastnum = inputData[i];
                        continue;
                    }
                    else
                    {
                        return "false";
                    }
                }
                return degree.ToString();
            }

            string checkForSqrt(List<double> inputData)
            {
                double lastnum = 0;
                double degree = 1;
                for (int i = 0; i < inputData.Count(); i++)
                {
                    if (i == 0)
                    {
                        lastnum = inputData[i];
                        continue;
                    }
                    if (i == 1)
                    {
                        for (int m = 2; m < 10; m++)
                        {
                            if ((double)Math.Pow(inputData[i], m) == lastnum)
                            {
                                degree = m;
                                break;
                            }
                        }
                    }
                    if (degree == 1)
                    {
                        return "false";
                    }
                    if (Math.Pow(inputData[i], degree) == lastnum)
                    {
                        lastnum = inputData[i];
                        continue;
                    }
                    else
                    {
                        return "false";
                    }
                }
                return degree.ToString();
            }

            if (checkForAddition(dataList) != "false")
            {
                return $"Rule: +{checkForAddition(dataList)}";
            }
            else if (checkForSubtraction(dataList) != "false")
            {
                return $"Rule: -{checkForSubtraction(dataList)}";
            }
            else if (checkForMultiplication(dataList) != "false")
            {
                return $"Rule: *{checkForMultiplication(dataList)}";
            }
            else if (checkForDivision(dataList) != "false")
            {
                return $"Rule: ÷{checkForDivision(dataList)}";
            }
            else if (checkForExponent(dataList) != "false")
            {
                return $"Rule: ^{checkForExponent(dataList)}";
            }
            else if (checkForSqrt(dataList) != "false")
            {
                return $"Rule: {checkForSqrt(dataList)}√";
            }

            return "Could not find a regular rule";
        }

        public static string solveSigma(int startVal, int endVal, string eq)
        {
            string eqString = "";

            for (int i = 0; i < eq.Length; i++)
            {
                if (eq[i] == 'n' || eq[i] == 'N')
                {
                    eqString += "x";
                    continue;
                }
                eqString += eq[i].ToString();
            }

            int sigmaSum = 0;

            for (int i = startVal; i <= endVal; i++)
            {
                sigmaSum += Int32.Parse(DetectPage.solveItem(eqString, i.ToString()));
            }

            return $"Σ {eq} = {sigmaSum.ToString()}";
        }

        public static string solveRecursion(int refVal, int refResult, int nVal, string eq)
        {
            string[] eqArr = eq.Split(new string[] { "f(n) = f(n - 1)" }, StringSplitOptions.None);
            eq = eqArr[1];
            string eqString = $"x{eq}";
            int difference = nVal - refVal;
            int lastResult = refResult;
            Console.WriteLine(lastResult.ToString());
            for (int i = 0; i < difference; i++)
            {
                lastResult = Int32.Parse(DetectPage.solveItem(eqString, lastResult.ToString()));
            }

            return $"f({nVal.ToString()}) = {lastResult.ToString()}";
        }

    }
}
