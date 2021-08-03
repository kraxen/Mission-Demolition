﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLine : MonoBehaviour
{
    static public ProjectileLine S; // Одиночка
    [Header("Set in Inspector")]
    public float minDist = 0.1f;

    private LineRenderer line;
    private GameObject _poi;
    private List<Vector3> points;

    
    public GameObject poi
    {
        get
        {
            return (_poi);
        }
        set
        {
            _poi = value;
            if (_poi != null)
            {
                //Если поле _poi содержит действительную ссылку, сбросить все остальные параметры в исходное состояние
                line.enabled = false;
                points = new List<Vector3>();
                AddPoint();
            }
        }
    }
    /// <summary>
    /// Возвращает местоположение последней добавленной точки
    /// </summary>
    public Vector3 lastPoint
    {
        get
        {
            if (points == null)
            {
                // Если точек нет, вернуть v3.zero
                return (Vector3.zero);
            }
            return (points[points.Count - 1]);
        }
    }

    private void Awake()
    {
        S = this; // Установить ссылку на объект-одиночку
        // Получить ссылку на LineRenderer
        line = GetComponent<LineRenderer>();
        // Выключить lineRernerer пока он не понадобится
        line.enabled = false;
        // Инициализировать список точек
        points = new List<Vector3>();
    }

    // Этот метод можно вызвать непосредственно, чтобы стереть линию
    public void Clear()
    {
        _poi = null;
        line.enabled = false;
        points = new List<Vector3>();
    }

    public void AddPoint()
    {
        // Вызывается для добавления точки в линии
        Vector3 pt = _poi.transform.position;
        if (points.Count > 0 && (pt - lastPoint).magnitude < minDist)
        {
            // Если точка недостаточно далека от предыдущей, просто выйти
            return;
        }
        if(points.Count == 0)
        {
            // Если это точка запуска
            Vector3 launchPosDiff = pt - Slingshot.LAUNCH_POS; // Для определения
            // ... добавить дополнительный фрагмент линии, чтобы помочь лучше прицелиться в будущем
            points.Add(pt + launchPosDiff);
            points.Add(pt);
            line.positionCount = 2;

            // Установить первые 2 точки
            line.SetPosition(0, points[0]);
            line.SetPosition(1, points[1]);
            // Вулючить lineRenderer
            line.enabled = true;
        }
        else
        {
            // Обычная последовательность добавления точки
            points.Add(pt);
            line.positionCount = points.Count;
            line.SetPosition(points.Count - 1, lastPoint);
            line.enabled = true;
        }
    }
    private void FixedUpdate()
    {
        if(poi == null)
        {
            //Если свойство poi содержит пустое значение, найти интересующий объект
            if(FollowCalm.POI != null)
            {
                if(FollowCalm.POI.tag == "Projectile")
                {
                    poi = FollowCalm.POI;
                }
                else
                {
                    return; // Выйти, если интересующий объект не найден
                }
            }
            else
            {
                return; // Выйти, если интересующий объект не найден
            }
        }
        // Если интересующий объект найден, попытаться добавить точку с его координатами в каждом fixedUpdate
        AddPoint();
        if( FollowCalm.POI == null)
        {
            // Если FollowCalm.POI содержит null, записать null в poi
            poi = null;
        }
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
