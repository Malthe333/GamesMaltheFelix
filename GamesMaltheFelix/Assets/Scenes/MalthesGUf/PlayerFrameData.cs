using UnityEngine;

[System.Serializable]
public class PlayerFrameData
{
    public Vector3 position; // Positionen som bliver målt omkring spilleren
    public Quaternion rotation; //Rotationen som bliver målt omkring spilleren
    public bool didAttack; // Angiver om spilleren har angrebet i denne frame
}

