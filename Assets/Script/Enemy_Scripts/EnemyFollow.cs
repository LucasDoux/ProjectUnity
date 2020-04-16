using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;
using System.Linq;

public class EnemyFollow : MonoBehaviour {

    #region Parameters
    public float knockbackForce;
    private bool canWalk;
    private bool canLeave;
    public float speed;

    public Player player;

    private Rigidbody2D rb;
    #endregion

    #region Control Variables

    #region Eating

    [SerializeField] private Transform closestFood;

    #endregion

    #region Distracted
    

    #endregion
    
    #region Leaving

    [SerializeField] private Transform closestLeavingPoint;

    #endregion
    
    #endregion

    #region State

    //State machine do Gato
    [Serializable]
    public enum CatState {
        Eating,
        Knockback,
        Distracted,
        Leaving
    }

    //Estado atual
    [SerializeField] private CatState _state;

    public CatState State {
        get => _state;
        //State initialization
        set {
            //Não pode sair do Leaving
            if (_state == CatState.Leaving)
                return;
            switch (value) {
                case CatState.Eating:
                    //Get closest food
                    closestFood = FindNearestWithTag("Finish").transform;
                    break;
                case CatState.Distracted:
                    break;
                case CatState.Knockback:
                    break;
                case CatState.Leaving:
                    //Get closest leaving point
                    closestLeavingPoint = FindNearestWithTag("Exit").transform;
                    //Disable collision
                    rb.isKinematic = true;
                    var direction = (closestLeavingPoint.position - transform.position).normalized;
                    rb.velocity = direction * speed;
                    //Max time
                    StartCoroutine(DestroyAfter(5f));
                    break;
            }
            _state = value;
        }
    }

    #endregion

    #region Unity Events

    private void Awake() {
        player = FindObjectOfType<Player>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Start() {
        State = CatState.Eating;
    }

    private void FixedUpdate() {
        switch (State) {
            case CatState.Eating: 
                EatingLogic();
                break;
            case CatState.Distracted:
                DistractedLogic();
                break;
            case CatState.Knockback:
                KnockbackLogic();
                break;
            case CatState.Leaving:
                LeavingLogic();
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Arma") && State != CatState.Knockback) {
            ReceiveKnockback(other.transform);
        }
    }

    private void OnDestroy() {
        GameController.Instance.Cats.Remove(this);
    }

    #endregion

    #region State Logic
    
    private void EatingLogic() {
        if (!GameController.Instance.Food.Any()) {
            State = CatState.Leaving;
            return;
        }

        if (closestFood == null) {
            closestFood = FindNearestWithTag("Finish")?.transform;
            if (closestFood == null) {
                State = CatState.Leaving;
                return;
            }
        }
        
        //Get direction
        var direction = (closestFood.position - transform.position).normalized;
        //Set movement
        rb.velocity = direction*speed;
        
        //Should be distracted?
        if (OrangeFood.Food.Count > 0) {
            if (ClosestDistraction() != null)
                _state = CatState.Distracted;
        }
    }

    private void DistractedLogic() {
        if (!OrangeFood.Food.Any()) {
            _state = CatState.Eating;
            return;
        }
        var dist = ClosestDistraction();
        if (dist == null) {
            _state = CatState.Eating;
            return;
        }
        
        var direction = (dist.transform.position - transform.position).normalized;
        rb.velocity = direction * speed;
    }

    private void KnockbackLogic() {
        
    }
    
    private void LeavingLogic() {
        var distance = Vector3.Distance(closestLeavingPoint.position, transform.position);
        if (distance <= 0.5f)
            Destroy(gameObject);
    }

    #endregion

    #region Functions

    private Transform ClosestDistraction()  {
        if (!OrangeFood.Food.Any())
            return null;
        
        var closestDistraction = OrangeFood.Food
            .Aggregate((f1,f2) => 
            Vector3.Distance(f1.transform.position,transform.position) < Vector3.Distance(f2.transform.position,transform.position) ? f1 : f2);

        if (closestDistraction.Radius >= Vector3.Distance(closestDistraction.transform.position, transform.position))
            return closestDistraction.transform;
        return null;
    }

    private GameObject FindNearestWithTag(string tag) {
        GameObject[] objects;
        objects = GameObject.FindGameObjectsWithTag(tag);
        GameObject closest = null;

        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject obj in objects) {
            Vector3 diff = obj.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance) {
                closest = obj;
                distance = curDistance;
            }
        }

        return closest;
    }

    public void ReceiveKnockback(Transform source) {
        if (State == CatState.Knockback)
            return;

        State = CatState.Knockback;
        var direction = (transform.position - player.transform.position).normalized;
        rb.velocity = direction*knockbackForce;
        StartCoroutine(EndKnockback(0.35f));
    }

    private IEnumerator EndKnockback(float delay) {
        yield return new WaitForSeconds(delay);
        //Ignore if state has been overriden
        if (State == CatState.Knockback)
        {
            State = CatState.Eating;
        }
    }

    private IEnumerator DestroyAfter(float seconds) {
        yield return new WaitForSeconds(seconds);
        Destroy(gameObject);
    }

    #endregion
    
}
