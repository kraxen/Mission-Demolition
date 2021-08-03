using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCalm : MonoBehaviour
{
    static public GameObject POI; //Ссылка на интересующий объект

    [Header("Set in Inspector")]
    public float easing = 0.5f;
    public Vector2 minXY = Vector2.zero;

    [Header("Set Dynamically")]
    public float camZ; //Желаемая координата Z камеры
    

    private void Awake()
    {
        camZ = this.transform.position.z;
    }

    private void FixedUpdate()
    {
        if (POI == null) return; // Выйти, если нет интересующего объекта

        // Получить позицию интересующего объекта
        Vector3 destination = POI.transform.position;
        // Ограничить X и Y минимальными значениями
        destination.x = Mathf.Max(minXY.x, destination.x);
        destination.y = Mathf.Max(minXY.y, destination.y);
        // Изменить размер orthographicSize камеры, чтобы земля оставалась в поле зрения
        Camera.main.orthographicSize = destination.y + 10;
        // Определить точку между текущим местоположением камеры и distination
        destination = Vector3.Lerp(transform.position, destination, easing);
        // Принудительно установить значение destination.z равным camZ, чтобы отодвинуть камеру подальше
        destination.z = camZ;
        // Поместить камеру в позицию destonation
        transform.position = destination;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
