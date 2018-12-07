using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    tiles = new Tile[1];
    tiles[0].Offset = new Vector2Int(0,0);
    tiles[0].Size = new Vector2Int(2,1);

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
        // Floor mesh
        GeneratePlane(tiles[i].Size.x, tiles[i].Size.y, ref Verts, ref Submeshes[tiles[i].TextureSet * 2], ref UVs);
    }

    MeshFilter filter = GetComponent<MeshFilter>();
    Mesh mesh = filter.sharedMesh;
    if(!mesh)
    {
      mesh = new Mesh();
      filter.mesh = mesh;
    }

    mesh.vertices = Verts.ToArray();
    mesh.uv = UVs.ToArray();

    mesh.subMeshCount = submeshCount;
    for(int i = 0; i < submeshCount; i++)
    {
      mesh.SetTriangles(Submeshes[i].ToArray(), i);
    }

    MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
    Material[] mats = new Material[submeshCount];

    for(int i = 0; i < TextureSets.Length; i++)
    {
      mats[2 * i] = TextureSets[i].floorMaterial;
      mats[(2 * i) + 1] = TextureSets[i].wallMaterial;
    }

    meshRenderer.sharedMaterials = mats;
  }

  private void GeneratePlane(int sizeX, int sizeY, ref List<Vector3> Verts, ref List<int> Tris, ref List<Vector2> UVs)
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

        Verts.Add(new Vector3(x * TileWidth, 0.0f, y * TileWidth));
        UVs.Add(new Vector2(x, y));
      }
    }
  }
}
