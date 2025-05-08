using UnityEngine;
using TMPro;

public class PlayerMonedas : MonoBehaviour
{
    // Referencias a los textos de la UI (TextMeshPro)
    public TextMeshProUGUI textoMonedasTotales;
    public TextMeshProUGUI textoMonedasSumadas;

    // Variables para el contador de monedas
    private int monedasTotales = 0;
    private int monedasSumadas = 0;
    private int monedasTemporales = 0; // Monedas recolectadas temporalmente

    // Variables para el temporizador
    private float tiempoTranscurrido = 0f;
    private bool mostrarMonedasSumadas = false;

    void Update()
    {
        // Si se está mostrando el texto de monedas sumadas, iniciar el temporizador
        if (mostrarMonedasSumadas)
        {
            tiempoTranscurrido += Time.deltaTime;

            // Si han pasado 3 segundos, actualizar el contador y desactivar el texto
            if (tiempoTranscurrido >= 3f)
            {
                // Sumar las monedas temporales al contador total
                monedasTotales += monedasTemporales;
                monedasSumadas = monedasTemporales; // Mostrar la cantidad sumada
                monedasTemporales = 0; // Reiniciar el contador temporal

                // Actualizar la UI
                ActualizarUI();

                // Desactivar el texto de monedas sumadas
                textoMonedasSumadas.gameObject.SetActive(false);
                mostrarMonedasSumadas = false;
            }
        }
    }

    // Función que se llama cuando el jugador colisiona con otro objeto (2D)
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Verifica si el objeto colisionado es una moneda
        if (other.CompareTag("Coin"))
        {
            // Activar el texto de monedas sumadas si no está activo
            if (!mostrarMonedasSumadas)
            {
                textoMonedasSumadas.gameObject.SetActive(true);
                mostrarMonedasSumadas = true;
            }

            // Destruye la moneda
            Destroy(other.gameObject);

            // Sumar 1 moneda al contador temporal
            monedasTemporales += 1;

            // Actualizar el texto de monedas sumadas
            textoMonedasSumadas.text = "+" + monedasTemporales.ToString();

            // Reiniciar el temporizador
            tiempoTranscurrido = 0f;
        }
    }

    // Función para actualizar la UI
    private void ActualizarUI()
    {
        textoMonedasTotales.text = monedasTotales.ToString();
        textoMonedasSumadas.text = "+" + monedasSumadas.ToString();
    }
}