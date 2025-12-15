using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace VTLTools
{
    public class FPS : MonoBehaviour
    {
        public Text fpsText;

        public float updateInterval = 0.5f;
        float accum = 0.0f;
        int frames = 0;
        float timeleft;
        float fps;

        private void Start()
        {
            timeleft = updateInterval;
        }

        // Update is called once per frame
        void Update()
        {
            timeleft -= Time.deltaTime;
            accum += Time.timeScale / Time.deltaTime;
            ++frames;

            // Interval ended - update GUI text and start new interval
            if (timeleft <= 0.0)
            {
                // display two fractional digits (f2 format)
                fps = (accum / frames);
                timeleft = updateInterval;
                accum = 0.0f;
                frames = 0;
            }

            fpsText.text = "FPS: " + fps.ToString("F2");
            if (fps >= 60)
                fpsText.color = Color.green;
            else
                if (fps >= 30)
                fpsText.color = Color.yellow;
            else
                fpsText.color = Color.red;

            //charNumberText.text = "Number: " + PlayerManager.instance.characterNumberControl.transform.childCount;
        }
    }
}
