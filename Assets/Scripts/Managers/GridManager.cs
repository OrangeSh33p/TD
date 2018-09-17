using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GridManager {
	//GRID LOGIC :
	//ABSOLUTE coordinates are the default Unity ones. They consist of the usual 3 float : x, y and z
	//GRID coordinates are specific to this script. They consist of 2 int : East and North. East correspond to x and North to z

	//Reference to GameManager
	static GameManager gm = GameManager.Instance;

	//Tile : Describes the state of a tile. Can be free, or taken by a tower or a path tile
	public enum Tile{free, tower, path};

	//Grid : a double array of Tiles, describing the state of the entire board
	static Tile[,] grid;


	public static void _Start () {
		//Create grid if needed
		if (gm.createGrid)
			CreateGrid ();

		//Fill grid array
		InitializeGrid ();
	}

	#region conversion functions
	///translates an absolute world coordinate into a grid coordinate (NON INTEGER)
	static float ToEast (float x) {return (x/gm.tileSize.x + (gm.gridSize.x-1)/2);}
	static float ToNorth (float z)	{return(z/gm.tileSize.y + (gm.gridSize.y-1)/2);}

	///translates a grid coordinate into an absolute world coordinate
	static float ToX (float east) {return(gm.tileSize.x*(east - (gm.gridSize.x-1)/2));}
	static float ToZ (float north) {return(gm.tileSize.y*(north - (gm.gridSize.y-1)/2));}

	///translates absolute coordinates to grid coordinates and vice versa
	static Vector2 ToGrid(Vector3 position) {return new Vector2 (ToEast(position.x), ToNorth(position.z));}
	static Vector3 ToAbs (Vector2 position) {return new Vector3 (ToX (position.x), 0, ToZ (position.y));}
	static Vector3 ToAbs (Vector2 position, float y) {return new Vector3 (ToX (position.x), y, ToZ (position.y));}
	#endregion


	#region External tools
	//Set "pos" and adjacent positions to "type"
	public static void SetAdjacentTiles (Vector2Int pos, Tile type) {
		int x = pos.x;
		int y = pos.y;

		bool left = x==0;
		bool right = x==grid.GetLength(0)-1;
		bool down = y==0;
		bool up = y==grid.GetLength(1)-1;

		if (!left) {
			if (!down) grid [x - 1, y - 1] = type;
			if (!up) grid [x - 1, y + 1] = type;
			grid [x - 1, y] = type;
		}

		if (!right) {
			if (!down) grid [x + 1, y - 1] = type;
			if (!up) grid [x + 1, y + 1] = type;
			grid [x + 1, y] = type;
		}

		if (!down) grid [x, y - 1] = type;
		if (!up) grid [x, y + 1] = type;
		grid [x, y] = type;
	}

	public static bool IsOnGrid(Vector3 target) {
		Vector2 gridPos = ToGrid (target);
		return (gridPos.x == Mathf.Clamp(gridPos.x, 0 , gm.gridSize.x-1) &&
			gridPos.y == Mathf.Clamp(gridPos.y, 0, gm.gridSize.y -1));
	}

	///Snaps gameObject to the middle of the closest tile. Does not change its pos.y. Returns tile coords
	public static Vector2Int SnapToTile (GameObject target) {
		Vector3 pos = target.transform.position;
		Vector2 gridPos = ToGrid (pos);

		//Clamp position to inside grid
		gridPos = new Vector2 (Mathf.Clamp (gridPos.x, 0, gm.gridSize.x-1), Mathf.Clamp (gridPos.y, 0, gm.gridSize.y-1));

		//Offset grid position by 0.5 tile then floor to get the middle of the closest tile
		gridPos = new Vector2 (Mathf.Floor (gridPos.x + 0.5f), Mathf.Floor (gridPos.y + 0.5f));

		//Move target to position translated to absolute coordinates + its original y
		target.transform.position = ToAbs (gridPos, pos.y);

		return new Vector2Int((int)gridPos.x, (int)gridPos.y);
	}

	public static bool TileIsFree (Vector2Int tile) {
		return (grid [tile.x, tile.y] == Tile.free);
	}

	/// displays a UI message if the tile is taken
	public static void TryBuildOnTile (Vector2Int tile) {
		Tile t = grid [tile.x, tile.y];
		if (t == Tile.path)
			UIManager.DisplayText(gm.cantBuildOnPathText);
		else if (t == Tile.tower)
			UIManager.DisplayText(gm.cantBuildOnTowerText);
	}
	#endregion


	///Use this as a tool to create the grid (once, not every time you start the game)
	static void CreateGrid () {
		//Destroy current grid
		foreach (Transform t in gm.greenHolder)
			gm._Destroy (t.gameObject);
		foreach (Transform t in gm.pathHolder)
			gm._Destroy (t.gameObject);

		//Instantiate every 2nd tile as my tile asset represents 2x2 tiles
		for (int i=0; i<gm.gridSize.x; i+=2)
			for (int j=0; j<gm.gridSize.y; j+=2)
				gm._Instantiate (gm.tileFree, ToAbs (new Vector2 (i, j)), Quaternion.identity, gm.greenHolder);

		//Instantiate path tiles
		foreach(Vector2Int p in gm.path)
			gm._Instantiate (gm.tilePath, ToAbs(new Vector2(2*p.x, 2*p.y), 0.01f), Quaternion.identity, gm.pathHolder);
	}

	///Create a grid array and fill it
	static void InitializeGrid () {
		grid = new Tile[gm.gridSize.x,gm.gridSize.y];

		//Grass tiles
		foreach (Transform t in gm.greenHolder) {
			Vector2 tGridPos = ToGrid(t.position);
			grid [(int)tGridPos.x, (int)tGridPos.y] = Tile.free;
		}

		//Path
		foreach(Vector2Int p in gm.path)
			SetAdjacentTiles (new Vector2Int(2*p.x, 2*p.y), Tile.path);
	}
}
