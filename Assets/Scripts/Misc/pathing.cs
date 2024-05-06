using UnityEngine;
using UnityEngine.AI;

public class pathing : MonoBehaviour
{
    public GameObject[] position;
    NavMeshAgent navage;
    int pos_value;
    
    // Start is called before the first frame update
    void Start()
    {
        navage=GetComponent<NavMeshAgent>();
        pos_value=0;
        
    }

    // Update is called once per frame
    void Update()
    {
        if(navage.remainingDistance<0.2f)
        {
            pos_value+=1;
            if(pos_value>position.Length-1)
            {
                pos_value=0;
            }
            navage.SetDestination(position[pos_value].transform.position);
            
        }
        
    }
}
