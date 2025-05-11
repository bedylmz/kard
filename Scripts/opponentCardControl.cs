using System.Collections;
using UnityEngine;

public class opponentCardControl : MonoBehaviour
{
    private GameObject playingMat;
    private float duration = 0.3f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void playCard()
    {
        playingMat =  GameObject.FindGameObjectWithTag("PlayMat");
        transform.position = new Vector3(transform.position.x, transform.position.y, -GameObject.Find("cardPool").transform.childCount);
        //0.25 , 0.35 
        
        Vector3 target = 
            new Vector3(playingMat.transform.position.x + Random.Range(-0.5f, 0.5f), 
            playingMat.transform.position.y + Random.Range(-0.5f, 0.5f), 
            - GameObject.Find("cardPool").transform.childCount); 

        StartCoroutine(MoveOverTime(target, duration));
        //transform.position = playingMat.transform.position;
        gameObject.GetComponent<CardEffects>().Use(1);// 1 indicate this card played by NPC
        transform.rotation = Quaternion.identity;
        gameObject.GetComponent<SpriteRenderer>().sprite = gameObject.GetComponent<CardEffects>().sprite;
        gameObject.GetComponent<SpriteRenderer>().color = Color.white;
    }

    public IEnumerator MoveOverTime(Vector3 endPos, float duration)
    {
        Vector3 startPos = transform.position;
        float elapsed = 0f;
 
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / (duration));

            // Apply ease-in (quadratic)
            float easeInT = t*t*t;

            transform.position = Vector3.Lerp(startPos, endPos, easeInT);
            yield return null;
        }

        transform.position = endPos;
    }
}
