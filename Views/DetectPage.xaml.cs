using AlgebraCalculatorApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml.Controls;

//TODO Change the version number on the "help" page every build
///TODO OPTIONAL consider adding UWP identifier to build number

namespace AlgebraCalculatorApp.Views
{
    public class DualSystemsEquation
    {
        //variables to store parts of the equation
        public string Coefficient;
        public string Addend;

        //method to assign values to coefficient and addend
        public void SeparateEquation(string eq)
        {
            string s = "";

            if (eq.Contains("y="))
            {
                for (int i = 2; i < eq.Length; i++)
                {
                    s += eq[i];
                }
                eq = s;
                s = "";
            }
            if (eq.Contains("y = "))
            {
                for (int i = 4; i < eq.Length; i++)
                {
                    s += eq[i];
                }
                eq = s;
                s = "";
            }

            for (int i = 0; i < eq.Length; i++)
            {
                if (eq[i] != 'x')
                {
                    s += eq[i];
                }
                if (eq[i] == 'x')
                {
                    if (s == "")
                    {
                        this.Coefficient = "1";
                    }
                    else
                    {
                        this.Coefficient = s;
                    }
                    s = "";

                    for (int n = (i + 1); n < eq.Length; n++)
                    {
                        s += eq[n];
                    }

                    if (s == "")
                    {
                        this.Addend = "0";
                    }
                    else
                    {
                        this.Addend = s;
                    }
                    break;
                }
            }
        }
    }

    public class TripleSystemsEquation
    {
        //variables that store the coefficients; imitates one row of a 3x3 matrix
        public string A;
        public string B;
        public string C;
        public string D;

        public void SeparateEquation(string eq)
        {
            string s = "";

            //loop to assign value to variables
            for (int i = 0; i < eq.Length; i++)
            {
                //add eq[i] to s until a variable is found, then assign the value of s
                if (eq[i] == 'x')
                {
                    this.A = s;
                }
                else if (eq[i] == 'y')
                {
                    this.B = s;
                }
                else if (eq[i] == 'z')
                {
                    this.C = s;
                }
                else if (eq[i] == ' ')
                {
                    continue;
                }
                else if (eq[i] == '=')
                {
                    if (eq[i + 1] == ' ')
                    {
                        for (int n = i + 2; n < eq.Length; n++)
                        {
                            s += eq[n];
                        }
                    }
                    else
                    {
                        for (int n = i + 1; n < eq.Length; n++)
                        {
                            s += eq[n];
                        }
                    }

                    this.D = s;
                }
                else
                {
                    s += eq[i];
                    continue;
                }

                //only reached if value of s was just assigned to this.*
                s = "";
            }

            //in case any values are 1
            if (this.A == "")
            {
                this.A = "1";
            }
            if (this.A == "-")
            {
                this.A = "-1";
            }
            if (this.B == "+")
            {
                this.B = "1";
            }
            if (this.B == "-")
            {
                this.B = "-1";
            }
            if (this.C == "+")
            {
                this.C = "1";
            }
            if (this.C == "-")
            {
                this.C = "-1";
            }
        }
    }

    public sealed partial class DetectPage : Page
    {
        public DetectViewModel ViewModel { get; } = new DetectViewModel();

        public DetectPage()
        {
            InitializeComponent();
        }

        private void DetectEqEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            SolveLauncher();
        }

        private void DetectXvalEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            SolveLauncher();
        }

        private void DetectOutput_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        public void SolveLauncher()
        {
            if (DetectEqEntry.Text == "")
            {
                DetectOutput.Text = "";
                StatusLabel.Text = "Waiting for input";
                return;
            }

            try
            {
                string eq = DetectEqEntry.Text.ToString();
                string xval = DetectXvalEntry.Text.ToString();

                string result = SolveType(eq, xval);

                DetectOutput.Text = result;
            }
            catch
            {
                return;
            }
        }

        public string SolveType(string eq, string xval)
        {
            //functions: return error%message if error occurs

            string mode = EqTypeBox.SelectedItem.ToString();

            string result = "";

            if (mode == "Basic/Equation")
            {
                result = findType(eq, xval);
            }
            else if (mode == "Quadratics")
            {
                mode = "quadratic";

                foreach (char c in eq)
                {
                    if (c == '(')
                    {
                        mode = "FOIL";
                    }
                }

                if (mode == "quadratic")
                {
                    result = solveQuadratic(eq);
                }
                else if (mode == "FOIL")
                {
                    result = expandEq(eq);
                }
            }
            else if (mode == "Solve Logs")
            {
                result = solveLogs(eq);
            }
            else if (mode == "Reduce Radicals")
            {
                result = reduceRadical(eq);
            }
            else if (mode == "Prime Factorization")
            {
                result = findFactors(eq);
            }
            else if(mode == "Systems of Linear Equations")
            {
                result = SystemsLauncher(eq);
            }

            if (result.Contains('%'))
            {
                string[] resultArr = result.Split('%');
                StatusLabel.Text = resultArr[0];
                return resultArr[1];
            }

            StatusLabel.Text = "Solving equation";

            return result;
        }

        public static string findType(string eq, string xval)
        {
            foreach (char c in eq)
            {
                if (c == '=')
                {
                    return solveForX(eq);
                }
            }

            return solveItem(eq, xval);
        }

        public static string solveItem(string eq, string xval)
        {
            //---------------------------------------------------------------------//
            //functions declaration begins here

            //function to replace x in equation with the user provided x value
            string replaceX(string stringToReplace)
            {
                //custom split method with no deliminator
                List<string> itemsList = new List<string>();

                foreach (char c in eq)
                {
                    itemsList.Add(c.ToString());
                }

                //if x has a coefficient, made it *x, otherwise insert x
                for (int i = 0; i < itemsList.Count; i++)
                {
                    if (itemsList[i] == "x")
                    {
                        try
                        {
                            //check if item before is number or symbol
                            Int32.Parse(itemsList[i - 1]);
                            itemsList[i] = $"*{xval}";
                        }
                        catch
                        {
                            itemsList[i] = xval;
                        }
                    }
                }

                //condense the list into a string and return it
                string resultval = string.Join("", itemsList);
                return resultval;
            }

            //recursive function to simplify all parenthesis
            string solveParenthesis(string eqString)
            {
                //make sure there are only full sets of parenthesis
                int opencount = 0;
                int closedcount = 0;
                foreach(char c in eqString)
                {
                    if(c == '(')
                    {
                        opencount += 1;
                    }
                    else if(c == ')')
                    {
                        closedcount += 1;
                    }
                }

                if(opencount != closedcount)
                {
                    return "";
                }
                if(opencount == 0)
                {
                    return eqString;
                }

                //find the outside parenthesis
                for (int i = 0; i < eqString.Length; i++)
                {
                    //when open parenthesis is found
                    if (eqString[i] == '(')
                    {
                        int openParenthesis = i;
                        int closeParenthesis = 0;
                        //find close parenthesis
                        for (int m = (eqString.Length - 1); m >= 0; m--)
                        {
                            if (eqString[m] == ')')
                            {
                                closeParenthesis = m;
                                break;
                            }

                            if (m == 0)
                            {
                                return "";
                            }
                        }

                        //get items in between the two parenthesis
                        string inParenthesis = "";
                        for (int m = (openParenthesis + 1); m < closeParenthesis; m++)
                        {
                            inParenthesis += eqString[m];
                        }

                        //recursion to solve inParenthesis
                        inParenthesis = findType(inParenthesis, xval);

                        //replace the parenthesis with the result; check to see if multiplication is present
                        string pre = "";
                        string post = "";
                        //assign a value to pre and post
                        for (int n = 0; n < eqString.Length; n++)
                        {
                            if (n < openParenthesis)
                            {
                                pre += eqString[n];
                            }
                            if (n > closeParenthesis)
                            {
                                post += eqString[n];
                            }
                        }

                        //create the new string
                        try
                        {
                            Int32.Parse(eqString[openParenthesis - 1].ToString());
                            eqString = $"{pre}*{inParenthesis}{post}";
                        }
                        catch
                        {
                            eqString = $"{pre}{inParenthesis}{post}";
                        }
                    }
                }

                return eqString;
            }

            //function to create a list from the full eq
            List<string> createEqList(string eqString)
            {
                List<string> resultList = new List<string>();
                string current = "";

                for (int i = 0; i < eqString.Length; i++)
                {
                    try
                    {
                        Int32.Parse(eqString[i].ToString());
                        current += eqString[i].ToString();
                    }
                    catch
                    {
                        if (eqString[i] == '.')
                        {
                            current += ".";
                        }
                        else
                        {
                            resultList.Add(current);
                            resultList.Add(eqString[i].ToString());
                            current = "";
                        }
                    }
                }

                if (current != "")
                {
                    resultList.Add(current);
                }

                return resultList;
            }

            //function to solve all operations
            List<string> solveOperations(List<string> eqList)
            {
                //perform exponents, multiplication, and division left to right
                for (int i = 0; i < eqList.Count; i++)
                {
                    if (eqList[i] == "^")
                    {
                        eqList[i - 1] = (Math.Pow(double.Parse(eqList[i - 1]), double.Parse(eqList[i + 1]))).ToString();
                        try
                        {
                            eqList.RemoveAt(i + 1);
                        }
                        catch { }
                        eqList.RemoveAt(i);
                        i -= 1;
                        continue;
                    }
                    if (eqList[i] == "*")
                    {
                        eqList[i - 1] = (float.Parse(eqList[i - 1]) * float.Parse(eqList[i + 1])).ToString();
                        try
                        {
                            eqList.RemoveAt(i + 1);
                        }
                        catch { }
                        eqList.RemoveAt(i);
                        i -= 1;
                        continue;
                    }
                    if (eqList[i] == "/")
                    {
                        eqList[i - 1] = (float.Parse(eqList[i - 1]) / float.Parse(eqList[i + 1])).ToString();
                        try
                        {
                            eqList.RemoveAt(i + 1);
                        }
                        catch { }
                        eqList.RemoveAt(i);
                        i -= 1;
                        continue;
                    }
                }

                //perform addition and subtraction
                for (int i = 0; i < eqList.Count; i++)
                {
                    if (eqList[i] == "+")
                    {
                        eqList[i - 1] = (float.Parse(eqList[i - 1]) + float.Parse(eqList[i + 1])).ToString();
                        try
                        {
                            eqList.RemoveAt(i + 1);
                        }
                        catch { }
                        eqList.RemoveAt(i);
                        i -= 1;
                        continue;
                    }
                    if (eqList[i] == "-")
                    {
                        eqList[i - 1] = (float.Parse(eqList[i - 1]) - float.Parse(eqList[i + 1])).ToString();
                        try
                        {
                            eqList.RemoveAt(i + 1);
                        }
                        catch { }
                        eqList.RemoveAt(i);
                        i -= 1;
                        continue;
                    }
                }

                return eqList;
            }


            //-------------------------------------------------------------------------------//
            //item processing begins here

            //subsitute x and parenthesis in eq
            if (eq.Contains('x'))
            {
                eq = replaceX(eq);
            }

            if (eq.Contains('('))
            {
                eq = solveParenthesis(eq);
            }

            //change to list form for further processing
            var eqValuesList = createEqList(eq);

            //perform all operations
            eqValuesList = solveOperations(eqValuesList);

            //convert list to string and return it
            string result = string.Join("", eqValuesList);

            return result;
        }

        public static string solveForX(string eq)
        {
            return "";
        }

        public static string reduceRadical(string eq)
        {
            if (eq.Length < 2)
            {
                return "";
            }

            string[] eqArr = eq.Split('√');

            try
            {
                eqArr[1].ToString();
            }
            catch (IndexOutOfRangeException)
            {
                return "";
            }

            string rawradical = eqArr[1];
            string radical = rawradical;
            string degree = "";

            string outRadical = "1";

            if (radical == "")
            {
                return "";
            }

            if (eqArr[0].Contains(" ") == true)
            {
                string[] outRadicalArr = eqArr[0].Split(' ');
                outRadical = outRadicalArr[0];
                degree = outRadicalArr[1];
            }
            else
            {
                degree = eqArr[0];
            }

            if (degree == "")
            {
                degree = "2";
            }

            try
            {
                double.Parse(radical);
                double.Parse(outRadical);
                double.Parse(degree);
            }
            catch (FormatException)
            {
                return "";
            }

            for (int i = 1; i < 100; i++)
            {
                if ((double.Parse(radical) % Math.Pow((double)i, double.Parse(degree))) == 0)
                {
                    outRadical = (double.Parse(outRadical) * i).ToString();
                    radical = (double.Parse(radical) / (Math.Pow((double)i, double.Parse(degree)))).ToString();
                    i = 1;
                }
            }

            string result = "";

            if (radical == "1")
            {
                result = $"{degree}√{rawradical} = {outRadical}";
            }
            else if (outRadical == "1")
            {
                result = $"{degree}√{rawradical} is already in simplest form";
            }
            else
            {
                result = $"{degree}√{rawradical} = {outRadical} {degree}√{radical}";
            }

            return result;
        }

        public static string solveLogs(string eq)
        {
            string eqType = "unidentified";
            char[] testArr = eq.ToCharArray();
            for (int i = 0; i < testArr.Count(); i++)
                if (testArr[0] == 'l')
                {
                    string[] checkArr = eq.Split(' ');
                    try
                    {
                        checkArr[3].ToString();
                    }
                    catch
                    {
                        return "";
                    }
                    eqType = "log";
                }
                else if (eq.Contains('^'))
                {
                    string[] checkArr = eq.Split(' ');
                    try
                    {
                        checkArr[2].ToString();
                    }
                    catch
                    {
                        return "";
                    }
                    eqType = "exponential";
                }
                else
                {
                    return "";
                }

            if (eqType == "unidentified")
            {
                return "";
            }

            else if (eqType == "log")
            {
                //extract values (log<base> <x> = <degree>)
                string[] eqArr = eq.Split(' ');
                string logbase = eqArr[0];
                string logresult = eqArr[1];
                string logdegree = eqArr[3];

                logbase = logbase.Remove(0, 3);

                string varResult = "";
                string result1 = "";
                string result2 = "";
                string result = "";

                if (logbase == "x")
                {
                    try
                    {
                        decimal logdegreedec = decimal.Parse(logdegree);
                        decimal exponent = (1m / logdegreedec);
                        varResult = Math.Round(Math.Pow(Int32.Parse(logresult), (double)exponent), 3).ToString();
                        result1 = String.Format("log{0} {1} = {2}", varResult, logresult, logdegree);
                        result2 = String.Format("{0}^{1} = {2}", varResult, logdegree, logresult);
                        result = "Logarithm: " + result1 + "   |   Exponential: " + result2;
                    }
                    catch (FormatException) { }
                }
                else if (logresult == "x")
                {
                    try
                    {
                        varResult = Math.Round(Math.Pow(Int32.Parse(logbase), Int32.Parse(logdegree)), 3).ToString();
                        result1 = String.Format("log{0} {1} = {2}", logbase, varResult, logdegree);
                        result2 = String.Format("{0}^{1} = {2}", logbase, logdegree, varResult);
                        result = "Logarithm: " + result1 + "   |   Exponential: " + result2;
                    }
                    catch (FormatException) { }
                }
                else if (logdegree == "x")
                {
                    try
                    {
                        varResult = Math.Round(Math.Log(Int32.Parse(logresult), Int32.Parse(logbase)), 3).ToString();
                    }
                    catch (FormatException)
                    {
                        return "";
                    }
                    result1 = String.Format("log{0} {1} = {2}", logbase, logresult, varResult);
                    result2 = String.Format("{0}^{1} = {2}", logbase, varResult, logresult);
                    result = "Logarithm: " + result1 + "   |   Exponential: " + result2;
                }

                return result;
            }

            else if (eqType == "exponential")
            {
                string[] exArr = eq.Split(' ');
                string exT1 = exArr[0];
                string exResult = exArr[2];

                string[] exT1Arr = exT1.Split('^');
                string exBase = exT1Arr[0];
                string exDegree = exT1Arr[1];

                string varResult = "";
                string result = "";
                string result1 = "";
                string result2 = "";

                if (exResult == "x")
                {
                    try
                    {
                        varResult = Math.Round(Math.Pow(Int32.Parse(exBase), Int32.Parse(exDegree)), 3).ToString();
                    }
                    catch (FormatException)
                    {
                        return "";
                    }
                    result1 = String.Format("{0}^{1} = {2}", exBase, exDegree, varResult);
                    result2 = String.Format("log{0} {1} = {2}", exBase, varResult, exDegree);
                    result = "Exponential: " + result1 + "   |   Logarithm: " + result2;
                }
                else if (exDegree == "x")
                {
                    try
                    {
                        varResult = Math.Round(Math.Log(Int32.Parse(exResult), Int32.Parse(exBase)), 3).ToString();
                    }
                    catch (FormatException)
                    {
                        return "";
                    }
                    result1 = String.Format("{0}^{1} = {2}", exBase, varResult, exResult);
                    result2 = String.Format("log{0} {1} = {2}", exBase, exResult, varResult);
                    result = "Exponential: " + result1 + "   |   Logarithm: " + result2;
                }
                else if (exBase == "x")
                {
                    try
                    {
                        decimal tempvar = 1m / decimal.Parse(exDegree);
                        varResult = Math.Round(Math.Pow(Int32.Parse(exResult), (double)tempvar), 3).ToString();
                    }
                    catch (FormatException)
                    {
                        return "";
                    }
                    result1 = String.Format("{0}^{1} = {2}", varResult, exDegree, exResult);
                    result2 = String.Format("log{0} {1} = {2}", varResult, exResult, exDegree);
                    result = "Exponential: " + result1 + "   |   Logarithm: " + result2;
                }
                else
                {
                    return "";
                }

                return result;
            }

            else
            {
                return "";
            }
        }

        public static string solveQuadratic(string eq)
        {
            if (eq.Length < 7)
            {
                return "";
            }

            string[] itemsArr = eq.Split('x');
            var itemsList = new List<string>(itemsArr);
            //remove ^2
            if (itemsList[1].Contains("^2"))
            {
                itemsList[1] = itemsList[1].Replace("^2", "");
            }
            //change null values to 1 (eg. x is the same as 1x)
            for (int i = 0; i < itemsList.Count(); i++)
            {
                if (itemsList[i] == "" || itemsList[i] == "+")
                {
                    itemsList[i] = "1";
                }
                else if (itemsList[i] == "-")
                {
                    itemsList[i] = "-1";
                }
            }

            double a = Double.Parse(itemsList[0]);
            double b = Double.Parse(itemsList[1]);
            double c = Double.Parse(itemsList[2]);

            double sqrtpart = b * b - 4 * a * c;
            double x, result1, result2, img;

            if (sqrtpart > 0)
            {
                result1 = (-b + System.Math.Sqrt(sqrtpart)) / (2 * a);
                result2 = (-b - System.Math.Sqrt(sqrtpart)) / (2 * a);

                result1 = Math.Round(result1, 2);
                result2 = Math.Round(result2, 2);
                return "Solutions: " + result1.ToString() + ", " + result2.ToString();
            }
            else if (sqrtpart < 0)
            {
                sqrtpart = -sqrtpart;
                x = -b / (2 * a);
                img = Math.Sqrt(sqrtpart) / (2 * a);

                result1 = Math.Round(img, 2);
                x = Math.Round(x, 2);
                return string.Format("Solutions [Complex]: ({0} + {1}i), ({2} - {3}i)", x, result1, x, result1);
            }
            else

            {
                x = (-b + Math.Sqrt(sqrtpart)) / (2 * a);
                return "Solution: " + (Math.Round(x, 2).ToString());
            }
        }

        public static string expandEq(string eq)
        {
            if (eq.Length < 10)
            {
                return "";
            }
            string[] eqArr = eq.Split('(', ')');
            //Convert to a list and remove the null values
            var eqListT = new List<string>(eqArr);
            var eqList = new List<string>();
            //list to store the values used in operation
            var eqItemsList = new List<string>();
            for (int i = 0; i < eqListT.Count(); i++)
            {
                if (eqListT[i] != "")
                {
                    eqList.Add(eqListT[i]);
                }
            }
            for (int i = 0; i < eqList.Count(); i++)
            {
                //Separate each equation into a list of items
                string[] itemArr = eqList[i].Split('x');
                if (itemArr[0] == "")
                {
                    eqItemsList.Add("1");
                }
                else
                {
                    eqItemsList.Add(itemArr[0]);
                }
                if (itemArr[1] == "")
                {
                    eqItemsList.Add("0");
                }
                else
                {
                    eqItemsList.Add(itemArr[1]);
                }
            }

            string aval = (Int32.Parse(eqItemsList[0]) * Int32.Parse(eqItemsList[2])).ToString();
            string bval = ((Int32.Parse(eqItemsList[0]) * Int32.Parse(eqItemsList[3])) + (Int32.Parse(eqItemsList[1]) * Int32.Parse(eqItemsList[2]))).ToString();
            string cval = (Int32.Parse(eqItemsList[1]) * Int32.Parse(eqItemsList[3])).ToString();

            string aval2 = (Int32.Parse(eqItemsList[0]) * Int32.Parse(eqItemsList[2])).ToString();
            string bval2 = ((Int32.Parse(eqItemsList[0]) * Int32.Parse(eqItemsList[3])) + (Int32.Parse(eqItemsList[1]) * Int32.Parse(eqItemsList[2]))).ToString();
            string cval2 = (Int32.Parse(eqItemsList[1]) * Int32.Parse(eqItemsList[3])).ToString();

            if (Int32.Parse(bval) > 0 || Int32.Parse(bval) == 0)
            {
                bval2 = "+" + bval;
                bval = " + " + bval;
            }
            else if (Int32.Parse(bval) < 0)
            {
                bval2 = "-" + (Math.Abs(Int32.Parse(bval))).ToString();
                bval = " - " + (Math.Abs(Int32.Parse(bval))).ToString();
            }

            if (Int32.Parse(cval) > 0)
            {
                cval2 = "+" + cval;
                cval = " + " + cval;
            }
            else if (Int32.Parse(cval) < 0)
            {
                cval2 = "-" + Math.Abs(Int32.Parse(cval)).ToString();
                cval = " - " + Math.Abs(Int32.Parse(cval)).ToString();
            }
            else if (Int32.Parse(cval) == 0)
            {
                cval2 = "";
                cval = "";
            }

            string result2 = aval2 + "x^2" + bval2 + "x" + cval2;
            result2 += " | " + solveQuadratic(result2);
            return result2;
        }

        public static string findFactors(string eq)
        {
            float number = Int32.Parse(eq);
            float tempnum = Int32.Parse(eq);
            int startnum = 2;
            List<int> divlist = new List<int>();

            for (int i = 0; i < 20; i++)
            {
                number = tempnum / startnum;
                if ((number % 1) > 0)
                {
                    number = tempnum;
                    startnum = startnum + 1;
                }
                else
                {
                    tempnum = number;
                    divlist.Add(startnum);
                    startnum = 2;
                }
            }
            if (divlist.Count() == 1)
            {
                divlist.Insert(0, 1);
            }
            else { }
            string result = String.Join(", ", divlist);
            if (tempnum == 1) { }
            else
            {
                result = result + ", ";
                result = result + tempnum.ToString();
            }

            return result;
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

        private void EqTypeBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SolveLauncher();
        }

        public static string SystemsLauncher(string eq)
        {
            string[] eqArr = eq.Split(new string[] { ", " }, StringSplitOptions.None);

            if (eqArr.Length == 2)
            {
                try
                {
                    return SolveSystems(eqArr[0], eqArr[1]);
                }
                catch
                {
                    return "";
                }
            }
            else if (eqArr.Length == 3)
            {
                try
                {
                    return SolveTripleSystems(eqArr[0], eqArr[1], eqArr[2]);
                }
                catch (Exception ex)
                {
                    return "";
                }
            }
            else if (eqArr.Length > 3)
            {
                return "Maximum number of systems is 3";
            }

            return "";
        }

        public static string SolveSystems(string eq1, string eq2)
        {
            //create an object for each equation
            DualSystemsEquation Equation1 = new DualSystemsEquation();
            DualSystemsEquation Equation2 = new DualSystemsEquation();

            //call method to automatically assign values
            Equation1.SeparateEquation(eq1);
            Equation2.SeparateEquation(eq2);

            //isolate the variables and move them to Equation1
            Equation1.Coefficient = (float.Parse(Equation1.Coefficient) - float.Parse(Equation2.Coefficient)).ToString();
            Equation2.Addend = (float.Parse(Equation2.Addend) - float.Parse(Equation1.Addend)).ToString();

            //divide by the coefficient
            Equation2.Addend = (float.Parse(Equation2.Addend) / float.Parse(Equation1.Coefficient)).ToString();

            //Equation2.Addend is the X value
            string xval = Equation2.Addend;

            //reset the value of Equation1 and then solve for Y
            Equation1.SeparateEquation(eq1);
            string yval = ((float.Parse(Equation1.Coefficient) * float.Parse(xval)) + float.Parse(Equation1.Addend)).ToString();

            //format the answer
            string result = $"X = {Math.Round(double.Parse(xval), 2)}, Y = {Math.Round(double.Parse(yval), 2)}";

            return result;
        }

        public static string SolveTripleSystems(string eq1, string eq2, string eq3)
        {
            //create an object representing a matrix row for each equation
            TripleSystemsEquation R1 = new TripleSystemsEquation();
            TripleSystemsEquation R2 = new TripleSystemsEquation();
            TripleSystemsEquation R3 = new TripleSystemsEquation();

            //assign values to the .a, .b, .c, and .d attributes of each row
            R1.SeparateEquation(eq1);
            R2.SeparateEquation(eq2);
            R3.SeparateEquation(eq3);

            //solve for the determinant, Dx, Dy, and Dz
            string Determinant = SolveDeterminant(R1.A, R1.B, R1.C, R2.A, R2.B, R2.C, R3.A, R3.B, R3.C);
            string Dx = SolveDeterminant(R1.D, R1.B, R1.C, R2.D, R2.B, R2.C, R3.D, R3.B, R3.C);
            string Dy = SolveDeterminant(R1.A, R1.D, R1.C, R2.A, R2.D, R2.C, R3.A, R3.D, R3.C);
            string Dz = SolveDeterminant(R1.A, R1.B, R1.D, R2.A, R2.B, R2.D, R3.A, R3.B, R3.D);

            //Solve for X, Y, and Z
            string Xval = (float.Parse(Dx) / float.Parse(Determinant)).ToString();
            string Yval = (float.Parse(Dy) / float.Parse(Determinant)).ToString();
            string Zval = (float.Parse(Dz) / float.Parse(Determinant)).ToString();

            return $"x={Xval}, y={Yval}, z={Zval}";

            //function to solve for the determinant
            //uses parameters instead of calling object directly
            //so that is can be used to solve for Dx as well
            string SolveDeterminant(string a1, string b1, string c1, string a2, string b2, string c2, string a3, string b3, string c3)
            {
                //store the a, b, and c variables as floats to save space later
                float A1 = float.Parse(a1);
                float B1 = float.Parse(b1);
                float C1 = float.Parse(c1);

                float A2 = float.Parse(a2);
                float B2 = float.Parse(b2);
                float C2 = float.Parse(c2);

                float A3 = float.Parse(a3);
                float B3 = float.Parse(b3);
                float C3 = float.Parse(c3);

                //calculate the determinant
                string DetA = (A1 * ((B2 * C3) - (C2 * B3))).ToString();
                string DetB = (B1 * ((A2 * C3) - (C2 * A3))).ToString();
                string DetC = (C1 * ((A2 * B3) - (B2 * A3))).ToString();

                string determinant = (float.Parse(DetA) - float.Parse(DetB) + float.Parse(DetC)).ToString();

                return determinant;
            }
        }
    }
}
