using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private Enemy currentBot;
    [SerializeField] private Transform infoCanvasTransform;
    private IState currentState;
    private float wallCheckDistance = 2f;
    public Enemy CurrentBot { get => currentBot; set => currentBot = value; }
    public void OnInit()
    {
        SetState(new IdleState(this));

    }
    public void Start()
    {
        OnInit();
    }
    private void Update()
    {
        if (currentBot.IsDead)
        {
            return;
        }
        if (GameManager.Instance.IsState(GameState.PLAYING))
        {
            currentState?.OnExecute();
        }
    }
    private void LateUpdate()
    {
        infoCanvasTransform.LookAt(infoCanvasTransform.position +
            Camera.main.transform.rotation * Vector3.forward, Vector3.up);
    }
    public void SetState(IState newState)
    {
        currentState?.OnExit();

        currentState = newState;
        currentState?.OnEnter();
    }
    public void Move(Vector3 direction)
    {
        CurrentBot.Move(direction);
    }
    public void ChangeAnim(string anim)
    {
        CurrentBot.ChangeAnim(anim);
    }
    public bool IsFacingWall(Vector3 direction)
    {

        RaycastHit hit;
        //Debug.DrawRay(transform.position + Vector3.up, direction.normalized * wallCheckDistance, Color.red, 10f);

        if (Physics.Raycast(transform.position + Vector3.up, direction.normalized, out hit, wallCheckDistance))
        {
            if (hit.collider.CompareTag(Tag.WALL) || hit.collider.CompareTag(Tag.OBSTACLE))
            {
                return true;
            }
        }
        return false;
    }
}
