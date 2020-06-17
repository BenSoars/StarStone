using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class User_Interface : MonoBehaviour
{
    //Kurtis Watson
    public TMPro.TextMeshProUGUI m_currentTimeText;

    private Wave_System r_waveSystem;

    private float m_targetTime;
    private int m_currentSecond;
    private int m_currentMinute;

    public List<int> m_waveTimes = new List<int>();

    // Start is called before the first frame update

    private void Start()
    {
        r_waveSystem = FindObjectOfType<Wave_System>();
    }
    // Update is called once per frame
    void Update()
    {
        if (r_waveSystem.m_startedWaves == true)
        {
            m_currentSecond = Mathf.FloorToInt(m_targetTime % 60);
            m_currentMinute = Mathf.FloorToInt(m_targetTime / 60);

            m_targetTime -= Time.deltaTime;

            m_currentTimeText.text = "" + m_currentMinute.ToString("00") + ":" + m_currentSecond.ToString("00");

            if (m_targetTime <= 0)
            {
                SceneManager.LoadScene("GameOver");
            }
        }
    }

    public void f_waveTimer()
    {
        m_targetTime = m_waveTimes[r_waveSystem.curRound];
    }
}
