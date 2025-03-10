using UnityEngine;
using UnityEngine.UI;
using TMPro; // Aseg�rate de incluir este namespace para TextMeshPro

public class ButtonSpriteSelector : MonoBehaviour
{
    public Color newColor; // El color que quieres aplicar al SpriteRenderer

    void Start()
    {
        // Obt�n el componente Button
        Button button = GetComponent<Button>();
        

        if (button != null)
        {
            // Obt�n el componente TextMeshProUGUI del bot�n
            TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();

            // Verifica si el texto del bot�n contiene "Item1", "Item2" o "Item3"
            if (buttonText != null)
            {
                // Busca el componente Image dentro del bot�n
                Image image = button.GetComponentInChildren<Image>();

                if (image != null)
                {
                    // Busca el componente SpriteRenderer dentro del objeto Image
                    Image image1 = image.GetComponent<Image>();

                    if (image1 != null)
                    {
                        if (buttonText.text.Contains("Item1"))
                        {
                            image1.color = Color.green;
                        }else if (buttonText.text.Contains("Item2"))
                        {
                            image1.color = Color.red;
                        }
                        else if (buttonText.text.Contains("Item3"))
                        {
                            image1.color = Color.cyan;
                        }
                        else if (buttonText.text.Contains("Item4"))
                        {
                            image1.color = Color.magenta;
                        }
                        else if (buttonText.text.Contains("Item5"))
                        {
                            image1.color = Color.yellow;
                        }
                        else if (buttonText.text.Contains("Item6"))
                        {
                            image1.color = Color.white;
                        }
                    }
                    else
                    {
                        Debug.LogWarning("No se encontr� un SpriteRenderer en el objeto Image del bot�n.");
                    }
                }
                else
                {
                    Debug.LogWarning("No se encontr� un componente Image en el bot�n.");
                }
            }
        }
        else
        {
            Debug.LogWarning("Este script debe estar adjunto a un objeto con un componente Button.");
        }
    }
}