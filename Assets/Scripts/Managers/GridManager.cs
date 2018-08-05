using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoSingleton<GridManager> 
{
	//GRID LOGIC :
	//ABSOLUTE coordinates are the default Unity ones. They consist of the usual 3 float : x, y and z
	//GRID coordinates are specific to this script. They consist of 2 int : East and North. East correspond to x and North to z

	[Header("Balancing")]
	[Header("Absolute X")]
	[SerializeField] float minX;
	[SerializeField] float maxX;
	[Header("Absolute Z")]
	[SerializeField] float minZ;
	[SerializeField] float maxZ;
	[Header("Grid Horizontal Axis")]
	[SerializeField] int minEast;
	[SerializeField] int maxEast;
	[Header("Grid Vertical Axis")]
	[SerializeField] int minNorth;
	[SerializeField] int maxNorth;

	[Header("Boring Variables")]
	[SerializeField] GameObject CantBuildOnPathText;
	[SerializeField] GameObject CantBuildOnTowerText;

	//Intermediate variables
	float absWidth;
	float absHeight;
	int gridWidth;
	int gridHeight;
	float HRatio;
	float VRatio;

	public enum Tile{path, free, tower};

	public Tile[,] grid;

	void Start ()
	{
		//Defining width and height of the board in both coordinates for easier calculations
		gridWidth = maxEast - minEast;
		gridHeight = maxNorth - minNorth;
		absWidth = maxX - minX;
		absHeight = maxZ - minZ;

		//Ratios between the absolute coordinates and the grid coordinates. Useful for switching from one to the other
		HRatio = absWidth / gridWidth;
		VRatio = absHeight / gridHeight;

		grid = new Tile[gridWidth+1,gridHeight+1];

		Transform[] tiles = GetComponentsInChildren<Transform> ();

		foreach (Transform t in tiles)
		{
			if (t!= transform)
			{
				Vector2 tGridPos = ToGrid (t.transform.position);
				grid [(int)tGridPos.x, (int)tGridPos.y] = Tile.free;
			}
		}

	}

	///Snaps gameObject to the middle of the closest tile. Does not change its pos.y
	public Vector2Int Snap (GameObject target)
	{
		Vector3 pos = target.transform.position;
		Vector2 gridPos = ToGrid (pos);

		//Clamp position to inside grid
		gridPos = new Vector2 (Mathf.Clamp (gridPos.x, minEast, maxEast), Mathf.Clamp (gridPos.y, minNorth, maxNorth));

		//Offset grid position by 0.5 tile then floor to get the middle of the closest tile
		gridPos = new Vector2 (Mathf.Floor (gridPos.x + 0.5f), Mathf.Floor (gridPos.y + 0.5f));

		//Move target to position translated to absolute coordinates + its original y
		target.transform.position = ToAbs (gridPos, pos.y);

		return new Vector2Int((int)gridPos.x, (int)gridPos.y);
	}


	/// displays a UI message is the tile is taken
	public bool TileIsFree (Vector2Int tile)
	{
		Tile t = grid [tile.x, tile.y];
		if (t == Tile.free)
			return true;
		else if (t == Tile.path)
		{
			StartCoroutine (DisplayCantBuildOnPathText ());
			return false;
		}
		else if (t == Tile.tower)
		{
			StartCoroutine (DisplayCantBuildOnTowerText ());
			return false;
		}
		return false;
	}

	IEnumerator DisplayCantBuildOnPathText ()
	{
		CantBuildOnPathText.SetActive (true);
		yield return new WaitForSeconds (1);
		CantBuildOnPathText.SetActive (false);
	}

	IEnumerator DisplayCantBuildOnTowerText ()
	{
		CantBuildOnTowerText.SetActive (true);
		yield return new WaitForSeconds (1);
		CantBuildOnTowerText.SetActive (false);
	}

	#region conversion functions
	///translates an absolute X coordinate into a grid East coordinate (NON INTEGER)
	float ToEast (float x) {return ((x - minX) / HRatio + minEast);}
	///translates an absolute Z coordinate into a grid North coordinate (NON INTEGER)
	float ToNorth (float z)	{return((z - minZ) / VRatio + minNorth);}

	///translates a grid East coordinate into an absolute X coordinate
	float ToX (float east) {return((east - minEast) * HRatio + minX);}
	///translates a grid North coordinate into an absolute Z coordinate
	float ToZ (float north) {return((north - minNorth) * VRatio + minZ);}

	///translates absolute coordinates to grid coordinates
	Vector2 ToGrid(Vector3 position) {return new Vector2 (ToEast(position.x), ToNorth(position.z));}

	///translates grid coordinates to absolute coordinates
	Vector3 ToAbs (Vector2 position) {return new Vector3 (ToX (position.x), 0, ToZ (position.y));}

	///translates grid coordinates and a y coordinate to absolute coordinates
	Vector3 ToAbs (Vector2 position, float y) {return new Vector3 (ToX (position.x), y, ToZ (position.y));}
	#endregion
}
