using UnityEngine;
using System.Collections.Generic; // ← added
using System.Collections; 

public class Bowl : MonoBehaviour
{
    private List<GameObject> ingredientsInBowl = new List<GameObject>();
    public List<GameObject> ingredientsNeeded = new List<GameObject>();
    public ParticleSystem completionEffect;
    private bool isComplete = false;


    void Start()
    {
        completionEffect = GetComponent <ParticleSystem>();
    }

    void Update()
    {
       if (!isComplete && CompareLists(ingredientsInBowl, ingredientsNeeded))
        {
            isComplete = true;
            Debug.Log("Dish is complete!");
            completionEffect.Stop();
            completionEffect.Clear();
            completionEffect.Play();
            StartCoroutine(TriggerTransition());
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // if (other.CompareTag("Ingredient"))
        // {   
        Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
        ingredientsInBowl.Add(other.gameObject);
        Debug.Log("Ingredient added: " + other.gameObject.name);
        // }
    }
    void OnTriggerExit(Collider other)
    {
        // if (other.CompareTag("Ingredient"))
        // {
        ingredientsInBowl.Remove(other.gameObject);
        // }
    }

    bool CompareLists<GameObject>(List<GameObject> aListA, List<GameObject> aListB)
    {
    if (aListA == null || aListB == null || aListA.Count != aListB.Count)
        return false;
    if (aListA.Count == 0)
        return true;

    Dictionary<GameObject, int> lookUp = new Dictionary<GameObject, int>();
    // create index for the first list
    for (int i = 0; i < aListA.Count; i++)
    {
        int count = 0;
        if (!lookUp.TryGetValue(aListA[i], out count))
        {
            lookUp.Add(aListA[i], 1);
            continue;
        }
        lookUp[aListA[i]] = count + 1;
    }
    for (int i = 0; i < aListB.Count; i++)
    {
        int count = 0;
        if (!lookUp.TryGetValue(aListB[i], out count))
        {
            // early exit as the current value in B doesn't exist in the lookUp (and not in ListA)
            return false;
        }
        count--;
        if (count <= 0)
            lookUp.Remove(aListB[i]);
        else
            lookUp[aListB[i]] = count;
    }
    // if there are remaining elements in the lookUp, that means ListA contains elements that do not exist in ListB
    return lookUp.Count == 0;


}

IEnumerator TriggerTransition()
    {
        yield return new WaitForSeconds(3f);
        FindObjectOfType<SceneTransitionManager>().TriggerTransition();
    }
}