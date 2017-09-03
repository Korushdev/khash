using KHash.Domain.Environment;
using KHash.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace KHash.CLI.Environment
{
    public enum CommandList
    {
        RUN_FILE,
        RUN_STRING,
        HELP
    }

    public class ConsoleEnvironment : IEnvironment
    {
        List<Command> commands = new List<Command>();
        Command callingCommand;

        public bool Setup( string[] args )
        {
            InitializeCommands();

            bool parsedArgsOk = ParseArgs( args );

            if ( parsedArgsOk == false || 
                (callingCommand != null && callingCommand.Key == CommandList.HELP ))
            {
                ExecuteHelp();
                return false;
            }
            return true;
        }

        private void InitializeCommands()
        {
            commands.Add(
                new Command()
                {
                    Key = CommandList.HELP,
                    ShortCode = "-h",
                    LongCode = "--help",
                    Description = "Lists all the available commands"
                }
            );
            commands.Add(
                new Command()
                {
                    Key = CommandList.RUN_FILE,
                    ShortCode = "-f",
                    LongCode = "--file",
                    Description = "Run a specific file",
                    ParamCount = 1
                }
            );
            commands.Add(
                new Command()
                {
                    Key = CommandList.RUN_STRING,
                    ShortCode = "-s",
                    LongCode = "--string",
                    Description = "Run a string directly",
                    ParamCount = 1,
                }
            );
        }

        public new Types GetType()
        {
            return Types.CONSOLE;
        }

        public string GetExecutableString()
        {
            if( callingCommand != null )
            {
                switch( callingCommand.Key )
                {
                    case CommandList.RUN_FILE:
                        string path = callingCommand.Params.FirstOrDefault();
                        if( File.Exists( path ) == false )
                        {
                            throw new InvalidCommandExecutionException( "File cannot be found at this location: " + path, true );
                        }
                        return File.ReadAllText( path );

                    case CommandList.RUN_STRING:
                        return callingCommand.Params.FirstOrDefault();

                }
            }

            throw new InvalidCommandExecutionException();
        }

        public void HandleOutput( object[] lines )
        {
            foreach( var l in lines )
            {
                Console.WriteLine( l );
            }
        }

        public string GetExecutablePath()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string executableFileName = assembly.ManifestModule.Name;
            string location = assembly.Location.Replace( executableFileName, "" );
            return Path.GetDirectoryName( location );
        }

        public string GetCurrentWorkingDirectory()
        {
            return System.Environment.CurrentDirectory;
        }

        private bool ParseArgs( string[] args )
        {
            int i = 0;
            if( args.Count() > 0 )
            {
                Command c = commands.Where( x => x.ShortCode == args[i] || x.LongCode == args[i] ).FirstOrDefault();
                if( c != null )
                {
                    callingCommand = c;
                    if( callingCommand.ParamCount > 0 )
                    {
                        i++;
                        int upperBounds = callingCommand.ParamCount + i;

                        for( int b = i; b < upperBounds; b++ )
                        {
                            callingCommand.Params.Add( args[b] );
                        }
                    }

                }
                else
                {
                    Console.WriteLine( "Cannot find command: " + args[i] );
                    return false;
                }
            }

            return true;
        }

        public void ExecuteHelp()
        {
            Command helpCommand = commands.Where( x => x.Key == CommandList.HELP ).FirstOrDefault();
            int intialIndentAmount = 5;
            int indentBetweenCodeAmount = 5;
            int indentBetweenDescAmount = 5;
            Console.WriteLine( "Welcome to KHash, developed by Korush Mahdavieh Copywrite 2017" );
            Console.WriteLine( "\n\nOptions: " );
            foreach (Command command in commands)
            {
                Console.WriteLine( String.Format( "{0," + intialIndentAmount + "}{1}{0," + indentBetweenCodeAmount + "}{0," + indentBetweenDescAmount + "}{3}", String.Empty, command.ShortCode, command.LongCode, command.Description ) );
            }

            Console.WriteLine( "\n\nPress ctrl + c to exit! " );
        }
    }

    public class Command
    {
        public CommandList Key { get; set; }
        public string ShortCode { get; set; }
        public string LongCode { get; set; }
        public string Description { get; set; }
        public int ParamCount = 0;

        public List<string> Params = new List<string>();

        public Command()
        {
        }

        public Command( CommandList key, string shortCode, string longCode, string description )
        {
            Key = key;
            ShortCode = shortCode;
            LongCode = longCode;
            Description = description;
        }
    }
}
