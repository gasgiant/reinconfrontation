using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;


public class InputManager : MonoBehaviour
{
    public string username;
    public TMP_InputField InputField;
    public int limit = 10;

    private void Start()
    {
        EventSystem.current.SetSelectedGameObject(InputField.gameObject);
        InputField.ActivateInputField();
        InputField.characterLimit = limit;
        if (username != null)
            InputField.text = username;
        InputField.onEndEdit.AddListener(StoreName);
    }

    public void StoreName(string username)
    {
        PlayerPrefs.SetString("username", username);
        Debug.Log(username);
        SceneManager.LoadScene(1);
    }
}
