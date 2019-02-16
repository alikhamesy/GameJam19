using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class timer : MonoBehaviour
{
	public Text text;
    public float day;
    public float night;
    private float cycle;
    private float ratio;
    float time = 0;
    // Start is called before the first frame update
    void Start()
    {
        float unixSol = 1480f;
        //day *= 60;
        //night *= 60;
        cycle = (unixSol/(day + night))/60f;
    }

    int date;
    int hour;
    int min;
    string dateS;
    string hourS;
    string minS;
    // Update is called once per frame
    void Update()
    {
        time+=Time.deltaTime;

        date = (int)((time*cycle) / 1440);
        dateS = date < 10 ? "0" + date.ToString() : date.ToString();
        hour = (int)(((time*cycle) / 60) % 24);
        hourS = hour < 10 ? "0" + hour.ToString() : hour.ToString();
        min = (int)((time*cycle) % 60);
        minS = min < 10 ? "0" + min.ToString() : min.ToString();

        text.text = "Sol " + date.ToString() +"  "+  hourS +":"+ minS;
    }
}
