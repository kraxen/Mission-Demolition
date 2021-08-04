using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameMode
{
    idle,
    playeng,
    levelEnd
}
/// <summary>
/// Класс для создания пользовательского интерфейса (кнопка, уровни, очки)
/// </summary>
public class MissionDemolition : MonoBehaviour
{
    static private MissionDemolition S; // скрытый объект одиночка

    [Header("Set in Inspector")]
    public Text uitLevel; // Ссылка на обект UIText_Level
    public Text uitShots; // Ссылка на обект UIText_Shots
    public Text uitButton; // Ссылка на дочерний объект Text в UIText_Button
    public Vector3 castlePos; // Местоположение замка
    public GameObject[] castles; // Массив замков

    [Header("Set Dynamicly")]
    public int level; // Текущий уровень
    public int levelMax; // Количество уровней
    public int shotsTaken; // Кол-во выстрелов
    public GameObject castle; // Текущий замок
    public GameMode mode = GameMode.idle;
    public string showing = "Show Slingshot"; // Режим FollowCalm

    // Start is called before the first frame update
    void Start()
    {
        S = this; // определить объект одиночку

        level = 0;
        levelMax = castles.Length;
        StartLevel();
    }
    /// <summary>
    /// Метод, который обнуляет объекты и создает уровень
    /// </summary>
    void StartLevel()
    {
        // Уничтожить прежний замок, если он существует
        if(castle != null)
        {
            Destroy(castle);
        }
        // Уничтожить все снаряды, если они существуют
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Projectile");
        foreach(GameObject Projectile in gos)
        {
            Destroy(Projectile);
        }

        // Создать новый замок
        castle = Instantiate<GameObject>(castles[level]);
        castle.transform.position = castlePos;
        shotsTaken = 0;

        // Переместить камеру в начальную позицию
        SwichView("Show Both");
        ProjectileLine.S.Clear();

        // Сбросить цель
        Goal.goalMet = false;

        UpdateGUI();

        mode = GameMode.playeng;
    }
    /// <summary>
    /// Обновить данные об уровне и счете в UI
    /// </summary>
    void UpdateGUI()
    {
        uitLevel.text = "Level: " + (level + 1) + "of " + levelMax;
        uitShots.text = "Shots Taken: " + shotsTaken;
    }
    /// <summary>
    /// Переход на следующий уровень
    /// </summary>
    void NextLevel()
    {
        level++;
        if (level == levelMax)
        {
            level = 0;
        }
        StartLevel();
    }
    /// <summary>
    /// Метод, который меняет положение камеры и текст внутри кнопки
    /// </summary>
    /// <param name="eView"></param>
    public void SwichView(string eView = "")
    {
        if(eView == "")
        {
            eView = uitButton.text;
        }
        showing = eView;
        switch (showing)
        {
            case "Show Slingshot":
                FollowCalm.POI = null;
                uitButton.text = "Show Castle";
                break;

            case "Show Castle":
                FollowCalm.POI = S.castle;
                uitButton.text = "Show Both";
                break;

            case "Show Both":
                FollowCalm.POI = GameObject.Find("ViewBoth");
                uitButton.text = "Show Slingshot";
                break;
        }
    }

    /// <summary>
    /// Статический метод, позволяющий из любого кода увеличить shortTakern
    /// </summary>
    public static void ShortFired()
    {
        S.shotsTaken++;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateGUI();

        // Прверить завершение уровня
        if( (mode == GameMode.playeng ) && Goal.goalMet)
        {
            // Изменить режим, чтобы прекратить проверку завершения уровня
            mode = GameMode.levelEnd;
            // Уменьшить масштаб
            SwichView("Show Both");
            // Начать новый уровень через 2 секунды
            Invoke("NextLevel", 2f);
        }
    }
}
