using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform hedef; // Buraya Inspector'da Player'ı sürükle
    public float yumusaklik = 0.125f;
    public Vector3 offset = new Vector3(0, 2, -10);

    void LateUpdate()
    {
        Vector3 istenenPozisyon = hedef.position + offset;
        transform.position = Vector3.Lerp(transform.position, istenenPozisyon, yumusaklik);
    }
}
