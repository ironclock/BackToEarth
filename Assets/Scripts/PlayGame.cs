using System.Collections;  
using System.Collections.Generic;  
using UnityEngine;  
using UnityEngine.SceneManagement;  

public class PlayGame : MonoBehaviour
{
        
    private DogeController dogeController;
    // Start is called before the first frame update
    void Start()
    {
        dogeController = GetComponent<DogeController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Scene1() {  
        SceneManager.LoadScene("SpaceShooter");  
    }


    public void Scene2() {  
        dogeController.respawn();
    }
}
