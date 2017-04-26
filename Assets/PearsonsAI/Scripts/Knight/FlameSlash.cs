using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameSlash : MonoBehaviour {
    public string Target;
    private Rigidbody rb;
    public ParticleSystem one, two;
    public GameObject light, Explode;
    private bool delete = false;
    private float deletetime = 8;
    public GameObject ExplosionRadius;
   
    public AudioSource firesource, explosionsource, explosioncast;
    // Use this for initialization
    private void Awake()
    {
        firesource.Play();
        explosioncast.Play();
        rb = GetComponent<Rigidbody>();
        delete = false;
    }


    private ParticleSystem Exploder;
    // Update is called once per frame
    void Update ()
    {
        Vector3 dir = Vector3.Normalize(transform.forward);

        rb.velocity = (dir * 15);
       
            deletetime -= Time.deltaTime;
            if(deletetime <= 0)
            { 
                
                Destroy(Explode);
                Destroy(gameObject);
            }
        
    }
    public ParticleSystem fire;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(Target))
        {
            if (other.GetComponent<AIMovement>().IsAlive)
            {
                explosionsource.Play();
                Explode.SetActive(true);
                rb.velocity = new Vector3(0, 0, 0);
                light.SetActive(false);
                delete = true;
                deletetime = 3;
                one.emissionRate = 0;
                two.emissionRate = 0;

                Explode.SetActive(true);
                explosionsource.Play();
            }

            other.GetComponent<AITakeDamage>().TakeDamageExplosion(100, transform.position);
        }
    }
        
}
