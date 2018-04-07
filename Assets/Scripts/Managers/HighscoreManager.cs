using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UI;

namespace Manager
{
    public class HighscoreManager : MonoBehaviour
    {
        public Highscore[] highscoresList;

        private int localHighscore;

        private const string playerPrefsKey = "LocalHighscore";
        private const string uploadedKey = "hasUploadedHighscore";
        private const string privateCode = "VqnbsBo9LEe_iN-ksRCzyQ84P3n4pBLE6rPNBPAsjIpg";
        private const string publicCode = "5ac7c821d6024519e07786bd";
        private const string url = "http://dreamlo.com/lb/";

        public static HighscoreManager instance;

        public void SaveLocalHighscore(int score)
        {
            int currentHighscore = LoadLocalHighscore();

            if (score > currentHighscore)
            {
                print("New highscore of " + score + "! Saving...");
                PlayerPrefs.SetInt(playerPrefsKey, score);

                // Player has got a new highscore, which obvs hasnt been uploaded yet
                PlayerPrefs.SetInt(uploadedKey, 0);
            }
        }


        public int LoadLocalHighscore()
        {
            return PlayerPrefs.GetInt(playerPrefsKey);
        }


        public void AddNewHighscore(string username, int score)
        {
            StartCoroutine(UploadNewHighscore(username, score));
        }

        IEnumerator UploadNewHighscore(string username, int score)
        {
            string id = System.DateTime.Now.ToString("MMddyyyyhhmmss");

            WWW www = new WWW(url + privateCode + "/add/" + WWW.EscapeURL(id+username) + "/" + score);
            yield return www;

            if (string.IsNullOrEmpty(www.error))
            {
                print("Upload successful!");
                PlayerPrefs.SetInt(uploadedKey, 1);
            }
            else
            {
                print("Error uploading: " + www.error);
            }
        }


        public void DownloadHighscores()
        {
            StartCoroutine(DownloadHighscoresFromWebsite());
        }

        IEnumerator DownloadHighscoresFromWebsite()
        {
            WWW www = new WWW(url + publicCode + "/pipe/0/10");
            yield return www;

            if (string.IsNullOrEmpty(www.error))
            {
                FormatHighscores(www.text);
                GameObject.FindObjectOfType<HighscoreDisplayHelper>().OnHighscoresDownloaded(highscoresList);
            }
            else
                print("Error downloading: " + www.error);
        }


        void FormatHighscores(string textStream)
        {
            string[] entries = textStream.Split(new char[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);

            highscoresList = new Highscore[entries.Length];

            for (int i = 0; i < entries.Length; i++)
            {
                string[] entryInfo = entries[i].Split(new char[] { '|' });

                // Get the username from online
                string usr = entryInfo[0];

                // Replace with spaces
                string username = usr.Replace('+', ' ');

                // Get the date from the username. The day will always be 14 in length
                //string date = DateTime.ParseExact(username.Substring(0, 14), "MMddyyyyhhmmss", null).ToString();

                // Dont want the date included in the username, so substring itself to show only the username
                username = username.Substring(14, (username.Length-14));

                int score = int.Parse(entryInfo[1]);

                highscoresList[i] = new Highscore(username, score);

                //print(highscoresList[i].username + ": " + highscoresList[i].score + " Date: "+date);
            }
        }


        void Awake()
        {
            if (instance)
            {
                DestroyImmediate(this.gameObject);
            }
            else
            {
                DontDestroyOnLoad(this.gameObject);
                instance = this;
            }

            //AddNewHighscore("Tom (The Developer)", 31994);
        }
    }


    public struct Highscore
    {
        public string username;
        public int score;

        public Highscore(string username, int score)
        {
            this.username = username;
            this.score = score;
        }
    }
}
