using UnityEngine;

public class onStart : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < 5; i++)
        {
            GameObject.Find("hand").GetComponent<player>().drawACard();
            GameObject.Find("opponent").GetComponent<opponent>().drawCard();
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
