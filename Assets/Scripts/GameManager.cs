using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.PostProcessing;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public static int Invert = 1;
    public static UnityEvent ResetEvent = new UnityEvent();
    public bool EnableControl;
    public GameObject my_audio_manager;
    private bool pauseToggle = false;
    public GameObject score_panel;

    public int RoundNumber = 0;
    public GameObject HSManager;

    [SerializeField]
    private GameObject tutorial;

    [SerializeField]
    private AnimationCurve lensDistortionCurve;

    [SerializeField]
    private PostProcessVolume postProcessVolume;
    [SerializeField]
    private RoundAnnouncer announcer;
    [SerializeField]
    private GameObject restartTip;

    float holdStartTime;
    float holdStartTimeQ;
    float exitTime;
    int failsCounter;


    private void Awake()
    {
        Instance = this;
        EnableControl = true;
        my_audio_manager.GetComponent<MyAudioManager>().PlayMusicOnLevel(0);
    }

    

    private void Start()
    {
        EnableControl = false;
        LensDistortion lensDistortion = null;
        postProcessVolume.profile.TryGetSettings(out lensDistortion);
        lensDistortion.intensity.value = 0f;
    }


    private void Play()
    {
        EnableControl = true;
        ResetEvent.Invoke();
        announcer.NewLevel(0);
    }

    private void Update()
    {
        if (tutorial.activeInHierarchy && Input.GetKeyDown(KeyCode.Return))
        {
            tutorial.SetActive(false);
            Play();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            holdStartTime = Time.time;
        }

        if (Input.GetKey(KeyCode.Space))
        {
            if (Time.time - holdStartTime > 2)
                SceneManager.LoadScene(1);
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            score_panel.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            score_panel.SetActive(false);
            Time.timeScale = 1;
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            holdStartTimeQ = Time.time;
        }

        if (Input.GetKey(KeyCode.Q))
        {
            if (Time.time - holdStartTimeQ > 2)
                SceneManager.LoadScene(0);
        }

        Highscores.AddNewHighscore(PlayerPrefs.GetString("username"), RoundNumber);
        HSManager.GetComponent<DisplayHighscores>().currentScore.text = "current: " + RoundNumber;
    }

    public void FinishRound(bool win)
    {
        CommandManager.Instance.TerminateExecution();
        if (win)
        {
            CommandManager.Instance.SaveRun();
            //Invert *= -1;
            RoundNumber++;
            my_audio_manager.GetComponent<MyAudioManager>().PlayMusicOnLevel(RoundNumber);
            AudioManager.Instance.PlaySound("Warp");
            failsCounter = 0;
            restartTip.SetActive(false);
        }
        else
        {
            failsCounter++;
            if (failsCounter > 5)
                restartTip.SetActive(true);
            CommandManager.Instance.ClearCurrentRun();
        }
        StartCoroutine(FinishRoutine(win));
        
    }

    IEnumerator FinishRoutine(bool win)
    {
        EnableControl = false;
        BulletSpawner.Instance.DestroyAllBullets();

        yield return null;
        ResetGame();
        if (win)
        {
            LensDistortion lensDistortion = null;
            postProcessVolume.profile.TryGetSettings(out lensDistortion);
            lensDistortion.intensity.value = 0f;

            float t = 0;
            while (t < 1)
            {
                t += Time.unscaledDeltaTime * 1.2f;
                lensDistortion.intensity.value = lensDistortionCurve.Evaluate(t) * 100;
                lensDistortion.scale.value = Mathf.Lerp(1, 0.5f, lensDistortionCurve.Evaluate(t));
                yield return null;
            }
            lensDistortion.intensity.value = 0f;
            lensDistortion.scale.value = 1;
            announcer.NewLevel(RoundNumber);
        }
        //
        //yield return new WaitForSecondsRealtime(0.5f);
        
        //Time.timeScale = 1;
        
    }

    private void ResetGame()
    {
        if (ResetEvent != null)
            ResetEvent.Invoke();
        EnableControl = true;
    }

    public class BoolEvent : UnityEvent<bool>
    {
    }
}
