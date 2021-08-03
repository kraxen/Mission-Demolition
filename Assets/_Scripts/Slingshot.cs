using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour
{
    // Поля, уставливаемые в инспекторе Unity
    [Header("Set in Inspector")]
    public GameObject perfabProjectile;
    public float velocityMult = 8f;

    // Поля, устанавливаемые динамически
    [Header("Set Dynamically")]
    public GameObject launchPoint;
    public Vector3 launchPos;
    public GameObject projectile;
    public bool aimingMode;
    private Rigidbody projectileRigitbody;

    private void Awake()
    {
        Transform launchPointTrans = transform.Find("LaunchPoint");
        launchPoint = launchPointTrans.gameObject;
        launchPoint.SetActive(false);
        launchPos = launchPointTrans.position;
    }
    private void OnMouseEnter()
    {
        //print("Slingshot:OnMouseEnter");
        launchPoint.SetActive(true);
    }
    private void OnMouseExit()
    {
        //print("Slingshot:OnMouseExit");
        launchPoint.SetActive(false);
    }

    private void OnMouseDown()
    {
        //Игрок нажал кнопку мыши, когда указатель находится под рогаткой
        aimingMode = true;
        // Создать снаряд
        projectile = Instantiate(perfabProjectile) as GameObject;
        // Поместить в точку launchPos
        projectile.transform.position = launchPos;
        // Сделать его кинематическим
        projectileRigitbody = projectile.GetComponent<Rigidbody>();
        projectileRigitbody.isKinematic = true;
    }
    private void Update()
    {
        // Если рогатка не в режиме прицеливания, не выполнять этот код
        if (!aimingMode) return;

        // Получить текущие экранные координаты указателя мыши
        Vector3 mousePos2D = Input.mousePosition;
        mousePos2D.z = -Camera.main.transform.position.z;
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);

        // Найти разность координат между launchPos и mousePos3D
        Vector3 mouseDelta = mousePos3D - launchPos;

        // Ограничить mouseDelta радиусом коллайдера объекта Slingshot
        float maxMagnitude = this.GetComponent<SphereCollider>().radius;
        if(mouseDelta.magnitude > maxMagnitude)
        {
            mouseDelta.Normalize();
            mouseDelta *= maxMagnitude;
        }

        // Передвинуть снаряд в новую позицию
        Vector3 projPos = launchPos + mouseDelta;
        projectile.transform.position = projPos;
        if(Input.GetMouseButtonUp(0))
        {
            // Кнопка мыши нажата
            aimingMode = false;
            projectileRigitbody.isKinematic = false;
            projectileRigitbody.velocity = -mouseDelta * velocityMult;
            FollowCalm.POI = projectile;
            projectile = null;
        }
    }
    //// Start is called before the first frame update
    //void Start()
    //{

    //}

    //// Update is called once per frame
    //void Update()
    //{

    //}
}
