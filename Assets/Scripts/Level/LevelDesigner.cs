using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Tile
{
  public Vector2Int Offset;
  public Vector2Int Size;
  public int TextureSet;
}

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class LevelDesigner : MonoBehaviour
{
  // list of square areas with a material
  //[HideInInspector]
  [SerializeField]
  private Tile[] tiles;

  [SerializeField]
  LevelTextureSet[] TextureSets;

  [SerializeField]
  float WallHeight = 5.0f;

  [SerializeField]
  float WallDepth = 0.2f;

  [SerializeField]
  float TileWidth = 5.0f;

	// Use this for initialization
	void Start () {
	}

  public void GenerateLevel()
  {
    Debug.Log("Generating level...");

    // TODO: pre-allocate the lists using the tile info
    List<Vector3> Verts = new List<Vector3>(10);

    int submeshCount = TextureSets.Length * 2;
    List<int>[] Submeshes = new List<int>[submeshCount];

    for(int i = 0; i < submeshCount; i++)
    {
      Submeshes[i] = new List<int>(10);
    }

    List<Vector2> UVs = new List<Vector2>(10);

    for(int i = 0; i < tiles.Length; i++)
    {
      // index of the first vert in this area
      int baseVertIDX = Verts.Count;

      GenerateFloor(tiles[i].Size.x, tiles[i].Size.y, tiles[i].Offset, ref Verts, ref Submeshes[tiles[i].TextureSet * 2], ref UVs);

      GenerateWalls(tiles[i].Size.y,tiles[i].Size.y, 0              , tiles[i].Offset, baseVertIDX, ref Verts, ref Submeshes[(tiles[i].TextureSet * 2) + 1], ref UVs, false, true);
      GenerateWalls(tiles[i].Size.y,tiles[i].Size.y, tiles[i].Size.x, tiles[i].Offset, baseVertIDX, ref Verts, ref Submeshes[(tiles[i].TextureSet * 2) + 1], ref UVs, false, false);

      GenerateWalls(tiles[i].Size.x, tiles[i].Size.y, tiles[i].Size.y, tiles[i].Offset, baseVertIDX, ref Verts, ref Submeshes[(tiles[i].TextureSet * 2) + 1], ref UVs, true, true);
      GenerateWalls(tiles[i].Size.x, tiles[i].Size.y, 0              , tiles[i].Offset, baseVertIDX, ref Verts, ref Submeshes[(tiles[i].TextureSet * 2) + 1], ref UVs, true, false);
    }

    MeshFilter filter = GetComponent<MeshFilter>();
    Mesh mesh = filter.sharedMesh;
    if(!mesh)
    {
      mesh = new Mesh();
      filter.mesh = mesh;
    }
    else
    {
      mesh.Clear();
    }

    GetComponent<MeshCollider>().sharedMesh = mesh;

    mesh.vertices = Verts.ToArray();
    mesh.uv = UVs.ToArray();

    mesh.subMeshCount = submeshCount;
    for(int i = 0; i < submeshCount; i++)
    {
      mesh.SetTriangles(Submeshes[i].ToArray(), i);
    }

    mesh.RecalculateBounds();
    mesh.RecalculateNormals();
    mesh.RecalculateTangents();

    MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
    Material[] mats = new Material[submeshCount];

    for(int i = 0; i < TextureSets.Length; i++)
    {
      mats[2 * i] = TextureSets[i].floorMaterial;
      mats[(2 * i) + 1] = TextureSets[i].wallMaterial;
    }

    meshRenderer.sharedMaterials = mats;
  }

  private void GenerateFloor(int sizeX, int sizeY, Vector2Int offset, ref List<Vector3> Verts, ref List<int> Tris, ref List<Vector2> UVs)
  {
    for(int x = 0; x <= sizeX; x++)
    {
      for(int y = 0; y <= sizeY; y++)
      {
        if(y != 0 && x != 0)
        {
          int idx  = Verts.Count;

          Tris.Add(idx);
          Tris.Add(idx - 1);
          Tris.Add(idx - sizeY - 1);

          Tris.Add(idx - sizeY - 1);
          Tris.Add(idx - 1);
          Tris.Add(idx - sizeY - 2);
        }

        Verts.Add(new Vector3((x + offset.x) * TileWidth, 0.0f, (y + offset.y) * TileWidth));
        UVs.Add(new Vector2(x, y));
      }
    }
  }

  private bool IsTileOccupied(Vector2Int Tile)
  {
    for(int i = 0; i < tiles.Length; i++)
    {
      if(Tile.x >= tiles[i].Offset.x && Tile.y >= tiles[i].Offset.y &&
          Tile.x < tiles[i].Offset.x + tiles[i].Size.x && Tile.y < tiles[i].Offset.y + tiles[i].Size.y)
      {
          return true;
      }
    }

    return false;
  }

  private bool ShouldGenerateWall(int i, int z, Vector2Int offset, bool XPlane, bool FlipFlace)
  {
    int Facing = FlipFlace ? 1 : -1;
    Vector2Int direction = new Vector2Int(XPlane ? 0 : -Facing, XPlane ? Facing : 0);

    int x = z - (FlipFlace ? 0 : 1);
    int y = i - 1;

    if(XPlane)
    {
      x = i - 1;
      y = z - (FlipFlace ? 1 : 0);
    }

    return !IsTileOccupied(direction + new Vector2Int(x, y) + offset);
  }

  private void GenerateWalls(int size, int oppSize, int z, Vector2Int offset, int baseVertIDX, ref List<Vector3> Verts, ref List<int> Tris, ref List<Vector2> UVs, bool XPlane = false, bool FlipFlace = false)
  {
    int x, y;

    for(int i = 0; i <= size; i++)
    {
      if(i != 0)
      {
        int idx  = Verts.Count;

        int j, k;
        if(XPlane)
        {
            j = z;
            k = i;
        }
        else
        {
            j = i;
            k = z;
        }

        if(ShouldGenerateWall(i, z, offset, XPlane, FlipFlace))
        {
          Tris.Add(idx + (FlipFlace ? -1 : 0));
          Tris.Add(idx + (FlipFlace ? 0 : -1));
          Tris.Add(baseVertIDX + j + ((oppSize+1) * k));

          Tris.Add(idx - 1);
          Tris.Add(baseVertIDX + j + ((oppSize+1) * k) + ((FlipFlace ? 0 : -1) * (XPlane ? oppSize+1 : 1)) );
          Tris.Add(baseVertIDX + j + ((oppSize+1) * k) + ((FlipFlace ? -1 : 0) * (XPlane ? oppSize+1 : 1)) );
        }
      }

      if(XPlane)
      {
        x = i;
        y = z;
      }
      else
      {
        y = i;
        x = z;
      }

      Verts.Add(new Vector3((x + offset.x) * TileWidth, WallHeight, (y + offset.y) * TileWidth));
      UVs.Add(new Vector2(x + (XPlane ? 0.0f : 1.0f), y + (XPlane ? 1.0f : 0.0f)));
    }
  }
}
