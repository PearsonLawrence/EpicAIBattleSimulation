using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleTime : MonoBehaviour {
    public float UpTime;

    // Use this for initialization
    private void Awake()
    {
        UpTime = 3;
    }

    // Update is called once per frame
    void Update () {
        UpTime -= Time.deltaTime;
        if(UpTime < 0)
        {
            Destroy(gameObject);
        }
	}
}
