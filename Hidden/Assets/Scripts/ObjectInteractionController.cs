using UnityEngine;
using System.Collections;
using App.Game.Utility;

public class ObjectInteractionController : MonoBehaviour
{


    
    public GameObject EKey;

    //Vector3 previousPos;

    //public bool inCover;

    //private Rigidbody2D rb;


    //public InteractionHandler handler;
    //public SpriteRenderer ESR;

    //Color solid = new Color(1, 1, 1, 1);
    //Color transparent = new Color(1, 1, 1, 0);

    void Start()
    {
        //ESR = EKey.GetComponent<SpriteRenderer>();
        /*
            if (handler == null)
                handler = gameObject.GetComponent<InteractionHandler>();

            handler.trigFired += new trigResponder(triggerResponder);*/

        EKey.SetActive(false);
        //inCover = false;
        //rb = GetComponent<Rigidbody2D>();

        //ESR.color = transparent;
        
    }

    /*
    void Update()
    {
        /*
        if (inCover)
        {
               if (Input.GetKeyDown(KeyCode.E))
            {
           
                print("Get out of cover");
                inCover = false;
                transform.position = previousPos;
                
            }
        }
    }*/

    void OnTriggerEnter2D(Collider2D trig)
    {
        if (trig.gameObject.tag == "Locker")
        {
            EKey.SetActive(true);
           
                //ESR.color = Color.Lerp(tran sparent, solid, 2 * Time.deltaTime);
           
        }
    }
    /*
    void OnTriggerStay2D(Collider2D trig)
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if(trig.gameObject.tag == "Locker")
            {
                if (inCover == false)
                {
                    //Run Hide in locker
                    print("HIDE IN LOCKER");
                    previousPos = transform.position; //Stores position
                    TakeCover(trig.gameObject, -2);
                    
                    
                }


            }
        }

    }
    */

        
    void OnTriggerExit2D(Collider2D trig)
    {
        if (trig.gameObject.tag == "Locker")
        {
            EKey.SetActive(false);
        }
    }

/*
    void TakeCover(GameObject obj, int layer)
    {
        print(layer);
        print("TAKE COVER");
        transform.position = obj.transform.position;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        StartCoroutine(waitTime());

    }

    IEnumerator waitTime()
    {
        yield return new WaitForSeconds(0.1f);
        inCover = true;

    }*/







    /*
    IEnumerator lerpColor(int intervals)
    {
        for (int i = 0; i < intervals; i++)
        {
            transparent.a = i / intervals;
            ESR.color = transparent;
            yield return null;
        }
    }
    */


    /*
    void triggerResponder(object sender, Collider2D trig, InteractionEventArgs args)
    {
        print("TRIGGERED");
        if (trig.gameObject.tag == "Locker")
        {
            if(args.type == eventType.Enter)
            {
                EKey.SetActive(true);
            }
        }
    }*/
}
