using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheep : MonoBehaviour
{

    public float runSpeed;
    public float gotHayDestroyDelay;
    public float dropDestroyDelay;
    public float heartOffset;
    public GameObject heartPrefab;

    private bool hitByHay;
    private bool droped;
    private Collider myCollider;
    private Rigidbody myRigidbody;
    private SheepSpawner sheepSpawner;

    // Start is called before the first frame update
    void Start()
    {
        myCollider = GetComponent<Collider>();
        myRigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * runSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hay") && !hitByHay) 
        {
            Destroy(other.gameObject); 
            HitByHay(); 
        }
        else if (other.CompareTag("DropSheep") && !droped)
        {
            Drop();
        }

    }
    private void HitByHay()
    {
        sheepSpawner.RemoveSheepFromList(gameObject);
        GameStateManager.Instance.SavedSheep();
        SoundManager.Instance.PlaySheepHitClip();
        Instantiate(heartPrefab, transform.position + new Vector3(0, heartOffset, 0), Quaternion.identity);
        TweenScale tweenScale = gameObject.AddComponent<TweenScale>(); ;
        tweenScale.targetScale = 0;
        tweenScale.timeToReachTarget = gotHayDestroyDelay;
        hitByHay = true;
        runSpeed = 0;
        Destroy(gameObject, gotHayDestroyDelay);
    }

    private void Drop()
    {
        GameStateManager.Instance.DroppedSheep();
        sheepSpawner.RemoveSheepFromList(gameObject);
        SoundManager.Instance.PlaySheepDroppedClip();
        droped = true;
        myRigidbody.isKinematic = false; 
        myCollider.isTrigger = false; 
        Destroy(gameObject, dropDestroyDelay); 
    }

    public void SetSpawner(SheepSpawner spawner)
    {
        sheepSpawner = spawner;
    }

}
