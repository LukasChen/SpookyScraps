public abstract class PlayerBaseState {
    protected readonly PlayerControl player;
    protected PlayerBaseState(PlayerControl player) {
        this.player = player;
    }
    public abstract void UpdateState();
    public abstract void EnterState();
    public abstract void ExitState();
}