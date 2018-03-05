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
        }
        public void Update()
        {
            widthbox = Screen.width;
            widthbutonfileA = Screen.width + 10;
            widthbutonfileB = Screen.width + 90;

        }
        public void OnGUI()
        {
            GUI.Box(new Rect(widthbox, 0, 170, 200), "Loot Spawn Menu");

            if (GUI.Button(new Rect(widthbutonfileA, 30, 80, 20), "LootBox"))
            {
            }
            if (GUI.Button(new Rect(widthbutonfileA, 50, 80, 20), "MedicalBox"))
            {
            }
            if (GUI.Button(new Rect(widthbutonfileA, 70, 80, 20), "AmmoBox"))
            {
            }
            if (GUI.Button(new Rect(widthbutonfileA, 90, 80, 20), "WeaponBox"))
            {
            }

            if (GUI.Button(new Rect(widthbutonfileB, 30, 80, 20), "Start Timer"))
            {
                //StartCoroutine method in server side
            }
            if (GUI.Button(new Rect(widthbutonfileB, 50, 80, 20), "Stop Timer"))
            {
                //StopCoroutine method in server side
            }
            if (GUI.Button(new Rect(widthbutonfileB, 70, 80, 20), "ClearBoxes"))
            {
            }
            if (GUI.Button(new Rect(widthbutonfileB, 90, 80, 20), "SpawnBoxes"))
            {
            }
        }
    }
}
