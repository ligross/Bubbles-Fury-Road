using UnityEngine;
using System;
using System.Collections;
using Random = UnityEngine.Random;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
	// probability that we spawn bad bubble
	private float badSpawnPerc = 10;
	private int flySpawnPerc = 2;
    private float bonusSpawnPerc = 0.1f;

	//following public variable is used to store the hex model prefab;
	//instantiate it by dragging the prefab on this variable using unity editor
	public GameObject goodBubble;
	public GameObject badBubble;
	public GameObject fastBubble;
	public GameObject slowBubble;
	public GameObject scoreBubble;
	public GameObject carmaBubble;
	public GameObject freeBubble;
	public GameObject ghostBubble;
	public GameObject fireBubble;
	public GameObject poisonBubble;
    public GameObject flyObject;


	//private GameObject fly;

	//width and height of starting grid
	private int gridWidthInHexes = 6;
	private int gridHeightInHexes = 20;

	//Hexagon tile width and height in game world
	private float hexWidth;
	private float hexHeight;	

	//Game object which is the parent of all the hex tiles
	private GameObject hexGrid;

	private ArrayList bubblesArray;

	//We need to add these vectors to save line coordinates we should delete and create bubbles rows
	private int rowToCreate = 0;
	
	//Method to calculate the position of the first hexagon tile
	//The center of the hex grid is (0,0,0)
	Vector3 calcInitPos()
	{
		//the initial position will be in the left upper corner
		Vector3  initPos = new Vector3(-hexWidth * gridWidthInHexes / 2f + hexWidth / 2, 0,
		                      gridHeightInHexes / 2f * hexHeight - hexHeight / 2);
		
		return initPos;
	}

	public void SetBadSpawnPerc(float badSpawnPerc){
		this.badSpawnPerc += badSpawnPerc;
		if (this.badSpawnPerc > 80)
			this.badSpawnPerc = 80;
		if (this.badSpawnPerc < 10)
			this.badSpawnPerc = 10;

	}

	public float GetBadSpawnPerc(){
		return this.badSpawnPerc;
	}

	public int GetFlySpawnPerc(){
		return this.flySpawnPerc;
	}

	public float BonusSpawnPerc{
		get { return bonusSpawnPerc;}
		set { 
			bonusSpawnPerc = value;
			StartCoroutine (DefaultSpawnPercent());
		}
	}

	IEnumerator DefaultSpawnPercent()
	{
		yield return new WaitForSeconds(5f);
		bonusSpawnPerc = 0.1f;
	}

	//method used to convert hex grid coordinates to game world coordinates
	public Vector3 calcWorldCoord(Vector2 gridPos)
	{
		//Position of the first hex tile
		Vector3 initPos = calcInitPos();
		//Every second row is offset by half of the tile width
		float offset = 0;
		if (gridPos.y % 2 != 0)
			offset = hexWidth/2;
		
		float x =  initPos.x + offset + gridPos.x * hexWidth;
		//Every new line is offset in z direction by 3/4 of the hexagon height
		float y = initPos.x - gridPos.y * hexHeight * 0.75f;
		return new Vector3(x, y, 0);
	}
	
	//Finally the method which initialises and positions all the tiles
	public void createGrid(int gridWidth, int gridHeigh)
	{		
		if (!hexGrid.activeInHierarchy) {
			return;
		}

		int y; 
		for (y = rowToCreate; y < rowToCreate + gridHeigh; y++)
		{
			for (int x = 0; x < gridWidth; x++)
			{
                Vector2 gridPos = new Vector2(x, y);
                //GameObject assigned to Hex public variable is cloned
                int whatToSpawn = Random.Range(0, 100);
				GameObject hex = null;
				if (whatToSpawn > GetBadSpawnPerc() || GameManager.Instance.IsFreeBonusActive())
				{
					if (gridHeigh == 1 && Random.value <= bonusSpawnPerc && !(y % 2 == 0 && (x == 0 || x == 5)) && !(y % 2 != 0 && x == 5)) {
						float typeChance = UnityEngine.Random.value;
                        if (typeChance <= 0.2)
                            hex = Instantiate(fastBubble);
                        else if (typeChance <= 0.4)
                            hex = Instantiate(slowBubble);
                        else if (typeChance <= 0.6)
                            hex = Instantiate(freeBubble);
                        else if (typeChance <= 0.8)
                        	hex = Instantiate (carmaBubble);
                        else
                        {
                            hex = Instantiate(scoreBubble);
                        }
                        // TODO uncomment on the next release
						
						//else if (typeChance <= 0.87) {
						//	hex = Instantiate (fireBubble);
						//} else if (typeChance <= 0.93) {
						//	hex = Instantiate (poisonBubble);
						//} else {
						//	hex = Instantiate (ghostBubble);
						
					} else {
						hex = Instantiate(goodBubble);
					}

				}
				else
				{
					hex = Instantiate(badBubble);
				}
				//Current position in grid
				if (x == 3) {
					hex.AddComponent<LineInvisible> ();
				}
				hex.transform.position = calcWorldCoord(gridPos);
				hex.transform.parent = hexGrid.transform;
				bubblesArray.Add(hex);

				// убираем возможность взорваться и заспаунить муху у обрезанных пупырок
				if ((y % 2 == 0 && (x == 0 || x == 5)) || (y % 2 != 0 && x == 5)) {
					Destroy (hex.GetComponent<EventTrigger> ());
					continue;
				}

				int flyToSpawn = Random.Range(0, 100);
				if (gridHeigh == 1 && flyToSpawn <= GetFlySpawnPerc() && !GameManager.Instance.IsFreeBonusActive()){
					hex = Instantiate(flyObject);
					hex.transform.position = calcWorldCoord(gridPos)+ new Vector3(-0.9f, -0.6f, 0);
					hex.transform.SetParent(hexGrid.transform);
			     	bubblesArray.Add(hex);
				}
			}
		}
		rowToCreate = y;
	}

	public void deleteGrid(){
		for (int i = 0; i < gridWidthInHexes; i++) {
			Destroy((GameObject)bubblesArray[i]);
		}
		bubblesArray.RemoveRange (0, gridWidthInHexes);
	}

		
	//The grid should be generated on a game start
	public void SetUpGrid()
	{
		hexWidth = this.goodBubble.GetComponent<Renderer>().bounds.size.x - 0.03f;
		hexHeight = this.goodBubble.GetComponent<Renderer>().bounds.size.y - 0.03f;
		hexGrid = new GameObject("HexGrid");
		bubblesArray = new ArrayList ();
		createGrid(gridWidthInHexes, gridHeightInHexes);
	}

	void OnDisable()
	{
		Debug.Log ("Disabling GridManager");
	}
}
