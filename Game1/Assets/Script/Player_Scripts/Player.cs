using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour {

    #region Variables

    public GameObject orangeFood;
    public GameObject blueFood;
    private GameObject weapon;
    private int currentWeapon = 1;
    private bool isShooting = false;
    private Color weaponColor;

    public Rigidbody2D rb;
    public Camera cam;
    public Animator legAnim;

    public float moveSpeed = 5f;
    public float distractionCooldown = 4f;
    Vector2 movement;
    Vector2 mousePos;

    private SpriteRenderer weaponRenderer;
    private PolygonCollider2D weaponCollider;
    private bool canDistract = true;
    [SerializeField] private int _blueFoodCount;
    public int BlueFoodCount {
        get => _blueFoodCount;
        set {
            _blueFoodCount = value;
            MainUI.Instance.SetBlueCount(value);
        }
    }

    #endregion

    #region Unity Events

    void Awake() {
        //legAnim = transform.GetChild(2).GetComponent<Animator>();
        foreach (Transform child in this.gameObject.transform) {
            if (child.tag == "Arma") {
                weapon = child.gameObject;
                break;
            }
        }
        
        weaponRenderer = weapon.GetComponent<SpriteRenderer>();
        weaponColor = weaponRenderer.color;
        weaponCollider = weapon.GetComponent<PolygonCollider2D>();
        MainUI.Instance.SetSelectedWeapon(1);
    }

    void Update() {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        //Weapon function
        if (Input.GetMouseButtonDown(0) && !isShooting) {
            isShooting = true;
            weaponColor.a = 0.75f;
            weaponRenderer.color = weaponColor;
            weaponCollider.enabled = true;
        } else if (Input.GetMouseButtonUp(0) && isShooting) {
            isShooting = false;
            weaponColor.a = 0.2f;
            weaponRenderer.color = weaponColor;
            weaponCollider.enabled = false;
        }
        

        //Food function
        if (Input.GetMouseButtonDown(1)) {
            switch (currentWeapon) {
                case 1:
                    Shoot1();
                    break;
                case 2:
                    Shoot2();
                    break;
            }
        }
        
        //Change weapon
        if (Input.GetButtonDown("Act1")) {
            currentWeapon = 1;
            MainUI.Instance.SetSelectedWeapon(1);
        }
        else if (Input.GetButtonDown("Act2")) {
            currentWeapon = 2;
            MainUI.Instance.SetSelectedWeapon(2);
        }
    }

    private void FixedUpdate() {
        rb.MovePosition(rb.position + moveSpeed * Time.fixedDeltaTime * movement);
        Vector2 lookDir = mousePos - rb.position;

        //Atan2 = Retorna o ângulo em radianos cujo Tan é y/x.
        //Valor de retorno é o ângulo entre o eixo x e um vetor 2D começando em zero e terminando em(x, y).
        //Constante de conversão de radianos para graus (somente leitura).
        //Isso é igual a 360 / (PI * 2).

        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = angle;

        if (movement == Vector2.zero)
            legAnim.SetBool("Moving", false);
        else
            legAnim.SetBool("Moving", true);
    }

    #endregion

    #region Weapon Shots

    private void Shoot1(){
        if (!canDistract)
            return;
        Instantiate(orangeFood, transform.position, Quaternion.identity); // o que é isso Quaternion.identity
        canDistract = false;
        StartCoroutine(DistractionCooldownCoroutine());
        //transform.rotation *= Quaternion.Euler(90, 0, 0); Rotate 90deg
        //weapon.GetComponent<SpriteRenderer>().enabled = false;
    }

    private void Shoot2() {
        if (BlueFoodCount <= 0)
            return;
        
        BlueFoodCount--;
        Instantiate(blueFood, transform.position, Quaternion.identity);
    }

    private IEnumerator DistractionCooldownCoroutine() {
        MainUI.Instance.StartCooldown(distractionCooldown);
        yield return new WaitForSeconds(distractionCooldown);
        canDistract = true;
    }

    #endregion

}
