﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Timers;
using Fougerite;
using Fougerite.Concurrent;
using Fougerite.Permissions;
using Fougerite.PluginLoaders;
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
        public static Dictionary<Vector3, LootType> LootPositions = new Dictionary<Vector3, LootType>();
        public Timer LootTimer;
        public bool RustBusterSupport = false;
        public GameObject go;
        public Loot LootClass;
        public static Random Randomizer;
        public static bool Announce = false;
        public static string AnnounceMSG = "Loot positions are now filled! Go grab them!";
        public static float Distance = 2.5f;
        
        public const string red = "[color #FF0000]";
        public const string yellow = "[color yellow]";
        public const string green = "[color green]";
        public const string orange = "[color #ffa500]";
        
        public override string Name
        {
            get { return "LootSpawner"; }
        }

        public override string Author
        {
            get { return "DreTaX & Salva"; }
        }

        public override string Description
        {
            get { return "LootSpawner"; }
        }

        public override Version Version
        {
            get { return new Version("1.2"); }
        }

        public override void Initialize()
        {
            ReloadConfig();
            Randomizer = new Random();
            foreach (var x in Settings.EnumSection("Positions"))
            {
                try
                {
                    string data = Settings.GetSetting("Positions", x);
                    int loottype = int.Parse(data);
                    Vector3 v = Util.GetUtil().ConvertStringToVector3(x);
                    LootPositions[v] = (LootType) loottype;
                }
                catch (Exception ex)
                {
                    Logger.LogError("[LootSpawner] Failed to read position: " + ex);
                }
            }
            Hooks.OnServerLoaded += OnServerLoaded;
            Hooks.OnModulesLoaded += OnModulesLoaded;
            Hooks.OnCommand += OnCommand;
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
            switch (cmd)
            {
                case "addloot":
                    if (player.Admin || PermissionSystem.GetPermissionSystem().PlayerHasPermission(player, "lootspawner.addloot"))
                    {
                        if (args.Length == 0 || args.Length > 1)
                        {
                            player.Message("Usage: /addloot 1-5");
                            player.Message("AmmoLootBox, MedicalLootBox, BoxLoot, WeaponLootBox, Random");
                            return;
                        }
                        AddSpawnPoint(player, args[0]);
                    }
                    break;
                case "clearloot":
                    if (player.Admin || PermissionSystem.GetPermissionSystem().PlayerHasPermission(player, "lootspawner.clearloot"))
                    {
                        ClearLoot(player);
                    }
                    break;
                case "reloadloot":
                    if (player.Admin || PermissionSystem.GetPermissionSystem().PlayerHasPermission(player, "lootspawner.reloadloot"))
                    {
                        LootPositions.Clear();
                        bool ret = ReloadConfig();
                        if (!ret)
                        {
                            player.Message("Failed to reload the loot, something wrong with the config!");
                            return;
                        }
                        
                        foreach (var x in Settings.EnumSection("Positions"))
                        {
                            try
                            {
                                int loottype = int.Parse(x);
                                string data = Settings.GetSetting("Positions", x);
                                Vector3 v = Util.GetUtil().ConvertStringToVector3(data);
                                LootPositions[v] = (LootType) loottype;
                            }
                            catch (Exception ex)
                            {
                                Logger.LogError("[LootSpawner] Failed to read position: " + ex);
                            }
                        }
                    }
                    break;
                case "forcespawnloot":
                    if (player.Admin || PermissionSystem.GetPermissionSystem().PlayerHasPermission(player, "lootspawner.forcespawnloot"))
                    {
                        SpawnLoots(player);
                    }
                    break;
            }
        }

        public void OnModulesLoaded()
        {
            foreach (BasePlugin x in PluginLoader.GetInstance().Plugins.Values)
            {
                if (!x.Name.ToLower().Contains("rustbuster")) continue;
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
                switch (split[0])
                {
                    case "IsAdmin":
                        msgc.ReturnMessage = (user.Player.Admin || PermissionSystem.GetPermissionSystem().PlayerHasPermission(user.Player, "lootspawner.spawn"))
                            ? "yes" : "no";
                        break;
                    case "direct":
                        DirectSpawn(user.Player, split[1]);
                        msgc.ReturnMessage = "done";
                        break;
                    case "spawn":
                        bool b = AddSpawnPoint(user.Player, split[1]);
                        msgc.ReturnMessage = b ? "yes" : "no";
                        break;
                    case "forcespawn":
                        SpawnLoots(user.Player);
                        msgc.ReturnMessage = "done";
                        break;
                    case "clearspawn":
                        ClearLoot(user.Player);
                        msgc.ReturnMessage = "done";
                        break;
                    case "starttimer":
                        if (!LootClass.LoadupLootEnabled)
                        {
                            LootClass.LoadupLootEnabled = true;
                            msgc.ReturnMessage = "yes";
                        }
                        else
                        {
                            msgc.ReturnMessage = "no";
                        }
                        break;
                    case "stoptimer":
                        if (LootClass.LoadupLootEnabled)
                        {
                            LootClass.LoadupLootEnabled = false;
                            msgc.ReturnMessage = "yes";
                        }
                        else
                        {
                            msgc.ReturnMessage = "no";
                        }
                        break;
                }
            }
        }

        public void OnServerLoaded()
        {
            go = new GameObject();
            LootClass = go.AddComponent<Loot>();
            UnityEngine.Object.DontDestroyOnLoad(go);
        }

        public void ClearLoot(Fougerite.Player player)
        {
            int num = 0;
            foreach (var x in LootPositions.Keys)
            {
                try
                {
                    var obj = Util.GetUtil().FindClosestEntity(x, 1.5f);
                    if (obj != null && obj.Object is LootableObject lootableObject)
                    {
                        Util.GetUtil().DestroyObject(lootableObject.gameObject);
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

        public void SpawnLoots(Fougerite.Player player)
        {
            LootClass.SpawnLootsMono();
        }

        public bool AddSpawnPoint(Fougerite.Player player, string data)
        {
            Vector3 plloc = player.Location;
            int type = 5;
            int.TryParse(data, out type);
            Vector3 findclosestpos = Vector3.zero;
            float currentdist = float.MaxValue;
            foreach (var x in LootPositions.Keys)
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
                LootPositions.Add(plloc, (LootType) type);
                Settings.AddSetting("Positions", plloc.ToString(), type.ToString());
                Settings.Save();
                player.Message("Successfully added spawnpoint for: " + GetPrefab(type));
                return true;
            }
            if (Vector3.zero != findclosestpos) // If we found the closest position.
            {
                if (Vector3.Distance(plloc, findclosestpos) > Distance)
                {
                    LootPositions.Add(plloc, (LootType) type);
                    Settings.AddSetting("Positions", plloc.ToString(), type.ToString());
                    Settings.Save();
                    player.Message("Successfully added spawnpoint for: " + GetPrefab(type));
                    return true;
                }
                player.Message("You need to be " + Distance + "m away atleast from an existing spawnpoint!");
            }
            return false;
        }

        public void DirectSpawn(Fougerite.Player player, string data)
        {
            Vector3 plloc = player.Location;
            int type = 5;
            int.TryParse(data, out type);
            string name = GetPrefab(type);
            World.GetWorld().Spawn(name, new Vector3(plloc.x, plloc.y - 1.6f, plloc.z));
            player.Message("Successfully Spawned: " + name);
        }

        public static string GetPrefab(int num)
        {
            LootType type = (LootType) num;
            switch (type)
            {
                case LootType.BoxLoot:
                    return "BoxLoot";
                case LootType.AmmoLootBox:
                    return "AmmoLootBox";
                case LootType.MedicalLootBox:
                    return "MedicalLootBox";
                case LootType.WeaponLootBox:
                    return "WeaponLootBox";
            }

            int i = Randomizer.Next(1, 5);
            return GetPrefab(i);
        }

        public static string GetPrefab(LootType type)
        {
            switch (type)
            {
                case LootType.BoxLoot:
                    return "BoxLoot";
                case LootType.AmmoLootBox:
                    return "AmmoLootBox";
                case LootType.MedicalLootBox:
                    return "MedicalLootBox";
                case LootType.WeaponLootBox:
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
                Settings.AddSetting("Settings", "Announce", "false");
                Settings.AddSetting("Settings", "Distance", "2.5");
                Settings.Save();
            }
            
            try
            {
                Settings = new IniParser(Path.Combine(ModuleFolder, "Settings.ini"));
                Time = int.Parse(Settings.GetSetting("Settings", "Time"));
                Announce = Settings.GetBoolSetting("Settings", "Announce");
                AnnounceMSG = Settings.GetSetting("Settings", "AnnounceMSG");
                Distance = float.Parse(Settings.GetSetting("Settings", "Distance"));
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
