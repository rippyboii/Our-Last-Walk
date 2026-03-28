using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 5f;


    public Transform activeplayer;
    public Transform ghost;
    public Transform dog;

    void Start()
    {
        activeplayer = ghost; // start as ghost
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (activeplayer == ghost){
                //turn of ghost mesh
                todog();
            }else{
                //turn on ghost mesh
                toghost();
            }
        }
    }



    void todog()
    {
        activeplayer = dog;
        ghost.gameObject.SetActive(false);
    }

    void toghost()
    {
        activeplayer = ghost;
        ghost.position = dog.position;
        ghost.rotation = dog.rotation;
        ghost.gameObject.SetActive(true);
    }
}
