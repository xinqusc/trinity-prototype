using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public GameObject gameplayManager;
    private float offset = 2.0f;
    private float speed = 5.0f;
    private float horizontalInput;
    private float verticalInput;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        Vector3 displacement = speed * Time.deltaTime * new Vector3(horizontalInput, verticalInput).normalized;
        Vector3 newPos = transform.position + displacement;
        if (gameplayManager.GetComponent<MapManager>().IsInMap(newPos, offset))
            transform.Translate(displacement);
        if (Input.GetKeyDown(KeyCode.J))
            GetComponent<PlayerCharacterPositioning>().GetClosestCharacter(1).GetComponent<CharacterInventory>().ActivateItem();
        if (Input.GetKeyDown(KeyCode.K))
            GetComponent<PlayerCharacterPositioning>().GetClosestCharacter(0).GetComponent<CharacterInventory>().ActivateItem();
        if (Input.GetKeyDown(KeyCode.L))
            GetComponent<PlayerCharacterPositioning>().GetClosestCharacter(-1).GetComponent<CharacterInventory>().ActivateItem();
    }
}
