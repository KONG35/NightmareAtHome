using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FollowTile : MonoBehaviour
{
    PlayerCharacter target;
    public GameObject[] Tile;

    public void Awake()
    {
        target = FindObjectOfType<PlayerCharacter>();
    }



    public void Update()
    {
        Vector3 CenterPos = Tile[(int)TilePos.C].transform.position;
        if (CenterPos.x + 15f < target.transform.position.x)
        {
            Tile[(int)TilePos.CL].transform.position += Vector3.right * 90;
            Tile[(int)TilePos.TL].transform.position += Vector3.right * 90;
            Tile[(int)TilePos.BL].transform.position += Vector3.right * 90;
            var c  = Tile[(int)TilePos.C];
            var t = Tile[(int)TilePos.T];
            var b = Tile[(int)TilePos.B];
            Tile[(int)TilePos.C] = Tile[(int)TilePos.CR];
            Tile[(int)TilePos.T] = Tile[(int)TilePos.TR];
            Tile[(int)TilePos.B] = Tile[(int)TilePos.BR];
            Tile[(int)TilePos.CR] = Tile[(int)TilePos.CL];
            Tile[(int)TilePos.TR] = Tile[(int)TilePos.TL];
            Tile[(int)TilePos.BR] = Tile[(int)TilePos.BL];
            Tile[(int)TilePos.CL] = c;
            Tile[(int)TilePos.TL] = t;
            Tile[(int)TilePos.BL] = b;
        }
        else if (CenterPos.y + 15f < target.transform.position.y)
        {
            Tile[(int)TilePos.B].transform.position += Vector3.up * 90f;
            Tile[(int)TilePos.BL].transform.position += Vector3.up * 90f;
            Tile[(int)TilePos.BR].transform.position += Vector3.up * 90f;
            var c = Tile[(int)TilePos.C];
            var cr = Tile[(int)TilePos.CR];
            var cl = Tile[(int)TilePos.CL];
            Tile[(int)TilePos.C] = Tile[(int)TilePos.T];
            Tile[(int)TilePos.CR] = Tile[(int)TilePos.TR];
            Tile[(int)TilePos.CL] = Tile[(int)TilePos.TL];
            Tile[(int)TilePos.T] = Tile[(int)TilePos.B];
            Tile[(int)TilePos.TR] = Tile[(int)TilePos.BR];
            Tile[(int)TilePos.TL] = Tile[(int)TilePos.BL];
            Tile[(int)TilePos.B] = c;
            Tile[(int)TilePos.BL] = cl;
            Tile[(int)TilePos.BR] = cr;
        }
        else if (CenterPos.x - 15f > target.transform.position.x)
        {
            Tile[(int)TilePos.CR].transform.position -= Vector3.right * 90;
            Tile[(int)TilePos.TR].transform.position -= Vector3.right * 90;
            Tile[(int)TilePos.BR].transform.position -= Vector3.right * 90;
            var c = Tile[(int)TilePos.C];
            var t = Tile[(int)TilePos.T];
            var b = Tile[(int)TilePos.B];
            Tile[(int)TilePos.C] = Tile[(int)TilePos.CL];
            Tile[(int)TilePos.T] = Tile[(int)TilePos.TL];
            Tile[(int)TilePos.B] = Tile[(int)TilePos.BL];
            Tile[(int)TilePos.CL] = Tile[(int)TilePos.CR];
            Tile[(int)TilePos.TL] = Tile[(int)TilePos.TR];
            Tile[(int)TilePos.BL] = Tile[(int)TilePos.BR];
            Tile[(int)TilePos.CR] = c;
            Tile[(int)TilePos.TR] = t;
            Tile[(int)TilePos.BR] = b;
        }
        else if (CenterPos.y - 15f > target.transform.position.y)
        {
            Tile[(int)TilePos.T].transform.position -= Vector3.up * 90f;
            Tile[(int)TilePos.TL].transform.position -= Vector3.up * 90f;
            Tile[(int)TilePos.TR].transform.position -= Vector3.up * 90f;
            var c = Tile[(int)TilePos.C];
            var cr = Tile[(int)TilePos.CR];
            var cl = Tile[(int)TilePos.CL];
            Tile[(int)TilePos.C] = Tile[(int)TilePos.B];
            Tile[(int)TilePos.CR] = Tile[(int)TilePos.BR];
            Tile[(int)TilePos.CL] = Tile[(int)TilePos.BL];
            Tile[(int)TilePos.B] = Tile[(int)TilePos.T];
            Tile[(int)TilePos.BR] = Tile[(int)TilePos.TR];
            Tile[(int)TilePos.BL] = Tile[(int)TilePos.TL];
            Tile[(int)TilePos.T] = c;
            Tile[(int)TilePos.TL] = cl;
            Tile[(int)TilePos.TR] = cr;
        }
    }


    enum TilePos
    {
        TL,
        T,
        TR,
        CL,
        C,
        CR,
        BL,
        B,
        BR,
        Count
    }
}
