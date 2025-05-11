using UnityEngine;

public class PlayableArea : MonoBehaviour
{

    private GameObject[] cards;

    void OnMouseOver()
    {
        cards = GameObject.FindGameObjectsWithTag("Card");
        if(cards.Length > 0)
        {
            foreach (var card in cards)
            {
                card.gameObject.GetComponent<DragnDropCards>().inPlayMat = true;
            }
        }
        
    }

    void OnMouseExit()
    {
        cards = GameObject.FindGameObjectsWithTag("Card");
        if (cards.Length > 0)
        {
            foreach (var card in cards)
            {
                card.gameObject.GetComponent<DragnDropCards>().inPlayMat = false;
            }
        }
    }

    
}
