using System;
using UnityEngine;
using UnityEngine.SceneManagement;


public class FingerDriverPlayer : MonoBehaviour
    {
        [SerializeField] private FingerDriverTrack m_Track;
        [SerializeField] private FingerDriverInput m_Input;
        //точко по которой будет проверяться нахождение на трассе;
        [SerializeField] private Transform m_trackpoint;
        [SerializeField] private float m_MaxSteer = 90f;
        [SerializeField] private float m_carspeed = 2f;
        

        private void Update()
        {
            if (m_Track.IsPointInTrack(m_trackpoint.transform.position))
            {
                transform.Translate(transform.up*Time.deltaTime *m_carspeed, Space.World);
                transform.Rotate(0f, 0f, m_MaxSteer* m_Input.SteerAxis*Time.deltaTime);
            }
            else
            {
                //Вывод счета
                print($"Game over, your score: {m_Track.SegmentsPassed}");
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }
