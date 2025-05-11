using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


public class player : MonoBehaviour
{
    public GameObject hand;
    DragnDropCards[] cardsInHand;

    public GameObject[] CardList;

    public GameObject healthText;
    public GameObject brainText;
    public GameObject cardLeftText;
    public GameObject cardDrawText;
    public GameObject raundText;

    public GameObject raundResultText;
    public GameObject raundResultBack;

    public int cardPlayed = 0;
    public int cardLimit = 5;
    public int cardDrawed = 0;
    private int cardDrawLimit = 8;
    public int round = 1;
    public bool reset = false;

    public float Health = 100f;
    public float Brain = 100f;

    private Vector3 middleCard = new Vector3(0, -3.6f, -50f);

    public AudioClip drawClip;

    public AudioSource audio;

    private void Awake()
    {
        
        //int savedScore = PlayerPrefs.GetInt("round");
        audio = GetComponent<AudioSource>();
        round = PlayerPrefs.GetInt("round");
        cardDrawed = PlayerPrefs.GetInt("cardDrawed");
    }

    // Update is called once per frame
    void Update()
    {
        healthText.GetComponent<TMP_Text>().text = Health.ToString();
        brainText.GetComponent<TMP_Text>().text = Brain.ToString();

        cardLeftText.GetComponent<TMP_Text>().text = (cardLimit - cardPlayed).ToString();
        cardDrawText.GetComponent<TMP_Text>().text = (cardDrawLimit - cardDrawed).ToString();
        if(round == 1)
        {
            raundText.GetComponent<TMP_Text>().text = "First\nRound";
        }
        else if (round == 2)
        {
            raundText.GetComponent<TMP_Text>().text = "Second\nRound";
        }
        else if (round == 3)
        {
            raundText.GetComponent<TMP_Text>().text = "Last\nRound";
        }

        if(cardLimit - cardPlayed == 0)
        {
            opponent opponent = GameObject.Find("opponent").GetComponent<opponent>();
            raundResultBack.SetActive(true);
            //who is the winner
            if (Health + Brain > opponent.Brain + opponent.Health) //player is winner
            {
                StartCoroutine(showWinner("Player is Winner", 5));
                //StartCoroutine(showWinner2(5));
                //Debug.Log("Player is Winner");
                StartCoroutine(reloadScene(4.5f));
            }
            else
            {
                StartCoroutine(showWinner("Opponent is Winner", 5));
                //StartCoroutine(showWinner2(5));
                //Debug.Log("Opponent is Winner");
                StartCoroutine(reloadScene(4.5f));
            }
        }

    }

    //wait until oppenent play
    public void unPlayableStatus()
    {
        cardsInHand = hand.GetComponentsInChildren<DragnDropCards>();
        foreach (var card in cardsInHand)
        {
            card.isPlayable = false;
        }
    }

    public void playableStatus()
    {
        cardsInHand = hand.GetComponentsInChildren<DragnDropCards>();
        foreach (var card in cardsInHand)
        {
            card.isPlayable = true;
        }
    }

    public void drawACard()
    {
        if (cardDrawed < cardDrawLimit)
        {
            GameObject cardI = Instantiate(CardList[Random.Range(0, CardList.Length)], GameObject.Find("Deck").transform.position, Quaternion.identity);
            //cardI.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            cardI.transform.SetParent(hand.transform, false);
            StartCoroutine(PlayDrawACard());
            //audio.PlayOneShot(drawClip);
            //AudioSource.PlayClipAtPoint(drawClip, transform.position);
            reArrangeHand();
            cardDrawed++;
        }
        
    }
    public IEnumerator PlayDrawACard()
    {
        audio.PlayOneShot(drawClip);
        yield return new WaitForSeconds(0.1f);
    }

    private float duration = 0.3f;
    public void reArrangeHand()
    {
        cardsInHand = hand.GetComponentsInChildren<DragnDropCards>();
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
                StartCoroutine(cardsInHand[index].MoveOverTime(new Vector3(-2.1f * a, -0.1f * (2 * a - 1), 0) + middleCard, duration));
                //cardsInHand[index].transform.position = new Vector3(-2.1f * a, -0.1f * (2 * a - 1), 0) + middleCard;
                cardsInHand[index].transform.eulerAngles = new Vector3(0, 0, a * 5);
                index++;
            }

            StartCoroutine(cardsInHand[n].MoveOverTime(middleCard, duration));
            //cardsInHand[n].transform.position = middleCard;
            cardsInHand[n].transform.eulerAngles = new Vector3(0, 0, 0);

            index = n + 1;
            //for right side
            for (int i = 0; i < n; i++)
            {//-(index-3)
                int a = (index - n);
                StartCoroutine(cardsInHand[index].MoveOverTime(new Vector3(2.1f * a, -0.1f * (2 * a - 1), 0) + middleCard, duration));
                //cardsInHand[index].transform.position = new Vector3(2.1f * a, -0.1f * (2 * a - 1), 0) + middleCard;
                cardsInHand[index].transform.eulerAngles = new Vector3(0, 0, - a * 5);
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
                StartCoroutine(cardsInHand[index].MoveOverTime(new Vector3((-2.1f * a) - shiftWidth, -0.1f * (2 * a - 1) - shiftHeight, 0) + middleCard, duration));
                //cardsInHand[index].transform.position = new Vector3((-2.1f * a) - shiftWidth, -0.1f * (2 * a - 1) - shiftHeight, 0) + middleCard;
                cardsInHand[index].transform.eulerAngles = new Vector3(0, 0, a * 5 + 5);
                index++;
            }

            //middle ones
            StartCoroutine(cardsInHand[cardsInHand.Length / 2 - 1].MoveOverTime(new Vector3(-shiftWidth, -shiftHeight, 0) + middleCard, duration));
            //cardsInHand[cardsInHand.Length / 2 - 1].transform.position = new Vector3(-shiftWidth, -shiftHeight, 0) + middleCard;
            cardsInHand[cardsInHand.Length / 2 - 1 ].transform.eulerAngles = new Vector3(0, 0, 5);
            StartCoroutine(cardsInHand[cardsInHand.Length / 2 ].MoveOverTime(new Vector3(shiftWidth, -shiftHeight, 0) + middleCard, duration));
            //cardsInHand[cardsInHand.Length / 2].transform.position = new Vector3(shiftWidth, -shiftHeight, 0) + middleCard;
            cardsInHand[cardsInHand.Length / 2].transform.eulerAngles = new Vector3(0, 0, -5);

            index = (cardsInHand.Length / 2) + 1;
            //for right side
            for (int i = 0; i < n; i++)
            {//-(index-3)
                int a = (index - cardsInHand.Length / 2);
                StartCoroutine(cardsInHand[index].MoveOverTime(new Vector3((2.1f * a) + shiftWidth, -0.1f * (2 * a - 1) - shiftHeight, 0) + middleCard, duration));
                //cardsInHand[index].transform.position = new Vector3((2.1f * a) + shiftWidth, -0.1f * (2 * a - 1) - shiftHeight, 0) + middleCard;
                cardsInHand[index].transform.eulerAngles = new Vector3(0, 0, - a * 5 - 5);
                index++;
            }
        }
    }

    public IEnumerator showWinner(string text, float duration)
    {
        yield return new WaitForSeconds(1f);
        float elapsed = 0f;

        Vector3 startPos = new Vector3(0, 0, 0);
        Vector3 endPos = new Vector3(20, 0, 0);
        
        raundResultText.SetActive(true);
        raundResultText.GetComponent<TMP_Text>().text = text;
        

        while (elapsed < duration)
        {
            float t = Mathf.Clamp01(elapsed / (duration));

            // Apply ease-in (quadratic)
            float easeInT = t * t * t;

            elapsed += Time.deltaTime;


            raundResultText.GetComponent<RectTransform>().position = Vector3.Lerp(startPos, endPos, easeInT);
            yield return null;
        }

        raundResultText.GetComponent<RectTransform>().position = endPos;

        raundResultText.SetActive(false);

    }

    public IEnumerator reloadScene(float duration)
    {
        yield return new WaitForSeconds(duration);
        round++;
        PlayerPrefs.SetInt("round", round);
        PlayerPrefs.SetInt("cardDrawed", cardDrawed-5);
        if (round == 4)
        {
            SceneManager.LoadScene("end");
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }



}
