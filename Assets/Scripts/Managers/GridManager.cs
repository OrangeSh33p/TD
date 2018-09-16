using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoSingleton<GridManager> {
	//GRID LOGIC :
	//ABSOLUTE coordinates are the default Unity ones. They consist of the usual 3 float : x, y and z
	//GRID coordinates are specific to this script. They consist of 2 int : East and North. East correspond to x and North to z

	[Header("Balancing")]
	[SerializeField] Vector2 tileSize;
	[SerializeField] Vector2Int gridSize;
	[SerializeField] List<Vector2Int> path;
	[SerializeField] bool createGrid;

	[Header("Boring Variables")]
	[SerializeField] GameObject cantBuildOnPathText;
	[SerializeField] GameObject cantBuildOnTowerText;
	[SerializeField] Transform greenHolder;
	[SerializeField] Transform pathHolder;
	[SerializeField] GameObject tileFree;
	[SerializeField] GameObject tilePath;

	//Tile : Describes the state of a tile. Can be free, or taken by a tower or a path tile
	public enum Tile{free, tower, path};

	//Grid : a double array of Tiles, describing the state of the entire board
	Tile[,] grid;


	void Start () {
		//Create grid if needed
		if (createGrid)
			CreateGrid ();

		//Fill grid array
		InitializeGrid ();
	}

	#region conversion functions
	///translates an absolute world coordinate into a grid coordinate (NON INTEGER)
	float ToEast (float x) {return (x/tileSize.x + (gridSize.x-1)/2);}
	float ToNorth (float z)	{return(z/tileSize.y + (gridSize.y-1)/2);}

	///translates a grid coordinate into an absolute world coordinate
	float ToX (float east) {return(tileSize.x*(east - (gridSize.x-1)/2));}
	float ToZ (float north) {return(tileSize.y*(north - (gridSize.y-1)/2));}

	///translates absolute coordinates to grid coordinates and vice versa
	Vector2 ToGrid(Vector3 position) {return new Vector2 (ToEast(position.x), ToNorth(position.z));}
	Vector3 ToAbs (Vector2 position) {return new Vector3 (ToX (position.x), 0, ToZ (position.y));}
	Vector3 ToAbs (Vector2 position, float y) {return new Vector3 (ToX (position.x), y, ToZ (position.y));}
	#endregion


	#region External tools
	//Set "pos" and adjacent positions to "type"
	public void SetAdjacentTiles (Vector2Int pos, Tile type) {
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

	///Snaps gameObject to the middle of the closest tile. Does not change its pos.y. Returns tile coords
	public Vector2Int SnapToTile (GameObject target) {
		Vector3 pos = target.transform.position;
		Vector2 gridPos = ToGrid (pos);

		//Clamp position to inside grid
		gridPos = new Vector2 (Mathf.Clamp (gridPos.x, 0, gridSize.x-1), Mathf.Clamp (gridPos.y, 0, gridSize.y-1));

		//Offset grid position by 0.5 tile then floor to get the middle of the closest tile
		gridPos = new Vector2 (Mathf.Floor (gridPos.x + 0.5f), Mathf.Floor (gridPos.y + 0.5f));

		//Move target to position translated to absolute coordinates + its original y
		target.transform.position = ToAbs (gridPos, pos.y);

		return new Vector2Int((int)gridPos.x, (int)gridPos.y);
	}

	public bool TileIsFree (Vector2Int tile) {
		return (grid [tile.x, tile.y] == Tile.free);
	}

	/// displays a UI message if the tile is taken
	public void TryBuildOnTile (Vector2Int tile) {
		Tile t = grid [tile.x, tile.y];
		if (t == Tile.path)
			StartCoroutine (DisplayCantBuildOnPathText ());
		else if (t == Tile.tower)
			StartCoroutine (DisplayCantBuildOnTowerText ());
	}
	#endregion


	///Use this as a tool to create the grid (once, not every time you start the game)
	void CreateGrid () {
		//Destroy current grid
		foreach (Transform t in greenHolder)
			Destroy (t.gameObject);
		foreach (Transform t in pathHolder)
			Destroy (t.gameObject);

		//Instantiate every 2nd tile as my tile asset represents 2x2 tiles
		for (int i=0; i<gridSize.x; i+=2)
			for (int j=0; j<gridSize.y; j+=2)
				Instantiate (tileFree, ToAbs (new Vector2 (i, j)), Quaternion.identity, greenHolder);

		//Instantiate path tiles
		foreach(Vector2Int p in path)
			Instantiate (tilePath, ToAbs(new Vector2(2*p.x, 2*p.y), 0.01f), Quaternion.identity, pathHolder);
	}

	///Create a grid array and fill it
	void InitializeGrid () {
		grid = new Tile[gridSize.x,gridSize.y];

		//Grass tiles
		foreach (Transform t in greenHolder) {
			Vector2 tGridPos = ToGrid(t.position);
			grid [(int)tGridPos.x, (int)tGridPos.y] = Tile.free;
		}

		//Path
		foreach(Vector2Int p in path)
			SetAdjacentTiles (new Vector2Int(2*p.x, 2*p.y), Tile.path);
	}


	IEnumerator DisplayCantBuildOnPathText () {
		cantBuildOnPathText.SetActive (true);
		yield return new WaitForSeconds (1);
		cantBuildOnPathText.SetActive (false);
	}

	IEnumerator DisplayCantBuildOnTowerText () {
		cantBuildOnTowerText.SetActive (true);
		yield return new WaitForSeconds (1);
		cantBuildOnTowerText.SetActive (false);
	}
}
