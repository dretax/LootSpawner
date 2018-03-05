using System;
using System.Runtime.InteropServices;
using System.Windows.Input;
using RustBuster2016;
using UnityEngine;

namespace LootSpawnerClient
{
    public class LootSpawnerClient : RustBuster2016.API.RustBusterPlugin
    {
        internal bool Enabled = false;
        internal bool Authorized = false;
        private KeyboardListener kbl;
        
        public override string Name
        {
            get { return "LootSpawnerClient"; }
        }

        public override string Author
        {
            get { return "DreTaX"; }
        }

        public override Version Version
        {
            get { return new Version("1.0"); }
        }

        public override void DeInitialize()
        {
            Authorized = false;
            RustBuster2016.API.Hooks.OnRustBusterClientConsole -= OnRustBusterClientConsole;
            kbl.KeyDown -= new RawKeyEventHandler(KListener_KeyDown);
            Enabled = false;
        }

        public override void Initialize()
        {
            if (this.IsConnectedToAServer)
            {
                RustBuster2016.API.Hooks.OnRustBusterClientConsole += OnRustBusterClientConsole;
                string answer = this.SendMessageToServer("IsAdmin-");
                if (answer == "yes")
                {
                    Authorized = true;
                    kbl = new KeyboardListener();
                    kbl.KeyDown += new RawKeyEventHandler(KListener_KeyDown);
                }
            }
        }

        public void KListener_KeyDown(object sender, RawKeyEventArgs args)
        {
            if (args.Key == Key.NumPad1 && args.SKeyEvent == InterceptKeys.KeyEvent.WM_KEYUP && Enabled) // AmmoLootBox
            {
                string msg = this.SendMessageToServer("spawn-1");
                if (msg == "yes")
                {
                    Rust.Notice.Inventory("", "Added Spawnpoint for AmmoLootBox!");   
                }
                else
                {
                    Rust.Notice.Inventory("", "Failed to Add Spawnpoint!");
                }
            }
            else if (args.Key == Key.NumPad2 && args.SKeyEvent == InterceptKeys.KeyEvent.WM_KEYUP && Enabled) // MedicalLootBox
            {
                string msg = this.SendMessageToServer("spawn-2");
                if (msg == "yes")
                {
                    Rust.Notice.Inventory("", "Added Spawnpoint for MedicalLootBox!");   
                }
                else
                {
                    Rust.Notice.Inventory("", "Failed to Add Spawnpoint!");
                }
            }
            else if (args.Key == Key.NumPad3 && args.SKeyEvent == InterceptKeys.KeyEvent.WM_KEYUP && Enabled) // BoxLoot
            {
                string msg = this.SendMessageToServer("spawn-3");
                if (msg == "yes")
                {
                    Rust.Notice.Inventory("", "Added Spawnpoint for BoxLoot!");   
                }
                else
                {
                    Rust.Notice.Inventory("", "Failed to Add Spawnpoint!");
                }
            }
            else if (args.Key == Key.NumPad4 && args.SKeyEvent == InterceptKeys.KeyEvent.WM_KEYUP && Enabled) // WeaponLootBox
            {
                string msg = this.SendMessageToServer("spawn-4");
                if (msg == "yes")
                {
                    Rust.Notice.Inventory("", "Added Spawnpoint for WeaponLootBox!");   
                }
                else
                {
                    Rust.Notice.Inventory("", "Failed to Add Spawnpoint!");
                }
            }
            else if (args.Key == Key.NumPad5 && args.SKeyEvent == InterceptKeys.KeyEvent.WM_KEYUP && Enabled) // Random
            {
                string msg = this.SendMessageToServer("spawn-5");
                if (msg == "yes")
                {
                    Rust.Notice.Inventory("", "Added Spawnpoint for Random!");   
                }
                else
                {
                    Rust.Notice.Inventory("", "Failed to Add Spawnpoint!");
                }
            }
        }

        public void OnRustBusterClientConsole(string msg)
        {
            if (Authorized)
            {
                if (msg == "loot.spawn")
                {
                    Enabled = !Enabled;
                    if (Enabled)
                    {
                        Rust.Notice.Inventory("", "Enabled Lootspawn editor! (num1 - num5)");
                    }
                    else
                    {
                        Rust.Notice.Inventory("", "Disabled Lootspawn editor!");
                    }
                }
            }
        }
    }
    
    
    internal class GlobalKeyboardHookEventArgs : EventArgs
    {
        public GlobalKeyboardHook.KeyboardState KeyboardState { get; private set; }
        public GlobalKeyboardHook.KBDLLHOOKSTRUCT KeyboardData { get; private set; }
        public bool Handled { get; set; }

        public GlobalKeyboardHookEventArgs(GlobalKeyboardHook.KBDLLHOOKSTRUCT keyboardData,
            GlobalKeyboardHook.KeyboardState keyboardState)
        {
            KeyboardData = keyboardData;
            KeyboardState = keyboardState;
            Handled = false;
        }
    }

    [FlagsAttribute]
    public enum EXECUTION_STATE : uint
    {
        ES_AWAYMODE_REQUIRED = 0x00000040,
        ES_CONTINUOUS = 0x80000000,
        ES_DISPLAY_REQUIRED = 0x00000002,
        ES_SYSTEM_REQUIRED = 0x00000001
        // Legacy flag, should not be used.
        // ES_USER_PRESENT = 0x00000004
    }

    internal class GlobalKeyboardHook : IDisposable
    {
        public event EventHandler<GlobalKeyboardHookEventArgs> KeyboardPressed;

        private readonly IntPtr _windowsHook;

        public GlobalKeyboardHook()
        {
            IntPtr hInstance = LoadLibrary("User32");
            _windowsHook = SetWindowsHookEx(WH_KEYBOARD_LL, LowLevelKeyboardProc, hInstance, 0);
        }

        public void Dispose()
        {
            UnhookWindowsHookEx(_windowsHook);
        }

        delegate IntPtr HookProc(int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll")]
        static extern IntPtr LoadLibrary(string lpFileName);

        /// <summary>
        /// The SetWindowsHookEx function installs an application-defined hook procedure into a hook chain.
        /// You would install a hook procedure to monitor the system for certain types of events. These events are
        /// associated either with a specific thread or with all threads in the same desktop as the calling thread.
        /// </summary>
        /// <param name="idHook">hook type</param>
        /// <param name="lpfn">hook procedure</param>
        /// <param name="hMod">handle to application instance</param>
        /// <param name="dwThreadId">thread identifier</param>
        /// <returns>If the function succeeds, the return value is the handle to the hook procedure.</returns>
        [DllImport("USER32", SetLastError = true)]
        static extern IntPtr SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hMod, int dwThreadId);

        /// <summary>
        /// The UnhookWindowsHookEx function removes a hook procedure installed in a hook chain by the SetWindowsHookEx function.
        /// </summary>
        /// <param name="hhk">handle to hook procedure</param>
        /// <returns>If the function succeeds, the return value is true.</returns>
        [DllImport("USER32", SetLastError = true)]
        public static extern bool UnhookWindowsHookEx(IntPtr hHook);

        /// <summary>
        /// The CallNextHookEx function passes the hook information to the next hook procedure in the current hook chain.
        /// A hook procedure can call this function either before or after processing the hook information.
        /// </summary>
        /// <param name="hHook">handle to current hook</param>
        /// <param name="code">hook code passed to hook procedure</param>
        /// <param name="wParam">value passed to hook procedure</param>
        /// <param name="lParam">value passed to hook procedure</param>
        /// <returns>If the function succeeds, the return value is true.</returns>
        [DllImport("USER32", SetLastError = true)]
        static extern IntPtr CallNextHookEx(IntPtr hHook, int code, IntPtr wParam, IntPtr lParam);

        [StructLayout(LayoutKind.Sequential)]
        public struct KBDLLHOOKSTRUCT
        {
            public int vkCode;
            public int scanCode;
            public int flags;
            public int time;
            public IntPtr dwExtraInfo;
        }

        public const int WH_KEYBOARD_LL = 13;
        //const int HC_ACTION = 0;

        public enum KeyboardState
        {
            KeyDown = 0x0100,
            KeyUp = 0x0101,
            SysKeyDown = 0x0104,
            SysKeyUp = 0x0105
        }

        public const int VkSnapshot = 0x2c;
        //const int VkLwin = 0x5b;
        //const int VkRwin = 0x5c;
        //const int VkTab = 0x09;
        //const int VkEscape = 0x18;
        //const int VkControl = 0x11;
        const int KfAltdown = 0x2000;
        public const int LlkhfAltdown = (KfAltdown >> 8);

        public IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            bool fEatKeyStroke = false;

            var wparamTyped = wParam.ToInt32();
            if (Enum.IsDefined(typeof(KeyboardState), wparamTyped))
            {
                object o = Marshal.PtrToStructure(lParam, typeof(KBDLLHOOKSTRUCT));
                KBDLLHOOKSTRUCT p = (KBDLLHOOKSTRUCT)o;

                var eventArguments = new GlobalKeyboardHookEventArgs(p, (KeyboardState)wparamTyped);

                EventHandler<GlobalKeyboardHookEventArgs> handler = KeyboardPressed;
                handler?.Invoke(this, eventArguments);

                fEatKeyStroke = eventArguments.Handled;
            }

            return fEatKeyStroke ? (IntPtr)1 : CallNextHookEx(IntPtr.Zero, nCode, wParam, lParam);
        }
    }
}