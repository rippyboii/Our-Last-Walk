using System.Runtime.ExceptionServices;
using UnityEngine;

public class Bond : MonoBehaviour
{   
    public LineRenderer bond;
    public GameObject character1;
    public GameObject character2;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bond.positionCount = 2; // set the number of positions in the line renderer to 2
    }

    // Update is called once per frame
    void Update()
    {
        bond.SetPosition(0, character1.transform.position-new Vector3(0,0.3f,0));
        bond.SetPosition(1, character2.transform.position-new Vector3(0,0.3f,0));

    }
}
