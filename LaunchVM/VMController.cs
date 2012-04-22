using System;
using System.Collections.Generic;
using System.Linq;
using Vestris.VMWareLib;

namespace LaunchVM
{
    public class VMController : IDisposable
    {

        public VMWareVirtualHost Host { get; set; }

        public VMWareVirtualMachine VM { get; set; }

        public string VMXFileName { get; set; }

        public string ResultFile { get; set; }
        public string GuestResultFile { get; set; }

        public bool SuspendWhenDone { get; set; }

        public bool MachineIsRunning { get; set; }

        public int Timeout { get; set; }

        public User User { get; set; }

        public VMController()
        {
            SuspendWhenDone = true;
            MachineIsRunning = false;
            Timeout = 100000;
        }

        private void Open(string fileName)
        {
            VMXFileName = fileName;
            Console.WriteLine("... Opening File: {0}", VMXFileName);
            VM = Host.Open(fileName);
            Console.WriteLine("... Power on VM");
            VM.PowerOn(Timeout);
            // wait for VMWare Tools
            VM.WaitForToolsInGuest(Timeout);

            MachineIsRunning = true;

        }

        public void Open()
        {
            Open(VMXFileName);
        }


        public string Script { get; set; }

        public void RunBashScript(string script, User user = null)
        {
            if (!MachineIsRunning)
                Open();

            if (user != null)
            {
                Console.WriteLine("... Login user: {0}", user.UserName);
                VM.LoginInGuest(user.UserName, user.Password);
            }

            RunBashScript(script);

            if (user != null && user.Logout)
            {
                Console.WriteLine("... Logout: {0}", user.UserName);
                VM.LogoutFromGuest(Timeout);
            }

        }

        public void RunBashScript(string script)
        {
            if (!MachineIsRunning)
                Open();

            Console.WriteLine("... Running Script");
            var process = VM.RunScriptInGuest("/bin/bash", script, Interop.VixCOM.Constants.VIX_RUNPROGRAM_ACTIVATE_WINDOW, 100000);

            
            if (!string.IsNullOrEmpty(ResultFile) && !string.IsNullOrEmpty(GuestResultFile))
            {
                if (VM.FileExistsInGuest(GuestResultFile, Timeout))
                {
                    Console.WriteLine("... Copy protocoll");
                    VM.CopyFileFromGuestToHost(GuestResultFile, ResultFile, Timeout);
                }

            }



        }

        public void UpdateUbuntu()
        {
            UpdateUbuntu(User);
        }

        public void UpdateUbuntu(User user)
        {
            var script = "#!/bin/bash" + "\n" +
                "touch /tmp/my_generated.txt" + "\n" +
                "echo starting update as root >> /tmp/my_generated.txt" + "\n" +
                "aptitude -y update  >>  /tmp/my_generated.txt" + "\n" +
                "aptitude -y full-upgrade >>  /tmp/my_generated.txt" + "\n" +
                "echo done upgrading >> /tmp/my_generated.txt" + "\n";

            GuestResultFile = "/tmp/my_generated.txt";

            RunBashScript(script, user);
        }

        public void TestTasks()
        {



            string script = "#!/bin/bash" + "\n" +
                "rm ~/my_generated.txt" + "\n" +
                "touch ~/my_generated.txt" + "\n" +
                "echo hello i have been genereated > ~/my_generated.txt" + "\n";

            Console.WriteLine("... Execute script1");
            VM.RunScriptInGuest("/bin/bash", script);

            Console.WriteLine("... Logout");
            VM.LogoutFromGuest();


            Console.WriteLine("... Login as root");
            VM.LoginInGuest("root", password);

            script = "#!/bin/bash" + "\n" +
                "echo starting update as root >> /home/fbergmann/my_generated.txt" + "\n" +
                "aptitude -y update  >>  /home/fbergmann/my_generated.txt" + "\n" +
                "aptitude -y full-upgrade >>  /home/fbergmann/my_generated.txt" + "\n" +
                "echo done upgrading >> /home/fbergmann/my_generated.txt" + "\n";


            Console.WriteLine("... Update VM");
            var process = VM.RunScriptInGuest("/bin/bash", script, Interop.VixCOM.Constants.VIX_RUNPROGRAM_ACTIVATE_WINDOW, 100000);

            Console.WriteLine("... Copy protocoll");
            VM.CopyFileFromGuestToHost("/home/fbergmann/my_generated.txt", ResultFile, Timeout);




        }



        public void Dispose()
        {
            if (SuspendWhenDone)
            {
                Console.WriteLine("... Suspend VM");
                VM.Suspend(Timeout);
            }
        }
    }
}
