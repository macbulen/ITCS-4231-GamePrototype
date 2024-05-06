using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.AI;

namespace BowArrow
{

    public class ArrowController : MonoBehaviour
    {

        [SerializeField] private float length = 0.7f;
        [SerializeField] private float cd = 1.8f;

        [SerializeField] private float GravitationalConstant = 0.667408f; // test
        private List<Rigidbody> Attractees = new List<Rigidbody>();

        private Rigidbody rB;
        private float tempDragCalc;
        private float dragOffset;

        public Rigidbody RB { get => rB; }
        public float Length { get => length; }
        private LineRenderer lr;
        public LayerMask whatIsGrappleable;
        public Transform RopeConnector;
        private GameObject FishCounter;

        AudioManager audioManager;

        private void Awake()
        {
            audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        }


        void Start()
        {
            FishCounter = GameObject.Find("CollectedBoxesCounter");
            lr = GetComponent<LineRenderer>();
            const float csa = 0.00015f;
            rB = GetComponent<Rigidbody>();
            tempDragCalc = 1.292f * csa * 0.5f * cd;
            // distance behind the CoG for the application of drag, 40% of length works well
            dragOffset = length * 0.4f;
        }

        void FixedUpdate()
        {
            float vel = rB.velocity.magnitude;
            float drag = tempDragCalc * vel * vel;
            rB.AddForceAtPosition(rB.velocity.normalized * -drag, rB.transform.TransformPoint(rB.centerOfMass) - transform.forward * dragOffset);

            foreach (Rigidbody attractee in Attractees)
            {
                if (attractee != this)
                {
                    Attract(attractee);
                }
            }
            
        }

        void LateUpdate()
        {
            DrawRope();
        }

        public void Launch(float velocity)
        {
            transform.parent = null;
            rB.isKinematic = false;
            rB.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            rB.velocity = transform.forward * velocity;
        }


        void OnCollisionEnter(Collision collision)
        {
            Collider col = GetComponent<CapsuleCollider>();
            col.enabled = false;
            transform.parent = collision.gameObject.transform;
            rB.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
            rB.isKinematic = true;


            
            
            // test stuff
            if(!(collision.rigidbody == null)) 
            {
                Rigidbody rbToAttract = collision.gameObject.GetComponent<Rigidbody>();
                Attractees.Add(rbToAttract);
                NavMeshAgent fishNav = collision.gameObject.GetComponent<NavMeshAgent>();
                Destroy(fishNav);
                //rbToAttract.gameObject.GetComponent<NavMeshAgent>().isStopped = true;
            }
            else
            {
                DestroyRope();
            }
            


            // if (Attractees.Contains(collision.rigidbody))
            // {
            //     if (!(collision.gameObject.CompareTag("Player")))
            //     {
            //         Attractees.Remove(collision.rigidbody);
            //         Destroy(collision.gameObject);
            //         _counter.Counter++;
            //     }
            // }
            
        }

        private void Attract(Rigidbody rbToAttract)
        {
            GameObject player = GameObject.Find("Red");
            Vector3 direction = player.transform.position - rbToAttract.position;

            
            
            float distance = direction.magnitude;

            if (distance <= 4f) 
            { 

                Attractees.Remove(rbToAttract);
                Destroy(rbToAttract.gameObject);
                FishCounter.GetComponent<OnScreenCounter>().counter();
                audioManager.PlaySFX(audioManager.fishCaught);

                return; 
            }

            float forceMagnitude = 0.0f;

            forceMagnitude = GravitationalConstant * (750.0f * rbToAttract.mass) / distance;

            Vector3 force = direction.normalized * forceMagnitude;

            rbToAttract.AddForce(force);
            

        }

        private void OnTriggerEnter(Collider other)
        {
            if (!(other.attachedRigidbody == null) && !(other.attachedRigidbody.isKinematic))
            {
                if (!(Attractees.Contains(other.attachedRigidbody))) // NOTE: Large objects (such as "planets") were getting added to the list multiple times. This check solves this issue.
                {
                    Attractees.Add(other.attachedRigidbody);
                }
            }
        }

    // private void OnTriggerStay(Collider other)
    // {
    //     if (Attractees.Contains(other.attachedRigidbody))
    //     {
    //         if (_manipulatorIsEnabled || _manipulatorToggledOn)
    //         {
    //             other.attachedRigidbody.useGravity = false;
    //         }
    //         else
    //         {
    //             if (!(other.gameObject.CompareTag("Player")))
    //             {
    //                 other.attachedRigidbody.useGravity = true;
    //             }
    //         }
    //     }
    // }

    private void OnTriggerExit(Collider other)
    {
        if (!(other.attachedRigidbody == null))
        {
            if (Attractees.Contains(other.attachedRigidbody))
            {
                Attractees.Remove(other.attachedRigidbody);
                if (!(other.gameObject.CompareTag("Player")))
                {
                    other.attachedRigidbody.useGravity = true;
                }
            }
        }
    }

    void DrawRope()
    {
        if(!(lr == null))
        {
            lr.SetPosition(0, RopeConnector.position);
            lr.SetPosition(1, GameObject.Find("Red").transform.position);  
        }
        
    }

    void DestroyRope()
    {
        Destroy(lr);
    }
    



    }
}