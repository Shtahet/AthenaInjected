using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Athena.DomainManager;

namespace Athena.QuickInjector
{
    public class DotNetInjector
    {
        #region Enums
        [Flags]
        public enum AllocationType
        {
            Commit = 0x1000,
            Reserve = 0x2000,
            Decommit = 0x4000,
            Release = 0x8000,
            Reset = 0x80000,
            Physical = 0x400000,
            TopDown = 0x100000,
            WriteWatch = 0x200000,
            LargePages = 0x20000000
        }

        [Flags]
        public enum MemoryProtection
        {
            Execute = 0x10,
            ExecuteRead = 0x20,
            ExecuteReadWrite = 0x40,
            ExecuteWriteCopy = 0x80,
            NoAccess = 0x01,
            ReadOnly = 0x02,
            ReadWrite = 0x04,
            WriteCopy = 0x08,
            GuardModifierflag = 0x100,
            NoCacheModifierflag = 0x200,
            WriteCombineModifierflag = 0x400
        }

        [Flags]
        public enum ProcessAccessFlags : uint
        {
            All = 0x001F0FFF,
            Terminate = 0x00000001,
            CreateThread = 0x00000002,
            VMOperation = 0x00000008,
            VMRead = 0x00000010,
            VMWrite = 0x00000020,
            DupHandle = 0x00000040,
            SetInformation = 0x00000200,
            QueryInformation = 0x00000400,
            Synchronize = 0x00100000
        }

        [Flags]
        public enum FreeType
        {
            Decommit = 0x4000,
            Release = 0x8000,
        }

        [Flags]
        public enum ThreadFlags
        {
            /// <summary>
            /// The thread will execute immediately.
            /// </summary>
            THREAD_EXECUTE_IMMEDIATELY = 0,
            /// <summary>
            /// The thread will be created in a suspended state.  Use <see cref="Imports.ResumeThread"/> to resume the thread.
            /// </summary>
            CREATE_SUSPENDED = 0x04,
            /// <summary>
            /// The dwStackSize parameter specifies the initial reserve size of the stack. If this flag is not specified, dwStackSize specifies the commit size.
            /// </summary>
            STACK_SIZE_PARAM_IS_A_RESERVATION = 0x00010000,
            /// <summary>
            /// The thread is still active.
            /// </summary>
            STILL_ACTIVE = 259,
        }

        [Flags]
        public enum ThreadWaitValues : uint
        {
            /// <summary>
            /// The object is in a signaled state.
            /// </summary>
            WAIT_OBJECT_0 = 0x00000000,
            /// <summary>
            /// The specified object is a mutex object that was not released by the thread that owned the mutex object before the owning thread terminated. Ownership of the mutex object is granted to the calling thread, and the mutex is set to nonsignaled.
            /// </summary>
            WAIT_ABANDONED = 0x00000080,
            /// <summary>
            /// The time-out interval elapsed, and the object's state is nonsignaled.
            /// </summary>
            WAIT_TIMEOUT = 0x00000102,
            /// <summary>
            /// The wait has failed.
            /// </summary>
            WAIT_FAILED = 0xFFFFFFFF,
            /// <summary>
            /// Wait an infinite amount of time for the object to become signaled.
            /// </summary>
            INFINITE = 0xFFFFFFFF,
        }

        #endregion

        #region DLLImport

        [DllImport("kernel32.dll")]
        static extern IntPtr VirtualAllocEx(IntPtr hProcess,
                                                    IntPtr lpAddress,
                                                    int dwSize,
                                                    AllocationType flAllocationType,
                                                    MemoryProtection flProtect);

        [DllImport("kernel32.dll")]
        static extern IntPtr OpenProcess(ProcessAccessFlags DesiredAccess,
                                                    bool bInheritHandle,
                                                    int dwProcessId);

        [DllImport("kernel32", CharSet = CharSet.Ansi)]
        static extern IntPtr GetProcAddress(IntPtr hModule,
                                                    string procedureName);


        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool WriteProcessMemory(IntPtr hProcess,
                                                    IntPtr lpBaseAddress,
                                                    IntPtr lpBuffer,
                                                    int nSize,
                                                    out int lpNumberOfBytesWritten);

        [DllImport("kernel32.dll")]
        static extern IntPtr CreateRemoteThread(IntPtr hProcess,
                                                    IntPtr lpThreadAttributes,
                                                    uint dwStackSize,
                                                    IntPtr lpStartAddress,
                                                    IntPtr lpParameter,
                                                    ThreadFlags dwCreationFlags,
                                                    out IntPtr lpThreadId);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern UInt32 WaitForSingleObject(IntPtr hHandle,
                                                    UInt32 dwMilliseconds);

        [DllImport("kernel32.dll")]
        static extern bool GetExitCodeThread(IntPtr hThread,
                                                out IntPtr lpExitCode);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool CloseHandle(IntPtr hObject);

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        static extern bool VirtualFreeEx(IntPtr hProcess,
                                                IntPtr lpAddress,
                                                int dwSize,
                                                FreeType dwFreeType);

        #endregion

        #region Memory Write
        private static bool WriteUnicodeString(IntPtr hProcess, IntPtr dwAddress, string Value)
        {
            IntPtr lpBuffer = IntPtr.Zero;
            int iBytesWritten = 0;
            int nSize = 0;

            try
            {
                nSize = Value.Length * 2;
                lpBuffer = Marshal.StringToHGlobalUni(Value);

                WriteProcessMemory(hProcess, dwAddress, lpBuffer, nSize, out iBytesWritten);

                if (nSize != iBytesWritten)
                    throw new Exception("WriteUnicodeString failed!  Number of bytes actually written differed from request.");
            }
            catch
            {
                return false;
            }
            finally
            {
                if (lpBuffer != IntPtr.Zero)
                    Marshal.FreeHGlobal(lpBuffer);
            }
            return true;
        }

        private static bool WriteAsciiString(IntPtr hProcess, IntPtr dwAddress, string Value)
        {
            IntPtr lpBuffer = IntPtr.Zero;
            int iBytesWritten = 0;
            int nSize = 0;

            try
            {
                nSize = Value.Length * 1;
                lpBuffer = Marshal.StringToHGlobalAnsi(Value);

                WriteProcessMemory(hProcess, dwAddress, lpBuffer, nSize, out iBytesWritten);

                if (nSize != iBytesWritten)
                    throw new Exception("WriteUnicodeString failed!  Number of bytes actually written differed from request.");
            }
            catch
            {
                return false;
            }
            finally
            {
                if (lpBuffer != IntPtr.Zero)
                    Marshal.FreeHGlobal(lpBuffer);
            }

            return true;
        }

        private static bool WriteBytes(IntPtr hProcess, IntPtr dwAddress, byte[] lpBytes)
        {
            IntPtr lpBuffer = IntPtr.Zero;
            int iBytesWritten = 0;

            try
            {
                lpBuffer = Marshal.AllocHGlobal(Marshal.SizeOf(lpBytes[0]) * lpBytes.Length); //allocate unmanaged memory

                Marshal.Copy(lpBytes, 0, lpBuffer, lpBytes.Length);

                WriteProcessMemory(hProcess, dwAddress, lpBuffer, lpBytes.Length, out iBytesWritten);

                if (lpBytes.Length != iBytesWritten)
                    throw new Exception("WriteBytes failed!  Number of bytes actually written differed from request.");
            }
            catch
            {
                return false;
            }
            finally
            {
                if (lpBuffer != IntPtr.Zero)
                    Marshal.FreeHGlobal(lpBuffer);
            }

            return true;
        }

        #endregion

        #region InjectDLL

        private static IntPtr InjectDllCreateThread(IntPtr hProcess, string szDllPath)
        {
            if (hProcess == IntPtr.Zero)
                throw new ArgumentNullException("hProcess");

            if (szDllPath.Length == 0)
                throw new ArgumentNullException("szDllPath");

            if (!szDllPath.Contains("\\"))
                szDllPath = System.IO.Path.GetFullPath(szDllPath);

            if (!System.IO.File.Exists(szDllPath))
                throw new ArgumentException("DLL not found.", "szDllPath");

            IntPtr dwBaseAddress = IntPtr.Zero;
            IntPtr lpLoadLibrary;
            IntPtr lpDll;
            IntPtr hThread;

            lpLoadLibrary = GetProcAddress(GetModuleHandle("kernel32.dll"), "LoadLibraryA");
            if (lpLoadLibrary != IntPtr.Zero)
            {
                lpDll = VirtualAllocEx(hProcess, IntPtr.Zero, 1000, AllocationType.Commit, MemoryProtection.ExecuteReadWrite);

                if (lpDll != IntPtr.Zero)
                {
                    if (WriteAsciiString(hProcess, lpDll, szDllPath))
                    {
                        IntPtr dwThreadId;
                        hThread = CreateRemoteThread(hProcess, IntPtr.Zero, 0, lpLoadLibrary, lpDll, ThreadFlags.THREAD_EXECUTE_IMMEDIATELY, out dwThreadId);

                        //wait for thread handle to have signaled state
                        //exit code will be equal to the base address of the dll
                        if (WaitForSingleObject(hThread, 5000) == (uint)ThreadWaitValues.WAIT_OBJECT_0)
                            GetExitCodeThread(hThread, out dwBaseAddress);

                        CloseHandle(hThread);
                    }

                    VirtualFreeEx(hProcess, lpDll, 0, FreeType.Release);
                }
            }

            return dwBaseAddress;
        }

        #endregion

        IntPtr MemoryAlloc = IntPtr.Zero;
        Process Process;
        public bool Injected { get; private set; }

        public DotNetInjector(Process process)
        {
            Process = process;
            Injected = false;
        }

        /// <summary>
        /// Get mscoree.dll Module
        /// </summary>
        private static System.Diagnostics.ProcessModule GetMsCoree
        {
            get
            {
                foreach (System.Diagnostics.ProcessModule pm in System.Diagnostics.Process.GetCurrentProcess().Modules)
                    if (pm.ModuleName.ToLower() == "mscoree.dll")
                        return pm;

                return null;
            }
        }

        /// <summary>
        /// Create a CLR in the target process that will load the assembly
        /// </summary>
        /// <param name="pId">Process Id to inject</param>
        /// <param name="Assembly">Type of the EntryPoint</param>
        /// <param name="Argument">Argement to pass to the Assembly</param>
        public bool InjectAndWait(Type Assembly, string Argument = "")
        {
            if (Process.HasExited)
                throw new Exception("Process no longer exist!");

            IntPtr handle = OpenProcess(ProcessAccessFlags.All, false, Process.Id);

            if (handle == IntPtr.Zero)
                throw new Exception("Error opening process!");

            //MethodInfo startMethod = Assembly.GetMethods(BindingFlags.Static | BindingFlags.Public).First(
            //   m => m.GetCustomAttributes(false).OfType<Dysis.JC2.EntryPoint>().Any());   

            MethodInfo startMethod = Assembly.GetMethods(BindingFlags.Static | BindingFlags.Public).First(
               m => m.GetCustomAttributes(false).OfType<EntryPoint>().Any());

            if (startMethod == null)
            {
                throw new EntryPointNotFoundException("Make sure the [EntryPoint] attribute is set on a static public method in the class");
            }

            var AssemblyPath = (new Uri(Assembly.Module.Assembly.CodeBase)).AbsolutePath;
            var TypeName = startMethod.DeclaringType.FullName;
            var MethodName = startMethod.Name;

            string CLRVersion = "v4.0.30319";

            Guid CLSID_CLRMetaHostGuid = new Guid(0x9280188D, 0xE8E, 0x4867, 0xb3, 0x0C, 0x7F, 0xA8, 0x38, 0x84, 0xE8, 0xDE);
            Guid IID_ICLRMetaHostGuid = new Guid(0xD332DB9E, 0xB9B3, 0x4125, 0x82, 0x07, 0xA1, 0x48, 0x84, 0xF5, 0x32, 0x16);
            Guid IID_ICLRRuntimeInfoGuid = new Guid(0xBD39D1D2, 0xBA2F, 0x486a, 0x89, 0xB0, 0xB4, 0xB0, 0xCB, 0x46, 0x68, 0x91);
            Guid CLSID_CLRRuntimeHostGuid = new Guid(0x90F1A06E, 0x7712, 0x4762, 0x86, 0xB5, 0x7A, 0x5E, 0xBA, 0x6B, 0xDB, 0x02);
            Guid IID_ICLRRuntimeHostGuid = new Guid(0x90F1A06C, 0x7712, 0x4762, 0x86, 0xB5, 0x7A, 0x5E, 0xBA, 0x6B, 0xDB, 0x02);

            IntPtr CLRCreateInstancePtr = GetProcAddress(GetModuleHandle("mscoree.dll"), "CLRCreateInstance");

            if (CLRCreateInstancePtr == IntPtr.Zero)
            {
                throw new Exception("Could not find mscoree.dll, make sure you have .NET installed");
            }

            IntPtr result = InjectDllCreateThread(handle, GetMsCoree.FileName);

            if (handle == null)
            {
                throw new Exception("Could not open the process");
            }

            //Allocate 1 block of memory in target process
            MemoryAlloc = VirtualAllocEx(handle, IntPtr.Zero, 1000, AllocationType.Commit, MemoryProtection.ExecuteReadWrite);

            IntPtr VersionStringPtr = MemoryAlloc;
            IntPtr AssemblyPathPtr = VersionStringPtr + CLRVersion.Length * 2 + 2; //Unicode
            IntPtr TypeNamePtr = AssemblyPathPtr + AssemblyPath.Length * 2 + 2; //Unicode
            IntPtr MethodNamePtr = TypeNamePtr + TypeName.Length * 2 + 2; //Unicode
            IntPtr ArgumentPtr = MethodNamePtr + MethodName.Length * 2 + 2; //Unicode

            IntPtr CLSID_CLRMetaHostPtr = ArgumentPtr + Argument.Length * 2 + 2;  //Unicode
            IntPtr IID_ICLRMetaHostPtr = CLSID_CLRMetaHostPtr + Marshal.SizeOf(CLSID_CLRMetaHostGuid) + 1;
            IntPtr IID_ICLRRuntimeInfoPtr = IID_ICLRMetaHostPtr + Marshal.SizeOf(IID_ICLRMetaHostGuid) + 1;
            IntPtr CLSID_CLRRuntimeHostPtr = IID_ICLRRuntimeInfoPtr + Marshal.SizeOf(IID_ICLRRuntimeInfoGuid) + 1;
            IntPtr IID_ICLRRuntimeHostPtr = CLSID_CLRRuntimeHostPtr + Marshal.SizeOf(CLSID_CLRRuntimeHostGuid) + 1;

            IntPtr codeCave_Code = IID_ICLRRuntimeHostPtr + Marshal.SizeOf(IID_ICLRRuntimeHostGuid) + 1;

            //Copy values in target process
            WriteUnicodeString(handle, VersionStringPtr, CLRVersion);
            WriteUnicodeString(handle, AssemblyPathPtr, AssemblyPath);
            WriteUnicodeString(handle, TypeNamePtr, TypeName);
            WriteUnicodeString(handle, MethodNamePtr, MethodName);
            WriteUnicodeString(handle, ArgumentPtr, Argument);

            WriteBytes(handle, CLSID_CLRMetaHostPtr, CLSID_CLRMetaHostGuid.ToByteArray());
            WriteBytes(handle, IID_ICLRMetaHostPtr, IID_ICLRMetaHostGuid.ToByteArray());
            WriteBytes(handle, IID_ICLRRuntimeInfoPtr, IID_ICLRRuntimeInfoGuid.ToByteArray());
            WriteBytes(handle, CLSID_CLRRuntimeHostPtr, CLSID_CLRRuntimeHostGuid.ToByteArray());
            WriteBytes(handle, IID_ICLRRuntimeHostPtr, IID_ICLRRuntimeHostGuid.ToByteArray());

            Fasm.ManagedFasm fasm = new Fasm.ManagedFasm(handle);

            //Local variables
            //dwRet ebp-0x2c
            //Host ebp-0x20
            //Info ebp-0x14
            //MetaHost ebp-0x8

            fasm.AddLine("push ebp");
            fasm.AddLine("mov ebp, esp");
            fasm.AddLine("sub esp, 0x38");

            //hr = CLRCreateInstance(CLSID_CLRMetaHost, IID_ICLRMetaHost,(PVOID*)&MetaHost);
            fasm.AddLine("lea eax, [ebp-0x8]"); //MetaHost
            fasm.AddLine("push eax");
            fasm.AddLine("push " + IID_ICLRMetaHostPtr);
            fasm.AddLine("push " + CLSID_CLRMetaHostPtr);
            fasm.AddLine("call " + CLRCreateInstancePtr);

            //hr = MetaHost->GetRuntime(TEXT("v4.0.30319"), IID_ICLRRuntimeInfo, (PVOID*)&Info);
            fasm.AddLine("lea eax, [ebp-0x14]"); //Info
            fasm.AddLine("push eax");
            fasm.AddLine("push " + IID_ICLRRuntimeInfoPtr);
            fasm.AddLine("push " + VersionStringPtr);
            fasm.AddLine("mov eax, [ebp-0x8]"); //MetaHost
            fasm.AddLine("push eax"); // This push             
            fasm.AddLine("mov eax, [eax]"); //Pointer to Instance             
            fasm.AddLine("mov eax, [eax+0xC]"); // GetRuntime vTable 
            fasm.AddLine("call eax"); //GetRuntime

            //hr = Info->GetInterface(CLSID_CLRRuntimeHost, IID_ICLRRuntimeHost, (PVOID*)&Host);
            fasm.AddLine("lea eax, [ebp-0x20]"); //Host
            fasm.AddLine("push eax");
            fasm.AddLine("push " + IID_ICLRRuntimeHostPtr);
            fasm.AddLine("push " + CLSID_CLRRuntimeHostPtr);
            fasm.AddLine("mov eax, [ebp-0x14]"); //Info            
            fasm.AddLine("push eax"); // This push 
            fasm.AddLine("mov eax, [eax]"); //Pointer to Instance 
            fasm.AddLine("mov eax, [eax+0x24]"); // GetInterface vTable 
            fasm.AddLine("call eax"); //GetInterface

            //hr = Host->Start();
            fasm.AddLine("mov eax, [ebp-0x20]"); //Host
            fasm.AddLine("push eax");
            fasm.AddLine("mov eax, [eax]"); //Pointer to Instance 
            fasm.AddLine("mov eax, [eax+0xC]"); // Start vTable 
            fasm.AddLine("call eax"); //Start

            //hr = Host->ExecuteInDefaultAppDomain(
            //AssemblyPath, TypeName, MethodName, Argument, &dwRet);
            fasm.AddLine("lea eax, [ebp-0x2C]"); //dwRet
            fasm.AddLine("push eax");
            fasm.AddLine("push " + ArgumentPtr);
            fasm.AddLine("push " + MethodNamePtr);
            fasm.AddLine("push " + TypeNamePtr);
            fasm.AddLine("push " + AssemblyPathPtr);
            fasm.AddLine("mov eax, [ebp-0x20]"); //Host
            fasm.AddLine("push eax"); // This push 
            fasm.AddLine("mov eax, [eax]"); //Pointer to Instance             
            fasm.AddLine("mov eax, [eax+0x2C]"); // ExecuteInDefaultAppDomain vTable 
            fasm.AddLine("call eax");

            //hr = Host->Stop();
            fasm.AddLine("mov eax, [ebp-0x20]"); //Host            
            fasm.AddLine("push eax"); // This push 
            fasm.AddLine("mov eax, [eax]"); //Pointer to Instance             
            fasm.AddLine("mov eax, [eax+0x10]"); // Stop vTable                        
            fasm.AddLine("call eax"); //Start

            //Host->Release();   This crash Fasm for some reason
            /*fasm.AddLine("mov eax, [ebp-0x20]"); //Host
            fasm.AddLine("push eax"); // This push 
            fasm.AddLine("mov eax, [eax]"); //Pointer to Instance 
            fasm.AddLine("mov eax, [eax+0x8]"); // Release vTable 
            fasm.AddLine("call eax"); //Release*/

            fasm.AddLine("add esp, 0x38");
            fasm.AddLine("mov esp, ebp");
            fasm.AddLine("pop ebp");

            fasm.AddLine("retn");

            //Inject ASM
            bool injected = fasm.Inject((uint)codeCave_Code);

            IntPtr lpThreadId;

            //Execute the codecave
            IntPtr hThread = CreateRemoteThread(handle, IntPtr.Zero, 0, codeCave_Code, IntPtr.Zero, ThreadFlags.THREAD_EXECUTE_IMMEDIATELY, out lpThreadId);
            if (hThread == IntPtr.Zero)
                throw new Win32Exception(Marshal.GetLastWin32Error());

            Injected = true;

            //Wait till ExecuteInDefaultAppDomain end.
            if (WaitForSingleObject(hThread, (uint)ThreadWaitValues.INFINITE) != (uint)ThreadWaitValues.WAIT_OBJECT_0)
                throw new Win32Exception(Marshal.GetLastWin32Error());

            Injected = false;

            IntPtr lpExitCode;
            if (!GetExitCodeThread(hThread, out lpExitCode))
                throw new Win32Exception(Marshal.GetLastWin32Error());

            //Clean up
            VirtualFreeEx(handle, MemoryAlloc, 0, FreeType.Release);
            CloseHandle(handle);

            return true;
        }



        public void InjectAndForget(Type Assembly, string Argument = "")
        {
            ThreadPool.QueueUserWorkItem(callback => InjectAndWait(Assembly, Argument));
        }
    }
}
