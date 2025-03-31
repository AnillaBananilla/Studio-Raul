using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Droplets : MonoBehaviour
{
    
    public GameObject puddlePrefab; // Prefab del charco
    public float puddleDuration = 5f; // Tiempo que dura el charco
    public float puddleOffsetY = -5f; // Valor para bajar el charco en Y
    private SpriteRenderer spriteRenderer;
    private PlayerAttack playerReference; // Referencia al script Player
    private IEnumerator CR_Countdown()
    {
        while (true)
        {
            yield return new WaitForSeconds(2);
            //Destroy(this.gameObject);
            PoolManager.Instance.ReturnObjectToPool(this.gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CR_Countdown());
        spriteRenderer = GetComponent<SpriteRenderer>();

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            playerReference = playerObject.GetComponent<PlayerAttack>();
        }
        else
        {
            Debug.LogError("No se encontró el objeto Player en la escena.");
        }

        UpdateColor();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void UpdateColor()
    {
        if (playerReference == null || spriteRenderer == null) return;

        switch (playerReference.GetCurrentColor())
        {
            case "Azul":
                spriteRenderer.color = Color.cyan;
                break;
            case "Amarillo":
                spriteRenderer.color = Color.magenta;
                break;
            case "Rojo":
                spriteRenderer.color = Color.yellow;
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
        {
            if (!collision.gameObject.CompareTag("Obstacle") && (!collision.gameObject.CompareTag("PlayerProjectile")))
            {
                GameObject Puddle = GameObject.Instantiate(puddlePrefab);
                Puddle.transform.position = this.gameObject.transform.position;
                PoolManager.Instance.ReturnObjectToPool(this.gameObject);
                SpriteRenderer puddleSpriteRenderer = Puddle.GetComponent<SpriteRenderer>();
                if (puddleSpriteRenderer != null)
                {
                    puddleSpriteRenderer.color = spriteRenderer.color;
                }
            }
            else
            {
                PoolManager.Instance.ReturnObjectToPool(this.gameObject);

            }
                

        }

    }
            
}
