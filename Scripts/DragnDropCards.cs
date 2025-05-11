using System.Collections;
using UnityEngine;

public class DragnDropCards : MonoBehaviour
{
    //private Camera cam;

    public bool inPlayMat = false;
    private bool isPlayed = false;
    public bool isPlayable = true;
    private bool isDragging = false;

    private int layer;

    private Vector3 mouseOffset;
    private Vector3 mouseDefaultPos;
    private Quaternion mouseDefaulRot;

    private Vector3 mouseOverPos;

    private Quaternion straightRot = Quaternion.identity;

    private Vector3 bigScale = new Vector3(0.9f, 0.9f, 0.9f);
    private Vector3 defaultScale = new Vector3(0.6f, 0.6f, 0.6f);

    /*
    private void Start()
    {
        playParticle();
        stopParticle();
    }*/

    //Drag N Drop
    private Vector3 getWorldPosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
    private void OnMouseDown()
    {
        if (!isPlayed && isPlayable)
        {
            mouseDefaultPos = gameObject.transform.position;
            mouseDefaulRot = gameObject.transform.rotation;
            gameObject.transform.rotation = straightRot;
            mouseOffset = gameObject.transform.position - getWorldPosition();
        }
    }

    private void OnMouseDrag()
    {
        if (!isPlayed && isPlayable)
        {
            transform.position = mouseOffset + getWorldPosition() + new Vector3(0, 0, -GameObject.Find("cardPool").transform.childCount -3);
            isDragging = true;
        }
    }

    private void OnMouseUp()
    {
        if (isPlayable)
        {
            if (!inPlayMat && !isPlayed)
            {
                transform.position = mouseDefaultPos;
                transform.rotation = mouseDefaulRot;
                //stopParticle();
            }
            if (inPlayMat && gameObject.GetComponentInParent<player>().cardPlayed < gameObject.GetComponentInParent<player>().cardLimit)
            {
                player dummy = GetComponentInParent<player>(); 
                isPlayed = true;
                gameObject.GetComponentInParent<player>().unPlayableStatus();
                gameObject.GetComponentInParent<player>().cardPlayed++;
                StartCoroutine(gameObject.GetComponentInParent<player>().PlayDrawACard());
                transform.SetParent(GameObject.Find("cardPool").transform);

                layer = -GameObject.Find("cardPool").transform.childCount;


                transform.position = new Vector3(transform.position.x , transform.position.y, layer);
                dummy.reArrangeHand();
                gameObject.GetComponent<CardEffects>().Use(0);// 0 indicate this card played by player
                isDragging = false;
                //stopParticle();
                StartCoroutine(opponentPlay());
                Debug.Log("Player Played!!!");
            }
        }
    }

    public IEnumerator opponentPlay()
    {
        yield return new WaitForSeconds(0.75f);
        GameObject.Find("opponent").GetComponent<opponent>().play();
    }

    //Hovering
    void OnMouseOver()
    {
        mouseOverPos = transform.position;
        if (!isPlayed)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, -55);
            transform.localScale = bigScale;
            //playParticle();
        }
    }

    void OnMouseExit()
    {
        /*if(inPlayMat)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, layer);
        }
        else
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, -50);

        }*/
        if (!isPlayed)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, -50);
            transform.localScale = defaultScale;
            //if(!isDragging) stopParticle();
        }
        //.position = mouseOverPos;
        //transform.localScale = defaultScale;
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
            float easeInT = t * t * t;

            transform.position = Vector3.Lerp(startPos, endPos, easeInT);
            yield return null;
        }

        transform.position = endPos;
    }

    /*
    void playParticle()
    {
        gameObject.GetComponent<ParticleSystem>().Play();
    }
    void stopParticle()
    {
        gameObject.GetComponent<ParticleSystem>().Pause();
        gameObject.GetComponent<ParticleSystem>().Clear();
    }
    */
}
