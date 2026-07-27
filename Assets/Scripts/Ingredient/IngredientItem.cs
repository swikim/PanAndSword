using System.Collections;
using UnityEngine;

public class IngredientItem : MonoBehaviour
{
    public IngredientData ingredientData;

    [Header("퍼짐 모션")]
    [SerializeField] private float scatterMinRadius = 0.5f;
    [SerializeField] private float scatterMaxRadius = 1.5f;
    [SerializeField] private float popHeight = 2f;
    [SerializeField] private float popDuration = 0.7f;

    [Header("레이어")]
    [SerializeField] private LayerMask groundLayerMask;
    [SerializeField] private LayerMask wallLayerMask;

    private Vector3 spawnPosition;

    private void OnEnable()
    {
        StartCoroutine(ScatterRoutine());
    }
    private IEnumerator ScatterRoutine()
    {
        Vector3 startPos = transform.position;

        float angle = Random.Range(0.0f, Mathf.PI * 2f);
        float radius = Random.Range(scatterMinRadius,scatterMaxRadius);
        Vector3 horizontalOffset = new Vector3(Mathf.Cos(angle),0f, Mathf.Sin(angle)) * radius;
        Vector3 endPos = startPos + horizontalOffset;


        if(Physics.Raycast(startPos, horizontalOffset.normalized, out RaycastHit wallHit, radius, wallLayerMask))
        {
            endPos = wallHit.point - horizontalOffset.normalized * 0.2f;
        }
        float floorY = startPos.y; // 기본값
        Vector3 rayOrigin = endPos + Vector3.up * 0.5f;

        if(Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, 5f, groundLayerMask))
        {
            floorY = hit.point.y;
        }
        

         float elapsed = 0f;
        while (elapsed < popDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / popDuration;

            Vector3 horizontalPos = Vector3.Lerp(startPos, endPos, t);

            float heightOffset = popHeight * 4f * t * (1f - t);

            transform.position = horizontalPos + Vector3.up * heightOffset;

            yield return null;
        }

        transform.position = new Vector3(endPos.x,floorY,endPos.z); 
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!other.CompareTag("Player")) return;

        IngredientManager.Instance.AddIngredient(ingredientData);
        Debug.Log($"[Item] {ingredientData.ingredientName} 수집");

        IngredientPool.Instance.Return(gameObject);
    }
}