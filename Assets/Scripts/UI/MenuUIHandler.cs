using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class MenuUIHandler : MonoBehaviour
{
    public TMPro.TMP_InputField playerInputName;
    
    // Start is called before the first frame update
    void Start()
    {
        PersistenceManager.instance.Load();
        if (!string.IsNullOrWhiteSpace(PersistenceManager.instance.lastPlayer))
        {
            Debug.Log("lastplayer:" + PersistenceManager.instance.lastPlayer);
            playerInputName.SetTextWithoutNotify(PersistenceManager.instance.lastPlayer);
        }
    }

    public void NewNameInserted(TMPro.TextMeshProUGUI text)
    {
        Debug.Log("Text:" + text.text + ":");

        PersistenceManager.instance.lastPlayer = text.text;
    }

    public void StartNew()
    {
        Debug.Log("StartNew");
        if (!string.IsNullOrWhiteSpace(PersistenceManager.instance.lastPlayer))
        {
            SceneManager.LoadScene(1);
        }
    }

    public void Exit()
    {
        PersistenceManager.instance.Save();
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif

    }

    public void ShowHighScore()
    {
        SceneManager.LoadScene(2);
    }
}
