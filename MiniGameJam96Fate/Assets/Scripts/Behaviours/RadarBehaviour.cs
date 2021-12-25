using UnityEngine;

public class RadarBehaviour : MonoBehaviour
{
    #region Members

    

    [SerializeField] private Transform PlayerPivot = null;

    #endregion

    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.RotateAround(PlayerPivot.localPosition, Vector3.back, Time.deltaTime * 10f);
    }
}
