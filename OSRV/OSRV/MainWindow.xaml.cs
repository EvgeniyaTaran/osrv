using Microsoft.Win32;
using OSRV.Compiler;
using OSRV.Compiler.Errors;
using OSRV.Loggers;
using OSRV.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
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

namespace OSRV
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Robot R1;
        Robot R2;
        Lexer lexer;
        bool isCompiledSuccess = false;
        RobotsActivity _activity = RobotsActivity.None;
        public MainWindow()
        {
            InitializeComponent();
            R1 = new Robot(Robot1, AppWindow);
            R2 = new Robot(Robot2, AppWindow);
            lexer = new Lexer();
            lexer.AddSentenceDefinition(new SentenceDefinition(new Regex(@"((r|R)otate|(m|M)ove)( )*(R|r)(1|2)( )*-?(\d+)( )*")));
            lexer.AddDefinition(new TokenDefinition("action", new Regex(@"((r|R)otate|(m|M)ove)")));
            lexer.AddDefinition(new TokenDefinition("robot", new Regex(@"(R|r)(1|2)")));
            lexer.AddDefinition(new TokenDefinition("parameter", new Regex(@"-?(\d+)")));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (isCompiledSuccess) 
            {
                foreach (var command in lexer.Commands)
                {
                    executeCommand(command);
                }
            }
        }

        private List<string> GenerateCommandText() 
        {
            var textCommands = new List<string>();
            var line = String.Empty;
            foreach (var command in lexer.Commands)
            {
                if (command.RobotName == "R1")
                {
                    if (command.ActionType == ActionType.Move)
                    {
                        line = String.Format("R1.Move({0});", command.Value);
                    }
                    else
                    {
                        line = String.Format("R1.Rotate({0});", command.Value);
                    }
                }
                else
                {
                    if (command.ActionType == ActionType.Move)
                    {
                        line = String.Format("R2.Move({0});", command.Value);
                    }
                    else
                    {
                        line = String.Format("R2.Rotate({0});", command.Value);
                    }
                }
                textCommands.Add(line);
            }
            return textCommands;
        }

        private void executeCommand(MyCommand command) 
        {
            if (command.RobotName == "R1")
            {
                if (command.ActionType == ActionType.Move)
                {
                    R1.Move(command.Value);
                }
                else
                {
                    R1.Rotate(command.Value);
                }
            }
            else 
            {
                if (command.ActionType == ActionType.Move)
                {
                    R2.Move(command.Value);
                }
                else
                {
                    R2.Rotate(command.Value);
                }
            }
        }

        private void btnCompile_Click(object sender, RoutedEventArgs e)
        {
            Canvas.SetLeft(AppWindow.Robot1, 0);
            Canvas.SetLeft(AppWindow.Robot2, 20);
            tbErrors.Text += (DateTime.Now.ToString() + ": Start compile...\r\n");
            lexer.Commands.Clear();
            lexer.SemanticErrors.Clear();
            lexer.SyntaxErrors.Clear();

            lexer.ParseToSentences(tbCode.Text);
            if (lexer.SemanticErrors.Any() || lexer.SyntaxErrors.Any())
            {
                var errors = String.Empty;
                foreach (var error in lexer.SemanticErrors)
                {
                    tbErrors.Text += (error.Message + "\r\n");
                }
                foreach (var error in lexer.SyntaxErrors)
                {
                    tbErrors.Text += (error.Message + "\r\n");
                }
                List<Error> listOfAllErrors = new List<Error>();

                //////////////////////////////////////////////////
                //добавить три строки:
                listOfAllErrors.AddRange(lexer.SyntaxErrors);
                listOfAllErrors.AddRange(lexer.SemanticErrors);
                CompilationLogger.Instance.Log(listOfAllErrors);
            }
            else
            {
                tbErrors.Text = "Compiled success";
                isCompiledSuccess = true;
                CompilationLogger.Instance.Log(new string[] { "Компиляция выполнена успешно." });
                ActionLogger.Instance.Log(new string[] { tbCode.Text });
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            tbCode.Text = String.Empty;
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog of = new OpenFileDialog();
            of.FileName = "D:\\Log.txt";
            string file = File.ReadAllText(of.FileName);
            MessageBox.Show(file);
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            AppWindow.Close();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            string programText = tbCode.Text;
            File.WriteAllText("D:\\UserInput.txt", programText);
            MessageBox.Show("Your program was saved to D:\\UserInput.txt");
        }
    }
}
