using System;
using System.Runtime.InteropServices;
using RustBuster2016;
using UnityEngine;

namespace LootSpawnerClient
{
    public class LootSpawnerClient : RustBuster2016.API.RustBusterPlugin
    {
        public LootSpawnerGUI LootGUI;
        public GameObject go;
        public static LootSpawnerClient Instance;
        internal static bool Enabled = false;
        internal static bool Authorized = false;
        
        public override string Name
        {
            get { return "LootSpawnerClient"; }
        }

        public override string Author
        {
            get { return "DreTaX & Salva"; }
        }

        public override Version Version
        {
            get { return new Version("1.0"); }
        }

        public override void DeInitialize()
        {
            if (LootGUI != null) UnityEngine.Object.DestroyImmediate(LootGUI);
            Authorized = false;
            RustBuster2016.API.Hooks.OnRustBusterClientConsole -= OnRustBusterClientConsole;
            Enabled = false;
        }

        public override void Initialize()
        {
            Instance = this;
            if (this.IsConnectedToAServer)
            {
                RustBuster2016.API.Hooks.OnRustBusterClientConsole += OnRustBusterClientConsole;
                string answer = this.SendMessageToServer("IsAdmin-");
                if (answer == "yes")
                {
                    Authorized = true;
                    go = new GameObject();
                    LootGUI = go.AddComponent<LootSpawnerGUI>();
                    UnityEngine.Object.DontDestroyOnLoad(LootGUI);
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
}