using UnityEngine;
using UnityEngine.Networking;

public class PlayerInput : NetworkBehaviour
{
    private PlayerController playerController;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
    }

    void FixedUpdate()
    {
        PlayerMovement();
    }

    void PlayerMovement()
    {
        //perché altrimenti chiama i metodi di entrambi i giocatori (????)
        if(isLocalPlayer)
        {
            //se il giocatore si vuole muovere --> -1 sinistra, 1 destra
            float move = 0;
            //se il giocatore vuole saltare
            bool jump = false;
            //se il giocatore vuole abbasarsi
            bool crouch = false;
            if (Input.GetKey(KeyCode.A))
                move = -1;
            else if (Input.GetKey(KeyCode.D))
                move = 1;
            if (Input.GetKey(KeyCode.W) && isLocalPlayer)
                jump = true;
            //if(move != 0 || jump || crouch)
                playerController.Move(move, crouch, jump);
        }

    }
}
