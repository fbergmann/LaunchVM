using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Vestris.VMWareLib;
using System.Text.RegularExpressions;
namespace LaunchVM
{
    class Program
    {

		static string password = "";


        static void Main(string[] args)
        {
            // declare a virtual host
            using (var host = new VMWareVirtualHost())
            {
                // connect to a local VMWare Workstation virtual host
                host.ConnectToVMWareWorkstation();

                using (var machine = new VMController { Host = host, 
                                                          VMXFileName = @"c:\Users\fbergmann\Documents\Virtual Machines\Ubuntu 64-bit\Ubuntu 64-bit.vmx", 
                                                          ResultFile = @"c:\Users\fbergmann\Desktop\file.txt", 
                                                          User = new User { UserName = "root", Password = password } })
                {
                    machine.UpdateUbuntu();
                }

                using (var machine1 = new VMController { Host = host, 
                                                           VMXFileName = @"C:\Users\Public\Documents\Shared Virtual Machines\Ubuntu\Ubuntu.vmx", 
                                                           ResultFile = @"c:\Users\fbergmann\Desktop\file2.txt", 
                                                           User = new User { UserName = "root", Password = password } })
                {
                    machine1.UpdateUbuntu();
                }


                //var result = ComTest.LaunchMachine(@"c:\Users\fbergmann\Documents\Virtual Machines\Ubuntu 64-bit\Ubuntu 64-bit.vmx");
                host.Disconnect();
                host.Close();
            }

        }
    }
}
