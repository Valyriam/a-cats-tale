using AssetKits.ParticleImage;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToastProjectile : MonoBehaviour
{
    Rigidbody2D rb;
    int rotationSpeed;
    bool exitedPlayerCollision;
    GameObject particleEffectContainer, visual;
    public float explosionForce;
    [SerializeField] float existenceTime;

    // Start is called before the first frame update
    void Start()
    {
        particleEffectContainer = transform.GetChild(0).gameObject;
        visual = transform.GetChild(1).gameObject;

        StartCoroutine(ExistenceTimer());

        //rotate toast
        rotationSpeed = Random.Range(1, 20);
        if(rotationSpeed %2 == 0) rotationSpeed *= -1; //if even rotate opposite direction      
    }

    private void Update()
    {
        transform.eulerAngles += new Vector3(0, 0, rotationSpeed);
    }

    IEnumerator ExistenceTimer()
    {
        WaitForSeconds wait = new WaitForSeconds(existenceTime);
        yield return wait;

        Destroy(this.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (exitedPlayerCollision == true && collision.gameObject.layer != 7)
            Explode();
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        exitedPlayerCollision = true;
    }

    public void MoveToast(bool isRightFacing)
    {
        rb = GetComponent<Rigidbody2D>();

        if (isRightFacing)
            rb.AddForce(new Vector2(explosionForce, explosionForce), ForceMode2D.Impulse);
        else
            rb.AddForce(new Vector2(-explosionForce, explosionForce), ForceMode2D.Impulse);
    }

    void Explode()
    {
        rotationSpeed = 0;
        transform.rotation.eulerAngles.Set(0,0,0);

        visual.SetActive(false);
        particleEffectContainer.SetActive(true);
    }

    public void DestroyMe() => Destroy(this.gameObject);
}
