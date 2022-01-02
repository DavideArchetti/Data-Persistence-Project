using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HighScores : MonoBehaviour
{
    public TMPro.TextMeshProUGUI highScoresText;

    private void Start()
    {
        if (PersistenceManager.instance != null)
        {
            StringBuilder sb = new StringBuilder();

            foreach (string key in PersistenceManager.instance.persistence.Keys)
            {
                sb.AppendLine($"{key} : {PersistenceManager.instance.persistence[key]}");
            }

            highScoresText.text = sb.ToString();
        }
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
        }
    }
}
