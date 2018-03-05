using System;
using System.Runtime.InteropServices;
using RustBuster2016;
using UnityEngine;

namespace LootSpawnerClient
{
    public class LootSpawnerClient : RustBuster2016.API.RustBusterPlugin
    {
        internal bool Enabled = false;
        internal bool Authorized = false;
        
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
                }
            }
        }

        public void KListener_KeyDown()
        {
            /*if (args.Key == Key.NumPad1 && args.SKeyEvent == InterceptKeys.KeyEvent.WM_KEYUP && Enabled) // AmmoLootBox
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
            }*/
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
}