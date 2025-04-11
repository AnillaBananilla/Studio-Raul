using UnityEngine;
using UnityEngine.UI;
using TMPro; // Asegurate de incluir este namespace para TextMeshPro

public class ButtonSpriteSelector : MonoBehaviour
{
    public Color newColor; // El color que quieres aplicar al SpriteRenderer
    public Sprite[] itemsImage;

    void Start()
    {
        // Obten el componente Button
        Button button = GetComponent<Button>();
        

        if (button != null)
        {
            // Obten el componente TextMeshProUGUI del boton
            TextMeshProUGUI buttonText = button.GetComponentInChildren<TextMeshProUGUI>();

            // Verifica si el texto del boton contiene "Item1", "Item2" o "Item3"
            if (buttonText != null)
            {
                // Busca el componente Image dentro del boton
                Image image = button.GetComponentInChildren<Image>();

                if (image != null)
                {
                    // Busca el componente SpriteRenderer dentro del objeto Image
                    Image image1 = image.GetComponent<Image>();

                    if (image1 != null)
                    {
                        if (buttonText.text.Contains("Item1"))
                        {
                            image1.sprite = itemsImage[0];
                        }else if (buttonText.text.Contains("Item2"))
                        {
                            image1.sprite = itemsImage[1];
                        }
                        else if (buttonText.text.Contains("Item3"))
                        {
                            image1.sprite = itemsImage[2];
                        }
                        else if (buttonText.text.Contains("Item4"))
                        {
                            image1.sprite = itemsImage[3];
                        }
                        else if (buttonText.text.Contains("Item5"))
                        {
                            image1.sprite = itemsImage[4];
                        }
                        else if (buttonText.text.Contains("Item6"))
                        {
                            image1.sprite = itemsImage[5];
                        }
                    }
                    else
                    {
                        Debug.LogWarning("No se encontro un SpriteRenderer en el objeto Image del boton.");
                    }
                }
                else
                {
                    Debug.LogWarning("No se encontro un componente Image en el boton.");
                }
            }
        }
        else
        {
            Debug.LogWarning("Este script debe estar adjunto a un objeto con un componente Button.");
        }
    }
}