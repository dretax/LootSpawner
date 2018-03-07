using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace LootSpawnerClient
{
    public class LootSpawnerGUI : MonoBehaviour
    {
        public int widthbox;
        public int widthbutonfileA;
        public int widthbutonfileB;

        public void Start()
        {
            widthbox = Screen.width;
            widthbutonfileA = Screen.width + 10;
            widthbutonfileB = Screen.width + 90;
        }
        
        public void OnGUI()
        {
            if (LootSpawnerClient.Enabled)
            {
                GUI.Box(new Rect(widthbox, 0, 170, 200), "Loot Spawn Menu");

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
    }
}
