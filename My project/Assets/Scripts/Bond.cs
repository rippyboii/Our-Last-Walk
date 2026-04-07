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
        bond.positionCount = 2;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
