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
        
    }

    public void Awake()
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
            Debug.LogError("No se encontr� el objeto Player en la escena.");
        }

        UpdateColor();
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateColor()
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
            case "Magenta":
                spriteRenderer.color = Color.yellow;
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.CompareTag("Ground"))
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
            else if(collision.gameObject.CompareTag("Pilar")){
                //aquí la pintura si cae en un pilar y es del color adecuado, este la recibe 
                //para abrir la puerta que corresponda
                PaintPillar pillar = collision.gameObject.GetComponent<PaintPillar>();
                if(pillar !=null){
                    string colorName = playerReference.GetCurrentColor();
                    pillar.ReceivePaint(colorName);
                }
                PoolManager.Instance.ReturnObjectToPool(this.gameObject);
            }
            else
            {
                //En este método, al tocar la gota de pintura a una entidad con
                //tag enemigo, esta le hace daño. Es 35 porque ese daño hace Drew,
                //pero con modificaciones de stats debe cambiar ese valor
                if (collision.gameObject.CompareTag("Enemy"))
                {
                    collision.gameObject.GetComponent<Healt>().Damage(35);
                }
                PoolManager.Instance.ReturnObjectToPool(this.gameObject);

            }
        }

    }
            
}
