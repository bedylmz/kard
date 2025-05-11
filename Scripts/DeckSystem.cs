using UnityEngine;

public class DeckSystem : MonoBehaviour
{

    private Vector3 bigScale = new Vector3(0.9f, 0.9f, 0.9f);
    private Vector3 defaultScale = new Vector3(0.6f, 0.6f, 0.6f);


    private void OnMouseDown()
    {
        GameObject.Find("hand").GetComponent<player>().drawACard();
    }

    //Hovering
    void OnMouseOver()
    {
        transform.localScale = bigScale;
    }

    void OnMouseExit()
    {
        transform.localScale = defaultScale;
    }
}
