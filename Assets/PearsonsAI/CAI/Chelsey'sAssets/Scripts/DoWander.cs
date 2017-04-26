using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;



namespace Dragon
{
    public class DoWander : MonoBehaviour
    {
        public float timer;
        public int newtarget;
        public float speed;
        public NavMeshAgent Dragon;
        public Vector3 Target;

        void Start()
        {
            Dragon = gameObject.GetComponent<NavMeshAgent>();

        }

        void Update()
        {
            timer += Time.deltaTime;

            if (timer >= newtarget)
            {
                newTarget();
                timer = 0;
                Dragon.speed = speed;

            }
        }
        public Vector3 newTarget()
        {
            float myx = gameObject.transform.position.x;
            float myz = gameObject.transform.position.z;

            float xpos = myx + Random.Range(myx - 100, myx + 100);
            float zpos = myz + Random.Range(myz - 100, myz + 100);

            Target = new Vector3(xpos, gameObject.transform.position.y, zpos);
            return Target;
        }
    }
}
