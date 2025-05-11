using UnityEngine;
using TMPro;

public class opponent : MonoBehaviour
{
    public GameObject hand;
    opponentCardControl[] cardsInHand;

    public GameObject[] CardList;


    public GameObject HealthText;
    public GameObject BrainText;

    public GameObject card;

    public float Health = 100f;
    public float Brain = 100f;

    private Vector3 middleCard = new Vector3(0, 4.3f, 0f);
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HealthText.GetComponent<TMP_Text>().text = Health.ToString();
        BrainText.GetComponent<TMP_Text>().text = Brain.ToString();
    }

    public void play()
    {
        
        cardsInHand = hand.GetComponentsInChildren<opponentCardControl>();
        if (cardsInHand.Length == 0) return;
        int randomCard = (int)(Random.Range(0, cardsInHand.Length));
        cardsInHand[randomCard].transform.SetParent(GameObject.Find("cardPool").transform, true);
        cardsInHand[randomCard].playCard();

        reArrange();
        Debug.Log("Opponent Played!!! Card: "+ randomCard);

        GameObject.Find("hand").GetComponent<player>().playableStatus();
    }

    public void drawCard()
    {
        GameObject cardI = Instantiate(CardList[Random.Range(0, CardList.Length)], GameObject.Find("Deck").transform.position, Quaternion.identity);
        //cardI.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        cardI.transform.SetParent(hand.transform, false);

        reArrange();
    }

    private float duration = 0.3f;
    public void reArrange()
    {
        cardsInHand = hand.GetComponentsInChildren<opponentCardControl>();
        if (cardsInHand.Length == 0) return;
        //this odd part
        if (cardsInHand.Length % 2 == 1)
        {
            int n = (int)(cardsInHand.Length / 2);
            int index = 0;

            //for left side
            for (int i = 0; i < n; i++)
            {//-(index-3)
                int a = -(index - n);
                StartCoroutine(cardsInHand[index].MoveOverTime(new Vector3(-2.1f * a, 0.1f * (2 * a - 1), cardsInHand[index].transform.position.z) + middleCard, duration));
                //cardsInHand[index].transform.position = new Vector3(-2.1f * a, 0.1f * (2 * a - 1), cardsInHand[index].transform.position.z) + middleCard;
                cardsInHand[index].transform.eulerAngles = new Vector3(0, 0, - a * 5);
                index++;
            }

            StartCoroutine(cardsInHand[n].MoveOverTime(middleCard, duration));
            //cardsInHand[n].transform.position = middleCard;
            cardsInHand[n].transform.eulerAngles = new Vector3(0, 0, cardsInHand[n].transform.position.z);

            index = n + 1;
            //for right side
            for (int i = 0; i < n; i++)
            {//-(index-3)
                int a = (index - n);
                StartCoroutine(cardsInHand[index].MoveOverTime(new Vector3(2.1f * a, 0.1f * (2 * a - 1), cardsInHand[index].transform.position.z) + middleCard, duration));
                //cardsInHand[index].transform.position = new Vector3(2.1f * a, 0.1f * (2 * a - 1), cardsInHand[index].transform.position.z) + middleCard;
                cardsInHand[index].transform.eulerAngles = new Vector3(0, 0, a * 5);
                index++;
            }
        }
        else
        {
            int n = (cardsInHand.Length - 2) / 2;
            int index = 0;
            float shiftWidth = 1.05f;
            float shiftHeight = 0.1f;


            //for left side
            for (int i = 0; i < n; i++)
            {//-(index-3)
                int a = -(index - n);
                StartCoroutine(cardsInHand[index].MoveOverTime(
                    new Vector3((-2.1f * a) - shiftWidth, 0.1f * (2 * a - 1) + shiftHeight, cardsInHand[index].transform.position.z) + middleCard, duration));
                //cardsInHand[index].transform.position = 
                //    new Vector3((-2.1f * a) - shiftWidth, 0.1f * (2 * a - 1) + shiftHeight, cardsInHand[index].transform.position.z) + middleCard;
                cardsInHand[index].transform.eulerAngles = new Vector3(0, 0, - a * 5 - 5);
                index++;
            }

            //middle ones
            
            StartCoroutine(cardsInHand[cardsInHand.Length / 2 - 1].MoveOverTime(
                new Vector3(-shiftWidth, shiftHeight, cardsInHand[cardsInHand.Length / 2 - 1].transform.position.z) + middleCard, duration));
            //cardsInHand[cardsInHand.Length / 2 - 1].transform.position 
            //    = new Vector3(-shiftWidth, shiftHeight, cardsInHand[cardsInHand.Length / 2 - 1].transform.position.z) + middleCard;
            cardsInHand[cardsInHand.Length / 2 - 1].transform.eulerAngles = new Vector3(0, 0, - 5);

            StartCoroutine(cardsInHand[cardsInHand.Length / 2].MoveOverTime(
                new Vector3(shiftWidth, shiftHeight, cardsInHand[cardsInHand.Length / 2].transform.position.z) + middleCard, duration));
            //cardsInHand[cardsInHand.Length / 2].transform.position 
            //    = new Vector3(shiftWidth, shiftHeight, cardsInHand[cardsInHand.Length / 2].transform.position.z) + middleCard;
            cardsInHand[cardsInHand.Length / 2].transform.eulerAngles = new Vector3(0, 0, 5);

            index = (cardsInHand.Length / 2) + 1;
            //for right side
            for (int i = 0; i < n; i++)
            {//-(index-3)
                int a = (index - cardsInHand.Length / 2);
                StartCoroutine(cardsInHand[index].MoveOverTime(
                    new Vector3((2.1f * a) + shiftWidth, 0.1f * (2 * a - 1) + shiftHeight, cardsInHand[index].transform.position.z) + middleCard, duration));
                //cardsInHand[index].transform.position = 
                //    new Vector3((2.1f * a) + shiftWidth, 0.1f * (2 * a - 1) + shiftHeight, cardsInHand[index].transform.position.z) + middleCard;
                cardsInHand[index].transform.eulerAngles = new Vector3(0, 0, a * 5 + 5);
                index++;
            }
        }
    }
}
