using UnityEngine;
//just testing interactions.
public class numgenerator : MonoBehaviour, IInteractable
{
 public void Interact() 
    {
        int randomNum = Random.Range(0, 100);
        Debug.Log("Generated number: " + randomNum);
    }
}
