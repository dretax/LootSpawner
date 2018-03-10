using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace LootSpawnerClient
{
    public class LootSpawnerGUI : MonoBehaviour
    {
        public int PosBoxA;
        public int widthbutonfileA;
        public int PosBoxB;
        public int widthbutonfileB;
        /*
        public void Start()
        {
            PosBoxA = Screen.width / 2;
            widthbutonfileA = (Screen.width / 2) + 5;
            PosBoxB = (Screen.width / 2) + 100;
            widthbutonfileB = (Screen.width / 2) + 105;
        }*/
        public void Update()
        {
            PosBoxA = (Screen.width / 2) + 200;
            widthbutonfileA = PosBoxA + 5;
            PosBoxB = PosBoxA + 100;
            widthbutonfileB = PosBoxB + 30;
        }
        /*
        public void OnGUI()
        {
            if (LootSpawnerClient.Authorized && LootSpawnerClient.Enabled)
            {
                GUI.Box(new Rect(PosBoxA, 0, 170, 200), "Loot Spawn Menu");

                if (GUI.Button(new Rect(widthbutonfileA, 30, 80, 20), "LootBox"))
                {
                    string msg = LootSpawnerClient.Instance.SendMessageToServer("spawn-3");
                    if (msg == "yes")
                    {
                        Rust.Notice.Inventory("", "Added Spawnpoint for AmmoLootBox!");   
                    }
                    else
                    {
                        Rust.Notice.Inventory("", "Failed to Add Spawnpoint!");
                    }
                }

                if (GUI.Button(new Rect(widthbutonfileA, 50, 80, 20), "MedicalBox"))
                {
                    string msg = LootSpawnerClient.Instance.SendMessageToServer("spawn-2");
                    if (msg == "yes")
                    {
                        Rust.Notice.Inventory("", "Added Spawnpoint for AmmoLootBox!");   
                    }
                    else
                    {
                        Rust.Notice.Inventory("", "Failed to Add Spawnpoint!");
                    }
                }

                if (GUI.Button(new Rect(widthbutonfileA, 70, 80, 20), "AmmoBox"))
                {
                    string msg = LootSpawnerClient.Instance.SendMessageToServer("spawn-1");
                    if (msg == "yes")
                    {
                        Rust.Notice.Inventory("", "Added Spawnpoint for AmmoLootBox!");   
                    }
                    else
                    {
                        Rust.Notice.Inventory("", "Failed to Add Spawnpoint!");
                    }
                }

                if (GUI.Button(new Rect(widthbutonfileA, 90, 80, 20), "WeaponBox"))
                {
                    string msg = LootSpawnerClient.Instance.SendMessageToServer("spawn-4");
                    if (msg == "yes")
                    {
                        Rust.Notice.Inventory("", "Added Spawnpoint for AmmoLootBox!");   
                    }
                    else
                    {
                        Rust.Notice.Inventory("", "Failed to Add Spawnpoint!");
                    }
                }
                
                if (GUI.Button(new Rect(widthbutonfileA, 110, 80, 20), "Random"))
                {
                    string msg = LootSpawnerClient.Instance.SendMessageToServer("spawn-5");
                    if (msg == "yes")
                    {
                        Rust.Notice.Inventory("", "Added Spawnpoint for AmmoLootBox!");   
                    }
                    else
                    {
                        Rust.Notice.Inventory("", "Failed to Add Spawnpoint!");
                    }
                }

                if (GUI.Button(new Rect(widthbutonfileB, 30, 80, 20), "Start Timer"))
                {
                    string msg = LootSpawnerClient.Instance.SendMessageToServer("starttimer-");
                    if (msg == "yes")
                    {
                        Rust.Notice.Inventory("", "Timer Started.");   
                    }
                    else
                    {
                        Rust.Notice.Inventory("", "Failed to Start Timer.");
                    }
                }

                if (GUI.Button(new Rect(widthbutonfileB, 50, 80, 20), "Stop Timer"))
                {
                    string msg = LootSpawnerClient.Instance.SendMessageToServer("stoptimer-");
                    if (msg == "yes")
                    {
                        Rust.Notice.Inventory("", "Timer Stopped.");   
                    }
                    else
                    {
                        Rust.Notice.Inventory("", "Failed to Stop Timer.");
                    }
                }

                if (GUI.Button(new Rect(widthbutonfileB, 70, 80, 20), "ClearBoxes"))
                {
                    LootSpawnerClient.Instance.SendMessageToServer("clearspawn-");
                }

                if (GUI.Button(new Rect(widthbutonfileB, 90, 80, 20), "SpawnBoxes"))
                {
                    LootSpawnerClient.Instance.SendMessageToServer("forcespawn-");
                }
            }
        }
        */
        public void OnGUI()
        {
            if (LootSpawnerClient.Authorized)
            {
                //mostrar boton de menu
                if (GUI.Button(new Rect(Screen.width - 80, 50, 80, 20), "Loot Menu"))
                {
                    if (LootSpawnerClient.Enabled)
                    {
                        LootSpawnerClient.Enabled = false;
                    }
                    else
                    {
                        LootSpawnerClient.Enabled = true;
                    }
                }

            }
            if (LootSpawnerClient.Enabled)
            {
                // A
                GUI.Box(new Rect(PosBoxA, 0, 90, 140), "DirectSpawn");
                if (GUI.Button(new Rect(widthbutonfileA, 30, 80, 20), "LootBox"))
                {
                    string msg = LootSpawnerClient.Instance.SendMessageToServer("direct-3");
                    if (msg == "done")
                    {
                        Rust.Notice.Inventory("", "Spawned AmmoLootBox!");
                    }
                    else
                    {
                        Rust.Notice.Inventory("", "Failed to Spawn AmmoLootBox!");
                    }
                }
                if (GUI.Button(new Rect(widthbutonfileA, 50, 80, 20), "MedicalBox"))
                {
                    string msg = LootSpawnerClient.Instance.SendMessageToServer("direct-2");
                    if (msg == "done")
                    {
                        Rust.Notice.Inventory("", "Spawned MedicalBox!");
                    }
                    else
                    {
                        Rust.Notice.Inventory("", "Failed to Spawn MedicalBox!");
                    }
                }
                if (GUI.Button(new Rect(widthbutonfileA, 70, 80, 20), "AmmoBox"))
                {
                    string msg = LootSpawnerClient.Instance.SendMessageToServer("direct-1");
                    if (msg == "done")
                    {
                        Rust.Notice.Inventory("", "Spawned AmmoBox!");
                    }
                    else
                    {
                        Rust.Notice.Inventory("", "Failed to Spawn AmmoBox!");
                    }
                }
                if (GUI.Button(new Rect(widthbutonfileA, 90, 80, 20), "WeaponBox"))
                {
                    string msg = LootSpawnerClient.Instance.SendMessageToServer("direct-4");
                    if (msg == "done")
                    {
                        Rust.Notice.Inventory("", "Spawned WeaponBox!");
                    }
                    else
                    {
                        Rust.Notice.Inventory("", "Failed to Spawn WeaponBox!");
                    }
                }
                if (GUI.Button(new Rect(widthbutonfileA, 110, 80, 20), "Random"))
                {
                    string msg = LootSpawnerClient.Instance.SendMessageToServer("direct-5");
                    if (msg == "done")
                    {
                        Rust.Notice.Inventory("", "Spawned RandomBox!");
                    }
                    else
                    {
                        Rust.Notice.Inventory("", "Failed to Spawn RandomBox!");
                    }
                }
                // B
                GUI.Box(new Rect(PosBoxB, 0, 140, 240), "Loot Spawner Config");
                if (GUI.Button(new Rect(widthbutonfileB, 30, 80, 20), "Start Timer"))
                {
                    string msg = LootSpawnerClient.Instance.SendMessageToServer("starttimer-");
                    if (msg == "yes")
                    {
                        Rust.Notice.Inventory("", "Timer Started.");
                    }
                    else
                    {
                        Rust.Notice.Inventory("", "Failed to Start Timer.");
                    }
                }
                if (GUI.Button(new Rect(widthbutonfileB, 50, 80, 20), "Stop Timer"))
                {
                    string msg = LootSpawnerClient.Instance.SendMessageToServer("stoptimer-");
                    if (msg == "yes")
                    {
                        Rust.Notice.Inventory("", "Timer Stopped.");
                    }
                    else
                    {
                        Rust.Notice.Inventory("", "Failed to Stop Timer.");
                    }
                }
                if (GUI.Button(new Rect(widthbutonfileB, 70, 80, 20), "ClearBoxes"))
                {
                    LootSpawnerClient.Instance.SendMessageToServer("clearspawn-");
                }
                if (GUI.Button(new Rect(widthbutonfileB, 90, 80, 20), "SpawnBoxes"))
                {
                    LootSpawnerClient.Instance.SendMessageToServer("forcespawn-");
                }

                if (GUI.Button(new Rect(widthbutonfileB, 130, 80, 20), "LootBox"))
                {
                    string msg = LootSpawnerClient.Instance.SendMessageToServer("spawn-3");
                    if (msg == "yes")
                    {
                        Rust.Notice.Inventory("", "Added Spawnpoint for AmmoLootBox!");
                    }
                    else
                    {
                        Rust.Notice.Inventory("", "Failed to Add Spawnpoint!");
                    }
                }
                if (GUI.Button(new Rect(widthbutonfileB, 150, 80, 20), "MedicalBox"))
                {
                    string msg = LootSpawnerClient.Instance.SendMessageToServer("spawn-2");
                    if (msg == "yes")
                    {
                        Rust.Notice.Inventory("", "Added Spawnpoint for MedicalBox!");
                    }
                    else
                    {
                        Rust.Notice.Inventory("", "Failed to Add Spawnpoint!");
                    }
                }
                if (GUI.Button(new Rect(widthbutonfileB, 170, 80, 20), "AmmoBox"))
                {
                    string msg = LootSpawnerClient.Instance.SendMessageToServer("spawn-1");
                    if (msg == "yes")
                    {
                        Rust.Notice.Inventory("", "Added Spawnpoint for AmmoBox!");
                    }
                    else
                    {
                        Rust.Notice.Inventory("", "Failed to Add Spawnpoint!");
                    }
                }
                if (GUI.Button(new Rect(widthbutonfileB, 190, 80, 20), "WeaponBox"))
                {
                    string msg = LootSpawnerClient.Instance.SendMessageToServer("spawn-4");
                    if (msg == "yes")
                    {
                        Rust.Notice.Inventory("", "Added Spawnpoint for WeaponBox!");
                    }
                    else
                    {
                        Rust.Notice.Inventory("", "Failed to Add Spawnpoint!");
                    }
                }
                if (GUI.Button(new Rect(widthbutonfileB, 210, 80, 20), "Random"))
                {
                    string msg = LootSpawnerClient.Instance.SendMessageToServer("spawn-5");
                    if (msg == "yes")
                    {
                        Rust.Notice.Inventory("", "Added Spawnpoint for RandomBox!");
                    }
                    else
                    {
                        Rust.Notice.Inventory("", "Failed to Add Spawnpoint!");
                    }
                }
            }
        }
    }
}
