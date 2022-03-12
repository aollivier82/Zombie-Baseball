using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuarantineLine : MonoBehaviour
{

    LineRenderer line;
    public int trailResolution = 50;
    List<Vector3> playerPos;

    public FoodGenerator foodGenerator;

    float elapsed = 0f;
    public float timeBetween = 0.1f;

    static int INF = 10000;



    // Start is called before the first frame update
    void Start()
    {
        playerPos = new List<Vector3>();
        line = gameObject.GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        elapsed += Time.deltaTime;
        if (elapsed >= timeBetween)
        {
            elapsed = elapsed % timeBetween;
            RecordPos();
            DrawLines();
        }
    }

    //Records the position of each segment of the line
    void RecordPos() {
        if (playerPos.Count < trailResolution)
        {
            playerPos.Add(gameObject.transform.position);
            line.positionCount++;
            
        }
        
        if (playerPos.Count >= trailResolution) {
            playerPos.Add(gameObject.transform.position);
            playerPos.RemoveAt(0);
        }

        //line.SetPosition(playerPos.Count - 1, player.transform.position);
        //print("playerPos.Count: " + playerPos.Count + ", trailResolution: " + trailResolution);
    }   

    //Passes line positions to renderer, checks if they are intersecting
    void DrawLines()
    {
        line.SetPositions(playerPos.ToArray());
        if (playerPos.Count >= 3)
        {
            for (int x = 0; x < playerPos.Count-3; x++)
            {
                Vector3 startPointOne = playerPos[playerPos.Count-2]; //Starting point of the last line drawn
                Vector3 endPointOne = playerPos[playerPos.Count-1];   //Ending point of the last line drawn 
                Vector3 startPointTwo = playerPos[x];                 //Starting point of the current iterated line
                Vector3 endPointTwo = playerPos[x+1];                 //Ending point of the current iterated line
                if (IsIntersecting(startPointOne, endPointOne, startPointTwo, endPointTwo))
                {
                    List<Vector3> loop = new List<Vector3>();
                    for (int y = x; y < playerPos.Count; y++)
                    {
                        loop.Add(playerPos[y]);
                    }
                    CloseCircle(loop);
                    //Debug.DrawLine(startPointOne, endPointOne, Color.black, 0.1f); ;
                    //Debug.DrawLine(startPointTwo, endPointTwo, Color.red, 0.1f);
                    //print("startPointOne: " + startPointOne + ", " + "endPointOne: " + endPointOne);
                    //print("startPointTwo: " + startPointTwo + ", "  + "endPointTwo: " + endPointTwo);
                }  
            }
        }
    }


    void CloseCircle(List<Vector3> loop)
    {
        foreach (Food food in foodGenerator.foodList)
        {
            
            //Draw a horizontal line from a point (zombie) infinitely to the right.
            Vector3 foodPos = food.gameObject.transform.position;
            Vector3 endPoint = foodPos + new Vector3(INF, 0, 0);

            int numLineCross = 0;

            //Count the number of times it intersects with edges in the polygon.
            for (int x = 0; x < loop.Count-1; x++)
            {
                if (IsIntersecting(foodPos, endPoint, loop[x], loop[x+1]))
                {
                    numLineCross++;
                }
            }

            //If odd
            if (numLineCross % 2 != 0)
            {
                foodGenerator.Eat(food);
            }
        }


        
        //A point is inside the polygon if either count of intersections is odd or point lies on an edge of polygon.  If none of the conditions is true, then point lies outside.


        //Resets the line
        playerPos.Clear();
        line.positionCount = 0;
    }

    //Checks if the line is intersecting with itself
    bool IsIntersecting(Vector3 startPointOne, Vector3 endPointOne, Vector3 startPointTwo, Vector3 endPointTwo)
    {
        bool isIntersecting = false;

        if (startPointOne == startPointTwo || startPointOne == endPointTwo || startPointTwo == endPointOne || startPointTwo == endPointTwo)
        {
            return false;
        }

        //3d -> 2d
        Vector2 p1 = new Vector2(startPointOne.x, startPointOne.z);
        Vector2 p2 = new Vector2(endPointOne.x, endPointOne.z);

        Vector2 p3 = new Vector2(startPointTwo.x, startPointTwo.z);
        Vector2 p4 = new Vector2(endPointTwo.x, endPointTwo.z);

        float denominator = (p4.y - p3.y) * (p2.x - p1.x) - (p4.x - p3.x) * (p2.y - p1.y);

        //Make sure the denominator is > 0, if so the lines are parallel
        if (denominator != 0)
        {
            float u_a = ((p4.x - p3.x) * (p1.y - p3.y) - (p4.y - p3.y) * (p1.x - p3.x)) / denominator;
            float u_b = ((p2.x - p1.x) * (p1.y - p3.y) - (p2.y - p1.y) * (p1.x - p3.x)) / denominator;

            //Is intersecting if u_a and u_b are between 0 and 1
            if (u_a >= 0 && u_a <= 1 && u_b >= 0 && u_b <= 1)
            {
                isIntersecting = true;
            }
        }

        return isIntersecting;
    }

}
