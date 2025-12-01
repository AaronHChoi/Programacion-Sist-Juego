using UnityEngine;

public class SkyboxRotator : MonoBehaviour
{
    [Tooltip("Degrees per second")]
    public float rotationSpeed = 2f;

    float currentRotation = 0f;

    void Update()
    {
        currentRotation += rotationSpeed * Time.deltaTime;
        currentRotation = Mathf.Repeat(currentRotation, 360f);   

        
        RenderSettings.skybox.SetFloat("_Rotation", currentRotation);

       
    }
}
