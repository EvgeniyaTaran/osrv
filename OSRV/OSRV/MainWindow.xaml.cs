using OSRV.Compiler;
using OSRV.Models;
using System;
using System.Collections.Generic;
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
            R1 = new Robot(Robot1);
            R2 = new Robot(Robot2);
            lexer = new Lexer();
            lexer.AddSentenceDefinition(new SentenceDefinition(new Regex(@"((r|R)otate|(m|M)ove)( )*(R|r)(1|2)( )*-?(\d+)( )*")));
            lexer.AddDefinition(new TokenDefinition("action", new Regex(@"((r|R)otate|(m|M)ove)")));
            lexer.AddDefinition(new TokenDefinition("robot", new Regex(@"(R|r)(1|2)")));
            lexer.AddDefinition(new TokenDefinition("parameter", new Regex(@"-?(\d+)")));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Robot R1 = new Robot(Robot1, AppWindow);
            Robot R2 = new Robot(Robot2, AppWindow);
            R1.Move(10);
            R1.Move(2);
            R1.Move(-3);
            //R1.Move("R1", 10);
            //R2.Move("R2", 15);
            //R1.Rotate("R1", 90);
            //R1.Move("R1", 2);
            //List<MyCommand> commands = new List<MyCommand>();
            //commands.Add(new MyCommand
            //{
            //    ActionType = ActionType.Move,
            //    RobotName = "R1",
            //    Value = 10
            //});
            //commands.Add(new MyCommand
            //{
            //    ActionType = ActionType.Move,
            //    RobotName = "R2",
            //    Value = 15
            //});
            //commands.Add(new MyCommand
            //{
            //    ActionType = ActionType.Rotate,
            //    RobotName = "R1",
            //    Value = 90
            //});
            //commands.Add(new MyCommand
            //{
            //    ActionType = ActionType.Move,
            //    RobotName = "R1",
            //    Value = 2
            //});
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
            }
            else
            {
                tbErrors.Text = "Compiled success";
                isCompiledSuccess = true;
            }
        }
    }
}
