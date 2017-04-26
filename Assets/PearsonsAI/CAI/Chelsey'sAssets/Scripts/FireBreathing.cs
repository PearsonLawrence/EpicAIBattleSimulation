using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Dragon
{
    public class FireBreathing : MonoBehaviour
    {

        public GameObject Body;
        MoveDragon Fire;

        // Use this for initialization
        void Start()
        {
            Fire = Body.GetComponent<MoveDragon>();
        }

        private void OnTriggerEnter(Collider other)
        {
            Fire.Cast = true;
        }
        private void OnTriggerExit(Collider other)
        {
            Fire.Cast = false;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}

