using UnityEngine;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform pointTransform;
    [SerializeField] private float scaleSpeed;

    private Vector2 startPosition;
    
    private void FixedUpdate()
    {
    #if UNITY_EDITOR
        PCController();
    #endif
        
    #if UNITY_ANDROID
        AndroidController();
    #endif
    }
    
    private void PCController()
    {
        if (Input.GetKey(KeyCode.W))
        {
            if (pointTransform.localScale.y >= 1.7) return;
            pointTransform.localScale += new Vector3(-1 * scaleSpeed * Time.deltaTime, scaleSpeed * Time.deltaTime, 0);
        }

        if (Input.GetKey(KeyCode.S))
        {
            if(pointTransform.localScale.y <= 0.25) return;
            pointTransform.localScale += new Vector3(scaleSpeed * Time.deltaTime, -1 * scaleSpeed * Time.deltaTime,0);
        }
    }


    private void AndroidController()
    {
        if (Input.touchCount <= 0) return;
        
        var touch = Input.GetTouch(0);

        if (touch.phase == TouchPhase.Began)
        {
            startPosition = touch.position;
        }
            
        if (touch.phase == TouchPhase.Moved)
        {
            if (touch.position.y > startPosition.y)
            {
                if (pointTransform.localScale.y >= 1.7f) return;
                pointTransform.localScale += new Vector3(-1 * scaleSpeed * Time.deltaTime, scaleSpeed * Time.deltaTime, 0);
                startPosition = touch.position;
            } 
                
            if (touch.position.y < startPosition.y)
            {
                if(pointTransform.localScale.y <= 0.25f) return;
                pointTransform.localScale += new Vector3(scaleSpeed * Time.deltaTime, -1 * scaleSpeed * Time.deltaTime,0);
                startPosition = touch.position;
            }
        }
    }
    
#if UNITY_ANDROID
    
    
    
#endif
}
