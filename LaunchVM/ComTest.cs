//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using VixCOM;

//namespace LaunchVM
//{
//    public class ComTest
//    {

//        internal static object LaunchMachine(string vmxFile)
//        {
//            var lib = new VixLibClass();
//            UInt64 err;
//            object results = null;

//            // 
//            IJob job = lib.Connect(Constants.VIX_API_VERSION,
//                                          Constants.VIX_SERVICEPROVIDER_VMWARE_WORKSTATION,
//                                          null,
//                                          0,
//                                          null,
//                                          null,
//                                          0,
//                                          null,
//                                          null);
//            err = job.Wait(new int[] { Constants.VIX_PROPERTY_JOB_RESULT_HANDLE },
//                           ref results);
//            if (lib.ErrorIndicatesFailure(err))
//            {
//                // Handle the error... 
//            }

//            IHost host = (IHost)((object[])results)[0];
//            CloseVixObject(job);

//            job = host.OpenVM(vmxFile, null);

//            err = job.Wait(new int[] { Constants.VIX_PROPERTY_JOB_RESULT_HANDLE },
//                           ref results);
//            if (lib.ErrorIndicatesFailure(err))
//            {
//                // Handle the error... 
//            }

//            IVM2 vm = (IVM2)((object[])results)[0];
//            CloseVixObject(job);

//            job = vm.PowerOn(Constants.VIX_VMPOWEROP_LAUNCH_GUI, null, null);
//            job.WaitWithoutResults();
//            if (lib.ErrorIndicatesFailure(err))
//            {
//                // Handle the error... 
//            }
//            CloseVixObject(job);

//            // Wait up to 300 seconds for tools to start
//            job = vm.WaitForToolsInGuest(300, null);
//            err = job.WaitWithoutResults();
//            if (lib.ErrorIndicatesFailure(err))
//            {
//                // Handle the error... 
//            }
//            CloseVixObject(job);

//            job = vm.LoginInGuest("fbergmann", password, 0, null);
//            err = job.WaitWithoutResults();
//            if (lib.ErrorIndicatesFailure(err))
//            {
//                // Handle the error...
//            }
//            CloseVixObject(job);




//            job = vm.CaptureScreenImage(VixCOM.Constants.VIX_CAPTURESCREENFORMAT_PNG, null, null);

//            //Retrieves a SAFEARRAY of BYTEs that contain the binary screenshot data
//            err = job.Wait(new int[] { VixCOM.Constants.VIX_PROPERTY_JOB_RESULT_SCREEN_IMAGE_DATA },
//                                 ref results);
//            if (lib.ErrorIndicatesFailure(err))
//            {
//                // Handle the error...
//            }

//            byte[] myarray = (byte[])((object[])results)[0];

//            File.WriteAllBytes(@"c:\test.png", myarray);



//            string script = "#!/bin/bash" + "\n" +
//                "touch ~/my_generated.txt" + "\n" +
//                "echo hello i have been genereated > ~/my_generated.txt" + "\n";

//            // Fill perlScript with desired perl script text

//            job = vm.RunScriptInGuest("/bin/bash", script, 0, null, null);
//            err = job.WaitWithoutResults();
//            if (lib.ErrorIndicatesFailure(err))
//            {
//                // Handle the error...
//            }
//            CloseVixObject(job);


//            // copy file 

//            job = vm.CopyFileFromGuestToHost("/home/fbergmann/my_generated.txt", @"c:\Users\fbergmann\Desktop\file.txt", 0, null, null);
//            err = job.WaitWithoutResults();
//            if (lib.ErrorIndicatesFailure(err))
//            {
//                // Handle the error...
//            }
//            CloseVixObject(job);


//            CloseVixObject(vm);

//            host.Disconnect();

//            return 0;
//        }


//        public ComTest()
//        {

//        }

//        static void CloseVixObject(Object vixObject)
//        {
//            try
//            {
//                ((IVixHandle2)vixObject).Close();
//            }
//            catch (Exception)
//            {
//                //Close is not supported in this version of Vix COM - Ignore
//            }
//        }

//    }
//}
