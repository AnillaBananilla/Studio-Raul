using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovProfundidadFondo : MonoBehaviour
{
    Transform camara; //posicion de la camara
    Vector3 camPosInicial; //posicion inicial de la camara 
    float distanciaMoverX, distanciaMoverY; //distancia a mover en ambos ejes

    GameObject[] fondos;
    Material[] mat;

    //aqu� se guardar�n las velocidades relativas de los fondos
    float[] velFondo;

    //valor en z del fondo m�s alejado para tomarlo como referencia del resto
    float fondoFinal;

    //para permitir ajustar la velocidad relativa de las capas en X
    [Range(0.01f, 1f)]
    public float velParallaxX = 0.7f;

    //para permitir ajustar la velocidad relativa de las capas en Y
    [Range(0.0f, 1f)]
    public float velParallaxY = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        //asignar valores iniciales en general
        camara = Camera.main.transform;
        camPosInicial = camara.position;
        //Inicia la variable con la cantidad de hijos que tenga el vacio padre
        int cuentaFondos = transform.childCount;
        //Asignar los tama�os de los arrays a la cantidad de hijos
        mat = new Material[cuentaFondos];
        velFondo = new float[cuentaFondos];
        fondos = new GameObject[cuentaFondos];
        /*
        IMPORTANTE
        Por cada hijo se asigna su respectiva imagen y shader usado.
        As�, despues se puede hacer la repetici�n del fondo cuando se
        "acaba" la imagen
        */
        for (int i = 0; i < cuentaFondos; i++)
        {
            fondos[i] = transform.GetChild(i).gameObject;
            mat[i] = fondos[i].GetComponent<Renderer>().material;
        }

        CalcularVelocidadFondo(cuentaFondos);
    }

    void CalcularVelocidadFondo(int cuentaFondos)
    {
        /*IMPORTANTE
         * Encontrar el fondo m�s alejado, pues a partir de ah� se calcula
         la vel relativa del resto*/
        for (int i = 0; i < cuentaFondos; i++)
        {
            if ((fondos[i].transform.position.z - camara.position.z) > fondoFinal)
            {
                fondoFinal = fondos[i].transform.position.z - camara.position.z;
            }
        }

        //Sabiendo el m�s lejano, se asigna a cada uno una velocidad relativa que aumenta en proporci�n a ese
        for (int i = 0; i < cuentaFondos; i++)
        {
            velFondo[i] = 1 - (fondos[i].transform.position.z - camara.position.z) / fondoFinal;
        }
    }

    //se usa lateupdate para que si o si la c�mara ya se haya movido antes de mover el fondo
    private void LateUpdate()
    {
        distanciaMoverX = camara.position.x - camPosInicial.x;
        distanciaMoverY = camara.position.y - camPosInicial.y;
        /* los valores de X, Y y Z  se pueden ir ajustando para 
        que encuadre en s� misma la imagen 
        (o mejor dicho el plano que se usa para colocar las im�genes), 
        aunque esta ya est� encuadrada, se sumar�a o restar�a en cada valor seg�n coresponda*/
        transform.position = new Vector3(camara.position.x, camara.position.y + 0.01f, 0.68f);

        for (int i = 0; i < fondos.Length; i++)
        {
            /*MUY IMPORTANTE
             * Aqui puede ser confuso, pero en general la l�gica es la siguiente
             primero con lo calculado antes se asigna a un nuevo float, tomando en cuenta
            el valor que nosotros asignamos desde el inspector
            Luego, ambas velocidades se multiplican en cada capa por su velocidad relativa
            As�, se usa el set texture off set para ir movi�ndolas a diferentes velocidades
            Finalmente, es importante recordar que hay que tener los modos de wrap en repeat*/
            float velocidadX = velFondo[i] * velParallaxX;
            float velocidadY = velFondo[i] * velParallaxY;
            mat[i].SetTextureOffset("_MainTex", new Vector2(distanciaMoverX * velocidadX, distanciaMoverY * velocidadY));
        }
    }
}
