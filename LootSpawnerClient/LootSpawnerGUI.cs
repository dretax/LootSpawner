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

        public void Update()
        {
            PosBoxA = (Screen.width / 2) + 200;
            widthbutonfileA = PosBoxA + 5;
            PosBoxB = PosBoxA + 100;
            widthbutonfileB = PosBoxB + 30;
        }

        public void OnGUI()
        {
            if (LootSpawnerClient.Authorized)
            {
                //mostrar boton de menu
                if (GUI.Button(new Rect(Screen.width - 80, 50, 80, 20), "Loot Menu"))
                {
                    LootSpawnerClient.Enabled = !LootSpawnerClient.Enabled;
                }
            }
            
            if (LootSpawnerClient.Enabled)
            {
                // A
                GUI.Box(new Rect(PosBoxA, 0, 90, 140), "DirectSpawn");
                if (GUI.Button(new Rect(widthbutonfileA, 30, 80, 20), "LootBox"))
                {
                    string msg = LootSpawnerClient.Instance.SendMessageToServer("direct-3");
                    Rust.Notice.Inventory("", msg == "done" ? "Spawned AmmoLootBox!" : "Failed to Spawn AmmoLootBox!");
                }
                if (GUI.Button(new Rect(widthbutonfileA, 50, 80, 20), "MedicalBox"))
                {
                    string msg = LootSpawnerClient.Instance.SendMessageToServer("direct-2");
                    Rust.Notice.Inventory("", msg == "done" ? "Spawned MedicalBox!" : "Failed to Spawn MedicalBox!");
                }
                if (GUI.Button(new Rect(widthbutonfileA, 70, 80, 20), "AmmoBox"))
                {
                    string msg = LootSpawnerClient.Instance.SendMessageToServer("direct-1");
                    Rust.Notice.Inventory("", msg == "done" ? "Spawned AmmoBox!" : "Failed to Spawn AmmoBox!");
                }
                if (GUI.Button(new Rect(widthbutonfileA, 90, 80, 20), "WeaponBox"))
                {
                    string msg = LootSpawnerClient.Instance.SendMessageToServer("direct-4");
                    Rust.Notice.Inventory("", msg == "done" ? "Spawned WeaponBox!" : "Failed to Spawn WeaponBox!");
                }
                if (GUI.Button(new Rect(widthbutonfileA, 110, 80, 20), "Random"))
                {
                    string msg = LootSpawnerClient.Instance.SendMessageToServer("direct-5");
                    Rust.Notice.Inventory("", msg == "done" ? "Spawned RandomBox!" : "Failed to Spawn RandomBox!");
                }
                // B
                GUI.Box(new Rect(PosBoxB, 0, 140, 240), "Loot Spawner Config");
                if (GUI.Button(new Rect(widthbutonfileB, 30, 80, 20), "Start Timer"))
                {
                    string msg = LootSpawnerClient.Instance.SendMessageToServer("starttimer-");
                    Rust.Notice.Inventory("", msg == "yes" ? "Timer Started." : "Failed to Start Timer.");
                }
                if (GUI.Button(new Rect(widthbutonfileB, 50, 80, 20), "Stop Timer"))
                {
                    string msg = LootSpawnerClient.Instance.SendMessageToServer("stoptimer-");
                    Rust.Notice.Inventory("", msg == "yes" ? "Timer Stopped." : "Failed to Stop Timer.");
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
                    Rust.Notice.Inventory("",
                        msg == "yes" ? "Added Spawnpoint for AmmoLootBox!" : "Failed to Add Spawnpoint!");
                }
                if (GUI.Button(new Rect(widthbutonfileB, 150, 80, 20), "MedicalBox"))
                {
                    string msg = LootSpawnerClient.Instance.SendMessageToServer("spawn-2");
                    Rust.Notice.Inventory("",
                        msg == "yes" ? "Added Spawnpoint for MedicalBox!" : "Failed to Add Spawnpoint!");
                }
                if (GUI.Button(new Rect(widthbutonfileB, 170, 80, 20), "AmmoBox"))
                {
                    string msg = LootSpawnerClient.Instance.SendMessageToServer("spawn-1");
                    Rust.Notice.Inventory("",
                        msg == "yes" ? "Added Spawnpoint for AmmoBox!" : "Failed to Add Spawnpoint!");
                }
                if (GUI.Button(new Rect(widthbutonfileB, 190, 80, 20), "WeaponBox"))
                {
                    string msg = LootSpawnerClient.Instance.SendMessageToServer("spawn-4");
                    Rust.Notice.Inventory("",
                        msg == "yes" ? "Added Spawnpoint for WeaponBox!" : "Failed to Add Spawnpoint!");
                }
                if (GUI.Button(new Rect(widthbutonfileB, 210, 80, 20), "Random"))
                {
                    string msg = LootSpawnerClient.Instance.SendMessageToServer("spawn-5");
                    Rust.Notice.Inventory("",
                        msg == "yes" ? "Added Spawnpoint for RandomBox!" : "Failed to Add Spawnpoint!");
                }
            }
        }
    }
}
