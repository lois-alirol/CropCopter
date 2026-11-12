using UnityEngine;


/*
 * Script qui permet al a caméra de suivre le drone avec un vecteur décalage
 *
 * drone: l'instance du drone 
 * offset: le vecteur de décalage par rapport au drone
 */
public class FollowDrone : MonoBehaviour
{
    public GameObject drone;
    public Vector3 offset;
    
    void Update() 
    {
        // vérification que le drone existe pour éviter le spam d'erreurs dans le cas
        // ou le jeu est terminé (et donc le drone détruit)
        if (drone)
        {
            transform.position = drone.transform.position + offset;
        }
    }
}