using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridSquare
{
    public int xPosition;
    public int yPosition;
    public bool isFilled;

    public GridSquare(int xPos, int yPos, bool isFill)
    {
        xPosition = xPos;
        yPosition = yPos;
        isFilled = isFill;
    }
}
    


public class GameController : MonoBehaviour
{
    private float gameTime = 0.0f;
    private bool paused = false;
    private bool controllingBrick = false;

    private GameObject brick;

    public Text gameTimeText;

    public List<GameObject> brickPrefabs;

    public List<GridSquare> grid;

    private float timeLastBrickUpdated = 0.0f;
    private int lastPositionOfBrick = 113;

    GameObject controlledBrick;

    public float gameSpeed = 1.0f;

    private bool gameOver = false;

    float sKeyTime = 0.0f;
    bool sKeyLetGo = false;
    bool sKeyTapped = false;

    private void Start()
    {
        grid = new List<GridSquare>();

        for (int y = 0; y < 12; y++)            
        {
            for (int x = 0; x < 10; x++)
            {
                grid.Add(new GridSquare((-288 + (int)transform.position.x + x * 64), (-353 + (int)transform.position.y + y * 64), false));
            }
        }

        controlledBrick = new GameObject();
    }

    void Update()
    {
        UpdateGameTime();

        UpdateBricks();
    }
          
    Vector3 GetGridSquarePosition(int gridSquare)
    {
        Vector3 temp = new Vector3(grid[gridSquare].xPosition, grid[gridSquare].yPosition, 0.0f);

        return temp;
    }

    void UpdateGameTime()
    {
        if (!paused && !gameOver)
        {
            gameTime += Time.deltaTime;

            int gTI = (int)gameTime;

            gameTimeText.text = gTI.ToString();
        }
    }

    void UpdateBricks()
    {
        // spawn brick
        if (!controllingBrick)
        {
            lastPositionOfBrick = 113;

            if (!CheckGridSquareIsFilled(lastPositionOfBrick))
            {
                controlledBrick = Instantiate(brickPrefabs[7], GetGridSquarePosition(lastPositionOfBrick), Quaternion.identity, transform);
            }
            else
            {
                gameOver = true;
            }
            
            controllingBrick = true;
        }
        // move left/right and drop brick down
        else
        {
            if (Input.GetKeyDown("a"))
            {
                if((lastPositionOfBrick - 1) % 10 != 9)
                {
                    if (!CheckGridSquareIsFilled(lastPositionOfBrick - 1))
                    {
                        controlledBrick.transform.position = GetGridSquarePosition(lastPositionOfBrick - 1);
                        lastPositionOfBrick -= 1;
                    }
                }
            }
            else if (Input.GetKeyDown("d"))
            {
                if((lastPositionOfBrick + 1) % 10 != 0)
                {
                    if (!CheckGridSquareIsFilled(lastPositionOfBrick + 1))
                    {
                        controlledBrick.transform.position = GetGridSquarePosition(lastPositionOfBrick + 1);
                        lastPositionOfBrick += 1;
                    }
                }
            }

            // drop brick down
            if (Input.GetKeyDown("s"))
            {
                sKeyTime = 0.0f;

                sKeyTapped = true;
            }

            if (Input.GetKey("s") && sKeyLetGo)
            {               
                sKeyTime += Time.deltaTime;
            }

            if (Input.GetKeyUp("s"))
            {
                sKeyTime = 0.0f;
                sKeyLetGo = true;
            }
                       
            if (gameTime - timeLastBrickUpdated > gameSpeed || sKeyTime > 0.2f || sKeyTapped)
            {
                if(sKeyTapped)
                {
                    sKeyTapped = false;
                }

                if (lastPositionOfBrick - 10 < 0 || CheckGridSquareIsFilled(lastPositionOfBrick - 10))
                {
                    controllingBrick = false;

                    grid[lastPositionOfBrick].isFilled = true;

                    sKeyTime = 0.0f;
                    sKeyLetGo = false;
                }
                else
                {
                    lastPositionOfBrick -= 10;
                }

                controlledBrick.transform.position = GetGridSquarePosition(lastPositionOfBrick);

                timeLastBrickUpdated = gameTime;
            }
        }
    }

    bool CheckGridSquareIsFilled(int gs)
    {
        if (grid[gs].isFilled)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
