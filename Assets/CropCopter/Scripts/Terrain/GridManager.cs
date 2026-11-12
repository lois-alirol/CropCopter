using System.Collections.Generic;
using UnityEngine;


/*
 * Gère la grille ou le joueur peut planter des plantes
 */
public class GridManager : MonoBehaviour
{
    // liste à 2 dimensions avec un association entre des coordonnées et un gameobject
    private List<List<(Vector2, GameObject)>> grid = new List<List<(Vector2, GameObject)>>();
    // taille réelle dans le monde d'une case
    public int boxSize = 5;
    
    // longueur et largeur réelles du box collider de la grille dans le monde
    private int length;
    private int height;
    
    void Start()
    {
        length = (int) GetComponent<BoxCollider>().size.x;
        height = (int) GetComponent<BoxCollider>().size.z;
        
        // parcoure les lignes
        for (int i = 0; i < length; i+=boxSize)
        {
            List<(Vector2, GameObject)> row = new List<(Vector2, GameObject)>();
            
            //parcours les colonnes
            for (int j = 0; j < height; j+=boxSize)
            {
                // on rajoute dans la liste de la ligne une case avec par défaut un gameobject sans plante
                row.Add((new Vector2(i - length /2, j - height/2), null));
            }
            
            // on ajoute dans la grille, la ligne
            grid.Add(row);
        }
    }

    public (Vector2, GameObject) GetClosestBox(Vector2 pos)
    {
        // pour éviter de donner une vraie case alors que le drone n'est pas sur la grille
        if((pos.x < -length / 2f || pos.x > length / 2f || pos.y < -height / 2f || pos.y > height / 2f)) {
            return (new Vector2(-1, -1), null);
        }
        
        (Vector2, GameObject) closest = (new Vector2(-1, -1), null);
        float minDistance = float.MaxValue;

        // parcours des lignes
        foreach (var row in grid)
        {
            
            //parcours des colonnes
            foreach (var box in row)
            {
                // on calcule la distance entre le params Vector2 pos et la case parcourue
                float distance = Vector2.Distance(box.Item1, pos);
                
                // si distance plus petit que la distance minimale
                // alors on remplace la distance minimale et on met la case la plus proche a celle
                //actuellement parcourue
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closest = (box.Item1, box.Item2);
                }
            }
            
        }

        return closest;
    }

    /*
     * Parcours la grille jusqu"a trouver la position passée en params
     * et remplace la plante par une nouvelle
     */
    public void SetBox(Vector2 pos, GameObject plant) {
        for (int i = 0; i < grid.Count; i++)
        {
            for (int j = 0; j < grid[i].Count; j++)
            {
                if (grid[i][j].Item1 == pos)
                {
                    grid[i][j] = (pos, plant);
                }
            }
        }
    }

    /*
     * Retourne la grille
     */
    public List<List<(Vector2, GameObject)>> GetGrid()
    {
        return grid;
    }
}