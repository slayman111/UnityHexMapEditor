using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SpinSelector : MonoBehaviour
{
    private HexCell mouseOverCell, posOverCell;
    private Vector3 cellPos;
    private bool active = false;
    private GameObject[] selection;
    private Renderer[] rend;
    private HexCell[] selectionPrevFrame, selectionPrevCell;
    private float[] selectionTimeSwapped;
    private float angle = 0f, degreesPerSwipe, xOffset, zOffset, slerpRot = 0f, prev, secondsRiseFall;
    private int numoptions, inFront = 0, swipeDir = 0;

    public HexGrid hexGrid;
    public GameObject[] prefabs;
    public GameObject lookAt;
    public CombinedInputs combinedInputs;
    public float yOffset = 0f, xzRadius = 30f, swipesPerSecond = 2f, percSwipeRiseFall = 0.25f;
    public bool smoothed = true, fadeRenderer = true;

    public int InFront { get => inFront; }

    private void Update()
    {
        if (combinedInputs.Tap && !EventSystem.current.IsPointerOverGameObject() && slerpRot == 0f)
        {
            if (active)
            {
                for (int i = 0; i < selection.Length; i++)
                    Destroy(selection[i]);
                active = false;
            }
            mouseOverCell = hexGrid.GetCell(Camera.main.ScreenPointToRay(Input.mousePosition));
            if (mouseOverCell is not null)
            {
                active = true;
                numoptions = prefabs.Length;
                if (fadeRenderer) rend = new Renderer[numoptions];
                selection = new GameObject[numoptions];
                secondsRiseFall = percSwipeRiseFall / swipesPerSecond;
                selectionPrevFrame = new HexCell[numoptions];
                selectionPrevCell = new HexCell[numoptions];
                selectionTimeSwapped = new float[numoptions];
                slerpRot = prev = 0f;
                degreesPerSwipe = 360f / numoptions;
                cellPos = mouseOverCell.transform.position;
                cellPos.y = yOffset;
                transform.position = cellPos;
                cellPos = lookAt.transform.position;
                cellPos.y = transform.position.y;
                transform.LookAt(cellPos);
                for (int i = 0; i < numoptions; i++)
                {
                    selection[i] = Instantiate(prefabs[i], transform);
                    if (fadeRenderer) rend[i] = selection[i].GetComponent<Renderer>();
                    UpdatePosition(i, angle, true);
                }
            }
        }
        else
        {
            if (active && combinedInputs.SwipeRight)
            {
                if (swipeDir == -1) slerpRot -= 1f;
                else if (slerpRot == 0f) slerpRot = -1f;
                swipeDir = 1;
            }
            else if (active && combinedInputs.SwipeLeft)
            {
                if (swipeDir == 1) slerpRot += 1f;
                else if (slerpRot == 0f) slerpRot = 1f;
                swipeDir = -1;
            }
            if (slerpRot != 0f)
            {
                cellPos = lookAt.transform.position;
                cellPos.y = transform.position.y;
                transform.LookAt(cellPos);
                for (int i = 0; i < selection.Length; i++)
                {
                    UpdatePosition(i, angle, false);
                }
                prev = slerpRot;
                slerpRot += swipeDir * swipesPerSecond * Time.deltaTime;
                if (swipeDir * slerpRot > 0f)
                {
                    inFront = Mathf.RoundToInt(angle / degreesPerSwipe);
                    while (inFront < 0) inFront += numoptions;
                    while (inFront >= numoptions) inFront -= numoptions;
                    slerpRot = 0f;
                }
                if (smoothed) angle += swipeDir * (Mathf.SmoothStep(0f, degreesPerSwipe, -swipeDir * slerpRot) - Mathf.SmoothStep(0f, degreesPerSwipe, -swipeDir * prev));
                else angle -= (slerpRot - prev) * degreesPerSwipe;
            }
        }
    }

    private void UpdatePosition(int i, float angle, bool initiate)
    {
        xOffset = xzRadius * Mathf.Sin(Mathf.Deg2Rad * (angle - i * degreesPerSwipe));
        zOffset = xzRadius * Mathf.Cos(Mathf.Deg2Rad * (angle - i * degreesPerSwipe));
        cellPos.y = 0f;
        cellPos.x = xOffset;
        cellPos.z = zOffset - xzRadius;
        selection[i].transform.localPosition = cellPos;

        Ray ray = new(selection[i].transform.position + HexMetrics.elevationStep * 10f * Vector3.up, Vector3.down);
        posOverCell = hexGrid.GetCell(ray);

        if (posOverCell != selectionPrevFrame[i])
        {
            if (initiate) selectionPrevFrame[i] = posOverCell;
            selectionPrevCell[i] = selectionPrevFrame[i];
            selectionTimeSwapped[i] = Time.time;
        }
        if (selection[i] is not null && posOverCell is not null && selectionPrevCell is not null)
        {
            cellPos.y = HexMetrics.elevationStep * (1f + Mathf.Lerp(selectionPrevCell[i].Elevation, posOverCell.Elevation,
                Mathf.Min(1f, (Time.time - selectionTimeSwapped[i]) / secondsRiseFall)));
        }

        selection[i].transform.localPosition = cellPos;
        if (fadeRenderer && rend[i])
        {
            Color32 col = rend[i].material.GetColor("_Color");
            col.a = (byte)(255f * (1 + cellPos.z / (2f * xzRadius)));
            rend[i].material.SetColor("_Color", col);
        }
        selectionPrevFrame[i] = posOverCell;
    }
}