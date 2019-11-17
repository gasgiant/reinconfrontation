using UnityEngine;
using System.Collections;
using UnityEngine.Networking;


public class Highscores : MonoBehaviour
{

    const string privateCode = "g3WBRYAIEEWAZtUR0_S_ngDBQ45aC4y02NAr-BuNiJtg";
    const string publicCode = "5dd0c55db5622f683c8877f9";
    const string webURL = "http://dreamlo.com/lb/";

    public Highscore[] highscoresList;
    static Highscores instance;
    public DisplayHighscores highscoreDisplay;

    void Awake()
    {
        instance = this;
        highscoreDisplay = GetComponent<DisplayHighscores>();

    }

    public static void AddNewHighscore(string username, int score)
    {
        instance.StartCoroutine(instance.UploadNewHighscore(username, score));
    }

    IEnumerator UploadNewHighscore(string username, int score)
    {
        UnityWebRequest request = UnityWebRequest.Get(webURL + privateCode + "/add/" + UnityWebRequest.EscapeURL(username) + "/" + score);
        yield return request.SendWebRequest();
        if (string.IsNullOrEmpty(request.error))
        {
            DownloadHighscores();
        }
        else
        {
            Debug.Log("Error uploading: " + request.error);
        }
    }

    public void DownloadHighscores()
    {
        StartCoroutine(DownloadHighscoresFromDatabase());
    }

    IEnumerator DownloadHighscoresFromDatabase()
    {
        UnityWebRequest request = UnityWebRequest.Get(webURL + publicCode + "/pipe/");
        yield return request.SendWebRequest();
        if (string.IsNullOrEmpty(request.error))
        {
            var txt = request.downloadHandler.text;
            Debug.Log(txt);
            FormatHighscores(txt);
            highscoreDisplay.OnHighscoresDownloaded(highscoresList);
        }
        else
        {
            print("Error Downloading: " + request.error);
        }
    }

    void FormatHighscores(string textStream)
        {
            string[] entries = textStream.Split(new char[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
            highscoresList = new Highscore[entries.Length];

            for (int i = 0; i < entries.Length; i++)
            {
                string[] entryInfo = entries[i].Split(new char[] { '|' });
                string username = entryInfo[0];
                int score = int.Parse(entryInfo[1]);
                highscoresList[i] = new Highscore(username, score);
                print(highscoresList[i].username + ": " + highscoresList[i].score);
            }
        }


    }
    public struct Highscore
    {
        public string username;
        public int score;

        public Highscore(string _username, int _score)
        {
            username = _username;
            score = _score;
        }

    }
