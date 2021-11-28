using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundTriggers : MonoBehaviour
{


    public AudioManager Audio;
    //public Collider m_collider;
    public GameObject self;

    
    
    
 
    private void OnTriggerEnter(Collider other)
    {
        Audio.Play(self.name);
        self.SetActive(false);
        //m_collider.enabled = false;
        //Audio.numFaixaAudio++;
        
            
    }
}
