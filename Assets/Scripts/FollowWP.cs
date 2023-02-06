using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class FollowWP : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtBenzina;
    [SerializeField] private TextMeshProUGUI txtDistance;
    [SerializeField] private GameObject goal;
    private float distance;
    public float benzina = 50;
    public List<GameObject> waypoints;
    int currentWP = 0;
    GameObject destination;
    UnityEngine.AI.NavMeshAgent agent;
    private int x = 0;
    public float speed = 5.0f;
    public float rotSpeed = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        distance = Vector3.Magnitude(goal.transform.position - transform.position);
        agent = this.GetComponent<UnityEngine.AI.NavMeshAgent>();
        waypoints = GameObject.FindGameObjectsWithTag("diposit").ToList();
        CheckDestination();
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Magnitude(goal.transform.position - transform.position);
        txtDistance.text = "Distancia: " + Mathf.Ceil(distance);
        /*if (Vector3.Distance(this.transform.position, waypoints[currentWP].transform.position) < 1)
        {
            
        }*/
            //currentWP++;

       // if (currentWP >= waypoints.Length)
         //   currentWP = 0;

        //this.transform.LookAt(waypoints[currentWP].transform);

        if (benzina > 0)
        {
            /*
            Quaternion lookatWP = new Quaternion();
            if (waypoints.Count >= 1 && distance/speed > benzina)
            {
                float distanceDestiny = 9999;
                foreach (var waypoint in waypoints)
                {
                    float d = Mathf.Sqrt(Mathf.Pow(waypoint.transform.position.x - transform.position.x,2) + Mathf.Pow(waypoint.transform.position.y - transform.position.y,2) + Mathf.Pow(waypoint.transform.position.z - transform.position.z,2));
                    if (distanceDestiny > d)
                    {
                        distanceDestiny = d;
                        destination = waypoint;
                    }
                }
                //agafar la bezina que esta més a prop del player
                //GameObject destination = waypoints.Single(p => )
                lookatWP = Quaternion.LookRotation(destination.transform.position - this.transform.position);
            }
            else
            {
                lookatWP = Quaternion.LookRotation(goal.transform.position - this.transform.position);
            }

            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, lookatWP, rotSpeed * Time.deltaTime);

            this.transform.Translate(0, 0, speed * Time.deltaTime);
            */
            benzina -= Time.deltaTime * agent.velocity.magnitude;
            
        }
        else
        {
            agent.isStopped = true;
        }
        txtBenzina.text = "Benzina: " + Mathf.Ceil(benzina);
       
    }


    private void CheckDestination()
    {
        if (distance - 3 <= benzina)
        {
            GoToGoal();
        }
        else
        {
            GoToNextWP();
        }
    }
    public void GoToGoal(){
        agent.SetDestination(goal.transform.position);
    }
    public void GoToNextWP() {
        agent.SetDestination(waypoints[x].transform.position);
        // g.AStar(currentNode, wps[4]);
        // currentWP = 0;
    }

   
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("diposit"))
        {
            benzina += collision.gameObject.GetComponent<DipositManager>().ReadBenzine();
            if (benzina >= 50)
            {
                benzina = 50;
            }
            waypoints.Remove(collision.gameObject);
            Destroy(collision.gameObject);
            x++;
            CheckDestination();
        } else if (collision.gameObject.CompareTag("Goal"))
        {
            Time.timeScale = 0;
        }
    }
    
}
