using UnityEngine;
using System.Collections;

public class RelaxGridManager : MonoBehaviour
{
    //following public variable is used to store the hex model prefab;
    //instantiate it by dragging the prefab on this variable using unity editor
    public GameObject relaxBubble;
    public Vector2 cameraCenter;
    public Vector2 minCoords;
    public Vector2 maxCoords;

    //width and height of starting grid
    public int gridWidthInHexes = 50;
    public int gridHeightInHexes = 50;

    //Hexagon tile width and height in game world
    private float hexWidth;
    private float hexHeight;    

    //Game object which is the parent of all the hex tiles
    private GameObject hexGrid;

    public ArrayList bubblesArray;

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
                GameObject hex = Instantiate(relaxBubble);;
                hex.transform.position = calcWorldCoord(gridPos);
                hex.transform.parent = hexGrid.transform;
                bubblesArray.Add(hex);

                if (gridHeightInHexes/2 == y && gridWidthInHexes/2 == x){
                    cameraCenter = calcWorldCoord(gridPos);
                    minCoords = cameraCenter;
                    maxCoords = cameraCenter;
                }

            }
        }
        rowToCreate = y;
        minCoords = new Vector2();
        foreach(GameObject hex in bubblesArray){
            float xCoord = hex.transform.position.x;
            float yCoord = hex.transform.position.y;
            if (xCoord < minCoords.x){
                minCoords.x = xCoord; 
            }
            if (xCoord > maxCoords.x)
            {
                maxCoords.x = xCoord;
            }
            if (yCoord < minCoords.y)
            {
                minCoords.y = yCoord;
            }
            if (yCoord > maxCoords.y)
            {
                maxCoords.y = yCoord;
            }
        }
    }

    public void ChangeGridShape(int height)
    {
        gridWidthInHexes = height;
        gridHeightInHexes = height;

        hexWidth = this.relaxBubble.GetComponent<Renderer>().bounds.size.x - 0.03f;
        hexHeight = this.relaxBubble.GetComponent<Renderer>().bounds.size.y - 0.03f;

        int y = 0;
        int x = 0;
        for (y = 0; y < gridHeightInHexes; y++)
        {
            for (x = 0; x < gridWidthInHexes; x++)
            {
                Vector2 gridPos = new Vector2(x, y);
                GameObject hex = null;
                //GameObject assigned to Hex public variable is cloned
                try
                {
                    hex = (GameObject)bubblesArray[x + y * gridWidthInHexes];
                }
                catch {
                    hex = Instantiate(relaxBubble);
                    bubblesArray.Add(hex);
                }
                hex.transform.position = calcWorldCoord(gridPos);
                hex.transform.parent = hexGrid.transform;

                if (gridHeightInHexes / 2 == y && gridWidthInHexes / 2 == x)
                {
                    cameraCenter = calcWorldCoord(gridPos);
                    minCoords = cameraCenter;
                    maxCoords = cameraCenter;
                }

            }
        }
        //for (int i = (x - 1) + (y - 1) * gridWidthInHexes; i < bubblesArray.Count; i++)
        //{
        //    Destroy((GameObject)bubblesArray[i]);
        //}
        for (int i = bubblesArray.Count - 1; i > (x - 1) + (y - 1) * gridWidthInHexes; i--)
        {
            Destroy((GameObject)bubblesArray[i]);
            bubblesArray.RemoveAt(i);
        }
        //bubblesArray.RemoveRange((x - 1) + (y - 1) * gridWidthInHexes, bubblesArray.Count - 1);

        minCoords = new Vector2();
        foreach (GameObject hex in bubblesArray)
        {
            float xCoord = hex.transform.position.x;
            float yCoord = hex.transform.position.y;
            if (xCoord < minCoords.x)
            {
                minCoords.x = xCoord;
            }
            if (xCoord > maxCoords.x)
            {
                maxCoords.x = xCoord;
            }
            if (yCoord < minCoords.y)
            {
                minCoords.y = yCoord;
            }
            if (yCoord > maxCoords.y)
            {
                maxCoords.y = yCoord;
            }
        } 

    }
        
    //The grid should be generated on a game start
    public void SetUpGrid(int height)
    {
        gridWidthInHexes = height;
        gridHeightInHexes = height;

        hexWidth = this.relaxBubble.GetComponent<Renderer>().bounds.size.x - 0.03f;
        hexHeight = this.relaxBubble.GetComponent<Renderer>().bounds.size.y - 0.03f;
        if (hexGrid != null)
        {
            Destroy(hexGrid);
        }
        rowToCreate = 0;
        hexGrid = new GameObject("HexGrid");
        bubblesArray = new ArrayList ();
        createGrid(gridWidthInHexes, gridHeightInHexes);
    }
}
