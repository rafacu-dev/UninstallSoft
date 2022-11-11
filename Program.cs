
using Microsoft.Win32;
using System;
using System.Diagnostics;

internal class UninstallSoft
{
    static List<string> Programs = new List<string>();
    private static void Main()
    {   
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("Welcome to UninstallSoft.From now on they list the softwares installed in his system...");
        Console.ForegroundColor = ConsoleColor.Gray;

        getProgramsFromRegistry();
        
        
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("Introduce what I number of the program that you have the desire to uninstall, "
        + " or press a letter to come out of the program...");
        
        int indexProgram;
        bool indexValid = Int32.TryParse(Console.ReadLine(),out indexProgram);


        if( indexValid && Programs.Count() >= indexProgram + 1){
            string program = Programs[indexProgram];
            uninstallProgram(program);
        }
        
        Console.WriteLine("Thank you for using UninstallSoft");
        Console.Read();
        
    }

    private static void getProgramsFromRegistry()
    {
        const string msiexec = "MsiExec.exe";

        int count = 1;

        RegistryKey regkey = Registry.LocalMachine
            .OpenSubKey("SOFTWARE").OpenSubKey("Microsoft").OpenSubKey("Windows")
            .OpenSubKey("CurrentVersion").OpenSubKey("Uninstall");

        if (regkey != null)
        {

            foreach (string productId in regkey.GetSubKeyNames())
            {
                RegistryKey productkey = regkey.OpenSubKey(productId);
                var displayName = productkey.GetValue("DisplayName");
                var displayVersion = productkey.GetValue("DisplayVersion");
                var publisher = productkey.GetValue("Publisher");
                var uninstallString = productkey.GetValue("UninstallString");

                string displayNameValue = displayName != null ? displayName.ToString() : string.Empty;
                string displayVersionValue = displayVersion != null ? displayVersion.ToString() : string.Empty;
                string publisherValue = publisher != null ? publisher.ToString() : string.Empty;
                string uninstallStringValue = uninstallString != null ? uninstallString.ToString() : string.Empty;


                if (!string.IsNullOrEmpty(displayNameValue) && !string.IsNullOrEmpty(uninstallStringValue))
                {   
                    int quantity = regkey.GetSubKeyNames().Count().ToString().Length;
                    int countLength = count.ToString().Length;
                    string spaces = "";

                    for(int length = countLength; length <= quantity; length++)
                    {
                        spaces += " ";
                    }

                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    Console.Write(count.ToString() + spaces + " - ");

                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine(displayNameValue);

                    Programs.Add(uninstallStringValue);
                    count++;
                }
                
            }
        }
    }

    private static void uninstallProgram(string program)
        {
            const string msiexec = "MsiExec.exe";

            if (program.ToLower().Contains(msiexec.ToLower()))
            {
                ProcessStartInfo info = new ProcessStartInfo(
                    msiexec, program.ToLower().Replace(msiexec.ToLower(), string.Empty));
                Process.Start(info);
            }
            else
            {
                Console.WriteLine("Unable to unistall " + program);
            }

        }


}