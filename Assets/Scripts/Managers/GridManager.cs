using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoSingleton<GridManager> 
{
	//GRID LOGIC :
	//ABSOLUTE coordinates are the default Unity ones. They consist of the usual 3 float : x, y and z
	//GRID coordinates are specific to this script. They consist of 2 int : East and North. East correspond to x and North to z

	[Header("Balancing")]
	[SerializeField] List<Vector2Int> path;

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
	[SerializeField] GameObject TileFree;
	[SerializeField] GameObject TilePath;
	[SerializeField] bool createGrid;

	//Intermediate variables
	float absWidth;
	float absHeight;
	int gridWidth;
	int gridHeight;
	float HRatio;
	float VRatio;

	//Declarations
	public enum Tile{free, tower, path};

	//State
	Tile[,] grid;


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

		//Create grid if needed
		if (createGrid)
			CreateGrid ();

		//Fill grid array
		InitializeGrid ();
	}

	///Use this as a tool to create the grid once, not every time you start the game
	void CreateGrid ()
	{
		//Destroy current grid
		foreach (GameObject go in transform.GetComponentsInChildren<GameObject>())
			Destroy (go);

		//Instantiate every 2nd tile as my asset represents 2x2 tiles
		for (int i = minEast; i <= maxEast; i+=2)
			for (int j = minNorth; j <= maxNorth; j+=2)
				Instantiate (TileFree, ToAbs (new Vector2 (i, j)), Quaternion.identity, transform);

		//Instantiate path tiles
		foreach(Vector2Int p in path)
			Instantiate (TilePath, ToAbs(new Vector2(2*p.x, 2*p.y), 0.01f), Quaternion.identity, transform);
	}

	///Create a grid array and fill it
	void InitializeGrid ()
	{
		grid = new Tile[gridWidth+1,gridHeight+1];

		Transform[] tiles = GetComponentsInChildren<Transform> ();

		foreach (Transform t in tiles)
			if (t!= transform)
				grid [(int)ToGrid (t.transform.position).x, (int)ToGrid (t.transform.position).y] = Tile.free;

		foreach(Vector2Int p in path)
			SetAdjacentTiles (new Vector2Int(2*p.x, 2*p.y), Tile.path);
	}

	//Set "pos" and adjacent positions to "type"
	public void SetAdjacentTiles (Vector2Int pos, Tile type) {
		int x = pos.x;
		int y = pos.y;

		bool left = x == 0;
		bool right = x == grid.GetLength(0)-1;
		bool down= y == 0;
		bool up = y == grid.GetLength(1)-1;

		if (!left)
		{
			if (!down) grid [x - 1, y - 1] = type;
			if (!up)
			{
				grid [x - 1, y + 1] = type;
			}
			grid [x - 1, y] = type;
		}
		if (!right)
		{
			if (!down) grid [x + 1, y - 1] = type;
			if (!up) grid [x + 1, y + 1] = type;
			grid [x + 1, y] = type;
		}
		if (!down) grid [x, y - 1] = type;
		if (!up) grid [x, y + 1] = type;
		grid [x, y] = type;
	}

	///Snaps gameObject to the middle of the closest tile. Does not change its pos.y. Returns tile coords
	public Vector2Int SnapToTile (GameObject target)
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

	///Snaps gameObject to the middle of the closest tile + a z offset
	public Vector2Int SnapToTile (GameObject target, float zOffset)
	{
		Vector3 pos = target.transform.position + new Vector3 (0, 0, zOffset);
		Vector2 gridPos = ToGrid (pos);

		gridPos = new Vector2 (Mathf.Clamp (gridPos.x, minEast, maxEast), Mathf.Clamp (gridPos.y, minNorth, maxNorth));
		gridPos = new Vector2 (Mathf.Floor (gridPos.x + 0.5f), Mathf.Floor (gridPos.y + 0.5f));
		target.transform.position = ToAbs (gridPos, pos.y);
		return new Vector2Int((int)gridPos.x, (int)gridPos.y);
	}

	public bool TileIsFree (Vector2Int tile) {
		Tile t = grid [tile.x, tile.y];
		return (t == Tile.free);
	}

	/// displays a UI message is the tile is taken
	public void TryBuildOnTile (Vector2Int tile) {
		Tile t = grid [tile.x, tile.y];
		if (t == Tile.path)
			StartCoroutine (DisplayCantBuildOnPathText ());
		else if (t == Tile.tower)
			StartCoroutine (DisplayCantBuildOnTowerText ());
	}

	IEnumerator DisplayCantBuildOnPathText () {
		CantBuildOnPathText.SetActive (true);
		yield return new WaitForSeconds (1);
		CantBuildOnPathText.SetActive (false);
	}

	IEnumerator DisplayCantBuildOnTowerText () {
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
