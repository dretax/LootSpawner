using System;
using System.Collections.Generic;
using System.IO;
using System.Timers;
using Fougerite;
using RustBuster2016Server;
using UnityEngine;
using Random = System.Random;

namespace LootSpawner
{
    public enum LootType
    {
        AmmoLootBox = 1,
        MedicalLootBox = 2,
        BoxLoot = 3,
        WeaponLootBox = 4,
        Random = 5
    }
    
    public class LootSpawner : Fougerite.Module
    {
        public IniParser Settings;
        public static int Time = 20;
        public static Dictionary<LootType, Vector3> LootPositions = new Dictionary<LootType, Vector3>();
        public Timer LootTimer;
        public bool RustBusterSupport = false;
        public GameObject go;
        public Loot LootClass;
        public static Random Randomizer;
        
        public override string Name
        {
            get { return "LootSpawner"; }
        }

        public override string Author
        {
            get { return "DreTaX"; }
        }

        public override string Description
        {
            get { return "LootSpawner"; }
        }

        public override Version Version
        {
            get { return new Version("1.0"); }
        }

        public override void Initialize()
        {
            if (!File.Exists(Path.Combine(ModuleFolder, "Settings.ini")))
            {
                File.Create(Path.Combine(ModuleFolder, "Settings.ini")).Dispose();
                Settings = new IniParser(Path.Combine(ModuleFolder, "Settings.ini"));
                Settings.AddSetting("Settings", "Time", "20");
                Settings.Save();
            }
            else
            {
                Settings = new IniParser(Path.Combine(ModuleFolder, "Settings.ini"));
            }

            foreach (var x in Settings.EnumSection("Positions"))
            {
                try
                {
                    int loottype = int.Parse(x);
                    string data = Settings.GetSetting("Positions", x);
                    Vector3 v = Util.GetUtil().ConvertStringToVector3(data);
                    LootPositions[(LootType) loottype] = v;
                }
                catch (Exception ex)
                {
                    Logger.LogError("[LootSpawner] Failed to read position: " + ex);
                }
            }
            Hooks.OnServerLoaded += OnServerLoaded;
            Hooks.OnModulesLoaded += OnModulesLoaded;
            Hooks.OnCommand += OnCommand;
            ReloadConfig();
            Randomizer = new Random();
        }

        public override void DeInitialize()
        {
            Hooks.OnServerLoaded -= OnServerLoaded;
            Hooks.OnModulesLoaded -= OnModulesLoaded;
            Hooks.OnCommand -= OnCommand;
            if (RustBusterSupport)
            {
                RustBuster2016Server.API.OnRustBusterUserMessage -= OnRustBusterUserMessage;
            }
        }

        public void OnCommand(Fougerite.Player player, string cmd, string[] args)
        {
            if (cmd == "addloot")
            {
                if (player.Admin)
                {
                    if (args.Length == 0 || args.Length > 1)
                    {
                        player.Message("Usage: /addloot 1-5");
                        player.Message("AmmoLootBox, MedicalLootBox, BoxLoot, WeaponLootBox, Random");
                        return;
                    }
                    AddSpawnPoint(player, args[0]);
                }
            }
            else if (cmd == "clearloot")
            {
                if (player.Admin)
                {
                    int num = 0;
                    foreach (var x in LootPositions.Values)
                    {
                        try
                        {
                            var obj = Util.GetUtil().FindClosestEntity(x, 1.5f);
                            if (obj != null && obj.Object is LootableObject)
                            {
                                Util.GetUtil().DestroyObject(((LootableObject) obj.Object).gameObject);
                                num++;
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.LogError("[LootSpawner] Error occured: " + ex);
                        }
                    }
                    player.Message(num + " loots were cleaned!");
                }
            }
            else if (cmd == "reloadloot")
            {
                if (player.Admin)
                {
                    LootPositions.Clear();
                    Settings = new IniParser(Path.Combine(ModuleFolder, "Settings.ini"));
                    ReloadConfig();
                    foreach (var x in Settings.EnumSection("Positions"))
                    {
                        try
                        {
                            int loottype = int.Parse(x);
                            string data = Settings.GetSetting("Positions", x);
                            Vector3 v = Util.GetUtil().ConvertStringToVector3(data);
                            LootPositions[(LootType) loottype] = v;
                        }
                        catch (Exception ex)
                        {
                            Logger.LogError("[LootSpawner] Failed to read position: " + ex);
                        }
                    }
                }
            }
        }

        public void OnModulesLoaded()
        {
            foreach (var x in Fougerite.ModuleManager.Modules)
            {
                if (!x.Plugin.Name.ToLower().Contains("rustbuster")) continue;
                RustBusterSupport = true;
                break;
            }
            if (RustBusterSupport)
            {
                RustBuster2016Server.API.OnRustBusterUserMessage += OnRustBusterUserMessage;
            }
        }

        public void OnRustBusterUserMessage(API.RustBusterUserAPI user, Message msgc)
        {
            if (msgc.PluginSender == "LootSpawnerClient")
            {
                string msg = msgc.MessageByClient;
                string[] split = msg.Split('-');
                if (split[0] == "IsAdmin")
                {
                    msgc.ReturnMessage = user.Player.Admin ? "yes" : "no";
                }
                else if (split[0] == "spawn")
                {
                    bool b = AddSpawnPoint(user.Player, split[1]);
                    msgc.ReturnMessage = b ? "yes" : "no";
                }
            }
        }

        public void OnServerLoaded()
        {
            go = new GameObject();
            LootClass = go.AddComponent<Loot>();
            UnityEngine.Object.DontDestroyOnLoad(go);
        }

        public bool AddSpawnPoint(Fougerite.Player player, string data)
        {
            Vector3 plloc = player.Location;
            float y = World.GetWorld().GetGround(plloc.x, plloc.y);
            plloc.y = y;
            int type = 5;
            int.TryParse(data, out type);
            Vector3 findclosestpos = Vector3.zero;
            float currentdist = float.MaxValue;
            foreach (var x in LootPositions.Values)
            {
                float dist = Vector3.Distance(plloc, x);
                if (dist < currentdist)
                {
                    findclosestpos = x;
                    currentdist = dist;
                }
            }

            if (findclosestpos == Vector3.zero) // If we had no other positions to compare to.
            {
                LootPositions.Add((LootType) type, plloc);
                Settings.AddSetting("Positions", type.ToString(), plloc.ToString());
                Settings.Save();
                player.Message("Successfully added spawnpoint for: " + GetPrefab(type));
                return true;
            }
            if (Vector3.zero != findclosestpos) // If we found the closest position.
            {
                if (Vector3.Distance(plloc, findclosestpos) > 2.5f)
                {
                    LootPositions.Add((LootType) type, plloc);
                    Settings.AddSetting("Positions", type.ToString(), plloc.ToString());
                    Settings.Save();
                    player.Message("Successfully added spawnpoint for: " + GetPrefab(type));
                    return true;
                }
                player.Message("You need to be 2.5m away atleast from an existing spawnpoint!");
            }
            return false;
        }

        public static string GetPrefab(int num)
        {
            LootType type = (LootType) num;
            if (type == LootType.BoxLoot)
            {
                return "BoxLoot";
            }
            if (type == LootType.AmmoLootBox)
            {
                return "AmmoLootBox";
            }
            if (type == LootType.MedicalLootBox)
            {
                return "MedicalLootBox";
            }
            if (type == LootType.WeaponLootBox)
            {
                return "WeaponLootBox";
            }
            int i = Randomizer.Next(1, 5);
            return GetPrefab(i);
        }

        public static string GetPrefab(LootType type)
        {
            if (type == LootType.BoxLoot)
            {
                return "BoxLoot";
            }
            if (type == LootType.AmmoLootBox)
            {
                return "AmmoLootBox";
            }
            if (type == LootType.MedicalLootBox)
            {
                return "MedicalLootBox";
            }
            if (type == LootType.WeaponLootBox)
            {
                return "WeaponLootBox";
            }
            int i = Randomizer.Next(1, 5);
            return GetPrefab(i);
        }

        private bool ReloadConfig()
        {
            if (!File.Exists(Path.Combine(ModuleFolder, "Settings.ini")))
            {
                File.Create(Path.Combine(ModuleFolder, "Settings.ini")).Dispose();
                Settings = new IniParser(Path.Combine(ModuleFolder, "Settings.ini"));
                Settings.AddSetting("Settings", "Time", "20");
                Settings.Save();
            }
            Settings = new IniParser(Path.Combine(ModuleFolder, "Settings.ini"));
            try
            {
                Time = int.Parse(Settings.GetSetting("Settings", "Time"));
            }
            catch (Exception ex)
            {
                Fougerite.Logger.LogError("[LootSpawner] Failed to read config, possible wrong value somewhere! Ex: " + ex);
                return false;
            }
            return true;
        }
    }
}