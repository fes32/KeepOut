using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGeneratorCell : MonoBehaviour
{
    public int XPosition { get; private set; }
    public int YPosition { get; private set; }
    public bool WallLeftActive { get; private set; } = true;
    public bool WallBottomActive { get; private set; } = true;
    public bool FloorActive { get; private set; } = true;
    public bool Visited { get; private set; }
    public int DistanceFromStart { get; private set; }
    public bool BottomDoorActive { get; private set; } = false;
    public bool LeftDoorActive { get; private set; } = false;

    public MazeGeneratorCell(int x, int y)
    {
        XPosition = x;
        YPosition = y;
    }

    public void SetXYPosition(int x,int y)
    {
        XPosition = x;
        YPosition = y;
    }

    public void SetDistanceFromStart(int dictance)
    {
        DistanceFromStart = dictance;
    }

    public void DisableLeftWall()
    {
        WallLeftActive = false;
    }

    public void DisableBottomWall()
    {
        WallBottomActive = false;
    }

    public void DisableFloor()
    {
        FloorActive = false;
    }
    public void VisitCell()
    {
        Visited = true;
    }

    public void ActivateLeftDoor()
    {
        LeftDoorActive = true;
    }

    public void ActivateBottomDoor()
    {
        BottomDoorActive = true;
    }
}
